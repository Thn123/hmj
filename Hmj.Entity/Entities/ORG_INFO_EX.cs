namespace Hmj.Entity.Entities
{
    public class ORG_INFO_EX : ORG_INFO
    {
        /// <summary>
        /// 公司管理员登陆账户ID
        /// </summary>
        public int USER_ID
        {
            get;
            set;
        }

        /// <summary>
        /// 公司管理员登陆账户名
        /// </summary>
        public string USER_NO
        {
            get;
            set;
        }

        /// <summary>
        /// 公司管理员登陆账户密码
        /// </summary>
        public string USER_PASS
        {
            get;
            set;
        }
        /// <summary>
        /// 公司管理员编号
        /// </summary>
        public int EMPLOYEEID
        {
            get;
            set;
        }

        /// <summary>
        /// 公司管理员角色ID
        /// </summary>
        public int ROLE_ID
        {
            get;
            set;
        }

        /// <summary>
        /// 门店数
        /// </summary>
        public int STORE_COUNT
        {
            get;
            set;
        }

        /// <summary>
        /// 公司管理员权限菜单编号
        /// </summary>
        public string MENU_IDS
        {
            get;
            set;
        }

        /// <summary>
        /// 父级
        /// </summary>
        public string PARENT_NAME
        {
            get;
            set;
        }

        /// <summary>
        /// 等级名称
        /// </summary>
        public string ORG_LEVEL_STR
        {
            get;
            set;
        }
        public string FILE_URL
        {
            get;
            set;
        }
        /// <summary>
        /// 报表开始小时
        /// </summary>
        public string HOURS_BEGIN
        {
            get;
            set;
        }
        /// <summary>
        /// 报表结束小时
        /// </summary>
        public string HOURS_END
        {
            get;
            set;
        }
    }
}
