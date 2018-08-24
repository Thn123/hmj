using Hmj.Common;
using Hmj.DataAccess.Repository;
using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Interface;

namespace Hmj.Business.ServiceImpl
{
    public class LoginService : ILoginService
    {
        LoginReponsitory lReponsitory;
        public LoginService()
        {
            lReponsitory = new LoginReponsitory();
        }
        

        public USER_INFO_EX GetUserById(int id)
        {
            return lReponsitory.GetUserById(id);
        }

        public USER_INFO_EX LoginSYSUser(string loginName, string pwd)
        {
            return lReponsitory.LoginSYSUser(loginName, pwd);
        }

        public USER_INFO_EX WXLoginUser(string loginName, string pwd, string orgNo)
        {
            loginName = Utility.ClearSafeStringParma(loginName);
            pwd = Utility.ClearSafeStringParma(pwd);
            orgNo = Utility.ClearSafeStringParma(orgNo);
            return lReponsitory.WXLoginUser(loginName, pwd,orgNo);
        }

        public USER_INFO_EX WXGetUserById(int id)
        {
            return lReponsitory.WXGetUserById(id);
        }

        /// <summary>
        /// 获取商户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ORG_INFO GetMerchants(int id)
        {
            return lReponsitory.GetMerchants(id);
        }

        /// <summary>
        /// 获取商户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ORG_INFO GetMerchantsBy(string id)
        {
            return lReponsitory.GetMerchantsBy(id);
        }

        /// <summary>
        /// 修改商户信息
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public int UpdateMerchants(ORG_INFO m)
        {
            return lReponsitory.UpdateMerchants(m);
        }

        /// <summary>
        /// 获取商户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ORG_INFO GetMerchants(string ToUserName)
        {
            return lReponsitory.GetMerchants(ToUserName);
        }
    }
}
