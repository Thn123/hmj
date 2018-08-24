using Hmj.Common;
using Hmj.Common.Utils;
using Hmj.DTO;
using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Interface;
using Hmj.WebApi.Models;
using log4net;
using Newtonsoft.Json;
using StructureMap.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using WeChatCRM.Common.Utils;

namespace Hmj.WebApi.Modules
{
    public class CommonModule : BaseModule
    {
        private static ILog _logerror = LogManager.GetLogger("logerror");
        private static ILog _logwarn = LogManager.GetLogger("logwarn");
        private static string error_message = "系统异常，请稍后重试";

        [SetterProperty]
        public IHmjCommonService DefutService { get; set; }

        [SetterProperty]
        public IHmjMemberService _hmjMember { get; set; }

        [SetterProperty]
        public IHmjCommonService _hmjCommonService { get; set; }

        public CommonModule() : base("/User")
        {
            Get["/GetCity"] = x =>
            {
                List<CityResDTO> list = DefutService.GetAdministrativeDivision();
                return ResponseJson(true, "OK", list);
            };

            Post["/SendTmps"] = x =>
            {
                BCJ_TMP_DETAIL request = base.BindObject<BCJ_TMP_DETAIL>();
                try
                {
                    if (string.IsNullOrEmpty(request.Template_Code))
                    {
                        return ResponseJson(false, "模板ID不能是空");
                    }

                    string resmsg = NetHelper.HttpRequest(AppConfig.BeautyChinaWebApp, "", "GET", 2000,
                        Encoding.UTF8, "application/json");

                    TokeRes toke = JsonHelper.DeserializeObject<TokeRes>(resmsg);

                    int str = _hmjMember.SendTmp(request, toke.Access_Token);

                    if (str == -1)
                    {
                        return ResponseJson(false, "没有该模板，请查看模板ID");
                    }

                    return ResponseJson(true, "OK");
                }
                catch (Exception ex)
                {
                    WriteLog("发送模板消息", ex);
                    return ResponseJsonError(false, error_message, ex);
                }
            };


            #region 微信卡券信息

            Get["/CreateStore"] = CreateStore;
            Get["/CreateCoupon"] = CreateCoupon;//创建卡券
            Get["/UpdateCoupon"] = UpdateCoupon;//修好卡券
            Get["/ImportCoupon"] = ImportCoupon;//导入卡券
            Get["/HXCoupon"] = HXCoupon;//核销卡券
            Get["/SettingCouponHXType"] = SettingCouponHXType;//设置券核销方式
            Get["/GetCardDetail"] = GetCardDetail;
            #endregion

            Post["/AddLog"] = AddLog;
        }

        #region 微信卡包
        private dynamic GetTimeSpan(dynamic arg)
        {
            //"2018-05-14 00:00:00"
            string Date= base.GetValue<string>("Date");
            TimeSpan ts = Convert.ToDateTime(Date) - Convert.ToDateTime("1970-01-01 00:00:00");
            int spend_time = Convert.ToInt32(ts.TotalSeconds);
            long begin_timestamp = spend_time;
            return ResponseJson(true, begin_timestamp.ToString());
            //2018 - 05 - 14 00:00:00
            //1526256000

            //2018 - 05 - 25 00:00:00
            //1527206400

            //2018 - 06 - 10 23:59:59
            //1528675199
        }

        #region 微信卡券
        private dynamic CreateStore(dynamic arg)
        {
            string resmsg = NetHelper.HttpRequest(AppConfig.BeautyChinaWebApp, "", "GET", 2000,
                        Encoding.UTF8, "application/json");

            TokeRes toke = JsonHelper.DeserializeObject<TokeRes>(resmsg);
            string token = toke.Access_Token;
            string url = "	http://api.weixin.qq.com/cgi-bin/poi/addpoi?access_token=" + token;
            string json = @"{""business"":{
                            ""base_info"":{
                            ""sid"":""56788392"",
                            ""business_name"":""佰草集"",
                            ""branch_name"":""全国各大专柜"",
                            ""province"":""上海市"",
                            ""city"":""上海市"",
                            ""district"":""虹口区"",
                            ""address"":""上海保定路527号"",
                            ""telephone"":""021-35907000"",
                            ""categories"":[""美容spa""],
                            ""offset_type"":3,
                            ""longitude"":121.50074,
                            ""latitude"":31.25493,
                            ""open_time"":""10:00-23:00""
                            }
                            }
                            }";
            var resMessage = NetHelper.HttpRequest(url, json, "POST", 2000, Encoding.UTF8, "application/json");
            return ResponseJson(true, resMessage);
        }

        private dynamic CreateCoupon(dynamic arg)
        {
            //TimeSpan ts = Convert.ToDateTime("2018-05-15 00:00:00") - Convert.ToDateTime("1970-01-01 00:00:00");
            //int spend_time = Convert.ToInt32(ts.TotalSeconds);
            //long begin_timestamp = spend_time;

            //ts = Convert.ToDateTime("2018-05-23 23:59:59") - Convert.ToDateTime("1970-01-01 00:00:00");
            //spend_time = Convert.ToInt32(ts.TotalSeconds);
            //long end_timestamp = spend_time;//计算出来的时间会往后推8小时

            long begin_timestamp = 1527177600;//2018-05-25 00:00:00
            //long begin_timestamp = 1526313600;//2018-05-15 00:00:00
            long end_timestamp = 1528646399;//2018-06-10 23:59:59
            //long end_timestamp = 1527091199;//2018-05-23 23:59:59
            string resmsg = NetHelper.HttpRequest(AppConfig.BeautyChinaWebApp, "", "GET", 2000,
                        Encoding.UTF8, "application/json");

            TokeRes toke = JsonHelper.DeserializeObject<TokeRes>(resmsg);
            string token = toke.Access_Token;
            string url = "https://api.weixin.qq.com/card/create?access_token=" + token;
            string json = @"{ 
            ""card"": {
              ""card_type"": ""GENERAL_COUPON"",
              ""general_coupon"": {
                  ""base_info"": {
                      ""logo_url"": 
            ""https://mmbiz.qlogo.cn/mmbiz_jpg/c8icWUnxBQmEib9ZUicSFGkBiaRg4cbYxq1p3JygXK3eC0Rfy5HocvYdbOJGQaRJNfCDfFJBib7AsRQeZFqFtNADwBw/0"",
                      ""brand_name"":""佰草集"",
                      ""code_type"":""CODE_TYPE_TEXT"",
                      ""title"": ""5折优惠券"",
                      ""sub_title"": ""新恒美紧肤洁面乳爆款招新"",
                      ""color"": ""Color020"",
                      ""notice"": ""请出示您的唯一优惠券码"",
                      ""description"": ""1）优惠券仅限本人使用，不可转赠，每人限领一张
            2）活动仅限指定产品
            3）该活动仅限在佰草集专柜首次购买的顾客"",
                                                ""date_info"": {
                                          ""type"": ""DATE_TYPE_FIX_TIME_RANGE"",
                                          ""begin_timestamp"": " + begin_timestamp + " ,"
                              + @"""end_timestamp"":  " + end_timestamp + ""
                         + @" },
                        ""sku"": {
                          ""quantity"": 0
                      },
                      ""get_limit"": 1,
                      ""use_custom_code"": true,
                      ""get_custom_code_mode"":""GET_CUSTOM_CODE_MODE_DEPOSIT"",
                      ""bind_openid"": false,
                      ""can_share"": false,
                      ""can_give_friend"": false,
                      ""center_title"": ""立即使用"",
                          ""center_sub_title"": """",
                          ""center_url"": """+AppConfig.HmjWebApp + @"assets/hmjweixin/html/hxcoupon.html""
                  },
               ""advanced_info"": {

                           ""time_limit"": [
                               {
                            ""type"": ""MONDAY"",
                                   ""begin_hour"":0,
                                   ""begin_minute"":0,
                                   ""end_hour"":23,
                                   ""end_minute"":59
                               },
                                 {
                            ""type"": ""TUESDAY"",
                               ""begin_hour"":0,
                                   ""begin_minute"":0,
                                   ""end_hour"":23,
                                   ""end_minute"":59
                               },
                                 {
                            ""type"": ""WEDNESDAY"",
                                  ""begin_hour"":0,
                                   ""begin_minute"":0,
                                   ""end_hour"":23,
                                   ""end_minute"":59
                               },
                                 {
                            ""type"": ""THURSDAY"",
                                   ""begin_hour"":0,
                                   ""begin_minute"":0,
                                   ""end_hour"":23,
                                   ""end_minute"":59
                               },
                                 {
                            ""type"": ""FRIDAY"",
                                ""begin_hour"":0,
                                   ""begin_minute"":0,
                                   ""end_hour"":23,
                                   ""end_minute"":59
                               },
                                 {
                            ""type"": ""SATURDAY"",
                                   ""begin_hour"":0,
                                   ""begin_minute"":0,
                                   ""end_hour"":23,
                                   ""end_minute"":59
                               },
                                 {
                            ""type"": ""SUNDAY"",
                                    ""begin_hour"":0,
                                   ""begin_minute"":0,
                                   ""end_hour"":23,
                                   ""end_minute"":59
                               }
                           ]

                       },
                  ""default_detail"": ""原价160元，现价80元""}
            }
            }";
            ///assets/hmjweixin/html/hxcoupon.html
            // ""center_url"": """+AppConfig.HmjWebApp + @"HmjMember/HXCoupon.do""
            var resMessage = NetHelper.HttpRequest(url, json, "POST", 2000, Encoding.UTF8, "application/json");
            CouponResponse response = JsonConvert.DeserializeObject<CouponResponse>(resMessage);
            //pDRuD1EutQ_b25Qd8c27hJ-rl7Do
            //{"status": 1,    "message": "{\"errcode\":0,\"errmsg\":\"ok\",\"card_id\":\"pDRuD1EutQ_b25Qd8c27hJ-rl7Do\"}",    "data": null}

            if (response.errmsg == "ok")
            {
                string cardId = response.card_id;
                //url = "https://api.weixin.qq.com/card/selfconsumecell/set?access_token=" + token;
                //json = @"{""card_id"":""" + cardId + @""",  ""is_open"": true}";//,""need_verify_cod"":false,""need_remark_amount"":false
                //resMessage = NetHelper.HttpRequest(url, json, "POST", 2000, Encoding.UTF8, "application/json");

                //if (resMessage.Split('"')[5] == "ok")
                //    return ResponseJson(true, "卡券创建成功，自助核销设置成功cardId="+ cardId);
                //else
                //    return ResponseJson(true, "卡券创建成功，自助核销设置失败cardId="+ cardId);
                return ResponseJson(true, "卡券创建成功，cardId=" + cardId);
            }
            else
            {
                return ResponseJson(false, "卡券创建失败");

            }
        }

        private dynamic UpdateCoupon(dynamic arg)
        {
            string resmsg = NetHelper.HttpRequest(AppConfig.BeautyChinaWebApp, "", "GET", 2000,
                        Encoding.UTF8, "application/json");
            string CardId = base.GetValue<string>("CardId");
            TokeRes toke = JsonHelper.DeserializeObject<TokeRes>(resmsg);
            string token = toke.Access_Token;
            string url = "https://api.weixin.qq.com/card/update?access_token=" + token;


            string json = @"{
                ""card_id"":"""+ CardId + @""",
               ""general_coupon"": {
                            ""base_info"": {
                            ""logo_url"": ""https://mmbiz.qlogo.cn/mmbiz_jpg/c8icWUnxBQmEib9ZUicSFGkBiaRg4cbYxq1p3JygXK3eC0Rfy5HocvYdbOJGQaRJNfCDfFJBib7AsRQeZFqFtNADwBw/0"",
                            ""color"": ""Color020"",
                           
                            ""center_title"": ""立即使用"",
                            ""center_sub_title"": """",
                            ""center_url"": """ + AppConfig.HmjWebApp + @"assets/hmjweixin/html/hxcoupon.html"",
                            ""get_limit"":10
                            }
               }
            }";
            var resMessage = NetHelper.HttpRequest(url, json, "POST", 2000, Encoding.UTF8, "application/json");
            CouponResponse response = JsonConvert.DeserializeObject<CouponResponse>(resMessage);

            if (response.errmsg == "ok")
            {

                return ResponseJson(true, "卡券修改成功，cardId=" + CardId);
            }
            else
            {
                return ResponseJson(false, "卡券修改失败");

            }
        }

        private dynamic SettingCouponHXType(dynamic arg)
        {
            string CardId = base.GetValue<string>("CardId");
            //string token = Token(AppConfig.FWHOriginalID);
            string resmsg = NetHelper.HttpRequest(AppConfig.BeautyChinaWebApp, "", "GET", 2000,
                        Encoding.UTF8, "application/json");

            TokeRes toke = JsonHelper.DeserializeObject<TokeRes>(resmsg);
            string token = toke.Access_Token;
            string url = "https://api.weixin.qq.com/card/selfconsumecell/set?access_token=" + token;
          string  json = @"{""card_id"":""" + CardId + @""",  ""is_open"": true}";
           string resMessage = NetHelper.HttpRequest(url, json, "POST", 2000, Encoding.UTF8, "application/json");

            if (resMessage.Split('"')[5] == "ok")
                return ResponseJson(true, "卡券自助核销设置成功");
            else
                return ResponseJson(true, "卡券自助核销设置失败");

        }

      

        public dynamic ImportCoupon(dynamic arg)
        {
            string message = "";
            string url = "";
            string json = "";
            string cardid = base.GetValue<string>("cardid");
            //string resmsg = NetHelper.HttpRequest(AppConfig.BeautyChinaWebApp, "", "GET", 2000,
            //    Encoding.UTF8, "application/json");
            //TokeRes toke = JsonHelper.DeserializeObject<TokeRes>(resmsg);
            //string token = Token(AppConfig.FWHOriginalID);
            string resmsg = NetHelper.HttpRequest(AppConfig.BeautyChinaWebApp, "", "GET", 2000,
                        Encoding.UTF8, "application/json");

            TokeRes toke = JsonHelper.DeserializeObject<TokeRes>(resmsg);
            string token = toke.Access_Token;
            List<WXCouponNoInfo> list = _hmjMember.QueryWXCouponNoInfo();
           
            int succ_cnt = 0;
            string codes = "";
            foreach (WXCouponNoInfo model in list)
            {
                url = "http://api.weixin.qq.com/card/code/deposit?access_token=" + token;
                json = @"{
                                   ""card_id"": """ + cardid + @""",
                                   ""code"": [
                                       """ + model.CouponNo + @"""
                                   ]
                                }";
                var resMessage = NetHelper.HttpRequest(url, json, "POST", 2000, Encoding.UTF8, "application/json");
                if (resMessage.Contains("succ_code")) //成功
                {
                    succ_cnt++;
                    _hmjMember.UpdateWXCouponNoInfoIsImport(model.id);
                }
                else
                {
                    message = resMessage + "<br>ID:" + model.id;
                }
                codes += "\""+model.CouponNo+"\",";
            }
            codes = codes.Substring(0, codes.Length - 1);
            
            if (succ_cnt > 0)
            {
                //修改库存
                url = "https://api.weixin.qq.com/card/modifystock?access_token=" + token;
                json = @"{""card_id"": """ + cardid + @""",""increase_stock_value"":"+ succ_cnt + @",""reduce_stock_value"": 0}";
                var resMessage = NetHelper.HttpRequest(url, json, "POST", 2000, Encoding.UTF8, "application/json");
                //if (resMessage.Contains("ok"))
                //{
                //    if (message != "")
                //    {
                //        message = "{\"meg\":\"" + message + "\",\"succ_cnt\":" + succ_cnt + "}";
                //    }
                //    else
                //    {
                //        message = "{\"meg\":\"ok\",\"succ_cnt\":" + succ_cnt + "}";
                //    }
                //}
            }

            //核查code接口
            url = "http://api.weixin.qq.com/card/code/checkcode?access_token=" + token;
            json = @"{""card_id"": """ + cardid + @""",""code"":[" + codes + @"]}";
            message = NetHelper.HttpRequest(url, json, "POST", 2000, Encoding.UTF8, "application/json");


            return message;
        }

        public dynamic HXCoupon(dynamic arg)
        {
            try
            {
                string code = base.GetValue<string>("code");
                string card_id = base.GetValue<string>("card_id");
                //string token = Token(AppConfig.FWHOriginalID);
                string resmsg = NetHelper.HttpRequest(AppConfig.BeautyChinaWebApp, "", "GET", 2000,
                        Encoding.UTF8, "application/json");

                TokeRes toke = JsonHelper.DeserializeObject<TokeRes>(resmsg);
                string token = toke.Access_Token;
                string url = "https://api.weixin.qq.com/card/code/consume?access_token=" + token;
                string json = @"{
  ""code"": ""{0}"",
  ""card_id"": """ + card_id + @"""}";
                json = json.Replace("{0}", code);
                var resMessage = NetHelper.HttpRequest(url, json, "POST", 2000, Encoding.UTF8, "application/json");
                //{ "errcode":0,"errmsg":"ok","card":{                "card_id":"pFS7Fjg8kV1IdDz01r4SQwMkuCKc"},"openid":"oFS7Fjl0WsZ9AMZqrI80nbIq8xrA"}
                if (resMessage.Split('"')[5] == "ok")
                {
                    _logwarn.Warn("核销成功！code=" + code + " ,card_id=" + card_id);
                    return ResponseJson(true, "ok", "核销成功");
                }
                return ResponseJson(false, "error", "核销失败");
            }
            catch (Exception ex)
            {
                return ResponseJson(false, "error", "核销异常" + ex.Message);
            }

        }

        public dynamic GetCardDetail(dynamic arg)
        {
            try
            {
                string code = base.GetValue<string>("code");
                string card_id = base.GetValue<string>("card_id");
                string resmsg = NetHelper.HttpRequest(AppConfig.BeautyChinaWebApp, "", "GET", 2000,
                         Encoding.UTF8, "application/json");

                TokeRes toke = JsonHelper.DeserializeObject<TokeRes>(resmsg);
                string token = toke.Access_Token;
                //string token = Token(AppConfig.FWHOriginalID);
                cardInfo cardInfo = new cardInfo();
                cardInfo.card_id = card_id;
                string url = "https://api.weixin.qq.com/card/code/decrypt?access_token=" + token;
                string json = @"{""encrypt_code"":""" + code + @"""}";
                string resMessage = NetHelper.HttpRequest(url, json, "POST", 2000, Encoding.UTF8, "application/json");

                //{"errcode":0,  "errmsg":"ok",  "code":"751234212312"}
                if (resMessage.Contains("\"ok\""))
                {
                    code = resMessage.Split('"')[9];
                    cardInfo.code = code;
                    //查询卡券信息
                    url = "https://api.weixin.qq.com/card/get?access_token=" + token;
                    json = @"{
                            ""card_id"": """ + card_id + @"""
                           }";
                    resMessage = NetHelper.HttpRequest(url, json, "POST", 2000, Encoding.UTF8, "application/json");
                    if (resMessage.Contains("\"ok\""))
                    {
                        try
                        {
                            var brand_name = resMessage.Substring(resMessage.IndexOf("brand_name") + 10).Split('"')[2];
                            var title = resMessage.Substring(resMessage.IndexOf("title") + 5).Split('"')[2];
                            var color = resMessage.Substring(resMessage.IndexOf("color") + 5).Split('"')[2];
                            cardInfo.brand_name = brand_name;
                            cardInfo.title = title;
                            cardInfo.color = color;
                        }
                        catch
                        {
                            cardInfo.brand_name = "";
                            cardInfo.title = "";
                            cardInfo.color = "";
                        }
                    }
                }
                string cardInfoMsg = JsonConvert.SerializeObject(cardInfo);
                return ResponseJson(true, "ok", cardInfoMsg);
            }
            catch (Exception ex)
            {
                return ResponseJson(false, "error", "核销异常" + ex.Message);
            }
        }
        #endregion

        #endregion

        public dynamic AddLog(dynamic arg)
        {
            logRequest request = base.BindObject<logRequest>();
            if(request==null)
                return ResponseJson(false, "没有请求数据", null);

            Tb_log log = new Tb_log();
            log.title = request.Title;
            log.msgType = request.MsgType;
            log.msgContent = request.MsgContent;
            log.createDate = DateTime.Now;
            if(_hmjCommonService.InsertLog(log)>0)
                return ResponseJson(true, "ok", null);
            else
                return ResponseJson(false, "添加失败", null);
        }

        private void WriteLog(string funcName, Exception ex)
        {
            _logerror.Error(funcName + "报错\r\n时间" + DateTime.Now.ToString() + "\r\n" + ex.Message + "\r\n" + ex.ToString());
        }
    }
}