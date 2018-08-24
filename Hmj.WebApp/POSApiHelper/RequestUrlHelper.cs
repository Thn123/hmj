using System.Configuration;

namespace Hmj.WebApp.POSApiHelper
{
    public static class RequestUrlHelper
    {
        //门店
        public static class Store
        {
            public static string List(int orgID)
            {
                return string.Format("{0}/wechat/resource/store/list.do/{1}", ConfigurationManager.AppSettings["POS_WebApi"], orgID.ToString());
            }
        }
    }
}