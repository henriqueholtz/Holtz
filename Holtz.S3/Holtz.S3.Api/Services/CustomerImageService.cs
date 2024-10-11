using Amazon.S3;
using Amazon.S3.Model;
using Holtz.S3.Api.Configurations;
using Holtz.S3.Api.Interfaces;
using Microsoft.Extensions.Options;

namespace Holtz.S3.Api.Services;

public class CustomerImageService : ICustomerImageService
{
    private readonly IAmazonS3 _amazonS3;
    private readonly IOptions<S3Settings> _s3Settings;
    private readonly string _bucketName;

    public CustomerImageService(IAmazonS3 amazonS3, IOptions<S3Settings> s3Settings)
    {
        _amazonS3 = amazonS3;
        _s3Settings = s3Settings;
        _bucketName = _s3Settings.Value.BucketName;
    }

    public Task<DeleteObjectResponse> DeleteImageAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private string GetS3Key(Guid id) => $"images/{id}";

    public async Task<GetObjectResponse> GetImageAsync(Guid id, CancellationToken cancellationToken)
    {
        var getObjectRequest = new GetObjectRequest
        {
            BucketName = _bucketName,
            Key = GetS3Key(id)
        };

        return await _amazonS3.GetObjectAsync(getObjectRequest, cancellationToken);
    }

    public async Task<PutObjectResponse> UploadImageAsync(Guid id, IFormFile file, CancellationToken cancellationToken)
    {
        var putObjetRequest = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = GetS3Key(id),
            InputStream = file.OpenReadStream(),
            ContentType = file.ContentType,
            Metadata = {
                ["x-amz-meta-originalname"] = file.FileName,
                ["x-amz-meta-originalextension"] = Path.GetExtension(file.FileName)
            }
        };

        return await _amazonS3.PutObjectAsync(putObjetRequest, cancellationToken);
    }
}
