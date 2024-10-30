using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using AutoMapper;
using Catalog.Core.Dtos;
using Catalog.Core.Entities.Product;
using Catalog.Core.Interfaces;
using Catalog.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductImagesRepository : IProductImagesRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IAmazonS3 _s3Client;
        private readonly CatalogContext _context;
        private readonly IMapper _mapper;

        public ProductImagesRepository(IConfiguration configuration, CatalogContext context, IAmazonS3 s3Client, IMapper mapper)
        {
            _configuration = configuration;
            _context = context;
            _s3Client = s3Client;
            _mapper = mapper;
        }

        public async Task<IList<ProductImageDto>> UploadProductImages(string merchantName, Guid productVariantId, IList<IFormFile> formFiles)
        {
            var response = new List<ProductImageDto>();
            string bucketName = _configuration.GetSection("AmazonS3Settings")["BucketName"];

            foreach (var file in formFiles)
            {
                using (var stream = file.OpenReadStream())
                {
                    // Remove the first file extension from the FileName
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);

                    // Compress the image before uploading
                    using (var image = await Image.LoadAsync(stream))
                    {
                        // Resize the image to your desired dimensions and quality settings
                        image.Mutate(x => x
                            .Resize(new ResizeOptions
                            {
                                Size = new Size(500, 500), // Adjust dimensions as needed
                                Mode = ResizeMode.Max
                            })
                        );

                        // Save the compressed image to a memory stream
                        using (var compressedStream = new MemoryStream())
                        {
                            var pngEncoder = new PngEncoder
                            {
                                CompressionLevel = PngCompressionLevel.BestCompression // Adjust compression level as needed
                            };

                            await image.SaveAsync(compressedStream, pngEncoder);

                            // Upload the compressed image to S3
                            var uploadRequest = new TransferUtilityUploadRequest
                            {
                                InputStream = compressedStream,
                                Key = $"{merchantName}/{productVariantId}/{fileNameWithoutExtension}.png",
                                BucketName = bucketName,
                                CannedACL = S3CannedACL.NoACL
                            };

                            try
                            {
                                using (var fileTransferUtility = new TransferUtility(_s3Client))
                                {
                                    await fileTransferUtility.UploadAsync(uploadRequest);
                                }

                                // Sending response
                                var result = new ProductImageDto
                                {
                                    ImageUrl =
                                        $"https://{uploadRequest.BucketName}.s3.amazonaws.com/{uploadRequest.Key}",
                                    IsMain = false,
                                    ProductVariantId = productVariantId
                                };
                                response.Add(result);

                                // Check if the imageUrl already exits for the variant 
                                var productImage = await GetProductImage(productVariantId, result.ImageUrl);

                                // Saving data to the db
                                try
                                {
                                    if (productImage == null)
                                    {
                                        await _context.ProductImages.AddAsync(_mapper.Map<ProductImage>(result));
                                        await _context.SaveChangesAsync();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception($"Error Saving Variant images: {ex.Message}", ex);
                                }
                            }
                            catch (Exception ex)
                            {
                                throw new AmazonS3Exception($"Error Saving S3 object: {ex.Message}", ex);
                            }
                        }
                    }
                }
            }
            return response;
        }

        public async Task<ProductImageDto> GetProductImage(Guid variantId, string imageUrl)
        {
            var productImage =  await _context.ProductImages
                .FirstOrDefaultAsync(pi => pi.ProductVariantId == variantId && pi.ImageUrl.Equals(imageUrl));

            return _mapper.Map<ProductImageDto>(productImage);
        }

        public async Task SetMainProductImage(ProductImage productImage)
        {
            _context.Entry(productImage).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<ProductImage> GetProductImageById(Guid id)
        {
            return await _context.ProductImages.FirstOrDefaultAsync(pi => pi.Id == id);
        }

        public async Task DeleteProductImages(ProductImage productImage)
        {
            string bucketName = _configuration.GetSection("AmazonS3Settings")["BucketName"];
            string[] parts = productImage.ImageUrl.Split('/');
            int bucketNameIndex = Array.IndexOf(parts, "alligatorshop.s3.amazonaws.com");

            if (bucketNameIndex != -1 && bucketNameIndex < parts.Length - 1)
            {
                // Extract the object key by joining the parts after the bucket name
                string objectKey = string.Join("/", parts, bucketNameIndex + 1, parts.Length - bucketNameIndex - 1);
                
                var deleteRequest = new DeleteObjectRequest
                {
                    BucketName = bucketName,
                    Key = objectKey
                };

                try
                {                    
                    // Attempt to delete the S3 image first
                    await _s3Client.DeleteObjectAsync(deleteRequest);
                    // If the S3 object deletion is successful, then proceed to delete from the db
                    _context.Set<ProductImage>().Remove(productImage);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new AmazonS3Exception($"Error deleting S3 object: {ex.Message}", ex);
                }
            }
            else
            {
                throw new ArgumentException("Invalid imageUrl format");
            }
         
        }
       
    }
}
