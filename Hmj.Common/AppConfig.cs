using System;

namespace Hmj.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class AppConfig
    {
        /// <summary>
        /// 是否调试模式
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is debug mode; otherwise, <c>false</c>.
        /// </value>
        public static bool IsDebugMode
        {
            get
            {
                string isdebug = GetAppSetting("IsDebugMode");
                if (string.IsNullOrEmpty(isdebug))
                {
                    return false;
                }
                else
                {
                    return string.Compare(isdebug, "true", true) == 0 || isdebug == "1";
                }
            }
        }

        public static bool ShowTop
        {
            get
            {
                string isShowTop = GetAppSetting("ShowTop");
                if (string.IsNullOrEmpty(isShowTop))
                {
                    return false;
                }
                else
                {
                    return string.Compare(isShowTop, "true", true) == 0 || isShowTop == "1";
                }
            }
        }
        public static string MainDbKey
        {
            get
            {
                return "maindb";
            }
        }

        public static string ThirdPlatformDbKey
        {
            get
            {
                return "thirdplatformdb";
            }
        }

        /// <summary>
        /// 企业号根目录ID
        /// </summary>
        public static int QYDeptRootID
        {
            get
            {
                string value = GetAppSetting("QYDeptRootID");

                return Convert.ToInt32(value);
            }
        }
        /// <summary>
        /// 企业号根目录ID
        /// </summary>
        public static string WXCorpID
        {
            get
            {
                string value = GetAppSetting("WXCorpID");

                return value;
            }
        }

        /// <summary>
        /// 虚拟BA姓名
        /// </summary>
        public static string EMPID
        {
            get
            {
                string value = GetAppSetting("EMPID");

                return value;
            }
        }

        /// <summary>
        /// 创建会员用门店编号
        /// </summary>
        public static string DEPTID
        {
            get
            {
                string value = GetAppSetting("DEPTID");

                return value;
            }
        }

        /// <summary>
        /// 服务号消息地址
        /// </summary>
        public static string QYHMsgUrl
        {
            get
            {
                string value = GetAppSetting("WxGetUrlCs");

                return value;
            }
        }

        /// <summary>
        /// 忠诚度品牌
        /// </summary>
        public static string LOYALTY_BRAND
        {
            get
            {
                string value = GetAppSetting("LOYALTY_BRAND");

                return value;
            }
        }

        /// <summary>
        /// 来源系统
        /// </summary>
        public static string SOURCE_SYSTEM
        {
            get
            {
                string value = GetAppSetting("SOURCE_SYSTEM");

                return value;
            }
        }

        /// <summary>
        /// 销售组织
        /// </summary>
        public static string VGROUP
        {
            get
            {
                string value = GetAppSetting("VGROUP");

                return value;
            }
        }


        /// <summary>
        /// 服务号原始ID
        /// </summary>
        public static string FWHOriginalID
        {
            get
            {
                string value = GetAppSetting("FWHOriginalID");

                return value;
            }
        }

        /// <summary>
        /// 开始营业时间
        /// </summary>
        /// <value>
        /// The start hour.
        /// </value>
        public static int StartHour
        {
            get
            {
                return 10;
            }
        }

        /// <summary>
        /// 结束营业时间
        /// </summary>
        /// <value>
        /// The end hour.
        /// </value>
        public static int EndHour
        {
            get
            {
                return 24;
            }
        }

        public static string Get(string key)
        {
            return GetAppSetting(key);

        }


        /// <summary>
        /// 会员开单地址
        /// </summary>
        /// <value>
        /// The card check in URL.
        /// </value>
        public static string CardCheckInUrl
        {
            get
            {
                return Get("CardCheckInUrl");
            }
        }
        /// <summary>
        /// 临时会员开单地址
        /// </summary>
        /// <value>
        /// The cash check in URL.
        /// </value>
        public static string CashCheckInUrl
        {
            get
            {
                return Get("CashCheckInUrl");
            }
        }
        /// <summary>
        /// 会员详细地址
        /// </summary>
        /// <value>
        /// The user edit URL.
        /// </value>
        public static string UserEditUrl
        {
            get
            {
                return Get("UserEditUrl");
            }
        }
        /// <summary>
        /// 会员卡储值地址
        /// </summary>
        /// <value>
        /// The card recharge URL.
        /// </value>
        public static string CardRechargeUrl
        {
            get
            {
                return Get("CardRechargeUrl");
            }
        }

        public static string CardAddUrl
        {
            get
            {
                return Get("CardAddUrl");
            }
        }
        /// <summary>
        /// 疗程卡开卡地址
        /// </summary>
        /// <value>
        /// The create card URL.
        /// </value>
        public static string CreateCardUrl
        {
            get
            {
                return Get("CreateCardUrl");
            }
        }

        public static string CheckInUrl
        {
            get
            {
                return Get("CheckInUrl");
            }
        }
        public static string ViewBillUrl
        {
            get
            {
                return Get("ViewBillUrl");
            }
        }
        public static string SMSContent
        {
            get
            {
                return Get("SMSContent");
            }
        }

        public static string WXCorpSecret
        {
            get
            {
                return Get("WXCorpSecret");
            }
        }


        #region private
        private static string GetAppSetting(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings.Get(key);
        }
        private static string GetConnectionStrings(string key)
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings[key].ConnectionString;
        }
        #endregion

        //public static string CRMServiceUrl { 
        //    get {
        //        return Get("CRMServiceUrl");
        //    } 
        //}
        public static string POSServiceUrl
        {
            get
            {
                return Get("POSServiceUrl");
            }
        }
        public static string CRMSMSRootUrl
        {
            get
            {
                string url = Get("CRMSMSRootUrl");
                if (string.IsNullOrEmpty(url))
                {
                    url = Get("CRMRootUrl");
                }
                return url;
            }
        }
        public static string CRMRootUrl
        {
            get
            {
                return Get("CRMRootUrl");
            }
        }
        public static string CRMPassword
        {
            get
            {
                return Get("CRMPassword");
            }
        }

        public static string CRMUserId
        {
            get
            {
                return Get("CRMUserId");
            }
        }

        public static string POSPassword
        {
            get
            {
                return Get("POSPassword");
            }
        }

        public static string POSUserId
        {
            get
            {
                return Get("POSUserId");
            }
        }
        public static string POSSource
        {
            get
            {
                return Get("POSSource");
            }
        }

        public static int MaxUploadImageSize
        {
            get
            {
                return 10 * 1024 * 1024;
            }
        }


        /// <summary>
        /// 上传文件路径
        /// </summary>
        public static string UploadTMP
        {
            get
            {
                return Get("UploadTMP");
            }
        }

        /// <summary>
        /// 上传文件路径(微信)
        /// </summary>
        public static string UploadWX
        {
            get
            {
                return Get("UploadWX");
            }
        }

        /// <summary>
        /// 上传文件路径(微信AR)
        /// </summary>
        public static string UploadWXAR
        {
            get
            {
                return Get("UploadWXAR");
            }
        }

        /// <summary>
        /// 临时二维码有效期(微信AR)
        /// </summary>
        public static string ExpireAr
        {
            get
            {
                return Get("ExpireAr");
            }
        }
        public static string ImageUrl
        {
            get
            {
                return Get("ImageUrl");
            }
        }
        //会员消费送积分
        public static string Consume_Points
        {
            get
            {
                return Get("Consume_Points");
            }
        }

        /// <summary>
        /// 用于企业号接口(判断是否是活跃)
        /// </summary>
        public static string WXCorpfwh
        {
            get
            {
                return Get("WXCorpfwh");
            }
        }

        /// <summary>
        /// 当前域名
        /// </summary>
        public static string CurrentUri
        {
            get
            {
                return Get("CurrentUri");
            }
        }

        /// <summary>
        /// 当前token
        /// </summary>
        public static string WXMPToken
        {
            get
            {
                return Get("WXMPToken");
            }
        }

        /// <summary>
        /// 微信封面图URL
        /// </summary>
        public static string WxFUrl
        {
            get
            {
                return Get("WxFUrl");
            }
        }

        /// <summary>
        /// 注册会员送积分
        /// </summary>
        public static string POINTS
        {
            get
            {
                return Get("POINTS");
            }
        }

        /// <summary>
        /// 华美家主地址
        /// </summary>
        public static string BeautyChinaUrl
        {
            get
            {
                return Get("BeautyChinaUrl");
            }
        }


        /// <summary>
        /// 华美家webapp主地址
        /// </summary>
        public static string HmjWebApp
        {
            get
            {
                return Get("HmjWebApp");
            }
        }

        /// <summary>
        /// 渠道来源
        /// </summary>
        public static string CHANNEL_SOURCE
        {
            get
            {
                return Get("CHANNEL_SOURCE");
            }
        }

        /// <summary>
        /// 品牌来源
        /// </summary>
        public static string BRAND_SOURCE
        {
            get
            {
                return Get("BRAND_SOURCE");
            }
        }

        /// <summary>
        /// 数据来源
        /// </summary>
        public static string DATA_SOURCE
        {
            get
            {
                return Get("DATA_SOURCE");
            }
        }

        /// <summary>
        /// 数据来源
        /// </summary>
        public static string BeautyChinaWebApp
        {
            get
            {
                return Get("BeautyChinaWebApp");
            }
        }

        /// <summary>
        /// 完善资料送积分
        /// </summary>
        public static string MemberSend
        {
            get
            {
                return Get("MemberSend");
            }
        }

        /// <summary>
        /// 绑定品牌送积分
        /// </summary>
        public static string BindBrandSend
        {
            get
            {
                return Get("BindBrandSend");
            }
        }

        /// <summary>
        /// 绑定老会员送积分
        /// </summary>
        public static string BindMemberSend
        {
            get
            {
                return Get("BindMemberSend");
            }
        }

        /// <summary>
        /// 华美家会员绑定时，新客注册赠送券新客的注册日期限制条件，比如只有在2018/05/15以后注册的会员绑定时才可以送券
        /// </summary>
        public static string HMJRegisterDate
        {
            get
            {
                string value = GetAppSetting("HMJRegisterDate");

                return value;
            }
        }

        /// <summary>
        /// 华美家招新应急赠送卡券的卡券id
        /// </summary>
        public static string CardId
        {
            get
            {
                string value = GetAppSetting("CardId");

                return value;
            }
        }


        /// <summary>
        /// 华美家招新应急活动开始日期
        /// </summary>
        public static string RecruitNewFansStartDate
        {
            get
            {
                string value = GetAppSetting("RecruitNewFansStartDate");

                return value;
            }
        }

        /// <summary>
        /// 华美家招新应急活动结束日期
        /// </summary>
        public static string RecruitNewFansEndDate
        {
            get
            {
                string value = GetAppSetting("RecruitNewFansEndDate");

                return value;
            }
        }

        public static string ThdPlatformWechatServiceBrandId
        {
            get
            {
                return GetAppSetting("ThdPlatformWechatServiceBrandId");
            }
        }

        public static string ThdPlatformSmallProgramBrandId
        {
            get
            {
                return GetAppSetting("ThdPlatformSmallProgramBrandId"); 
            }
        }

        //小程序AppId
        public static string MiniProgramAppId
        {
            get
            {
                return GetAppSetting("MiniProgramAppId");
            }
        }

        //小程序Secret
        public static string MiniProgramSecret
        {
            get
            {
                return GetAppSetting("MiniProgramSecret");
            }
        }
        
        //生成二维码路径
        public static string QrCodePath
        {
            get
            {
                return GetAppSetting("QrCodePath");
            }
        }

        /// <summary>
        /// HmjWebApi https
        /// </summary>
        public static string HmjWebApi_https
        {
            get
            {
                return Get("HmjWebApi_https");
            }
        }

        /// <summary>
        /// WebUrl
        /// </summary>
        public static string WebUrl
        {
            get
            {
                return Get("WebUrl");
            }
        }

        /// <summary>
        /// 卡券活动数据库链接
        /// </summary>
        public static string CouponDbKey
        {
            get
            {
                return "coupondb";
            }
        }

        public static int IsOpenTimeTask
        {
            get
            {
                string key = System.Configuration.ConfigurationManager.AppSettings.Get("IsOpenTimeTask");
                if (string.IsNullOrEmpty(key))
                    return 0;
                else
                {
                    try
                    {
                        return int.Parse(key);
                    }
                    catch
                    {
                        return 0;
                    }
                }
            }
        }
    }
}
