using Worknet.Shared.Models.DTOs;

namespace Worknet.Shared.Interfaces;
public interface IGoogleDriveService
{
    public Task<GoogleDriveFileDto> UploadFileAsync(Stream fileStream, string originalFileName);
    public Task<GoogleDriveFileDto> GetFileByIdAsync(string fileId);
    public Task<List<GoogleDriveFileDto>> GetFilesByIdsAsync(List<string> fileIds);
    public Task DeleteFileByIdAsync(string fileId);
}