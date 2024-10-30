using Catalog.Core.Entities.Product;
using System.Net;
using Catalog.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    public class ProductVariantController : BaseApiController
    {
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly ILogger<ProductVariantController> _logger;

        public ProductVariantController(IProductVariantRepository productVariantRepository, ILogger<ProductVariantController> logger)
        {
            this._productVariantRepository = productVariantRepository;
            this._logger = logger;
        }


        [HttpGet("{id:length(36)}", Name = "GetProductVariant")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProductVariant), (int)HttpStatusCode.OK)]

        public async Task<ActionResult<ProductVariant>> GetProductVariantById(Guid id)
        {
            var productVariant = await _productVariantRepository.GetProductVariant(id);

            if (productVariant == null)
            {
                _logger.LogError($"Product Variant with Id: {id}, Not Found");
                return NotFound();
            };

            return Ok(productVariant);
        }


        [HttpDelete("{id:length(36)}", Name = "DeleteProductVariant")]
        [ProducesResponseType(typeof(ProductVariant), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProductVariant(Guid id)
        {
            var productVariantToDelete = await _productVariantRepository.GetProductVariant(id);

            if (productVariantToDelete == null)
            {
                _logger.LogError($"Product Variant with Id: {id}, Not Found");
                return NotFound();
            };

            await _productVariantRepository.DeleteProductVariant(productVariantToDelete);
            return Ok();
        }
    }
}
