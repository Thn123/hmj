using System.Collections.Generic;

namespace Hmj.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId">用户标识</param>
        /// <returns>用户信息</returns>
        IUser GetUserInfo(string userId);

        /// <summary>
        /// 获取用户的权限列表
        /// </summary>
        /// <param name="userId">用户标识</param>
        /// <returns>
        /// 权限列表
        /// </returns>
        List<IPrivilege> GetUserPrivilegeList(string userId);
       
    }
}
