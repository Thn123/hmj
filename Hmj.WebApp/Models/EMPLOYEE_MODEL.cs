using Hmj.Entity;

namespace Hmj.WebApp.Models
{
    public class EMPLOYEE_MODEL : EMPLOYEE
    {
        public int EmpGroupId { get; set; }
        public int EmpGroupType { get; set; }
    }
}