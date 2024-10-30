using System.Net;
using Catalog.Core.Entities.Product;
using Catalog.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    public class ProductAttributeController : BaseApiController
    {
        private readonly ILogger<ProductAttributeController> _logger;
        private readonly IProductAttributeRepository _productAttributeRepository;

        public ProductAttributeController(IProductAttributeRepository productAttributeRepository, ILogger<ProductAttributeController> logger)
        {
            _productAttributeRepository = productAttributeRepository;
            _logger = logger;
        }


        [HttpDelete("{productAttributedId:length(36)}", Name = "DeleteProductAttribute")]
        [ProducesResponseType(typeof(ProductAttribute), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProductVariant(Guid productAttributedId)
        {
            var productProductAttributeToDelete = await _productAttributeRepository.GetProductAttributeById(productAttributedId);

            if (productProductAttributeToDelete == null)
            {
                _logger.LogError($"Product Attribute Image with Id: {productAttributedId}, Not Found");
                return NotFound();
            };

            await _productAttributeRepository.DeleteProductAttribute(productProductAttributeToDelete);
            return Ok();
        }
    }
}