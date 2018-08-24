using Hmj.Business;
using Hmj.Common;
using Hmj.Common.Utils;
using Hmj.DTO;
using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Interface;
using Hmj.Interface.Service;
using Hmj.WebApi.Models;
using Hmj.WebService;
using log4net;
using StructureMap.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Hmj.WebApi.Modules
{
    public class MemberCouponModule : BaseModule
    {
        private static ILog _logerror = LogManager.GetLogger("logerror");
        [SetterProperty]
        public ICustMemberService _custMember { get; set; }
        [SetterProperty]
        public IThdPlatformService _thdplatform { get; set; }
        public MemberCouponModule()
           : base("/MemberCoupon")
        {
            //判断客人是否为华美家会员
            //Get["/GetMemberInfo"] = GetMemberInfo;
            //获取客人券信息
            Get["/GetMemberCoupon"] = GetMemberCoupon;
            Get["/GetAccountID"] = GetAccountID;
        }

        #region 判断客人是否为华美家会员
        /// <summary>
        /// 判断客人是否为华美家会员
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private dynamic GetMemberInfo(dynamic arg)
        {
            string openid = base.GetValue<string>("openid");//openid 为小程序的openid
            //通过第三方开发平台，获取微信公众号下面客人的openid
            ThdPlatformCustInfo ThdPlatformCustInfo = _thdplatform.GetThdPlatformCustInfo(openid, AppConfig.ThdPlatformWechatServiceBrandId);
            if (ThdPlatformCustInfo != null)
            {
                //根据openid 查询cust_member表，如果有记录就返回account_id,/* 没有记录就调用家化接口返回account_id */
                CUST_MEMBER member = _custMember.GetMemberByOpenId(ThdPlatformCustInfo.OpenId);
                if (member == null)
                {
                    return ResponseJson(false, "没有绑定微信，无法查询卡券信息");
                }
                return ResponseJson(true, "OK", member.MEMBERNO);
            }
            else
                return ResponseJson(false, "第三方开发平台找不到小程序的openid");

        }
        #endregion

        #region 获取客人券信息
        /// <summary>
        /// 获取客人券信息
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private dynamic GetMemberCoupon(dynamic arg)
        {
            try
            {
                string ACCOUNT_ID = base.GetValue<string>("ACCOUNT_ID");
                MEMBER_COUPON coupon = GetCoupon(ACCOUNT_ID);
                return ResponseJson(true, "OK", coupon);
            }
            catch (Exception ex)
            {
                _logerror.Error("获取客人券信息:报错\r\n时间" + DateTime.Now.ToString() + "\r\n" + ex.Message + "\r\n" + ex.ToString());
                return ResponseJson(false, ex.Message, null);
            }
        }
        #endregion

        private dynamic GetAccountID(dynamic arg)
        {
            string minikey = base.GetValue<string>("minikey");
            string account_id = "";

            return ResponseJson(true, "ok", account_id);
        }

        /// <summary>
        /// 获取优惠券
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public MEMBER_COUPON GetCoupon(string account_id)
        {
            MEMBER_COUPON coupon = new MEMBER_COUPON();
            dt_Dyn_PosCpSearch_req reqs = new dt_Dyn_PosCpSearch_req();
            reqs.ACCOUNT_ID = account_id;//会员账号和MOB_NUMBER至少输入一个
            reqs.DATA_SOURCE = "0000";//0002为 佰草集

            reqs.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
            reqs.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
            reqs.VGROUP = AppConfig.VGROUP; //销售组织

            dt_Dyn_PosCpSearch_res resok = WebApiHelp.PosCpSearch(reqs);
            //已使用
            List<COUPONS> shiyong = new List<COUPONS>();

            //未使用
            List<COUPONS> meiyou = new List<COUPONS>();

            //已过期
            List<COUPONS> guoqi = new List<COUPONS>();
            if (resok != null)
            {

                if (resok.ZTYPE != "Y")
                {
                    return null;
                }

               

                if (resok.ZPARTERN_CP2.Count() > 0)
                {
                    foreach (ZPARTERN_CP2 item in resok.ZPARTERN_CP2)
                    {
                        //查询另外一个集合的数据
                        dt_Dyn_PosCpSearch_resITEM resout = resok.ZCPQ_RESULT.Where(a => a.ZCP_NUM == item.ZCP_NUM).
                            ToList().FirstOrDefault();


                        string QrCode = resout.ZCP_PASSW + ".jpg";

                        GenerateCode(resout.ZCP_PASSW);


                        //RULE = {"type":"DJQ","value":"100"}  这个就解析成100元代金券 
                        //RULE = {"type":"ZKQ","value":"5"} 解析成 5者折扣券 
                        string RULE = "";
                        if (!string.IsNullOrWhiteSpace(resout.RULE))
                        {
                            CouponRule CouponRule = JsonHelper.DeserializeObject<CouponRule>(resout.RULE);
                            if (CouponRule != null)
                            {
                                if (CouponRule.type == "DJQ")
                                {
                                    RULE = CouponRule.value;
                                }
                                else if (CouponRule.type == "ZKQ")
                                {
                                    RULE = CouponRule.value;
                                }
                            }
                        }
                        COUPONS one = new COUPONS()
                        {
                            BPEXT = item.BPEXT,
                            ZCP_EDATE = resout.ZCP_EDATE,
                            ZCP_EDATE_EX = resout.ZCP_EDATE,
                            ZCP_JE = resout.ZCP_JE,
                            ZCP_POINT = resout.ZCP_POINT,
                            ZCP_PROD = resout.ZCP_PROD,
                            ZCP_TYPE = item.ZCP_TYPE,
                            ZCP_YHQDES = resout.ZCP_YHQDES,
                            ZCP_ZK = resout.ZCP_ZK,
                            ZCP_NUM = resout.ZCP_NUM,
                            ZCP_YHQ = resout.ZCP_YHQ,
                            ZCP_BDATE = resout.ZCP_BDATE,
                            ZCP_PASSW = resout.ZCP_PASSW,
                            IS_BOOK = 0,
                            QrCode = AppConfig.HmjWebApi_https + "/QrCode/" + QrCode,
                            CONTENT = resout.CONTENT,
                            RULE = RULE,
                            ZCP_USE_FLAG = item.ZCP_USE_FLAG
                        };

                        one.ZCPUDATE = item.ZCPUDATE;


                        //已使用
                        if (item.ZCP_USE_FLAG == "X")
                        {
                            shiyong.Add(one);
                        }

                        //未使用
                        if (string.IsNullOrEmpty(item.ZCP_USE_FLAG) || item.ZCP_USE_FLAG == "N") //item.ZCP_USE_FLAG="N" 未使用未激活
                        {
                            DateTime endTime = one.ZCP_EDATE;
                            if (endTime >= DateTime.Parse(DateTime.Now.ToShortDateString()))
                            {
                                //BCJ_BOOK_EX book = _book.GetBookByNo(one.ZCP_NUM);

                                //if (book != null)
                                //{
                                //    if (book.STATUS == 1)
                                //    {
                                //        shiyong.Add(one);
                                //    }
                                //    else if (book.STATUS == 0)
                                //    {
                                //        one.IS_BOOK = 1;
                                //        meiyou.Add(one);
                                //    }
                                //}
                                //else
                                //{
                                //    meiyou.Add(one);
                                //}
                                meiyou.Add(one);

                            }
                            else
                            {
                                guoqi.Add(one);
                            }
                        }
                    }
                }
            }
            coupon.ALREADY_USE = shiyong.OrderByDescending(a => a.ZCPUDATE).ToList();
            coupon.NOT_USE = meiyou.OrderBy(a => a.ZCP_EDATE_EX).ToList();
            coupon.OBSOLETE = guoqi.OrderByDescending(a => a.ZCP_EDATE_EX).ToList();
            return coupon;
        }


        private bool GenerateCode(string CouponNo)
        {
            try
            {
                string img = AppConfig.QrCodePath;
                if (!System.IO.Directory.Exists(img))
                {
                    Directory.CreateDirectory(img);
                }
                if (File.Exists(img + CouponNo + ".jpg"))
                {
                    return true;//二维码生成过
                }
                else
                {
                    bool res = Hmj.WebApi.Models.Common.GetOauthCode(CouponNo, img);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}