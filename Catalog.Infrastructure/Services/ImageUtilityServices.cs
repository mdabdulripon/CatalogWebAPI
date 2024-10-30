using Amazon.S3;
using Amazon.S3.Model;

namespace Catalog.Infrastructure.Services;

public static class ImageUtilityServices
{
    public static async Task<string> GetVersionId(string bucketName, string objectKey, IAmazonS3 s3Client)
    {
        try
        {
            var request = new GetObjectMetadataRequest
            {
                BucketName = bucketName,
                Key = objectKey
            };
        
            var response = await s3Client.GetObjectMetadataAsync(request);
            return response.VersionId;
        }
        catch (AmazonS3Exception ex)
        {
            throw new AmazonS3Exception($"An error occurred with Amazon S3: {ex.Message}");
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred:: {ex.Message}");
        }
    }
}