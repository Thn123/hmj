using Hmj.Business.ServiceImpl;
using Hmj.Common;
using Hmj.DTO;
using Hmj.Entity;
using Hmj.Interface;
using Hmj.WebApp.DicountService;
using Hmj.WebApp.SelectMemberNew;
using Hmj.WebApp.SelectOrderNew;
using Hmj.WebApp.UpdateMemberNew;
using Newtonsoft.Json;
using StructureMap.Attributes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

/// <summary>
///WeiPage 的摘要说明
/// </summary>
public class WeiPage : System.Web.UI.Page
{
    ISystemService sbo = new SystemService();
    IBcjStoreService bcj = new BcjStoreService();
    public MySmallShopService mss = new MySmallShopService();
    public ILogService log = new LogService();

    public HmjMemberService _hmjMember = new HmjMemberService();

    public WeiPage()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }
    public void Base()
    {
        if (Request.QueryString["code"] != null)
        {
            try
            {
                string url = string.Format(@"https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + GetAppid(AppConfig.FWHOriginalID)
                    + "&secret=" + GetSecret(AppConfig.FWHOriginalID) +
                    "&code={0}&grant_type=authorization_code",
                    Request.QueryString["code"].ToString());
                string token = PostRequest(url);
                if (token.Contains("7200"))
                {
                    string[] b = token.Split('\"');
                    Session["FromUserName"] = b[13];
                    Session["ToUserName"] = AppConfig.FWHOriginalID;
                }
            }
            catch (Exception)
            {

            }
        }
    }


    public void tagopenid(string openid)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();

        dic.Add("openid", openid);

        JsonDTO<bool> result = RequestHelp.RequestGet<bool>("Member/ChckBind", dic).Result;

        //如果没有绑定会员才会打标签
        if (!result.Data)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/tags/members/batchtagging?access_token="
            + Token(AppConfig.FWHOriginalID);
            string json = "{\"openid_list\":[\"" + openid + "\"],\"tagid\" : 100 }";
            var resMessage = HttpXmlPostRequest(url, json, Encoding.UTF8);
        }
    }

    public string WebUrl
    {
        get { return ConfigurationSettings.AppSettings["WebUrl"]; }
    }

    public void BL()
    {
        //if (IsTest) //如果是测试
        //{
        //Session["FromUserName"] = oneopenid;
        //Session["ToUserName"] = mjuserid;
        if ((Request.QueryString["FromUserName"] != null || Session["FromUserName"] != null) && (Request.QueryString["ToUserName"] != null || Session["ToUserName"] != null))
        {
            string user = Request.QueryString["FromUserName"] == null ? Session["FromUserName"].ToString() : Request.QueryString["FromUserName"].ToString();
            string user2 = Request.QueryString["ToUserName"] == null ? Session["ToUserName"].ToString() : Request.QueryString["ToUserName"].ToString();
            Session["FromUserName"] = user;
            Session["ToUserName"] = user2;
        }
        //}
        if (Session["FromUserName"] == null && Session["ToUserName"] == null)
        {
            HttpCookie cookie = Request.Cookies["cookiedtmoon"];
            if (cookie == null)
            {
                cookie = new HttpCookie("cookiedtmoon");
                cookie.Expires = DateTime.Now.AddDays(3);
                cookie.Name = "cookiedtmoon";
                cookie.Value = DateTime.Now.AddDays(3).ToString("yyyy.MM.dd");
                HttpContext.Current.Response.Cookies.Add(cookie);
                BaseLoad2();
            }
            else
            {
                DateTime dt = DateTime.Parse(cookie.Value);
                if (dt < DateTime.Now)
                {
                    cookie.Value = DateTime.Now.AddDays(3).ToString("yyyy.MM.dd");
                    cookie.Expires = DateTime.Now.AddDays(3);
                    HttpContext.Current.Response.Cookies.Add(cookie);
                    BaseLoad2();
                }
                else
                {
                    BaseLoad();
                }
            }
        }
        Page.RegisterStartupScript("hiddenurl", "<input type=\"hidden\" value=\"" + Server.UrlEncode(AbsoluteUri) + "\" id=\"url\" />");
        //Response.Write("<input type=\"hidden\" value=\"" + Server.UrlEncode(AbsoluteUri) + "\" id=\"url\" />");
    }

    public string OpenID
    {
        get
        {
            if ((Request.QueryString["FromUserName"] != null || Session["FromUserName"] != null) && (Request.QueryString["ToUserName"] != null || Session["ToUserName"] != null))
            {
                string user = Request.QueryString["FromUserName"] == null ? Session["FromUserName"].ToString() : Request.QueryString["FromUserName"].ToString();
                string user2 = Request.QueryString["ToUserName"] == null ? Session["ToUserName"].ToString() : Request.QueryString["ToUserName"].ToString();
                Session["FromUserName"] = user;
                Session["ToUserName"] = user2;
                return user;
            }
            else return null;
        }
    }

    /// <summary>
    /// 当前页面访问地址
    /// </summary>
    /// <returns></returns>
    public string AbsoluteUri
    {
        get
        {
            return Server.UrlEncode(System.Web.HttpContext.Current.Request.Url.AbsoluteUri);
        }
    }

    /// <summary>
    /// 官方美街微店微信账号
    /// </summary>
    /// <returns></returns>
    public string mjuserid { get { return AppConfig.FWHOriginalID; } }

    /// <summary>
    /// 一个粉丝openid
    /// </summary>
    public string oneopenid { get { return "oS7pm1iNL2P2pjdgHO3xC2NRdWE8"; } }

    //MySmallShopService mss = new MySmallShopService();
    public string tousername = "";
    public string versions = "3";//css版本号
    private static ILoginService _ILoginService;
    private static ILoginService CLoginService
    {
        get
        {
            if (_ILoginService == null)
            {
                _ILoginService = ServiceFactory.GetInstance<ILoginService>();
            }
            return _ILoginService;
        }
    }

    public ORG_INFO GetNewWD
    {
        get
        {


            ORG_INFO WD = CLoginService.GetMerchants(mjuserid);
            System.Web.HttpContext.Current.Session["WD"] = WD;
            return WD;

            return new ORG_INFO { AppID = "" };

        }
    }

    /// <summary>
    /// 每次需要重新授权，获取最新的昵称和头像
    /// </summary>
    public void BaseLoad2()
    {
        if ((System.Web.HttpContext.Current.Session["ToUserName"] == null && System.Web.HttpContext.Current.Session["FromUserName"] == null) || (System.Web.HttpContext.Current.Session["FromUserName"] != null && mss.GetOAByDay(System.Web.HttpContext.Current.Session["FromUserName"].ToString()) == 0)) //如果当天没有授权过，则重新授权，获取最新头像
        {
            tousername = mjuserid;//WXLOG wlog = new WXLOG { TIME = DateTime.Now };
            if (System.Web.HttpContext.Current.Request.QueryString["code"] != null)
            {
                //mss.SaveLog(new WXLOG { CON = System.Web.HttpContext.Current.Request.QueryString["code"], TIME = DateTime.Now });
                try
                {
                    string url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + GetAppid(tousername) + "&secret=" + GetSecret(tousername) + "&code=" + (System.Web.HttpContext.Current.Request.QueryString["code"].ToString().Split(',').Length > 1 ? System.Web.HttpContext.Current.Request.QueryString["code"].ToString().Split(',')[1] : System.Web.HttpContext.Current.Request.QueryString["code"].ToString().Split(',')[0]) + "&grant_type=authorization_code";
                    string token = PostRequest(url);
                    // mss.SaveLog(new WXLOG { CON = token, TIME = DateTime.Now });
                    if (token.Contains("7200") || token.Contains("expires_in"))
                    {
                        OpenInfo autho = JsonConvert.DeserializeObject<OpenInfo>(token);
                        System.Web.HttpContext.Current.Session["FromUserName"] = autho.openid;
                        System.Web.HttpContext.Current.Session["ToUserName"] = tousername;
                        // mss.SaveLog(new WXLOG { CON = System.Web.HttpContext.Current.Session["ToUserName"].ToString(), TIME = DateTime.Now });
                        try
                        {
                            if (autho.scope == "snsapi_userinfo")
                            {
                                OAauth_Log oa = new OAauth_Log();
                                oa.CreateDate = DateTime.Now;
                                oa.FromUserName = autho.openid;
                                oa.ToUserName = tousername;
                                url = "https://api.weixin.qq.com/sns/userinfo?access_token=" + autho.access_token + "&openid=" + autho.openid + "&lang=zh_CN";
                                token = PostRequest(url);
                                autho = JsonConvert.DeserializeObject<OpenInfo>(token);
                                oa.headimgurl = autho.headimgurl;
                                oa.Nickname = autho.nickname;
                                //oa.sex = autho.sex;
                                //oa.country = autho.country;
                                //oa.province = autho.province;
                                //oa.city = autho.city;
                                OAauth_Log oa1 = mss.GetOA(autho.openid);
                                if (oa1 == null)
                                {
                                    //DownHeadImage(oa);
                                    mss.SaveOAtuh(oa);
                                }
                                else
                                {
                                    oa1.headimgurl = oa.headimgurl;
                                    oa1.Nickname = oa.Nickname;
                                    //oa1.sex = oa.sex;
                                    //oa1.country = oa.country;
                                    //oa1.province = oa.province;
                                    //oa1.city = oa.city;
                                    oa1.CreateDate = DateTime.Now;
                                    //if (!string.IsNullOrEmpty(oa1.DownPic))
                                    //    DeleteHeadImage(oa1.DownPic);
                                    //DownHeadImage(oa1);
                                    mss.SaveOAtuh(oa1);
                                }
                            }
                            else
                            {
                                OAuth(GetNewWD.AppID, System.Web.HttpContext.Current.Request.Url.AbsoluteUri);
                            }

                        }
                        catch (Exception ex)
                        {
                            WXLOG log = new WXLOG();
                            log.CON = ex.Message.ToString();
                            log.TIME = DateTime.Now;
                            mss.SaveLog(log);
                        }
                    }
                    else
                    {
                        OAuth(GetNewWD.AppID, System.Web.HttpContext.Current.Request.Url.AbsoluteUri);
                    }
                }
                catch (Exception ex)
                {
                    mss.SaveLog(new WXLOG { CON = ex.Message + ex.StackTrace, TIME = DateTime.Now });
                }
                //System.Web.HttpContext.Current.Response.Write(" <input type='hidden'' value='" + AbsoluteUri + "' id='url' />");
            }
            else
            {
                OAuth(GetNewWD.AppID, System.Web.HttpContext.Current.Request.Url.AbsoluteUri);
            }
        }
        else
        {
            //System.Web.HttpContext.Current.Response.Write(" <input type='hidden'' value='" + AbsoluteUri + "' id='url' />");
        }
    }

    /// <summary>
    /// 只需授权一次
    /// </summary>
    public void BaseLoad()
    {
        if ((System.Web.HttpContext.Current.Session["ToUserName"] == null && System.Web.HttpContext.Current.Session["FromUserName"] == null) || (System.Web.HttpContext.Current.Session["FromUserName"] != null && mss.GetISOA(System.Web.HttpContext.Current.Session["FromUserName"].ToString()) == 0))
        {
            tousername = mjuserid;
            if (System.Web.HttpContext.Current.Request.QueryString["code"] != null)
            {
                try
                {
                    string url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + GetAppid(tousername) + "&secret=" + GetSecret(tousername) + "&code=" + (System.Web.HttpContext.Current.Request.QueryString["code"].ToString().Split(',').Length > 1 ? System.Web.HttpContext.Current.Request.QueryString["code"].ToString().Split(',')[1] : System.Web.HttpContext.Current.Request.QueryString["code"].ToString().Split(',')[0]) + "&grant_type=authorization_code";
                    string token = PostRequest(url);
                    //  mss.SaveLog(new WXLOG { CON = token, TIME = DateTime.Now });
                    if (token.Contains("7200") || token.Contains("expires_in"))
                    {
                        OpenInfo autho = JsonConvert.DeserializeObject<OpenInfo>(token);
                        System.Web.HttpContext.Current.Session["FromUserName"] = autho.openid;
                        System.Web.HttpContext.Current.Session["ToUserName"] = tousername;
                        try
                        {
                            if (autho.scope == "snsapi_userinfo")
                            {
                                OAauth_Log oa = new OAauth_Log();
                                oa.CreateDate = DateTime.Now;
                                oa.FromUserName = autho.openid;
                                oa.ToUserName = tousername;
                                url = "https://api.weixin.qq.com/sns/userinfo?access_token=" + autho.access_token + "&openid=" + autho.openid + "&lang=zh_CN";
                                token = PostRequest(url);
                                autho = JsonConvert.DeserializeObject<OpenInfo>(token);
                                oa.headimgurl = autho.headimgurl;
                                oa.Nickname = autho.nickname;
                                //oa.sex = autho.sex;
                                //oa.country = autho.country;
                                //oa.province = autho.province;
                                //oa.city = autho.city;
                                OAauth_Log oa1 = mss.GetOA(autho.openid);
                                if (oa1 == null)
                                {
                                    //DownHeadImage(oa);
                                    mss.SaveOAtuh(oa);
                                }
                                else
                                {
                                    oa1.headimgurl = oa.headimgurl;
                                    oa1.Nickname = oa.Nickname;
                                    //oa1.sex = oa.sex;
                                    //oa1.country = oa.country;
                                    //oa1.province = oa.province;
                                    //oa1.city = oa.city;
                                    oa1.CreateDate = DateTime.Now;
                                    //if (!string.IsNullOrEmpty(oa1.DownPic))
                                    //    DeleteHeadImage(oa1.DownPic);
                                    //DownHeadImage(oa1);
                                    mss.SaveOAtuh(oa1);
                                }
                            }
                            else
                            {
                                OAauth_Log oa = mss.GetOA(autho.openid);
                                if (oa == null)
                                {
                                    OAuth(GetNewWD.AppID, System.Web.HttpContext.Current.Request.Url.AbsoluteUri);
                                }
                                //else if (oa.DownPic == null)
                                //{
                                //    OAuth(GetNewWD.AppID, System.Web.HttpContext.Current.Request.Url.AbsoluteUri);
                                //}
                            }

                        }
                        catch (Exception ex)
                        {
                            WXLOG log = new WXLOG();
                            log.CON = ex.Message.ToString();
                            log.TIME = DateTime.Now;
                            mss.SaveLog(log);
                        }
                    }
                    else
                    {
                        OAuth(GetNewWD.AppID, System.Web.HttpContext.Current.Request.Url.AbsoluteUri);
                    }
                }
                catch (Exception)
                {

                }
                //System.Web.HttpContext.Current.Response.Write(" <input type='hidden'' value='" + AbsoluteUri + "' id='url' />");
            }
            else
            {
                OAuth2(GetNewWD.AppID, System.Web.HttpContext.Current.Request.Url.AbsoluteUri);
            }
        }
        else
        {
            //System.Web.HttpContext.Current.Response.Write(" <input type='hidden'' value='" + AbsoluteUri + "' id='url' />");
        }
    }

    /// <summary>
    /// 授权 snsapi_userinfo方式  需要用户点击授权  可获取用户详细信息
    /// </summary>
    /// <param name="appid">公众号APPID</param>
    /// <param name="redirect_uri">回调地址</param>
    public void OAuth(string appid, string redirect_uri)
    {
        if (appid != "")
        {
            string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={1}&redirect_uri={2}&st=1&response_type=code&scope=snsapi_userinfo&state={3}&component_appid={0}#wechat_redirect";
            url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state={2}#wechat_redirect";
            url = string.Format(url, appid, redirect_uri, System.Web.HttpContext.Current.Request.Params["id"] != null ? System.Web.HttpContext.Current.Request.Params["id"] : "0");
            System.Web.HttpContext.Current.Response.Redirect(url, false);
        }
        else
        {
            System.Web.HttpContext.Current.Response.Redirect("/wechat/500.jpg");
        }
    }

    /// <summary>
    /// 授权 snsapi_base方式  不需要用户点击授权  只可获取用户openid
    /// </summary>
    /// <param name="appid">公众号APPID</param>
    /// <param name="redirect_uri">回调地址</param>
    public void OAuth2(string appid, string redirect_uri)
    {
        if (appid != "")
        {
            string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={1}&redirect_uri={2}&st=2&response_type=code&scope=snsapi_base&state={3}&component_appid={0}#wechat_redirect";
            url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state={2}#wechat_redirect";
            url = string.Format(url, appid, redirect_uri, System.Web.HttpContext.Current.Request.Params["id"] != null ? System.Web.HttpContext.Current.Request.Params["id"] : "0");
            System.Web.HttpContext.Current.Response.Redirect(url, false);
        }
        else
        {
            System.Web.HttpContext.Current.Response.Redirect("/wechat/500.jpg");
        }
    }

    #region webservice
    public Z_LOY_BP_CHANGEResponse UpdateMemberByBD(string fromusername, string cardno, Z_LOY_BP_GETDETAILResponse zloy)
    {
        ZWS_Z_LOY_BP_CHANGEClient client = new ZWS_Z_LOY_BP_CHANGEClient();
        Z_LOY_BP_CHANGE param = new Z_LOY_BP_CHANGE();


        param.I_PARTNER = cardno;


        Hmj.WebApp.UpdateMemberNew.ZZCENTRAL item = new Hmj.WebApp.UpdateMemberNew.ZZCENTRAL();
        item.TITLE_KEY = zloy.T_CENTRAL[0].TITLE_KEY;
        item.SEARCHTERM1 = zloy.T_CENTRAL[0].SEARCHTERM1;
        item.SEARCHTERM2 = zloy.T_CENTRAL[0].SEARCHTERM2;
        item.TITLELETTER = zloy.T_CENTRAL[0].TITLELETTER;
        item.DATAORIGINTYPE = zloy.T_CENTRAL[0].DATAORIGINTYPE;
        param.I_CENTRAL = item;


        Hmj.WebApp.UpdateMemberNew.ZZCENTRALDATAPERSON item2 = new Hmj.WebApp.UpdateMemberNew.ZZCENTRALDATAPERSON();
        item2.LASTNAME = zloy.T_CENTRALDATAPERSON[0].LASTNAME;
        item2.BIRTHDATE = zloy.T_CENTRALDATAPERSON[0].BIRTHDATE;
        item2.PREFIX1 = zloy.T_CENTRALDATAPERSON[0].PREFIX1;
        item2.BIRTHPLACE = zloy.T_CENTRALDATAPERSON[0].BIRTHPLACE;
        item2.MARITALSTATUS = zloy.T_CENTRALDATAPERSON[0].MARITALSTATUS;
        item2.TITLE_ACA1 = zloy.T_CENTRALDATAPERSON[0].TITLE_ACA1;
        item2.TITLE_ACA2 = zloy.T_CENTRALDATAPERSON[0].TITLE_ACA2;
        item2.TITLE_SPPL = zloy.T_CENTRALDATAPERSON[0].TITLE_SPPL;
        item2.BIRTHNAME = zloy.T_CENTRALDATAPERSON[0].BIRTHNAME;
        item2.FIRSTNAME = zloy.T_CENTRALDATAPERSON[0].FIRSTNAME;
        item2.OCCUPATION = zloy.T_CENTRALDATAPERSON[0].OCCUPATION;
        item2.MIDDLENAME = zloy.T_CENTRALDATAPERSON[0].MIDDLENAME;
        item2.SECONDNAME = zloy.T_CENTRALDATAPERSON[0].SECONDNAME;
        param.I_CENTRALDATAPERSON = item2;


        Hmj.WebApp.UpdateMemberNew.ZZADDRESSDATA item3 = new Hmj.WebApp.UpdateMemberNew.ZZADDRESSDATA();
        item3.COUNTRY = zloy.T_ADDRESSDATA[0].COUNTRY;
        item3.REGION = zloy.T_ADDRESSDATA[0].REGION;
        item3.CITY = zloy.T_ADDRESSDATA[0].CITY;
        item3.STREET = zloy.T_ADDRESSDATA[0].STREET;
        item3.POSTL_COD1 = zloy.T_ADDRESSDATA[0].POSTL_COD1;
        item3.STR_SUPPL1 = zloy.T_ADDRESSDATA[0].STR_SUPPL1;
        item3.STR_SUPPL2 = zloy.T_ADDRESSDATA[0].STR_SUPPL2;
        item3.STR_SUPPL3 = zloy.T_ADDRESSDATA[0].STR_SUPPL3;
        item3.LOCATION = fromusername;//暂时只能用短的代替，CRM有限制
        param.I_ADDRESSDATA = item3;


        Hmj.WebApp.UpdateMemberNew.ZZCUSTOMER01 itemcust1 = new Hmj.WebApp.UpdateMemberNew.ZZCUSTOMER01();
        itemcust1.ZA01 = zloy.T_CUSTOMER01[0].ZA01;
        itemcust1.ZA02 = zloy.T_CUSTOMER01[0].ZA02;
        itemcust1.ZA03 = zloy.T_CUSTOMER01[0].ZA03;
        itemcust1.ZA04 = zloy.T_CUSTOMER01[0].ZA04;
        itemcust1.ZA05 = zloy.T_CUSTOMER01[0].ZA05;
        itemcust1.ZA06 = zloy.T_CUSTOMER01[0].ZA06;
        itemcust1.ZA07 = zloy.T_CUSTOMER01[0].ZA07;
        itemcust1.ZA08 = zloy.T_CUSTOMER01[0].ZA08;
        itemcust1.ZA09 = zloy.T_CUSTOMER01[0].ZA09;
        itemcust1.ZA10 = zloy.T_CUSTOMER01[0].ZA10;
        param.I_CUSTOMER01 = itemcust1;


        Hmj.WebApp.UpdateMemberNew.ZZCUSTOMER02 itemcust2 = new Hmj.WebApp.UpdateMemberNew.ZZCUSTOMER02();
        itemcust2.ZA11 = zloy.T_CUSTOMER02[0].ZA11;
        itemcust2.ZA12 = zloy.T_CUSTOMER02[0].ZA12;
        itemcust2.ZA13 = zloy.T_CUSTOMER02[0].ZA13;
        itemcust2.ZA14 = zloy.T_CUSTOMER02[0].ZA14;
        param.I_CUSTOMER02 = itemcust2;


        Hmj.WebApp.UpdateMemberNew.ZZCUSTOMER03 itemcust3 = new Hmj.WebApp.UpdateMemberNew.ZZCUSTOMER03();
        itemcust3.ZA15 = zloy.T_CUSTOMER03[0].ZA15;
        itemcust3.ZA16 = zloy.T_CUSTOMER03[0].ZA16;
        itemcust3.ZA17 = zloy.T_CUSTOMER03[0].ZA17;
        itemcust3.ZA18 = zloy.T_CUSTOMER03[0].ZA18;
        itemcust3.ZA19 = zloy.T_CUSTOMER03[0].ZA19;
        itemcust3.ZA20 = zloy.T_CUSTOMER03[0].ZA20;
        itemcust3.ZA21 = zloy.T_CUSTOMER03[0].ZA21;
        itemcust3.ZA22 = zloy.T_CUSTOMER03[0].ZA22;
        itemcust3.ZA23 = zloy.T_CUSTOMER03[0].ZA23;
        itemcust3.ZA24 = zloy.T_CUSTOMER03[0].ZA24;
        itemcust3.ZA25 = zloy.T_CUSTOMER03[0].ZA25;
        param.I_CUSTOMER03 = itemcust3;


        Hmj.WebApp.UpdateMemberNew.ZZCUSTOMER04 itemcust4 = new Hmj.WebApp.UpdateMemberNew.ZZCUSTOMER04();
        itemcust4.ZA26 = zloy.T_CUSTOMER04[0].ZA26;
        itemcust4.ZA27 = zloy.T_CUSTOMER04[0].ZA27;
        itemcust4.ZA28 = zloy.T_CUSTOMER04[0].ZA28;
        itemcust4.ZA29 = zloy.T_CUSTOMER04[0].ZA29;
        itemcust4.ZA30 = zloy.T_CUSTOMER04[0].ZA30;
        itemcust4.ZA31 = zloy.T_CUSTOMER04[0].ZA31;
        itemcust4.ZA32 = zloy.T_CUSTOMER04[0].ZA32;
        itemcust4.ZA33 = zloy.T_CUSTOMER04[0].ZA33;
        param.I_CUSTOMER04 = itemcust4;


        Hmj.WebApp.UpdateMemberNew.ZZCUSTOMER05 itemcust5 = new Hmj.WebApp.UpdateMemberNew.ZZCUSTOMER05();
        itemcust5.ZA35 = zloy.T_CUSTOMER05[0].ZA35;
        itemcust5.ZA36 = zloy.T_CUSTOMER05[0].ZA36;
        itemcust5.ZA37 = zloy.T_CUSTOMER05[0].ZA37;
        itemcust5.ZA38 = zloy.T_CUSTOMER05[0].ZA38;
        param.I_CUSTOMER05 = itemcust5;


        Hmj.WebApp.UpdateMemberNew.ZZCUSTOMER06 itemcust6 = new Hmj.WebApp.UpdateMemberNew.ZZCUSTOMER06();
        itemcust6.ZA39 = zloy.T_CUSTOMER06[0].ZA39;
        itemcust6.ZA40 = zloy.T_CUSTOMER06[0].ZA40;
        itemcust6.ZA41 = zloy.T_CUSTOMER06[0].ZA41;
        itemcust6.ZA42 = zloy.T_CUSTOMER06[0].ZA42;
        itemcust6.ZA43 = zloy.T_CUSTOMER06[0].ZA43;
        itemcust6.ZA44 = zloy.T_CUSTOMER06[0].ZA44;
        param.I_CUSTOMER06 = itemcust6;


        Hmj.WebApp.UpdateMemberNew.ZZCUSTOMER07 itemcust7 = new Hmj.WebApp.UpdateMemberNew.ZZCUSTOMER07();
        itemcust7.ZA45 = zloy.T_CUSTOMER07[0].ZA45;
        itemcust7.ZA46 = zloy.T_CUSTOMER07[0].ZA46;
        itemcust7.ZA47 = zloy.T_CUSTOMER07[0].ZA47;
        itemcust7.ZA48 = zloy.T_CUSTOMER07[0].ZA48;
        itemcust7.ZA49 = zloy.T_CUSTOMER07[0].ZA49;
        itemcust7.ZA50 = zloy.T_CUSTOMER07[0].ZA50;
        itemcust7.ZA51 = zloy.T_CUSTOMER07[0].ZA51;
        itemcust7.ZA52 = zloy.T_CUSTOMER07[0].ZA52;
        itemcust7.ZA53 = zloy.T_CUSTOMER07[0].ZA53;
        itemcust7.ZA54 = zloy.T_CUSTOMER07[0].ZA54;
        itemcust7.ZA55 = zloy.T_CUSTOMER07[0].ZA55;
        itemcust7.ZA56 = zloy.T_CUSTOMER07[0].ZA56;
        itemcust7.ZA57 = zloy.T_CUSTOMER07[0].ZA57;
        itemcust7.ZA58 = zloy.T_CUSTOMER07[0].ZA58;
        param.I_CUSTOMER07 = itemcust7;


        param.T_TELEFONDATA = new Hmj.WebApp.UpdateMemberNew.ZZTELEFONDATA[] {
        new Hmj.WebApp.UpdateMemberNew.ZZTELEFONDATA(){TELEPHONE = zloy.T_TELEFONDATA[0].TELEPHONE}
        };

        param.T_E_MAILDATA = new Hmj.WebApp.UpdateMemberNew.ZZE_MAILDATA[] {
        new Hmj.WebApp.UpdateMemberNew.ZZE_MAILDATA(){E_MAIL = zloy.T_E_MAILDATA[0].E_MAIL}
        };

        param.T_RETURN = new Hmj.WebApp.UpdateMemberNew.ZZRETURN2[] { };

        var response = client.Z_LOY_BP_CHANGE(param);

        return response;

    }

    public Z_LOY_BP_CHANGEResponse UpdateMember(string cardno, string name, string lastname, string sf, string cs, string address, string sex, string xz)
    {

        Z_LOY_BP_GETDETAILResponse zloy = SelectMemberByUpdate(cardno);

        ZWS_Z_LOY_BP_CHANGEClient client = new ZWS_Z_LOY_BP_CHANGEClient();
        Z_LOY_BP_CHANGE param = new Z_LOY_BP_CHANGE();
        param.I_PARTNER = cardno;
        Hmj.WebApp.UpdateMemberNew.ZZCENTRAL item = new Hmj.WebApp.UpdateMemberNew.ZZCENTRAL();
        item.TITLE_KEY = zloy.T_CENTRAL[0].TITLE_KEY;
        item.SEARCHTERM1 = zloy.T_CENTRAL[0].SEARCHTERM1;
        item.SEARCHTERM2 = zloy.T_CENTRAL[0].SEARCHTERM2;
        item.TITLELETTER = zloy.T_CENTRAL[0].TITLELETTER;
        item.DATAORIGINTYPE = zloy.T_CENTRAL[0].DATAORIGINTYPE;
        param.I_CENTRAL = item;


        Hmj.WebApp.UpdateMemberNew.ZZCENTRALDATAPERSON item2 = new Hmj.WebApp.UpdateMemberNew.ZZCENTRALDATAPERSON();
        item2.LASTNAME = zloy.T_CENTRALDATAPERSON[0].LASTNAME;
        item2.BIRTHDATE = zloy.T_CENTRALDATAPERSON[0].BIRTHDATE;
        item2.PREFIX1 = zloy.T_CENTRALDATAPERSON[0].PREFIX1;
        item2.BIRTHPLACE = zloy.T_CENTRALDATAPERSON[0].BIRTHPLACE;
        item2.MARITALSTATUS = zloy.T_CENTRALDATAPERSON[0].MARITALSTATUS;
        item2.TITLE_ACA1 = zloy.T_CENTRALDATAPERSON[0].TITLE_ACA1;
        item2.TITLE_ACA2 = zloy.T_CENTRALDATAPERSON[0].TITLE_ACA2;
        item2.TITLE_SPPL = zloy.T_CENTRALDATAPERSON[0].TITLE_SPPL;
        item2.BIRTHNAME = zloy.T_CENTRALDATAPERSON[0].BIRTHNAME;
        item2.FIRSTNAME = zloy.T_CENTRALDATAPERSON[0].FIRSTNAME;
        item2.OCCUPATION = zloy.T_CENTRALDATAPERSON[0].OCCUPATION;
        item2.MIDDLENAME = zloy.T_CENTRALDATAPERSON[0].MIDDLENAME;
        item2.SECONDNAME = zloy.T_CENTRALDATAPERSON[0].SECONDNAME;
        param.I_CENTRALDATAPERSON = item2;


        Hmj.WebApp.UpdateMemberNew.ZZADDRESSDATA item3 = new Hmj.WebApp.UpdateMemberNew.ZZADDRESSDATA();
        item3.COUNTRY = zloy.T_ADDRESSDATA[0].COUNTRY;
        item3.REGION = sf;
        item3.CITY = cs;
        if (address != "")
        {
            item3.STREET = address;
        }
        else
        {
            item3.STREET = zloy.T_ADDRESSDATA[0].STREET;
        }

        item3.POSTL_COD1 = zloy.T_ADDRESSDATA[0].POSTL_COD1;
        item3.STR_SUPPL1 = zloy.T_ADDRESSDATA[0].STR_SUPPL1;
        item3.STR_SUPPL2 = zloy.T_ADDRESSDATA[0].STR_SUPPL2;
        item3.STR_SUPPL3 = zloy.T_ADDRESSDATA[0].STR_SUPPL3;
        item3.LOCATION = zloy.T_ADDRESSDATA[0].LOCATION;   //暂时只能用短的代替，CRM有限制
        param.I_ADDRESSDATA = item3;


        Hmj.WebApp.UpdateMemberNew.ZZCUSTOMER01 itemcust1 = new Hmj.WebApp.UpdateMemberNew.ZZCUSTOMER01();
        itemcust1.ZA01 = zloy.T_CUSTOMER01[0].ZA01;
        itemcust1.ZA02 = zloy.T_CUSTOMER01[0].ZA02;
        itemcust1.ZA03 = zloy.T_CUSTOMER01[0].ZA03;
        itemcust1.ZA04 = zloy.T_CUSTOMER01[0].ZA04;
        itemcust1.ZA05 = zloy.T_CUSTOMER01[0].ZA05;
        itemcust1.ZA06 = zloy.T_CUSTOMER01[0].ZA06;
        itemcust1.ZA07 = zloy.T_CUSTOMER01[0].ZA07;
        itemcust1.ZA08 = zloy.T_CUSTOMER01[0].ZA08;
        itemcust1.ZA09 = zloy.T_CUSTOMER01[0].ZA09;
        itemcust1.ZA10 = zloy.T_CUSTOMER01[0].ZA10;
        param.I_CUSTOMER01 = itemcust1;


        Hmj.WebApp.UpdateMemberNew.ZZCUSTOMER02 itemcust2 = new Hmj.WebApp.UpdateMemberNew.ZZCUSTOMER02();
        itemcust2.ZA11 = zloy.T_CUSTOMER02[0].ZA11;
        itemcust2.ZA12 = zloy.T_CUSTOMER02[0].ZA12;
        itemcust2.ZA13 = zloy.T_CUSTOMER02[0].ZA13;
        itemcust2.ZA14 = zloy.T_CUSTOMER02[0].ZA14;
        param.I_CUSTOMER02 = itemcust2;


        Hmj.WebApp.UpdateMemberNew.ZZCUSTOMER03 itemcust3 = new Hmj.WebApp.UpdateMemberNew.ZZCUSTOMER03();
        itemcust3.ZA15 = zloy.T_CUSTOMER03[0].ZA15;
        itemcust3.ZA16 = zloy.T_CUSTOMER03[0].ZA16;
        itemcust3.ZA17 = zloy.T_CUSTOMER03[0].ZA17;
        itemcust3.ZA18 = zloy.T_CUSTOMER03[0].ZA18;
        itemcust3.ZA19 = zloy.T_CUSTOMER03[0].ZA19;
        itemcust3.ZA20 = zloy.T_CUSTOMER03[0].ZA20;
        itemcust3.ZA21 = zloy.T_CUSTOMER03[0].ZA21;
        itemcust3.ZA22 = zloy.T_CUSTOMER03[0].ZA22;
        itemcust3.ZA23 = zloy.T_CUSTOMER03[0].ZA23;
        itemcust3.ZA24 = zloy.T_CUSTOMER03[0].ZA24;
        itemcust3.ZA25 = zloy.T_CUSTOMER03[0].ZA25;
        param.I_CUSTOMER03 = itemcust3;


        Hmj.WebApp.UpdateMemberNew.ZZCUSTOMER04 itemcust4 = new Hmj.WebApp.UpdateMemberNew.ZZCUSTOMER04();
        itemcust4.ZA26 = zloy.T_CUSTOMER04[0].ZA26;
        itemcust4.ZA27 = zloy.T_CUSTOMER04[0].ZA27;
        itemcust4.ZA28 = zloy.T_CUSTOMER04[0].ZA28;
        itemcust4.ZA29 = zloy.T_CUSTOMER04[0].ZA29;
        itemcust4.ZA30 = zloy.T_CUSTOMER04[0].ZA30;
        itemcust4.ZA31 = zloy.T_CUSTOMER04[0].ZA31;
        itemcust4.ZA32 = zloy.T_CUSTOMER04[0].ZA32;
        itemcust4.ZA33 = zloy.T_CUSTOMER04[0].ZA33;
        param.I_CUSTOMER04 = itemcust4;


        Hmj.WebApp.UpdateMemberNew.ZZCUSTOMER05 itemcust5 = new Hmj.WebApp.UpdateMemberNew.ZZCUSTOMER05();
        itemcust5.ZA35 = zloy.T_CUSTOMER05[0].ZA35;
        itemcust5.ZA36 = zloy.T_CUSTOMER05[0].ZA36;
        itemcust5.ZA37 = zloy.T_CUSTOMER05[0].ZA37;
        itemcust5.ZA38 = zloy.T_CUSTOMER05[0].ZA38;
        param.I_CUSTOMER05 = itemcust5;


        Hmj.WebApp.UpdateMemberNew.ZZCUSTOMER06 itemcust6 = new Hmj.WebApp.UpdateMemberNew.ZZCUSTOMER06();
        if (xz.Contains("手机"))
        {
            itemcust6.ZA39 = "X";
        }
        else
        {
            itemcust6.ZA39 = "";
        }

        if (xz.Contains("短信"))
        {
            itemcust6.ZA40 = "X";
        }
        else
        {
            itemcust6.ZA40 = "";
        }

        if (xz.Contains("邮件"))
        {
            itemcust6.ZA41 = "X";
        }
        else
        {
            itemcust6.ZA41 = "";
        }

        if (xz.Contains("礼品"))
        {
            itemcust6.ZA42 = "X";
        }
        else
        {
            itemcust6.ZA42 = "";
        }

        if (xz.Contains("直邮"))
        {
            itemcust6.ZA43 = "X";
        }
        else
        {
            itemcust6.ZA43 = "";
        }
        if (xz.Contains("活动"))
        {
            itemcust6.ZA44 = "X";
        }
        else
        {
            itemcust6.ZA44 = "";
        }
        param.I_CUSTOMER06 = itemcust6;


        Hmj.WebApp.UpdateMemberNew.ZZCUSTOMER07 itemcust7 = new Hmj.WebApp.UpdateMemberNew.ZZCUSTOMER07();
        itemcust7.ZA45 = zloy.T_CUSTOMER07[0].ZA45;
        itemcust7.ZA46 = zloy.T_CUSTOMER07[0].ZA46;
        itemcust7.ZA47 = zloy.T_CUSTOMER07[0].ZA47;
        itemcust7.ZA48 = zloy.T_CUSTOMER07[0].ZA48;
        itemcust7.ZA49 = zloy.T_CUSTOMER07[0].ZA49;
        itemcust7.ZA50 = zloy.T_CUSTOMER07[0].ZA50;
        itemcust7.ZA51 = zloy.T_CUSTOMER07[0].ZA51;
        itemcust7.ZA52 = zloy.T_CUSTOMER07[0].ZA52;
        itemcust7.ZA53 = zloy.T_CUSTOMER07[0].ZA53;
        itemcust7.ZA54 = zloy.T_CUSTOMER07[0].ZA54;
        itemcust7.ZA55 = zloy.T_CUSTOMER07[0].ZA55;
        itemcust7.ZA56 = zloy.T_CUSTOMER07[0].ZA56;
        itemcust7.ZA57 = zloy.T_CUSTOMER07[0].ZA57;
        itemcust7.ZA58 = zloy.T_CUSTOMER07[0].ZA58;
        param.I_CUSTOMER07 = itemcust7;


        param.T_TELEFONDATA = new Hmj.WebApp.UpdateMemberNew.ZZTELEFONDATA[] {
        new Hmj.WebApp.UpdateMemberNew.ZZTELEFONDATA(){TELEPHONE = zloy.T_TELEFONDATA[0].TELEPHONE}
        };

        param.T_E_MAILDATA = new Hmj.WebApp.UpdateMemberNew.ZZE_MAILDATA[] {
        new Hmj.WebApp.UpdateMemberNew.ZZE_MAILDATA(){E_MAIL = zloy.T_E_MAILDATA[0].E_MAIL}
        };

        param.T_RETURN = new Hmj.WebApp.UpdateMemberNew.ZZRETURN2[] { };

        var response = client.Z_LOY_BP_CHANGE(param);



        return response;

    }



    public Z_LOY_BP_CHANGEResponse UpdateMemberByBF(string cardno, string name, string lastname, string sf, string cs, string address, string sex, string xz)
    {

        Z_LOY_BP_GETDETAILResponse zloy = SelectMemberByUpdate(cardno);


        ZWS_Z_LOY_BP_CHANGEClient client = new ZWS_Z_LOY_BP_CHANGEClient();
        Z_LOY_BP_CHANGE param = new Z_LOY_BP_CHANGE();

        param.I_PARTNER = cardno;

        Hmj.WebApp.UpdateMemberNew.ZZCENTRAL item = new Hmj.WebApp.UpdateMemberNew.ZZCENTRAL();
        if (sex == "先生")
        {
            sex = "Z001";
        }
        else
        {
            sex = "Z002";
        }
        item.TITLE_KEY = sex;
        param.I_CENTRAL = item;

        Hmj.WebApp.UpdateMemberNew.ZZCENTRALDATAPERSON item3 = new Hmj.WebApp.UpdateMemberNew.ZZCENTRALDATAPERSON();
        item3.LASTNAME = name;
        param.I_CENTRALDATAPERSON = item3;

        Hmj.WebApp.UpdateMemberNew.ZZADDRESSDATA item4 = new Hmj.WebApp.UpdateMemberNew.ZZADDRESSDATA();
        item4.COUNTRY = "CN";
        item4.REGION = sf;
        item4.CITY = cs;
        if (address != "")
        {
            item4.STREET = address;
        }
        param.I_ADDRESSDATA = item4;


        Hmj.WebApp.UpdateMemberNew.ZZCUSTOMER06 item11 = new Hmj.WebApp.UpdateMemberNew.ZZCUSTOMER06();
        if (xz.Contains("手机"))
        {
            item11.ZA39 = "X";
        }
        else
        {
            item11.ZA39 = "";
        }

        if (xz.Contains("短信"))
        {
            item11.ZA40 = "X";
        }
        else
        {
            item11.ZA40 = "";
        }

        if (xz.Contains("邮件"))
        {
            item11.ZA41 = "X";
        }
        else
        {
            item11.ZA41 = "";
        }

        if (xz.Contains("礼品"))
        {
            item11.ZA42 = "X";
        }
        else
        {
            item11.ZA42 = "";
        }

        if (xz.Contains("直邮"))
        {
            item11.ZA43 = "X";
        }
        else
        {
            item11.ZA43 = "";
        }

        if (xz.Contains("活动"))
        {
            item11.ZA44 = "X";
        }
        else
        {
            item11.ZA44 = "";
        }


        param.I_CUSTOMER06 = item11;

        param.T_RETURN = new Hmj.WebApp.UpdateMemberNew.ZZRETURN2[] { };

        var response = client.Z_LOY_BP_CHANGE(param);
        return response;
    }


    public Z_LOY_BP_GETDETAILResponse SelectMemberByUpdate(string cardno)
    {
        ZWS_Z_LOY_BP_GETDETAILClient clientselect = new ZWS_Z_LOY_BP_GETDETAILClient();
        Z_LOY_BP_GETDETAIL paramselect = new Z_LOY_BP_GETDETAIL();
        List<ZZINPUT_RANGE> list = new List<ZZINPUT_RANGE>();
        ZZINPUT_RANGE item1 = new ZZINPUT_RANGE();
        item1.SIGN = "I";
        item1.OPTION = "EQ";
        item1.LOW = cardno;
        item1.HIGH = "";
        list.Add(item1);
        paramselect.T_INPUT_RANGE = list.ToArray();
        paramselect.T_CENTRAL = new Hmj.WebApp.SelectMemberNew.ZZCENTRAL[] { };
        paramselect.T_CENTRALDATAPERSON = new Hmj.WebApp.SelectMemberNew.ZZCENTRALDATAPERSON[] { };
        paramselect.T_ADDRESSDATA = new Hmj.WebApp.SelectMemberNew.ZZADDRESSDATA[] { };
        paramselect.T_CUSTOMER01 = new Hmj.WebApp.SelectMemberNew.ZZCUSTOMER01[] { };
        paramselect.T_CUSTOMER02 = new Hmj.WebApp.SelectMemberNew.ZZCUSTOMER02[] { };
        paramselect.T_CUSTOMER03 = new Hmj.WebApp.SelectMemberNew.ZZCUSTOMER03[] { };
        paramselect.T_CUSTOMER04 = new Hmj.WebApp.SelectMemberNew.ZZCUSTOMER04[] { };
        paramselect.T_CUSTOMER05 = new Hmj.WebApp.SelectMemberNew.ZZCUSTOMER05[] { };
        paramselect.T_CUSTOMER06 = new Hmj.WebApp.SelectMemberNew.ZZCUSTOMER06[] { };
        paramselect.T_CUSTOMER07 = new Hmj.WebApp.SelectMemberNew.ZZCUSTOMER07[] { };
        paramselect.T_TELEFONDATA = new Hmj.WebApp.SelectMemberNew.ZZTELEFONDATA[] { };
        paramselect.T_E_MAILDATA = new Hmj.WebApp.SelectMemberNew.ZZE_MAILDATA[] { };
        paramselect.T_RETURN = new Hmj.WebApp.SelectMemberNew.ZZRETURN2[] { };
        var response = clientselect.Z_LOY_BP_GETDETAIL(paramselect);
        return response;
    }

    public Z_LOY_BP_GETDETAILResponse SelectMember(string phone)
    {
        ZWS_Z_LOY_BP_GETDETAILClient client = new ZWS_Z_LOY_BP_GETDETAILClient();
        Z_LOY_BP_GETDETAIL param = new Z_LOY_BP_GETDETAIL();
        List<ZZINPUT_RANGE> list = new List<ZZINPUT_RANGE>();
        ZZINPUT_RANGE item1 = new ZZINPUT_RANGE();
        item1.SIGN = "I";
        item1.OPTION = "EQ";
        item1.LOW = phone;
        item1.HIGH = "";
        list.Add(item1);
        param.T_INPUT_RANGE = list.ToArray();
        param.T_CENTRAL = new Hmj.WebApp.SelectMemberNew.ZZCENTRAL[] { };
        param.T_ADDRESSDATA = new Hmj.WebApp.SelectMemberNew.ZZADDRESSDATA[] { };
        param.T_TELEFONDATA = new Hmj.WebApp.SelectMemberNew.ZZTELEFONDATA[] { };
        param.T_CENTRALDATAPERSON = new Hmj.WebApp.SelectMemberNew.ZZCENTRALDATAPERSON[] { };
        //param.T_CUSTOMER01 = new ZZCUSTOMER01[] { };
        //param.T_CUSTOMER02 = new ZZCUSTOMER02[] { };
        //param.T_CUSTOMER03 = new ZZCUSTOMER03[] { };
        //param.T_CUSTOMER04 = new ZZCUSTOMER04[] { };
        //param.T_CUSTOMER05 = new ZZCUSTOMER05[] { };
        param.T_CUSTOMER06 = new Hmj.WebApp.SelectMemberNew.ZZCUSTOMER06[] { };
        //param.T_CUSTOMER07 = new ZZCUSTOMER07[] { };
        var response = client.Z_LOY_BP_GETDETAIL(param);
        return response;
    }

    public Z_LOY_BP_GETORDERResponse SelectOrder(string CardNo, string timestart, string timeend)
    {
        ZWS_Z_LOY_BP_GETORDERClient client = new ZWS_Z_LOY_BP_GETORDERClient();
        Z_LOY_BP_GETORDER param = new Z_LOY_BP_GETORDER();
        ZZSCRMD_ORDER z = new ZZSCRMD_ORDER();
        param.I_ZORP = CardNo;
        param.I_ZCREATED_AT_BEGIN = timestart;
        param.I_ZCREATED_AT_END = timeend;
        param.T_ZCRMD_ORDER = new ZZSCRMD_ORDER[] { };
        param.T_RETURN = new Hmj.WebApp.SelectOrderNew.ZZRETURN2[] { };
        var response = client.Z_LOY_BP_GETORDER(param);
        return response;
    }

    public Z_CRM_LOY_WELFAREResponse SelectDiscount(string UserNo, string CarNo)
    {
        ZWS_LOY_WELFAREClient client = new ZWS_LOY_WELFAREClient();
        Z_CRM_LOY_WELFARE param = new Z_CRM_LOY_WELFARE();
        //client.ClientCredentials.UserName.UserName = "bob";
        //client.ClientCredentials.UserName.Password = "1234";DiscountService
        param.IV_ZCPIDO = CarNo;
        param.IV_ZCPLOY = UserNo;
        var response = client.Z_CRM_LOY_WELFARE(param);
        return response;
    }

    //public Z_CRM_LOY_WELFAREResponse SelectDiscount1(string UserNo, string CarNo)
    //{
    //    zws_loy_welfare client = new zws_loy_welfare();
    //    Z_CRM_LOY_WELFARE param = new Z_CRM_LOY_WELFARE();
    //    //client.ClientCredentials.UserName.UserName = "bob";
    //    //client.ClientCredentials.UserName.Password = "1234";DiscountService
    //    param.IV_ZCPIDO = CarNo;
    //    param.IV_ZCPLOY = UserNo;
    //    var response = client.Z_CRM_LOY_WELFARE(param);
    //    return response;
    //}
    #endregion

    #region 功能  
    /// <summary>
    /// 第三方平台授权
    /// </summary>
    /// <param name="uri"></param>
    /// <returns></returns>
    public string AuthCode(string uri)
    {
        string urls = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx32364024f2c86185&redirect_uri={0}&response_type=code&scope={1}&state=STATE&connect_redirect=1#wechat_redirect";
        urls = string.Format(urls, HttpUtility.UrlEncode(AppConfig.Get("WXGetCode") + "?url=" + uri + "&appid=bf_m_23SDFv2&timestamp=" + Utility.ConvertDateTimeInt(DateTime.Now).ToString() + "&sign=" + Utility.GetSignTest()), "snsapi_base");
        return urls;
    }


    ORG_INFO m;
    public string Token(string ToUserName)
    {
        m = sbo.GetMerchantsByToUserName(ToUserName);
        if (m == null)
            return "";
        string Access_token = "";
        if (m.Access_token != "")
        {
            Access_token = m.Access_token;
            string url = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + Access_token + "&openid=" +
                (m.OneOpenID == null ? "oQaIMwPgnsBpwQYfwLQnUBbmQKS4" : m.OneOpenID);
            string b = PostRequest(url);
            if (b.Contains("errcode"))  //返回错误信息
            {
                Access_token = GetAccess(m);
                if(string.IsNullOrEmpty(Access_token))
                    Access_token = GetAccess(m);
                m.Access_token = Access_token;
                sbo.SaveMerchants(m);
            }
            if (m.OneOpenID == "" || m.OneOpenID == null)
            {
                WXCUST_FANS fans = sbo.GetOneFans(m.ToUserName);
                if (fans != null)
                {
                    m.OneOpenID = fans.FROMUSERNAME;
                    sbo.SaveMerchants(m);
                }
            }
        }
        else
        {
            if (m.OneOpenID == "" || m.OneOpenID == null)
            {
                WXCUST_FANS fans = sbo.GetOneFans(m.ToUserName);
                if (fans != null)
                {
                    m.OneOpenID = fans.FROMUSERNAME;
                    // sbo.SaveMerchants(m);
                }
            }
            Access_token = GetAccess(m);
            if (string.IsNullOrEmpty(Access_token))
                Access_token = GetAccess(m);
            m.Access_token = Access_token;
            sbo.SaveMerchants(m);

        }
        return Access_token;
    }
    /// <summary>
    /// 获取最新JSAPI_TICKET凭证
    /// </summary>
    /// <param name="ToUserName"></param>
    /// <returns></returns>
    public string GetJSAPI_Ticket()
    {

        string JSapi_ticket = "";
        ORG_INFO m = sbo.GetMerchantsByToUserName(AppConfig.FWHOriginalID);
        if (m.JSapi_Ticket != "" && m.JSapi_Ticket != null && (m.GetTicketTime == null ? DateTime.Now.AddHours(-3) : m.GetTicketTime.Value).AddHours(2) > DateTime.Now) //不为空，并且获取时间没有超过2小时
        {
            return m.JSapi_Ticket;
        }
        else
        {
            JSapi_ticket = m.JSapi_Ticket;
            string url = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + Token(AppConfig.FWHOriginalID) + "&type=jsapi";
            string b = PostRequest(url);
            tickresult ticket = JsonConvert.DeserializeObject<tickresult>(b);
            if (ticket.errcode == 0)  //正确
            {
                m.JSapi_Ticket = ticket.ticket;
                m.GetTicketTime = DateTime.Now;
                sbo.SaveMerchants(m);
                return m.JSapi_Ticket;
            }
            return "";
        }
        //string JSapi_ticket = "";

        //ApiTicket at = new ApiTicket();
        //at = sbo.GetModelJsApi();
        //if (at.JSapi_Ticket != "" && at.JSapi_Ticket != null && (at.GetTicketTime == null ? DateTime.Now.AddHours(-3) : at.GetTicketTime.Value).AddHours(2) > DateTime.Now) //不为空，并且获取时间没有超过2小时
        //{
        //    return at.JSapi_Ticket;
        //}
        //else
        //{
        //    JSapi_ticket = at.JSapi_Ticket;
        //    string url = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + Token(AppConfig.FWHOriginalID) + "&type=jsapi";
        //    string b = PostRequest(url);
        //    tickresult ticket = JsonConvert.DeserializeObject<tickresult>(b);
        //    if (ticket.errcode == 0)  //正确
        //    {
        //        at.JSapi_Ticket = ticket.ticket;
        //        at.GetTicketTime = DateTime.Now;
        //        sbo.SaveApiTicket(at);
        //        return at.JSapi_Ticket;
        //    }
        //    return "";
        //}
    }

    public string GetCardApi()
    {
        IHmjMemberService _xyhService = new HmjMemberService();
        //XHYCouponService _xyhService = new XHYCouponService();
        string Cardapi_ticket = "";
        CardApiTicket at = new CardApiTicket();
        at = _xyhService.GetModelCardApi();
        if (at == null)
            at = new CardApiTicket();
        if (at.Cardapi_Ticket != "" && at.Cardapi_Ticket != null && (at.GetTicketTime == null ? DateTime.Now.AddHours(-3) : at.GetTicketTime.Value).AddHours(2) > DateTime.Now) //不为空，并且获取时间没有超过2小时
        {
            return at.Cardapi_Ticket;
        }
        else
        {
            Cardapi_ticket = at.Cardapi_Ticket;
            string url = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + Token(AppConfig.FWHOriginalID) + "&type=wx_card";
            string b = PostRequest(url);
            CardApi ticket = JsonConvert.DeserializeObject<CardApi>(b);
            if (ticket.errcode == 0)  //正确
            {
                at.Cardapi_Ticket = ticket.ticket;
                at.GetTicketTime = DateTime.Now;
                _xyhService.AddCardApi(at);
                return at.Cardapi_Ticket;
            }
            return "";
        }
    }

    private string GetAccess(ORG_INFO m)
    {
        string Access_token = "";
        string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + m.AppID + "&secret=" + m.Appsecret;
        try
        {
            string token = PostRequest(url);
            //获取token失败：{"access_token":"10_wo7ifVhCueuaGeW7B52dWfohZNLgV2YHaOr_wpIhAdINWiaBdBvUKkjeWHiVfCdUS1CIwPc2_gdXbz9N22zjgK13D3lFZ2eMGKQfMITlvfxM_F4SB6LttZlcp9AHUHiABAOBO","expires_in":7199}
            if(!token.Contains("errcode"))//if (token.Contains("7200"))
            {
                string[] b = token.Split('\"');
                Access_token = b[3];
            }
            else
            {
                AddWxLog("获取token失败：" + token);
            }
        }
        catch (Exception ex)
        {
            AddWxLog("获取token失败："+ex.Message);
            Access_token = "";
        }
        return Access_token;
    }


    /// <summary>
    /// 保存地理位置
    /// </summary>
    /// <param name="requestXML"></param>
    /// <returns></returns>
    public int SaveLocation(WXCUST_MSG_HIS requestXML)
    {
        WD_Location l = new WD_Location();
        l.accuracy = requestXML.SCALE;
        l.Createdate = DateTime.Now;
        l.FromUserName = requestXML.FROMUSERNAME;
        l.latitude = requestXML.LOCATION_X;
        l.longitude = requestXML.LOCATION_Y;
        l.speed = "0";
        l.ToUserName = requestXML.TOUSERNAME;
        if (requestXML.WXEVENT.ToLower() == "location") //如果是自动上报的地理位置，调换X/Y轴，因为赋值的时候赋错了
        {
            l.latitude = requestXML.LOCATION_Y;
            l.longitude = requestXML.LOCATION_X;
        }
        GetBaiDuMap(ref l);
        return mss.SaveLocation(l);
    }


    /// <summary>
    /// 插入粉丝信息
    /// </summary>
    /// <param name="FromUserName">微信号</param>
    /// <param name="state">状态</param>
    /// <returns></returns>
    public int InsertFS(string FromUserName, string ToUserName, int state, string code = "",
        string number = "")
    {
        code = code.Trim();
        //获取Token字符串
        string access_token = Token(ToUserName);
        AddWxLog("插入粉丝：access_token=" + access_token);
        //获取用户信息列表
        string info = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + access_token + "&openid=" + FromUserName + "&lang=zh_CN";
        string infomes = PostRequestGet(info);
        Info winfo = JsonConvert.DeserializeObject<Info>(infomes);
        AddWxLog("插入粉丝返回结果：infomes=" + infomes);
    
        //把用户的信息存到粉丝表,保存之前先判断粉丝表里面有没有这个用户
        WXCUST_FANS cfan = sbo.GetFansByFromUserName(FromUserName);
        //未关注时更新状态
        if (cfan != null)
        {
            if (state == 1)
            {
                if (winfo.subscribe == 1)
                {
                    cfan.NAME = winfo.nickname.Length > 50 ? winfo.nickname.Substring(0, 50) : winfo.nickname;
                    cfan.GENDER = winfo.sex == "1" ? true : false;
                    cfan.COUNTRY = winfo.country;
                    cfan.PROVINCE = winfo.province;
                    cfan.CITY = winfo.city;
                    cfan.FROMUSERNAME = winfo.openid;
                    cfan.NOTICE_DATE = UnixTimeToTime(winfo.subscribe_time);
                    cfan.IMAGE = winfo.headimgurl;
                }
                cfan.ToUserName = ToUserName;
                cfan.STATUS = state;
                
                cfan.CANCEL_DATE = DateTime.Now;
                //cfan.CREATE_DATE = DateTime.Now;
                cfan.CREATE_USER = "system";
                cfan.LAST_CONN_DATE = DateTime.Now;
                cfan.LAST_MODI_DATE = DateTime.Parse("1900-01-01");
                cfan.REMARK = "";
                cfan.LAST_MODI_USER = "system";
                //cfan.STORE_CODE = code;
            }
            else  //取消关注
            {
                cfan.STATUS = state;
                cfan.CANCEL_DATE = DateTime.Now;
            }
            return sbo.UpdateFans(cfan);

        }
        else
        {
            if (state == 1)
            {
                cfan = new WXCUST_FANS();
                if (winfo.subscribe == 1)
                {
                    cfan.NAME = winfo.nickname.Length > 25 ? winfo.nickname.Substring(0, 24) : winfo.nickname;
                    cfan.GENDER = winfo.sex == "1" ? true : false;
                    cfan.COUNTRY = winfo.country;
                    cfan.PROVINCE = winfo.province;
                    cfan.CITY = winfo.city;
                    cfan.FROMUSERNAME = winfo.openid;
                    cfan.IMAGE = winfo.headimgurl;
                    cfan.NOTICE_DATE = UnixTimeToTime(winfo.subscribe_time);
                }
                cfan.ToUserName = ToUserName;
                cfan.STATUS = 1;

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
                cfan.STORE_CODE = code;
                cfan.IS_REGISTER = 0;

                int fansid = sbo.InsertFans(cfan);

                //if (fansid > 0)
                //{
                //    bcj.GoToSendPoint(fansid, winfo.openid);
                //}

                return fansid;
            }
            else
                return 0;//取消关注不插入粉丝表
        }
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

    public string PostRequest(string url)
    {
        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);  //定义请求对象，并设置好请求URL地址
        request.Method = "post";
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();    //定义响应对象，request在调用GetResponse方法事执行请求了，而不是在HttpWebRequest.Create的时候执行。       
        Stream stream = response.GetResponseStream(); //定义一个流对象，来获取响应流
        StreamReader sr = new StreamReader(stream);  //定义一个流读取对象，读取响应流
        string responseHTML = sr.ReadToEnd();
        return responseHTML;
    }


    public string GetAppid(string ToUserName)
    {
        m = sbo.GetMerchantsByToUserName(ToUserName);
        return m == null ? "" : m.AppID;
    }

    public string GetSecret(string ToUserName)
    {
        m = sbo.GetMerchantsByToUserName(ToUserName);
        return m == null ? "" : m.Appsecret;
    }

    /// <summary>
    /// 谷歌坐标转换百度坐标
    /// </summary>
    /// <param name="l"></param>
    public void GetBaiDuMap(ref WD_Location l)
    {
        try
        {
            string url = "http://api.map.baidu.com/geoconv/v1/?coords=" + l.longitude + "," + l.latitude + "&from=3&to=5&ak=42095ab67452cfefd9b5d3743d197f49";
            string message = PostRequest(url);
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


    public bool AddWxLog(string str)
    {
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
    #endregion

    //public string GetCardApi(string token)
    //{
    //    string Cardapi_ticket = "";
    //    CardApiTicket at = new CardApiTicket();

    //    at = _hmjMember.GetModelCardApi();
    //    if(at==null)
    //        at = new CardApiTicket();
    //    if (at.Cardapi_Ticket != "" && at.Cardapi_Ticket != null && (at.GetTicketTime == null ? DateTime.Now.AddHours(-3) : at.GetTicketTime.Value).AddHours(2) > DateTime.Now) //不为空，并且获取时间没有超过2小时
    //    {
    //        return at.Cardapi_Ticket;
    //    }
    //    else
    //    {
    //        Cardapi_ticket = at.Cardapi_Ticket;
    //        string url = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + token + "&type=wx_card";
    //        string b = PostRequest(url);
    //        CardApi ticket = JsonConvert.DeserializeObject<CardApi>(b);
    //        if (ticket.errcode == 0)  //正确
    //        {
    //            at.Cardapi_Ticket = ticket.ticket;
    //            at.GetTicketTime = DateTime.Now;
    //            _hmjMember.AddCardApi(at);
    //            return at.Cardapi_Ticket;
    //        }
    //        return "";
    //    }
    //}
    /// <summary>
    /// datetime转换为unixtime
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public int ConvertDateTimeInt(DateTime time)
    {
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        return (int)(time - startTime).TotalSeconds;
    }


    #region 类
    public class OpenInfo
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string openid { get; set; }
        public string scope { get; set; }
        public string nickname { get; set; }
        public string headimgurl { get; set; }
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
    class baidumap
    {
        public int status { get; set; }
        public zuobiao[] result { get; set; }
    }

    class zuobiao
    {
        public decimal x { get; set; }
        public decimal y { get; set; }
    }
    class tickresult
    {
        public int errcode { get; set; }
        public string errmsg { get; set; }
        public string ticket { get; set; }
        public int expires_in { get; set; }
    }

    class CardApi
    {
        public int errcode { get; set; }
        public string errmsg { get; set; }
        public string ticket { get; set; }
        public int expires_in { get; set; }
    }
    #endregion
}