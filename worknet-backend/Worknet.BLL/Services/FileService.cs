using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Worknet.BLL.Exceptions;
using Worknet.BLL.Interfaces;
using Worknet.Core.Entities;
using Worknet.DAL;
using Worknet.Shared.Interfaces;
using Worknet.Shared.Models.DTOs;

namespace Worknet.BLL.Services;

public class FileService(
    WorknetDbContext dbContext, 
    IGoogleDriveService googleDriveService,
    IMapper mapper
    ) : IFileService
{
    public async Task DeleteFileByIdAsync(string fileId)
    {
        await googleDriveService.DeleteFileByIdAsync(fileId);

        var file = dbContext.Files.Where(f => f.Id ==  fileId).FirstOrDefault();
        if(file is null)
            throw new WorknetException("File not found.", $"File was expected but found null.");

        dbContext.Files.Remove(file);
        await dbContext.SaveChangesAsync();
    }

    public async Task<GoogleDriveFileDto> GetFileByIdAsync(string fileId)
    {
        var file = dbContext.Files.Where(f => f.Id == fileId).FirstOrDefault();

        var fileDto = mapper.Map<GoogleDriveFileDto>(file);

        if (fileDto is null)
            fileDto = await googleDriveService.GetFileByIdAsync(fileId);

            
        return fileDto ?? throw new WorknetException("File not found.", $"File was expected but found null."); ;
    }

    public async Task<List<GoogleDriveFileDto>> GetFilesByIdsAsync(List<string> fileIds)
    {
        var files = dbContext.Files
            .Where(f => fileIds.Contains(f.Id))
            .AsNoTracking()
            .ToList();

        var dtos = mapper.Map<List<GoogleDriveFileDto>>(files);

        var missingIds = fileIds.Except(files.Select(f => f.Id)).ToList();

        foreach (var missingId in missingIds)
        {
            var driveFile = await googleDriveService.GetFileByIdAsync(missingId);
            if (driveFile is not null)
                dtos.Add(driveFile);
        }

        return dtos;
    }

    public async Task<GoogleDriveFileDto> UploadFileAsync(Stream fileStream, string originalFileName)
    {
        var file = await googleDriveService.UploadFileAsync(fileStream, originalFileName);

        var uploadedFile = await googleDriveService.GetFileByIdAsync(file.Id);

        if (uploadedFile is null)
            throw new WorknetException("Upload failed", "Cannot retrieve uploaded file metadata.");

        var fileEntity = mapper.Map<GoogleDriveFile>(uploadedFile);
        dbContext.Files.Add(fileEntity);
        await dbContext.SaveChangesAsync();

        return uploadedFile;
    }
}