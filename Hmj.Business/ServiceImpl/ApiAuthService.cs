using Hmj.Common;
using Hmj.Interface;
using System.Linq;

namespace Hmj.Business.ServiceImpl
{
    public class ApiAuthService : IApiAuthService
    {

        public bool Auth(string appID, string timestamp, string sign, out string errorMsg)
        {
            if (string.IsNullOrEmpty(appID))
            {
                errorMsg = "缺少认证参数[appid]";
                return false;
            }
            if (string.IsNullOrEmpty(timestamp))
            {
                errorMsg = "缺少认证参数[timestamp]";
                return false;
            }
            if (string.IsNullOrEmpty(sign))
            {
                errorMsg = "缺少认证参数[sign]";
                return false;
            }
            AppUserInfo appUserInfo = AppUserManager.Create().Get(appID);
            if (appUserInfo == null)
            {
                errorMsg = "无效的[appid]";
                return false;
            }
            string input = string.Format("appid={0}&secretkey={1}&timestamp={2}", appID, appUserInfo.AppSecret, timestamp);
            string checksign = CryptographyManager.Md5Encrypt(input);
            if (string.Compare(sign, checksign, true) != 0)
            {
                errorMsg = "认证方式错误";
                return false;
            }

            errorMsg = "";
            return true;
        }

        public bool Auth(string appID, string timestamp, string sign,string ip, out string errorMsg)
        {
            bool isValidIP = this.IsValidIP(ip);//验证是否是白名单里的IP
            if (isValidIP == false)
            {
                if (string.IsNullOrEmpty(appID))
                {
                    errorMsg = "缺少认证参数[appid]";
                    return false;
                }
                if (string.IsNullOrEmpty(timestamp))
                {
                    errorMsg = "缺少认证参数[timestamp]";
                    return false;
                }
                if (string.IsNullOrEmpty(sign))
                {
                    errorMsg = "缺少认证参数[sign]";
                    return false;
                }
                AppUserInfo appUserInfo = AppUserManager.Create().Get(appID);
                if (appUserInfo == null)
                {
                    errorMsg = "无效的[appid]";
                    return false;
                }
                string input = string.Format("appid={0}&secretkey={1}&timestamp={2}", appID, appUserInfo.AppSecret, timestamp);

                string checksign = CryptographyManager.MD5(input);
                if (string.Compare(sign, checksign, true) != 0)
                {
                    errorMsg = "认证方式错误";
                    return false;
                }
            }
                errorMsg = "";
            return true;
        }
        private bool IsValidIP(string ip)
        {
            var ipList = new string[3] { "199.12.36.69", "", "" };
            var info = ipList.FirstOrDefault(m => m == ip);
            return info != null;
        }
    }
}
