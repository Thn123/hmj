namespace Hmj.Entity.SearchEntity
{
    public class EmployeeSearch
    {
        public string EMP_NAME { get; set; }//员工姓名
        public string STATUS { get; set; }//状态
        public string EMP_TYPE { get; set; }//类型 
        public int POST_ID { get; set; }//员工职位
        public int ORG_ID { get; set; }//所属公司
        public int STORE_ID { get; set; }//所属门店
        //public string CuUserType { get; set; }
        //public int CuUserStore { get; set; }
        public string UserType { get; set; }
        public int CuEmpId{ get; set; }
        public string EMPLOYEE_NO { get; set; }
        
    }
}
