using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Entity.WxModel;
using Hmj.Interface;
using Hmj.WebService;
using Newtonsoft.Json;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WeChatCRM.Common.Utils;

namespace Hmj.Common
{
    public class CommonHelp
    {
        /// <summary>
        /// 多线程送积分
        /// </summary>
        /// <param name="fansid"></param>
        /// <param name="openid"></param>
        public static async void SendPoint(int fansid, string openid)
        {
            await SendPiontAsync(fansid, openid);
        }

        /// <summary>
        /// 把类转化成键值对
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        internal static Dictionary<string, string> GetDic<T>(T data)
        {
            Type type = data.GetType();

            PropertyInfo[] pro = type.GetProperties();//获取这个类中的所有公共属性

            //如果没有对象用这种方法获取
            //object cl = Activator.CreateInstance(type);//获取类的对象

            Dictionary<string, string> dic = new Dictionary<string, string>();

            //属性
            foreach (PropertyInfo item in pro)
            {
                string key = item.Name;
                string values = item.GetValue(data, null).ToString();
                dic.Add(key, values);
            }

            return dic;
        }

        /// <summary>
        /// 自己封装耗时的方法
        /// </summary>
        /// <returns></returns>
        static Task SendPiontAsync(int fansid, string openid)
        {
            ICustMemberService api = ObjectFactory.GetInstance<ICustMemberService>();
            return Task.Run(() =>
            {
                string bp = api.GetOldMember(openid);
                if (!string.IsNullOrEmpty(bp))
                {
                    ZCRMT302_Dyn member = api.GetMemberModelByBp(bp);

                    if (member != null)
                    {
                        CUST_MEMBER model = new CUST_MEMBER() { PARTNER = bp, FANS_ID = fansid };

                        api.InserMeber(model);

                        //发送积分
                        //api.SendPiod(member.ACCOUNT_ID, "300");

                        //如果未激活就执行激活操作
                        if (member.ZZAFLD000004 == "E0005")
                        {
                            api.ChageSatus(member.ACCOUNT_ID);
                        }
                        //标记当前粉丝已经送过积分
                        api.UpdateFans(fansid);
                    }
                }
            });
        }

        //private static ILog _logwarn = LogManager.GetLogger("logwarn");
        //调用异步方法
        public static async void SendTmps(List<WX_TMP_HIS> hise, string accToke)
        {
            await SendTmpAsync(hise, accToke);
        }

        /// <summary>
        /// 自己封装耗时的方法
        /// </summary>
        /// <returns></returns>
        static Task SendTmpAsync(List<WX_TMP_HIS> his, string accToke)
        {
            IBcjStoreService api = ObjectFactory.GetInstance<IBcjStoreService>();
            return Task.Run(() =>
            {
                foreach (WX_TMP_HIS item in his)
                {
                    string resot = NetHelper.HttpRequest("https://api.weixin.qq.com/cgi-bin/message/template/send?access_token="
                    + accToke, item.DETAIL, "POST", 2000, Encoding.UTF8, "application/json");

                    if (resot.Contains("ok"))
                    {
                        api.UpdateOk(item.ID);
                    }
                }
            });
        }



        /// <summary>
        /// 得到配置键值对
        /// </summary>
        /// <param name="request"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static Dictionary<string, TemplateData> GetTmpPar(BCJ_TMP_DETAIL request,
            List<WX_TMP_CONFIG> config)
        {
            Dictionary<string, TemplateData> dic = new Dictionary<string, TemplateData>();

            WX_TMP_CONFIG P_1 = config.Where(a => a.P_CODE == nameof(request.P_1)).FirstOrDefault();
            WX_TMP_CONFIG P_2 = config.Where(a => a.P_CODE == nameof(request.P_2)).FirstOrDefault();
            WX_TMP_CONFIG P_3 = config.Where(a => a.P_CODE == nameof(request.P_3)).FirstOrDefault();
            WX_TMP_CONFIG P_4 = config.Where(a => a.P_CODE == nameof(request.P_4)).FirstOrDefault();
            WX_TMP_CONFIG P_5 = config.Where(a => a.P_CODE == nameof(request.P_5)).FirstOrDefault();
            WX_TMP_CONFIG P_6 = config.Where(a => a.P_CODE == nameof(request.P_6)).FirstOrDefault();
            WX_TMP_CONFIG P_7 = config.Where(a => a.P_CODE == nameof(request.P_7)).FirstOrDefault();

            if (request.Template_Code == "ZX7xJb7KkhUf1VMzYvsh6a-tsiGwomkIgXoZCbfKzis")//特殊处理 
            {
                if (P_1 != null)
                {
                    dic.Add(P_1.P_NAME, new TemplateData() { Color = "#000000", Value = "" });
                }

                if (P_2 != null)
                {
                    dic.Add(P_2.P_NAME, new TemplateData() { Color = "#000000", Value = request.P_1 });
                }

                if (P_3 != null)
                {
                    dic.Add(P_3.P_NAME, new TemplateData() { Color = "#000000", Value = request.P_2 });
                }

                if (P_4 != null)
                {
                    dic.Add(P_4.P_NAME, new TemplateData() { Color = "#000000", Value = request.P_3 });
                }

                if (P_5 != null)
                {
                    dic.Add(P_5.P_NAME, new TemplateData() { Color = "#000000", Value = request.P_4 });
                }

                if (P_6 != null)
                {
                    dic.Add(P_6.P_NAME, new TemplateData() { Color = "#000000", Value = request.P_5 });
                }
                if (P_7 != null)
                {
                    dic.Add(P_7.P_NAME, new TemplateData() { Color = "#000000", Value = request.P_6 });
                }
            }
            else {
                if (P_1 != null)
                {
                    dic.Add(P_1.P_NAME, new TemplateData() { Color = "#000000", Value = request.P_1 });
                }

                if (P_2 != null)
                {
                    dic.Add(P_2.P_NAME, new TemplateData() { Color = "#000000", Value = request.P_2 });
                }

                if (P_3 != null)
                {
                    dic.Add(P_3.P_NAME, new TemplateData() { Color = "#000000", Value = request.P_3 });
                }

                if (P_4 != null)
                {
                    dic.Add(P_4.P_NAME, new TemplateData() { Color = "#000000", Value = request.P_4 });
                }

                if (P_5 != null)
                {
                    dic.Add(P_5.P_NAME, new TemplateData() { Color = "#000000", Value = request.P_5 });
                }


                if (P_6 != null)
                {
                    dic.Add(P_6.P_NAME, new TemplateData() { Color = "#000000", Value = request.P_6 });
                }
                if (P_7 != null)
                {
                    dic.Add(P_7.P_NAME, new TemplateData() { Color = "#000000", Value = request.P_7 });
                }
            }
            return dic;
        }

        /// <summary>
        /// 得到配置键值对
        /// </summary>
        /// <param name="request"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static Dictionary<string, TemplateData> GetTmpParEx(Template_Param request,
            List<WX_TMP_CONFIG> config,string Template_Code)
        {
            Dictionary<string, TemplateData> dic = new Dictionary<string, TemplateData>();

            WX_TMP_CONFIG P_1 = config.Where(a => a.P_CODE == nameof(request.P_1)).FirstOrDefault();
            WX_TMP_CONFIG P_2 = config.Where(a => a.P_CODE == nameof(request.P_2)).FirstOrDefault();
            WX_TMP_CONFIG P_3 = config.Where(a => a.P_CODE == nameof(request.P_3)).FirstOrDefault();
            WX_TMP_CONFIG P_4 = config.Where(a => a.P_CODE == nameof(request.P_4)).FirstOrDefault();
            WX_TMP_CONFIG P_5 = config.Where(a => a.P_CODE == nameof(request.P_5)).FirstOrDefault();
            WX_TMP_CONFIG P_6 = config.Where(a => a.P_CODE == nameof(request.P_6)).FirstOrDefault();
            WX_TMP_CONFIG P_7 = config.Where(a => a.P_CODE == nameof(request.P_7)).FirstOrDefault();
            if (Template_Code == "ZX7xJb7KkhUf1VMzYvsh6a-tsiGwomkIgXoZCbfKzis")//特殊处理 
            {
                if (P_1 != null)
                {
                    dic.Add(P_1.P_NAME, new TemplateData() { Color = "#000000", Value = "" });
                }

                if (P_2 != null)
                {
                    dic.Add(P_2.P_NAME, new TemplateData() { Color = "#000000", Value = request.P_1 });
                }

                if (P_3 != null)
                {
                    dic.Add(P_3.P_NAME, new TemplateData() { Color = "#000000", Value = request.P_2 });
                }

                if (P_4 != null)
                {
                    dic.Add(P_4.P_NAME, new TemplateData() { Color = "#000000", Value = request.P_3 });
                }

                if (P_5 != null)
                {
                    dic.Add(P_5.P_NAME, new TemplateData() { Color = "#000000", Value = request.P_4 });
                }

                if (P_6 != null)
                {
                    dic.Add(P_6.P_NAME, new TemplateData() { Color = "#000000", Value = request.P_5 });
                }
                if (P_7 != null)
                {
                    dic.Add(P_7.P_NAME, new TemplateData() { Color = "#000000", Value = request.P_6 });
                }
            }
            else
            {
                if (P_1 != null)
                {
                    dic.Add(P_1.P_NAME, new TemplateData() { Color = "#000000", Value = request.P_1 });
                }

                if (P_2 != null)
                {
                    dic.Add(P_2.P_NAME, new TemplateData() { Color = "#000000", Value = request.P_2 });
                }

                if (P_3 != null)
                {
                    dic.Add(P_3.P_NAME, new TemplateData() { Color = "#000000", Value = request.P_3 });
                }

                if (P_4 != null)
                {
                    dic.Add(P_4.P_NAME, new TemplateData() { Color = "#000000", Value = request.P_4 });
                }

                if (P_5 != null)
                {
                    dic.Add(P_5.P_NAME, new TemplateData() { Color = "#000000", Value = request.P_5 });
                }

                if (P_6 != null)
                {
                    dic.Add(P_6.P_NAME, new TemplateData() { Color = "#000000", Value = request.P_6 });
                }
                if (P_7 != null)
                {
                    dic.Add(P_7.P_NAME, new TemplateData() { Color = "#000000", Value = request.P_7 });
                }
            }
            return dic;
        }

        /// <summary>
        /// 获取随机字符串
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string GetRand(int len)
        {
            Random ran = new Random();
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < len; i++)
            {
                sb.Append(ran.Next(0, 10));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 获取反序列化后的实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="mothd"></param>
        /// <returns></returns>
        public static T GetNetResModel<T>(string url, object data, string mothd)
        {
            string datas = string.Empty;

            if (data.ToString() != "")
            {
                datas = JsonConvert.SerializeObject(data);
            }
            string str = NetHelper.HttpRequest(url, datas, mothd, 6000,
                Encoding.UTF8, "application/json");

            return JsonConvert.DeserializeObject<T>(str);
        }
    }
}
