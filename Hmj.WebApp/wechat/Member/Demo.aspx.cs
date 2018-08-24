using Hmj.Business.ServiceImpl;
using Hmj.Common;
using Hmj.Entity;
using Hmj.Interface;
using Hmj.WebApp.SelectMemberNew;
using LitJson;
//using Hmj.WebApp.InsertMember;
//using Hmj.WebApp.SelectOrderNew;
//using Hmj.WebApp.SendSMS;
//using Hmj.WebApp.UpdateMember;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Hmj.Entity.Entities;
using System.Collections;
using System.Reflection;
using Hmj.Entity.PageSearch;
using Hmj.Entity.SearchEntity;
using WeChatCRM.Common.Utils;
using System.Net;
using System.Web.Security;

namespace Hmj.WebApp.wechat.Member
{

    public partial class Demo : WeiPage
    {
        ISystemService sbo = new SystemService();
        //MySmallShopService mss = new MySmallShopService();
        protected void Page_Load(object sender, EventArgs e)
        {

            //string data = string.Format("openId={0}&content={1}&type={2}", "oQaIMwK7r6LOeAU5Vxm7xRyAbQe8", "啊啊啊", 1);
            //string result = NetHelper.HttpRequest("http://testqyh.censh.cn/censh/dialogue/receiver", data, HttpVerbs.POST, 6000, Encoding.UTF8, ContentTypes.FORM);
            //Response.Write(result);
            //    string url = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + new WeiPage().Token(AppConfig.FWHOriginalID);
            //    //这里需要修改
            //    int ewmid_max = sbo.GetMaxEwmId();//获取最大的二维码id
            //    string Ticket = "";
            //    string d = @"{ ""action_name"": ""QR_LIMIT_SCENE"", ""action_info"": {""scene"": {""scene_id"": {0}}}

            //}";
            //      d = d.Replace("{0}","10000");
            //        string mes = WeiPage.HttpXmlPostRequest(url, d, Encoding.UTF8);
            //        JsonData info = JsonMapper.ToObject(mes);
            //        string[] b = mes.Split('\"');
            //        string ticket = info["ticket"].ToString();
            //       Ticket = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + ticket + "\r\n";
            //    Response.Redirect("Ticket");

            //opEwmId();
            Response.Write(PostBinaryData());
        }

        /// <summary>
        /// PostBinaryData
        /// </summary>
        /// <param name="url">要发送的 url 网址</param>
        /// <param name="bytes">要发送的数据流</param>
        /// <returns></returns>
        public string PostBinaryData()
        {
            //下面是测试例子
         
            string url = "http://wechat.censh.com/Api/SendImage.do?appid=bf_m_221FG32v2&timestamp=" + Utility.ConvertDateTimeInt(DateTime.Now).ToString() + "&sign=" + GetSignTest();
            string img = HttpContext.Current.Server.MapPath("./images/btn01.png");
            byte[] bytes = File.ReadAllBytes(img);
            HttpWebRequest wRequest = (HttpWebRequest)WebRequest.Create(url);
            wRequest.ContentType = "multipart/form-data";
            wRequest.ContentLength = bytes.Length;
            wRequest.Method = "POST";
            Stream stream = wRequest.GetRequestStream();
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();
            HttpWebResponse wResponse = (HttpWebResponse)wRequest.GetResponse();
            StreamReader sReader = new StreamReader(wResponse.GetResponseStream(), System.Text.Encoding.UTF8);
            string str = sReader.ReadToEnd();
            sReader.Close();
            wResponse.Close();
            return str;
        }

        private string GetSignTest()
        {
            string[] ArrTmp = { "appid=bf_m_221FG32v2", "secretkey=pZdrLtOnkgWzqi3U503TtA==", "timestamp=" + Utility.ConvertDateTimeInt(DateTime.Now).ToString() };
            Array.Sort(ArrTmp);     //字典排序
            string tmpStr = string.Join("&", ArrTmp);
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "MD5");
            tmpStr = tmpStr.ToLower();
            return tmpStr;
        }

        public void opEwmId()
        {
            WeiPage wp = new WeiPage();
            string url = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + wp.Token("gh_b43dad5de37a");
            //这里需要修改
            string Ticket = "";
            for (int i = 1; i <= 10; i++)
            {
                string d = @"{ ""action_name"": ""QR_LIMIT_SCENE"", ""action_info"": {""scene"": {""scene_id"": {0}}}
        
        }";
                d = d.Replace("{0}", i.ToString());
                string mes = WeiPage.HttpXmlPostRequest(url, d, Encoding.UTF8);
                JsonData info = JsonMapper.ToObject(mes);
                string[] b = mes.Split('\"');
                string ticket = info["ticket"].ToString();
                Ticket += i + "|https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + ticket + "\r\n";
            }
        }

        public void backfan()
        {
            List<CUST_INFO> list = mss.GetNullFans();
            foreach (var item in list)
            {
                try
                {
                    InsertFS(item.FROM_USER_NAME, AppConfig.FWHOriginalID, 0);
                }
                catch (Exception)
                {

                    continue;
                }

            }


        }

        public string FanDemo(int index)
        {
            DataSet ds = new DataSet();
            WeiPage w = new WeiPage();
            List<FansList_Ex> list = sbo.QueryFansListByBuy();
            RoleSearch search = new RoleSearch();
            PageView view = new PageView();
            view.PageIndex = index;
            view.PageSize = 10;
            PagedList<CUST_INFO> list_cust = sbo.QueryCustList(search, view);
            //foreach (var item in list_cust.DataList)
            //{
            //    if (!string.IsNullOrEmpty(item.MOBILE))
            //    {
            //        try
            //        {


            //        Z_LOY_BP_GETDETAILResponse zloy = w.SelectMemberByUpdate(item.MOBILE);
            //        int code = 0;
            //        if (zloy.T_ADDRESSDATA.Length > 0)
            //        {
            //            if (zloy.T_ADDRESSDATA[0].CITY!="")
            //            {
            //                code = int.Parse(zloy.T_ADDRESSDATA[0].CITY);
            //            }

            //        }
            //        City c = mss.GetCityByCode(code);
            //        CUST_INFO cust = mss.GetCust(item.FROM_USER_NAME);
            //        cust.CustLevel = zloy.E_LEVEL;
            //        cust.CustCity = c == null ? "" : c.CityName;
            //        mss.UpdateCustInfoS(cust);
            //        }
            //        catch (Exception e)
            //        {

            //            continue;
            //        }
            //    }

            //}

            ds.Tables.Add(ToDataTable(list));
            NewNPOI.ExportDataTableToExcel(ds.Tables[0], "test1.xls", "test1");
            return (index + 1).ToString() + "_" + list_cust.Total;
        }


        public void login()
        {
            string redirectUrlSM = Server.UrlEncode("http://wechat.censh.com/Token.aspx");
            string url = "https://open.weixin.qq.com/connect/qrconnect?appid=wx32364024f2c86185&redirect_uri=" + redirectUrlSM + "&response_type=code&scope=snsapi_login&state=STATE#wechat_redirect";
            Response.Redirect(url);
        }
        public void ApiDemo()
        {
            string url = "http://test2.censh.cn/customer/account/sso";
            string d = @"{ ""callback"": ""wechat.censh.com"", ""channel"" :""wx"" ,""unique"":""oQaIMwPgnsBpwQYfwLQnUBbmQKS10"" }";
            string mes = WeiPage.HttpXmlPostRequest(url, d, Encoding.UTF8);
        }
        public void GjDemo()
        {
            string url = "http://qyh.censh.com/shengshi/sa/updateRedisCache?fromUserName=oQaIMwO7X_dUu6Q7j1nHvF6_ciN0&mobile=18501752942&type=add";
            string mes = PostRequest(url);
        }

        public void Add()
        {
            Emp_Cust e = sbo.GetEmp_Cust("oQaIMwPgnsBpwQYfwLQnUBbmQKS10");
            if (e == null)
            {
                e = new Emp_Cust();
                e.EmpId = int.Parse("10008");
                e.FromUserName = "oQaIMwPgnsBpwQYfwLQnUBbmQKS10";
                e.CreateTime = DateTime.Now;
                sbo.AddEmp_Cust(e);
            }
        }
        public void Get()
        {

            DataTable dt_base = ImportExcelFile(@"C:\Users\Administrator\Desktop\test2016.xls");
            DataTable dt = Distinct(dt_base);//自身去重
            List<EMPLOYEE> emplist = sbo.QueryAllEmp();
            DataTable dtNew1 = dt.Clone();
            foreach (var item in emplist)
            {
                DataRow[] drArr1 = dt.Select(" B ='" + item.MOBILE + "'");
                for (int i = 0; i < drArr1.Length; i++)
                {
                    // dtNew1.ImportRow(drArr1[i]);
                    dt.Rows.Remove(drArr1[i]);//删除重复数据
                }

            }
            WeiPage wp = new WeiPage();
            string url = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + wp.Token(AppConfig.FWHOriginalID);
            //这里需要修改
            int ewmid_max = sbo.GetMaxEwmId();//获取最大的二维码id
            string Ticket = "";
            int j = 0;
            for (int i = (ewmid_max + 1); i < (ewmid_max + dt.Rows.Count + 1); i++)
            {
                string d = @"{ ""action_name"": ""QR_LIMIT_SCENE"", ""action_info"": {""scene"": {""scene_id"": {0}}}
        
        }";
                d = d.Replace("{0}", i.ToString());
                string mes = WeiPage.HttpXmlPostRequest(url, d, Encoding.UTF8);
                JsonData info = JsonMapper.ToObject(mes);
                string[] b = mes.Split('\"');
                string ticket = info["ticket"].ToString();
                Ticket += "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + ticket + "\r\n";
                EMPLOYEE emp = new EMPLOYEE();
                emp.USERID = dt.Rows[j]["B"].ToString();
                emp.NAME = dt.Rows[j]["A"].ToString();
                emp.MOBILE = dt.Rows[j]["B"].ToString();
                emp.StoreName = dt.Rows[j]["C"].ToString();
                emp.AreaName = dt.Rows[j]["D"].ToString();
                emp.EwmId = i;
                emp.EwmUrl = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + ticket;
                sbo.SaveEMPLOYEE(emp);
                j++;
            }


            string ti = Ticket;
        }

        /// <summary>
        /// mobile重复的删除
        /// </summary>
        /// <param name="SourceDt"></param>
        /// <returns></returns>
        public DataTable Distinct(DataTable SourceDt)
        {

            for (int i = SourceDt.Rows.Count; i > 0; i--)
            {
                DataRow[] rows = SourceDt.Select(string.Format("{0}='{1}'", "B", SourceDt.Rows[i]["B"]));
                if (rows.Length > 1)
                {
                    SourceDt.Rows.RemoveAt(i);
                }
            }
            return SourceDt;


        }

        #region 导入excel
        HSSFWorkbook hssfworkbook;
        public DataTable ImportExcelFile(string filePath)
        {
            #region//初始化信息
            try
            {
                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    hssfworkbook = new HSSFWorkbook(file);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            #endregion

            ISheet sheet = hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            DataTable dt = new DataTable();
            for (int j = 0; j < (sheet.GetRow(0).LastCellNum); j++)
            {
                dt.Columns.Add(Convert.ToChar(((int)'A') + j).ToString());//添加列名
            }
            while (rows.MoveNext())
            {
                HSSFRow row = (HSSFRow)rows.Current;
                DataRow dr = dt.NewRow();
                for (int i = 0; i < row.LastCellNum; i++)
                {
                    ICell cell = row.GetCell(i);
                    if (cell == null)
                    {
                        dr[i] = null;
                    }
                    else
                    {
                        dr[i] = cell.ToString();
                    }
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public DataTable ToDataTable(IList list)
        {
            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    result.Columns.Add(pi.Name, pi.PropertyType);
                }
                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }
        #endregion

        //private System.Text.Encoding encoding;
        //public System.Text.Encoding Encoding
        //{
        //    get
        //    {
        //        if (encoding == null)
        //        {
        //            encoding = System.Text.Encoding.UTF8;
        //        }
        //        return encoding;
        //    }

        //    set
        //    {
        //        encoding = value;
        //    }
        //}
        //public string Encrypt3DES(string strString)
        //{
        //    DESCryptoServiceProvider DES = new DESCryptoServiceProvider();

        //    DES.Key = Encoding.GetBytes("ShengShi");
        //    DES.Mode = CipherMode.ECB;
        //    DES.Padding = PaddingMode.Zeros;

        //    ICryptoTransform DESEncrypt = DES.CreateEncryptor();

        //    byte[] Buffer = encoding.GetBytes(strString);

        //    return Convert.ToBase64String(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
        //}
        //public void UpdateMember()
        //{
        //    ZWS_Z_LOY_BP_CHANGEClient client = new ZWS_Z_LOY_BP_CHANGEClient();
        //    Z_LOY_BP_CHANGE param = new Z_LOY_BP_CHANGE();

        //    param.I_PARTNER = "1000000055";

        //    ZZCENTRAL item = new ZZCENTRAL();
        //    item.SEARCHTERM1 = "李四";
        //    item.SEARCHTERM2 = "";
        //    item.TITLELETTER = "经纬大厦";
        //    item.TITLE_KEY = "Z001";  //代表男士
        //    item.DATAORIGINTYPE = "Z002"; //代表来自微信
        //    param.I_CENTRAL = item;

        //    ZZCENTRALDATAPERSON item3 = new ZZCENTRALDATAPERSON();
        //    item3.BIRTHDATE = "19941027";
        //    item3.LASTNAME = "李";
        //    item3.PREFIX1 = "";
        //    item3.BIRTHPLACE = "";
        //    item3.MARITALSTATUS = "";
        //    item3.TITLE_ACA1 = "";
        //    item3.TITLE_ACA2 = "";
        //    item3.TITLE_SPPL = "";
        //    item3.BIRTHNAME = "";
        //    item3.FIRSTNAME = "";
        //    item3.OCCUPATION = "Z001";
        //    item3.MIDDLENAME = "";
        //    item3.SECONDNAME = "";
        //    param.I_CENTRALDATAPERSON = item3;

        //    ZZADDRESSDATA item4 = new ZZADDRESSDATA();
        //    item4.COUNTRY = "CN";
        //    item4.REGION = "491";
        //    item4.CITY = "60";
        //    item4.STREET = "哈哈哈";
        //    item4.POSTL_COD1 = "";
        //    item4.STR_SUPPL1 = "";
        //    item4.STR_SUPPL2 = "";
        //    item4.STR_SUPPL3 = "20151106";
        //    item4.BUILDING = "";
        //    param.I_ADDRESSDATA = item4;



        //    ZZCUSTOMER06 item11 = new ZZCUSTOMER06();
        //    item11.ZA39 = "X";
        //    item11.ZA40 = "X";
        //    item11.ZA41 = "X";
        //    item11.ZA42 = "";
        //    item11.ZA43 = "";
        //    item11.ZA44 = "";
        //    param.I_CUSTOMER06 = item11;

        //    param.T_RETURN = new ZZRETURN2[] { };

        //    var response = client.Z_LOY_BP_CHANGE(param);
        //}


        //public void SelectOrder()
        //{
        //    ZWS_Z_LOY_BP_GETORDERClient client = new ZWS_Z_LOY_BP_GETORDERClient();
        //    Z_LOY_BP_GETORDER param = new Z_LOY_BP_GETORDER();
        //    ZZSCRMD_ORDER z = new ZZSCRMD_ORDER();
        //    param.I_ZORP = "1000000011";
        //    param.I_ZCREATED_AT_BEGIN = "";
        //    param.I_ZCREATED_AT_END = "";
        //    param.T_ZCRMD_ORDER = new ZZSCRMD_ORDER[] { };
        //    param.T_RETURN = new ZZRETURN2[] { };
        //    var response = client.Z_LOY_BP_GETORDER(param);
        //}


        //public void SendSMS()
        //{
        //    Hmj.WebApp.SendSMS.SMSServiceSoapClient s = new Hmj.WebApp.SendSMS.SMSServiceSoapClient();
        //    int yzm = 123456;
        //    string jg = s.SendSMS("18221595758", 8, "您的验证码是" + yzm + "，在30分钟内有效，如非本人操作请忽略该短信。", DateTime.Parse("2010-01-01"), "7101008830419833");
        //}

        //public void SelectMember()
        //{
        //    ZWS_Z_LOY_BP_GETDETAILClient client = new ZWS_Z_LOY_BP_GETDETAILClient();
        //    Z_LOY_BP_GETDETAIL param = new Z_LOY_BP_GETDETAIL();
        //    List<ZZINPUT_RANGE> list = new List<ZZINPUT_RANGE>();
        //    ZZINPUT_RANGE item1 = new ZZINPUT_RANGE();
        //    item1.SIGN = "I";
        //    item1.OPTION = "EQ";
        //    item1.LOW = "13917963289";
        //    item1.HIGH = "";
        //    list.Add(item1);
        //    param.T_INPUT_RANGE = list.ToArray();
        //    param.T_CENTRAL = new ZZCENTRAL[] { };
        //    param.T_ADDRESSDATA = new ZZADDRESSDATA[] { };
        //    var response = client.Z_LOY_BP_GETDETAIL(param);
        //}



        //public void InsertMember()
        //{
        //    ZWS_Z_LOY_BP_CREATEClient client = new ZWS_Z_LOY_BP_CREATEClient();
        //    Z_LOY_BP_CREATE param = new Z_LOY_BP_CREATE();

        //    ZZCENTRAL item = new ZZCENTRAL();
        //    item.SEARCHTERM1 = "李四";
        //    item.SEARCHTERM2 = "";
        //    item.TITLELETTER = "经纬大厦";
        //    item.TITLE_KEY = "Z001";  //代表男士
        //    item.DATAORIGINTYPE = "Z002"; //代表来自微信
        //    param.I_CENTRAL = item;

        //    ZZCENTRALDATAPERSON item3 = new ZZCENTRALDATAPERSON();
        //    item3.BIRTHDATE = "19941027";
        //    item3.LASTNAME = "李";
        //    item3.PREFIX1 = "";
        //    item3.BIRTHPLACE = "";
        //    item3.MARITALSTATUS = "";
        //    item3.TITLE_ACA1 = "";
        //    item3.TITLE_ACA2 = "";
        //    item3.TITLE_SPPL = "";
        //    item3.BIRTHNAME = "";
        //    item3.FIRSTNAME = "";
        //    item3.OCCUPATION = "Z001";
        //    item3.MIDDLENAME = "";
        //    item3.SECONDNAME = "";
        //    param.I_CENTRALDATAPERSON = item3;

        //    ZZADDRESSDATA item4 = new ZZADDRESSDATA();
        //    item4.COUNTRY = "CN";
        //    item4.REGION = "485";
        //    item4.CITY = "2";
        //    item4.STREET = "";
        //    item4.POSTL_COD1 = "";
        //    item4.STR_SUPPL1 = "";
        //    item4.STR_SUPPL2 = "";
        //    item4.STR_SUPPL3 = "20151106";
        //    item4.BUILDING = "";
        //    param.I_ADDRESSDATA = item4;


        //    ZZTELEFONDATA item5 = new ZZTELEFONDATA();
        //    item5.TELEPHONE = "18501752941";
        //    param.I_TELEFONDATA = item5;

        //    ZZE_MAILDATA item10 = new ZZE_MAILDATA();
        //    item10.E_MAIL = "terry.zhang@puman.com";
        //    param.I_E_MAILDATA = item10;



        //    //ZZCUSTOMER01 item6 = new ZZCUSTOMER01();
        //    //item6.ZA01 = "";
        //    //item6.ZA02 = "";
        //    //item6.ZA03 = "";
        //    //item6.ZA04 = "";
        //    //item6.ZA05 = "";
        //    //item6.ZA06 = "";
        //    //item6.ZA07 = "";
        //    //item6.ZA08 = "";
        //    //item6.ZA09 = "";
        //    //item6.ZA10 = "";
        //    //param.I_CUSTOMER01 = item6;


        //    //ZZCUSTOMER02 item7 = new ZZCUSTOMER02();
        //    //item7.ZA11 = "";
        //    //item7.ZA12 = "";
        //    //item7.ZA13 = "";
        //    //item7.ZA14 = "";
        //    //param.I_CUSTOMER02 = item7;


        //    //ZZCUSTOMER03 item8 = new ZZCUSTOMER03();
        //    //item8.ZA15 = "";
        //    //item8.ZA16 = "";
        //    //item8.ZA17 = "";
        //    //item8.ZA18 = "";
        //    //item8.ZA19 = "";
        //    //item8.ZA20 = "";
        //    //item8.ZA21 = "";
        //    //item8.ZA22 = "";
        //    //item8.ZA23 = "";
        //    //item8.ZA24 = "";
        //    //item8.ZA25 = "";
        //    //param.I_CUSTOMER03 = item8;


        //    //ZZCUSTOMER04 item9 = new ZZCUSTOMER04();
        //    //item9.ZA26 = "";
        //    //item9.ZA27 = "";
        //    //item9.ZA28 = "";
        //    //item9.ZA29 = "";
        //    //item9.ZA30 = "";
        //    //item9.ZA31 = "";
        //    //item9.ZA32 = "";
        //    //item9.ZA33 = "";
        //    //param.I_CUSTOMER04 = item9;

        //    ZZCUSTOMER06 item11 = new ZZCUSTOMER06();
        //    item11.ZA39 = "X";
        //    item11.ZA40 = "";
        //    item11.ZA41 = "";
        //    item11.ZA42 = "";
        //    item11.ZA43 = "";
        //    item11.ZA44 = "";
        //    param.I_CUSTOMER06 = item11;


        //    List<BAPIRET2> list = new List<BAPIRET2>();
        //    BAPIRET2 item2 = new BAPIRET2();
        //    list.Add(item2);
        //    param.T_RETURN = list.ToArray();

        //    var response = client.Z_LOY_BP_CREATE(param);
        //}
    }

}