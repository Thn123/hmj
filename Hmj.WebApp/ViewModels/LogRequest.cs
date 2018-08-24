using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hmj.WebApp.ViewModels
{
    public class LogRequest
    {
        public string Title
        {
            get;
            set;
        }

        //日志类型 Exception异常，Message消息
        public string MsgType
        {
            get;
            set;
        }
        
        public string MsgContent
        {
            get;
            set;
        }
    }
}