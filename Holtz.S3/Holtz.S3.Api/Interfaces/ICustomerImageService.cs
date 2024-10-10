using Amazon.S3.Model;

namespace Holtz.S3.Api.Interfaces;

public interface ICustomerImageService
{
    Task<PutObjectResponse> UploadImageAsync(Guid id, IFormFile file, CancellationToken cancellationToken);
    Task<GetObjectResponse> GetImageAsync(Guid id, CancellationToken cancellationToken);
    Task<DeleteObjectResponse> DeleteImageAsync(Guid id, CancellationToken cancellationToken);
}
