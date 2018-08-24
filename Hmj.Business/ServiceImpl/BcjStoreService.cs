using Hmj.Common;
using Hmj.DataAccess.Repository;
using Hmj.Entity;
using Hmj.Entity.apiEntity;
using Hmj.Entity.Entities;
using Hmj.Entity.PageSearch;
using Hmj.Entity.SearchEntity;
using Hmj.Entity.WxModel;
using Hmj.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using WeChatCRM.Common.Utils;

namespace Hmj.Business.ServiceImpl
{
    public class BcjStoreService : IBcjStoreService
    {
        BcjStoreRepository _bcjstor;
        ILoginService CLoginService;
        public BcjStoreService()
        {
            _bcjstor = new BcjStoreRepository();
            CLoginService = new LoginService();
        }

        /// <summary>
        /// 判断门店是否存在
        /// </summary>
        /// <param name="storeCode"></param>
        /// <returns></returns>
        public int ChckStore(string storeCode)
        {
            BCJ_STORES store = _bcjstor.GetStoreByCode(storeCode.ToUpper());

            if (store == null)
            {
                return 0;
            }

            return 1;
        }

        /// <summary>
        /// 得到佰草集门店详细信息
        /// </summary>
        /// <param name="storeCode"></param>
        /// <returns></returns>
        public BCJ_STORES_EX GetStoreEntity(string storeCode)
        {
            return _bcjstor.GetStoreEntity(storeCode);
        }

        /// <summary>
        /// 得到佰草集门店
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public List<BCJ_STORES_EX> GetStoresByCityCode(string code)
        {
            return _bcjstor.GetStoresByCityCode(code);
        }

        /// <summary>
        /// 插入发送日志
        /// </summary>
        /// <param name="template_Id"></param>
        /// <param name="url"></param>
        /// <param name="touser"></param>
        /// <param name="tmpStr"></param>
        /// <returns></returns>
        public int InsertLog(WX_TMP_HIS his)
        {
            return (int)_bcjstor.Insert(his);
        }

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public int SendTmp(BCJ_TMP_DETAIL request, string access_token)
        {
            //得到配置文件
            List<WX_TMP_CONFIG> config = _bcjstor.GetTmps(request.Template_Code);
            

            if (config == null || config.Count <= 0)
            {
                return -1;
            }

            Dictionary<string, TemplateData> dic = CommonHelp.GetTmpPar(request, config);

            TemplateSend tmp = new TemplateSend()
            {
                Url = "",
                Template_Id = request.Template_Code,
                Touser = request.Contact_Information,
                Data = dic
            };

            string tmpStr = JsonConvert.SerializeObject(tmp);

            bool issend = false;
            bool isselect = false;

            //如果是实时接口就调用发送模板
            if (request.IsRealTime)
            {
                isselect = true;
                string resot = NetHelper.HttpRequest("https://api.weixin.qq.com/cgi-bin/message/template/send?access_token="
                    + access_token, tmpStr, "POST", 2000, Encoding.UTF8, "application/json");

                //如果发送成功
                if (resot.Contains("ok"))
                {
                    issend = true;
                }
            }

            //记录发送的
            WX_TMP_HIS log = new WX_TMP_HIS()
            {
                DETAIL = tmpStr,
                OPENID = request.Contact_Information,
                CAMPAIGN_CODE = request.Campaign_Code,
                DATA_SOURCE = request.Data_source,
                CAMPAIGN_NAME = request.Campaign_Name,
                INVOKE_TIME = request.Invoke_Time,
                ISREAL_TIME = request.IsRealTime,
                IS_SEND = issend,
                LOYALTY_BRAND = request.Loyalty_Brand,
                SEND_TIME = request.Send_Time,
                VGROUP = request.Vgroup,
                SOURCE_SYSTEM = request.Data_source,
                TMP_ID = request.Template_Code,
                IS_SELECT = isselect
            };

            long count = _bcjstor.Insert(log);

            return 1;
        }

        /// <summary>
        /// 登录门店
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="storecode"></param>
        /// <returns></returns>
        public long StoreLogin(string mobile, string storecode, string pwd)
        {
            BCJ_STORES store = _bcjstor.GetStoreByCode(storecode.ToUpper(), pwd);

            //没有该门店
            if (store == null)
            {
                return -2;
            }

            //记录登录日志
            BCJ_STORE_LOG log = new BCJ_STORE_LOG()
            {
                CREATE_TIME = DateTime.Now,
                MOBILE = mobile,
                STORE_CODE = storecode.ToUpper()
            };

            return _bcjstor.Insert(log);
        }

        /// <summary>
        /// 开始发送接口
        /// </summary>
        public void StartSendTmp()
        {
            while (true)
            {
                List<WX_TMP_HIS> wxTmp = _bcjstor.GetTmpHis();

                int mylen = wxTmp.Count;
                
                int len = (int)Math.Ceiling(mylen * 1.0m / 1000);

                for (int i = 0; i < len; i++)
                {
                    List<WX_TMP_HIS> arry = wxTmp.GroupBy(a => a.ID).Take(1000).Skip(i * 1000).ToList()[0].ToList();
                    
                    string acctoke = MyToken(AppConfig.FWHOriginalID);
                    CommonHelp.SendTmps(arry, acctoke);
                }
            }
        }

        public string MyToken(string tusers)
        {
            var cuo = CLoginService.GetMerchantsBy(tusers);
            //Session["CurrentMerchants"] = cuo;
            string Access_token = "";
            if (cuo.Access_token != "")
            {
                Access_token = cuo.Access_token;
                string url = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + Access_token
                    + "&openid=" + (cuo.OneOpenID == null ? "oAWd50qK6bOaprTwyLAn4hvLWDdM" : cuo.OneOpenID);
                string b = PostRequest(url);
                if (b.Contains("errcode"))  //返回错误信息
                {
                    Access_token = GetAccess(Access_token, cuo);
                    cuo.Access_token = Access_token;
                    SaveMerchants(cuo);
                }
                if (cuo.OneOpenID == "" || cuo.OneOpenID == null)
                {
                    ISystemService sbo = new SystemService();
                    WXCUST_FANS fans = sbo.GetOneFans(cuo.ToUserName);
                    if (fans != null)
                    {
                        cuo.OneOpenID = fans.FROMUSERNAME;
                        SaveMerchants(cuo);
                    }
                }
            }
            return Access_token;
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

        private string GetAccess(string Access_token, ORG_INFO info)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + info.AppID + "&secret=" +
                info.Appsecret;
            try
            {
                string token = PostRequest(url);
                if (token.Contains("7200"))
                {
                    string[] b = token.Split('\"');
                    Access_token = b[3];
                }
            }
            catch (Exception)
            {
                Access_token = "";
            }
            return Access_token;
        }

        public void SaveMerchants(ORG_INFO m)
        {
            CLoginService.UpdateMerchants(m);
            //HttpContext.Current.Session["CurrentMerchants"] = m;
        }

        /// <summary>
        /// 改变状态
        /// </summary>
        /// <param name="iD"></param>
        public void UpdateOk(int iD)
        {
            _bcjstor.UpdateOk(iD);
        }

        /// <summary>
        /// 發送
        /// </summary>
        /// <param name="fansid"></param>
        /// <param name="openid"></param>
        public void GoToSendPoint(int fansid, string openid)
        {
            CommonHelp.SendPoint(fansid, openid);
        }

        /// <summary>
        /// 报错信息
        /// </summary>
        /// <param name="search"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public PagedList<STORES_EXCEL> QueryExcels(ExcelSearch search, PageView view)
        {
            return _bcjstor.QueryExcels(search, view);
        }

        public bool QueryBcjStores()
        {
            ApiGeocoderService _service=new ApiGeocoderService ();
            List<BCJ_STORES>  stores=_bcjstor.QueryBcjStores();
            foreach (BCJ_STORES store in stores)
            {
                GeocoderResponse response= _service.GetGeocoder(store.ADDRESS);
                store.LATITUDE=response.result.location.lat.ToString();
                store.LONGITUDE=response.result.location.lng.ToString();
                _bcjstor.Update(store);
            }
            return true;
        }
    }
}
