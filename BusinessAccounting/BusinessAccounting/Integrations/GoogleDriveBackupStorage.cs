using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;

namespace BusinessAccounting.Integrations
{
    internal class GoogleDriveBackupStorage
    {
        public event Action<string> OnUpdateStatus;
        public event Action<string> OnFailed;
        public event EventHandler OnAutoBackup;

        private UserCredential _credential;
        private string _databaseFileFullPath;
        private string _remoteFolderId;
        private string _remoteFileId;
        private int _autoBackupInterval;

        public GoogleDriveBackupStorage(string DatabaseFileFullPath, string RemoteFolderId, string RemoteFileId, int AutoBackupInterval)
        {
            _databaseFileFullPath = DatabaseFileFullPath;
            _remoteFolderId = RemoteFolderId;
            _remoteFileId = RemoteFileId;
            _autoBackupInterval = AutoBackupInterval;
        }

        public void MakeBackup()
        {
            if (_remoteFileId != null)
            {
                BackupToFile();
            }
            else if (_remoteFolderId != null)
            {
                BackupToFolder();
            }
        }

        public async void MakeAutoBackup()
        {
            if (_autoBackupInterval <= 0)
                return;

            var lastBackup = await GetLastBackupDate();
            if (lastBackup.HasValue && lastBackup.Value.AddDays(_autoBackupInterval) <= DateTime.Now)
            {
                MakeBackup();
                OnAutoBackup?.Invoke(this, null);
            }
        }

        private DriveService GetService()
        {
            if (_credential == null)
            {
                Authorize();
            }

            return new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = _credential,
                ApplicationName = Assembly.GetExecutingAssembly().GetName().Name
            });
        }

        private void Authorize()
        {
            using (var stream = new FileStream("google_drive_api.json", FileMode.Open, FileAccess.Read))
            {
                var credentialPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                credentialPath = Path.Combine(credentialPath, ".credentials/business-accounting.json");

                _credential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets,
                    new[] { DriveService.Scope.DriveFile }, "user", CancellationToken.None, new FileDataStore(credentialPath, true)).Result;
            }
        }

        private async Task<DateTime?> GetLastBackupDate()
        {
            if (_remoteFileId == null)
                return null;

            var service = GetService();
            var getMetaRequest = service.Files.Get(_remoteFileId);
            getMetaRequest.Fields = "modifiedTime";
            var getMetaResponse = await getMetaRequest.ExecuteAsync();
            return getMetaResponse.ModifiedTime;
        }

        private async void BackupToFile()
        {
            var service = GetService();

            var uploadStream = new FileStream(_databaseFileFullPath, FileMode.Open, FileAccess.Read);
            var updateRequest = service.Files.Update(
                new Google.Apis.Drive.v3.Data.File
                {
                    Name = "ba.sqlite"
                },
                _remoteFileId,
                uploadStream, 
                "application/x-sqlite3");

            updateRequest.ProgressChanged += Upload_ProgressChanged;
            updateRequest.ResponseReceived += Upload_ResponseReceived;

            await updateRequest.UploadAsync();
        }

        private async void BackupToFolder()
        {
            var service = GetService();

            var uploadStream = new FileStream(_databaseFileFullPath, FileMode.Open, FileAccess.Read);
            var insertRequest = service.Files.Create(
                new Google.Apis.Drive.v3.Data.File
                {
                    Name = "ba.sqlite",
                    Parents = !string.IsNullOrEmpty(_remoteFolderId) ? new List<string> { _remoteFolderId } : null
                },
                uploadStream,
                "application/x-sqlite3");

            insertRequest.ProgressChanged += Upload_ProgressChanged;
            insertRequest.ResponseReceived += Upload_ResponseReceived;

            await insertRequest.UploadAsync();
        }

        private void Upload_ProgressChanged(IUploadProgress progress)
        {
            var status = progress.Status.ToString();

            switch (progress.Status)
            {
                case UploadStatus.Starting:
                    status = "Начинаю загрузку...";
                    break;
                case UploadStatus.NotStarted:
                    status = "Загрузка не начата.";
                    break;
                case UploadStatus.Uploading:
                    status = "Загрузка...";
                    break;
                case UploadStatus.Completed:
                    status = "Завершено.";
                    break;
                case UploadStatus.Failed:
                    status = "Ошибка.";
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
            OnUpdateStatus?.Invoke(status);
        }

        private void SetFailed(string message)
        {
            OnFailed?.Invoke(message);
        }

        private void Upload_ResponseReceived(Google.Apis.Drive.v3.Data.File file)
        {
            UpdateStatus(file.Name + " был успешно загружен.");
        }
    }
}
