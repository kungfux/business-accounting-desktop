using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;

namespace BusinessAccounting.Integrations
{
    internal class GoogleDriveBackupStorage
    {
        public async void MakeBackup(string fileToBackup, string remoteFolder = null)
        {
            if (_credential == null)
                Authorize();

            var service = GetService();

            var uploadStream = new FileStream(fileToBackup, FileMode.Open, FileAccess.Read);
            var insertRequest = service.Files.Create(
                new Google.Apis.Drive.v3.Data.File { Name = "ba.sqlite", Parents = !string.IsNullOrEmpty(remoteFolder) ? new List<string> { remoteFolder } : null },
                uploadStream, "application/x-sqlite3");

            insertRequest.ProgressChanged += Upload_ProgressChanged;
            insertRequest.ResponseReceived += Upload_ResponseReceived;

            await insertRequest.UploadAsync();
        }

        public event Action<string> OnUpdateStatus;
        private void UpdateStatus(string status)
        {
            if (OnUpdateStatus == null) return;

            OnUpdateStatus(status);
        }

        public event Action<string> OnFailed;
        private void SetFailed(string message)
        {
            if (OnFailed == null) return;

            OnFailed(message);
        }

        private UserCredential _credential;

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

        private DriveService GetService()
        {
            return new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = _credential,
                ApplicationName = Assembly.GetExecutingAssembly().GetName().Name
            });
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

        private void Upload_ResponseReceived(Google.Apis.Drive.v3.Data.File file)
        {
            UpdateStatus(file.Name + " был успешно загружен.");
        }
    }
}
