using Newtonsoft.Json;
using Hmj.Business.ServiceImpl;
using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Interface;
using Hmj.WebApp.InsertMemberNew;
using Hmj.WebApp.SelectMemberNew;
using Hmj.WebApp.UpdateMemberNew;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Security;
using CyRong.Common;
using Hmj.Common;

namespace WeChatCRM.WebApp.WeChat
{
    /// <summary>
    /// Business 的摘要说明
    /// </summary>
    public class Business : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        MySmallShopService mss = new MySmallShopService();
        LoginService lgo = new LoginService();
        OpinionService obo = new OpinionService();
        ILogService _log = new LogService();
        ISystemService sbo = new SystemService();
        public void ProcessRequest(HttpContext context)
        {
            if (GetQeuryString("para", context) == "jsapi") //获取js接口凭证
            {
                try
                {
                    ORG_INFO m = mss.GetWD(AppConfig.FWHOriginalID);
                    if (m != null)
                    {
                        WeiPage w = new WeiPage();
                        string token = w.Token(m.ToUserName);
                        string sj = ConvertDateTimeInt(DateTime.Now).ToString();//时间戳
                        string sjm = Guid.NewGuid().ToString("d"); //随机码
                        string ticket = GetJSAPI_Ticket(token, m); //凭证
                        string dz = context.Server.UrlDecode(context.Request.Params["apiurl"]);
                        string noncestr = "noncestr=" + sjm;
                        string jsapi_ticket = "jsapi_ticket=" + ticket;
                        string timestamp = "timestamp=" + sj;
                        string url = "url=" + dz;
                        string[] ArrTmp = { noncestr, jsapi_ticket, timestamp, url };
                        Array.Sort(ArrTmp);     //字典排序
                        string tmpStr = string.Join("&", ArrTmp);
                        tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
                        tmpStr = tmpStr.ToLower();
                        if (ticket == "")
                        {
                            context.Response.Write("{\"status\":\"" + -1 + "\"}");
                        }
                        else
                        {
                            var re = new
                            {
                                state = 0,
                                appId = m.AppID,
                                timestamp = sj,
                                nonceStr = sjm,
                                signature = tmpStr,
                                url = dz,
                                link = (dz.IndexOf("&") == -1 ? dz : dz.Substring(0, dz.IndexOf("&"))),
                                title = m.nick_name + "预约系统",
                                imgUrl = "http://wechat.beautyfarm.com.cn/wechat/logo.gif",//"http://www.meijiewd.com/assets/images/meijie.png",
                                desc = "美丽田园源自欧洲，作为顶级专业护肤机构、美容企业领军者享誉21载。在北京、上海等近40座城市的高端商圈开设超百家美容中心。秉承德国严谨的质控流程、国际化专业服务，实现了高科技与天然保养品的完美结合，让每一位客人体验到完美的呵护和心灵的放松。"
                            };
                            string ret = JsonConvert.SerializeObject(re);
                            context.Response.Write(ret);
                        }
                    }
                }
                catch (Exception ex)
                {
                    context.Response.Write(ex.Message + "," + ex.StackTrace);
                }

            }
            else if (GetQeuryString("para", context) == "SaveLocation") //保存地理位置
            {
                try
                {
                    ORG_INFO m = mss.GetWD(context.Session["ToUserName"].ToString());
                    if (m != null)
                    {
                        WD_Location l = new WD_Location();
                        l.accuracy = context.Request.Params["accuracy"];
                        l.Createdate = DateTime.Now;
                        l.FromUserName = context.Session["FromUserName"].ToString();
                        l.latitude = context.Request.Params["latitude"];
                        l.longitude = context.Request.Params["longitude"];
                        l.speed = context.Request.Params["speed"];
                        l.ToUserName = context.Session["ToUserName"].ToString();
                        new WeiPage().GetBaiDuMap(ref l);
                        mss.SaveLocation(l);
                        context.Response.Write("0");
                    }
                }
                catch (Exception ex)
                {
                    context.Response.Write(ex.Message + "," + ex.StackTrace);
                }

            }
            else if (GetQeuryString("para", context) == "share") //保存分享记录
            {
                try
                {
                    ORG_INFO m = mss.GetWD(context.Session["ToUserName"].ToString());
                    if (m != null)
                    {
                        WD_Share s = new WD_Share();
                        s.apiurl = context.Server.UrlDecode(context.Request.Params["apiurl"]); ;
                        s.CreateDate = DateTime.Now;
                        s.FromUserName = context.Session["FromUserName"].ToString();
                        s.state = int.Parse(context.Request.Params["state"]);
                        s.type = int.Parse(context.Request.Params["type"]);
                        mss.SaveShare(s);
                        context.Response.Write("{\"message\":\"成功。\",\"status\":\"" + -1 + "\"}");
                    }
                }
                catch (Exception ex)
                {
                    context.Response.Write("{\"message\":\"" + ex.Message + ex.StackTrace + "。\",\"status\":\"" + -1 + "\"}");
                }

            }
            else if (GetQeuryString("para", context) == "Ticket")//领券
            {
                if (context.Session["FromUserName"] != null && context.Request.Params["Ticketid"] != null)
                {
                    WXTicketForPerson per = mss.GetTicketByOpenID(context.Session["FromUserName"].ToString(), int.Parse(context.Request.Params["Ticketid"].ToString()));
                    if (per != null)
                    {
                        context.Response.Write("{\"message\":\"该券已经领取，无需重复领取。\",\"status\":\"" + -1 + "\"}");
                    }
                    else
                    {
                        per = new WXTicketForPerson();
                        per.FromUserName = context.Session["FromUserName"].ToString();
                        per.GetTime = DateTime.Now;
                        per.Store = "";
                        per.TicketID = int.Parse(context.Request.Params["Ticketid"].ToString());
                        mss.SaveTicketForPerson(per);
                        WXTicket t = mss.GetTicket((int)per.TicketID);
                        if (t != null)
                        {
                            t.HavNum += 1;
                            mss.UpdateTicket(t);
                        }
                        context.Response.Write("{\"message\":\"领取成功。\",\"status\":\"" + 0 + "\"}");
                    }
                }

                else
                {
                    context.Response.Write("{\"message\":\"数据异常，请重新访问。\",\"status\":\"" + -1 + "\"}");
                }
            }
            else if (GetQeuryString("para", context) == "booking")//预约
            {
                string mdid = context.Request.Params["mdid"];
                string yyxm = context.Request.Params["yyxm"];
                string empno = context.Request.Params["empno"];
                string time = context.Request.Params["time"];
                string name = context.Request.Params["name"];
                string phone = context.Request.Params["phone"];
                string sex = context.Request.Params["sex"];
                string sid = context.Request.Params["sid"];
                string sname = context.Request.Params["sname"];
                if (yyxm.Length > 2 && yyxm.Substring(yyxm.Length - 1, 1) == ",") //最后一位如果是逗号，则将逗号替换
                {
                    yyxm = yyxm.Substring(0, yyxm.Length - 1);
                }
                if (DateTime.Parse(time) < DateTime.Now)
                {
                    context.Response.Write("{\"message\":\"预约时间不可早于当前时间。\",\"status\":\"" + -1 + "\"}");
                }
                else
                {
                    ORG_INFO m = mss.GetWD(context.Session["ToUserName"].ToString());
                    if (m != null)
                    {
                        if (m.DOMAIN_PREFIX != "" && m.DOMAIN_PREFIX != null)
                        {
                            string url = m.DOMAIN_PREFIX + "booking/submit";
                            string json = @"{
    ""store_no"": ""{0}"",
    ""wx_open_id"": ""{1}"",
    ""cust_name"": ""{2}"",
    ""cust_mobile"": ""{3}"",
    ""cust_gender"": ""{4}"",
    ""begin_time"": ""{5}"",
    ""end_time"": ""{6}"",
    ""category_no"": ""{7}"",
    ""booking_num"": 1,
    ""org_no"":""beautyfarm""
  }";
                            json = json.Replace("{0}", mdid).Replace("{1}", context.Session["FromUserName"].ToString()).Replace("{2}", name).Replace("{3}", phone).Replace("{4}", sex == "0" ? "女" : "男").Replace("{5}", time).Replace("{6}", DateTime.Parse(time).AddMinutes(30).ToString()).Replace("{7}", yyxm.Replace("面部", "S005007").Replace("身体", "S005002"));
                            string message = HttpXmlPostRequest(url, json, Encoding.UTF8);
                            result data = JsonConvert.DeserializeObject<result>(message);
                            if (data.status == 1) //预约成功
                            {
                                SaveBooking(context, mdid, yyxm, empno, time, name, phone, sid, sname, data);
                                //context.Response.Write("{\"message\":\"预约成功，您的预约申请已经提交，门店工作人员将会与您电话确认，敬请留意。\",\"status\":\"" + 1 + "\"}");
                                context.Response.Write(message);
                            }
                            else
                            {
                                context.Response.Write(message);
                            }
                        }
                        else
                        {
                            SaveBooking(context, mdid, yyxm, empno, time, name, phone, sid, sname, null);
                            context.Response.Write("{\"message\":\"预约成功。\",\"status\":\"" + 1 + "\"}");
                        }
                    }

                }
            }
            else if (GetQeuryString("para", context) == "getbooking")//获取预约记录
            {
                ORG_INFO m = mss.GetWD(context.Session["ToUserName"].ToString());
                if (m != null)
                {
                    booklist data = null;
                    if (m.DOMAIN_PREFIX != "" && m.DOMAIN_PREFIX != null)
                    {
                        string url = m.DOMAIN_PREFIX + "booking/list/user?org_no=beautyfarm&wx_open_id=" + context.Session["FromUserName"].ToString();
                        string message = PostRequest(url);
                        data = JsonConvert.DeserializeObject<booklist>(message);
                    }
                    else
                    {
                        List<book_ex> ex = mss.GetBookListByOpenid(context.Session["FromUserName"].ToString());
                        data = new booklist { data = ex.ToArray() };
                    }
                    if (data.data != null)
                    {
                        string html = "";
                        foreach (book_ex b in data.data)
                        {
                            html += string.Format(@"<a class='oitem' href='{6}'>
				<dl class='fr'>
					<dt>{0}</dt>
					<dd><span class='more_arrows'>{5}</span></dd>
				</dl>
				<dl class='fl'>
					<dt>{1}</dt>
					<dd>{2}  {3}  {4}</dd>
				</dl>
			   </a>", (b.status == 0 ? "已确认" : b.status == 1 ? "已开单" : b.status == 2 ? "已结账" : b.status == 3 ? "已到店" : b.status == 7 ? "待确认" : b.status == 8 ? "已爽约" : "已取消"), b.service_item, DateTime.Parse(b.begin_time).ToString("MM月dd日"), GetWeek(DateTime.Parse(b.begin_time)), DateTime.Parse(b.begin_time).ToString("HH:mm"), b.store_name, "../meijie/bookingdetail.aspx?id=" + b.id + "," + b.store_no + "," + "0" + "," + b.store_name + "," + b.cust_mobile + "," + b.category_name + "," + b.cust_name + "," + b.begin_time + "," + b.status);
                        }
                        context.Response.Write(html);
                    }
                    else
                    {
                        context.Response.Write("");
                    }
                }
            }
            else if (GetQeuryString("para", context) == "getinfo")//根据预约记录获取用户信息
            {
                ORG_INFO m = mss.GetWD(context.Session["ToUserName"].ToString());
                if (m != null)
                {
                    if (m.DOMAIN_PREFIX != null && m.DOMAIN_PREFIX != "")
                    {
                        string url = m.DOMAIN_PREFIX + "booking/list/user?org_no=beautyfarm&wx_open_id=" + context.Session["FromUserName"].ToString();
                        string message = PostRequest(url);
                        booklist data = JsonConvert.DeserializeObject<booklist>(message);
                        if (data.data != null && data.data.Length > 0)
                        {
                            context.Response.Write("{\"name\":\"" + data.data[0].cust_name + "\",\"phone\":\"" + data.data[0].cust_mobile + "\",\"status\":\"0\"}"); ;

                        }
                        else if (data.data != null)
                        {
                            CUST_INFO c = mss.GetCust(context.Session["FromUserName"].ToString());
                            if (c != null)
                            {
                                context.Response.Write("{\"name\":\"" + c.NAME + "\",\"phone\":\"" + c.MOBILE + "\",\"status\":\"0\"}"); ;
                            }
                            else
                                context.Response.Write("{\"name\":\"\",\"phone\":\"\",\"status\":\"0\"}"); ;

                        }
                        else
                        {
                            context.Response.Write("{\"message\":\"数据异常，请稍后重试。\",\"status\":\"" + -1 + "\"}");
                        }
                    }
                    else
                    {
                        List<book_ex> ex = mss.GetBookListByOpenid(context.Session["FromUserName"].ToString());
                        booklist data = new booklist { data = ex.ToArray() };
                        if (data.data != null && data.data.Length > 0)
                        {
                            context.Response.Write("{\"name\":\"" + data.data[0].cust_name + "\",\"phone\":\"" + data.data[0].cust_mobile + "\",\"status\":\"0\"}"); ;

                        }
                        else if (data.data != null)
                        {
                            context.Response.Write("{\"name\":\"\",\"phone\":\"\",\"status\":\"0\"}"); ;

                        }
                        else
                        {
                            context.Response.Write("{\"message\":\"数据异常，请稍后重试。\",\"status\":\"" + -1 + "\"}");
                        }
                    }
                }
            }
            else if (GetQeuryString("para", context) == "getlastbooking")//获取最后一条预约信息
            {
                ORG_INFO m = mss.GetWD(context.Session["ToUserName"].ToString());
                if (m != null)
                {
                    string url = m.DOMAIN_PREFIX + "booking/list/user?org_no=beautyfarm&wx_open_id=" + context.Session["FromUserName"].ToString();
                    string message = PostRequest(url);
                    booklist data = JsonConvert.DeserializeObject<booklist>(message);
                    if (data.data != null && data.data.Length > 0)
                    {
                        context.Response.Write("<span>" + DateTime.Parse(data.data[0].begin_time).ToString("MM月dd号") + "<br>（" + GetWeek(DateTime.Parse(data.data[0].begin_time)) + "）</span><span>" + DateTime.Parse(data.data[0].begin_time).ToString("HH:mm") + "</span>"); ;

                    }
                    else
                    {
                        context.Response.Write("<span>无</span>");
                    }
                }
            }
            else if (GetQeuryString("para", context) == "SendDX")//发送短信
            {
                Random r = new Random();
                int num = r.Next(100000, 999999);
                //num = 6;
                //string jg = "OK";
                Hmj.WebApp.SendSMS.SMSServiceSoapClient s = new Hmj.WebApp.SendSMS.SMSServiceSoapClient();
                string jg = s.SendSMS(context.Request.Params["phone"], 8, "您的验证码是" + num + "，在30分钟内有效，如非本人操作请忽略该短信。", DateTime.Parse("2010-01-01"), "7101008830419833");
                _log.Info(context.Request.Params["phone"] + "短信平台log:" + jg);
                if (jg.Substring(0, 2) == "OK")
                {
                    context.Session["RanPhone"] = context.Request.Params["phone"];
                    context.Session["RanNum"] = num.ToString();
                    context.Response.Write("{\"message\":\"发送成功。\",\"status\":\"" + 0 + "\"}"); ;
                }
                else
                {
                    context.Response.Write("{\"message\":\"发送失败。\",\"status\":\"" + -1 + "\"}");
                }
            }
            else if (GetQeuryString("para", context) == "GetJb")//解除绑定
            {
                Emp_Cust ec = mss.GetEmp_CustByFromUserName(context.Session["FromUserName"].ToString());
                if (ec != null)
                {
                    try
                    {
                        int ret = mss.DeleteEmp_Cust(ec.Id);
                        if (ret > 0)
                        {
                            DelEmp_Cust dec = new DelEmp_Cust();
                            dec.FromUserName = ec.FromUserName;
                            dec.Mobile = ec.Mobile;
                            dec.Remark = "用户解绑";
                            dec.CreateTime = DateTime.Now;
                            mss.SaveDelEmp_Cust(dec);
                            context.Response.Write("{\"message\":\"解除绑定关系成功。\",\"status\":\"" + 1 + "\"}");
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteTxt("DeleteRedisCache：" + ex);
                    }
                }
                else
                {
                    context.Response.Write("{\"message\":\"解绑失败。\",\"status\":\"" + -2 + "\"}");
                }
            }
            else if (GetQeuryString("para", context) == "CheckMem")//验证会员是否存在
            {
                ORG_INFO m = mss.GetWD(context.Session["ToUserName"].ToString());
                if (m != null)
                {
                    string url = m.DOMAIN_PREFIX + "customer/bind/validate?org_no=beautyfarm&wx_open_id=" + context.Session["FromUserName"].ToString();
                    string message = PostRequest(url);
                    CUST_INFO cust = obo.GetCustinfoByFromusername(context.Session["FromUserName"].ToString());
                    if (cust != null)
                    {
                        if (cust.LAST_MODI_DATE != null && (cust.LAST_MODI_DATE < DateTime.Parse("2015-05-5") || cust.LAST_MODI_DATE > DateTime.Parse("2015-06-16")))
                        {
                            context.Response.Write("{\"message\":\"" + cust.NAME + "\",\"status\":\"" + -1 + "\"}");
                        }
                        else
                        {
                            Scratch scr = mss.GetScratch(context.Session["FromUserName"].ToString());
                            if (scr != null)
                            {
                                context.Response.Write("{\"message\":\"" + cust.NAME + "\",\"status\":\"" + -1 + "\"}");
                            }
                            else
                            {
                                context.Response.Write(message);
                            }
                        }
                    }
                    else
                    {
                        context.Response.Write(message);
                    }
                }
            }
            else if (GetQeuryString("para", context) == "Reg")//注册
            {
                if (context.Session["RanNum"] != null)
                {
                    if (context.Request["sr"] == null || context.Request["sr"] == "")
                    {
                        _log.Debug("----注册-生日没有填");
                        context.Response.Write("{\"message\":\"生日没有填\",\"status\":\"" + -1 + "\"}");
                        context.Response.End();
                    }
                    if (context.Request["city"] == "请选择城市" || context.Request["city"] == "0")
                    {
                        _log.Debug("----注册-城市没有填");
                        context.Response.Write("{\"message\":\"城市没有填\",\"status\":\"" + -1 + "\"}");
                        context.Response.End();
                    }
                    if (context.Session["RanNum"].ToString() != context.Request["yzm"] || context.Session["RanPhone"].ToString() != context.Request["phone"])
                    {
                        context.Response.Write("{\"message\":\"验证码错误\",\"status\":\"" + -1 + "\"}");
                    }

                    else
                    {
                        WeiPage w = new WeiPage();
                        _log.Info("注册会员-查询crm会员log:" + context.Request["phone"]);
                        Z_LOY_BP_GETDETAILResponse zloy = w.SelectMember(context.Request["phone"]);
                        _log.Info("注册会员-查询crm会员log:" + context.Request["phone"] + "cardno" + zloy.E_USER_NO);
                        if (zloy != null)
                        {
                            CUST_INFO info = new CUST_INFO();

                            CUST_INFO infos = obo.GetCustinfoByFromusername(context.Session["FromUserName"].ToString());
                            if (infos != null)
                            {
                                info = infos;
                            }
                            if (zloy.T_CENTRAL != null && zloy.T_CENTRAL.Length > 0)
                            {
                                context.Response.Write("{\"message\":\"该手机已在盛时其他渠道注册，请点击老会员绑定\",\"status\":\"" + -1 + "\"}");
                            }
                            else
                            {

                                _log.Debug(context.Request["phone"] + "传入值:name=" + GetEmployeeT(context.Session["FromUserName"].ToString()).NAME + "phone=" + GetEmployeeT(context.Session["FromUserName"].ToString()).MOBILE + "storename=" + GetEmployeeT(context.Session["FromUserName"].ToString()).STORENAME);
                                Z_LOY_BP_CREATEResponse zly = InsertMember(GetEmployeeT(context.Session["FromUserName"].ToString()).NAME, GetEmployeeT(context.Session["FromUserName"].ToString()).MOBILE, GetEmployeeT(context.Session["FromUserName"].ToString()).STORENAME, context.Request["name"], context.Request["phone"], context.Request["sex"], context.Request["sf"], context.Request["city"], context.Request["sr"], context.Session["FromUserName"].ToString());
                                _log.Debug("zly:" + zly.T_RETURN + "zly-return:" + zly.E_BP);

                                if (zly != null)
                                {
                                    if (zly.E_BP != "" && zly.E_BP != null)
                                    {
                                        int code = 0;
                                        if (context.Request["city"] != null)
                                        {
                                            code = int.Parse(context.Request["city"]);
                                        }
                                        City c = mss.GetCityByCode(code);
                                        info.CardNo = zly.E_BP.ToString();
                                        info.NAME = context.Request["name"] == null ? "未知用户名" : context.Request["name"].ToString();
                                        info.FROM_USER_NAME = context.Session["FromUserName"] == null ? "未知fromusername" : context.Session["FromUserName"].ToString();
                                        info.LAST_MODI_DATE = DateTime.Now;
                                        info.MOBILE = context.Request["phone"] == null ? "未知电话" : context.Request["phone"];
                                        info.CustLevel = "Register";
                                        info.CustCity = c.CityName;
                                        info.CustPoint = 0;
                                        obo.UpdateCust(info);
                                        this.InsertFS(info.FROM_USER_NAME, context);
                                        context.Response.Write("{\"message\":\"注册成功\",\"status\":\"" + 1 + "\"}");
                                        _log.Info("注册会员-注册成功:" + context.Request["phone"] + "cardno" + zloy.E_USER_NO);
                                    }
                                    else
                                    {

                                        context.Response.Write("{\"message\":\"" + zly.T_RETURN[0].MESSAGE + "\",\"status\":\"" + -1 + "\"}");
                                    }
                                }
                                else
                                {
                                    context.Response.Write("{\"message\":\"插入zly失败\",\"status\":\"" + -1 + "\"}");
                                }
                            }
                        }
                        else
                        {
                            context.Response.Write("{\"message\":\"Crm系统获取数据失败\",\"status\":\"" + -1 + "\"}");
                        }
                    }
                }
                else
                {
                    context.Response.Write("{\"message\":\"请先获取验证码。\",\"status\":\"" + -1 + "\"}");
                }
            }

            else if (GetQeuryString("para", context) == "Update")//更新
            {
                WeiPage w = new WeiPage();
                try
                {

                    Z_LOY_BP_CHANGEResponse zl = w.UpdateMember(context.Request.Params["cardno"], context.Request.Params["name"], context.Request.Params["name"].Substring(0, 1), context.Request.Params["sf"], context.Request.Params["cs"], context.Request.Params["add"], context.Request.Params["sex"], context.Request.Params["xz"]);

                    if (zl.T_RETURN[0].TYPE == "S")
                    {
                        context.Response.Write("{\"message\":\"修改成功\",\"status\":\"" + 1 + "\"}");
                    }
                    else
                    {
                        context.Response.Write("{\"message\":\"" + zl.T_RETURN[0].MESSAGE + "\",\"status\":\"" + -1 + "\"}");
                    }
                }
                catch (Exception ex)
                {
                    WXLOG log = new WXLOG();
                    log.CON = ex.Message.ToString() + "Member";
                    log.TIME = DateTime.Now;
                    mss.SaveLog(log);
                }
            }
            else if (GetQeuryString("para", context) == "CheckOld")//检查手机号是否是老会员
            {
                WeiPage w = new WeiPage();
                string phone = context.Request["phone"];
                try
                {
                    Z_LOY_BP_GETDETAILResponse zloy = w.SelectMember(phone);
                    if (zloy.T_CENTRAL != null && zloy.T_CENTRAL.Length > 0)
                    {
                        context.Response.Write("{\"message\":\"手机号已存在,请点击老会员绑定\",\"status\":\"" + -1 + "\"}");
                    }
                }
                catch (Exception ex)
                {
                    WXLOG log = new WXLOG();
                    log.CON = ex.Message.ToString() + "CheckOld";
                    log.TIME = DateTime.Now;
                    mss.SaveLog(log);
                }
            }
            else if (GetQeuryString("para", context) == "BD2")//绑定
            {
                if (context.Session["RanNum"] != null)
                {
                    if (context.Session["RanNum"].ToString() != context.Request["yzm"] || context.Session["RanPhone"].ToString() != context.Request["phone"])
                    {
                        context.Response.Write("{\"message\":\"验证码错误\",\"status\":\"" + -1 + "\"}");
                    }
                    else
                    {
                        WeiPage w = new WeiPage();
                        Z_LOY_BP_GETDETAILResponse zloy = w.SelectMemberByUpdate(context.Request["phone"]);
                        _log.Info("绑定会员-查询crm会员log:" + zloy.T_CENTRAL.Length);
                        if (zloy != null)
                        {
                            CUST_INFO info = new CUST_INFO();

                            CUST_INFO infos = obo.GetCustinfoByFromusername(context.Session["FromUserName"].ToString());
                            if (infos != null)
                            {
                                info = infos;
                            }
                            if (zloy.T_CENTRAL != null && zloy.T_CENTRAL.Length > 0)
                            {
                                int code = 0;
                                if (zloy.T_ADDRESSDATA.Length > 0)
                                {
                                    code = int.Parse(zloy.T_ADDRESSDATA[0].CITY);
                                }
                                City c = mss.GetCityByCode(code);
                                info.CardNo = zloy.E_USER_NO.ToString();
                                info.NAME = zloy.T_CENTRALDATAPERSON[0] == null ? "未知用户名" : zloy.T_CENTRALDATAPERSON[0].LASTNAME.ToString();
                                info.FROM_USER_NAME = context.Session["FromUserName"] == null ? "未知fromusername" : context.Session["FromUserName"].ToString();
                                info.LAST_MODI_DATE = DateTime.Now;
                                info.MOBILE = context.Request["phone"] == null ? "未知电话" : context.Request["phone"].ToString();
                                info.CustLevel = zloy.T_ADDRESSDATA.Length > 0 ? zloy.E_LEVEL : "";
                                info.CustCity = c == null ? "" : c.CityName;
                                obo.UpdateCust(info);
                                this.InsertFS(info.FROM_USER_NAME, context);
                                var member_crm = w.UpdateMemberByBD(context.Session["FromUserName"].ToString(), zloy.E_USER_NO.ToString(), zloy);
                                _log.Info("绑定会员-更新crm会员log:" + member_crm.T_RETURN[0].TYPE);
                                context.Response.Write("{\"message\":\"绑定成功\",\"status\":\"" + 1 + "\"}");
                            }
                            else
                            {

                                context.Response.Write("{\"message\":\"手机号码不存在，请进行注册。\",\"status\":\"" + -1 + "\"}");

                            }
                        }
                        else
                        {
                            context.Response.Write("{\"message\":\"Crm手机号码不存在，请进行注册。\",\"status\":\"" + -1 + "\"}");
                        }

                    }
                    //}
                }
                else
                {
                    context.Response.Write("{\"message\":\"请先获取验证码。\",\"status\":\"" + -1 + "\"}");
                }
            }
            else if (GetQeuryString("para", context) == "getcardlist")//获取卡级列表
            {
                ORG_INFO m = mss.GetWD(context.Session["ToUserName"].ToString());
                if (m != null)
                {
                    string url = m.DOMAIN_PREFIX + "customer/card/list?wx_open_id=" + context.Session["FromUserName"].ToString();
                    string message = PostRequest(url);
                    cardlist data = JsonConvert.DeserializeObject<cardlist>(message);
                    string html = "";
                    if (data.data != null)
                    {
                        foreach (card d in data.data)
                        {
                            html += "<div class='swiper-slide'><div class='ka'><span style='font-size:28px;'>" + d.card_name + "</span><p>" + (int)Math.Round(d.balance_amt, 0) + "</p></div></div>";
                        }
                        context.Response.Write(html); ;

                    }
                    else
                    {
                        context.Response.Write("<div class='swiper-slide'><div class='ka'><span>无</span><p>0</p></div></div>");
                    }
                }
            }
            else if (GetQeuryString("para", context) == "getorderlist")//获取订单记录列表
            {
                ORG_INFO m = mss.GetWD(context.Session["ToUserName"].ToString());
                if (m != null)
                {
                    string url = m.DOMAIN_PREFIX + "customer/order/list?wx_open_id=" + context.Session["FromUserName"].ToString();
                    string message = PostRequest(url);
                    orderlilst data = JsonConvert.DeserializeObject<orderlilst>(message);
                    string html = "<div class='tit'>消费明细</div>";
                    if (data.data != null && data.data.Length > 0)
                    {
                        foreach (order d in data.data)
                        {
                            html += "<dl><dt>" + d.item_name + "</dt><dd>" + d.trans_date + " <p>" + d.curr_price + "</p></dd></dl>";
                        }
                        context.Response.Write(html); ;

                    }
                    else
                    {
                        context.Response.Write("<dl><dd><p style='left:40px;text-align: center;'>无消费记录</p></dd></dl>");
                    }
                }

            }

            //else if (GetQeuryString("para", context) == "test3")//测试
            //{
            //    string url = "http://wechat.beautyfarm.com.cn/SendMessage/SendBookTemplateMsg.do";
            //    string json = "{\"OpenID\":\"o0xwit2rY-AOwWOULtQknw-kOMLU\",\"first\":\"预约订单信息\",\"keyword1\":\"王芳\",\"keyword2\":\"南京德基店\",\"keyword3\":\"2015年04月20日 17:00\",\"keyword4\":\"面部\",\"remark\":\"感谢您选择美丽田园，我们恭候您的光临！别忘了到店后，在“我的专区”签到，完成护理后为我们的服务打分即可获得价值30元的护理免券哦~\"}";
            //    string mess = HttpXmlPostRequest(url, json, Encoding.UTF8, "application/json");
            //    context.Response.Write(mess);
            //}
            //else if (GetQeuryString("para", context) == "test4")//测试
            //{
            //    string url = "http://wechat.beautyfarm.com.cn/SendMessage/SendOrderTemplateMsg.do";
            //    string json = "{\"OpenID\":\"o0xwitywP3zTaZ6e-Yn-lk6o75FA\",\"first\":\"订单消费成功\",\"time\":\"2015-04-24\",\"org\":\"南京德基店\",\"type\":\"SPM BODY（任一部位）\",\"money\":\"380.00\",\"point\":\"0\",\"remark\":\"点击[详情]为本次护理服务打分,完成签到即可获得30元免费护理券,可直接用于下一次护理抵扣！！\",\"Orderid\":\"1962\",\"StoreNo\":\"BF0053\"}";
            //    string mess = HttpXmlPostRequest(url, json, Encoding.UTF8, "application/json");
            //    context.Response.Write(mess);
            //}
            //            else if (GetQeuryString("para", context) == "test5")//测试
            //            {
            //                string url = "http://wechat.beautyfarm.com.cn/SendMessage/SendTopTemplateMsg.do";
            //                string json = "{\"OpenID\":\"o0xwit3BZRmwOOpYuUKBiVWlVZ3Q\",\"first\":\"会员充值成功\",\"accountType\":\"会员卡号\",\"account\":\"00000000001\",\"amount\":\"1500\",\"result\":\"成功\",\"remark\":\"如有疑问，请联系美街微店\"}";
            //                string mess = HttpXmlPostRequest(url, json, Encoding.UTF8, "application/json");
            //                context.Response.Write(mess);
            //            }

            else if (GetQeuryString("para", context) == "SaveEvaluation")//评价
            {
                try
                {


                    string zongti = context.Request.Params["zongti"];
                    string jishu = context.Request.Params["jishu"];
                    string huanjing = context.Request.Params["huanjing"];
                    string fuwu = context.Request.Params["fuwu"];
                    string liuyan = context.Request.Params["liuyan"];
                    string orderid = context.Server.UrlDecode(context.Request.Params["orderid"]).Split(',')[0];
                    string storeno = context.Server.UrlDecode(context.Request.Params["orderid"]).Split(',')[1];
                    if ((context.Request.QueryString["FromUserName"] != null || context.Session["FromUserName"] != null) && (context.Request.QueryString["ToUserName"] != null || context.Session["ToUserName"] != null))
                    {
                        string user = context.Request.QueryString["FromUserName"] == null ? context.Session["FromUserName"].ToString() : context.Request.QueryString["FromUserName"].ToString();
                        string user2 = context.Request.QueryString["ToUserName"] == null ? context.Session["ToUserName"].ToString() : context.Request.QueryString["ToUserName"].ToString();
                        WD_Evaluation e = mss.GetEvaluation(orderid, user);
                        if (e == null)
                        {
                            e = new WD_Evaluation
                            {
                                ZongTi = int.Parse(zongti),
                                JiShu = int.Parse(jishu),
                                HuangJing = int.Parse(huanjing),
                                FuWu = int.Parse(fuwu),
                                LiuYan = liuyan,
                                OrderID = orderid,
                                StoreNo = storeno,
                                FromUserName = user,
                                CreateTime = DateTime.Now,
                                ToUserName = user2
                            };
                            WD_SIGN s = mss.GetSignByOpenid(user);
                            if (s == null)
                            {
                                e.SIGN_ID = 0;
                                SaveEvaluation(context, e, "评价成功"); //没有签到记录直接保存评价
                            }
                            else if (s.CreateDate.Value.ToString("yyyy-MM-dd") != DateTime.Now.ToString("yyyy-MM-dd"))
                            {
                                e.SIGN_ID = 0;
                                SaveEvaluation(context, e, "评价成功"); //签到记录不是当天直接保存评价
                            }
                            else
                            {
                                try
                                {
                                    Hmj.WebApp.TicketService.VoucherWebServiceSoapClient d = new Hmj.WebApp.TicketService.VoucherWebServiceSoapClient();
                                    string a = d.SendSignVoucherByFromUserName(user, 3880);
                                    server ser = JsonConvert.DeserializeObject<server>(a);
                                    if (ser.Code == 0)
                                    {
                                        e.SIGN_ID = s.ID;
                                        try
                                        {
                                            string url = "http://wechat.beautyfarm.com.cn/SendMessage/SendTicketTemplateMsg.do";
                                            string json = "{\"OpenID\":\"" + user + "\",\"first\":\"恭喜您获得30元护理抵用券！\",\"keyword1\":\"护理抵用券\",\"keyword2\":\"30元\",\"keyword3\":\"" + DateTime.Now.AddDays(15).ToString("yyyy年MM月dd日") + "\",\"remark\":\"非常感谢您的点评，30元护理抵用券现已计入您的会员卡内，可用于下次护理金额抵扣。5月30前，护理券抵用券累积无上限。我们期待您的再次光临！\"}";
                                            string mess = HttpXmlPostRequest(url, json, Encoding.UTF8, "application/json");
                                        }
                                        catch (Exception)
                                        {

                                        }
                                        SaveEvaluation(context, e, "评价成功，券已送达。"); //签到记录不是当天直接保存评价
                                    }
                                    else
                                    {
                                        context.Response.Write("{\"message\":\"" + ser.Msg + "。\",\"status\":\"" + -1 + "\"}");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    context.Response.Write("{\"message\":\"" + ex.Message + "。\",\"status\":\"" + -1 + "\"}");
                                }
                            }

                        }
                        else
                        {
                            context.Response.Write("{\"message\":\"该订单已评价。\",\"status\":\"" + -1 + "\"}");
                        }
                    }

                }
                catch (Exception ex)
                {
                    context.Response.Write("{\"message\":\"" + ex.Message + ex.StackTrace + "。\",\"status\":\"" + -1 + "\"}");
                }

            }

            else if (GetQeuryString("para", context) == "LoadStore") //加载门店
            {
                try
                {
                    List<WD_STORE> list = mss.GetStoreList(context.Session["ToUserName"].ToString(), context.Request.Params["city"]);

                    string Slist = "";
                    foreach (WD_STORE s in list)
                    {
                        double x = 0;
                        double y = 0;
                        bd_decrypt(s.Lat == null ? 0 : s.Lat.Value, s.Lng == null ? 0 : s.Lng.Value, ref y, ref x);
                        Slist += string.Format(@"<a class='sitem' >
				<img src='{2}' alt='' onclick=""javascript:location='{5}.aspx?tousername={4}&id={3}'"" class='p' style='border-radius: 50%;width:114px;height:114px;'>
				<dl class='dl1'  onclick=""javascript:location='{5}.aspx?tousername={4}&id={3}'"">
					<dt>{0}</dt>
					<dd class='ellipsis'>{1}</dd>
				</dl>
				<dl class='dl2'>
					<img src='images/8.png' style='width:80px;' onclick=""showlocal({6},{7},'{8}','{9}')"" alt=''></dl></a>",
                        s.STORE_NAME + "-" + s.TELEPHONE, s.ADDRESS, ConfigurationSettings.AppSettings["WebUrl"] + s.Logo, s.ID, context.Session["ToUserName"].ToString(), context.Request.QueryString["state"].ToString() == "-1" ? "booking" : "booking", x, y, s.ADDRESS, s.STORE_NAME);
                    }
                    context.Response.Write(Slist);

                }
                catch (Exception ex)
                {
                    context.Response.Write(ex.Message + "," + ex.StackTrace);
                }

            }
            else if (GetQeuryString("para", context) == "qiandao") //签到
            {
                try
                {

                    WD_Location location = mss.GetLocation(context.Session["FromUserName"].ToString());
                    if (location == null)
                    {
                        context.Response.Write("{\"message\":\"未获取到您的地理位置。\",\"status\":\"" + -1 + "\"}");
                    }
                    else
                    {
                        WD_STORE_EX store = mss.GetStoreByLocation(location.BaiduX, location.BaiduY);
                        if (store.jl > 800)
                        {
                            WD_SIGN s = new WD_SIGN { CreateDate = DateTime.Now, FromUserName = location.FromUserName, JL = store.jl, LocationID = location.ID, StoreID = store.ID, StoreName = store.STORE_NAME, StoreNO = store.STORE_NO };
                            mss.SaveSign(s);
                            context.Response.Write("{\"message\":\"距您门店太远，签到失败。\",\"status\":\"" + -1 + "\"}");
                        }
                        else
                        {
                            WD_SIGN s = new WD_SIGN { CreateDate = DateTime.Now, FromUserName = location.FromUserName, JL = store.jl, LocationID = location.ID, StoreID = store.ID, StoreName = store.STORE_NAME, StoreNO = store.STORE_NO };
                            mss.SaveSign(s);
                            context.Response.Write("{\"message\":\"成功签到" + store.STORE_NAME + "。\",\"status\":\"" + 1 + "\"}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    context.Response.Write("{\"message\":\"意外。\",\"status\":\"" + -1 + "\"}");
                }

            }
            else if (GetQeuryString("para", context) == "getsign") //获取记录签到
            {
                try
                {

                    if ((context.Request.QueryString["FromUserName"] != null || context.Session["FromUserName"] != null) && (context.Request.QueryString["ToUserName"] != null || context.Session["ToUserName"] != null))
                    {
                        string user = context.Request.QueryString["FromUserName"] == null ? context.Session["FromUserName"].ToString() : context.Request.QueryString["FromUserName"].ToString();
                        string user2 = context.Request.QueryString["ToUserName"] == null ? context.Session["ToUserName"].ToString() : context.Request.QueryString["ToUserName"].ToString();
                        WD_SIGN s = mss.GetSignByOpenid(user);
                        if (s == null)
                        {
                            context.Response.Write("{\"message\":\"当天没有签到记录，评价后无法获得护理券;<br>到\\\"我的信息\\\"中签到后再评价将获得30元券。\",\"status\":\"1\"}");
                        }
                        else
                        {
                            if (s.CreateDate.Value.ToString("yyyy-MM-dd") != DateTime.Now.ToString("yyyy-MM-dd"))
                            {
                                context.Response.Write("{\"message\":\"当天没有签到记录，评价后无法获得护理券;<br>到\\\"我的信息\\\"中签到后再评价将获得30元券。\",\"status\":\"1\"}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    context.Response.Write("{\"message\":\"" + ex.Message + "\",\"status\":\"" + -1 + "\"}");
                }

            }
            else if (GetQeuryString("para", context) == "getorderlist2") //获取订单列表
            {
                try
                {

                    if ((context.Request.QueryString["FromUserName"] != null || context.Session["FromUserName"] != null) && (context.Request.QueryString["ToUserName"] != null || context.Session["ToUserName"] != null))
                    {
                        string user = context.Request.QueryString["FromUserName"] == null ? context.Session["FromUserName"].ToString() : context.Request.QueryString["FromUserName"].ToString();
                        string user2 = context.Request.QueryString["ToUserName"] == null ? context.Session["ToUserName"].ToString() : context.Request.QueryString["ToUserName"].ToString();
                        ORG_INFO m = mss.GetWD(context.Session["ToUserName"].ToString());
                        if (m != null)
                        {
                            string url = m.DOMAIN_PREFIX + "customer/order/list?wx_open_id=" + context.Session["FromUserName"].ToString() + "&org_no=beautyfarm";
                            string mes = PostRequest(url);
                            orderdata orderdata = JsonConvert.DeserializeObject<orderdata>(mes);
                            List<WD_Evaluation_EX> elist = mss.GetMyEvaluationList(user);
                            List<orderlist2> olist = orderdata.data.ToList();
                            foreach (WD_Evaluation_EX v in elist)
                            {
                                foreach (orderlist2 o in olist)
                                {
                                    if (o.id.ToString() == v.OrderID)
                                    {
                                        olist.Remove(o);
                                        break;
                                    }
                                }
                            }
                            string html = "";
                            foreach (orderlist2 l in olist)
                            {
                                html += "<li class='redse' onclick=\"javascript:window.location.href='Evaluation.aspx?tousername=" + user2 + "&id=" + l.id + "," + l.store_no + "'\"><span><bdo class='redse'>待评价</bdo><a href='Evaluation.aspx?tousername=" + user2 + "&id=" + l.id + "," + l.store_no + "'>" + l.store_name + "</a></span><strong><em></em>" + l.trans_date + "</strong></li>";
                            }
                            context.Response.Write(html);
                        }
                    }
                }
                catch (Exception ex)
                {
                    context.Response.Write("");
                }

            }
        }

        /// <summary>
        /// 验证会员信息
        /// </summary>
        class getmeminfo
        {
            public int status { get; set; }
            public string message { get; set; }
            public meminfo data { get; set; }
        }
        /// <summary>
        /// 会员信息
        /// </summary>
        class meminfo
        {
            public int id { get; set; }
            public string name { get; set; }
            public string mobile { get; set; }
            public string card_no { get; set; }
        }

        /// <summary>
        /// 获取订单
        /// </summary>
        class orderdata
        {
            public int status { get; set; }
            public string msg { get; set; }
            public orderlist2[] data { get; set; }
        }

        /// <summary>
        /// 订单列表
        /// </summary>
        class orderlist2
        {
            public int id { get; set; }
            public string order_no { get; set; }
            public string trans_date { get; set; }
            public string store_no { get; set; }
            public string store_name { get; set; }
        }

        /// <summary>
        /// 保存评价
        /// </summary>
        /// <param name="context"></param>
        /// <param name="e"></param>
        private void SaveEvaluation(HttpContext context, WD_Evaluation e, string message)
        {
            mss.SaveEvaluation(e);
            context.Response.Write("{\"message\":\"" + message + "\",\"status\":\"" + 1 + "\"}");
        }

        const double pi = 3.14159265358979324;
        const double a = 6378245.0;
        const double ee = 0.00669342162296594323;
        const double x_pi = 3.14159265358979324 * 3000.0 / 180.0;
        /// <summary>
        /// 百度坐标转谷歌坐标（火星坐标）
        /// </summary>
        /// <param name="bd_lat"></param>
        /// <param name="bd_lon"></param>
        /// <param name="gg_lat"></param>
        /// <param name="gg_lon"></param>
        public void bd_decrypt(double bd_lat, double bd_lon, ref double gg_lat, ref double gg_lon)
        {

            double x = bd_lon - 0.0065, y = bd_lat - 0.006;

            double z = Math.Sqrt(x * x + y * y) - 0.00002 * Math.Sin(y * x_pi);

            double theta = Math.Atan2(y, x) - 0.000003 * Math.Cos(x * x_pi);

            gg_lon = z * Math.Cos(theta);

            gg_lat = z * Math.Sin(theta);

        }

        #region js接口调用
        /// <summary>
        /// 获取最新JSAPI_TICKET凭证
        /// </summary>
        /// <param name="ToUserName"></param>
        /// <returns></returns>
        public string GetJSAPI_Ticket(string Token, ORG_INFO m)
        {
            string JSapi_ticket = "";
            if (m.JSapi_Ticket != "" && m.JSapi_Ticket != null && (m.GetTicketTime == null ? DateTime.Now.AddHours(-3) : m.GetTicketTime.Value).AddHours(2) > DateTime.Now) //不为空，并且获取时间没有超过2小时
            {
                return m.JSapi_Ticket;
            }
            else
            {
                JSapi_ticket = m.JSapi_Ticket;
                string url = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + Token + "&type=jsapi";
                string b = PostRequest(url);
                tickresult ticket = JsonConvert.DeserializeObject<tickresult>(b);
                if (ticket.errcode == 0)  //正确
                {
                    m.JSapi_Ticket = ticket.ticket;
                    m.GetTicketTime = DateTime.Now;
                    mss.SaveMD(m);
                    return m.JSapi_Ticket;
                }
                return "";
            }
        }

        /// <summary>
        /// jsapi_ticket
        /// </summary>
        class tickresult
        {
            public int errcode { get; set; }
            public string errmsg { get; set; }
            public string ticket { get; set; }
            public int expires_in { get; set; }
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

        #endregion

        /// <summary>
        /// 保存预约，推送消息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mdid"></param>
        /// <param name="yyxm"></param>
        /// <param name="empno"></param>
        /// <param name="time"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="sid"></param>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <param name="data"></param>
        private void SaveBooking(HttpContext context, string mdid, string yyxm, string empno, string time, string name, string phone, string sid, string sname, result data)
        {
            try
            {
                WD_BOOKING book = new WD_BOOKING
                {
                    ARRIVE_TIME = DateTime.Parse(time),
                    BOOKING_NUM = 1,
                    CONFIRM_DATE = DateTime.Now,
                    CONFIRM_EMPLOYEE = 0,
                    CREATE_DATE = DateTime.Now,
                    CREATE_USER = "wechat",
                    CUST_MOBILE = phone,
                    CUST_NAME = name,
                    EMPLOYEE_ID = empno,
                    FromUserName = context.Session["FromUserName"].ToString(),
                    IS_WX_CONFIRM = false,
                    LAST_MODI_DATE = DateTime.Now,
                    LAST_MODI_USER = "",
                    PosID = (data == null ? 0 : data.data),
                    REMARK = "",
                    STORE_ID = int.Parse(sid),
                    ServerType = yyxm,
                    STATUS = 0,
                    STORE_NAME = mdid.ToString(),
                    WX_CONFIRM_DATE = DateTime.Now,
                    WX_CONFIRM_STAFF = 0
                };
                mss.SaveBooking(book);//保存本地预约

            }
            catch (Exception)
            {

            }
            //给商户管理员推送消息
            //if (sid != "")
            //{
            //    List<WD_STORE_SET> wslist = mss.GetStoreSetList(int.Parse(sid));
            //    foreach (WD_STORE_SET s in wslist)
            //    {
            //        string url = "http://www.meijiewd.com/SendMessage/SendMJBookMsg.do";
            //        string json = "{\"OpenID\":\"" + s.FromUserName + "\",\"first\":\"新预约提醒\",\"keyword1\":\"" + name + "\",\"keyword2\":\"" + sname + "\",\"keyword3\":\"" + time + "\",\"keyword4\":\"新预约提醒\",\"posid\":\"" + data.data + "\",\"storeid\":\"" + mdid + "\",\"sid\":\"" + sid + "\",\"remark\":\"有预约啦，快去看看吧！\"}";
            //        string mess = HttpXmlPostRequest(url, json, Encoding.UTF8, "application/json");
            //    }
            //}
            string url = "http://wechat.beautyfarm.com.cn/SendMessage/SendBookTemplateMsg.do";
            string json = "{\"OpenID\":\"" + context.Session["FromUserName"].ToString() + "\",\"first\":\"预约申请已提交\",\"keyword1\":\"" + name + "\",\"keyword2\":\"" + sname + "\",\"keyword3\":\"" + time + "\",\"keyword4\":\"" + yyxm + "\",\"remark\":\"我们已经收到您的预约申请，门店会尽快同您联系确认，非常感谢您对美丽田园的支持！愿您永远健康美丽。\"}";
            string mess = HttpXmlPostRequest(url, json, Encoding.UTF8, "application/json");
        }

        public string GetQeuryString(string para, HttpContext context)
        {
            if (context.Request.QueryString[para] != null) return context.Request.QueryString[para].ToString();
            else return "";
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        //获取微信会员绑定管家
        private GROUP_EMPLOYER_EX GetEmployeeT(string fromusername)
        {
            GROUP_EMPLOYER_EX group = new GROUP_EMPLOYER_EX();
            group.NAME = "";
            group.MOBILE = "";
            group.STORENAME = "";
            GROUP_EMPLOYER_EX e = sbo.GetStoreNameByFromUserName(fromusername);
            if (e != null)
            {
                group = e;
            }
            return group;
        }

        public Z_LOY_BP_CREATEResponse InsertMember(string tname, string tphone, string tstorename, string name, string phone, string sex, string sf, string city, string sr, string fromusername)
        {
            ZWS_Z_LOY_BP_CREATEClient client = new ZWS_Z_LOY_BP_CREATEClient();
            Z_LOY_BP_CREATE param = new Z_LOY_BP_CREATE();
            ZZE_ORGAN organ = new ZZE_ORGAN();
            organ.NAME1 = tname;//管家姓名
            organ.NAME2 = tphone;//管家号码
            Hmj.WebApp.InsertMemberNew.ZZCENTRAL item = new Hmj.WebApp.InsertMemberNew.ZZCENTRAL();
            item.SEARCHTERM1 = "";
            item.SEARCHTERM2 = "";
            item.TITLELETTER = tstorename;//门店名称
            item.TITLE_KEY = sex;  //代表男士
            item.DATAORIGINTYPE = "Z002"; //代表来自微信
            param.I_CENTRAL = item;
            param.I_ORGAN = organ;
            Hmj.WebApp.InsertMemberNew.ZZCENTRALDATAPERSON item3 = new Hmj.WebApp.InsertMemberNew.ZZCENTRALDATAPERSON();
            item3.BIRTHDATE = sr;
            item3.LASTNAME = name;
            item3.PREFIX1 = "";
            item3.BIRTHPLACE = "";
            item3.MARITALSTATUS = "";
            item3.TITLE_ACA1 = "";
            item3.TITLE_ACA2 = "";
            item3.TITLE_SPPL = "";
            item3.BIRTHNAME = "";
            item3.FIRSTNAME = "";
            item3.OCCUPATION = "Z001";
            item3.MIDDLENAME = "";
            item3.SECONDNAME = "";
            param.I_CENTRALDATAPERSON = item3;

            Hmj.WebApp.InsertMemberNew.ZZADDRESSDATA item4 = new Hmj.WebApp.InsertMemberNew.ZZADDRESSDATA();
            item4.COUNTRY = "CN";
            item4.REGION = sf;
            item4.CITY = city;
            item4.STREET = "";
            item4.POSTL_COD1 = "";
            item4.STR_SUPPL1 = "";
            item4.STR_SUPPL2 = "";
            item4.STR_SUPPL3 = DateTime.Now.ToString("yyyyMMdd");
            item4.LOCATION = fromusername;//暂时用短的代替
            param.I_ADDRESSDATA = item4;


            Hmj.WebApp.InsertMemberNew.ZZTELEFONDATA item5 = new Hmj.WebApp.InsertMemberNew.ZZTELEFONDATA();
            item5.TELEPHONE = phone;
            param.I_TELEFONDATA = item5;

            Hmj.WebApp.InsertMemberNew.ZZE_MAILDATA item10 = new Hmj.WebApp.InsertMemberNew.ZZE_MAILDATA();
            item10.E_MAIL = "";
            param.I_E_MAILDATA = item10;



            //ZZCUSTOMER01 item6 = new ZZCUSTOMER01();
            //item6.ZA01 = "";
            //item6.ZA02 = "";
            //item6.ZA03 = "";
            //item6.ZA04 = "";
            //item6.ZA05 = "";
            //item6.ZA06 = "";
            //item6.ZA07 = "";
            //item6.ZA08 = "";
            //item6.ZA09 = "";
            //item6.ZA10 = "";
            //param.I_CUSTOMER01 = item6;


            //ZZCUSTOMER02 item7 = new ZZCUSTOMER02();
            //item7.ZA11 = "";
            //item7.ZA12 = "";
            //item7.ZA13 = "";
            //item7.ZA14 = "";
            //param.I_CUSTOMER02 = item7;


            //ZZCUSTOMER03 item8 = new ZZCUSTOMER03();
            //item8.ZA15 = "";
            //item8.ZA16 = "";
            //item8.ZA17 = "";
            //item8.ZA18 = "";
            //item8.ZA19 = "";
            //item8.ZA20 = "";
            //item8.ZA21 = "";
            //item8.ZA22 = "";
            //item8.ZA23 = "";
            //item8.ZA24 = "";
            //item8.ZA25 = "";
            //param.I_CUSTOMER03 = item8;


            //ZZCUSTOMER04 item9 = new ZZCUSTOMER04();
            //item9.ZA26 = "";
            //item9.ZA27 = "";
            //item9.ZA28 = "";
            //item9.ZA29 = "";
            //item9.ZA30 = "";
            //item9.ZA31 = "";
            //item9.ZA32 = "";
            //item9.ZA33 = "";
            //param.I_CUSTOMER04 = item9;

            Hmj.WebApp.InsertMemberNew.ZZCUSTOMER06 item11 = new Hmj.WebApp.InsertMemberNew.ZZCUSTOMER06();
            item11.ZA39 = "X";
            item11.ZA40 = "X";
            item11.ZA41 = "X";
            item11.ZA42 = "X";
            item11.ZA43 = "X";
            item11.ZA44 = "X";
            param.I_CUSTOMER06 = item11;


            //List<Hmj.WebApp.InsertMember.BAPIRET2> list = new List<Hmj.WebApp.InsertMember.BAPIRET2>();
            //Hmj.WebApp.InsertMember.BAPIRET2 item2 = new Hmj.WebApp.InsertMember.BAPIRET2();
            //list.Add(item2);
            //param.T_RETURN = list.ToArray();
            param.T_RETURN = new Hmj.WebApp.InsertMemberNew.ZZBP_RETURN[] { };
            var response = client.Z_LOY_BP_CREATE(param);
            return response;
        }

        public bool WriteTxt(string str)
        {
            ISystemService sbo = new SystemService();
            try
            {
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

        public string GetWeek(DateTime time)
        {
            string[] Day = new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
            string week = Day[Convert.ToInt32(time.DayOfWeek.ToString("d"))].ToString();
            return week;
        }

        public static string HttpXmlPostRequest(string postUrl, string postXml, Encoding encoding, string contype = "text/xml")
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
            request.ContentType = contype;

            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(byteArray, 0, byteArray.Length);
            }

            using (var responseStream = request.GetResponse().GetResponseStream())
            {
                return new StreamReader(responseStream, encoding).ReadToEnd();
            }
        }

        public string PostRequest(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);  //定义请求对象，并设置好请求URL地址
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();    //定义响应对象，request在调用GetResponse方法事执行请求了，而不是在HttpWebRequest.Create的时候执行。       
            Stream stream = response.GetResponseStream(); //定义一个流对象，来获取响应流
            StreamReader sr = new StreamReader(stream);  //定义一个流读取对象，读取响应流
            string responseHTML = sr.ReadToEnd();
            return responseHTML;
        }


        //插入粉丝信息
        private void InsertFS(string FromUserName, HttpContext context)
        {
            try
            {
                string ToUserName = context.Request.QueryString["ToUserName"] == null ? context.Session["ToUserName"].ToString() : context.Request.QueryString["ToUserName"].ToString();
                //获取Token字符串
                string access_token = new WeiPage().Token(ToUserName);
                //获取用户信息列表
                string info = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + access_token + "&openid=" + FromUserName + "&lang=zh_CN";
                string infomes = HttpRequestHelper.HttpGetRequest(info);
                OpenInfo winfo = JsonConvert.DeserializeObject<OpenInfo>(infomes);

                //把用户的信息存到粉丝表,保存之前先判断粉丝表里面有没有这个用户
                WXCUST_FANS fans = sbo.GetFansByFromUserName(FromUserName);
                //未关注时更新状态
                if (fans == null)
                {
                    fans = new WXCUST_FANS
                    {
                        NAME = winfo.nickname,
                        GENDER = winfo.sex == "1" ? true : false,
                        COUNTRY = winfo.country,
                        PROVINCE = winfo.province,
                        CITY = winfo.city,
                        FROMUSERNAME = winfo.openid,
                        ToUserName = ToUserName,
                        IMAGE = winfo.headimgurl,
                        STATUS = 1,
                        NOTICE_DATE = UnixTimeToTime(winfo.subscribe_time),
                        CANCEL_DATE = DateTime.Parse("1900-01-01"),
                        CREATE_DATE = DateTime.Now,
                        CREATE_USER = "system",
                        LAST_CONN_DATE = DateTime.Parse("1900-01-01"),
                        LAST_MODI_DATE = DateTime.Parse("1900-01-01"),
                        REMARK = "",
                        LAST_MODI_USER = "system",
                        AVAL_OPPR = 1,
                        TOTAL_OPPR = 1,
                        UNIN_CODE = "",
                        REFE_CODE = "",
                        IS_BUYER = false
                    };
                    fans.STATUS = 1;

                    sbo.SaveCustFans(fans);
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
            }

        }
        private DateTime UnixTimeToTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        class result
        {
            public int status { get; set; }
            public string message { get; set; }
            public int? data { get; set; }
        }

        /// <summary>
        /// 预约记录列表
        /// </summary>
        class booklist
        {
            public book_ex[] data { get; set; }
        }
        /// <summary>
        /// 预约记录
        /// </summary>
        //class book
        //{
        //    public string cust_name { get; set; }
        //    public string cust_mobile { get; set; }
        //    public string begin_time { get; set; }
        //    public string end_time { get; set; }
        //    public string service_item { get; set; }
        //    public string booking_num { get; set; }
        //    public string store_id { get; set; }
        //    public string store_name { get; set; }
        //    public string store_tel { get; set; }
        //    public string store_addr { get; set; }
        //    /// <summary>
        //    /// 0：未处理；1：确认；2：已到店；3：已取消
        //    /// </summary>
        //    public int state { get; set; }
        //}
        /// <summary>
        /// 卡级列表
        /// </summary>
        class cardlist
        {
            public card[] data { set; get; }
        }
        /// <summary>
        /// 卡级
        /// </summary>
        class card
        {
            public string card_name { get; set; }
            public decimal service_discount { get; set; }
            public decimal product_discount { get; set; }
            public decimal lccard_discount { set; get; }
            public string begin_date { get; set; }
            public string end_date { get; set; }
            public decimal total_amt { get; set; }
            public decimal balance_amt { set; get; }
        }
        /// <summary>
        /// 订单记录列表
        /// </summary>
        class orderlilst
        {
            public order[] data { get; set; }
        }
        /// <summary>
        /// 订单记录
        /// </summary>
        class order
        {
            public string order_no { get; set; }
            public string trans_date { get; set; }
            public string item_id { get; set; }
            public decimal ori_price { get; set; }
            public decimal curr_price { get; set; }
            public int number { get; set; }
            public string emp_name { get; set; }
            public string item_name { get; set; }
            public string store_name { get; set; }
        }

        class server
        {
            public int Code { get; set; }
            public string Msg { get; set; }
        }



    }
}