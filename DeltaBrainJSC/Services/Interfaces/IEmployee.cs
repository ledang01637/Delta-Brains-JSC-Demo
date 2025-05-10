using BTBackendOnline2.Configurations;
using DeltaBrainJSC.DTOs.Request;
using DeltaBrainJSC.DTOs.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeltaBrainJSC.Services.Interfaces
{
    public interface IEmployee
    {
        /// <summary>
        /// Lấy danh sách Employee
        /// </summary>
        /// <returns></returns>
        Task<ApiResponse<List<EmployeeRes>>> GetAllAsync();

        /// <summary>
        /// Lấy Employee theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApiResponse<EmployeeRes>> GetById(int id);

        /// <summary>
        /// Thêm Employee
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResponse<EmployeeRes>> Create(EmployeeReq request);

        /// <summary>
        /// Sửa Employee
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApiResponse<EmployeeRes>> Update(EmployeeUpdate request, int id);

        /// <summary>
        /// Xóa Employee
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> Delete(int id);
    }
}
