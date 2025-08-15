using DianaShop.Data.Entities;
using DianaShop.Service.Interfaces;
using DianaShop.Service.RequestModel.QueryRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace DianaShop.API.Controllers
{
    public class ImageController : BaseController
    {
        private readonly IImageService _imageService;
        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }
        // GET: api/image
        [HttpGet]
        [Authorize(Roles = "Staff,Manager")]
        public async Task<IActionResult> GetAll([FromQuery] ImageQueryRequest request)
        {
            var response = await _imageService.GetAllAsync(request);
            return Ok(response);
        }
        // GET: api/image/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Staff,Manager")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _imageService.GetByIdAsync(id);
            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Staff,Manager")]
        public async Task<IActionResult> Create(List<IFormFile> files)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _imageService.AddAsync(files);
            return Ok(response);
        }
        //[HttpPost]
        ////[Authorize(Roles = "Staff,Manager")]
        //public async Task<IActionResult> Create(List<String> files)
        //{
        //    //if (!ModelState.IsValid) return BadRequest(ModelState);
        //    var response = await _imageService.AddAsyncB64(files);
        //    return Ok(response);
        //}

        [HttpPut("{id}")]
        [Authorize(Roles = "Staff,Manager")]
        public async Task<IActionResult> Update(int id, IFormFile file)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _imageService.UpdateAsync(id, file);
            return NoContent();
        }
        [HttpDelete("soft-deletion/{id}")]
        [Authorize(Roles = "Staff,Manager")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var response = await _imageService.GetByIdAsync(id);
            if (response == null) return NotFound();
            await _imageService.SoftDeleteAsync(id);
            return NoContent();
        }
        [HttpDelete("hard-deletion/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> HardDelete(int id)
        {
            var response = await _imageService.GetByIdAsync(id);
            if (response == null) return NotFound();
            await _imageService.HardDeleteAsync(id);
            return NoContent();
        }

        [HttpPost("product/{productId}")]
        [Authorize(Roles = "Staff,Manager")]
        public async Task<IActionResult> AddImageToProduct(int productId, int imageId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _imageService.AddImageToProductAsync(productId, imageId);
            return NoContent();
        }

        [HttpDelete("product/{productId}")]
        [Authorize(Roles = "Staff,Manager")]
        public async Task<IActionResult> RemoveImageFromProduct(int productId, int imageId)
        {
            await _imageService.RemoveImageFromProductAsync(productId, imageId);
            return NoContent();
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetImagesByProductId(int productId)
        {
            var response = await _imageService.GetImagesByProductIdAsync(productId);
            return Ok(response);
        }


        //[HttpPost("post/{postId}")]
        //[Authorize(Roles = "Staff,Manager")]
        //public async Task<IActionResult> AddImageToPost(int postId, int imageId)
        //{
        //    if (!ModelState.IsValid) return BadRequest(ModelState);
        //    await _imageService.AddImageToPostAsync(postId, imageId);
        //    return NoContent();
        //}
        //[HttpDelete("post/{postId}")]
        //[Authorize(Roles = "Staff,Manager")]
        //public async Task<IActionResult> RemoveImageFromPost(int postId, int imageId)
        //{
        //    await _imageService.RemoveImageFromPostAsync(postId, imageId);
        //    return NoContent();
        //}
        //[HttpGet("post/{postId}")]
        //public async Task<IActionResult> GetImagesByPostId(int postId)
        //{
        //    var response = await _imageService.GetImagesByPostIdAsync(postId);
        //    return Ok(response);
        //}

        [HttpPost("profile/{userId}")]
        [Authorize]
        public async Task<IActionResult> AddProfileImage(int userId, IFormFile file)
        {
            await _imageService.AddProfileImageAsync(userId, file);
            return NoContent();
        }
    }
}
