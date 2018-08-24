using Hmj.Common;
using Hmj.Common.Utils;
using Hmj.ExtendAPI.WeiXin.Models;
using System.Collections.Generic;

namespace Hmj.ExtendAPI.WeiXin
{
    public class WXQYClientServiceApi : ServiceApiBase
    {
        private static string WXCorpID = AppConfig.Get("WXCorpID");
        private static string WXCorpSecret = AppConfig.Get("WXCorpSecret");

        protected override string ApiUrl
        {
            get
            {
                return AppConfig.Get("WXCorpApiUrl");
            }
        }

        private static WXQYClientServiceApi _client;
        private static object _lockobject = new object();

        private WXQYClientServiceApi()
        {

        }

        /// <summary>
        /// 单例对象
        /// </summary>
        /// <returns></returns>
        public static WXQYClientServiceApi Create()
        {
            if (_client == null)
            {
                lock (_lockobject)
                {
                    if (_client == null)
                    {
                        _client = new WXQYClientServiceApi();
                    }
                }
            }
            return _client;
        }

        public AccessTokenResponse GetAccessToken()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("corpid", WXCorpID);
            data.Add("corpsecret", WXCorpSecret);

            string result = this.DoFormRequest("cgi-bin/gettoken", data, "GET");
            if (!string.IsNullOrEmpty(result))
            {
                return JsonHelper.DeserializeObject<AccessTokenResponse>(result);
            }

            return null;
        }

        public DeptListResponse QueryDept(string accessToken, int id)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("id", id);

            string result = this.DoFormRequest("cgi-bin/department/list?access_token=" + accessToken, data, "GET");
            if (!string.IsNullOrEmpty(result))
            {
                return JsonHelper.DeserializeObject<DeptListResponse>(result);
            }

            return null;
        }


        public DeptResponse CreateDept(string accessToken, string name, int parentId, int order)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("name", name);
            data.Add("parentid", parentId);
            data.Add("order", order);
            //data.Add("id", id);

            string result = this.DoJSONRequest("cgi-bin/department/create?access_token=" + accessToken, data);
            if (!string.IsNullOrEmpty(result))
            {
                return JsonHelper.DeserializeObject<DeptResponse>(result);
            }

            return null;
        }


        public DeptResponse UpdateDept(string accessToken, int id, string name, int parentId, int order)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("id", id);
            data.Add("name", name);
            data.Add("parentid", parentId);
            data.Add("order", order);

            string result = this.DoJSONRequest("cgi-bin/department/update?access_token=" + accessToken, data);
            if (!string.IsNullOrEmpty(result))
            {
                return JsonHelper.DeserializeObject<DeptResponse>(result);
            }

            return null;
        }

        public DeptResponse DeleteDept(string accessToken, int id)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("id", id);

            string result = this.DoFormRequest("cgi-bin/department/delete?access_token=" + accessToken, data, "GET");
            if (!string.IsNullOrEmpty(result))
            {
                return JsonHelper.DeserializeObject<DeptResponse>(result);
            }

            return null;
        }

        public WeiXinResponseBase CreateUser(string accessToken,
            string userid, string name, List<int> department,
            string position, string mobile,string gender,
            string email,string weixinId)
        {

            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("userid", userid);
            data.Add("name", name);
            data.Add("department", department);
            data.Add("position", position);
            data.Add("mobile", mobile);
            data.Add("gender", gender);
            data.Add("email", email);
            data.Add("weixinid", weixinId);
            //data.Add("avatar_mediaid", null);
            //data.Add("extattr", null);

            string result = this.DoJSONRequest("cgi-bin/user/create?access_token=" + accessToken, data);
            if (!string.IsNullOrEmpty(result))
            {
                return JsonHelper.DeserializeObject<WeiXinResponseBase>(result);
            }

            return null;
        }


        public WeiXinResponseBase UpdateUser(string accessToken,
            string userid, string name, List<int> department,
            string position, string mobile, string gender,
            string email, string weixinId)
        {

            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("userid", userid);
            data.Add("name", name);
            data.Add("department", department);
            data.Add("position", position);
            data.Add("mobile", mobile);
            data.Add("gender", gender);
            data.Add("email", email);
            data.Add("weixinid", weixinId);
            //data.Add("avatar_mediaid", null);
            //data.Add("extattr", null);

            string result = this.DoJSONRequest("cgi-bin/user/update?access_token=" + accessToken, data);
            if (!string.IsNullOrEmpty(result))
            {
                return JsonHelper.DeserializeObject<WeiXinResponseBase>(result);
            }

            return null;
        }
        public WeiXinResponseBase DeleteUser(string accessToken, string userId)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("userid", userId);

            string result = this.DoFormRequest("cgi-bin/user/delete?access_token=" + accessToken, data, "GET");
            if (!string.IsNullOrEmpty(result))
            {
                return JsonHelper.DeserializeObject<WeiXinResponseBase>(result);
            }

            return null;
        }
        public UserListResponse QueryUser(string accessToken, int deptId)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("department_id", deptId);
            data.Add("fetch_child", 1);
            data.Add("status", 0);

            string result = this.DoFormRequest("cgi-bin/user/list?access_token=" + accessToken, data, "GET");
            if (!string.IsNullOrEmpty(result))
            {
                return JsonHelper.DeserializeObject<UserListResponse>(result);
            }

            return null;
        }
        public UserInfo GetUser(string accessToken, string userId)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("userid", userId);

            string result = this.DoFormRequest("cgi-bin/user/get?access_token=" + accessToken, data, "GET");
            if (!string.IsNullOrEmpty(result))
            {
                return JsonHelper.DeserializeObject<UserInfo>(result);
            }

            return null;
        }
    }
}
