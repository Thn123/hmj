using System;

namespace Hmj.Entity.Entities
{
    public class EMPLOYEE_EX : EMPLOYEE
    {
        public int FansNum { get; set; }
        public int DeptID { get; set; }
        public string DeptCode { get; set; }
        public string DeptName { get; set; }
        public byte DeptType { get; set; }
        public String FullDeptName { get; set; }
    }
}
