using Worknet.Shared.Helpers;
using Worknet.Shared.Interfaces;
using Worknet.Shared.Models.DTOs;

namespace Worknet.Infrastructure.GoogleDrive;
public class GoogleDriveService(GoogleDriveClient googleDriveClient) : IGoogleDriveService
{
    public async Task<GoogleDriveFileDto> GetFileByIdAsync(string fileId)
    {
        var file = await googleDriveClient.GetFileByIdAsync(fileId);
        var extension = Path.GetExtension(file.Name);

        return MapFileToDto(file, extension);
    }

    public async Task<List<GoogleDriveFileDto>> GetFilesByIdsAsync(List<string> fileIds)
    {
        var files = await googleDriveClient.GetFilesByIdsAsync(fileIds);

        var fileDtos = new List<GoogleDriveFileDto>();
        foreach (var file in files) 
        {
            var extension = Path.GetExtension(file.Name);

            fileDtos.Add(MapFileToDto(file, extension));
        }

        return fileDtos;
    }

    public async Task<GoogleDriveFileDto> UploadFileAsync(Stream fileStream, string originalFileName)
    {
        var extension = Path.GetExtension(originalFileName);

        var mimeType = MimeTypeHelper.GetStringFromMimeString(extension);

        var uploadedFile = await googleDriveClient.UploadFileAsync(fileStream, originalFileName, mimeType);

        return MapFileToDto(uploadedFile, extension);
    }
    public async Task DeleteFileByIdAsync(string fileId) => await googleDriveClient.DeleteFileByIdAsync(fileId);

    private GoogleDriveFileDto MapFileToDto(Google.Apis.Drive.v3.Data.File file, string extension) =>
        new()
        {
            Id = file.Id,
            Name = file.Name,
            ExtensionType = MimeTypeHelper.GetMimeEnumFromExtension(extension),
            ViewLink = FileHelper.GetPublicImageUrl(file.Id)
        };
}