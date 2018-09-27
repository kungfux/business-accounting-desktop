using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using BusinessAccounting.Properties;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;

namespace BusinessAccounting.Integrations
{
    internal class GoogleDriveBackupStorage
    {
        public event Action<string> OnStatusUpdated;
        public event Action<string> OnFailed;
        public event Action OnCompleted;

        private const string DB_FILE_NAME = "ba.sqlite";
        private const string MIME_TYPE = "application/x-sqlite3";
        private const string FILE_FIELDS = "modifiedTime";
        private const string FILE_API_KEY = "google_drive_api.json";
        private const string CREDENTIALS_LOCATION = ".credentials/business-accounting.json";

        private enum BackupTo
        {
            File,
            Folder,
            Undefined
        };

        private UserCredential _credential;
        private readonly BackupTo _backupTo;
        private readonly string _databaseFileFullPath;
        private readonly string _remoteFolderId;
        private readonly string _remoteFileId;
        private readonly int _autoBackupInterval;

        public GoogleDriveBackupStorage(string DatabaseFileFullPath, string RemoteFolderId, string RemoteFileId, int BackupInterval)
        {
            if (string.IsNullOrEmpty(DatabaseFileFullPath))
            {
                throw new ArgumentException("DatabaseFileFullPath cannot be empty.");
            }

            _databaseFileFullPath = DatabaseFileFullPath;
            _remoteFolderId = RemoteFolderId;
            _remoteFileId = RemoteFileId;
            _autoBackupInterval = BackupInterval;

            _backupTo = !string.IsNullOrEmpty(_remoteFolderId) ? BackupTo.Folder : BackupTo.Undefined;
            _backupTo = !string.IsNullOrEmpty(_remoteFileId) ? BackupTo.File : _backupTo;
        }

        public void MakeBackup()
        {
            switch(_backupTo)
            {
                case BackupTo.Folder:
                    CreateNewBackup();
                    break;
                case BackupTo.File:
                    UpdateBackup();
                    break;
                default:
                    SetFailed(ResourcesRU.NoBackupLocationDefined);
                    break;
            }
        }

        public async void MakeBackupIfOutOfDate()
        {
            if (_autoBackupInterval <= 0 || _backupTo == BackupTo.Undefined)
                return;

            var lastBackupTime = _backupTo == BackupTo.File ? await GetLastBackupTimeFromFile(_remoteFileId) : await GetLastBackupTimeFromFolder(_remoteFolderId);
            if (lastBackupTime.HasValue && lastBackupTime.Value.AddHours(_autoBackupInterval) <= DateTime.Now)
            {
                MakeBackup();
            }
        }

        private async Task<DriveService> GetService()
        {
            if (_credential == null)
                await Authorize();

            try
            {
                return new DriveService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = _credential,
                    ApplicationName = Assembly.GetExecutingAssembly().GetName().Name
                });
            }
            catch(Exception e)
            {
                SetFailed(e.Message);
            }
            return null;
        }

        private async Task Authorize()
        {
            try
            {
                using (var stream = new FileStream(FILE_API_KEY, FileMode.Open, FileAccess.Read))
                {
                    var credentialPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    credentialPath = Path.Combine(credentialPath, CREDENTIALS_LOCATION);

                    _credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets,
                        new[] { DriveService.Scope.DriveFile }, "user", CancellationToken.None, new FileDataStore(credentialPath, true));
                }
            }
            catch(Exception e)
            {
                SetFailed(e.Message);
            }
        }

        private async Task<DateTime?> GetLastBackupTimeFromFile(string FileResourceId)
        {
            var service = await GetService();

            try
            {
                FilesResource.GetRequest getMetaRequest;
                getMetaRequest = service.Files.Get(FileResourceId);
                getMetaRequest.Fields = FILE_FIELDS;
                Google.Apis.Drive.v3.Data.File getMetaResponse;
                getMetaResponse = await getMetaRequest.ExecuteAsync();
                return getMetaResponse.ModifiedTime;
            }
            catch (Exception e)
            {
                SetFailed(e.Message);
            }
            return null;
        }

        private async Task<DateTime?> GetLastBackupTimeFromFolder(string DirectoryResourceId)
        {
            var service = await GetService();

            try
            {
                FilesResource.GetRequest getMetaRequest;
                getMetaRequest = service.Files.Get(DirectoryResourceId);
                getMetaRequest.Fields = FILE_FIELDS;
                Google.Apis.Drive.v3.Data.File getMetaResponse;
                getMetaResponse = await getMetaRequest.ExecuteAsync();
                return getMetaResponse.ModifiedTime;
            }
            catch (Exception e)
            {
                SetFailed(e.Message);
            }
            return null;
        }

        private async void UpdateBackup()
        {
            var service = await GetService();

            try
            {
                var uploadStream = new FileStream(_databaseFileFullPath, FileMode.Open, FileAccess.Read);
                var updateRequest = service.Files.Update(
                    new Google.Apis.Drive.v3.Data.File
                    {
                        Name = DB_FILE_NAME
                    },
                    _remoteFileId,
                    uploadStream,
                    MIME_TYPE);

                updateRequest.ProgressChanged += Upload_ProgressChanged;
                updateRequest.ResponseReceived += Upload_ResponseReceived;

                await updateRequest.UploadAsync();
            }
            catch(Exception e)
            {
                SetFailed(e.Message);
            }
        }

        private async void CreateNewBackup()
        {
            var service = await GetService();

            try
            {
                var uploadStream = new FileStream(_databaseFileFullPath, FileMode.Open, FileAccess.Read);
                var insertRequest = service.Files.Create(
                    new Google.Apis.Drive.v3.Data.File
                    {
                        Name = DB_FILE_NAME,
                        Parents = !string.IsNullOrEmpty(_remoteFolderId) ? new List<string> { _remoteFolderId } : null
                    },
                    uploadStream,
                    MIME_TYPE);

                insertRequest.ProgressChanged += Upload_ProgressChanged;
                insertRequest.ResponseReceived += Upload_ResponseReceived;

                await insertRequest.UploadAsync();
            }
            catch(Exception e)
            {
                SetFailed(e.Message);
            }
        }

        private void Upload_ProgressChanged(IUploadProgress progress)
        {
            var status = progress.Status.ToString();

            switch (progress.Status)
            {
                case UploadStatus.Starting:
                    status = ResourcesRU.UploadingStatusStarting;
                    break;
                case UploadStatus.NotStarted:
                    status = ResourcesRU.UploadingStatusNotStarted;
                    break;
                case UploadStatus.Uploading:
                    status = ResourcesRU.UploadingStatusUploading;
                    break;
                case UploadStatus.Completed:
                    status = ResourcesRU.UploadingStatusCompleted;
                    break;
                case UploadStatus.Failed:
                    status = ResourcesRU.UploadingStatusFailed;
                    break;
            }
            UpdateStatus(status);

            if (progress.Status == UploadStatus.Failed)
            {
                SetFailed(progress.Exception.Message);
            }
        }

        private void UpdateStatus(string status)
        {
            OnStatusUpdated?.Invoke(status);
        }

        private void SetFailed(string message)
        {
            OnFailed?.Invoke($"{ResourcesRU.BackupFailed}{Environment.NewLine}{Environment.NewLine}{message}");
        }

        private void SetCompleted()
        {
            OnCompleted?.Invoke();
        }

        private void Upload_ResponseReceived(Google.Apis.Drive.v3.Data.File file)
        {
            SetCompleted();
        }
    }
}
