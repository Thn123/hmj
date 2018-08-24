using Hmj.Entity;
using Hmj.Interface;
using Hmj.WebApi.Models;
using Newtonsoft.Json;
using StructureMap.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using WeChatCRM.Common.Utils;
using Hmj.Entity.Entities;
using Hmj.Common;
using Newtonsoft.Json.Linq;

namespace Hmj.WebApi.Modules
{
    //小程序
    public class MiniProgramModule : BaseModule
    {
        [SetterProperty]
        public ICustMemberService _custMember { get; set; }
        [SetterProperty]
        public IMiniProgramService _miniprogram { get; set; }
        [SetterProperty]
        public IThdPlatformService _thdplatform { get; set; }
        public MiniProgramModule()
        : base("/MiniProgram")
        {
            //获取用户openid ，sessionkey,account_id
            Get["/sys_login"] = sys_login;
        }

        private dynamic sys_login(dynamic arg)
        {
            try
            {
                string code = base.GetValue<string>("code");
                string appid = AppConfig.MiniProgramAppId;// "wx7f0a5d01dce079bf";//小程序id
                string secret = AppConfig.MiniProgramSecret;// "5147ec0051af368bb64e98fa99a28a7e";//小程序secret
                string url = string.Format("https://api.weixin.qq.com/sns/jscode2session?appid={0}&secret={1}&js_code={2}&grant_type=authorization_code", appid, secret, code);

                var resMessage = NetHelper.HttpRequest(url, "", "GET", 5000, Encoding.UTF8, "application/json");

                if (resMessage.Contains("errcode"))
                {
                    return ResponseJson(false, "登录失败", null);
                }
                else
                {
                    string openid = "";
                    string session_key = "";
                    string unionid = "";
                    string account_id = "";

                    miniKeyResponse response = JsonConvert.DeserializeObject<miniKeyResponse>(resMessage);
                    openid = response.openid;
                    session_key = response.session_key;
                    unionid = response.unionid;
                    string minikey = openid + session_key + unionid;

                    if (!string.IsNullOrWhiteSpace(unionid))//unionid
                    {
                        ThdPlatformCustInfo ThdPlatformCustInfo = _thdplatform.GetThdPlatformCustInfoByUnionid(unionid, AppConfig.ThdPlatformWechatServiceBrandId);
                        if (ThdPlatformCustInfo != null)
                        {
                            //根据openid 查询cust_member表，如果有记录就返回account_id,/* 没有记录就调用家化接口返回account_id */
                            CUST_MEMBER member = _custMember.GetMemberByOpenId(ThdPlatformCustInfo.OpenId);
                            if (member != null)
                            {
                                account_id = member.MEMBERNO;
                            }
                        }
                    }
                    else
                    {
                        return ResponseJson(false, "登录失败", "请取消关注公众号并重新关注");
                        //if (!string.IsNullOrWhiteSpace(openid))//openid:小程序openid
                        //{
                        //    ThdPlatformCustInfo ThdPlatformCustInfo = _thdplatform.GetThdPlatformCustInfo(openid, AppConfig.ThdPlatformWechatServiceBrandId);
                        //    if (ThdPlatformCustInfo != null)
                        //    {
                        //        //根据openid 查询cust_member表，如果有记录就返回account_id,/* 没有记录就调用家化接口返回account_id */
                        //        CUST_MEMBER member = _custMember.GetMemberByOpenId(ThdPlatformCustInfo.OpenId);
                        //        if (member != null)
                        //        {
                        //            account_id = member.MEMBERNO;
                        //        }
                        //    }
                        //}
                    }

                    Mini_sessionkey msk = new Mini_sessionkey();
                    msk.minikey = minikey;
                    msk.openid = openid;
                    msk.session_key = session_key;
                    msk.unionid = unionid;
                    msk.account_id = account_id;
                    msk.create_date = DateTime.Now;
                    long ret = _miniprogram.insertMiniKey(msk);
                    if (ret > 0)
                    {
                        miniCustResponse miniCustResponse = new miniCustResponse();
                        miniCustResponse.minikey = minikey;
                        miniCustResponse.account_id = account_id;
                        return ResponseJson(true, "登录成功", miniCustResponse);
                    }
                    else
                    {
                        return ResponseJson(false, "登录失败", "数据保存失败");
                    }
                }
            }
            catch (Exception ex)
            {
                return ResponseJson(false, "失败了", ex.Message);
            }
        }
    }
}