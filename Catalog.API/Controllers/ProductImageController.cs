using System.Net;
using Catalog.Core.Dtos;
using Catalog.Core.Entities.Product;
using Catalog.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    public class ProductImagesController : BaseApiController
    {
        private readonly ILogger<ProductImagesController> _logger;
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly IProductImagesRepository _productImagesRepository;

        public ProductImagesController(IProductVariantRepository productVariantRepository, IProductImagesRepository productImagesRepository, ILogger<ProductImagesController> logger)
        {
            _productVariantRepository = productVariantRepository;
            _productImagesRepository = productImagesRepository;
            _logger = logger;
        }


        /// <summary>
        /// Upload product images for a specific product variant.
        /// </summary>
        /// <remarks>
        /// Route to upload product images for a specific product (id) associated with a merchant (merchantName).
        /// </remarks>
        /// <param name="merchantName">The merchant's name like 124124, zahins.</param>
        /// <param name="variantId">The productVariantId ID (it is a guid).</param>
        [HttpPost("{merchantName}/{variantId:length(36)}")]
        public async Task<ActionResult<ProductImageDto>> UploadProductImages(string merchantName, Guid variantId, IList<IFormFile> formFiles)
        {
            if (formFiles == null || formFiles.Count == 0)
            {
                return BadRequest("No Images uploaded.");
            }

            var productVariant = await _productVariantRepository.GetProductVariant(variantId);
            if (productVariant == null)
            {
                return BadRequest("No product variant found.");
            }

            var productImages = await _productImagesRepository.UploadProductImages(merchantName, variantId, formFiles);
            return Ok(productImages);
        }

        [HttpPut("{variantId:length(36)}/{imageId:length(36)}")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> SetMainProductImage(Guid variantId, Guid imageId)
        {
            var productVariant = await _productVariantRepository.GetProductVariant(variantId);

            foreach (var pi in productVariant.ProductImages)
            {
                pi.IsMain = pi.Id == imageId;
                await _productImagesRepository.SetMainProductImage(pi);
            }

            return Ok(productVariant.ProductImages);
        }


        [HttpDelete("{imageId:length(36)}", Name = "DeleteProductImage")]
        [ProducesResponseType(typeof(ProductImage), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProductVariant(Guid imageId)
        {
            var productImageToDelete = await _productImagesRepository.GetProductImageById(imageId);

            if (productImageToDelete == null)
            {
                _logger.LogError($"Product Image with Id: {imageId}, Not Found");
                return NotFound();
            };

            await _productImagesRepository.DeleteProductImages(productImageToDelete);
            return Ok();
        }
    }
}
