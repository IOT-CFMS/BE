using DianaShop.Service.Interfaces;
using DianaShop.Service.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DianaShop.API.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _categoryService.GetAllAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _categoryService.GetByIdAsync(id);
            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpPost]
        //[Authorize(Roles = "Staff,Manager")]
        public async Task<IActionResult> Create([FromForm] CategoryRequestModel request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _categoryService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "Staff,Manager")]
        public async Task<IActionResult> Update(int id, [FromForm] CategoryRequestModel request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _categoryService.GetByIdAsync(id);
            if (response == null) return NotFound();
            var updateResponse = await _categoryService.UpdateAsync(id, request);
            if (updateResponse == null) return NotFound();
            return Ok(updateResponse);
        }

        [HttpDelete("soft-deletion/{id}")]
        //[Authorize(Roles = "Staff,Manager")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var response = await _categoryService.GetByIdAsync(id);
            if (response == null) return NotFound();
            await _categoryService.SoftDeleteAsync(id);
            return NoContent();
        }

        [HttpDelete("hard-deletion/{id}")]
        //[Authorize(Roles = "Manager")]
        public async Task<IActionResult> HardDelete(int id)
        {
            var response = await _categoryService.GetByIdAsync(id);
            if (response == null) return NotFound();
            await _categoryService.HardDeleteAsync(id);
            return NoContent();
        }
    }
}
