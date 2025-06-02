
namespace Market.Application.Interfaces.Services;

public interface IFileStorageService
{
    Task<string> UploadFileAsync(Stream stream, string fileName, string contentType, long size);
}
