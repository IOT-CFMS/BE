using DianaShop.Service.Interfaces;
using DianaShop.Service.RequestModel;
using DianaShop.Service.RequestModel.QueryRequest;
using DianaShop.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DianaShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KioskController : ControllerBase
    {
        private readonly IKioskService _kioskService;

        public KioskController(IKioskService kioskService)
        {
            _kioskService = kioskService;
        }

        // GET: api/product
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _kioskService.GetAllKiosks();
            return Ok(response);
        }

        [HttpPost]
        //[Authorize(Roles = "Staff,Manager")]
        public async Task<IActionResult> Create([FromForm] KioskRequestModel request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _kioskService.CreateKiosk(request);
            return Ok(response);
        }
    }
}
