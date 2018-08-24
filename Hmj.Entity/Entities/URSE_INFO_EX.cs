namespace Hmj.Entity.Entities
{
    public class USER_INFO_EX : USER_INFO
    {
        public string EMPLOYEE_NAME
        {
            get;
            set;
        }

        /// <summary>
        /// 用户角色：0公司管理员，1标准角色，2自定义角色
        /// </summary>
        public int? ROLE_TYPE
        {
            get;
            set;
        }
        /// <summary>
        /// 用户所在公司CODE
        /// </summary>
        public string ORG_NO
        {
            get;
            set;
        } 
        /// <summary>
        /// 用户所在公司名称
        /// </summary>
        public string ORG_NAME
        {
            get;
            set;
        } 

        /// <summary>
        /// 用户所在集团\区域\门店名称
        /// </summary>
        public string STORE_NAME
        {
            get;
            set;
        }

        /// <summary>
        /// 用户类型
        /// </summary>
        public string USER_TYPE_STR 
        {
            get;
            set;
        }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string ROLE_NAME
        {
            get;
            set;
        }


        /// <summary>
        /// 数据权限
        /// </summary>
        public string USER_STORES_OBJ
        {
            get;
            set;
        }

        //卡级类型
        public int CARD_MODE
        {
            get;
            set;
        }

        //金额四舍五入类型
        public int POINT_MODE
        {
            get;
            set;
        }

        public string EMPLOYEE_NO { get; set; }
         
    }
}
