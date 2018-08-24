using Hmj.Entity;
using Hmj.Entity.Entities;

namespace Hmj.Interface
{
    public interface ILoginService
    {
        USER_INFO_EX GetUserById(int id);
        USER_INFO_EX WXLoginUser(string loginName, string pwd,string orgNo);
        USER_INFO_EX WXGetUserById(int id);
        ORG_INFO GetMerchants(int id);
        ORG_INFO GetMerchantsBy(string id);
        
        ORG_INFO GetMerchants(string ToUserName);
        int UpdateMerchants(ORG_INFO m);
        USER_INFO_EX LoginSYSUser(string loginName, string pwd);
    }
}
