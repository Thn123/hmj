using Hmj.Business;
using Hmj.Business.ServiceImpl;
using Hmj.Business.WXService;
using Hmj.Common;
using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Interface;
using Hmj.Interface.WXService;
using HmjNew.Service;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Xml;

namespace Hmj.WebApp
{
    public partial class Default : WeiPage
    {

        ISystemService sbo = new SystemService();
        ICustMemberService _custMember = new CustMemberService();
        ILogService _log = new LogService();
        private static ILog _logwarn = LogManager.GetLogger("logwarn");
        ICommonService _mservice = new CommonService();

        //const string Token = "fveSHTUB";		//与微信平台那边填写的token一致
        protected void Page_Load(object sender, EventArgs e)
        {
            string postStr = "";
            if (Request.HttpMethod.ToLower() == "post")
            {
                // 微信加密签名    
                String signature = Request["signature"];
                // 时间戳    
                String timestamp = Request["timestamp"];
                // 随机数    
                String nonce = Request["nonce"];

                Stream s = System.Web.HttpContext.Current.Request.InputStream;
                byte[] b = new byte[s.Length];
                s.Read(b, 0, (int)s.Length);
                postStr = Encoding.UTF8.GetString(b);

                if (!string.IsNullOrEmpty(postStr))
                {
                    _logwarn.Warn(postStr);
                    //封装请求类
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(postStr);
                    XmlElement rootElement = doc.DocumentElement;

                    XmlNode MsgType = rootElement.SelectSingleNode("MsgType");

                    WXCUST_MSG_HIS requestXML = new WXCUST_MSG_HIS();
                    requestXML.OLDXML = postStr;
                    requestXML.OriginalXml = "";

                    requestXML.TOUSERNAME = rootElement.SelectSingleNode("ToUserName").InnerText;
                    requestXML.FROMUSERNAME = rootElement.SelectSingleNode("FromUserName").InnerText;
                    requestXML.CREATE_DATE = UnixTimeToTime(rootElement.SelectSingleNode("CreateTime").InnerText);
                    requestXML.CONTENT = "";
                    requestXML.EVENTKEY = "";
                    requestXML.LABEL = "";
                    requestXML.LOCATION_X = "";
                    requestXML.LOCATION_Y = "";
                    requestXML.MSGID = "";
                    requestXML.PICURL = "";
                    requestXML.RESXML = "";
                    requestXML.SCALE = "";
                    requestXML.WXEVENT = "";
                    requestXML.VEDIOURL = "";
                    requestXML.STATUS = "0";

                    requestXML.MSGTYPE = MsgType.InnerText;

                    if (requestXML.MSGTYPE == "text")
                    {
                        requestXML.CONTENT = rootElement.SelectSingleNode("Content").InnerText;
                        requestXML.MSGID = rootElement.SelectSingleNode("MsgId").InnerText;
                    }
                    else if (requestXML.MSGTYPE == "location")
                    {
                        requestXML.LOCATION_X = rootElement.SelectSingleNode("Location_X").InnerText;
                        requestXML.LOCATION_Y = rootElement.SelectSingleNode("Location_Y").InnerText;
                        requestXML.SCALE = rootElement.SelectSingleNode("Scale").InnerText;
                        requestXML.LABEL = rootElement.SelectSingleNode("Label").InnerText;
                    }
                    else if (requestXML.MSGTYPE == "image")
                    {
                        requestXML.PICURL = rootElement.SelectSingleNode("PicUrl").InnerText;
                    }
                    else if (requestXML.MSGTYPE == "event")
                    {
                        requestXML.WXEVENT = rootElement.SelectSingleNode("Event").InnerText.ToLower();
                        if (requestXML.WXEVENT.ToLower() == "location")  //高级接口获取到的地理位置
                        {
                            requestXML.LOCATION_Y = rootElement.SelectSingleNode("Latitude").InnerText;  //纬度
                            requestXML.LOCATION_X = rootElement.SelectSingleNode("Longitude").InnerText;  //经度
                            requestXML.SCALE = rootElement.SelectSingleNode("Precision").InnerText;  //精度
                        }
                        else if (requestXML.WXEVENT.ToLower() == "templatesendjobfinish")
                        {
                            requestXML.STATUS = rootElement.SelectSingleNode("Status").InnerText;

                        }
                        else if (requestXML.WXEVENT.ToLower() == "user_get_card")//领取卡券
                        {
                            requestXML.EVENTKEY = rootElement.SelectSingleNode("CardId").InnerText;
                            requestXML.CONTENT = rootElement.SelectSingleNode("UserCardCode").InnerText;
                        }
                        else if (requestXML.WXEVENT.ToLower() == "user_consume_card")//核销卡券
                        {
                            requestXML.EVENTKEY = rootElement.SelectSingleNode("CardId").InnerText;
                            requestXML.CONTENT = rootElement.SelectSingleNode("UserCardCode").InnerText;
                        }
                        else
                        {
                            var evtKey = rootElement.SelectSingleNode("EventKey");
                            if (evtKey != null)
                            {

                                requestXML.EVENTKEY = evtKey.InnerText;
                            }
                        }
                    }
                    else
                    {

                    }
                    WriteTxt("----------粉丝发送过来的消息，消息类型：" + requestXML.MSGTYPE + "----------：" + postStr);
                    //回复消息


                    ResponseMsg(requestXML, postStr, signature, timestamp, nonce);
                }
            }
            else
            {
                // WriteTxt("异常：");
                Valid();
            }
        }

        /// <summary>
        /// 验证微信签名
        /// </summary>
        /// * 将token、timestamp、nonce三个参数进行字典序排序
        /// * 将三个参数字符串拼接成一个字符串进行sha1加密
        /// * 开发者获得加密后的字符串可与signature对比，标识该请求来源于微信。
        /// <returns></returns>
        private bool CheckSignature()
        {
            string signature = Request.QueryString["signature"];
            string timestamp = Request.QueryString["timestamp"];
            string nonce = Request.QueryString["nonce"];
            string token = AppConfig.Get("WXMPToken");
            string[] ArrTmp = { token, timestamp, nonce };
            Array.Sort(ArrTmp);     //字典排序
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();
            if (tmpStr == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Valid()
        {
            string echoStr = Request.QueryString["echoStr"];
            if (CheckSignature())
            {
                if (!string.IsNullOrEmpty(echoStr))
                {
                    Response.Write(echoStr);
                    Response.End();
                }
            }
        }

        /// <summary>
        /// 回复消息(微信信息返回)
        /// </summary>
        /// <param name="weixinXML"></param>
        private void ResponseMsg(WXCUST_MSG_HIS requestXML, string postStr, string signature, string timestamp, string nonce)
        {
            //\(^o^)/~ h5游戏转发关注粉丝

            //try
            //{
            //    var postUrl = "http://censh.c.yuemeiad.com.cn/api/receive_wechat_message?signature=" + signature + "&timestamp=" + timestamp + "&nonce=" + nonce + "&openid=" + requestXML.FROMUSERNAME;

            //    var d = HttpXmlPostRequest(postUrl, postStr, Encoding.UTF8);
            //    _log.Error("h5_game:" + d + "/n signature+" + signature + "/n nonce" + nonce + "/n requestXML.FROMUSERNAME" + requestXML.FROMUSERNAME);
            //}
            //catch (Exception)
            //{

            //    throw;
            //}
            string resxml = "<xml><ToUserName><![CDATA[" + requestXML.FROMUSERNAME + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.TOUSERNAME + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime>";
            try
            {
                #region 收到文本消息
                if (requestXML.MSGTYPE == "text")
                {
                    int count = 0;
                    List<Information_EX> list = sbo.GetModelList(string.Format("  ((KeyWords like '%{0}%' and MatchingType=0) or (KeyWords ='{0}' and MatchingType=1)) AND replytype={1} and ToUserName='{2}'", requestXML.CONTENT, 1, requestXML.TOUSERNAME));
                    if (requestXML.CONTENT.ToString().Contains("客服") || requestXML.CONTENT.ToString().Contains("人工"))
                    {
                        //_logwarn.Warn(requestXML.CONTENT + "客服进来了1");
                        //SendText(requestXML.FROMUSERNAME,
                        //    "你好，欢迎访问华美家会员专属微信，客服妹妹正在赶来中",
                        //    requestXML.TOUSERNAME);
                        //resxml += "<MsgType><![CDATA[transfer_customer_service]]></MsgType></xml>";
                    }
                    else if (list.Count == 1 || (list.Count > 1 && list[0].MsgType != "news"))
                    {
                        Information_EX f = list[0];// ibo.GetModel(requestXML.EventKey, 4); //自定义菜单回复

                        if (f != null)
                        {

                            switch (f.MsgType)
                            {
                                case "text":
                                    resxml += "<MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + f.Content + "]]></Content><FuncFlag>0</FuncFlag></xml>";
                                    break;
                                case "image":
                                    resxml += "<MsgType><![CDATA[image]]></MsgType><Image><MediaId><![CDATA[" + f.Media_ID + "]]></MediaId></Image></xml>";
                                    break;
                                case "news":
                                    resxml += "<MsgType><![CDATA[news]]></MsgType><ArticleCount>1</ArticleCount><Articles>";
                                    resxml += "<item><Title><![CDATA[" + f.Title + "]]></Title><Description><![CDATA[" + f.Description + "]]></Description><PicUrl><![CDATA[" + ConfigurationSettings.AppSettings["WebUrl"] + f.PicUrl + "]]></PicUrl><Url><![CDATA[" + (f.IsURL ? (f.FulltextUrl.Contains("?id") ? (f.FulltextUrl + "&FromUserName=" + requestXML.FROMUSERNAME + "&ToUserName=" + requestXML.TOUSERNAME + "") : (f.FulltextUrl + "?FromUserName=" + requestXML.FROMUSERNAME + "&ToUserName=" + requestXML.TOUSERNAME + "")) : (ConfigurationSettings.AppSettings["WebUrl"] + "/GraphicDisplay.aspx?id=" + f.DID)) + "]]></Url></item>";//URL是点击之后跳转去那里，这里跳转到百度
                                    resxml += "</Articles><FuncFlag>0</FuncFlag></xml>";
                                    break;
                                case "music":
                                    resxml += string.Format(@"<MsgType><![CDATA[music]]></MsgType>
                                 <Music>
                                 <Title><![CDATA[{0}]]></Title>
                                 <Description><![CDATA[{1}]]></Description>
                                 <MusicUrl><![CDATA[{2}]]></MusicUrl>
                                 <HQMusicUrl><![CDATA[{2}]]></HQMusicUrl>
                                 </Music>
                                 <FuncFlag>0</FuncFlag>
                                 </xml>", f.Title, f.Description, ConfigurationSettings.AppSettings["WebUrl"] + f.PicUrl);
                                    break;
                                default:
                                    //resxml += "<MsgType><![CDATA[text]]></MsgType><Content><![CDATA[感谢您的支持]]></Content><FuncFlag>0</FuncFlag></xml>";
                                    break;
                            }

                        }
                    }
                    else if (list.Count > 1) //多图
                    {
                        count = list.Count;
                        resxml += "<MsgType><![CDATA[news]]></MsgType><ArticleCount>" + count + "</ArticleCount><Articles>";
                        for (int i = 0; i < count; i++)
                        {
                            resxml += "<item><Title><![CDATA[" + list[i].Title + "]]></Title><Description><![CDATA[" + list[i].Description + "]]></Description><PicUrl><![CDATA[" + ConfigurationSettings.AppSettings["WebUrl"] + list[i].PicUrl + "]]></PicUrl><Url><![CDATA[" + (list[i].IsURL ? (list[i].FulltextUrl.Contains("?id") ? (list[i].FulltextUrl + "&FromUserName=" + requestXML.FROMUSERNAME + "&ToUserName=" + requestXML.TOUSERNAME + "") : (list[i].FulltextUrl + "?FromUserName=" + requestXML.FROMUSERNAME + "&ToUserName=" + requestXML.TOUSERNAME + "")) : (ConfigurationSettings.AppSettings["WebUrl"] + "/GraphicDisplay.aspx?id=" + list[i].DID)) + "]]></Url></item>";//URL是点击之后跳转去那里，这里跳转到百度
                        }
                        resxml += "</Articles><FuncFlag>0</FuncFlag></xml>";
                    }
                    else
                    {
                        list = sbo.GetModelList(string.Format("   replytype={0} and ToUserName='{1}'", 3, requestXML.TOUSERNAME));
                        if (list.Count == 1 || (list.Count > 1 && list[0].MsgType != "news"))
                        {
                            Information_EX f = list[0];

                            if (f != null)
                            {

                                switch (f.MsgType)
                                {
                                    case "text":
                                        resxml += "<MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + f.Content + "]]></Content><FuncFlag>0</FuncFlag></xml>";
                                        break;
                                    case "news":
                                        resxml += "<MsgType><![CDATA[news]]></MsgType><ArticleCount>1</ArticleCount><Articles>";
                                        resxml += "<item><Title><![CDATA[" + f.Title + "]]></Title><Description><![CDATA[" + f.Description + "]]></Description><PicUrl><![CDATA[" + ConfigurationSettings.AppSettings["WebUrl"] + f.PicUrl + "]]></PicUrl><Url><![CDATA[" + (f.IsURL ? (f.FulltextUrl.Contains("?id") ? (f.FulltextUrl + "&FromUserName=" + requestXML.FROMUSERNAME + "&ToUserName=" + requestXML.TOUSERNAME + "") : (f.FulltextUrl + "?FromUserName=" + requestXML.FROMUSERNAME + "&ToUserName=" + requestXML.TOUSERNAME + "")) : (ConfigurationSettings.AppSettings["WebUrl"] + "/GraphicDisplay.aspx?id=" + f.DID)) + "]]></Url></item>";//URL是点击之后跳转去那里，这里跳转到百度
                                        resxml += "</Articles><FuncFlag>0</FuncFlag></xml>";
                                        break;
                                    case "music":
                                        resxml += string.Format(@"<MsgType><![CDATA[music]]></MsgType>
                                 <Music>
                                 <Title><![CDATA[{0}]]></Title>
                                 <Description><![CDATA[{1}]]></Description>
                                 <MusicUrl><![CDATA[{2}]]></MusicUrl>
                                 <HQMusicUrl><![CDATA[{2}]]></HQMusicUrl>
                                 </Music>
                                 <FuncFlag>0</FuncFlag>
                                 </xml>", f.Title, f.Description, ConfigurationSettings.AppSettings["WebUrl"] + f.PicUrl);
                                        break;
                                    default:
                                        //resxml += "<MsgType><![CDATA[text]]></MsgType><Content><![CDATA[感谢您的支持]]></Content><FuncFlag>0</FuncFlag></xml>";
                                        break;
                                }

                            }
                        }
                        else if (list.Count > 1) //多图
                        {
                            int count2 = list.Count;
                            resxml += "<MsgType><![CDATA[news]]></MsgType><ArticleCount>" + count + "</ArticleCount><Articles>";
                            for (int z = 0; z < count2; z++)
                            {
                                resxml += "<item><Title><![CDATA[" + list[z].Title + "]]></Title><Description><![CDATA[" + list[z].Description + "]]></Description><PicUrl><![CDATA[" + ConfigurationSettings.AppSettings["WebUrl"] + list[z].PicUrl + "]]></PicUrl><Url><![CDATA[" + (list[z].IsURL ? (list[z].FulltextUrl.Contains("?id") ? (list[z].FulltextUrl + "&FromUserName=" + requestXML.FROMUSERNAME + "&ToUserName=" + requestXML.TOUSERNAME + "") : (list[z].FulltextUrl + "?FromUserName=" + requestXML.FROMUSERNAME + "&ToUserName=" + requestXML.TOUSERNAME + "")) : (ConfigurationSettings.AppSettings["WebUrl"] + "/GraphicDisplay.aspx?id=" + list[z].DID)) + "]]></Url></item>";//URL是点击之后跳转去那里，这里跳转到百度
                            }
                            resxml += "</Articles><FuncFlag>0</FuncFlag></xml>";
                        }
                        else
                        {
                            //txt未处理的发过去                    
                            try
                            {
                                this.SendMsgToQYH(requestXML.FROMUSERNAME, requestXML.CONTENT, 1, postStr);

                            }
                            catch (Exception ex)
                            {
                                WriteTxt(ex.Message);
                            }
                        }
                    }

                }
                #endregion
                //图片目前全部发过去
                #region 收到图片消息
                else if (requestXML.MSGTYPE == "image")
                {
                    try
                    {

                        this.SendMsgToQYH(requestXML.FROMUSERNAME, requestXML.PICURL, 2, postStr);
                    }
                    catch (Exception ex)
                    {
                        WriteTxt(ex.Message);
                    }
                }
                #endregion
                //语音目前全部发过去
                #region 收到语音消息
                else if (requestXML.MSGTYPE == "voice")
                {
                    try
                    {
                        //string retcs = NetHelper.HttpRequest(AppConfig.QYHMsgUrl, "con=" + postStr, HttpVerbs.POST, 6000, Encoding.UTF8, ContentTypes.FORM);
                    }
                    catch (Exception ex)
                    {
                        WriteTxt(ex.Message);
                    }
                }
                #endregion
                else if (requestXML.MSGTYPE == "event")
                {
                    if (requestXML.WXEVENT == "click")
                    {

                        if (requestXML.EVENTKEY.ToLower() == "dkf") //多客服接口
                        {
                            resxml += "<MsgType><![CDATA[transfer_customer_service]]></MsgType></xml>";
                        }
                        else
                        {
                            List<CustomMenu_EX> list = sbo.GetCustomMenuModelList(string.Format("  c.ID='{0}'",
                                requestXML.EVENTKEY));
                            if (list.Count == 1 || (list.Count > 1 && list[0].MsgType != "news"))
                            {
                                CustomMenu_EX f = list[0];// ibo.GetModel(requestXML.EventKey, 4); //自定义菜单回复

                                if (f != null)
                                {

                                    switch (f.MsgType)
                                    {
                                        case "text":
                                            if (requestXML.CONTENT.Contains("客服"))
                                            {
                                                _logwarn.Warn(requestXML.CONTENT + "客服进来了2");
                                                SendText(requestXML.FROMUSERNAME, "联系华美家，请致电：4008216188。",
                                                    requestXML.TOUSERNAME);

                                                resxml += "<MsgType><![CDATA[transfer_customer_service]]></MsgType></xml>";
                                            }
                                            else
                                            {
                                                resxml += "<MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + f.Content + "]]></Content><FuncFlag>0</FuncFlag></xml>";
                                            }
                                            break;
                                        case "news":
                                            resxml += "<MsgType><![CDATA[news]]></MsgType><ArticleCount>1</ArticleCount><Articles>";
                                            resxml += "<item><Title><![CDATA[" + f.Title + "]]></Title><Description><![CDATA[" + f.Description + "]]></Description><PicUrl><![CDATA[" + ConfigurationSettings.AppSettings["WebUrl"] + f.PicUrl + "]]></PicUrl><Url><![CDATA[" + (f.IsURL ? (f.FulltextUrl.Contains("?id") ? (f.FulltextUrl + "&FromUserName=" + requestXML.FROMUSERNAME + "&ToUserName=" + requestXML.TOUSERNAME + "") : (f.FulltextUrl + "?FromUserName=" + requestXML.FROMUSERNAME + "&ToUserName=" + requestXML.TOUSERNAME + "")) : (ConfigurationSettings.AppSettings["WebUrl"] + "/GraphicDisplay.aspx?id=" + f.DID)) + "]]></Url></item>";//URL是点击之后跳转去那里，这里跳转到百度
                                            resxml += "</Articles><FuncFlag>0</FuncFlag></xml>";
                                            break;
                                        case "music":
                                            resxml += string.Format(@"<MsgType><![CDATA[music]]></MsgType>
                                 <Music>
                                 <Title><![CDATA[{0}]]></Title>
                                 <Description><![CDATA[{1}]]></Description>
                                 <MusicUrl><![CDATA[{2}]]></MusicUrl>
                                 <HQMusicUrl><![CDATA[{2}]]></HQMusicUrl>
                                 </Music>
                                 <FuncFlag>0</FuncFlag>
                                 </xml>", f.Title, f.Description, ConfigurationSettings.AppSettings["WebUrl"] + f.PicUrl);
                                            break;
                                        default:
                                            //resxml += "<MsgType><![CDATA[text]]></MsgType><Content><![CDATA[感谢您的支持]]></Content><FuncFlag>0</FuncFlag></xml>";
                                            break;
                                    }

                                }
                            }
                            else if (list.Count > 1) //多图
                            {
                                int count = list.Count;
                                resxml += "<MsgType><![CDATA[news]]></MsgType><ArticleCount>" + count + "</ArticleCount><Articles>";
                                for (int i = 0; i < count; i++)
                                {
                                    resxml += "<item><Title><![CDATA[" + list[i].Title + "]]></Title><Description><![CDATA[" + list[i].Description + "]]></Description><PicUrl><![CDATA[" + ConfigurationSettings.AppSettings["WebUrl"] + list[i].PicUrl + "]]></PicUrl><Url><![CDATA[" + (list[i].IsURL ? (list[i].FulltextUrl.Contains("?id") ? (list[i].FulltextUrl + "&FromUserName=" + requestXML.FROMUSERNAME + "&ToUserName=" + requestXML.TOUSERNAME + "") : (list[i].FulltextUrl + "?FromUserName=" + requestXML.FROMUSERNAME + "&ToUserName=" + requestXML.TOUSERNAME + "")) : (ConfigurationSettings.AppSettings["WebUrl"] + "/GraphicDisplay.aspx?id=" + list[i].DID)) + "]]></Url></item>";//URL是点击之后跳转去那里，这里跳转到百度
                                }
                                resxml += "</Articles><FuncFlag>0</FuncFlag></xml>";
                            }
                        }

                    }
                    else if (requestXML.WXEVENT == "templatesendjobfinish")
                    {
                        _log.Warn("template:" + requestXML.WXEVENT);
                    }
                    else if (requestXML.WXEVENT == "unsubscribe")
                    {
                        //取消关注 把粉丝表更新状态为0

                        InsertFS(requestXML.FROMUSERNAME, requestXML.TOUSERNAME, 0);  //插入粉丝信息

                        //异步
                        ThreadPool.QueueUserWorkItem(a =>
                        {
                            dt_Dyn_WechatStateTran_req req = new dt_Dyn_WechatStateTran_req();
                            req.OPENID = requestXML.FROMUSERNAME;
                            req.NEW_STATE = "2";
                            req.DATA_SOURCE = AppConfig.DATA_SOURCE;
                            req.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
                            req.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
                            req.VGROUP = AppConfig.VGROUP; //销售组织
                            WebHmjApiHelp.WechatStateTran(req);
                        });

                        //bool bo = _custMember.DeleteMember(requestXML.FROMUSERNAME);
                    }
                    else if (requestXML.WXEVENT.ToLower() == "location") //获取地理位置
                    {
                        SaveLocation(requestXML);
                    }
                    //关注
                    else if (requestXML.WXEVENT.ToLower() == "subscribe")
                    {
                        //给粉丝打上未注册的标签
                        tagopenid(requestXML.FROMUSERNAME);


                     

                        //用户未关注时，进行关注后的事件推送
                        #region 带参数的关注
                        if (requestXML.EVENTKEY != "")
                        {
                            string[] Arrstr = requestXML.EVENTKEY.Split(new char[] { '_' },
                                 StringSplitOptions.RemoveEmptyEntries);

                            string code = requestXML.EVENTKEY.Contains("_")?Arrstr[1].Trim():"";

                            string number = "";
                            try
                            {
                                //判断code是否为数字
                                if (Regex.IsMatch(code, @"^[+-]?\d*[.]?\d*$"))
                                {
                                    code = "";
                                    number = code;
                                }

                                #region 获取被关注的返回值
                                //获取被关注回复的消息
                                List<Information_EX> list = sbo.GetModelList(string.Format("   replytype={0} and ToUserName='{1}'", 2, requestXML.TOUSERNAME));
                                if (list.Count == 1 || (list.Count > 1 && list[0].MsgType != "news"))
                                {
                                    Information_EX i = list[0];
                                    if (i != null)
                                    {
                                        switch (i.MsgType)
                                        {
                                            case "text":
                                                resxml += "<MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + i.Content + "]]></Content><FuncFlag>0</FuncFlag></xml>";
                                                break;
                                            case "news":
                                                resxml += "<MsgType><![CDATA[news]]></MsgType><ArticleCount>1</ArticleCount><Articles>";
                                                resxml += "<item><Title><![CDATA[" + i.Title + "]]></Title><Description><![CDATA[" + i.Description + "]]></Description><PicUrl><![CDATA[" + ConfigurationSettings.AppSettings["WebUrl"] + i.PicUrl + "]]></PicUrl><Url><![CDATA[" + (i.IsURL ? (i.FulltextUrl.Contains("?id") ? (i.FulltextUrl + "&FromUserName=" + requestXML.FROMUSERNAME + "&ToUserName=" + requestXML.TOUSERNAME + "") : (i.FulltextUrl + "?FromUserName=" + requestXML.FROMUSERNAME + "&ToUserName=" + requestXML.TOUSERNAME + "")) : (ConfigurationSettings.AppSettings["WebUrl"] + "/GraphicDisplay.aspx?id=" + i.DID)) + "]]></Url></item>";//URL是点击之后跳转去那里，这里跳转到百度
                                                resxml += "</Articles><FuncFlag>0</FuncFlag></xml>";
                                                break;
                                            case "music":
                                                resxml += string.Format(@"<MsgType><![CDATA[music]]></MsgType>
                                 <Music>
                                 <Title><![CDATA[{0}]]></Title>
                                 <Description><![CDATA[{1}]]></Description>
                                 <MusicUrl><![CDATA[{2}]]></MusicUrl>
                                 <HQMusicUrl><![CDATA[{2}]]></HQMusicUrl>
                                 </Music>
                                 <FuncFlag>0</FuncFlag>
                                 </xml>", i.Title, i.Description, ConfigurationSettings.AppSettings["WebUrl"] + i.PicUrl);
                                                break;
                                            default:
                                                //resxml += "<MsgType><![CDATA[text]]></MsgType><Content><![CDATA[感谢您的关注]]></Content><FuncFlag>0</FuncFlag></xml>";
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        // resxml += "<MsgType><![CDATA[text]]></MsgType><Content><![CDATA[感谢您的关注]]></Content><FuncFlag>0</FuncFlag></xml>";
                                    }
                                }
                                else if (list.Count > 1) //多图
                                {
                                    int count = list.Count;
                                    resxml += "<MsgType><![CDATA[news]]></MsgType><ArticleCount>" + count + "</ArticleCount><Articles>";
                                    for (int i = 0; i < count; i++)
                                    {
                                        resxml += "<item><Title><![CDATA[" + list[i].Title + "]]></Title><Description><![CDATA[" + list[i].Description + "]]></Description><PicUrl><![CDATA[" + ConfigurationSettings.AppSettings["WebUrl"] + list[i].PicUrl + "]]></PicUrl><Url><![CDATA[" + (list[i].IsURL ? (list[i].FulltextUrl.Contains("?id") ? (list[i].FulltextUrl + "&FromUserName=" + requestXML.FROMUSERNAME + "&ToUserName=" + requestXML.TOUSERNAME + "") : (list[i].FulltextUrl + "?FromUserName=" + requestXML.FROMUSERNAME + "&ToUserName=" + requestXML.TOUSERNAME + "")) : (ConfigurationSettings.AppSettings["WebUrl"] + "/GraphicDisplay.aspx?id=" + list[i].DID)) + "]]></Url></item>";//URL是点击之后跳转去那里，这里跳转到百度
                                    }
                                    resxml += "</Articles><FuncFlag>0</FuncFlag></xml>";

                                }
                                #endregion
                            }
                            catch { }

                            //resxml += "<MsgType><![CDATA[news]]></MsgType><ArticleCount>" + 2 + "</ArticleCount><Articles>";
                            //for (int i = 0; i < 2; i++)
                            //{
                            //    resxml += "<item><Title><![CDATA[测试头]]></Title><Description><![CDATA[看我描述" + i + "]]></Description><PicUrl><![CDATA[https://mmbiz.qpic.cn/mmbiz_jpg/PNZr31F6icTjvWWzhY2NjGf0EnjDqobGBa6wvrnHqldR1butbicbQX6y1QUWYpQZvgwuSlBK3icE9YzicWAkO0icToQ/640?wx_fmt=jpeg&tp=webp&wxfrom=5&wx_lazy=1]]></PicUrl><Url><![CDATA[www.baidu.com]]></Url></item>";//URL是点击之后跳转去那里，这里跳转到百度
                            //}
                            //resxml += "</Articles><FuncFlag>0</FuncFlag></xml>";

                            InsertFS(requestXML.FROMUSERNAME, requestXML.TOUSERNAME, 1, code, number);  //插入粉丝信息
                        }
                        #endregion
                        #region 不带参数关注
                        else
                        {
                            WXCUST_FANS fans = sbo.GetFansByFromUserName(requestXML.FROMUSERNAME);
                            if (fans != null)
                            {
                                #region 获取被关注的返回值
                                //获取被关注回复的消息
                                List<Information_EX> list = sbo.GetModelList(string.Format("   replytype={0} and ToUserName='{1}'", 2, requestXML.TOUSERNAME));
                                if (list.Count == 1 || (list.Count > 1 && list[0].MsgType != "news"))
                                {
                                    Information_EX i = list[0];
                                    if (i != null)
                                    {
                                        switch (i.MsgType)
                                        {
                                            case "text":
                                                resxml += "<MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + i.Content + "]]></Content><FuncFlag>0</FuncFlag></xml>";
                                                break;
                                            case "news":
                                                resxml += "<MsgType><![CDATA[news]]></MsgType><ArticleCount>1</ArticleCount><Articles>";
                                                resxml += "<item><Title><![CDATA[" + i.Title + "]]></Title><Description><![CDATA[" + i.Description + "]]></Description><PicUrl><![CDATA[" + ConfigurationSettings.AppSettings["WebUrl"] + i.PicUrl + "]]></PicUrl><Url><![CDATA[" + (i.IsURL ? (i.FulltextUrl.Contains("?id") ? (i.FulltextUrl + "&FromUserName=" + requestXML.FROMUSERNAME + "&ToUserName=" + requestXML.TOUSERNAME + "") : (i.FulltextUrl + "?FromUserName=" + requestXML.FROMUSERNAME + "&ToUserName=" + requestXML.TOUSERNAME + "")) : (ConfigurationSettings.AppSettings["WebUrl"] + "/GraphicDisplay.aspx?id=" + i.DID)) + "]]></Url></item>";//URL是点击之后跳转去那里，这里跳转到百度
                                                resxml += "</Articles><FuncFlag>0</FuncFlag></xml>";
                                                break;
                                            case "music":
                                                resxml += string.Format(@"<MsgType><![CDATA[music]]></MsgType>
                                 <Music>
                                 <Title><![CDATA[{0}]]></Title>
                                 <Description><![CDATA[{1}]]></Description>
                                 <MusicUrl><![CDATA[{2}]]></MusicUrl>
                                 <HQMusicUrl><![CDATA[{2}]]></HQMusicUrl>
                                 </Music>
                                 <FuncFlag>0</FuncFlag>
                                 </xml>", i.Title, i.Description, ConfigurationSettings.AppSettings["WebUrl"] + i.PicUrl);
                                                break;
                                            default:
                                                //resxml += "<MsgType><![CDATA[text]]></MsgType><Content><![CDATA[感谢您的关注]]></Content><FuncFlag>0</FuncFlag></xml>";
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        // resxml += "<MsgType><![CDATA[text]]></MsgType><Content><![CDATA[感谢您的关注]]></Content><FuncFlag>0</FuncFlag></xml>";
                                    }
                                }
                                else if (list.Count > 1) //多图
                                {
                                    int count = list.Count;
                                    resxml += "<MsgType><![CDATA[news]]></MsgType><ArticleCount>" + count + "</ArticleCount><Articles>";
                                    for (int i = 0; i < count; i++)
                                    {
                                        resxml += "<item><Title><![CDATA[" + list[i].Title + "]]></Title><Description><![CDATA[" + list[i].Description + "]]></Description><PicUrl><![CDATA[" + ConfigurationSettings.AppSettings["WebUrl"] + list[i].PicUrl + "]]></PicUrl><Url><![CDATA[" + (list[i].IsURL ? (list[i].FulltextUrl.Contains("?id") ? (list[i].FulltextUrl + "&FromUserName=" + requestXML.FROMUSERNAME + "&ToUserName=" + requestXML.TOUSERNAME + "") : (list[i].FulltextUrl + "?FromUserName=" + requestXML.FROMUSERNAME + "&ToUserName=" + requestXML.TOUSERNAME + "")) : (ConfigurationSettings.AppSettings["WebUrl"] + "/GraphicDisplay.aspx?id=" + list[i].DID)) + "]]></Url></item>";//URL是点击之后跳转去那里，这里跳转到百度
                                    }
                                    resxml += "</Articles><FuncFlag>0</FuncFlag></xml>";

                                }
                                #endregion
                                //resxml += "<MsgType><![CDATA[text]]></MsgType><Content><![CDATA[感谢您重新回来。]]></Content><FuncFlag>0</FuncFlag></xml>";
                                sbo.UpdateCustInfoBySms(requestXML.FROMUSERNAME);
                            }
                            else
                            {
                                #region 获取被关注的返回值
                                //获取被关注回复的消息
                                List<Information_EX> list = sbo.GetModelList(string.Format("   replytype={0} and ToUserName='{1}'", 2, requestXML.TOUSERNAME));
                                if (list.Count == 1 || (list.Count > 1 && list[0].MsgType != "news"))
                                {
                                    Information_EX i = list[0];
                                    if (i != null)
                                    {
                                        switch (i.MsgType)
                                        {
                                            case "text":
                                                resxml += "<MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + i.Content + "]]></Content><FuncFlag>0</FuncFlag></xml>";
                                                break;
                                            case "news":
                                                resxml += "<MsgType><![CDATA[news]]></MsgType><ArticleCount>1</ArticleCount><Articles>";
                                                resxml += "<item><Title><![CDATA[" + i.Title + "]]></Title><Description><![CDATA[" + i.Description + "]]></Description><PicUrl><![CDATA[" + ConfigurationSettings.AppSettings["WebUrl"] + i.PicUrl + "]]></PicUrl><Url><![CDATA[" + (i.IsURL ? (i.FulltextUrl.Contains("?id") ? (i.FulltextUrl + "&FromUserName=" + requestXML.FROMUSERNAME + "&ToUserName=" + requestXML.TOUSERNAME + "") : (i.FulltextUrl + "?FromUserName=" + requestXML.FROMUSERNAME + "&ToUserName=" + requestXML.TOUSERNAME + "")) : (ConfigurationSettings.AppSettings["WebUrl"] + "/GraphicDisplay.aspx?id=" + i.DID)) + "]]></Url></item>";//URL是点击之后跳转去那里，这里跳转到百度
                                                resxml += "</Articles><FuncFlag>0</FuncFlag></xml>";
                                                break;
                                            case "music":
                                                resxml += string.Format(@"<MsgType><![CDATA[music]]></MsgType>
                                 <Music>
                                 <Title><![CDATA[{0}]]></Title>
                                 <Description><![CDATA[{1}]]></Description>
                                 <MusicUrl><![CDATA[{2}]]></MusicUrl>
                                 <HQMusicUrl><![CDATA[{2}]]></HQMusicUrl>
                                 </Music>
                                 <FuncFlag>0</FuncFlag>
                                 </xml>", i.Title, i.Description, ConfigurationSettings.AppSettings["WebUrl"] + i.PicUrl);
                                                break;
                                            default:
                                                //resxml += "<MsgType><![CDATA[text]]></MsgType><Content><![CDATA[感谢您的关注]]></Content><FuncFlag>0</FuncFlag></xml>";
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        // resxml += "<MsgType><![CDATA[text]]></MsgType><Content><![CDATA[感谢您的关注]]></Content><FuncFlag>0</FuncFlag></xml>";
                                    }
                                }
                                else if (list.Count > 1) //多图
                                {
                                    int count = list.Count;
                                    resxml += "<MsgType><![CDATA[news]]></MsgType><ArticleCount>" + count + "</ArticleCount><Articles>";
                                    for (int i = 0; i < count; i++)
                                    {
                                        resxml += "<item><Title><![CDATA[" + list[i].Title + "]]></Title><Description><![CDATA[" + list[i].Description + "]]></Description><PicUrl><![CDATA[" + ConfigurationSettings.AppSettings["WebUrl"] + list[i].PicUrl + "]]></PicUrl><Url><![CDATA[" + (list[i].IsURL ? (list[i].FulltextUrl.Contains("?id") ? (list[i].FulltextUrl + "&FromUserName=" + requestXML.FROMUSERNAME + "&ToUserName=" + requestXML.TOUSERNAME + "") : (list[i].FulltextUrl + "?FromUserName=" + requestXML.FROMUSERNAME + "&ToUserName=" + requestXML.TOUSERNAME + "")) : (ConfigurationSettings.AppSettings["WebUrl"] + "/GraphicDisplay.aspx?id=" + list[i].DID)) + "]]></Url></item>";//URL是点击之后跳转去那里，这里跳转到百度
                                    }
                                    resxml += "</Articles><FuncFlag>0</FuncFlag></xml>";

                                }
                                #endregion
                            }
                            InsertFS(requestXML.FROMUSERNAME, requestXML.TOUSERNAME, 1);  //插入粉丝信息
                        }
                        #endregion

                        #region 第一次关注crm关注
                        ThreadPool.QueueUserWorkItem(a =>
                        {
                            dt_Dyn_WechatStateTran_req req = new dt_Dyn_WechatStateTran_req();
                            req.OPENID = requestXML.FROMUSERNAME;
                            req.NEW_STATE = "1";
                            req.DATA_SOURCE = AppConfig.DATA_SOURCE;
                            req.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
                            req.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
                            req.VGROUP = AppConfig.VGROUP; //销售组织
                            WebHmjApiHelp.WechatStateTran(req);

                            //通知crm创建
                            dt_Dyn_CreateLead_req crreq = new dt_Dyn_CreateLead_req();
                            ZCRMT342_Dyn meber = new ZCRMT342_Dyn();
                            meber.WECHATFOLLOWSTATUS = "1";
                            meber.WECHAT = requestXML.FROMUSERNAME;
                            meber.NAME1_TEXT = "华美家潜客";//全名
                            meber.DATA_SOURCE = AppConfig.DATA_SOURCE;
                            meber.EMPID = "HMWX001";
                            meber.DEPTID = AppConfig.DEPTID;//入会门店
                            meber.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
                            meber.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
                            meber.VGROUP = AppConfig.VGROUP; //销售组织
                            crreq.INFO_QK = new ZCRMT342_Dyn[] { meber };
                            dt_Dyn_CreateLead_res qianke = WebHmjApiHelp.CreateLead(crreq);
                        });
                        #endregion
                    }
                    //微信用户扫带参数二维码，无操作
                    else if (requestXML.WXEVENT.ToLower() == "scan")
                    {

                    }
                    else if (requestXML.WXEVENT.ToLower() == "view")
                    {


                    }
                    else if (requestXML.WXEVENT.ToLower() == "user_get_card")//领取卡券
                    {
                        try
                        {
                            IWXCouponService wxcouponservice = new WXCouponService();
                            string openid = requestXML.FROMUSERNAME;
                            string CardId = requestXML.EVENTKEY;
                            string UserCardCode = requestXML.CONTENT;
                            //将卡券标准已领取
                            wxcouponservice.UpdateWXCouponIsGet(openid, UserCardCode, CardId);
                        }
                        catch (Exception ex)
                        {
                            WriteTxt("领取卡券异常：CardId=" + requestXML.EVENTKEY + " OpenId=" + requestXML.CONTENT + " 异常信息：" + ex.Message);
                        }

                    }
                    else if (requestXML.WXEVENT.ToLower() == "user_consume_card")//核销卡券
                    {
                        try
                        {
                            IWXCouponService wxcouponservice = new WXCouponService();
                            string openid = requestXML.FROMUSERNAME;
                            string CardId = requestXML.EVENTKEY;
                            string UserCardCode = requestXML.CONTENT;
                            //将卡券标准已核销
                            wxcouponservice.UpdateWXCouponIsHX(openid, UserCardCode, CardId);
                        }
                        catch (Exception ex)
                        {
                            WriteTxt("核销卡券异常：CardId=" + requestXML.EVENTKEY + " OpenId=" + requestXML.CONTENT + " 异常信息：" + ex.Message);
                        }

                    }
                    //else if (requestXML.WXEVENT.ToLower() == "user_get_card")//领取卡券
                    //{
                    //    string openid = requestXML.FROMUSERNAME;
                    //    string CardId = requestXML.EVENTKEY;
                    //    string UserCardCode = requestXML.CONTENT;
                    //    //将卡券标准已领取
                    //    sbo.UpdateWXCouponIsGet(openid, UserCardCode, CardId);

                    //}
                    //else if (requestXML.WXEVENT.ToLower() == "user_consume_card")//核销卡券
                    //{
                    //    string openid = requestXML.FROMUSERNAME;
                    //    string CardId = requestXML.EVENTKEY;
                    //    string UserCardCode = requestXML.CONTENT;
                    //    //将卡券标准已核销
                    //    sbo.UpdateWXCouponIsHX(openid, UserCardCode, CardId);

                    //}
                    //
                }
                else if (requestXML.MSGTYPE == "location") //地理位置
                {
                    //SaveLocation(requestXML);//只有自动上报的才添加到地理位置表
                    List<ORG_STORE_EX> list = sbo.GetORG_STOREOrderBy(string.Format("SELECT TOP 5 dbo.jl({0},{1},Lat,Lng) jl,* FROM dbo.ORG_STORE ORDER BY dbo.jl({0},{1},Lat,Lng)", requestXML.LOCATION_X, requestXML.LOCATION_Y));
                    int count = list.Count;
                    resxml += "<MsgType><![CDATA[news]]></MsgType><ArticleCount>" + count + "</ArticleCount><Articles>";
                    for (int i = 0; i < count; i++)
                    {
                        resxml += "<item><Title><![CDATA[【" + Math.Round(list[i].jl / 1000, 2) + "公里】" + list[i].NAME + ",电话：" + list[i].TELEPHONE + "]]></Title><Description><![CDATA[【" + Math.Round(list[i].jl / 1000, 2) + "公里】" + list[i].NAME + ",电话：" + list[i].TELEPHONE + "]]></Description><PicUrl><![CDATA[" + ConfigurationSettings.AppSettings["WebUrl"] + (i == 0 ? "/ml2.jpg" : "/ml.jpg") + "]]></PicUrl><Url><![CDATA[http://" + ConfigurationSettings.AppSettings["ServerIP"] + "/Store/Map.aspx?p1=" + requestXML.LOCATION_X + "&p2=" + requestXML.LOCATION_Y + "&p3=" + list[i].Lat + "&p4=" + list[i].Lng + "]]></Url></item>";//URL是点击之后跳转去那里，这里跳转到百度
                    }
                    resxml += "</Articles><FuncFlag>0</FuncFlag></xml>";
                }

                requestXML.RESXML = resxml;
                if (!resxml.Contains("</xml>"))
                {
                    _log.Fatal("requestXML.MSGTYPE:" + requestXML.MSGTYPE);
                    _log.Fatal("requestXML.WXEVENT:" + requestXML.WXEVENT);
                    resxml = "";
                    //resxml += "<MsgType><![CDATA[text]]></MsgType><Content></Content><FuncFlag>0</FuncFlag></xml>";
                }
                if (requestXML.MSGTYPE == "text")
                {

                    try
                    {
                        WXCUST_MSG_RECORD msg = new WXCUST_MSG_RECORD();
                        msg.CONTENT = requestXML.CONTENT;
                        msg.CREATE_DATE = DateTime.Now;
                        msg.FROMUSERNAME = requestXML.FROMUSERNAME;
                        msg.GraphicID = 0;
                        msg.IS_RETURN = false;
                        msg.IS_STAR = false;
                        msg.MSGTYPE = "text";
                        msg.ReturnID = 0;
                        msg.State = 0;
                        msg.TOUSERNAME = requestXML.TOUSERNAME;
                        sbo.SaveCUST_MSG_RECORD(msg);
                        //用于久峰接口对接(48小时)
                        //var cpresponse = PostRequest("http://qyh.censh.com/censh/fwh/activeStatus/update/" + requestXML.FROMUSERNAME + "");
                        //_log.Debug("text:" + requestXML.FROMUSERNAME);
                    }
                    catch (Exception)
                    {

                    }
                }


            }
            catch (Exception ex)
            {
                WriteTxt("异常：" + ex.Message);
                resxml += "<MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + ex.Message.ToString() + "]]></Content><FuncFlag>0</FuncFlag></xml>";
                Response.Write(resxml);
            }


            //if (!resxml.Contains("</xml>"))
            //{
            //    resxml += "<MsgType><![CDATA[text]]></MsgType><Content><![CDATA[]]></Content><FuncFlag>0</FuncFlag></xml>";
            //}
            WriteTxt("返回给粉丝的消息：" + resxml);
            requestXML.RESXML = resxml;
            try
            {
                sbo.AddCUST_MSG_HIS(requestXML);
                //将粉丝扫二维码的记录存入另外一张表
                if (requestXML.MSGTYPE == "event" 
                    &&
                        ((requestXML.WXEVENT == "scan" && requestXML.EVENTKEY != "") 
                        || (requestXML.WXEVENT == "subscribe" && requestXML.EVENTKEY != ""))
                    )
                {
                    WXCustScanRecord scanrecord = new WXCustScanRecord();
                    scanrecord.TOUSERNAME = requestXML.TOUSERNAME;
                    scanrecord.FROMUSERNAME = requestXML.FROMUSERNAME;
                    scanrecord.MSGTYPE = requestXML.MSGTYPE;
                    scanrecord.WXEVENT = requestXML.WXEVENT;
                    scanrecord.EVENTKEY = requestXML.EVENTKEY;
                    scanrecord.CREATE_DATE = DateTime.Now;
                    sbo.AddWXCustScanRecord(scanrecord);
                }

            }
            catch (Exception ex)
            {
                WriteTxt(@"插入错误：" + ex.ToString() + requestXML.FROMUSERNAME + "------" +
                    "TOUSERNAME：" + requestXML.TOUSERNAME + "------" +
                    "TO_FANSID：" + requestXML.TO_FANSID + "------" +
                    "FROMUSERNAME：" + requestXML.FROMUSERNAME + "------" +
                    "FROM_FANSID：" + requestXML.FROM_FANSID + "------" +
                    "MSGTYPE：" + requestXML.MSGTYPE + "------" +
                    "CONTENT：" + requestXML.CONTENT + "------" +
                    "MSGID：" + requestXML.MSGID + "------" +
                    "WXEVENT：" + requestXML.WXEVENT + "------" +
                    "EVENTKEY：" + requestXML.EVENTKEY + "------" +
                    "LOCATION_X：" + requestXML.LOCATION_X + "------" +
                    "LOCATION_Y：" + requestXML.LOCATION_Y + "------" +
                    "SCALE：" + requestXML.SCALE + "------" +
                    "LABEL：" + requestXML.LABEL + "------" +
                    "PICURL：" + requestXML.PICURL + "------" +
                    "VEDIOURL：" + requestXML.VEDIOURL + "------" +
                    "RESXML：" + requestXML.RESXML + "------" +
                    "ReturnID：" + requestXML.ReturnID + "------" +
                    "Return_Con：" + requestXML.Return_Con + "------" +
                    "IS_RETURN：" + requestXML.IS_RETURN + "------" +
                    "IS_STAR：" + requestXML.IS_STAR + "------" +
                    "STATUS：" + requestXML.STATUS + "------" +
                    "CREATE_DATE：" + requestXML.CREATE_DATE + "------" +
                    "OLDXML：" + requestXML.OLDXML + "------" +
                    "OriginalXml：" + requestXML.OriginalXml + "------" +
                    "ISOriginalXml：" + requestXML.ISOriginalXml);
            }
            Response.Write(resxml);
            Response.End();
        }

        public string SendText(string OpenId, string Content, string tousers)
        {
            var text = new
            {
                content = Content
            };
            var postjson = new
            {
                touser = OpenId,
                msgtype = "text",
                text = text
            };
            string jsonpost = JsonConvert.SerializeObject(postjson);
            return Send(jsonpost, tousers);
        }

        public string Send(string post, string tousers)
        {
            BasePage bpage = new BasePage();
            string access_token = bpage.MyToken(tousers);
            string url = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + access_token;
            _logwarn.Warn(url + "===" + post);
            return PostJson(url, post);
        }

        public string PostJson(string url, string json)
        {
            var resMessage = HttpJsonPostRequest(url, json, Encoding.UTF8);
            _logwarn.Warn(resMessage);
            return resMessage;

        }
        public string HttpJsonPostRequest(string postUrl, string postJson, Encoding encoding)
        {
            StringBuilder sb = new StringBuilder();
            string IP = HttpContext.Current.Request.UserHostAddress;
            sb.Append("=====（" + IP + "）=====").AppendLine().
                Append("请求开始：").Append("URL:" + postUrl).AppendLine().Append("DATA:" + postJson);
            if (string.IsNullOrEmpty(postUrl))
            {
                throw new ArgumentNullException("HttpJsonPost ArgumentNullException :  postUrl IsNullOrEmpty");
            }

            if (string.IsNullOrEmpty(postJson))
            {
                throw new ArgumentNullException("HttpJsonPost ArgumentNullException : postJson IsNullOrEmpty");
            }
            var request = (HttpWebRequest)WebRequest.Create(postUrl);
            byte[] byteArray = encoding.GetBytes(postJson);
            request.ContentLength = byteArray.Length;
            request.Method = "post";
            request.ContentType = "application/json;charset=utf-8";

            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(byteArray, 0, byteArray.Length);
            }

            using (var responseStream = request.GetResponse().GetResponseStream())
            {
                string res = new StreamReader(responseStream, encoding).ReadToEnd();

                sb.AppendLine();
                sb.Append("请求结束：" + res);

                return res;
            }
        }

        /// <summary>
        /// 转发消息到企业号
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="content"></param>
        /// <param name="type"></param>
        private void SendMsgToQYH(string openId, string content, int type, string postStr)
        {

        }
        #region 用于图片上传

        private int UploadImgForWx(string name)
        {
            string filePath = string.Concat(AppConfig.UploadWX, "h1.jpg");
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            int byteLength = (int)fileStream.Length;
            byte[] fileBytes = new byte[byteLength];
            fileStream.Read(fileBytes, 0, byteLength);
            //文件流关閉,文件解除锁定
            fileStream.Close();

            MemoryStream ms = GetMemoryStrem(new MemoryStream(fileBytes), name, Color.Black);
            string Extension = Path.GetExtension(filePath).ToLower();
            string url = "1";
            FILES fileEntity = _mservice.UploadFile(Extension, AppConfig.UploadWX, "image/jpeg", fileBytes, url, "wechat");
            return fileEntity.ID;
        }

        private MemoryStream GetMemoryStrem(MemoryStream s, string name, Color c)
        {
            System.Drawing.Image Images = System.Drawing.Image.FromStream(s);
            Bitmap bmp = new Bitmap(Images);
            Graphics g = Graphics.FromImage(bmp);
            SolidBrush sbrush = new SolidBrush(c);
            //g.SmoothingMode = SmoothingMode.HighQuality;
            int font_num = 45;
            if (name.Length > 2)
            {
                g.DrawString("我是您的钟表管家", new Font("微软雅黑", font_num), sbrush, new PointF(30, 30));
                g.DrawString("“" + name + "”", new Font("微软雅黑", 50), sbrush, new PointF(500, 20));
                g.DrawString(",将", new Font("微软雅黑", font_num), sbrush, new PointF(830, 30));
                g.DrawString("为您在“选表、购表、售后、维修”", new Font("微软雅黑", font_num), sbrush, new PointF(30, 130));
                g.DrawString("的每一步提供“一对一钟表管家”", new Font("微软雅黑", font_num), sbrush, new PointF(30, 230));
                g.DrawString("个性化服务！让您零烦恼！", new Font("微软雅黑", font_num), sbrush, new PointF(30, 330));
            }
            else
            {
                g.DrawString("我是您的钟表管家", new Font("微软雅黑", font_num), sbrush, new PointF(60, 30));
                g.DrawString("“" + name + "”", new Font("微软雅黑", 50), sbrush, new PointF(520, 20));
                g.DrawString(",将为", new Font("微软雅黑", font_num), sbrush, new PointF(780, 30));
                g.DrawString("您在“选表、购表、售后、维修”", new Font("微软雅黑", font_num), sbrush, new PointF(60, 130));
                g.DrawString("的每一步提供“一对一钟表管家”", new Font("微软雅黑", font_num), sbrush, new PointF(60, 230));
                g.DrawString("个性化服务！让您零烦恼！", new Font("微软雅黑", font_num), sbrush, new PointF(60, 330));
            }

            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms;
        }



        /// <summary>
        /// unix时间转换为datetime
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        private DateTime UnixTimeToTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        /// <summary>
        /// datetime转换为unixtime
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        /// <summary>
        /// 记录bug，以便调试
        /// </summary>
        /// 
        /// <returns></returns>
        public bool WriteTxt(string str)
        {
            try
            {
                //FileStream fs = new FileStream(@"D:\WeiXin\web\buglog.txt", FileMode.Append);
                //StreamWriter sw = new StreamWriter(fs);
                ////开始写入
                //sw.WriteLine(str);
                ////清空缓冲区
                //sw.Flush();
                ////关闭流
                //sw.Close();
                //fs.Close();
                WXLOG l = new WXLOG();
                l.CON = str;
                l.TIME = DateTime.Now;
                sbo.AddLog(l);
            }
            catch (Exception)
            {
                return false;
            }
            return true;

        }


        //发送GET请求
        public string PostRequestGet(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";  //定义请求对象，并设置好请求URL地址      
            //request.ProtocolVersion = HttpVersion.Version10;
            //request.ContentType = "image/jpg";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();    //定义响应对象，request在调用GetResponse方法事执行请求了，而不是在HttpWebRequest.Create的时候执行。
            //response.ContentType = "image/jpg";
            Stream stream = response.GetResponseStream(); //定义一个流对象，来获取响应流
            StreamReader sr = new StreamReader(stream, Encoding.UTF8);  //定义一个流读取对象，读取响应流
            string responseHTML = sr.ReadToEnd();
            return responseHTML;
        }

        ////发送post请求
        //public string PostRequest(string url)
        //{
        //    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);  //定义请求对象，并设置好请求URL地址

        //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();    //定义响应对象，request在调用GetResponse方法事执行请求了，而不是在HttpWebRequest.Create的时候执行。
        //    Stream stream = response.GetResponseStream(); //定义一个流对象，来获取响应流
        //    StreamReader sr = new StreamReader(stream);  //定义一个流读取对象，读取响应流
        //    string responseHTML = sr.ReadToEnd();
        //    return responseHTML;
        //}
        #endregion
        class Info
        {
            public int subscribe { get; set; }
            public string openid { get; set; }
            public string nickname { get; set; }
            public string sex { get; set; }
            public string language { get; set; }
            public string city { get; set; }
            public string province { get; set; }
            public string country { get; set; }
            public string headimgurl { get; set; }
            public string subscribe_time { get; set; }
        }
    }





    class mediainfo
    {
        public string type { get; set; }
        public string media_id { get; set; }
        public int created_at { get; set; }
    }
    class kfinfo
    {
        public string errcode { get; set; }
        public string errmsg { get; set; }
    }
}
