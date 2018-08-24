using Hmj.Common;
using Hmj.Common.Utils;
using Hmj.ExtendAPI.WeiXin.Models;
using System.Collections.Generic;

namespace Hmj.ExtendAPI.WeiXin
{
    public class WXMPClientServiceApi : ServiceApiBase
    {
        //private static string WXAppID = AppConfig.Get("WXAppID");
        //private static string WXAppSecret = AppConfig.Get("WXAppSecret");

        protected override string ApiUrl
        {
            get
            {
                return AppConfig.Get("WXMPApiUrl");
            }
        }

        private static WXMPClientServiceApi _client;
        private static object _lockobject = new object();

        private WXMPClientServiceApi()
        {

        }

        /// <summary>
        /// 单例对象
        /// </summary>
        /// <returns></returns>
        public static WXMPClientServiceApi Create()
        {
            if (_client == null)
            {
                lock (_lockobject)
                {
                    if (_client == null)
                    {
                        _client = new WXMPClientServiceApi();
                    }
                }
            }
            return _client;
        }

        //public AccessTokenResponse GetOAuth2AccessToken(string code)
        //{
        //    Dictionary<string, object> data = new Dictionary<string, object>();
        //    data.Add("appid", WXAppID);
        //    data.Add("secret", WXAppSecret);
        //    data.Add("code", code);
        //    data.Add("grant_type", "authorization_code");

        //    string result = this.DoFormRequest("sns/oauth2/access_token", data, "GET");
        //    if (!string.IsNullOrEmpty(result))
        //    {
        //        return JsonHelper.DeserializeObject<AccessTokenResponse>(result);
        //    }

        //    return null;
        //}


        public QRCodeResponse CreateQRCode(string access_token, int paramId)
        {
            //{"action_name": "QR_LIMIT_SCENE", "action_info": {"scene": {"scene_id": 123}}}
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("action_name", "QR_LIMIT_SCENE");
            var scene = new { scene_id = paramId };
            data.Add("action_info", new { scene = scene });

            string result = this.DoJSONRequest("cgi-bin/qrcode/create?access_token=" + access_token, data, "POST");
            if (!string.IsNullOrEmpty(result))
            {
                return JsonHelper.DeserializeObject<QRCodeResponse>(result);
            }

            return null;
        }

        public QRCodeResponse CreateTempQRCode(string access_token, int paramId,string expire)
        {
            //{"action_name": "QR_LIMIT_SCENE", "action_info": {"scene": {"scene_id": 123}}}
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("expire_seconds", expire);
            data.Add("action_name", "QR_SCENE");
            var scene = new { scene_id = paramId };
            data.Add("action_info", new { scene = scene });

            string result = this.DoJSONRequest("cgi-bin/qrcode/create?access_token=" + access_token, data, "POST");
            if (!string.IsNullOrEmpty(result))
            {
                return JsonHelper.DeserializeObject<QRCodeResponse>(result);
            }

            return null;
        }

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string SendTemplateMsg(string access_token, string data)
        {
            string result = this.DoJSONRequest("cgi-bin/message/template/send?access_token=" + access_token, data, "POST");
   
            return result;

        }
    }
}
