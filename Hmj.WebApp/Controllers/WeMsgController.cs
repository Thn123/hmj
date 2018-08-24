using CyRong.Common;
using Hmj.Business.ServiceImpl;
using Hmj.Common;
using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Extension;
using Hmj.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using System.Xml;
using WeChatCRM.Common.Utils;

namespace Hmj.WebApp.Controllers
{
    [NoAuthorize]
    public class WeMsgController : Controller
    {
        //
        // GET: /We/
        string Token = AppConfig.WXMPToken;
        ISystemService _wechat = new SystemService();
        ILogService _log = new LogService();
        ICommonService _mservice = new CommonService();
        //获取微信access_token
        WeiPage ba = new WeiPage();

        //校验开发者资质
        public ActionResult WeiXin(string signature, string timestamp, string nonce, string echostr)
        {
            if (CheckSignature(signature, timestamp, nonce))
            {
                if (!string.IsNullOrEmpty(echostr))
                {
                    return Content(echostr);
                }
            }
            return Content("");
        }

        //接收/回复 消息接口
        [HttpPost]
        public ActionResult WeiXin()
        {
            //回复微信请求
            string resContent = "success";
            // 微信加密签名    
            String signature = Request["signature"];
            // 时间戳    
            String timestamp = Request["timestamp"];
            // 随机数    
            String nonce = Request["nonce"];
            try
            {
                string postStr = "";

                if (Request.HttpMethod.ToLower() == "post")
                {
                    Stream s = System.Web.HttpContext.Current.Request.InputStream;
                    byte[] b = new byte[s.Length];
                    s.Read(b, 0, (int)s.Length);
                    postStr = Encoding.UTF8.GetString(b);

                    if (!string.IsNullOrEmpty(postStr))
                    {
                        //封装请求类
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(postStr);
                        XmlElement rootElement_AES = doc.DocumentElement;
                        if (rootElement_AES.SelectSingleNode("Encrypt") != null)
                        {
                            string tousername = AppConfig.FWHOriginalID;
                            postStr = Cryptography.AES_decrypt(rootElement_AES.SelectSingleNode("Encrypt").InnerText, "0X44WEGjt40oLYGBo92BD9qo8urVIimUjnikIPFx3u2", ref tousername);
                            //int v= wxcpt.DecryptMsg(signature, timestamp, nonce,postStr , ref sMsg);
                        }
                        doc.LoadXml(postStr);
                        XmlElement rootElement = doc.DocumentElement;

                        XmlNode MsgType = rootElement.SelectSingleNode("MsgType");

                        WXCUST_MSG_HIS requestXML = new WXCUST_MSG_HIS();
                        requestXML.OLDXML = postStr;
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


                        requestXML.MSGTYPE = MsgType.InnerText.ToLower();

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
                            requestXML.WXEVENT = rootElement.SelectSingleNode("Event").InnerText;

                            if (requestXML.WXEVENT.ToLower() == "location")  //高级接口获取到的地理位置
                            {
                                requestXML.LOCATION_Y = rootElement.SelectSingleNode("Latitude").InnerText;  //纬度
                                requestXML.LOCATION_X = rootElement.SelectSingleNode("Longitude").InnerText;  //经度
                                requestXML.SCALE = rootElement.SelectSingleNode("Precision").InnerText;  //精度
                            }
                            else if (requestXML.WXEVENT.ToLower() == "templatesendjobfinish")
                            {
                                requestXML.STATUS = rootElement.SelectSingleNode("Status").InnerText;
                                requestXML.MSGID = rootElement.SelectSingleNode("MsgID").InnerText;
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
                        //回复消息
                        resContent = ResponseMsg(requestXML, signature, timestamp, nonce, postStr);
                    }
                }

            }
            catch (Exception e)
            {
                _log.Error("e:" + e.Message);
            }

            return Content(resContent);
        }

        #region 微信消息处理

        //回复消息(微信信息返回)
        private string ResponseMsg(WXCUST_MSG_HIS requestXML, string signature, string timestamp, string nonce, string postStr)
        {
            string resxml = "<xml><ToUserName><![CDATA[" + requestXML.FROMUSERNAME + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.TOUSERNAME + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime>";
            try
            {
                switch (requestXML.MSGTYPE.ToLower())
                {
                    case "text":
                        //处理文本消息
                        resxml += GetTextMessage(requestXML, postStr);
                        break;
                    case "image":
                        //处理图片消息
                        resxml += GetImageMessage(requestXML, 001, postStr);
                        break;
                    case "voice":
                        //处理语音消息
                        resxml += GetVoiceMessage(requestXML);
                        break;
                    case "video":
                        //处理视频消息
                        resxml += GetVideoMessage(requestXML);
                        break;
                    case "music":
                        //处理视频消息
                        resxml += GetVideoMessage(requestXML);
                        break;
                    case "shortvideo":
                        //处理小视频消息
                        resxml += GetShortVideoMessage(requestXML);
                        break;
                    case "location":
                        //处理位置消息
                        resxml += GetLocationMessage(requestXML);
                        break;
                    case "link":
                        //处理链接消息
                        resxml += GetLinkMessage(requestXML);
                        break;
                    case "event":
                        //处理事件消息,用户在关注与取消关注公众号时，微信会向我们的公众号服务器发送事件消息,开发者接收到事件消息后就可以给用户下发欢迎消息
                        resxml += GetEventMessage(requestXML);
                        break;
                    default:
                        break;
                }

                if (!resxml.Contains("</xml>"))
                {
                    resxml = "success";
                }
                requestXML.RESXML = resxml;

                try
                {
                    _wechat.AddCUST_MSG_HIS(requestXML);
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

            }
            catch (Exception ex)
            {
                _log.Error("responseXML:" + ex.Message);
            }
            finally
            {

            }
            return resxml;
        }

        //接收文本消息
        private string GetTextMessage(WXCUST_MSG_HIS requestXML, string postStr)
        {
            string xmlRe = SetTestInformation(requestXML);
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
            _wechat.SaveCUST_MSG_RECORD(msg);

            #region 同时往测试环境通知消息
            HttpRequestHelper.HttpGetRequest("http://testqyh.censh.cn/censh/fwh/activeStatus/update/" + requestXML.FROMUSERNAME);
            #endregion

            CheckoutActiveToQYH(requestXML);
            this.SendMsgToQYH(requestXML.FROMUSERNAME, requestXML.CONTENT, 1, postStr);


            return xmlRe;
        }

        //接收图片消息
        private string GetImageMessage(WXCUST_MSG_HIS requestXML, int mediaid, string postStr)
        {
            List<Information_EX> info = _wechat.GetModelListByKeywordList(1, requestXML.CONTENT, requestXML.TOUSERNAME);
            string xmlRe = "";
            if (info != null)
            {
                xmlRe = string.Format(@"<MsgType><![CDATA[image]]></MsgType>
                                        <Image>
                                        <MediaId><![CDATA[{0}]]></MediaId>
                                        </Image>
                                        </xml>", mediaid);

                xmlRe = "";
            }
            try
            {

                this.SendMsgToQYH(requestXML.FROMUSERNAME, requestXML.PICURL, 2, postStr);
            }
            catch (Exception)
            {
            }
            return xmlRe;
        }

        //接收语音消息
        private string GetVoiceMessage(WXCUST_MSG_HIS requestXML)
        {
            List<Information_EX> info = _wechat.GetModelListByKeywordList(1, requestXML.CONTENT, requestXML.TOUSERNAME);
            string xmlRe = "";
            if (info != null)
            {
                xmlRe = string.Format(@"<MsgType><![CDATA[voice]]></MsgType>
                                        <Voice>
                                        <MediaId><![CDATA[media_id]]></MediaId>
                                        </Voice>
                                        </xml>", 001);
            }
            return xmlRe;
        }

        //接收视频消息
        private string GetVideoMessage(WXCUST_MSG_HIS requestXML)
        {
            List<Information_EX> info = _wechat.GetModelListByKeywordList(1, requestXML.CONTENT, requestXML.TOUSERNAME);
            string xmlRe = "";
            if (info != null)
            {
                xmlRe = string.Format(@"<MsgType><![CDATA[video]]></MsgType>
                                        <Video>
                                        <MediaId><![CDATA[{0}]]></MediaId>
                                        <Title><![CDATA[{1}]]></Title>
                                        <Description><![CDATA[{2}]]></Description>
                                        </Video> 
                                        </xml>", 001, info[0].Title, info[0].Description);
            }
            return xmlRe;
        }

        //接收小视频消息
        private string GetShortVideoMessage(WXCUST_MSG_HIS requestXML)
        {
            List<Information_EX> info = _wechat.GetModelListByKeywordList(1, requestXML.CONTENT, requestXML.TOUSERNAME);
            string xmlRe = "";
            if (info.Count > 0)
            {
                xmlRe = string.Format(@"<MsgType><![CDATA[shortvideo]]></MsgType>
                                        <MediaId><![CDATA[media_id]]></MediaId>
                                        <ThumbMediaId><![CDATA[{0}]]></ThumbMediaId>
                                        <MsgId>1234567890123456</MsgId>
                                        </xml>", 001, info[0].Title, info[0].Description);
            }
            return xmlRe;
        }

        //接收位置消息
        private string GetLocationMessage(WXCUST_MSG_HIS requestXML)
        {
            string xmlRe = "";
            SaveLocation(requestXML);
            List<ORG_STORE_EX> list = _wechat.GetORG_STOREOrderBy(string.Format("SELECT TOP 5 dbo.jl({0},{1},Lat,Lng) jl,* FROM dbo.ORG_STORE ORDER BY dbo.jl({0},{1},Lat,Lng)", requestXML.LOCATION_X, requestXML.LOCATION_Y));
            int count = list.Count;
            xmlRe += "<MsgType><![CDATA[news]]></MsgType><ArticleCount>" + count + "</ArticleCount><Articles>";
            for (int i = 0; i < count; i++)
            {
                xmlRe += "<item><Title><![CDATA[【" + Math.Round(list[i].jl / 1000, 2) + "公里】" + list[i].NAME + ",电话：" + list[i].TELEPHONE + "]]></Title><Description><![CDATA[【" + Math.Round(list[i].jl / 1000, 2) + "公里】" + list[i].NAME + ",电话：" + list[i].TELEPHONE + "]]></Description><PicUrl><![CDATA[" + ConfigurationSettings.AppSettings["WebUrl"] + (i == 0 ? "/ml2.jpg" : "/ml.jpg") + "]]></PicUrl><Url><![CDATA[http://" + ConfigurationSettings.AppSettings["ServerIP"] + "/Store/Map.aspx?p1=" + requestXML.LOCATION_X + "&p2=" + requestXML.LOCATION_Y + "&p3=" + list[i].Lat + "&p4=" + list[i].Lng + "]]></Url></item>";//URL是点击之后跳转去那里，这里跳转到百度
            }
            xmlRe += "</Articles><FuncFlag>0</FuncFlag></xml>";
            return xmlRe;
        }

        //上传图片到微信服务器
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

        //接收链接消息
        private string GetLinkMessage(WXCUST_MSG_HIS requestXML)
        {
            string xmlRe = "";
            SaveLocation(requestXML);
            return xmlRe;
        }

        //接收事件消息
        private string GetEventMessage(WXCUST_MSG_HIS requestXML)
        {
            string xmlRe = "";
            if (requestXML.WXEVENT.ToLower() == "click")//点击菜单拉取消息时的事件推送
            {
                if (requestXML.EVENTKEY.ToLower() == "dkf") //多客服接口
                {
                    xmlRe += "<MsgType><![CDATA[transfer_customer_service]]></MsgType></xml>";
                }
                else if (requestXML.EVENTKEY.ToLower() == "153") //腕表趣闻
                {
                    //上传素材
                    string url = string.Format("http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", new WeiPage().Token(requestXML.TOUSERNAME), "image");
                    string json = Utility.HttpUploadFile(url, AppConfig.UploadWX + "mianfei.jpg");
                    mediainfo media = JsonConvert.DeserializeObject<mediainfo>(json);
                    xmlRe += "<MsgType><![CDATA[image]]></MsgType><Image><MediaId><![CDATA[" + media.media_id + "]]></MediaId></Image></xml>";
                }
                else if (requestXML.EVENTKEY.ToLower() == "162") //私人管家
                {
                    //上传素材
                    string url = string.Format("http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", new WeiPage().Token(requestXML.TOUSERNAME), "image");
                    string json = Utility.HttpUploadFile(url, AppConfig.UploadWX + "SiRenGuanJia.jpg");
                    mediainfo media = JsonConvert.DeserializeObject<mediainfo>(json);
                    xmlRe += "<MsgType><![CDATA[image]]></MsgType><Image><MediaId><![CDATA[" + media.media_id + "]]></MediaId></Image></xml>";
                }
                else
                {
                    List<CustomMenu_EX> list = _wechat.GetCustomMenuModelList(requestXML.EVENTKEY, requestXML.TOUSERNAME);
                    if (list.Count == 1 || (list.Count > 1 && list[0].MsgType != "news"))
                    {
                        CustomMenu_EX f = list[0];// ibo.GetModel(requestXML.EventKey, 4); //自定义菜单回复

                        if (f != null)
                        {
                            xmlRe += SetInfomation(f.MsgType, f.Content, f.Description, f.Title, f.PicUrl, requestXML.TOUSERNAME, requestXML.FROMUSERNAME, f.FulltextUrl, f.DID, f.IsURL);

                        }
                    }
                    else if (list.Count > 1) //多图
                    {
                        int count = list.Count;
                        xmlRe += "<MsgType><![CDATA[news]]></MsgType><ArticleCount>" + count + "</ArticleCount><Articles>";
                        for (int i = 0; i < count; i++)
                        {
                            xmlRe += "<item><Title><![CDATA[" + list[i].Title + "]]></Title><Description><![CDATA[" + list[i].Description + "]]></Description><PicUrl><![CDATA[" + AppConfig.CurrentUri + list[i].PicUrl + "]]></PicUrl><Url><![CDATA[" + (list[i].IsURL ? (list[i].FulltextUrl.Contains("?id") ? (list[i].FulltextUrl + "&FromUserName=" + requestXML.FROMUSERNAME + "&ToUserName=" + requestXML.TOUSERNAME + "") : (list[i].FulltextUrl + "?FromUserName=" + requestXML.FROMUSERNAME + "&ToUserName=" + requestXML.TOUSERNAME + "")) : (AppConfig.CurrentUri + "/Admin/GraphicDisplay?id=" + list[i].DID)) + "]]></Url></item>";//URL是点击之后跳转去那里，这里跳转到百度
                        }
                        xmlRe += "</Articles><FuncFlag>0</FuncFlag></xml>";
                    }
                }
                CheckoutActiveToQYH(requestXML);

            }
            else if (requestXML.WXEVENT.ToLower() == "location")
            {
                SaveLocation(requestXML);//只有自动上报的才添加到地理位置表

            }
            else if (requestXML.WXEVENT.ToLower() == "subscribe")//关注
            {

                if (!string.IsNullOrEmpty(requestXML.EVENTKEY))//扫描带参数的二维码(未关注)
                {

                    if (int.Parse(requestXML.EVENTKEY.Substring(8)) > 10000)
                    {

                        try
                        {
                            Emp_Cust e = _wechat.GetEmp_Cust(requestXML.FROMUSERNAME.ToString());
                            WriteTxt(requestXML.FROMUSERNAME.ToString());
                            if (e == null)
                            {
                                EMPLOYEE em = _wechat.GetEmpByEwmId(requestXML.EVENTKEY.Substring(8));
                                if (em != null)
                                {
                                    InsertFS(requestXML.FROMUSERNAME, requestXML.TOUSERNAME, 1);  //插入粉丝信息
                                    Emp_Cust ec = new Emp_Cust();
                                    ec.EmpId = int.Parse(requestXML.EVENTKEY.Substring(8));
                                    ec.FromUserName = requestXML.FROMUSERNAME;
                                    ec.CreateTime = DateTime.Now;
                                    ec.Mobile = em.USERID;
                                    _wechat.AddEmp_Cust(ec);
                                    //上传素材
                                    int id = UploadImgForWx(em.NAME);
                                    FILES fileEntity = _mservice.GetUploadFile(id);
                                    string url = string.Format("http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", new WeiPage().Token(requestXML.TOUSERNAME), "image");
                                    string json = Utility.HttpUploadFile(url, fileEntity.FILE_NAME);
                                    mediainfo media = JsonConvert.DeserializeObject<mediainfo>(json);
                                    //客服消息
                                    var postUrl = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + new WeiPage().Token(requestXML.TOUSERNAME);
                                    string d = "{\"touser\":\"" + requestXML.FROMUSERNAME + "\",\"msgtype\":\"text\",\"text\":{\"content\":\"我是“" + em.NAME + "”, \" }}";
                                    var resMessage = HttpRequestHelper.HttpXmlPostRequest(postUrl, d, Encoding.UTF8);
                                    kfinfo r = JsonConvert.DeserializeObject<kfinfo>(resMessage);

                                    if (r.errcode == "0")
                                    {
                                        xmlRe += "<MsgType><![CDATA[image]]></MsgType><Image><MediaId><![CDATA[" + media.media_id + "]]></MediaId></Image></xml>";
                                    }

                                }

                            }
                        }
                        catch (Exception e)
                        {

                            _log.Error("emp:" + e.Message);
                        }

                    }
                    //临时二维码
                    else
                    {
                        string ar_id = requestXML.EVENTKEY.Substring(8);
                        string responseStr = this.ResponseTempQrCode(requestXML.FROMUSERNAME, ar_id);
                        xmlRe += responseStr;
                        //AR_QR_FANS ar = _wechat.QueryArInfoByArId(int.Parse(ar_id));
                        //ar.OPENID = requestXML.FROMUSERNAME;
                        //if (_wechat.SaveArInfo(ar) > 0)
                        //{
                        //    xmlRe += "<MsgType><![CDATA[image]]></MsgType><Image><MediaId><![CDATA[" + ar.MEDIA_ID + "]]></MediaId></Image></xml>";

                        //}
                    }

                }
                else
                {
                    //重新关注
                    WXCUST_FANS fans = _wechat.GetFansByFromUserName(requestXML.FROMUSERNAME);
                    if (fans != null)
                    {
                        xmlRe += "<MsgType><![CDATA[text]]></MsgType><Content><![CDATA[感恩和你再次相遇，盛时表行和盛时网将会为您心爱的手表提供更多服务，绑定老会员，享受电子优惠券，售后服务，手表关怀等钟表管家专属服务，敬请期待！]]></Content><FuncFlag>0</FuncFlag></xml>";
                        _wechat.UpdateCustInfoBySms(requestXML.FROMUSERNAME);
                    }
                    else
                    {
                        List<Information_EX> info = _wechat.GetModelListByKeywordList(2, "", requestXML.TOUSERNAME);
                        if (info.Count > 0)
                        {
                            xmlRe = "<MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + info[0].Content + "]]></Content><FuncFlag>0</FuncFlag></xml>";
                        }
                    }
                }
                CheckoutActiveToQYH(requestXML);
                InsertFS(requestXML.FROMUSERNAME, requestXML.TOUSERNAME, 1);  //插入粉丝信息


            }
            else if (requestXML.WXEVENT.ToLower() == "unsubscribe")//取消关注
            {
                //取消关注 把粉丝表更新状态为0
                InsertFS(requestXML.FROMUSERNAME, requestXML.TOUSERNAME, 0);  //插入粉丝信息

            }
            else if (requestXML.WXEVENT.ToLower() == "scan")
            {

                if (!string.IsNullOrEmpty(requestXML.EVENTKEY))
                {
                    if (int.Parse(requestXML.EVENTKEY) > 10000) //扫描带参数的二维码获取验证码(已关注)
                    {
                        EMPLOYEE em = _wechat.GetEmpByEwmId(requestXML.EVENTKEY);
                        if (em != null)
                        {
                            Emp_Cust e = _wechat.GetEmp_Cust(requestXML.FROMUSERNAME.ToString());

                            WriteTxt(requestXML.FROMUSERNAME.ToString());
                            if (e == null)
                            {
                                Emp_Cust ec = new Emp_Cust();
                                ec.EmpId = int.Parse(requestXML.EVENTKEY);
                                ec.FromUserName = requestXML.FROMUSERNAME;
                                ec.CreateTime = DateTime.Now;
                                ec.Mobile = em.USERID;
                                _wechat.AddEmp_Cust(ec);


                                //上传素材
                                int id = UploadImgForWx(em.NAME);
                                FILES fileEntity = _mservice.GetUploadFile(id);
                                string url = string.Format("http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", new WeiPage().Token(requestXML.TOUSERNAME), "image");
                                string json = Utility.HttpUploadFile(url, fileEntity.FILE_NAME);
                                mediainfo media = JsonConvert.DeserializeObject<mediainfo>(json);
                                //客服消息
                                var postUrl = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + new WeiPage().Token(requestXML.TOUSERNAME);
                                string d = "{\"touser\":\"" + requestXML.FROMUSERNAME + "\",\"msgtype\":\"text\",\"text\":{\"content\":\"我是“" + em.NAME + "”\" }}";
                                var resMessage = HttpRequestHelper.HttpXmlPostRequest(postUrl, d, Encoding.UTF8);
                                kfinfo r = JsonConvert.DeserializeObject<kfinfo>(resMessage);
                                if (r.errcode == "0")
                                {
                                    xmlRe += "<MsgType><![CDATA[image]]></MsgType><Image><MediaId><![CDATA[" + media.media_id + "]]></MediaId></Image></xml>";
                                }
                            }
                            else
                            {
                                //已经绑定的发送文字提示
                                EMPLOYEE myGuanJia = _wechat.GetEmpByEwmId(e.EmpId.ToString());
                                if (myGuanJia != null)
                                {
                                    string content = "您已经绑定“" + myGuanJia.NAME + "”";
                                    xmlRe += "<MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + content + "]]></Content><FuncFlag>0</FuncFlag></xml>";
                                }
                            }
                        }
                        else
                        {

                            _log.Info(string.Format("不存在的EwmID：{0}", requestXML.EVENTKEY));
                        }
                    }
                    //临时二维码
                    else
                    {
                        string ar_id = requestXML.EVENTKEY;

                        string responseStr = this.ResponseTempQrCode(requestXML.FROMUSERNAME, ar_id);
                        xmlRe += responseStr;
                        //AR_QR_FANS ar = _wechat.QueryArInfoByArId(int.Parse(ar_id));
                        //ar.OPENID = requestXML.FROMUSERNAME;
                        //if (_wechat.SaveArInfo(ar) > 0)
                        //{
                        //    xmlRe += "<MsgType><![CDATA[image]]></MsgType><Image><MediaId><![CDATA[" + ar.MEDIA_ID + "]]></MediaId></Image></xml>";

                        //}
                    }

                }
                InsertFS(requestXML.FROMUSERNAME, requestXML.TOUSERNAME, 1);  //插入粉丝信息
                CheckoutActiveToQYH(requestXML);
            }
            else if (requestXML.WXEVENT.ToLower() == "view")//点击菜单跳转链接时的事件推送
            {
                CheckoutActiveToQYH(requestXML);
            }
            else if (requestXML.WXEVENT.ToLower() == "templatesendjobfinish")//模板消息推送
            {
            }
            else
            {

            }
            return xmlRe;
        }

        #endregion

        #region 画图

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
        /// 绘制文本自动换行（超出截断）
        /// </summary>
        /// <param name=\"graphic\">绘图图面</param>
        /// <param name=\"font\">字体</param>
        /// <param name=\"text\">文本</param>
        /// <param name=\"recangle\">绘制范围</param>
        private void DrawStringWrap(Graphics graphic, Font font, string text, Rectangle recangle)
        {
            List<string> textRows = GetStringRows(graphic, font, text, recangle.Width);
            int rowHeight = (int)(Math.Ceiling(graphic.MeasureString("测试", font).Height));

            int maxRowCount = recangle.Height / rowHeight;
            int drawRowCount = (maxRowCount < textRows.Count) ? maxRowCount : textRows.Count;

            int top = (recangle.Height - rowHeight * drawRowCount) / 2;

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Center;

            for (int i = 0; i < drawRowCount; i++)
            {
                PointF f = new PointF();
                f.X = 1;
                Rectangle fontRectanle = new Rectangle(recangle.Left, top + rowHeight * i, recangle.Width, rowHeight);
                // graphic.DrawString(textRows, font, new SolidBrush(Color.Black), f, sf);
            }

        }

        private List<string> GetStringRows(Graphics graphic, Font font, string text, int width)
        {
            int RowBeginIndex = 0;
            int rowEndIndex = 0;
            int textLength = text.Length;
            List<string> textRows = new List<string>();

            for (int index = 0; index < textLength; index++)
            {
                rowEndIndex = index;

                if (index == textLength - 1)
                {
                    textRows.Add(text.Substring(RowBeginIndex));
                }
                else if (rowEndIndex + 1 < text.Length && text.Substring(rowEndIndex, 2) == "\\r\\n")
                {
                    textRows.Add(text.Substring(RowBeginIndex, rowEndIndex - RowBeginIndex));
                    rowEndIndex = index += 2;
                    RowBeginIndex = rowEndIndex;
                }
                else if (graphic.MeasureString(text.Substring(RowBeginIndex, rowEndIndex - RowBeginIndex + 1), font).Width > width)
                {
                    textRows.Add(text.Substring(RowBeginIndex, rowEndIndex - RowBeginIndex));
                    RowBeginIndex = rowEndIndex;
                }
            }

            return textRows;
        }
        #endregion

        #region 功能

        //微信验证
        private bool CheckSignature(string signature, string timestamp, string stringnonce)
        {
            string[] ArrTmp = { Token, timestamp, stringnonce };
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

        //处理文本消息
        private string SetTestInformation(WXCUST_MSG_HIS requestXML)
        {
            List<Information_EX> list = _wechat.GetModelListByKeywordList(1, requestXML.CONTENT, requestXML.TOUSERNAME);
            string resxml = "";
            int count = list.Count;
            if (list.Count == 1 || (list.Count > 1 && list[0].MsgType != "news"))
            {
                Information_EX f = list[0];

                if (f != null)
                {

                    resxml += SetInfomation(f.MsgType, f.Content, f.Description, f.Title, f.PicUrl, requestXML.TOUSERNAME, requestXML.FROMUSERNAME, f.FulltextUrl, f.DID, f.IsURL);

                }
            }
            else if (list.Count > 1) //多图
            {
                int count2 = list.Count;
                resxml += "<MsgType><![CDATA[news]]></MsgType><ArticleCount>" + count + "</ArticleCount><Articles>";
                for (int z = 0; z < count2; z++)
                {
                    resxml += "<item><Title><![CDATA[" + list[z].Title + "]]></Title><Description><![CDATA[" + list[z].Description + "]]></Description><PicUrl><![CDATA[" + AppConfig.CurrentUri + list[z].PicUrl + "]]></PicUrl><Url><![CDATA[" + (list[z].IsURL ? (list[z].FulltextUrl.Contains("?id") ? (list[z].FulltextUrl + "&FromUserName=" + requestXML.FROMUSERNAME + "&ToUserName=" + requestXML.TOUSERNAME + "") : (list[z].FulltextUrl + "?FromUserName=" + requestXML.FROMUSERNAME + "&ToUserName=" + requestXML.TOUSERNAME + "")) : (AppConfig.CurrentUri + "/Admin/GraphicDisplay?id=" + list[z].DID)) + "]]></Url></item>";//URL是点击之后跳转去那里，这里跳转到百度
                }
                resxml += "</Articles><FuncFlag>0</FuncFlag></xml>";
            }
            else
            {
                list = _wechat.GetModelListByKeywordList(3, "", requestXML.TOUSERNAME);
                if (list.Count == 1 || (list.Count > 1 && list[0].MsgType != "news"))
                {
                    Information_EX f = list[0];

                    if (f != null)
                    {

                        resxml += SetInfomation(f.MsgType, f.Content, f.Description, f.Title, f.PicUrl, requestXML.TOUSERNAME, requestXML.FROMUSERNAME, f.FulltextUrl, f.DID, f.IsURL);

                    }
                }
                else if (list.Count > 1) //多图
                {
                    int count2 = list.Count;
                    resxml += "<MsgType><![CDATA[news]]></MsgType><ArticleCount>" + count + "</ArticleCount><Articles>";
                    for (int z = 0; z < count2; z++)
                    {
                        resxml += "<item><Title><![CDATA[" + list[z].Title + "]]></Title><Description><![CDATA[" + list[z].Description + "]]></Description><PicUrl><![CDATA[" + AppConfig.CurrentUri + list[z].PicUrl + "]]></PicUrl><Url><![CDATA[" + (list[z].IsURL ? (list[z].FulltextUrl.Contains("?id") ? (list[z].FulltextUrl + "&FromUserName=" + requestXML.FROMUSERNAME + "&ToUserName=" + requestXML.TOUSERNAME + "") : (list[z].FulltextUrl + "?FromUserName=" + requestXML.FROMUSERNAME + "&ToUserName=" + requestXML.TOUSERNAME + "")) : (AppConfig.CurrentUri + "/Admin/GraphicDisplay?id=" + list[z].DID)) + "]]></Url></item>";//URL是点击之后跳转去那里，这里跳转到百度
                    }
                    resxml += "</Articles><FuncFlag>0</FuncFlag></xml>";
                }
            }
            return resxml;
        }

        //设置(判断回复的是文本,图文,音乐)
        private string SetInfomation(string MsgType, string Content, string Description, string Title, string PicUrl, string ToUserName, string FromUsername, string FulltextUrl, int DID, bool IsURL)
        {
            string xmlRe = "";
            switch (MsgType)
            {
                case "text":
                    xmlRe += "<MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + Content + "]]></Content><FuncFlag>0</FuncFlag></xml>";
                    break;
                case "news":
                    xmlRe += "<MsgType><![CDATA[news]]></MsgType><ArticleCount>1</ArticleCount><Articles>";
                    xmlRe += "<item><Title><![CDATA[" + Title + "]]></Title><Description><![CDATA[" + Description + "]]></Description><PicUrl><![CDATA[" + AppConfig.CurrentUri + PicUrl + "]]></PicUrl><Url><![CDATA[" + (IsURL ? (FulltextUrl.Contains("?id") ? (FulltextUrl + "&FromUserName=" + FromUsername + "&ToUserName=" + ToUserName + "") : (FulltextUrl + "?FromUserName=" + FromUsername + "&ToUserName=" + ToUserName + "")) : (AppConfig.CurrentUri + "/Admin/GraphicDisplay?id=" + DID)) + "]]></Url></item>";//URL是点击之后跳转去那里，这里跳转到百度
                    xmlRe += "</Articles><FuncFlag>0</FuncFlag></xml>";
                    break;
                case "music":
                    xmlRe += string.Format(@"<MsgType><![CDATA[music]]></MsgType>
                                 <Music>
                                 <Title><![CDATA[{0}]]></Title>
                                 <Description><![CDATA[{1}]]></Description>
                                 <MusicUrl><![CDATA[{2}]]></MusicUrl>
                                 <HQMusicUrl><![CDATA[{2}]]></HQMusicUrl>
                                 </Music>
                                 <FuncFlag>0</FuncFlag>
                                 </xml>", Title, Description, AppConfig.CurrentUri + PicUrl);
                    break;
                default:
                    //resxml += "<MsgType><![CDATA[text]]></MsgType><Content><![CDATA[感谢您的支持]]></Content><FuncFlag>0</FuncFlag></xml>";
                    break;
            }
            return xmlRe;
        }

        //unix时间转换为datetime
        private DateTime UnixTimeToTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        //datetime转换为unixtime
        private int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        //保存地理位置
        public int SaveLocation(WXCUST_MSG_HIS requestXML)
        {
            WD_Location info = new WD_Location();
            info.accuracy = requestXML.SCALE;
            info.Createdate = DateTime.Now;
            info.FromUserName = requestXML.FROMUSERNAME;
            info.latitude = requestXML.LOCATION_X;
            info.longitude = requestXML.LOCATION_Y;
            info.speed = "0";
            info.ToUserName = requestXML.TOUSERNAME;
            if (requestXML.WXEVENT.ToLower() == "location") //如果是自动上报的地理位置，调换X/Y轴，因为赋值的时候赋错了
            {
                info.latitude = requestXML.LOCATION_Y;
                info.longitude = requestXML.LOCATION_X;
            }
            GetBaiDuMap(ref info);
            return _wechat.SaveLocation(info);
        }

        //谷歌坐标转换百度坐标
        public void GetBaiDuMap(ref WD_Location l)
        {
            try
            {
                string url = "http://api.map.baidu.com/geoconv/v1/?coords=" + l.longitude + "," + l.latitude + "&from=3&to=5&ak=42095ab67452cfefd9b5d3743d197f49";
                string message = HttpRequestHelper.HttpGetRequest(url);
                baidumap m = JsonConvert.DeserializeObject<baidumap>(message);
                if (m.status == 0)
                {
                    l.BaiduX = m.result[0].x.ToString();
                    l.BaiduY = m.result[0].y.ToString();
                }
            }
            catch (Exception)
            {

            }
        }

        //插入粉丝信息
        private int InsertFS(string FromUserName, string ToUserName, int state)
        {
            //获取Token字符串
            string access_token = ba.Token(ToUserName);
            //获取用户信息列表
            string info = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + access_token + "&openid=" + FromUserName + "&lang=zh_CN";
            string infomes = HttpRequestHelper.HttpGetRequest(info);
            OpenInfo winfo = JsonConvert.DeserializeObject<OpenInfo>(infomes);
            WXCUST_FANS fans = new WXCUST_FANS();
            //把用户的信息存到粉丝表,保存之前先判断粉丝表里面有没有这个用户
            WXCUST_FANS fan = _wechat.GetFansByFromUserName(FromUserName);
            //未关注时更新状态
            if (fan != null)
            {
                fans = fan;
                if (state == 1) //新增
                {
                    fans.NAME = winfo.nickname;
                    fans.GENDER = winfo.sex == "1" ? true : false;
                    fans.COUNTRY = winfo.country;
                    fans.PROVINCE = winfo.province;
                    fans.CITY = winfo.city;
                    fans.FROMUSERNAME = winfo.openid;
                    fans.ToUserName = ToUserName;
                    fans.IMAGE = winfo.headimgurl;
                    fans.STATUS = state;
                    fans.NOTICE_DATE = UnixTimeToTime(winfo.subscribe_time);
                    fans.CANCEL_DATE = DateTime.Now;
                    //cfan.CREATE_DATE = DateTime.Now;
                    fans.CREATE_USER = "system";
                    fans.LAST_CONN_DATE = DateTime.Parse("1900-01-01");
                    fans.LAST_MODI_DATE = DateTime.Parse("1900-01-01");
                    fans.REMARK = "";
                    fans.LAST_MODI_USER = "system";
                }
                else  //取消关注
                {
                    fans.STATUS = state;
                    fans.CANCEL_DATE = DateTime.Now;
                }
                return _wechat.SaveCustFans(fans);

            }
            else
            {
                fans = new WXCUST_FANS();
                fans.NAME = winfo.nickname;
                fans.GENDER = winfo.sex == "1" ? true : false;
                fans.COUNTRY = winfo.country;
                fans.PROVINCE = winfo.province;
                fans.CITY = winfo.city;
                fans.FROMUSERNAME = winfo.openid;
                fans.ToUserName = ToUserName;
                fans.IMAGE = winfo.headimgurl;
                fans.STATUS = 1;
                fans.NOTICE_DATE = UnixTimeToTime(winfo.subscribe_time);
                fans.CANCEL_DATE = DateTime.Parse("1900-01-01");
                fans.CREATE_DATE = DateTime.Now;
                fans.CREATE_USER = "system";
                fans.LAST_CONN_DATE = DateTime.Parse("1900-01-01");
                fans.LAST_MODI_DATE = DateTime.Parse("1900-01-01");
                fans.REMARK = "";
                fans.LAST_MODI_USER = "system";
                fans.AVAL_OPPR = 1;
                fans.TOTAL_OPPR = 1;
                fans.UNIN_CODE = "";
                fans.REFE_CODE = "";
                return _wechat.SaveCustFans(fans);
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
            try
            {
                //新接口
                string data = string.Format("openId={0}&content={1}&type={2}", openId, content, type);
                NetHelper.HttpRequest(AppConfig.QYHMsgUrl, data, WeChatCRM.Common.Utils.HttpVerbs.POST, 6000, Encoding.UTF8, ContentTypes.FORM);

                #region 同时往测试环境通知消息，如不需要可以去掉
                NetHelper.HttpRequest("http://testqyh.censh.cn/censh/dialogue/receiver", data, WeChatCRM.Common.Utils.HttpVerbs.POST, 6000, Encoding.UTF8, ContentTypes.FORM);
                #endregion
                //老的接口
                //NetHelper.HttpRequest(AppConfig.QYHMsgUrl, "con=" + postStr, WeChatCRM.Common.Utils.HttpVerbs.POST, 6000, Encoding.UTF8, ContentTypes.FORM);
            }
            catch (Exception ex)
            {
                _log.Error("转发消息：" + ex.Message);
            }

        }

        private string ResponseTempQrCode(string fromUserName, string ar_id)
        {
            AR_QR_FANS ar = _wechat.QueryArInfoByArId(int.Parse(ar_id));
            ar.OPENID = fromUserName;
            if (_wechat.SaveArInfo(ar) > 0)
            {
                return "<MsgType><![CDATA[image]]></MsgType><Image><MediaId><![CDATA[" + ar.MEDIA_ID + "]]></MediaId></Image></xml>";

            }
            return "";
        }

        /// <summary>
        /// 用于企业号检查是否活跃
        /// </summary>
        /// <param name="requestXML"></param>
        private void CheckoutActiveToQYH(WXCUST_MSG_HIS requestXML)
        {
            try
            {
                var cpresponse = HttpRequestHelper.HttpGetRequest(AppConfig.WXCorpfwh + requestXML.FROMUSERNAME);

            }
            catch (Exception ex)
            {
                _log.Error("活跃度异常：" + ex.Message);
            }
        }
        //管家绑定会员微信消息提醒(暂时不用)
        private string EmpBindMsg(WXCUST_MSG_HIS requestXML)
        {
            string xmlRe = "";
            try
            {

                Emp_Cust e = _wechat.GetEmp_Cust(requestXML.FROMUSERNAME.ToString());
                WriteTxt(requestXML.FROMUSERNAME.ToString());

                if (e == null)
                {

                    EMPLOYEE em = _wechat.GetEmpByEwmId(requestXML.EVENTKEY.Substring(8));
                    if (em != null)
                    {

                        Emp_Cust ec = new Emp_Cust();
                        ec.EmpId = int.Parse(requestXML.EVENTKEY.Substring(8));
                        ec.FromUserName = requestXML.FROMUSERNAME;
                        ec.CreateTime = DateTime.Now;
                        ec.Mobile = em.MOBILE;
                        _wechat.AddEmp_Cust(ec);


                        //上传素材
                        int id = UploadImgForWx(em.NAME);
                        FILES fileEntity = _mservice.GetUploadFile(id);
                        string url = string.Format("http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", new WeiPage().Token(requestXML.TOUSERNAME), "image");
                        string json = Utility.HttpUploadFile(url, fileEntity.FILE_NAME);
                        mediainfo media = JsonConvert.DeserializeObject<mediainfo>(json);
                        //客服消息
                        var postCustomerUrl = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + new WeiPage().Token(requestXML.TOUSERNAME);
                        string data = "{\"touser\":\"" + requestXML.FROMUSERNAME + "\",\"msgtype\":\"text\",\"text\":{\"content\":\"我是“" + em.NAME + "”\" }}";
                        var resMessages = HttpRequestHelper.HttpXmlPostRequest(postCustomerUrl, data, Encoding.UTF8);
                        kfinfo rs = JsonConvert.DeserializeObject<kfinfo>(resMessages);
                        if (rs.errcode == "0")
                        {

                            xmlRe += "<MsgType><![CDATA[image]]></MsgType><Image><MediaId><![CDATA[" + media.media_id + "]]></MediaId></Image></xml>";
                        }

                    }
                }
            }
            catch (Exception e)
            {

                _log.Error("saoma:" + e.Message);
            }
            return xmlRe;
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
                _wechat.AddLog(l);
            }
            catch (Exception)
            {
                return false;
            }
            return true;

        }
        #endregion
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
        public int msgid { get; set; }
    }
}
