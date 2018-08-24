using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hmj.WebApp.WechatApiHelper
{
    public static class RequestUrlHelper
    {
        //自定义菜单
        public static class CustomMenu
        {
            public static string Create(string accessToken)
            {
                return string.Format("https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}", accessToken);
            }
        }

        public static class Message
        {
            public static string Send(string accessToken)
            {
                return string.Format("https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}", accessToken);
            }
        }
    }
}