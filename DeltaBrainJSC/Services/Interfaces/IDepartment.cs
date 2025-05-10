using BTBackendOnline2.Configurations;
using DeltaBrainJSC.DTOs.Request;
using DeltaBrainJSC.DTOs.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeltaBrainJSC.Services.Interfaces
{
    public interface IDepartment
    {
        /// <summary>
        /// Lấy danh sách Department
        /// </summary>
        /// <returns></returns>
        Task<ApiResponse<List<DepartmentRes>>> GetAllAsync();

        /// <summary>
        /// Lấy danh sách Employee theo Department
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApiResponse<DepartmentRes>> GetEmployeesByDepartment(int id);

        /// <summary>
        /// Lấy danh sách Employee theo Department
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> RemoveEmployeeFromDepartment(int employeeId, int departmentId);

        /// <summary>
        /// Thêm Department
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResponse<DepartmentRes>> Create(DepartmentReq request);

        /// <summary>
        /// Sửa Department
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApiResponse<DepartmentRes>> Update(DepartmentUpdate request, int id);

        /// <summary>
        /// Xóa Department
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> Delete(int id);

        
    }
}
