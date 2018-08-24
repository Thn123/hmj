using Hmj.Business.ServiceImpl;
using Hmj.Entity;
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
    public partial class Reg : WeiPage
    {
        MySmallShopService mss = new MySmallShopService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["s"] == null)
                {
                    //Session["FromUserName"] = "oQaIMwPgnsBpwQYfwLQnUBbmQKS10";
                    //Session["ToUserName"] = "oQaIMwPgnsBpwQYfwLQnUBbmQKS3";
                    Base();
                    if ((Request.QueryString["FromUserName"] != null || Session["FromUserName"] != null) && (Request.QueryString["ToUserName"] != null || Session["ToUserName"] != null))
                    {
                        string user = Request.QueryString["FromUserName"] == null ? Session["FromUserName"].ToString() : Request.QueryString["FromUserName"].ToString();
                        string user2 = Request.QueryString["ToUserName"] == null ? Session["ToUserName"].ToString() : Request.QueryString["ToUserName"].ToString();
                        Session["FromUserName"] = user;
                        Session["ToUserName"] = user2;
                        CUST_INFO c = mss.GetCust(user);
                        if (c != null)
                        {
                            Response.Redirect("https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx32364024f2c86185&redirect_uri=http://wechat.censh.com/wechat/NewMember/ka.aspx&response_type=code&scope=snsapi_base&state=STATE&connect_redirect=1#wechat_redirect");
                        }

                    }
                    List<Region> list = mss.GetRegion();

                    DataTable dt = ListToDataTable(list);

                    ddlS.Items.Clear();
                    ddlS.Items.Add(new ListItem("请选择省份", "0"));
                    foreach (DataRow dr in dt.Rows)
                    {
                        ddlS.Items.Add(new ListItem(dr["RegionName"].ToString(), dr["RegionCode"].ToString()));
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

    }
}