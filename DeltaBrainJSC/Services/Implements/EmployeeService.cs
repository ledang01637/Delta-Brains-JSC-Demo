using AutoMapper;
using BTBackendOnline2.Configurations;
using DeltaBrainJSC.DB;
using DeltaBrainJSC.DTOs.Request;
using DeltaBrainJSC.DTOs.Response;
using DeltaBrainJSC.Enum;
using DeltaBrainJSC.Models;
using DeltaBrainJSC.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DeltaBrainJSC.Services.Implements
{
    public class EmployeeService : IEmployee
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;

        public EmployeeService(AppDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<EmployeeRes>> Create(EmployeeReq request)
        {
            try
            {
                if (request.Phone.Length < 10 || request.Phone.Length > 11 || !request.Phone.All(char.IsDigit))
                {
                    return ApiResponse<EmployeeRes>.Fail("Số điện thoại phải từ 10 đến 11 chữ số.");
                }

                var phoneExists = await _context.Employees.AnyAsync(e => e.Phone == request.Phone);
                if (phoneExists)
                {
                    return ApiResponse<EmployeeRes>.Fail("Số điện thoại đã tồn tại.");
                }

                var emailExists = await _context.Employees.AnyAsync(e => e.Email == request.Email);
                if (emailExists)
                {
                    return ApiResponse<EmployeeRes>.Fail("Email đã tồn tại.");
                }


                if (string.IsNullOrWhiteSpace(request.Code) || request.Code.Trim().ToLower() == "null")
                {
                    var lastEmployee = await _context.Employees
                        .OrderByDescending(e => e.Id)
                        .FirstOrDefaultAsync();

                    request.Code = lastEmployee == null || string.IsNullOrWhiteSpace(lastEmployee.Code)
                        ? "00001-Emp"
                        : GenerateNextCode(lastEmployee.Code);
                }


                var existCode = _context.Employees.FirstOrDefault(a => a.Code.Equals(request.Code));

                if (existCode != null)
                {
                    return ApiResponse<EmployeeRes>.Fail("Code đã tồn tại");
                }

                var employee = _mapper.Map<Employee>(request);

                employee.Sex = GetEnumSex(request.SexReq);

                employee.Avatar = request.Avatar;

                _context.Employees.Add(employee);

                await _context.SaveChangesAsync();

                if (request.DepartmentIds != null && request.DepartmentIds.Any())
                {

                    var validDepartmentIds = await _context.Departments
                       .Where(d => request.DepartmentIds.Contains(d.Id))
                       .Select(d => d.Id)
                       .ToListAsync();

                    if (!validDepartmentIds.Any())
                    {
                        return ApiResponse<EmployeeRes>.Fail("Không có phòng ban hợp lệ.");
                    }

                    var depEmpList = validDepartmentIds.Select(depId => new DepartmentEmployee
                    {
                        EmployeeId = employee.Id,
                        DepartmentId = depId

                    }).ToList();

                    _context.DepartmentEmployee.AddRange(depEmpList);
                    await _context.SaveChangesAsync();
                }

                var fullEmployee = await _context.Employees
                    .Include(e => e.DepartmentEmployees)
                    .ThenInclude(de => de.Department)
                    .FirstOrDefaultAsync(e => e.Id == employee.Id);

                var response = _mapper.Map<EmployeeRes>(fullEmployee);

                response.Avatar = fullEmployee.Avatar;

                response.Sex = fullEmployee.Sex.ToString();

                return ApiResponse<EmployeeRes>.Success(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<EmployeeRes>.Fail(ex.Message);
            }
        }



        public async Task<ApiResponse<bool>> Delete(int id)
        {
            try
            {
                var exist = await _context.Employees
                    .Include(e => e.DepartmentEmployees)
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (exist == null)
                {
                    return ApiResponse<bool>.NotFound();
                }

                if (exist.DepartmentEmployees != null && exist.DepartmentEmployees.Any())
                {
                    _context.DepartmentEmployee.RemoveRange(exist.DepartmentEmployees);
                }

                _context.Employees.Remove(exist);
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail("Lỗi khi xóa nhân viên: " + ex.Message);
            }
        }


        public async Task<ApiResponse<List<EmployeeRes>>> GetAllAsync()
        {
            try
            {
                var fullEmployee = await _context.Employees
                    .Include(e => e.DepartmentEmployees)
                    .ThenInclude(de => de.Department)
                    .ToListAsync();

                var response = _mapper.Map<List<EmployeeRes>>(fullEmployee);

                return ApiResponse<List<EmployeeRes>>.Success(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<EmployeeRes>>.Fail(ex.Message);
            }
        }



        public async Task<ApiResponse<EmployeeRes>> Update(EmployeeUpdate request, int id)
        {
            try
            {
                if (request.Phone.Length < 10 || request.Phone.Length > 11 || !request.Phone.All(char.IsDigit))
                {
                    return ApiResponse<EmployeeRes>.Fail("Số điện thoại phải từ 10 đến 11 chữ số.");
                }

                var phoneExists = await _context.Employees
                    .AnyAsync(e => e.Phone == request.Phone && e.Id != id);
                if (phoneExists)
                {
                    return ApiResponse<EmployeeRes>.Fail("Số điện thoại đã tồn tại.");
                }

                var emailExists = await _context.Employees
                    .AnyAsync(e => e.Email == request.Email && e.Id != id);
                if (emailExists)
                {
                    return ApiResponse<EmployeeRes>.Fail("Email đã tồn tại.");
                }


                var exist = await _context.Employees
                    .Include(e => e.DepartmentEmployees)
                    .ThenInclude(de => de.Department)
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (exist == null)
                {
                    return ApiResponse<EmployeeRes>.NotFound();
                }

                exist.Name = request.Name;
                exist.Email = request.Email;
                exist.Phone = request.Phone;
                exist.Sex = GetEnumSex(request.SexReq);
                exist.Avatar = request.Avatar;

                if (request.DepartmentIds != null)
                {
                    _context.DepartmentEmployee.RemoveRange(exist.DepartmentEmployees);

                    var newDepEmpList = request.DepartmentIds.Select(depId => new DepartmentEmployee
                    {
                        EmployeeId = exist.Id,
                        DepartmentId = depId

                    }).ToList();

                    await _context.DepartmentEmployee.AddRangeAsync(newDepEmpList);
                }

                await _context.SaveChangesAsync();

                var updatedEmployee = await _context.Employees
                    .Include(e => e.DepartmentEmployees)
                    .ThenInclude(de => de.Department)
                    .FirstOrDefaultAsync(e => e.Id == id);

                var response = _mapper.Map<EmployeeRes>(updatedEmployee);

                response.Avatar = updatedEmployee.Avatar;
                response.Sex = updatedEmployee.Sex.ToString();

                return ApiResponse<EmployeeRes>.Success(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<EmployeeRes>.Fail("Lỗi khi cập nhật nhân viên: " + ex.Message);
            }
        }

        public async Task<ApiResponse<EmployeeRes>> GetById(int id)
        {
            try
            {
                var exist = await _context.Employees
                    .Include(e => e.DepartmentEmployees)
                    .ThenInclude(de => de.Department)
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (exist == null)
                {
                    return ApiResponse<EmployeeRes>.NotFound();
                }

                var response = _mapper.Map<EmployeeRes>(exist);

                response.Avatar = exist.Avatar;
                response.Sex = exist.Sex.ToString();

                return ApiResponse<EmployeeRes>.Success(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<EmployeeRes>.Fail("Lỗi khi lấy nhân viên: " + ex.Message);
            }
        }


        private static Sex GetEnumSex(string sex)
        {
            return sex.ToLower() switch
            {
                "nam" => Sex.Male,
                "nữ" => Sex.Female,
                _ => Sex.Other
            };
        }


        private static string GenerateNextCode(string lastCode)
        {
            var regex = new Regex(@"\d+");
            var match = regex.Match(lastCode);

            if (!match.Success)
            {
                return "00001-Emp";
            }

            var numberPart = match.Value;
            var prefix = lastCode.Substring(0, match.Index);
            var suffix = lastCode.Substring(match.Index + match.Length);

            if (int.TryParse(numberPart, out int number))
            {
                int nextNumber = number + 1;
                string nextNumberStr = nextNumber.ToString(new string('0', numberPart.Length));
                return $"{prefix}{nextNumberStr}{suffix}";
            }

            return "00001-Emp";
        }

        
    }
}
