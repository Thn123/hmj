using Hmj.Common;
using Hmj.DataAccess.Repository;
using Hmj.Interface;
using System.Collections.Generic;

namespace Hmj.Business.ServiceImpl
{
    public class UserService:IUserService
    {
         UserRepository _urepo;
         public UserService()
         {
             _urepo = new UserRepository();
         }
        public IUser GetUserInfo(string userId)
        {           
            return new IdentityUser()
            {
                UserId = "TEST_USER",
                FullName = "测试用户",
                EmployID = "1001001",
                StoreId = 1001,
                OrgId =1 
            };
            //VieEmployee user = _urepo.GetEmployeeById(userId);
            //if (user != null)
            //{
            //    return new IdentityUser()
            //    {
            //        UserId = user.EmployeeID,
            //        FullName =user.EmployeeName,
            //        EmployID = user.EmployeeID,
            //        SubBranchId = user.SubbranchID
            //    };
            //}
            //else
            //{
            //    return null;
            //}
           
        }

        public List<IPrivilege> GetUserPrivilegeList(string userId)
        {
            return null;
        }
    }
}
