using DianaShop.Service.Interfaces;
using DianaShop.Service.RequestModel;
using DianaShop.Service.RequestModel.QueryRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DianaShop.API.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/product
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ProductQueryRequest request)
        {
            var response = await _productService.GetAllProductsAsync(request);
            return Ok(response);
        }

        // GET: api/product/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _productService.GetProductByIdAsync(id);
            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpPost]
        //[Authorize(Roles = "Staff,Manager")]
        public async Task<IActionResult> Create([FromForm] ProductRequestModel request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _productService.AddProductAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "Staff,Manager")]
        public async Task<IActionResult> Update(int id, [FromForm] ProductRequestModel request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _productService.GetProductByIdAsync(id);
            if (response == null) return NotFound();
            await _productService.UpdateProductAsync(id, request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "Staff,Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _productService.GetProductByIdAsync(id);
            if (response == null) return NotFound();
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }
    }
}
