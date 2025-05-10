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
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartment _service;
        public DepartmentController(IDepartment service)
        {
            _service = service;
        }

        [HttpPost("create")]
        public async Task<IActionResult> AddNewDepartmentAsync([FromBody] DepartmentReq request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Dữ liệu không hợp lệ.");

            var response = await _service.Create(request);

            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpGet("get-list")]
        public async Task<IActionResult> GetListDepartment()
        {
            var response = await _service.GetAllAsync();

            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPost("get-employee-by-department")]
        public async Task<IActionResult> GetEmployeesByDepartmenAsynct(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Dữ liệu không hợp lệ.");

            var response = await _service.GetEmployeesByDepartment(id);

            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateEmployeeAsync([FromBody] DepartmentUpdate update)
        {
            if (!ModelState.IsValid)
                return BadRequest("Dữ liệu không hợp lệ.");

            var response = await _service.Update(update, update.Id);

            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var response = await _service.Delete(id);

            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("remove-employee-from-department")]
        public async Task<IActionResult> RemoveEmployeeFromDepartment(int employeeId, int departmentId)
        {
            var response = await _service.RemoveEmployeeFromDepartment(employeeId, departmentId);

            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }
    }
}
