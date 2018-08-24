using Hmj.Entity;
using Hmj.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.Security;
using WeChatCRM.Common.Utils;

namespace Hmj.Common
{
    /// <summary>
    /// 公共的帮助类
    /// </summary>
    public class Utility
    {
        public const double EARTH_RADIUS = 6378.137;//地球半径
        public static string ClearSafeStringParma(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                return input.Replace("--", "").Replace("'", "").Replace(";", "；");
            }
            return "";
        }
        public static string Base64EnCode(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return "";
            }
            byte[] b = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(b);
        }
        public static string Base64Decode(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return "";
            }
            byte[] b = Convert.FromBase64String(input);
            return Encoding.UTF8.GetString(b);
        }
        /// <summary>
        /// 这个方法是两个URL第一个url是微信的，第二个是本地图片路径
        /// </summary>
        /// <param name="url"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string HttpUploadFile(string url, string path)
        {
            // 设置参数
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            request.Method = "POST";
            string boundary = DateTime.Now.Ticks.ToString("X"); // 随机分隔线
            request.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;
            byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
            byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");

            int pos = path.LastIndexOf("\\");
            string fileName = path.Substring(pos + 1);

            //请求头部信息
            StringBuilder sbHeader = new StringBuilder(string.Format("Content-Disposition:form-data;name=\"file\";filename=\"{0}\"\r\nContent-Type:application/octet-stream\r\n\r\n", fileName));
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sbHeader.ToString());

            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] bArr = new byte[fs.Length];
            fs.Read(bArr, 0, bArr.Length);
            fs.Close();

            Stream postStream = request.GetRequestStream();
            postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
            postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
            postStream.Write(bArr, 0, bArr.Length);
            postStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
            postStream.Close();

            //发送请求并获取相应回应数据
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream instream = response.GetResponseStream();
            StreamReader sr = new StreamReader(instream, Encoding.UTF8);
            //返回结果网页（html）代码
            string content = sr.ReadToEnd();
            return content;
        }


        /// <summary>    
        /// 计算坐标点的距离    
        /// </summary>    
        /// <param name="begin">开始的经度纬度</param>    
        /// <param name="end">结束的经度纬度</param>    
        /// <returns>距离(公里)*1000=米</returns>    
        public static double GetDistance(Poin begin, Poin end)
        {
            double lat = begin.RadLat - end.RadLat;
            double lng = begin.RadLng - end.RadLng;

            double dis = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(lat / 2), 2) + Math.Cos(begin.RadLat) * Math.Cos(end.RadLat) * Math.Pow(Math.Sin(lng / 2), 2)));
            dis = dis * EARTH_RADIUS;
            dis = Math.Round(dis * 1e4) / 1e4;

            return dis * 1000;//乘以1000就是 米数 计算出来的原本是 公里
        }
        //C# 获取农历日期

        ///<summary>
        /// 实例化一个 ChineseLunisolarCalendar
        ///</summary>
        private static ChineseLunisolarCalendar ChineseCalendar = new ChineseLunisolarCalendar();

        ///<summary>
        /// 十天干
        ///</summary>
        private static string[] tg = { "甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸" };

        ///<summary>
        /// 十二地支
        ///</summary>
        private static string[] dz = { "子", "丑", "寅", "卯", "辰", "巳", "午", "未", "申", "酉", "戌", "亥" };

        ///<summary>
        /// 十二生肖
        ///</summary>
        private static string[] sx = { "鼠", "牛", "虎", "免", "龙", "蛇", "马", "羊", "猴", "鸡", "狗", "猪" };

        ///<summary>
        /// 返回农历天干地支年
        ///</summary>
        ///<param name="year">农历年</param>
        ///<return s></return s>
        public static string GetLunisolarYear(int year)
        {
            if (year > 3)
            {
                int tgIndex = (year - 4) % 10;
                int dzIndex = (year - 4) % 12;

                return string.Concat(tg[tgIndex], dz[dzIndex], "[", sx[dzIndex], "]");
            }

            throw new ArgumentOutOfRangeException("无效的年份!");
        }

        ///<summary>
        /// 农历月
        ///</summary>

        ///<return s></return s>
        private static string[] months = { "正", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "十二(腊)" };

        ///<summary>
        /// 农历日
        ///</summary>
        private static string[] days1 = { "初", "十", "廿", "三" };
        ///<summary>
        /// 农历日
        ///</summary>
        private static string[] days = { "一", "二", "三", "四", "五", "六", "七", "八", "九", "十" };


        ///<summary>
        /// 返回农历月
        ///</summary>
        ///<param name="month">月份</param>
        ///<return s></return s>
        public static string GetLunisolarMonth(int month)
        {
            if (month < 13 && month > 0)
            {
                return months[month - 1];
            }

            throw new ArgumentOutOfRangeException("无效的月份!");
        }

        /// <summary>
        /// datetime时间转换为unix
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }


        ///<summary>
        /// 返回农历日
        ///</summary>
        ///<param name="day">天</param>
        ///<return s></return s>
        public static string GetLunisolarDay(int day)
        {
            if (day > 0 && day < 32)
            {
                if (day != 20 && day != 30)
                {
                    return string.Concat(days1[(day - 1) / 10], days[(day - 1) % 10]);
                }
                else
                {
                    return string.Concat(days[(day - 1) / 10], days1[1]);
                }
            }

            throw new ArgumentOutOfRangeException("无效的日!");
        }

        public static void RequestUrl(string url, string data)//发送方法 
        {
            var request = WebRequest.Create(url);
            request.Method = "post";
            request.ContentType = "text/xml";
            request.Headers.Add("charset:utf-8");
            var encoding = Encoding.GetEncoding("utf-8");
            if (data != null)
            {
                byte[] buffer = encoding.GetBytes(data);
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
            }
            else
            {
                //request.ContentLength = 0; 
            }
            //using (HttpWebResponse wr = request.GetResponse() as HttpWebResponse) 
            //{ 
            //    using (StreamReader reader = new StreamReader(wr.GetResponseStream(), encoding)) 
            //    { 
            //        return reader.ReadToEnd(); 
            //    } 
            //} 
        }


        ///<summary>
        /// 根据公历获取农历日期
        ///</summary>
        ///<param name="datetime">公历日期</param>
        ///<return s></return s>
        public static string GetChineseDateTime(DateTime datetime)
        {
            int year = ChineseCalendar.GetYear(datetime);
            int month = ChineseCalendar.GetMonth(datetime);
            int day = ChineseCalendar.GetDayOfMonth(datetime);
            //获取闰月， 0 则表示没有闰月
            int leapMonth = ChineseCalendar.GetLeapMonth(year);

            bool isleap = false;

            if (leapMonth > 0)
            {
                if (leapMonth == month)
                {
                    //闰月
                    isleap = true;
                    month--;
                }
                else if (month > leapMonth)
                {
                    month--;
                }
            }

            return string.Concat(GetLunisolarYear(year), "年", isleap ? "闰" : string.Empty, GetLunisolarMonth(month), "月", GetLunisolarDay(day));
        }

        public static string GetSignTest()
        {
            string[] ArrTmp = { "appid=bf_m_23SDFv2", "secretkey=ahOm9yGmOJI9pV5kqSxhDA==", "timestamp=" + Utility.ConvertDateTimeInt(DateTime.Now).ToString() };
            Array.Sort(ArrTmp);     //字典排序
            string tmpStr = string.Join("&", ArrTmp);
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "MD5");
            tmpStr = tmpStr.ToLower();
            return tmpStr;
        }

        /// <summary>
        /// 得到描述
        /// </summary>
        /// <returns></returns>
        public static string GetDetailDis(string key)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("ZFT", "社区发帖");
            dic.Add("ZFW", "访问社区");
            dic.Add("ZHT", "社区回帖");
            dic.Add("ZZR", "积分转移");
            dic.Add("ZHD", "社区奖励活动");
            dic.Add("ZTZ", "手工调整积分");
            dic.Add("ZZJ", "手工增加积分");
            dic.Add("ZJS", "手工减少积分");
            dic.Add("ZTIER", "手工调增级别");
            dic.Add("ZRE", "家化会员退货");
            dic.Add("ZPRODUCT", "家化会员购买");
            dic.Add("PRODUCT_REDEEM", "家化会员兑换");
            dic.Add("ZRC", "社区日常活动");
            dic.Add("ZXY", "微信积分大回馈");
            dic.Add("ZRDH", "会员兑换冲销");
            dic.Add("ZTJ", "会员推荐获得积分");
            dic.Add("ZBT", "被推荐会员获得积分");


            dic.Add("ZPGQ", "积分过期");
            dic.Add("ZTHFC", "家化会员退货反冲");
            dic.Add("ZPDX", "积分抵现");

            dic.Add("ZCJ", "会员抽奖扣减积分");
            dic.Add("ZDH", "兑礼扣减积分");
            dic.Add("ZXZ", "奖励活动积分扣减");
            dic.Add("ZHK", "会员换卡");

            if (!dic.Keys.Contains(key))
            {
                return key;
            }

            return dic[key.ToUpper()];
        }

        /// <summary>
        /// 会员等级描述
        /// </summary>
        /// <returns></returns>
        public static string GetMemberLvl(string key)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("ZHMJCJ0100", "普通会员");
            dic.Add("ZHMJCJ0101", "普通会员");
            dic.Add("ZHMJCJ0102", "白银会员");
            dic.Add("ZHMJCJ0104", "铂金会员");
            dic.Add("ZHMJCJ0105", "钻石会员");
            dic.Add("ZHMJCJ0103", "黄金会员");
            if (!dic.Keys.Contains(key))
            {
                return key;
            }

            return dic[key.ToUpper()];
        }

        #region 会员状态
        /// <summary>
        /// 会员状态
        /// </summary>
        /// <returns></returns>
        public static string GetMemberState(string key)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("E0001", "已激活");
            dic.Add("E0005", "待激活");
            #region 真实的代码值
            //dic.Add("E0002", "注销");
            //dic.Add("E0003", "黑名单");
            //dic.Add("E0004", "兑换冻结");
            //dic.Add("E0006", "账号重复");
            #endregion

            #region 业务需求的代码值
            dic.Add("E0002", "状态异常");
            dic.Add("E0003", "状态异常");
            dic.Add("E0004", "状态异常");
            dic.Add("E0006", "状态异常");
            #endregion

            if (!dic.Keys.Contains(key))
            {
                return key;
            }

            return dic[key.ToUpper()];
        }
        #endregion

        /// <summary>
        /// 得到城市
        /// </summary>
        /// <returns></returns>
        public static TenAPI GetLocal(string lo, string ln)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            string json = NetHelper.HttpRequest("http://api.map.baidu.com/geocoder/v2/?location=" + lo + "," + ln
                + "&output=json&pois=1&ak=MpW2GCV1IVTax54FvODYh8XjO405KFxL", "",
                                   "GET", 2000, Encoding.UTF8, "application/json");

            return js.Deserialize<TenAPI>(json);
        }

    }
}
