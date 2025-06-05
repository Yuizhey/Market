using Microsoft.AspNetCore.Http;

namespace Market.Application.Interfaces.Services;

public interface IMinioService
{
    Task<string> UploadCoverImageAsync(IFormFile file, Guid productId, CancellationToken cancellationToken);
    Task<string> GetCoverImageUrlAsync(string objectName, CancellationToken cancellationToken);
    Task DeleteFileAsync(string filePath, CancellationToken cancellationToken);
}