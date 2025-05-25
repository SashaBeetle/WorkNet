using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Worknet.Core.Configurations;
using Worknet.Shared.Constantsl;

namespace Worknet.Infrastructure.GoogleDrive
{
    public class GoogleDriveClient
    {
        private readonly DriveService _driveService;
        private GoogleDriveConfig _googleDriveConfing;

        private readonly string _timestampFormat = "yyyyMMdd_HHmmss";
        private readonly string _uploadFileFields = "id, webViewLink, name";
        private readonly string _getFileFields = "id, name, mimeType, webViewLink, createdTime, size";
        private readonly string _makeFilePublicFields = "id";
        private readonly string _makeFilePublicType = "anyone";
        private readonly string _makeFilePublicRole= "reader";

        public GoogleDriveClient(IHostEnvironment env, IOptions<GoogleDriveConfig> googleDriveOptions)
        {
            _googleDriveConfing = googleDriveOptions.Value;

            var keyPath = Path.Combine(env.ContentRootPath, AppSettings.AppDataFolderName, _googleDriveConfing.JsonName);

            var credential = GoogleCredential.FromFile(keyPath).CreateScoped(DriveService.Scope.DriveFile);

            _driveService = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = AppSettings.AppName
            });
        }

        public async Task<Google.Apis.Drive.v3.Data.File> UploadFileAsync(Stream fileStream, string originalFileName, string mimeType) 
        {
            var fileName = MakeUniqueFileName(originalFileName);

            var fileMetadata = new Google.Apis.Drive.v3.Data.File
            {
                Name = fileName,
                Parents = new[] { _googleDriveConfing.FolderId }
            };

            var request = _driveService.Files.Create(fileMetadata, fileStream, mimeType);
            request.Fields = _uploadFileFields;
            await request.UploadAsync();

            var uploadedFile = request.ResponseBody;

            if (uploadedFile == null)
                throw new Exception("Upload failed: response is null.");

            await MakeFilePublicAsync(uploadedFile.Id);

            return uploadedFile;
        }

        public async Task<Google.Apis.Drive.v3.Data.File?> GetFileByIdAsync(string fileId)
        {
            var request = _driveService.Files.Get(fileId);
            request.Fields = _getFileFields;
            var file = await request.ExecuteAsync();
            return file;
        }

        public async Task<List<Google.Apis.Drive.v3.Data.File>> GetFilesByIdsAsync(List<string> fileIds)
        {
            var files = new List<Google.Apis.Drive.v3.Data.File>();

            foreach (var fileId in fileIds)
            {
                var file = await GetFileByIdAsync(fileId);
                if (file != null)
                {
                    files.Add(file);
                }
            }

            return files;
        }

        public async Task DeleteFileByIdAsync(string fileId)
        {
            var deleteRequest = _driveService.Files.Delete(fileId);
            await deleteRequest.ExecuteAsync();
        }

        private async Task MakeFilePublicAsync(string fileId)
        {
            var permission = new Google.Apis.Drive.v3.Data.Permission
            {
                Type = _makeFilePublicType,
                Role = _makeFilePublicRole
            };

            var request = _driveService.Permissions.Create(permission, fileId);
            request.Fields = _makeFilePublicFields;
            await request.ExecuteAsync();
        }

        private string MakeUniqueFileName(string originalFileName)
        {
            var extension = Path.GetExtension(originalFileName);
            var baseName = Path.GetFileNameWithoutExtension(originalFileName);
            var timestamp = DateTime.UtcNow.ToString(_timestampFormat);

            return $"{baseName}_{timestamp}{extension}";
        }
    }
}