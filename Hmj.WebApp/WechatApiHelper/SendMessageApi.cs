using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Hmj.WebApp.WechatApiHelper
{
    public class SendMessageApi : BasePage
    {
        public Result SendMessage(string toUserName, string content)
        {
            var json = GetTextMessageJson(toUserName, content);
            var accessToken = base.Token();
            var postUrl = RequestUrlHelper.Message.Send(accessToken);
            return JsonConvert.DeserializeObject<Result>(base.HttpXmlPostRequest(postUrl, json, Encoding.UTF8));
        }

        public string GetTextMessageJson(string toUserName, string content)
        {
            var message = new Wechat_TextMessage();
            message.touser = toUserName;
            message.text.content = content;
            return Newtonsoft.Json.JsonConvert.SerializeObject(message);
        }

        #region wechat obj

        public class Wechat_Message
        {
            public string touser;
        }

        public class Wechat_TextMessage : Wechat_Message
        {
            public string msgtype = "text";
            public Wechat_TextMessage_text text = new Wechat_TextMessage_text();
        }

        public class Wechat_TextMessage_text
        {
            public string content;
        }

        public class Result
        {
            public int errcode;
            public string errmsg;
        }

        #endregion
    }
}