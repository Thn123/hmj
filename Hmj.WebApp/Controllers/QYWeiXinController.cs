using FaceTouch.WelfareGarden.ExtendAPI.WeiXin;
using Hmj.Common.Utils;
using Hmj.ExtendAPI.WeiXin.Lib;
using Hmj.ExtendAPI.WeiXin.Models;
using Hmj.Extension;
using Hmj.Interface;
using StructureMap.Attributes;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Hmj.WebApp.Controllers
{
    public class QYWeiXinController : BaseController
    {
        [SetterProperty]
        public ILogService LogService { get; set; }
        [SetterProperty]
        public IEmployeeService EmployeeService { get; set; }
        //
        // GET: /QYWeiXin/
        [HttpGet]
        public ActionResult Index(string msg_signature, string timestamp, string nonce, string echostr)
        {
            string sReqData = StreamHelper.Read(Request.InputStream);
            LogService.Warn("GET解密前的数据: " + sReqData);

            WXBizMsgCrypt wxcpt = new WXBizMsgCrypt();
            string sEchoStr = "";
            int ret = wxcpt.VerifyURL(msg_signature, timestamp, nonce, echostr, ref sEchoStr);
            if (ret != 0)
            {
                LogService.Fatal("ERR: VerifyURL fail, ret: " + ret);
            }

            return Content(sEchoStr);
        }

        [HttpPost]
        public ActionResult Index(string msg_signature, string timestamp, string nonce)
        {
            string sReqData = StreamHelper.Read(Request.InputStream);

            WXBizMsgCrypt wxcpt = new WXBizMsgCrypt();
            string sMsg = "";

            int ret = wxcpt.DecryptMsg(msg_signature, timestamp, nonce, sReqData, ref sMsg);
            if (ret != 0)
            {
                LogService.Fatal("ERR: VerifyURL fail, ret: " + ret);
            }
            //LogService.Fatal("解密后的数据: " + sMsg);

            XDocument doc = XmlHelper.LoadXML(sMsg);
            RequestMsgType msgType = MsgTypeHelper.GetRequestMsgType(doc);
            switch (msgType)
            {
                case RequestMsgType.Event:
                    RequestEventMessage message = (RequestEventMessage)XmlHelper.Deserialize<RequestEventMessage>(sMsg);
                    //成员关注、取消关注企业号的事件
                    if (message != null && message.Event == "subscribe" || message.Event == "unsubscribe")
                    {
                        var userId = message.FromUserName;

                        //关注状态: 1=已关注，2=已冻结，4=未关注
                        int status = message.Event == "subscribe" ? 1 : 4;
                        int rows = EmployeeService.UpdateEmpStatus(userId, (byte)status);

                        //LogService.Fatal("修改结果: " + rows);
                    }
                    break;

            }

            return Content(sMsg);
        }

        public ActionResult Test()
        {
            string sMsg = @"<xml><ToUserName><![CDATA[wx42c6025ff9bb2576]]></ToUserName>
<FromUserName><![CDATA[15021311065]]></FromUserName>
<CreateTime>1478775431</CreateTime>
<MsgType><![CDATA[event]]></MsgType>
<AgentID>0</AgentID>
<Event><![CDATA[unsubscribe]]></Event>
<EventKey><![CDATA[]]></EventKey>
</xml>";
            XDocument doc = XmlHelper.LoadXML(sMsg);
            LogService.Fatal("msgType: " + JsonHelper.SerializeObject(doc));
            RequestMsgType msgType = MsgTypeHelper.GetRequestMsgType(doc);
            switch (msgType)
            {
                case RequestMsgType.Event:
                    RequestEventMessage message = (RequestEventMessage)XmlHelper.Deserialize<RequestEventMessage>(sMsg);
                    //成员关注、取消关注企业号的事件
                    if (message.Event == "subscribe" || message.Event == "unsubscribe")
                    {
                        var userId = message.FromUserName;

                        //关注状态: 1=已关注，2=已冻结，4=未关注
                        int status = message.Event == "subscribe" ? 1 : 4;
                        int rows = EmployeeService.UpdateEmpStatus(userId, (byte)status);

                        LogService.Fatal("修改结果: " + rows);
                    }
                    break;

            }
            return Content(sMsg);
        }

    }
}
