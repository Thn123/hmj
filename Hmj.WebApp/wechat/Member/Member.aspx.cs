using Hmj.Business.ServiceImpl;
using Hmj.Entity;
using Hmj.WebApp.SelectMemberNew;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hmj.WebApp.wechat.Member
{
    public partial class Member : WeiPage
    {
        MySmallShopService mss = new MySmallShopService();
        public static string address = "";
        public static string nameval = "";
        public static string cardnoval = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            //Session["FromUserName"] = "oQaIMwPgnsBpwQYfwLQnUBbmQKS4";
            //Session["ToUserName"] = "oQaIMwPgnsBpwQYfwLQnUBbmQKS10";
            if (!IsPostBack)
            {
                Base();
                if (Request.QueryString["s"] == null)
                {
                    if ((Request.QueryString["FromUserName"] != null || Session["FromUserName"] != null) && (Request.QueryString["ToUserName"] != null || Session["ToUserName"] != null))
                    {
                        string user = Request.QueryString["FromUserName"] == null ? Session["FromUserName"].ToString() : Request.QueryString["FromUserName"].ToString();
                        string user2 = Request.QueryString["ToUserName"] == null ? Session["ToUserName"].ToString() : Request.QueryString["ToUserName"].ToString();
                        Session["FromUserName"] = user;
                        Session["ToUserName"] = user2;
                        try
                        {
                            Bind(user);
                        }
                        catch (Exception ex)
                        {
                            WXLOG log = new WXLOG();
                            log.CON = ex.Message.ToString() + "Member";
                            log.TIME = DateTime.Now;
                            mss.SaveLog(log);
                        }
                    }
                }
                else if (Request.QueryString["s"] != null)
                {
                    string html = "<option value='0'>请选择城市</option>";
                    List<City> list = mss.GetCityByReg(Request.QueryString["s"]);
                    foreach (City c in list)
                    {
                        html += "<option value='" + c.CityCode + "'>" + c.CityName + "</option>";
                    }
                    Response.Write(html);
                    Response.End();
                }
            }
        }
        public static DataTable ListToDataTable(IList list)
        {
            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    //获取类型
                    Type colType = pi.PropertyType;
                    //当类型为Nullable<>时
                    if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    {
                        colType = colType.GetGenericArguments()[0];
                    }
                    result.Columns.Add(pi.Name, colType);
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

        public void Bind(string FromUserName)
        {

            List<Region> list = mss.GetRegion();

            DataTable dt = ListToDataTable(list);

            ddlS.Items.Clear();
            ddlS.Items.Add(new ListItem("请选择省份", "0"));
            foreach (DataRow dr in dt.Rows)
            {
                ddlS.Items.Add(new ListItem(dr["RegionName"].ToString(), dr["RegionCode"].ToString()));
            }
            CUST_INFO cust = mss.GetCust(FromUserName);
            if (cust != null)
            {
                Z_LOY_BP_GETDETAILResponse zloy = SelectMember(cust.CardNo);
                if (zloy.T_CENTRAL != null && zloy.T_CENTRAL.Length > 0)
                {
                    address = zloy.T_ADDRESSDATA[0].STREET;
                    nameval = zloy.T_CENTRALDATAPERSON[0].LASTNAME;
                    cardnoval = zloy.E_USER_NO;
                    City c = mss.GetCityByCode(int.Parse(zloy.T_ADDRESSDATA[0].CITY));
                    ddlC.Items.Clear();
                    ddlC.Items.Add(new ListItem(c.CityName, c.CityCode.ToString()));
                    ddlS.SelectedValue = zloy.T_ADDRESSDATA[0].REGION;
                    cardno.InnerHtml = "<em id='cardval'>" + zloy.E_USER_NO + "</em>";
                    name.InnerHtml = "<em id='nameval'>" + zloy.T_CENTRALDATAPERSON[0].LASTNAME + "</em>";
                    phone.InnerHtml = "<em id='phoneval'>" + zloy.T_TELEFONDATA[0].TELEPHONE + "</em>";
                    if (zloy.T_CENTRAL[0].TITLE_KEY == "Z001")
                    {
                        sex.InnerHtml = "<span class='nandeicon'><em id='sexval'>先生</em></span>";
                    }
                    else
                    {
                        sex.InnerHtml = "<span class='nvdeicon'><em id='sexval'>女士</em></span>";
                    }
                    string birth = zloy.T_CENTRALDATAPERSON[0].BIRTHDATE;
                    string zctime = zloy.T_ADDRESSDATA[0].STR_SUPPL3;
                    if (birth.Length > 6)
                    {
                        bir.InnerHtml = "<strong>您的生日</strong><span><bdo>" + birth.Substring(0, 4) + "年" + birth.Substring(4, 2) + "月" + birth.Substring(6, 2) + "日</bdo></span>";
                    }
                    else
                    {
                        create.InnerHtml = "<strong>您的生日</strong><span><bdo>未填写</bdo></span>";
                    }
                    if (zctime.Length > 6)
                    {
                        create.InnerHtml = "<strong>注册时间</strong><span><bdo>" + zctime.Substring(0, 4) + "年" + zctime.Substring(4, 2) + "月" + zctime.Substring(6, 2) + "日</bdo></span>";
                    }
                    else
                    {
                        create.InnerHtml = "<strong>注册时间</strong><span><bdo>未填写</bdo></span>";
                    }
                    guitai.InnerHtml = "<strong>注册柜台</strong><span><bdo>" + zloy.T_CENTRAL[0].TITLELETTER + "</bdo></span>";

                    if (zloy.T_CUSTOMER06[0].ZA39 == "X")
                    {
                        radsj.InnerHtml = "<input name='' type='radio' value='手机' class='radiocss checked2'>";
                    }
                    if (zloy.T_CUSTOMER06[0].ZA40 == "X")
                    {
                        raddx.InnerHtml = "<input name='' type='radio' value='短信' class='radiocss checked2'>";
                    }
                    if (zloy.T_CUSTOMER06[0].ZA41 == "X")
                    {
                        radyj.InnerHtml = "<input name='' type='radio' value='邮件' class='radiocss checked2'>";
                    }
                    if (zloy.T_CUSTOMER06[0].ZA42 == "X")
                    {
                        radlp.InnerHtml = "<input name='' type='radio' value='礼品' class='radiocss checked2'>";
                    }
                    if (zloy.T_CUSTOMER06[0].ZA43 == "X")
                    {
                        radzy.InnerHtml = "<input name='' type='radio' value='直邮' class='radiocss checked2'>";
                    }
                    if (zloy.T_CUSTOMER06[0].ZA44 == "X")
                    {
                        radhd.InnerHtml = "<input name='' type='radio' value='活动' class='radiocss checked2'>";
                    }
                }
            }
            else
            {
                Response.Redirect("https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx32364024f2c86185&redirect_uri=http://wechat.censh.com/wechat/Member/Band.aspx&response_type=code&scope=snsapi_base&state=STATE&connect_redirect=1#wechat_redirect");
            }
        }
    }

}