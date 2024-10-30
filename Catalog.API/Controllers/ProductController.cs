using System.Net;
using Catalog.Core.Dtos;
using Catalog.Core.Entities.Product;
using Catalog.Core.Interfaces;
using Catalog.Core.RequestHelpers;
using Catalog.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    public class ProductController : BaseApiController
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductRepository productRepository, ILogger<ProductController> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedList<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PagedList<Product>>> GetAllProducts([FromQuery] ProductParams productParams)
        {
            var products = await _productRepository.GetAllProducts(productParams);
            Response.AddPaginationHeader(products.MetaData);
            return Ok(products);
        }

        [HttpGet("{id:length(36)}", Name = "GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]

        public async Task<ActionResult<Product>> GetProductById(Guid id)
        {
            var product = await _productRepository.GetProduct(id);

            if (product == null)
            {
                _logger.LogError($"Product with Id: {id}, Not Found");
                return NotFound();
            };

            return Ok(product);
        }


        [Route("[action]/{categoryName}", Name = "GetProductByCategory")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string categoryName)
        {
            var products = await _productRepository.GetProductsByCategory(categoryName);

            return Ok(products);
        }


        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await _productRepository.CreateProduct(product);

            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }


        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductDto product)
        {
            /******
            * * NOTE: IActionResult => because there is no response waiting form the result
            * * Use IActionResult if you do want to return any specific type of respponse 
            *******/
            await _productRepository.UpdateProduct(product);
            return Ok();
        }


        [HttpDelete("{id:length(36)}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            /******
            * * NOTE: IActionResult => because there is no response waiting form the result
            * * Use IActionResult if you do want to return any specific type of respponse 
            *******/
            var productToDelete = await _productRepository.GetProduct(id);

            if (productToDelete == null)
            {
                _logger.LogError($"Product with Id: {id}, Not Found");
                return NotFound();
            };

            await _productRepository.DeleteProduct(productToDelete);
            return Ok();
        }

        [HttpGet("filters")]
        public async Task<IActionResult> GetFilters(string merchantName) 
        {
            var filters = await _productRepository.GetFilters(merchantName);
            return Ok(filters);
        }

    }
}
