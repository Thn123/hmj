using Hmj.Business.ServiceImpl;
using Hmj.Common;
using Hmj.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web.UI.WebControls;

namespace Hmj.WebApp.wechat.Area
{
    public partial class daohang : WeiPage
    {
        MySmallShopService mss = new MySmallShopService();
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["X"] = "121.459441";
                Session["Y"] = "31.224461";
                List<MDSearch> list = mss.GetAllProvince();

                DataTable dt2 = ListToDataTable(list);

                DropDownList1.Items.Clear();
                DropDownList1.Items.Add(new ListItem("请选择省份"));
                foreach (DataRow dr in dt2.Rows)
                {
                    DropDownList1.Items.Add(new ListItem(dr["Province"].ToString()));
                }
                try
                {


                    if (Request.QueryString["code"] != null)
                    {
                        try
                        {
                            string url = string.Format(@"https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + GetAppid(AppConfig.FWHOriginalID) + "&secret=" + GetSecret(AppConfig.FWHOriginalID) + "&code={0}&grant_type=authorization_code", Request.QueryString["code"].ToString());
                            string token = PostRequest(url);
                            if (token.Contains("7200"))
                            {
                                string[] b = token.Split('\"');
                                Session["FromUserName"] = b[13];
                                Session["ToUserName"] = GetAppid(AppConfig.FWHOriginalID);
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }

                }
                catch (Exception)
                {

                }
            }

            try
            {
                if (!IsPostBack)
                {
                    PageLoad();
                }

            }
            catch (Exception)
            {

            }
        }
        private void PageLoad()
        {
            if ((Request.QueryString["FromUserName"] != null || Session["FromUserName"] != null) && (Request.QueryString["ToUserName"] != null || Session["ToUserName"] != null))
            {
                string user = Request.QueryString["FromUserName"] == null ? Session["FromUserName"].ToString() : Request.QueryString["FromUserName"].ToString();
                string user2 = Request.QueryString["ToUserName"] == null ? Session["ToUserName"].ToString() : Request.QueryString["ToUserName"].ToString();
                List<WXCUST_MSG_HIS> list = mss.GetWXCUST_MSG_HIS(string.Format(" Wxevent='location' AND FromUserName='{0}' ORDER BY id DESC ", user));  //获取用户最新地址
                if (list.Count == 0)  //如果没有用户地址，则使用默认地址
                {
                    Response.Write(@"<script> var markerArr = [{ title: '没有你的坐标', content: '没有你的坐标', point: '121.459441|31.224461', isOpen: 0, icon: { w: 23, h: 25, l: 46, t: 21, x: 9, lb: 12} }
		 ];
        function createMap(){
        var map = new BMap.Map('dituContent');
        var point = new BMap.Point(121.488186,31.249162);
        map.centerAndZoom(point,13);
        window.map = map;
    }</script>");
                    Session["X"] = "121.459441";
                    Session["Y"] = "31.224461";
                }
                else
                {
                    Session["X"] = list[0].LOCATION_X;
                    Session["Y"] = list[0].LOCATION_Y;
                    string mp = @"<script> var markerArr = [{ title: '你的位置', content: '你的位置', point: '{0}|{1}', isOpen: 0, icon: {w:23,h:25,l:23,t:21,x:9,lb:12} }
		 ];
function createMap(){
        var map = new BMap.Map('dituContent');
        var point = new BMap.Point({0},{1});
        map.centerAndZoom(point,13);
        window.map = map;
    }</script>";
                    mp = mp.Replace("{0}", Session["X"].ToString()).Replace("{1}", Session["Y"].ToString());
                    Response.Write(mp);

                    WebClient client = new WebClient();
                    string url2 = string.Format("http://api.map.baidu.com/geocoder/v2/?ak=42095ab67452cfefd9b5d3743d197f49&callback=renderReverse&location={0},{1}&output=json&pois=1", list[0].LOCATION_Y, list[0].LOCATION_X);// +list[0].Location_X + "," + list[0].Location_Y + "";  //根据用户坐标查找用户地址
                    client.Encoding = Encoding.UTF8;
                    string responseTest = client.DownloadString(url2);

                    foreach (ListItem l in DropDownList1.Items)
                    {
                        if (responseTest.Contains(l.Text))
                            DropDownList1.SelectedValue = l.Text;   //显示用户所在城市门店
                    }
                    //DropDownList1_SelectedIndexChanged(null, null);

                }
            }
            else
            {
                Response.Write(@"<script> var markerArr = [{ title: '没有你的坐标', content: '没有你的坐标', point: '121.459441|31.224461', isOpen: 0, icon: { w: 23, h: 25, l: 46, t: 21, x: 9, lb: 12} }
		 ];
function createMap(){
        var map = new BMap.Map('dituContent');//在百度地图容器中创建一个地图
        var point = new BMap.Point(121.488186,31.249162);//定义一个中心点坐标
        map.centerAndZoom(point,13);//设定地图的中心点和坐标并将地图显示在地图容器中
        window.map = map;//将map变量存储在全局
    }</script>");
            }
            DropDownList1_SelectedIndexChanged(null, null);
        }
        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<MDSearch> list = mss.GetAllCity(DropDownList1.SelectedValue);

            DataTable dt = ListToDataTable(list);
            DropDownList2.Items.Clear();
            DropDownList2.Items.Add(new ListItem("请选择城市"));
            foreach (DataRow dr in dt.Rows)
            {
                DropDownList2.Items.Add(new ListItem(dr["City"].ToString()));
            }
            DropDownList2_SelectedIndexChanged(null, null);
        }
        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Province = "";
            if (DropDownList1.SelectedValue != "请选择省份")
                Province = " Province='" + DropDownList1.SelectedValue + "' ";
            List<AreaManage> list = null;
            if (DropDownList2.SelectedValue != "请选择城市")
            {
                list = mss.GetMoArea(Province + " and City = '" + DropDownList2.SelectedValue + "'");
            }
            else
            {
                list = mss.GetMoArea(Province);
            }

            string html = "<ul>";
            foreach (AreaManage area in list)
            {
                if (area.Title != "")
                {
                    if (area.URL != "")
                    {
                        html += string.Format("<li><p><a href='{1}'>{0}</a></p></li>", area.Title, area.URL);
                    }
                    else
                    {
                        html += string.Format("<li><p><a href='Html.aspx?Id={1}'>{0}</a></p></li>", area.Title, area.Id);
                    }
                }

            }
            html += "</ul>";

            se.InnerHtml = html;
        }

        /// <summary>
        ///计算两点GPS坐标的距离
        /// </summary>
        /// <param name="n1">第一点的纬度坐标</param>
        /// <param name="e1">第一点的经度坐标</param>
        /// <param name="n2">第二点的纬度坐标</param>
        /// <param name="e2">第二点的经度坐标</param>
        /// <returns></returns>
        public double Distance(double n1, double e1, double n2, double e2)
        {
            double jl_jd = 102834.74258026089786013677476285;
            double jl_wd = 111712.69150641055729984301412873;
            double b = Math.Abs((e1 - e2) * jl_jd);
            double a = Math.Abs((n1 - n2) * jl_wd);
            return Math.Sqrt((a * a + b * b));

        }
        protected void DropDownList3_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<MDSearch> list = mss.GetAllCity(DropDownList3.SelectedValue);
            DataTable dt = ListToDataTable(list);
            DropDownList1.Items.Clear();
            DropDownList1.Items.Add(new ListItem("请选择城市"));
            foreach (DataRow dr in dt.Rows)
            {
                DropDownList1.Items.Add(new ListItem(dr["City"].ToString()));
            }
            DropDownList1_SelectedIndexChanged(null, null);
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            DropDownList1_SelectedIndexChanged(null, null);
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            DropDownList2_SelectedIndexChanged(null, null);
        }

        protected void Button5_Click(object sender, EventArgs e)
        {

            Session["X"] = "121.459441";
            Session["Y"] = "31.224461";
            List<MDSearch> list = mss.GetAllProvince();

            DataTable dt2 = ListToDataTable(list);

            DropDownList1.Items.Clear();
            DropDownList1.Items.Add(new ListItem("请选择省份"));
            foreach (DataRow dr in dt2.Rows)
            {
                DropDownList1.Items.Add(new ListItem(dr["Province"].ToString()));
            }
            //BaseLoad();
            PageLoad();
        }
    }
}