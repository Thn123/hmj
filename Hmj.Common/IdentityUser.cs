using Hmj.Interface;

namespace Hmj.Common
{
    public class IdentityUser:IUser
    {
        public string UserId { get; set; }
        /// <summary>
        /// 工号
        /// </summary>
        /// <value>The user ID.</value>
        public string EmployID { get; set; }
        public string FullName { get; set; }

        public int UserLevel { get; set; }
        /// <summary>
        /// 所属组织分组Code
        /// </summary>
        /// <value>The org code.</value>
        public int OrgId { get; set; }
        /// <summary>
        /// 所属组织分组名称
        /// </summary>
        /// <value>The name of the org.</value>
        public string OrgName { get; set; }

       public int StoreId { get; set; }
    }
}
