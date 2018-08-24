using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Entity.PageSearch;
using Hmj.Entity.SearchEntity;
using System.Collections.Generic;

namespace Hmj.Interface
{
    public interface IEmployeeService
    {

        PagedList<EMPLOYEE> QueryEmpList(EmpInfoSearch search, PageView view);

        List<EMPLOYEE_EX> QueryEmpList(int deptId);

        int SaveEmployee(string accessToken, EMPLOYEE entity, DeptInfo dept, ref string errMsg);

        EMPLOYEE_EX GetEmp(int empId);

        int DeleteEmployee(string accessToken, int empId);

        List<EMPLOYEE> QueryAllEmpWithNoQrCode(string access_token);

        int ModifyEmpDept(string accessToken, int empId, int deptId, ref string errMsg);

        int UpdateEmpStatus(string userId, byte status);
    }
}
