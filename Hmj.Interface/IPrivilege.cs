namespace Hmj.Interface
{
    public interface IPrivilege
    {
        /// <summary>
        /// 权限标识
        /// </summary>
        string PrivilegeCode
        {
            get;
            set;
        }

        /// <summary>
        /// 权限名称
        /// </summary>
        string PrivilegeName
        {
            get;
            set;
        }

        /// <summary>
        /// 权限类型： 0 菜单权限  1 一般权限
        /// </summary>
        int PrivilegeType
        {
            get;
            set;
        }

        /// <summary>
        /// 上级标识
        /// </summary>
        string ParentCode
        {
            get;
            set;
        }

        /// <summary>
        /// 资源
        /// </summary>
        string Resource
        {
            get;
            set;
        }
    }
}
