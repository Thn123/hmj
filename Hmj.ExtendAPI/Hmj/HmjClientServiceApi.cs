using Hmj.Common;
using System.Collections.Generic;

namespace Hmj.ExtendAPI.CenSH
{
    public class HmjClientServiceApi : ServiceApiBase
    {
        protected override string ApiUrl
        {
            get { return AppConfig.Get("WebUrl"); }
        }
        private static HmjClientServiceApi _client;
        private static object _lockobject = new object();

        private HmjClientServiceApi()
        {

        }

        /// <summary>
        /// 单例对象
        /// </summary>
        /// <returns></returns>
        public static HmjClientServiceApi Create()
        {
            if (_client == null)
            {
                lock (_lockobject)
                {
                    if (_client == null)
                    {
                        _client = new HmjClientServiceApi();
                    }
                }
            }
            return _client;
        }



        public string GetAccessToken()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();

            string result = this.DoFormRequest("api/getQYToken.do", data, "GET");

            return result;
        }
    }
}
