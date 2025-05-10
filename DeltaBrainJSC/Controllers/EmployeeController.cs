using DeltaBrainJSC.DTOs.Request;
using DeltaBrainJSC.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace DeltaBrainJSC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployee _service;
        public EmployeeController(IEmployee service)
        {
            _service = service;
        }

        [HttpPost("create")]
        public async Task<IActionResult> AddNewEmployeeAsync([FromForm] EmployeeReq request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Dữ liệu không hợp lệ.");

            var errorMsg = ValidateEmployeeInput(request.SexReq, request.AvatarFile, out byte[] avatarBytes, false);

            if (!string.IsNullOrEmpty(errorMsg))
                return BadRequest(errorMsg);

            request.Avatar = avatarBytes;

            var response = await _service.Create(request);

            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpGet("get-list")]
        public async Task<IActionResult> GetListEmployee()
        {
            var response = await _service.GetAllAsync();

            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var response = await _service.GetById(id);

            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateEmployeeAsync([FromForm] EmployeeUpdate update)
        {
            if (!ModelState.IsValid)
                return BadRequest("Dữ liệu không hợp lệ.");

            var errorMsg = ValidateEmployeeInput(update.SexReq, update.AvatarFile, out byte[] avatarBytes, true);

            if (!string.IsNullOrEmpty(errorMsg))
                return BadRequest(errorMsg);

            if (avatarBytes != null)
                update.Avatar = avatarBytes;

            var response = await _service.Update(update, update.Id);

            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var response = await _service.Delete(id);

            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        private static string ValidateEmployeeInput(string sexReq, IFormFile avatarFile, out byte[] avatarBytes, bool isUpdate = false)
        {
            avatarBytes = null;

            if (avatarFile != null)
            {
                using var ms = new MemoryStream();
                avatarFile.CopyTo(ms);
                avatarBytes = ms.ToArray();
            }

            if (string.IsNullOrEmpty(sexReq) ||
                (sexReq.ToLower() != "nam" && sexReq.ToLower() != "nữ" && sexReq.ToLower() != "khác"))
            {
                return "Giới tính phải là Nam, Nữ hoặc Khác.";
            }

            return string.Empty;
        }

    }
}
