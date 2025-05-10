using AutoMapper;
using BTBackendOnline2.Configurations;
using DeltaBrainJSC.DB;
using DeltaBrainJSC.DTOs.Request;
using DeltaBrainJSC.DTOs.Response;
using DeltaBrainJSC.Models;
using DeltaBrainJSC.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeltaBrainJSC.Services.Implements
{
    public class DepartmentService : IDepartment
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;

        public DepartmentService(AppDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ApiResponse<DepartmentRes>> Create(DepartmentReq request)
        {
            try
            {
                var existCode = _context.Departments.FirstOrDefault(a => a.Code.Equals(request.Code));

                if (existCode != null)
                {
                    return ApiResponse<DepartmentRes>.Fail("Code đã tồn tại");
                }

                var department = _mapper.Map<Department>(request);

                _context.Departments.Add(department);
                await _context.SaveChangesAsync();

                var response = _mapper.Map<DepartmentRes>(department);


                return ApiResponse<DepartmentRes>.Success(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<DepartmentRes>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> Delete(int id)
        {
            try
            {
                var exist = ExistDepartment(id);

                if (exist == null)
                {
                    return ApiResponse<bool>.NotFound();
                }
                _context.Departments.Remove(exist);
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<List<DepartmentRes>>> GetAllAsync()
        {
            try
            {
                var departments = await _context.Departments
                    .Include(d => d.DepartmentEmployees)
                    .ThenInclude(de => de.Employee)
                    .ToListAsync();

                var response = departments.Select(d => new DepartmentRes
                {
                    Id = d.Id,
                    Code = d.Code,
                    Name = d.Name,
                    Employees = d.DepartmentEmployees
                        .Select(de => _mapper.Map<EmployeeRes>(de.Employee))
                        .ToList()
                }).ToList();

                return ApiResponse<List<DepartmentRes>>.Success(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<DepartmentRes>>.Fail("Lỗi khi lấy danh sách phòng ban: " + ex.Message);
            }
        }

        public async Task<ApiResponse<DepartmentRes>> GetEmployeesByDepartment(int id)
        {
            try
            {

                var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);

                if (department == null)
                {
                    return ApiResponse<DepartmentRes>.Fail("Phòng ban không tồn tại.");
                }

                var departmentEmployees = await _context.DepartmentEmployee
                    .Where(de => de.DepartmentId == department.Id)
                    .Include(de => de.Employee)
                    .Select(de => de.Employee)
                    .ToListAsync();

                if (departmentEmployees == null || !departmentEmployees.Any())
                {
                    return ApiResponse<DepartmentRes>.Fail("Không có nhân viên trong phòng ban này.");
                }


                var employeeResList = _mapper.Map<List<EmployeeRes>>(departmentEmployees);

                var departmentRes = new DepartmentRes
                {
                    Code = department.Code,
                    Name = department.Name,
                    Id = id,
                    Employees = employeeResList
                };

                return ApiResponse<DepartmentRes>.Success(departmentRes);
            }
            catch(Exception ex)
            {
                return ApiResponse<DepartmentRes>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> RemoveEmployeeFromDepartment(int employeeId, int departmentId)
        {
            try
            {
                var relation = await _context.DepartmentEmployee
                    .FirstOrDefaultAsync(de => de.EmployeeId == employeeId && de.DepartmentId == departmentId);

                if (relation == null)
                {
                    return ApiResponse<bool>.Fail("Không tìm thấy mối quan hệ nhân viên - phòng ban.");
                }

                _context.DepartmentEmployee.Remove(relation);
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail("Lỗi khi xóa nhân viên khỏi phòng ban: " + ex.Message);
            }
        }

        public async Task<ApiResponse<DepartmentRes>> Update(DepartmentUpdate request, int id)
        {
            try
            {
                var exist = ExistDepartment(id);

                if (exist == null)
                {
                    return ApiResponse<DepartmentRes>.NotFound();
                }

                exist.Name = request.Name;

                _context.Departments.Update(exist);
                await _context.SaveChangesAsync();

                var response = _mapper.Map<DepartmentRes>(exist);

                return ApiResponse<DepartmentRes>.Success(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<DepartmentRes>.Fail(ex.Message);
            }
        }

        private Department ExistDepartment(int id)
        {
            var existDepartment = _context.Departments.FirstOrDefault(a => a.Id.Equals(id));

            return existDepartment ?? null;
        }
    }
}
