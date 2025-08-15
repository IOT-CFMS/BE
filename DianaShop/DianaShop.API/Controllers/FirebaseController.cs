using DianaShop.Service.Interfaces;
using DianaShop.Service.ReponseModel;
using DianaShop.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace DianaShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirebaseController : ControllerBase
    {
        private readonly IFirebaseService _firebaseService;
        private readonly IKioskCameraService _kioskCameraService;

        public FirebaseController(IFirebaseService firebaseService, IKioskCameraService kioskCameraService)
        {
            _firebaseService = firebaseService;
            _kioskCameraService = kioskCameraService;
        }

        /// <summary>
        /// Lấy ảnh (base64), nhiệt độ và thời gian từ Firebase
        /// </summary>
        [HttpGet("{deviceId}")]
        public async Task<IActionResult> GetAllSensor(string deviceId)
        {
            var respond = await _firebaseService.GetSensorData(deviceId);
            return Ok(respond);
        }

        //[HttpGet("{path}")]
        //public async Task<IActionResult> GetData(string path)
        //{
        //    var result = await _firebaseService.GetRawDataAsync(path);
        //    if (result != null)
        //    {
        //        var response = await _kioskCameraService.ManualAddCamera(result);
        //    }
        //    return Ok(result);
        //}

        //[HttpGet("image-temp")]
        //public async Task<ActionResult<FirebaseRespondModel>> GetImageAndTemp()
        //{
        //    var result = await _firebaseService.GetSensorData();
        //    if (result == null)
        //        return NotFound("No data found in Firebase.");

        //    return Ok(result);
        //}
    }
}
