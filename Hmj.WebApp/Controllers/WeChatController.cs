using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Entity.Jsons;
using Hmj.Entity.PageSearch;
using Hmj.Entity.SearchEntity;
using Hmj.Extension;
using Hmj.Interface;
using Hmj.WebApp.WechatApiHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WeChatCRM.Common.Utils;

namespace WeChat.WebApp.Controllers
{
    public class WeChatController : WXMyControllerBase
    {
        private IWXMessageRecordService _wxservice;
        private ISystemService _service;
        private ICommonService _mservice;
        public WeChatController(ISystemService service, ICommonService mservice, IWXMessageRecordService wxservice)
        {
            _mservice = mservice;
            _service = service;
            _wxservice = wxservice;
        }
        //
        // GET: /WeChat/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetLocation()
        {
            return View();
        }

        public ActionResult GoodList()
        {
            return View();
        }

        public ActionResult GoodEdit(int id)
        {
            if (id == 0)
                return View(new TT_Detail { ID = 0 });
            return View(_service.GetTT_Detail(id));
        }

        [HttpPost]
        public JsonResult GetGoodsList(GraphicSearch search, FormCollection form)
        {
             
            PageView view = new PageView(form);
            if (search == null)
            {
                search = new GraphicSearch();
            }
            search.Merchants_ID = CurrentLoginUser.ORG_ID;  //商户id
            string colkey = form["colkey"];
            string colsinfo = form["colsinfo"];
            PagedList<TT_Detail> pList = _service.QueryGoodsList(search, view);
            JsonQTable fdata = JsonQTable.ConvertFromPagedList<TT_Detail>(pList, colkey, colsinfo.Split(','));
            
            return Json(fdata);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveGood(TT_Detail CuObj, FormCollection form)
        {
            JsonSMsg rMsg = new JsonSMsg();
            try
            {
                rMsg.Status = 0;
                CuObj.CreateTime = DateTime.Now;
                _service.SaveTT_Detail(CuObj);

            }
            catch (Exception ex)
            {
                rMsg.Status = -1;
                rMsg.Message = ex.Message;
            }
            return Json(rMsg);
        }

        #region  实时消息

        [MyAuthorize]
        public ActionResult CustMsgHis()
        {
            return View(new CUST_MSG_RECORD_EX());
        }
        [MyAuthorize]
        public ActionResult CustMsgHisN()
        {
            return View(new CUST_MSG_RECORD_EX());
        }
        [HttpPost]
        public JsonResult QueryFansMessages(FormCollection form)
        {
            PageView view = new PageView(form);
            string colkey = form["colkey"];
            string colsinfo = form["colsinfo"];

            FansMessageSearch fms = new FansMessageSearch();
            fms.ToUserName = WXMyContext.CurrentMerchants.ToUserName;
            if (!string.IsNullOrEmpty(form["SEARCHTYPE"]))
            {
                switch (form["SEARCHTYPE"])
                {
                    case "0":
                        break;
                    case "1":
                        fms.IsReturn = false;
                        break;
                    case "2":
                        fms.IsReturn = true;
                        break;
                    case "3":
                        fms.IsStar = true;
                        break;
                }
            }

            if (!string.IsNullOrEmpty(form["SearchText"]))
            {
                fms.SearchText = form["SearchText"];
            }

            PagedList<CUST_MSG_RECORD_EX> pList = _wxservice.QueryFansMessages(view, fms);
            JsonQTable fdata = JsonQTable.ConvertFromPagedList<CUST_MSG_RECORD_EX>(pList, colkey, colsinfo.Split(','));
            return Json(fdata);
        }


        [HttpPost]
        public JsonResult QueryFansMessagesN(FormCollection form)
        {
            PageView view = new PageView(form);
            string colkey = form["colkey"];
            string colsinfo = form["colsinfo"];

            FansMessageSearch fms = new FansMessageSearch();
            fms.ToUserName = WXMyContext.CurrentMerchants.ToUserName;
            if (!string.IsNullOrEmpty(form["SEARCHTYPE"]))
            {
                switch (form["SEARCHTYPE"])
                {
                    case "0":
                        break;
                    case "1":
                        fms.IsReturn = false;
                        break;
                    case "2":
                        fms.IsReturn = true;
                        break;
                    case "3":
                        fms.IsStar = true;
                        break;
                }
            }

            if (!string.IsNullOrEmpty(form["SearchText"]))
            {
                fms.SearchText = form["SearchText"];
            }

            PagedList<CUST_MSG_RECORD_EX> pList = _wxservice.QueryFansMessagesN(view, fms);
            JsonQTable fdata = JsonQTable.ConvertFromPagedList<CUST_MSG_RECORD_EX>(pList, colkey, colsinfo.Split(','));
            return Json(fdata);
        }

        [HttpPost]
        public JsonResult QueryReplyMessages(int id)
        {
            JsonSMsg rMsg = new JsonSMsg();
            try
            {
                var messages = _wxservice.QueryReplyMessages(id);
                rMsg.Status = 0;
                rMsg.Message = "success";
                rMsg.Data = messages;
            }
            catch
            {
                rMsg.Status = -1;
                rMsg.Message = "fail";
            }
            return Json(rMsg);
        }

        [HttpPost]
        public JsonResult UpdateFanMessages(int id, CUST_MSG_RECORD_EX fanMessage)
        {
            JsonSMsg rMsg = new JsonSMsg();
            try
            {
                if (id <= 0)
                {
                    rMsg.Status = -1;
                    rMsg.Message = "参数错误！";
                    return Json(rMsg);
                }
                int ret = _wxservice.UpdateFansMessageIsStar(fanMessage);
                if (ret > 0)
                {
                    rMsg.Status = 1;
                    rMsg.Data = ret;
                }
                else
                {
                    rMsg.Status = -1;
                    rMsg.Message = "意外错误，请稍后重试";
                }
            }
            catch (Exception ex)
            {
                rMsg.Status = -1;
                rMsg.Message = ex.Message;
            }
            return Json(rMsg);
        }

        [HttpPost]
        public JsonResult ReplyMessage(CUST_MSG_RECORD_EX replyMessage)
        {
            JsonSMsg rMsg = new JsonSMsg();
            try
            {
                var fansMessage = _wxservice.GetMessage(replyMessage.ReturnID.Value);
                SendMessageApi sma = new SendMessageApi();
                var result = sma.SendMessage(fansMessage.FROMUSERNAME, replyMessage.CONTENT);
                rMsg.Status = result.errcode;
                rMsg.Message = result.errmsg;
                //hb 2014.12.17
                if (result.errmsg == "ok")
                {
                    replyMessage.CONTENT += "[已发送]";
                    rMsg.Status = 0;
                    rMsg.Message = "保存成功";
                }
                else if (result.errcode.ToString() == "45015")
                {
                    replyMessage.CONTENT += "[发送失败:" + result.errcode + "]";
                    rMsg.Status = -1;
                    rMsg.Message = "由于长时间未互动，发送失败！";
                }
                else if (result.errcode.ToString().Contains("4"))
                {
                    replyMessage.CONTENT += "[发送失败:" + result.errcode + "]";
                    rMsg.Status = -1;
                    rMsg.Message = "由于未知错误，发送失败！";
                }
                //hb 2014.12.17
                //if (result.errcode == 0)  //微信发送消息成功
                //{
                var num = _wxservice.ReplyMessage(fansMessage, replyMessage.CONTENT);
                if (num <= 0)
                {
                    rMsg.Status = -1;
                    rMsg.Message = "数据库保存失败";
                }
                //}
            }
            catch (Exception ex)
            {
                rMsg.Status = -1;
                rMsg.Message = ex.ToString();
            }
            return Json(rMsg);
        }

        #endregion

        public ActionResult GraphicList()
        {
            return View(new Graphic_Detail_EX() { ID = 0, List_ID = 0 });
        }

        //列表
        [HttpPost]
        public JsonResult GraphicDataList(GraphicSearch search, FormCollection form)
        {
            PageView view = new PageView(form);
            if (search == null)
            {
                search = new GraphicSearch();
            }
            search.Merchants_ID = CurrentLoginUser.ORG_ID;  //商户id
            string colkey = form["colkey"];
            string colsinfo = form["colsinfo"];
            PagedList<Graphic_Detail_EX> pList = _service.QueryGetGraphicDetail(search, view);
            JsonQTable fdata = JsonQTable.ConvertFromPagedList<Graphic_Detail_EX>(pList, colkey, colsinfo.Split(','));
            return Json(fdata);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="CuObj"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveGraphic(WXGraphicDetail CuObj, FormCollection form)
        {
            JsonSMsg rMsg = new JsonSMsg();
            try
            {

                if (Request.Files.Count > 0 && (Request.Files[0].InputStream.Length > 0 || Request["Pic"].Trim() != ""))
                {
                    HttpPostedFileBase file = Request.Files[0];
                    string Extension = Path.GetExtension(file.FileName).ToLower();

                    byte[] data = new byte[file.InputStream.Length];
                    file.InputStream.Read(data, 0, data.Length);
                    string url = Url.Action("../home/ViewImage");
                    string remark = Request["filename"];
                    FILES fileEntity = _mservice.UploadFile(Extension, file.ContentType, data, url, remark);
                    WXGraphicList l = new Hmj.Entity.WXGraphicList();
                    l.CreateDate = DateTime.Now;
                    l.Merchants_ID = CurrentLoginUser.ORG_ID;
                    l.Title = "";// Request["Name"];
                    int lid = 0;
                    if (Request["List_ID"] == "0")
                        lid = _service.SaveGraphicList(l);
                    else
                        lid = int.Parse(Request["List_ID"]);
                    CuObj = new Hmj.Entity.WXGraphicDetail();
                    CuObj.ID = int.Parse(Request["ID"]);
                    CuObj.Body = Request["ckeditor"];
                    CuObj.Describe = Request["Describe"];
                    CuObj.IsURL = Request.Form["IsURL"] == null ? false : true;
                    CuObj.List_ID = lid;
                    CuObj.Pic = fileEntity.FILE_URL;
                    CuObj.Sorting = int.Parse(Request["Sorting"]);
                    CuObj.Title = Request["Title"];
                    CuObj.URL = Request["URL"];
                    _service.SaveGraphicDetail(CuObj);

                    //_mservice.InsertREL_CUST_FILES(cust);
                    //rMsg.Status = 0;
                    //rMsg.Message = "保存成功";
                    return Redirect("GraphicList.do");
                }
            }
            catch (Exception ex)
            {
                rMsg.Status = -1;
                rMsg.Message = ex.Message;
            }
            return Json(rMsg);
        }


        //修改       
        public ActionResult GraphicEdit(int? id)
        {
            Graphic_Detail_EX cuobj = null;
            var jmsg = new JsonSMsg();
            try
            {
                if (id == 0)
                    cuobj = new Graphic_Detail_EX() { ID = 0 };
                else
                {
                    cuobj = _service.GetGraphicDetail(int.Parse(id.ToString()));
                }

                jmsg.Status = 0;
                jmsg.Data = cuobj;
            }
            catch (Exception ex)
            {
                jmsg.Status = -1;
                jmsg.Message = ex.Message;
            }
            return Json(jmsg);
        }

        [MyAuthorize]
        public ActionResult Informations(int? replyType)
        {
            return View(new WXInformation() { ID = 0, Merchants_ID = CurrentLoginUser.ORG_ID, replyType = replyType });
        }

        [MyAuthorize]
        public ActionResult InformationsEdit(int? id)
        {
            string tuwen = "";
            List<WXGraphicList> list2 = _service.GetAllGraphicList(CurrentLoginUser.ORG_ID);
            foreach (WXGraphicList g in list2)
            {
                List<Graphic_Detail_EX> list = _service.GetAllGraphicDetail(g.ID);
                tuwen += "<li id='li" + g.ID + "' class='col-xs-4'>";
                for (int i = 0; i < list.Count; i++)
                {
                    if (i == 0)
                    {
                        tuwen += string.Format(@" <div class='tuwen'>
                 <div class='onetuwen'>
                     <div class='tuwenTitle'><a href='#'>{0}</a></div>
                     <div class='tuwenTime'>{3}</div>
                     <div class='tuwencon duotucon'>
                       <img src='{1}' onerror=""javascript:this.src='/assets/images/default.png';"">
                       <p>{2}</p>
                     </div>
                 </div>", list[i].Title, ConfigurationSettings.AppSettings["WebUrl"] + list[i].Pic, list[i].Title, list[i].CreateDate.ToString("MM月dd日"));
                    }
                    if (i == 1)
                    {
                        tuwen += " <div class='duotuwen'>";
                    }
                    if (i != 0)
                    {
                        tuwen += string.Format(@"<p><img src='{0}' width='80' height='80'  onerror=""javascript:this.src='/assets/images/default.png';""><span><a href='#'>{1}</a></span></p>", ConfigurationSettings.AppSettings["WebUrl"] + list[i].Pic, list[i].Title);
                    }
                }
                if (list.Count > 1)
                    tuwen += "</div>";

                tuwen += string.Format(@" <div class='hoverbg'></div>
             <bdo style='color:#ffffff'>{0}</bdo>", g.ID);
            }
            Information_EX wx = _service.GetInformation(id);
            if (wx == null)
            {
                wx = new Information_EX { ID = 0, Merchants_ID = CurrentMerchants.ID };
            }
            List<WXBiaoqing> listbq = null;
            if (Session["WXbiaoqing"] != null)
            {
                listbq = Session["WXbiaoqing"] as List<WXBiaoqing>;
            }
            else
            {
                listbq = _service.GetBQList();
            }
            string biaoqing = "";
            foreach (WXBiaoqing w in listbq)
            {
                biaoqing += @" <img style='margin:5px;'  onerror=""javascript:this.src='/assets/images/default.png';"" src='/assets/img/biaoqing/" + (w.ID - 1) + ".png' alt='" + w.Text + "' onclick='javascript:addbq(\"" + w.Text + "\"," + (w.ID - 1) + ")' onmouseout=\"hidephoto('" + (w.ID - 1) + "',this)\"  onmouseover=\"showphoto('" + (w.ID - 1) + "','" + w.Text + "',this)\" />";
            }
            wx.Biaoqing = biaoqing;

            wx.tuwen = tuwen;
            return View(wx);
        }

        /// <summary>
        /// 获取一个图文
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetTW(int? id)
        {
            List<Graphic_Detail_EX> list = _service.GetAllGraphicDetail(id);
            var jmsg = new JsonSMsg();
            try
            {
                string tuwen = "";
                if (list.Count > 0)
                {
                    tuwen += "<li id='lituwen' class='col-xs-4'>";
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (i == 0)
                        {
                            tuwen += string.Format(@" <div class='tuwen'>
                 <div class='onetuwen'>
                     <div class='tuwenTitle'><a href='{4}'   target='_blank'>{0}</a></div>
                     <div class='tuwenTime'>{3}</div>
                     <div class='tuwencon duotucon'>
                       <img src='{1}'  onerror=""javascript:this.src='/assets/images/default.png';"">
                       <p>{2}</p>
                     </div>
                 </div>", list[i].Title, ConfigurationSettings.AppSettings["WebUrl"] + list[i].Pic, list[i].Title, list[i].CreateDate.ToString("MM月dd日"), list[i].IsURL == true ? list[i].URL : (ConfigurationSettings.AppSettings["WebUrl"] + "/GraphicDisplay.aspx?id=" + list[i].ID));
                        }
                        if (i == 1)
                        {
                            tuwen += " <div class='duotuwen'>";
                        }
                        if (i != 0)
                        {
                            tuwen += string.Format(@"<p><img src='{0}' width='80' height='80'  onerror=""javascript:this.src='/assets/images/default.png';""><span><a href='{2}'  target='_blank'>{1}</a></span></p>", ConfigurationSettings.AppSettings["WebUrl"] + list[i].Pic, list[i].Title, list[i].IsURL == true ? list[i].URL : (ConfigurationSettings.AppSettings["WebUrl"] + "/GraphicDisplay.aspx?id=" + list[i].ID));
                        }
                    }
                    if (list.Count > 1)
                        tuwen += "</div>";
                    jmsg.Status = 0;
                    //cuobj[0].FullPicUrl = ConfigurationSettings.AppSettings["WebUrl"] + cuobj[0].Pic;
                    jmsg.Data = tuwen;
                }
                else
                {
                    jmsg.Status = -1;
                    jmsg.Data = "";
                }
            }
            catch (Exception ex)
            {
                jmsg.Status = -1;
                jmsg.Message = ex.Message;
            }
            return Json(jmsg);
        }

        //列表
        [HttpPost]
        public JsonResult InformationList(RoleSearch search, FormCollection form)
        {
            PageView view = new PageView(form);
            if (search == null)
            {
                search = new RoleSearch();
            }
            search.ORG_ID = base.CurrentLoginUser.ORG_ID;
            string colkey = form["colkey"];
            string colsinfo = form["colsinfo"];
            PagedList<Information_EX> pList = _service.QueryInformationList(search, view);
            JsonQTable fdata = JsonQTable.ConvertFromPagedList<Information_EX>(pList, colkey, colsinfo.Split(','));
            return Json(fdata);
        }

        //修改   
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult InformationEdit(int? id)
        {
            WXInformation cuobj = null;
            var jmsg = new JsonSMsg();
            try
            {
                if (id == 0)
                    cuobj = new WXInformation() { ID = 0 };
                else
                {
                    cuobj = _service.GetInformation(int.Parse(id.ToString()));
                }

                jmsg.Status = 0;
                jmsg.Data = cuobj;
            }
            catch (Exception ex)
            {
                jmsg.Status = -1;
                jmsg.Message = ex.Message;
            }
            return Json(jmsg);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="CuObj"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveInformation(WXInformation CuObj, FormCollection form)
        {
            JsonSMsg rMsg = new JsonSMsg();
            try
            {
                CuObj.CreateTime = DateTime.Now.ToString("yyyy-MM-dd");
                CuObj.Merchants_ID = CurrentLoginUser.ORG_ID;
                int num = _service.GetCountByreplyType(CuObj.ID, CuObj.replyType, CuObj.Merchants_ID);
                if (CuObj.replyType != 1 && num > 0)
                {
                    rMsg.Status = -1;
                    rMsg.Message = "被关注回复及自动回复只可添加一条数据";
                }
                else
                {
                    int ret = _service.SaveInformation(CuObj);
                    if (ret >= 0)
                    {
                        rMsg.Status = 0;
                        rMsg.Data = CuObj.ID;
                    }
                    else
                    {
                        rMsg.Status = -1;
                        rMsg.Message = "意外错误，请稍后重试";
                    }
                }
            }
            catch (Exception ex)
            {
                rMsg.Status = -1;
                rMsg.Message = ex.Message;
            }
            return Json(rMsg);
        }

        [MyAuthorize]
        public ActionResult FansList()
        {
            return View(new CUST_FANS_EX() { ID = 0 });
        }

        public ActionResult GetFansList(string openid, int nextcount, int ycount)
        {
            var jmsg = new JsonSMsg();
            try
            {
                int lastcount = nextcount;  //本次开始记录从上次结束记录开始
                string returnmes = "";
                BasePage bpage = new BasePage();
                string access_token = bpage.Token();
                string url = "https://api.weixin.qq.com/cgi-bin/user/get?access_token=" + access_token;
                if (openid != "")
                    url = "https://api.weixin.qq.com/cgi-bin/user/get?access_token=" + access_token + "&next_openid=" + openid;
                if (Session["returnmes"] != null)
                {
                    returnmes = Session["returnmes"].ToString();
                }
                else
                {
                    returnmes = PostRequestGet(url);
                    Session["returnmes"] = returnmes;//保存到session里，下次直接读取
                    nextcount = 0;//新的集合
                }
                Data data = JsonConvert.DeserializeObject<Data>(returnmes);
                nextcount += 20;
                ycount += 20;
                if (nextcount >= int.Parse(data.count))  //本次获取的记录结束
                {
                    nextcount = int.Parse(data.count);
                    Session["returnmes"] = null;
                }
                if (ycount > int.Parse(data.total))
                {
                    ycount = int.Parse(data.total); //总拉取条数
                }
                data.nextcount = nextcount.ToString();
                data.ycount = ycount.ToString();
                jmsg.Data = data;
                jmsg.Message = data.next_openid;
                if (int.Parse(data.total) <= ycount) //拉取完毕
                {
                    jmsg.Message = "";
                }
                WXCUST_FANS C = new WXCUST_FANS();
                if (data.data != null)
                {
                    for (int i = lastcount; i < nextcount; i++)
                    {
                        try
                        {
                            string info = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + access_token + "&openid=" + data.data.openid[i] + "&lang=zh_CN";
                            string infomes = PostRequestGet(info);
                            Info winfo = JsonConvert.DeserializeObject<Info>(infomes);

                            //把用户的信息存到粉丝表,保存之前先判断粉丝表里面有没有这个用户
                            WXCUST_FANS cfan = _service.GetFansByFromUserName(winfo.openid);
                            //未关注时更新状态
                            if (cfan != null)
                            {
                                cfan.NAME = winfo.nickname;
                                cfan.GENDER = winfo.sex == "1" ? true : false;
                                cfan.COUNTRY = winfo.country;
                                cfan.PROVINCE = winfo.province;
                                cfan.CITY = winfo.city;
                                cfan.FROMUSERNAME = winfo.openid;
                                cfan.ToUserName = WXMyContext.CurrentMerchants.ToUserName;
                                cfan.IMAGE = winfo.headimgurl;
                                cfan.STATUS = 1;
                                cfan.NOTICE_DATE = UnixTimeToTime(winfo.subscribe_time);
                                // cfan.CANCEL_DATE = DateTime.Parse("1900-01-01");
                                // cfan.CREATE_DATE = DateTime.Now;
                                cfan.CREATE_USER = "system";
                                cfan.LAST_CONN_DATE = DateTime.Now;
                                cfan.LAST_MODI_DATE = DateTime.Parse("1900-01-01");
                                cfan.REMARK = "";
                                cfan.LAST_MODI_USER = "system";
                                _service.UpdateFans(cfan);

                            }
                            else if (winfo.openid != null)
                            {
                                cfan = new WXCUST_FANS();
                                cfan.NAME = winfo.nickname;
                                cfan.GENDER = winfo.sex == "1" ? true : false;
                                cfan.COUNTRY = winfo.country;
                                cfan.PROVINCE = winfo.province;
                                cfan.CITY = winfo.city;
                                cfan.FROMUSERNAME = winfo.openid;
                                cfan.ToUserName = WXMyContext.CurrentMerchants.ToUserName; ;
                                cfan.IMAGE = winfo.headimgurl;
                                cfan.STATUS = 1;
                                cfan.NOTICE_DATE = UnixTimeToTime(winfo.subscribe_time);
                                cfan.CANCEL_DATE = DateTime.Parse("1900-01-01");
                                cfan.CREATE_DATE = DateTime.Now;
                                cfan.CREATE_USER = "system";
                                cfan.LAST_CONN_DATE = DateTime.Now;
                                cfan.LAST_MODI_DATE = DateTime.Parse("1900-01-01");
                                cfan.REMARK = "";
                                cfan.LAST_MODI_USER = "system";
                                cfan.AVAL_OPPR = 1;
                                cfan.TOTAL_OPPR = 1;
                                cfan.UNIN_CODE = "";
                                cfan.REFE_CODE = "";
                                _service.InsertFans(cfan);
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                    if (nextcount >= int.Parse(data.count)) //本次集合结束
                    {
                        nextcount = 0; //重置起点
                    }
                    jmsg.Status = 0;
                }
                else
                {
                    jmsg.Status = -1;
                    jmsg.Message = "拉取失败";
                }


                // jmsg.Data = cuobj;
            }
            catch (Exception ex)
            {
                jmsg.Status = -1;
                jmsg.Message = ex.Message;
            }
            return Json(jmsg);
        }


        //列表
        [HttpPost]
        public JsonResult GetFansLists(FansSearch search, FormCollection form)
        {
            PageView view = new PageView(form);
            if (search == null)
            {
                search = new FansSearch();
            }
            search.ToUserName = base.CurrentMerchants.ToUserName == null ? "" : CurrentMerchants.ToUserName;
            string colkey = form["colkey"];
            string colsinfo = form["colsinfo"];
            PagedList<CUST_FANS_EX> pList = _service.QueryGetFansDetail(search, view);
            JsonQTable fdata = JsonQTable.ConvertFromPagedList<CUST_FANS_EX>(pList, colkey, colsinfo.Split(','));
            return Json(fdata);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="CuObj"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveMessage(CUST_FANS_EX CuObj, FormCollection form)
        {
            JsonSMsg rMsg = new JsonSMsg();
            try
            {
                //CuObj.CreateTime = DateTime.Now.ToString("yyyy-MM-dd");
                //CuObj.Merchants_ID = CurrentLoginUser.Merchants_ID;
                //int num = _service.GetCountByreplyType(CuObj.ID, CuObj.replyType, CuObj.Merchants_ID);
                //if (CuObj.replyType != 1 && num > 0)
                //{
                //    rMsg.Status = -1;
                //    rMsg.Message = "被关注回复及自动回复只可添加一条数据";
                //}
                //else
                //{
                //    int ret = _service.SaveInformation(CuObj);
                //    if (ret >= 0)
                //    {
                //        rMsg.Status = 0;
                //        rMsg.Data = CuObj.ID;
                //    }
                //    else
                //    {
                //        rMsg.Status = -1;
                //        rMsg.Message = "意外错误，请稍后重试";
                //    }
                //}
            }
            catch (Exception ex)
            {
                rMsg.Status = -1;
                rMsg.Message = ex.Message;
            }
            return Json(rMsg);
        }

        //修改       
        public ActionResult FansEdit(int? id)
        {
            CUST_FANS_EX cuobj = null;
            var jmsg = new JsonSMsg();
            try
            {
                if (id == 0)
                    cuobj = new CUST_FANS_EX() { ID = 0 };
                else
                {
                    cuobj = _service.GetFansByID(int.Parse(id.ToString()));
                    if (cuobj != null)
                    {
                        cuobj.HTML = "<img id='imgphoto' src='" + cuobj.IMAGE + "' class='img-circle'  style='width:180px;height:180px;float:left;margin:20px;'/>";
                        cuobj.HTML += "<DIV style='float:left;width:180px;margin:20px;'>姓名：" + cuobj.XM;
                        cuobj.HTML += "<br>性别：" + cuobj.xb;
                        cuobj.HTML += "<br>手机：" + cuobj.Phone;
                        cuobj.HTML += "<br>会员类型：" + cuobj.MEM_LEVEL;
                        cuobj.HTML += "<br>生日：" + DateTime.Parse(cuobj.Birthday == null ? "1900-01-01" : cuobj.Birthday).ToString("yyyy.MM.dd");
                        cuobj.HTML += "<br>昵称：" + cuobj.NAME;
                        // cuobj.HTML += "<br>微信编号：" + cuobj.FROMUSERNAME;
                        cuobj.HTML += "<br>国籍：" + cuobj.COUNTRY;
                        cuobj.HTML += "<br>省份：" + cuobj.PROVINCE;
                        cuobj.HTML += "<br>城市：" + cuobj.CITY;
                        // cuobj.HTML += "<br>状态：" + cuobj.qx;

                        cuobj.HTML += "</DIV>";
                    }
                }

                jmsg.Status = 0;
                jmsg.Data = cuobj;
            }
            catch (Exception ex)
            {
                jmsg.Status = -1;
                jmsg.Message = ex.Message;
            }
            return Json(jmsg);
        }

        public ActionResult GraphicShow()
        {
            string tuwen = "";
            List<WXGraphicList> list2 = _service.GetAllGraphicList(CurrentLoginUser.ORG_ID);
            foreach (WXGraphicList g in list2)
            {
                List<Graphic_Detail_EX> list = _service.GetAllGraphicDetail(g.ID);
                tuwen += "<li id='li" + g.ID + "' class='col-xs-4'>";
                for (int i = 0; i < list.Count; i++)
                {
                    if (i == 0)
                    {
                        tuwen += string.Format(@" <div class='tuwen'>
                 <div class='onetuwen'>
                     <div class='tuwenTitle'><a href='{4}' target='_Blank'>{0}</a></div>
                     <div class='tuwenTime'>{3}</div>
                     <div class='tuwencon duotucon'>
                       <img src='{1}'  onerror=""javascript:this.src='/assets/images/default.png';"">
                      <!-- <p>{2}</p>-->
                     </div>
                 </div>", list[i].Title, ConfigurationSettings.AppSettings["WebUrl"] + list[i].Pic, list[i].Title, list[i].CreateDate.ToString("MM月dd日 HH时mm分ss秒"), list[i].IsURL == true ? list[i].URL : (ConfigurationSettings.AppSettings["WebUrl"] + "/GraphicDisplay.aspx?id=" + list[i].ID));
                    }
                    if (i == 1)
                    {
                        tuwen += " <div class='duotuwen'>";
                    }
                    if (i != 0)
                    {
                        tuwen += string.Format(@"<p><img src='{0}' width='80' height='80'  onerror=""javascript:this.src='/assets/images/default.png';""><span><a href='{2}' target='_Blank'>{1}</a></span></p>", ConfigurationSettings.AppSettings["WebUrl"] + list[i].Pic, list[i].Title, list[i].IsURL == true ? list[i].URL : (ConfigurationSettings.AppSettings["WebUrl"] + "/GraphicDisplay.aspx?id=" + list[i].ID));
                    }
                }
                if (list.Count > 1)
                    tuwen += "</div>";

                tuwen += string.Format(@"<div class='tuwenEdit'>
                    <ol><span><a href='{0}' class='editda'><bdo>编辑</bdo></a></span></ol>
                    <ol><a href='{1}' class='delda'><bdo>删除</bdo></a></ol>
                 </div>
               </div>    
        </li>", list.Count == 1 ? "OneTW.do?id=" + list[0].ID : "MoreTW.do?id=" + list[0].List_ID, "javascript:deletetw(" + list[0].List_ID + ")");
            }

            return View(new Graphic_Detail_EX() { Tuwen = tuwen, Listcount = list2.Count });
        }

        [MyAuthorize]
        public ActionResult OneTW(int? id)
        {
            Graphic_Detail_EX detail = null;
            if (id != null && id != 0)
            {
                detail = _service.GetGraphicDetail(int.Parse(id.ToString()));
                detail.FullPicUrl = ConfigurationSettings.AppSettings["WebUrl"] + detail.Pic;
            }
            else
            {
                detail = new Graphic_Detail_EX { ID = int.Parse(id.ToString()), List_ID = int.Parse(id.ToString()), Title = "标题", FullPicUrl = ConfigurationSettings.AppSettings["WebUrl"] + "/assets/images/default.png" };
            }
            return View(detail);
        }

        [MyAuthorize]
        public ActionResult MemCard()
        {
            MembershipCard_EX ex = new MembershipCard_EX { Card = new WXMembershipCard { ID = 0, Merchants_ID = CurrentUserMerchantsID }, Seting = _service.GetMerchantsSetingByMerchantsID(CurrentLoginUser.ORG_ID) == null ? new WXMerchantsSeting() { ID = 0, IsBigWheelReg = false, IsCouponsReg = false, IsMemCardReg = false, IsShowActivity = false, IsShowCoupons = false, IsShowMemCardAmount = false, IsShowMemCardPrivilege = false, IsShowMemInfo = false } : _service.GetMerchantsSetingByMerchantsID(CurrentLoginUser.ORG_ID) };
            return View(ex);
        }

        public ActionResult GetMerSeting()
        {
            MerchantsSeting_EX m = _service.GetMerchantsSetingByMerchantsID(CurrentLoginUser.ORG_ID);
            if (m == null)
                m = new MerchantsSeting_EX { ID = 0, IsBigWheelReg = false, IsCouponsReg = false, IsMemCardReg = false, IsShowActivity = false, IsShowCoupons = false, IsShowMemCardAmount = false, IsShowMemCardPrivilege = false, IsShowMemInfo = false };
            var jmsg = new JsonSMsg();
            try
            {
                jmsg.Status = 0;
                jmsg.Data = m;
            }
            catch (Exception ex)
            {
                jmsg.Status = -1;
                jmsg.Message = ex.Message;
            }
            return Json(jmsg);
        }

        [HttpPost]
        public ActionResult SaveMerSeting(MerchantsSeting_EX CuObj, FormCollection form)
        {
            JsonSMsg rMsg = new JsonSMsg();
            try
            {
                WXMerchantsSeting wx = _service.GetMerchantsSetingByMerchantsID(CurrentMerchants.ID);
                if (wx == null)
                {
                    wx = new WXMerchantsSeting { ID = 0, Merchants_ID = CurrentMerchants.ID, IsShowMemInfo = false, IsShowMemCardPrivilege = false, IsShowMemCardAmount = false, IsShowCoupons = false, IsShowActivity = false, IsMemCardReg = false, IsCouponsReg = false, IsBigWheelReg = false };
                }

                wx.Merchants_ID = CurrentMerchants.ID;

                if (CuObj.SetingID != 0)
                    wx.ID = CuObj.SetingID;
                if (CuObj.IsBigWheelReg != null)
                    wx.IsBigWheelReg = CuObj.IsBigWheelReg;
                if (CuObj.IsCouponsReg != null)
                    wx.IsCouponsReg = CuObj.IsCouponsReg;
                if (CuObj.IsMemCardReg != null)
                    wx.IsMemCardReg = CuObj.IsMemCardReg;
                if (CuObj.IsShowActivity != null)
                    wx.IsShowActivity = CuObj.IsShowActivity;
                if (CuObj.IsShowCoupons != null)
                    wx.IsShowCoupons = CuObj.IsShowCoupons;
                if (CuObj.IsShowMemCardAmount != null)
                    wx.IsShowMemCardAmount = CuObj.IsShowMemCardAmount;
                if (CuObj.IsShowMemCardPrivilege != null)
                    wx.IsShowMemCardPrivilege = CuObj.IsShowMemCardPrivilege;
                if (CuObj.IsShowMemInfo != null)
                    wx.IsShowMemInfo = CuObj.IsShowMemInfo;
                _service.SaveMerchantsSeting(wx);
                rMsg.Status = 0;
                rMsg.Message = "成功";
            }
            catch (Exception ex)
            {
                rMsg.Status = -1;
                rMsg.Message = ex.Message;
            }
            return Json(rMsg);
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveMemCard(WXMembershipCard CuObj, FormCollection form)
        {
            JsonSMsg rMsg = new JsonSMsg();
            try
            {
                CuObj.Merchants_ID = CurrentLoginUser.ORG_ID;
                CuObj.CreateDate = DateTime.Now;
                _service.SaveMembershipCard(CuObj);
            }
            catch (Exception ex)
            {
                rMsg.Status = -1;
                rMsg.Message = ex.Message;
            }
            return Json(rMsg);
        }

        //列表
        [HttpPost]
        public JsonResult GetMemCardList(MembershipCardSearch search, FormCollection form)
        {
            PageView view = new PageView(form);
            if (search == null)
            {
                search = new MembershipCardSearch();
            }
            search.Merchants_ID = base.CurrentLoginUser.ORG_ID;
            string colkey = form["colkey"];
            string colsinfo = form["colsinfo"];
            PagedList<WXMembershipCard> pList = _service.QueryGetMembershipCard(search, view);
            JsonQTable fdata = JsonQTable.ConvertFromPagedList<WXMembershipCard>(pList, colkey, colsinfo.Split(','));
            return Json(fdata);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UploadImage(WXGraphicDetail CuObj, FormCollection form)
        {
            JsonSMsg rMsg = new JsonSMsg();
            try
            {

                if (Request.Files.Count > 0 && (Request.Files[0].InputStream.Length > 0 || Request["Pic"].Trim() != ""))
                {
                    HttpPostedFileBase file = Request.Files[0];
                    string Extension = Path.GetExtension(file.FileName).ToLower();

                    byte[] data = new byte[file.InputStream.Length];
                    file.InputStream.Read(data, 0, data.Length);
                    string url = Url.Action("../home/ViewImage");
                    string remark = "";// Request["filename"];
                    FILES fileEntity = _mservice.UploadFile(Extension, file.ContentType, data, url, remark);
                    fileEntity.FILE_NAME = ConfigurationSettings.AppSettings["WebUrl"] + fileEntity.FILE_URL;

                    //_mservice.InsertREL_CUST_FILES(cust);
                    rMsg.Status = 0;
                    rMsg.Message = "保存成功";
                    rMsg.Data = fileEntity;
                }
            }
            catch (Exception ex)
            {
                rMsg.Status = -1;
                rMsg.Message = ex.Message;
            }
            return Json(rMsg, "text/html");
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UploadImageByWx(WXGraphicDetail CuObj, FormCollection form)
        {
            JsonSMsg rMsg = new JsonSMsg();
            try
            {

                if (Request.Files.Count > 0 && (Request.Files[0].InputStream.Length > 0 || Request["Pic"].Trim() != ""))
                {
                    HttpPostedFileBase file = Request.Files[0];
                    string Extension = Path.GetExtension(file.FileName).ToLower();

                    byte[] data = new byte[file.InputStream.Length];
                    file.InputStream.Read(data, 0, data.Length);
                    string url = Url.Action("../home/ViewImage");
                    string remark = "";// Request["filename"];
                    FILES fileEntity = _mservice.UploadFile(Extension, file.ContentType, data, url, remark);
                    BasePage bpage = new BasePage();
                    string Access_token = bpage.Token();
                    string wxurl = "http://api.weixin.qq.com/cgi-bin/material/add_material?access_token=" + Access_token;

                    NameValueCollection nvc = new NameValueCollection();
                    nvc.Add("type", "image");
                    List<string> list = new List<string>();
                    list.Add(fileEntity.FILE_NAME);
                    string str33 = NetHelper.HttpUploadFile(wxurl, "media", file.ContentType, nvc, list);

                    JavaScriptSerializer js = new JavaScriptSerializer();
                    Ret jg = js.Deserialize<Ret>(str33);
                    fileEntity.FILE_NAME = jg.media_id;
                    rMsg.Status = 0;
                    rMsg.Message = "保存成功";
                    rMsg.Data = fileEntity;
                }
            }
            catch (Exception ex)
            {
                rMsg.Status = -1;
                rMsg.Message = ex.Message;
            }
            return Json(rMsg, "text/html");
        }

        public class Ret
        {
            public string media_id { get; set; }
            public string url { get; set; }
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SaveOneGraphic(WXGraphicDetail CuObj, FormCollection form)
        {
            JsonSMsg rMsg = new JsonSMsg();
            try
            {

                //if (Request.Files.Count > 0 && (Request.Files[0].InputStream.Length > 0 || Request["Pic"].Trim() != ""))
                //{
                //    HttpPostedFileBase file = Request.Files[0];
                //    string Extension = Path.GetExtension(file.FileName).ToLower();

                //    byte[] data = new byte[file.InputStream.Length];
                //    file.InputStream.Read(data, 0, data.Length);
                //    string url = Url.Action("../home/ViewImage");
                //    string remark = Request["filename"];
                //    FILES fileEntity = _mservice.UploadFile(Extension, file.ContentType, data, url, remark);
                WXGraphicList l = new Hmj.Entity.WXGraphicList();
                l.CreateDate = DateTime.Now;
                l.Merchants_ID = CurrentLoginUser.ORG_ID;
                l.Title = "";// Request["Name"];
                int lid = 0;
                if (Request["List_ID"] == "0")
                    lid = _service.SaveGraphicList(l);
                else
                    lid = int.Parse(Request["List_ID"]);
                if (CuObj == null)
                    CuObj = new Hmj.Entity.WXGraphicDetail();
                CuObj.ID = int.Parse(Request["ID"]);
                // CuObj.Body = Request["ckeditor"];
                CuObj.Describe = Request["Describe"];
                CuObj.URL = Request["URL"];
                CuObj.IsURL = CuObj.URL == "" ? false : true;
                CuObj.List_ID = lid;
                CuObj.Pic = Request["FILE_URL"];
                CuObj.Sorting = 0;// int.Parse(Request["Sorting"]);
                CuObj.Title = Request["Title"];

                _service.SaveGraphicDetail(CuObj);

                //_mservice.InsertREL_CUST_FILES(cust);
                rMsg.Status = 0;
                rMsg.Message = "保存成功";
                CuObj.List_ID = lid;
                rMsg.Data = CuObj;
                //return Redirect("GraphicList.do");
                //}
            }
            catch (Exception ex)
            {
                rMsg.Status = -1;
                rMsg.Message = ex.Message;
            }
            return Json(rMsg);
        }

        /// <summary>
        /// 一键同步图文
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DownLoad()
        {
            JsonSMsg rMsg = new JsonSMsg();
            try
            {
                CustomMenuApi cma = new CustomMenuApi();
                string str = cma.DownLoad();

                rMsg.Status = 0;
                rMsg.Message = "成功";
            }
            catch (Exception ex)
            {

            }

            return Json(rMsg);
        }

        [HttpPost]
        public JsonResult DeleteTW(int id)
        {
            JsonSMsg rMsg = new JsonSMsg();
            try
            {


                _service.DeleteTW(id);
                rMsg.Status = 0;
                rMsg.Message = "保存成功";
                //return Redirect("GraphicList.do");
                //}
            }
            catch (Exception ex)
            {
                rMsg.Status = -1;
                rMsg.Message = ex.Message;
            }
            return Json(rMsg);
        }

        [HttpPost]
        public JsonResult DeleteInformation(int id)
        {
            JsonSMsg rMsg = new JsonSMsg();
            try
            {


                _service.DeleteInformation(id);
                rMsg.Status = 0;
                rMsg.Message = "保存成功";
                //return Redirect("GraphicList.do");
                //}
            }
            catch (Exception ex)
            {
                rMsg.Status = -1;
                rMsg.Message = ex.Message;
            }
            return Json(rMsg);
        }

        [HttpPost]
        public JsonResult DeleteStore(int id)
        {
            JsonSMsg rMsg = new JsonSMsg();
            try
            {


                _service.DeleteStore(id);
                rMsg.Status = 0;
                rMsg.Message = "删除成功";
                //return Redirect("GraphicList.do");
                //}
            }
            catch (Exception ex)
            {
                rMsg.Status = -1;
                rMsg.Message = ex.Message;
            }
            return Json(rMsg);
        }

        [HttpPost]
        public JsonResult UpdateShow(int Id, string IsShow)
        {
            JsonSMsg rMsg = new JsonSMsg();
            try
            {
                _service.UpdateShow(Id, IsShow);
                rMsg.Status = 0;
                rMsg.Message = "保存成功";
                //return Redirect("GraphicList.do");
                //}
            }
            catch (Exception ex)
            {
                rMsg.Status = -1;
                rMsg.Message = ex.Message;
            }
            return Json(rMsg);
        }

        [HttpPost]
        public JsonResult GetGraphicByRowID(int listid, int rowid)
        {
            JsonSMsg rMsg = new JsonSMsg();
            try
            {


                Graphic_Detail_EX g = _service.GetGraphic_DetailByRowID(listid, rowid);
                if (g == null)
                    g = new Graphic_Detail_EX { ID = 0, List_ID = int.Parse(listid.ToString()), Title = "标题", FullPicUrl = "~/favicon.ico" };
                else
                {
                    g.FullPicUrl = ConfigurationSettings.AppSettings["WebUrl"] + g.Pic;
                }
                rMsg.Data = g;
                rMsg.Status = 0;
                rMsg.Message = "保存成功";
                //return Redirect("GraphicList.do");
                //}
            }
            catch (Exception ex)
            {
                rMsg.Status = -1;
                rMsg.Message = ex.Message;
            }
            return Json(rMsg);
        }

        [MyAuthorize]
        public ActionResult MoreTW(int? id)
        {
            List<Graphic_Detail_EX> detail = null;
            if (id != null && id != 0)
            {

                detail = _service.GetAllGraphicDetail(int.Parse(id.ToString()));
                foreach (Graphic_Detail_EX d in detail)
                {
                    d.FullPicUrl = ConfigurationSettings.AppSettings["WebUrl"] + d.Pic;
                }
            }
            else
            {
                detail = new List<Graphic_Detail_EX> { new Graphic_Detail_EX { ID = int.Parse(id.ToString()), List_ID = int.Parse(id.ToString()), Title = "标题", FullPicUrl = ConfigurationSettings.AppSettings["WebUrl"] + "/assets/images/default.png" } }; ;
            }
            return View(detail);
        }

        [MyAuthorize]
        public ActionResult MsgRecord(int? id)
        {
            string tuwen = "";
            List<WXGraphicList> list2 = _service.GetAllGraphicList(CurrentLoginUser.ORG_ID);
            foreach (WXGraphicList g in list2)
            {
                List<Graphic_Detail_EX> list = _service.GetAllGraphicDetail(g.ID);
                tuwen += "<li id='li" + g.ID + "' class='col-xs-4'>";
                for (int i = 0; i < list.Count; i++)
                {
                    if (i == 0)
                    {
                        tuwen += string.Format(@" <div class='tuwen'>
                 <div class='onetuwen'>
                     <div class='tuwenTitle'><a href='{4}' target='_Blank'>{0}</a></div>
                     <div class='tuwenTime'>{3}</div>
                     <div class='tuwencon duotucon'>
                       <img src='{1}'  onerror=""javascript:this.src='/assets/images/default.png';"">
                       <p>{2}</p>
                     </div>
                 </div>", list[i].Title, ConfigurationSettings.AppSettings["WebUrl"] + list[i].Pic, list[i].Title, list[i].CreateDate.ToString("MM月dd日"), list[i].IsURL == true ? list[i].URL : (ConfigurationSettings.AppSettings["WebUrl"] + "/GraphicDisplay.aspx?id=" + list[i].ID));
                    }
                    if (i == 1)
                    {
                        tuwen += " <div class='duotuwen'>";
                    }
                    if (i != 0)
                    {
                        tuwen += string.Format(@"<p><img src='{0}' width='80' height='80'  onerror=""javascript:this.src='/assets/images/default.png';""><span><a href='{2}' target='_Blank'>{1}</a></span></p>", ConfigurationSettings.AppSettings["WebUrl"] + list[i].Pic, list[i].Title, list[i].IsURL == true ? list[i].URL : (ConfigurationSettings.AppSettings["WebUrl"] + "/GraphicDisplay.aspx?id=" + list[i].ID));
                    }
                }
                if (list.Count > 1)
                    tuwen += "</div>";

                tuwen += string.Format(@" <div class='hoverbg'></div>
             <bdo style='color:#ffffff'>{0}</bdo>", g.ID);
            }
            List<WXBiaoqing> listbq = null;
            if (Session["WXbiaoqing"] != null)
            {
                listbq = Session["WXbiaoqing"] as List<WXBiaoqing>;
            }
            else
            {
                listbq = _service.GetBQList();
            }
            string biaoqing = "";
            foreach (WXBiaoqing w in listbq)
            {
                biaoqing += @" <img style='margin:5px;'  onerror=""javascript:this.src='/assets/images/default.png';"" src='/assets/img/biaoqing/" + (w.ID - 1) + ".png' alt='" + w.Text + "' onclick='javascript:addbq(\"" + w.Text + "\"," + (w.ID - 1) + ")' onmouseout=\"hidephoto('" + (w.ID - 1) + "',this)\"  onmouseover=\"showphoto('" + (w.ID - 1) + "','" + w.Text + "',this)\" />";
            }
            List<CUST_MSG_RECORD_EX> listMsg = _service.GetMsgList(id);
            if (listMsg.Count >= 1)
            {
                listMsg[0].Tuwen = tuwen;
                listMsg[0].Biaoqing = biaoqing;
                listMsg[0].BQlist = listbq;
            }
            if (listMsg[0].ReturnID == null)
                listMsg[0].ReturnID = 0;
            return View(listMsg);
        }

        [HttpPost]
        public JsonResult DelGraphicByRowID(int listid, int rowid)
        {
            JsonSMsg rMsg = new JsonSMsg();
            try
            {


                _service.DelGraphic_DetailByRowID(listid, rowid);

                rMsg.Status = 0;
                rMsg.Message = "删除成功";
                //return Redirect("GraphicList.do");
                //}
            }
            catch (Exception ex)
            {
                rMsg.Status = -1;
                rMsg.Message = ex.Message;
            }
            return Json(rMsg);
        }

        /// <summary>
        /// 发送微信消息到粉丝   图文与文本消息已合并
        /// </summary>
        /// <param name="CuObj"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SendText(CUST_MSG_RECORD_EX CuObj, FormCollection form)
        {
            JsonSMsg rMsg = new JsonSMsg();
            try
            {
                rMsg.Data = CuObj;
                CUST_MSG_RECORD_EX msg = _service.GetMsgByID(CuObj.ReturnID);
                if (msg != null)
                {
                    // msg.ReturnID = CuObj.ReturnID;
                    msg.IS_RETURN = true;
                    _service.SaveMsg(msg);
                }
                CuObj.CREATE_DATE = DateTime.Now;
                CuObj.ID = 0;
                CuObj.IS_RETURN = false;
                CuObj.IS_STAR = false;
                if (CuObj.MSGTYPE == null || CuObj.MSGTYPE == "")
                {
                    CuObj.MSGTYPE = "text";
                }
                if (CuObj.MSGTYPE == "text")
                {
                    CuObj.GraphicID = 0;
                }
                // CuObj.ReturnID = 0;
                CuObj.State = 1;


                BasePage bpage = new BasePage();
                string Access_token = bpage.Token();
                string d = "";

                var postUrl = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + Access_token;
                if (CuObj.MSGTYPE == "text")
                {

                    d = @"{""touser"":""{0}"",""msgtype"":""text"",""text"":{""content"":""{1}""}}";
                    d = d.Replace("{0}", CuObj.FROMUSERNAME).Replace("{1}", CuObj.CONTENT);

                }
                else
                {
                    List<Graphic_Detail_EX> list = _service.GetAllGraphicDetail(CuObj.GraphicID);
                    string tw = "";
                    foreach (Graphic_Detail_EX g in list)
                    {
                        tw += @"{""title"":""{0}"",""description"":""{1}"",""url"":""{2}"",""picurl"":""{3}""},";
                        tw = tw.Replace("{0}", g.Title).Replace("{1}", g.Describe).Replace("{2}", bool.Parse(g.IsURL.ToString()) ? g.URL : (ConfigurationSettings.AppSettings["WebUrl"] + "/GraphicDisplay.aspx?id=" + g.ID)).Replace("{3}", ConfigurationSettings.AppSettings["WebUrl"] + g.Pic);
                    }
                    if (list.Count > 0)
                        tw = tw.Substring(0, tw.Length - 1);
                    d = @"{""touser"":""{0}"",""msgtype"":""news"",""news"":{""articles"": [{1}]}}";
                    d = d.Replace("{0}", CuObj.FROMUSERNAME).Replace("{1}", tw);
                    //var resMessage = HttpXmlPostRequest(postUrl, d, Encoding.UTF8);
                }
                var resMessage = HttpXmlPostRequest(postUrl, d, Encoding.UTF8);
                if (resMessage.Contains("ok"))
                {
                    CuObj.CONTENT += "[已发送]";
                    rMsg.Status = 0;
                    rMsg.Message = "保存成功";
                }
                else if (resMessage.Contains("45015"))
                {
                    CuObj.CONTENT += "[发送失败:" + resMessage + "]";
                    rMsg.Status = -1;
                    rMsg.Message = "由于长时间未互动，发送失败！";
                }
                else if (resMessage.Contains("4"))
                {
                    CuObj.CONTENT += "[发送失败:" + resMessage + "]";
                    rMsg.Status = -1;
                    rMsg.Message = "由于未知错误，发送失败！";
                }

                _service.SaveMsg(CuObj);
            }
            catch (Exception ex)
            {
                rMsg.Status = -1;
                rMsg.Message = ex.Message;
            }
            return Json(rMsg);
        }

        /// <summary>
        /// 发送微信消息   仅图文消息，该方法已停用
        /// </summary>
        /// <param name="CuObj"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SendNews(CUST_MSG_RECORD_EX CuObj, FormCollection form)
        {
            JsonSMsg rMsg = new JsonSMsg();
            try
            {
                rMsg.Data = CuObj;
                CUST_MSG_RECORD_EX msg = _service.GetMsgByID(CuObj.ReturnID);
                if (msg != null)
                {
                    msg.IS_RETURN = true;
                    _service.SaveMsg(msg);
                }
                CuObj.CREATE_DATE = DateTime.Now;
                CuObj.ID = 0;
                CuObj.IS_RETURN = false;
                CuObj.IS_STAR = false;
                CuObj.MSGTYPE = "news";
                // CuObj.ReturnID = 0;
                CuObj.State = 1;
                _service.SaveMsg(CuObj);

                List<Graphic_Detail_EX> list = _service.GetAllGraphicDetail(CuObj.GraphicID);

                BasePage bpage = new BasePage();
                string Access_token = bpage.Token();

                var postUrl = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + Access_token;
                string tw = "";
                foreach (Graphic_Detail_EX g in list)
                {
                    tw += @"{
             ""title"":""{0}"",
             ""description"":""{1}"",
             ""url"":""{2}"",
             ""picurl"":""{3}""
         },";
                    tw = tw.Replace("{0}", g.Title).Replace("{1}", g.Describe).Replace("{2}", bool.Parse(g.IsURL.ToString()) ? g.URL : (ConfigurationSettings.AppSettings["WebUrl"] + "/GraphicDisplay.aspx?id=" + g.ID)).Replace("{3}", ConfigurationSettings.AppSettings["WebUrl"] + g.Pic);
                }
                if (list.Count > 0)
                    tw = tw.Substring(0, tw.Length - 1);
                string d = @"{
    ""touser"":""{0}"",
    ""msgtype"":""news"",
    ""news"":{
        ""articles"": [
        {1}
         ]
    }
}";
                d = d.Replace("{0}", CuObj.FROMUSERNAME).Replace("{1}", tw);
                var resMessage = HttpXmlPostRequest(postUrl, d, Encoding.UTF8);

                rMsg.Status = 0;
                rMsg.Message = "保存成功";


            }
            catch (Exception ex)
            {
                rMsg.Status = -1;
                rMsg.Message = ex.Message;
            }
            return Json(rMsg);
        }


        [HttpPost]
        public JsonResult AddStar(int id, int star)
        {
            JsonSMsg rMsg = new JsonSMsg();
            try
            {


                CUST_MSG_RECORD_EX msg = _service.GetMsgByID(id);
                if (msg != null)
                {
                    msg.IS_STAR = star == 1 ? true : false;
                    _service.SaveCUST_MSG_RECORD(msg);
                }
                rMsg.Status = 0;
                rMsg.Message = star == 1 ? "收藏消息成功" : "取消收藏成功";
                //return Redirect("GraphicList.do");
                //}
            }
            catch (Exception ex)
            {
                rMsg.Status = -1;
                rMsg.Message = ex.Message;
            }
            return Json(rMsg);
        }


        [HttpPost]
        [ValidateInput(false)]
        public JsonResult AddBQ(string text)
        {
            JsonSMsg rMsg = new JsonSMsg();
            try
            {
                List<WXBiaoqing> bqlist = null;
                if (Session["WXbiaoqing"] != null)
                {
                    bqlist = Session["WXbiaoqing"] as List<WXBiaoqing>;
                }
                else
                {
                    bqlist = _service.GetBQList();
                }
                //  List<WXBiaoqing> bqlist = _service.GetBQList();
                if (text != null)
                {
                    text = text.Replace("\r\n", "<br>").Replace("\n", "<br>");
                }
                if (text != null && text.Contains("/"))
                {
                    foreach (WXBiaoqing w in bqlist)
                    {
                        text = text.Replace(w.Text, " <img  src='/assets/img/biaoqing/" + (w.ID - 1) + ".gif'  />");
                    }
                    rMsg.Data = text;
                }
                else
                {
                    rMsg.Data = text;
                }
                rMsg.Status = 0;
            }
            catch (Exception ex)
            {
                rMsg.Status = -1;
                rMsg.Message = ex.Message;
            }
            return Json(rMsg);
        }
        public ActionResult AreaManage()
        {
            return View();
        }
        public ActionResult StoreManage()
        {
            return View();
        }
        public ActionResult StoreEdit(int? id)
        {
            Store_EX sto = null;
            var jmsg = new JsonSMsg();

            if (id == 0)
                sto = new Store_EX() { ID = 0 };
            else
            {
                sto = _service.GetStore(int.Parse(id.ToString()));
            }
            return View(sto);
        }
        public ActionResult AreaEdit(int? id)
        {
            AreaManage_EX area = null;
            var jmsg = new JsonSMsg();

            if (id == 0)
                area = new AreaManage_EX() { Id = 0 };
            else
            {
                area = _service.GetArea(int.Parse(id.ToString()));
            }
            return View(area);
        }
        public JsonResult AreaList(FormCollection form)
        {
            PageView view = new PageView(form);
            PagedList<AreaManage_EX> pList = _service.QueryGetArea(view);
            string colkey = form["colkey"];
            string colsinfo = form["colsinfo"];
            JsonQTable fdata = JsonQTable.ConvertFromPagedList<AreaManage_EX>(pList, colkey, colsinfo.Split(','));
            return Json(fdata);
        }
        public JsonResult StoreList(FormCollection form)
        {
            PageView view = new PageView(form);
            PagedList<Store_EX> pList = _service.QueryGetStore(view);
            string colkey = form["colkey"];
            string colsinfo = form["colsinfo"];
            JsonQTable fdata = JsonQTable.ConvertFromPagedList<Store_EX>(pList, colkey, colsinfo.Split(','));
            return Json(fdata);
        }
        //修改       
        public ActionResult MemCardEdit(int? id)
        {
            WXMembershipCard cuobj = null;
            var jmsg = new JsonSMsg();
            try
            {
                if (id == 0)
                    cuobj = new WXMembershipCard() { ID = 0 };
                else
                {
                    cuobj = _service.GetMembershipCard(int.Parse(id.ToString()));
                }

                jmsg.Status = 0;
                jmsg.Data = cuobj;
            }
            catch (Exception ex)
            {
                jmsg.Status = -1;
                jmsg.Message = ex.Message;
            }
            return Json(jmsg);
        }


        [MyAuthorize]
        public ActionResult Ticket()
        {
            WXMerchantsSeting wx = _service.GetMerchantsSetingByMerchantsID(CurrentMerchants.ID);
            if (wx == null)
                wx = new WXMerchantsSeting { ID = 0, Merchants_ID = CurrentMerchants.ID };
            return View(wx);
        }

        [MyAuthorize]
        public ActionResult CustList()
        {
            return View();
        }

        public ActionResult Activity(int? userid)
        {
            AreaManage area = _service.GetAreaManage(userid);
            if (area == null)
            {
                area = new AreaManage { };
            }
            return View(area);
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="CuObj"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveAreaManage(AreaManage CuObj, FormCollection form)
        {
            JsonSMsg rMsg = new JsonSMsg();
            if (CuObj.URL == null)
            {
                CuObj.URL = "";
            }
            if (CuObj.Title == null)
            {
                CuObj.Title = "";
            }
            try
            {
                int ret = _service.SaveAreaManage(CuObj);
                if (ret >= 0)
                {
                    rMsg.Status = 0;
                    rMsg.Data = CuObj.Id;
                }
                else
                {
                    rMsg.Status = -1;
                    rMsg.Message = "意外错误，请稍后重试";
                }

            }
            catch (Exception ex)
            {
                rMsg.Status = -1;
                rMsg.Message = ex.Message;
            }
            return Json(rMsg);
        }
        public ActionResult SaveStore(Store_EX CuObj, FormCollection form)
        {
            JsonSMsg rMsg = new JsonSMsg();
            //if (CuObj.URL == null)
            //{
            //    CuObj.URL = "";
            //}
            try
            {
                int ret = _service.SaveStore(CuObj);
                if (ret >= 0)
                {
                    rMsg.Status = 0;
                    rMsg.Data = CuObj.ID;
                }
                else
                {
                    rMsg.Status = -1;
                    rMsg.Message = "意外错误，请稍后重试";
                }

            }
            catch (Exception ex)
            {
                rMsg.Status = -1;
                rMsg.Message = ex.Message;
            }
            return Json(rMsg);
        }
        public ActionResult SaveArea(AreaManage_EX CuObj, FormCollection form)
        {
            JsonSMsg rMsg = new JsonSMsg();

            try
            {
                if (CuObj.Id == 0)
                {
                    USER_INFO u = new USER_INFO();
                    u.ID = 0;
                    u.USER_NO = CuObj.User_No;
                    u.USER_PASS = CuObj.User_Pass;
                    u.EMPLOYEE_ID = 2;
                    u.ROLE_ID = 1;
                    u.ORG_ID = 7;
                    u.STORE_ID = 0;
                    u.USER_TYPE = "0";
                    u.CREATE_DATE = DateTime.Now;
                    u.CREATE_USER = "max admin";
                    u.LAST_MODI_DATE = DateTime.Now;
                    u.LAST_MODI_USER = "max admin";
                    u.FromUserName = "o0UMCj3Thp6cliR_vMjufwBGo2fM";

                    AreaManage a = new AreaManage();
                    a.Id = 0;
                    a.Title = "";
                    int areano = _service.GetTopId() + 1;
                    a.AreaNo = "A" + areano.ToString();
                    a.Body = "";
                    a.Name = CuObj.Name;
                    a.URL = "http://wechat.censh.com/wechat/Area/mr.html";
                    a.IsShow = "是";
                    a.UserId = _service.SaveUser_Info(u);

                    int ret = _service.SaveAreaManage(a);
                    if (ret >= 0)
                    {
                        rMsg.Status = 0;
                        rMsg.Data = CuObj.Id;
                    }
                    else
                    {
                        rMsg.Status = -1;
                        rMsg.Message = "意外错误，请稍后重试";
                    }

                }
                else
                {
                    int ret = _service.UpdateAreaName(CuObj.Id, CuObj.Name, CuObj.Title);
                    int ret2 = _service.UpdateUserInfo(CuObj.UserId, CuObj.User_No, CuObj.User_Pass);
                }
            }
            catch (Exception ex)
            {
                rMsg.Status = -1;
                rMsg.Message = ex.Message;
            }
            return Json(rMsg);
        }
        public ActionResult OpinionList()
        {
            return View(new WXOpinion { ID = 0 });
        }

        /// <summary>
        /// 留言列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetOpinionList(RoleSearch search, FormCollection form)
        {
            PageView view = new PageView(form);
            if (search == null)
            {
                search = new RoleSearch();
            }
            search.ORG_ID = base.CurrentLoginUser.ID;
            string colkey = form["colkey"];
            string colsinfo = form["colsinfo"];
            PagedList<WXOpinion_EX> pList = _service.GetWXOpinionList(view);
            JsonQTable fdata = JsonQTable.ConvertFromPagedList<WXOpinion_EX>(pList, colkey, colsinfo.Split(','));
            return Json(fdata);
        }

        [MyAuthorize]
        public ActionResult CustImport()
        {
            return View();
        }

        [MyAuthorize]
        public ActionResult TicketEdit(int? id)
        {
            WXTicket wx = _service.GetTicket(id);
            if (wx == null)
            {
                wx = new WXTicket { ID = 0, Merchants_ID = CurrentMerchants.ID, HavNum = 0, Pic = "/assets/images/default.png" };
            }
            return View(wx);
        }

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult NewQRCode(int? id, string name)
        {
            var jmsg = new JsonSMsg();
            Qrcode q = new Qrcode();
            q.Ticket = "";
            BasePage bpage = new BasePage();
            string access_token = bpage.Token();
            string jason = "{\"action_name\": \"QR_LIMIT_SCENE\", \"action_info\": {\"scene\": {\"scene_id\": " + id + "}}}";
            string resMessage = HttpXmlPostRequest("https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + access_token, jason, Encoding.UTF8);
            string[] a = resMessage.Split('\"');
            if (a.Length > 3)
            {
                q.Ticket = Server.UrlEncode(a[3]);
                q.QID = id;
                q.Name = name;
                _service.SaveQrCode(q);
                jmsg.Data = q;
            }
            else
            {
                jmsg.Data = resMessage;
            }
            return Json(jmsg);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="CuObj"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveTicket(WXTicket CuObj, FormCollection form)
        {
            JsonSMsg rMsg = new JsonSMsg();
            try
            {
                CuObj.Merchants_ID = CurrentLoginUser.ORG_ID;
                CuObj.UsableNum = CuObj.TotalNum - CuObj.HavNum;
                int ret = _service.SaveTicket(CuObj);
                if (ret >= 0)
                {
                    rMsg.Status = 0;
                    rMsg.Data = CuObj.ID;
                }
                else
                {
                    rMsg.Status = -1;
                    rMsg.Message = "意外错误，请稍后重试";
                }

            }
            catch (Exception ex)
            {
                rMsg.Status = -1;
                rMsg.Message = ex.Message;
            }
            return Json(rMsg);
        }

        //列表
        [HttpPost]
        public JsonResult TickdtList(RoleSearch search, FormCollection form)
        {
            PageView view = new PageView(form);
            if (search == null)
            {
                search = new RoleSearch();
            }
            search.ORG_ID = base.CurrentLoginUser.ORG_ID;
            string colkey = form["colkey"];
            string colsinfo = form["colsinfo"];
            PagedList<WXTicket> pList = _service.QueryTicketList(search, view);
            JsonQTable fdata = JsonQTable.ConvertFromPagedList<WXTicket>(pList, colkey, colsinfo.Split(','));
            return Json(fdata);
        }

        //列表
        [HttpPost]
        public JsonResult GetCustList(RoleSearch search, FormCollection form)
        {
            PageView view = new PageView(form);
            if (search == null)
            {
                search = new RoleSearch();
            }
            search.ORG_ID = base.CurrentLoginUser.ORG_ID;
            string colkey = form["colkey"];
            string colsinfo = form["colsinfo"];
            PagedList<CUST_INFO> pList = _service.QueryCustList(search, view);
            JsonQTable fdata = JsonQTable.ConvertFromPagedList<CUST_INFO>(pList, colkey, colsinfo.Split(','));
            return Json(fdata);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpLoad(WXGraphicDetail CuObj, FormCollection form)
        {
            JsonSMsg rMsg = new JsonSMsg();
            try
            {

                if (Request.Files.Count > 0 && (Request.Files[0].InputStream.Length > 0))
                {
                    HttpPostedFileBase file = Request.Files[0];
                    string Extension = Path.GetExtension(file.FileName).ToLower();

                    byte[] data = new byte[file.InputStream.Length];
                    file.InputStream.Read(data, 0, data.Length);
                    string url = Url.Action("../home/ViewImage");
                    string remark = "";// Request["filename"];
                    FILES fileEntity = _mservice.UploadFile(Extension, file.ContentType, data, url, remark);

                    string result = string.Empty;
                    string strConn;
                    strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + fileEntity.FILE_NAME + "; " + "Extended Properties=Excel 8.0;";
                    OleDbDataAdapter myCommand = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", strConn);
                    DataSet myDataSet = new DataSet();
                    myCommand.Fill(myDataSet, "ExcelInfo");
                    System.Data.DataTable tab = myDataSet.Tables["ExcelInfo"].DefaultView.ToTable();
                    CUST_INFO c = new CUST_INFO();
                    int num = 0;
                    foreach (DataRow dr in tab.Rows)
                    {
                        try
                        {
                            c.NAME = dr[1].ToString();
                            c.MOBILE = dr[2].ToString();
                            c.BIRTHDAY = dr[4].ToString();
                            if (c.NAME != "" && c.MOBILE != "")
                            {
                                _service.SaveCustInfo(c);
                                num++;
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                    rMsg.Status = 0;
                    rMsg.Message = "成功导入" + num + "条数据";
                }
            }
            catch (Exception ex)
            {
                rMsg.Status = -1;
                rMsg.Message = ex.Message;
            }


            return Json(rMsg, "text/html");
        }

        #region 公用类
        private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        /// <summary>
        /// HttpPost发送XML并返回响应
        /// </summary>
        /// <param name="postUrl"></param>
        /// <param name="xml"></param>        
        /// <param name="encoding"></param>         
        /// <returns>Response响应</returns>
        public static string HttpXmlPostRequest(string postUrl, string postXml, Encoding encoding)
        {
            if (string.IsNullOrEmpty(postUrl))
            {
                throw new ArgumentNullException("HttpXmlPost ArgumentNullException :  postUrl IsNullOrEmpty");
            }

            if (string.IsNullOrEmpty(postXml))
            {
                throw new ArgumentNullException("HttpXmlPost ArgumentNullException : postXml IsNullOrEmpty");
            }

            var request = (HttpWebRequest)WebRequest.Create(postUrl);
            byte[] byteArray = encoding.GetBytes(postXml);
            request.ContentLength = byteArray.Length;
            request.Method = "post";
            request.ContentType = "text/xml";

            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(byteArray, 0, byteArray.Length);
            }

            using (var responseStream = request.GetResponse().GetResponseStream())
            {
                return new StreamReader(responseStream, encoding).ReadToEnd();
            }
        }


        class Data
        {
            public string total { get; set; }
            public string count { get; set; }
            public Model data { get; set; }
            public string next_openid { get; set; }
            public string nextcount { get; set; }
            public string ycount { get; set; } //每拉完一万条后重置nextcount，并记录已拉取总数

        }

        class Model
        {
            public string[] openid { get; set; }
        }


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



        //发送GET请求
        public string PostRequestGet(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";  //定义请求对象，并设置好请求URL地址      
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();    //定义响应对象，request在调用GetResponse方法事执行请求了，而不是在HttpWebRequest.Create的时候执行。
            Stream stream = response.GetResponseStream(); //定义一个流对象，来获取响应流
            StreamReader sr = new StreamReader(stream, Encoding.UTF8);  //定义一个流读取对象，读取响应流
            string responseHTML = sr.ReadToEnd();
            return responseHTML;
        }
        #endregion
    }
}
