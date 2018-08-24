namespace Hmj.Entity.Entities
{
    /// <summary>
    /// 角色权限菜单
    /// </summary>
    public class SYS_RIGHT_EX : SYS_RIGHT
    {
        /// <summary>
        /// 角色菜单
        /// </summary>
        private int _ROLE_RIGHT_ID;
        public int ROLE_RIGHT_ID
        {
            get { return _ROLE_RIGHT_ID; }
            set { _ROLE_RIGHT_ID = value; OnPropertyChanged("ROLE_RIGHT_ID"); }
        }

        private string _PARENT_NAME;
        /// <summary>
        /// 父级菜单名称
        /// </summary>
        public string PARENT_NAME
        {
            get { return _PARENT_NAME; }
            set { _PARENT_NAME = value; }

        }
    }
}
