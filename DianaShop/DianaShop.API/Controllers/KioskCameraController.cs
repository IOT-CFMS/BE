using DianaShop.Service.Interfaces;
using DianaShop.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DianaShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KioskCameraController : ControllerBase
    {
        private readonly IKioskCameraService _kioskCameraService;

        public KioskCameraController(IKioskCameraService kioskCameraService)
        {
            _kioskCameraService = kioskCameraService;
        }

        [HttpGet("{path}")]
        public async Task<IActionResult> GetData(string path)
        {
            var result = await _kioskCameraService.ManualAddCamera(path);
            return Ok(result);
        }
    }
}
