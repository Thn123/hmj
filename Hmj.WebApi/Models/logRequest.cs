using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hmj.WebApi.Models
{
    public class logRequest
    {
        [JsonProperty("title")]
        public string Title
        {
            get;
            set;
        }

        //日志类型 Exception异常，Message消息
        [JsonProperty("msgType")]
        public string MsgType
        {
            get;
            set;
        }

        [JsonProperty("msgContent")]
        public string MsgContent
        {
            get;
            set;
        }

    }
}