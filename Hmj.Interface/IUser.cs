namespace Hmj.Interface
{
    public interface IUser
    {
        string UserId { get; }
        string FullName { get; }

        string EmployID { get; }
        //0 全公司  1 分公司  2 本店
        int UserLevel { get; }
        int StoreId { get; set; }
        int OrgId { get; set; }

        string OrgName { get; set; }


        ///// <summary>
        ///// 登陆账户编号
        ///// </summary>
        //int USER_ID { get; }
        ///// <summary>
        ///// 登陆账户名称
        ///// </summary>
        //string USER_NO { get; }
        ///// <summary>
        ///// 员工编号
        ///// </summary>
        //string EMPLOYEE_ID { get; }
        ///// <summary>
        ///// 员工名称
        ///// </summary>
        //string EMPLOYEE_NAME { get; }
        ///// <summary>
        ///// 门店编号
        ///// </summary>
        //int STORE_ID { get; set; }
        ///// <summary>
        ///// 门店名称
        ///// </summary>
        //string STORE_NAME { get; set; }
        ///// <summary>
        ///// 公司编号
        ///// </summary>
        //int ORG_ID { get; set; }
        ///// <summary>
        ///// 公司名称
        ///// </summary>
        //string ORG_NAME { get; set; }
        ///// <summary>
        /////  用户角色：0公司管理员，1标准角色，2自定义角色
        ///// </summary>
        //int? ROLE_TYPE { get; }
    }
}
