using Hmj.Business;
using Hmj.Common;
using Hmj.DTO;
using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Interface;
using HmjNew.Service;
using log4net;
using Newtonsoft.Json;
using StructureMap.Attributes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Hmj.WebApi.Modules
{
    public class MemberModule : BaseModule
    {

        private static ILog _logerror = LogManager.GetLogger("logerror");
        private static ILog _logdebug = LogManager.GetLogger("logdebug");
        private static string error_message = "系统异常，请稍后重试";


        [SetterProperty]
        public IHmjMemberService _hmjMember { get; set; }

        [SetterProperty]
        public ICustMemberService _custMember { get; set; }

        public MemberModule()
            : base("/Member")
        {
            //注册
            Post["/RegisterMember"] = RegisterMember;
            //绑定
            Post["/Binding"] = Binding;//此情况是 客户是华美家crm，但不是我们微信本地会员
            //获取会员详细信息
            Post["/GetMemberDetail"] = GetMemberDetail;
            //判断本地粉丝表和会员表是否关联
            Get["/ChckBind"] = ChckBind;
            //更新本地头像
            Post["/UploadImage"] = UploadImage;
            //获取会员更多信息
            Get["/GetMemberExtendMsg"] = GetMemberExtendMsg;
            //更新会员扩展信息
            Post["/UpdateMemberExtendMsg"] = UpdateMemberExtendMsg;
            //得到会员基本信息
            Get["/GetCrmMember"] = GetCrmMember;
            //更新会员基本信息
            Post["/UpdateMember"] = UpdateMember;
            //品牌会员绑定华美家会员
            Get["/BindingRelShip"] = BindingRelShip;
            //品牌积分查询
            Get["/QueryMemberShipBinding"] = QueryMemberShipBinding;
            //发送短信
            Get["/SendMsg"] = SendMsg;
            //获取会员积分明细
            Get["/GetPointDetail"] = GetPointDetail;

            //通过手机号进行品牌积分查询
            Get["/QueryBindingByMobile"] = QueryBindingByMobile;

            //修改会员兑礼密码（手机号未使用）
            Post["/UpdateChangePwd"] = UpdateChangePwd;

            Get["/CanGetCoupon"] = CanGetCoupon;
            Get["/IsNewCustForBCJ"] = IsNewCustForBCJ;

            //通过openid获取到用户信息 手机号，姓名
            Get["/GetUserInfoByOpenid"] = GetUserInfoByOpenid;
        }

        #region 得到积分详情
        /// <summary>
        /// 得到积分详情
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private dynamic GetPointDetail(dynamic arg)
        {
            try
            {
                //得到请求参数
                string OPENOD = base.GetValue<string>("OPENID");

                string mobile = _hmjMember.GetMemberMobileByOpenId(OPENOD);

                if (string.IsNullOrEmpty(mobile))
                {
                    return ResponseJson(false, "对不起没有该会员");
                }

                dt_Dyn_GetPointDetail_req req = new dt_Dyn_GetPointDetail_req();
                req.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
                req.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
                req.VGROUP = AppConfig.VGROUP; //销售组织

                req.DATA_SOURCE = AppConfig.DATA_SOURCE;

                //得到一个年之内的历史记录
                req.ZSTART_DATE = DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd");
                req.ZEND_DATE = DateTime.Now.ToString("yyyy-MM-dd");
                //req.POINT_TYPE = "ZHMJF01";
                req.ACCOUNT_ID = mobile;

                ZCRMT402_Dyn[] lists = WebHmjApiHelp.GetPointDetail(req).ZCRMT402;

                List<PointDetailResDTO> list = new List<PointDetailResDTO>();

                foreach (ZCRMT402_Dyn model in lists)
                {
                    list.Add(model.MapTo<PointDetailResDTO>());
                }

                return ResponseJson(true, "成功", list);
            }
            catch (Exception ex)
            {
                WriteLog("得到品牌积分", ex);
                return ResponseJsonError(false, error_message, ex);
            }
        }
        #endregion

        #region 发送短息
        /// <summary>
        /// 发送短息
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private dynamic SendMsg(dynamic arg)
        {
            try
            {
                //得到请求参数
                string mobile = base.GetValue<string>("MOBILE");

                if (string.IsNullOrEmpty(mobile))
                {
                    return ResponseJson(false, "手机号不能是空");
                }

                Random r = new Random();
                int num = r.Next(100000, 999999);
                //string message = string.Format("本次微信平台获取的验证码是：" + num);
                //【上海家化】您的验证码为：620384。
                //此验证码10分钟内有效，如非本人操作，请联系Jahwa华美家微信后台。
                string message = string.Format("您的验证码为："
                    + num + "此验证码10分钟内有效，如非本人操作，请联系上海家化华美家微信后台。");

                dt_SMSInsert_req req = new dt_SMSInsert_req
                {
                    SMS_ITEM = new SMS_ITEM[] { new SMS_ITEM() {
                        CONTENT = message,
                        MESSAGEID = "0000001",
                        MESSAGETYPE = "BC_WX_SMS",
                        MOBILE = mobile,
                        MSGFORMAT = "8",
                        SRCNUMBER = "1069048560003"
                        }
                    }
                };

                dt_SMSInsert_res res = WebHmjApiHelp.SMSInsert(req, true);

                if (res.zstatus == "1")
                {
                    return ResponseJson(true, "发送成功", num);
                }

                return ResponseJson(false, "发送失败");
            }
            catch (Exception ex)
            {
                WriteLog("发送验证码", ex);
                return ResponseJsonError(false, error_message, ex);
            }
        }
        #endregion

        #region 注册会员
        /// <summary>
        /// 注册会员
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private dynamic RegisterMember(dynamic arg)
        {
            try
            {
                //得到请求参数
                MemberRegisterReqDTO request = base.BindObject<MemberRegisterReqDTO>();

                if (string.IsNullOrEmpty(request.NAME_LAST))
                {
                    return ResponseJson(false, "请填写您的姓名。");
                }

                if (string.IsNullOrEmpty(request.GENDER))
                {
                    return ResponseJson(false, "请填写您的性别。");
                }

                if (string.IsNullOrEmpty(request.NAME_FIRST))
                {
                    return ResponseJson(false, "请填写您的姓名。");
                }

                if (string.IsNullOrEmpty(request.BIRTHDAY))
                {
                    return ResponseJson(false, "请填写您的生日。");
                }

                if (DateTime.Now < DateTime.Parse(request.BIRTHDAY))
                {
                    return ResponseJson(false, "对不起，生日不要超过当前时间");
                }

                string msg = string.Empty;

                //注册
                string flg = _hmjMember.RegisterMember(request, ref msg);

                //注册成功
                if (flg == "1")
                {
                    QueryMemberShipBindingResDTO DTO = _hmjMember.QueryMemberShipBinding(request.OPENID);
                    int ok = 1;
                    int isbind = 1;
                    bool isBcjMember = false;

                    if (DTO.BRAND_LIST == null || DTO.BRAND_LIST.Count() <= 0)
                    {
                        ok = 0;
                        //新会员，且无品牌，则激活
                        _hmjMember.ChageSatus(request.MOBILE);
                    }
                    else
                    {
                        foreach (QueryMemberBindDetailResDTO model in DTO.BRAND_LIST)
                        {
                            if (model.IF_BINDING == "0")
                            {
                                isbind = 0;
                            }
                            ////判断华美家下面是否有佰草集品牌
                            //if (model.LOYALTY_BRAND2 == "28")
                            //    isBcjMember = true;
                        }
                    }

                    #region 招新应急
                    //DateTime StartDate = DateTime.MinValue;
                    //DateTime EndDate = DateTime.MinValue;
                    //if (!string.IsNullOrWhiteSpace(AppConfig.RecruitNewFansStartDate))
                    //{
                    //    try
                    //    {
                    //        StartDate = DateTime.Parse(AppConfig.RecruitNewFansStartDate + " 00:00:00");
                    //    }
                    //    catch { }
                    //}

                    //if (!string.IsNullOrWhiteSpace(AppConfig.RecruitNewFansEndDate))
                    //{
                    //    try
                    //    {
                    //        EndDate = DateTime.Parse(AppConfig.RecruitNewFansEndDate + " 23:59:59");
                    //    }
                    //    catch { }
                    //}

                    //if (StartDate <= DateTime.Now && EndDate >= DateTime.Now)
                    //{
                    //    if (!isBcjMember)
                    //    {
                    //        //品牌积分查询
                    //        //QueryMemberShipBindingResDTO result =  _hmjMember.QueryMemberShipBinding(request.OPENID);
                    //        if (DTO != null && DTO.BRAND_LIST.Count > 0)
                    //        {
                    //            for (int i = 0; i < DTO.BRAND_LIST.Count; i++)
                    //            {
                    //                //是否要判断已经绑定？？？
                    //                if (DTO.BRAND_LIST[i].DATA_SOURCE2 == "0001" || DTO.BRAND_LIST[i].DATA_SOURCE2 == "0002")
                    //                {
                    //                    isBcjMember = true;
                    //                    _logdebug.Debug("招新应急-注册-" + request.OPENID + "：为佰草集老客");
                    //                    break;
                    //                }
                    //            }

                    //            if (!isBcjMember)
                    //                _logdebug.Debug("招新应急-注册-" + request.OPENID + "：为佰草集新客");
                    //        }
                    //        else
                    //            _logdebug.Debug("招新应急-注册-" + request.OPENID + "：为佰草集新客");


                    //    }

                    //    if (!isBcjMember)//满足发券资格
                    //    {
                    //        WXCouponGiveInfo WXCouponGiveInfo = new WXCouponGiveInfo();
                    //        WXCouponGiveInfo.Openid = request.OPENID;
                    //        WXCouponGiveInfo.CreateDate = DateTime.Now;
                    //        WXCouponGiveInfo.CardId = AppConfig.CardId;
                    //        _hmjMember.insertWXCouponGiveInfo(WXCouponGiveInfo);
                    //        //WriteLog("注册会员-满足发券资格", request.OPENID);
                    //        _logdebug.Debug("招新应急-注册-" + request.OPENID + "：客人满足发券资格");
                    //    }
                    //    else
                    //        _logdebug.Debug("招新应急-注册-" + request.OPENID + "：客人不满足发券资格");

                    //}

                    #endregion
                    return ResponseJson(true, "OK", new { IS_POINT = isbind, IS_BRAND = ok });
                }

                return ResponseJson(false, msg);
            }
            catch (Exception ex)
            {
                WriteLog("注册会员", ex);
                return ResponseJsonError(false, error_message, ex);
            }
        }
        #endregion

        #region 修改会员信息
        /// <summary>
        /// 修改会员信息
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private dynamic UpdateMember(dynamic arg)
        {
            try
            {
                //得到请求参数
                MemberUpdateReqDTO request = base.BindObject<MemberUpdateReqDTO>();

                string str = _hmjMember.UpdateMember(request);

                if (str == "1")
                {
                    return ResponseJson(true, "OK");

                }

                return ResponseJson(false, str);
            }
            catch (Exception ex)
            {
                WriteLog("修改会员信息", ex);
                return ResponseJsonError(false, error_message, ex);
            }
        }
        #endregion

        #region 得到crm的会员信息
        /// <summary>
        /// 得到crm的会员信息
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private dynamic GetCrmMember(dynamic arg)
        {
            try
            {
                //手机号
                string mobile = base.GetValue<string>("mobile");
                CrmMemberResDTO crmDto = GetCrmMemberDto(mobile);

                if (crmDto == null)
                {
                    return ResponseJson(false, "crm不存在该会员");
                }

                return ResponseJson(true, "OK", crmDto);
            }
            catch (Exception ex)
            {
                WriteLog("得到crm的会员信息", ex);
                return ResponseJsonError(false, error_message, ex);
            }
        }
        #endregion

        #region 修改会员扩展信息
        /// <summary>
        /// 修改会员扩展信息
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private dynamic UpdateMemberExtendMsg(dynamic arg)
        {
            try
            {
                //得到请求参数
                MemberExtendDTO request = base.BindObject<MemberExtendDTO>();

                if (string.IsNullOrWhiteSpace(request.PARTNER))
                {
                    return ResponseJson(false, "请输入正确的会员号");
                }

                if (!string.IsNullOrWhiteSpace(request.KID_BIRTHDAY))
                {
                    if (request.KID_BIRTHDAY.Contains("null"))
                    {
                        return ResponseJson(false, "不可包含null");
                    }

                    if (DateTime.Now < DateTime.Parse(request.KID_BIRTHDAY))
                    {
                        return ResponseJson(false, "对不起，大宝生日不要超过当前时间");
                    }
                }

                if (!string.IsNullOrWhiteSpace(request.KID_BIRTHDAY2))
                {
                    if (request.KID_BIRTHDAY2.Contains("null"))
                    {
                        return ResponseJson(false, "不可包含null");
                    }

                    if (DateTime.Now < DateTime.Parse(request.KID_BIRTHDAY2))
                    {
                        return ResponseJson(false, "对不起，二宝生日不要超过当前时间");
                    }
                }

                string flg = string.Empty;

                string mobile = _hmjMember.GetMemberMobileByBP(request.PARTNER);

                if (string.IsNullOrWhiteSpace(mobile))
                {
                    return ResponseJson(false, "对不起查不到该会员");
                }

                string msg = UpdateMemberExtend(request, mobile, ref flg);


                return ResponseJson(true, msg);
            }
            catch (Exception ex)
            {
                WriteLog("修改会员扩展信息", ex);
                return ResponseJsonError(false, error_message, ex);
            }
        }
        #endregion

        #region 得到会员的扩展信息
        /// <summary>
        /// 得到会员的扩展信息
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private dynamic GetMemberExtendMsg(dynamic arg)
        {
            try
            {
                //openid
                string openid = base.GetValue<string>("openid");

                MemberDetailResDTO detail = _hmjMember.GetMemberDetail(openid, "1");

                if (detail == null)
                {
                    return ResponseJson(false, "对不起查无此人");
                }

                MemberExtendDTO exten = GetMemberExt(detail.PARTNER);

                if (exten != null)
                {
                    exten.PARTNER = detail.PARTNER;
                }


                return ResponseJson(true, "OK", exten);
            }
            catch (Exception ex)
            {
                WriteLog("得到会员的扩展信息", ex);
                return ResponseJsonError(false, error_message, ex);
            }
        }
        #endregion

        #region 上传图片
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private dynamic UploadImage(dynamic arg)
        {
            try
            {
                HttpPostedFile files = HttpContext.Current.Request.Files["Image_Data"];
                //获取第一个文件流
                Stream st = files.InputStream;

                string openid = base.GetValue<string>("openid");

                //获取全路径
                string path = HttpContext.Current.Server.MapPath("../Image");
                string upinge = DateTime.Now.ToString("yyyyMM");
                string day = DateTime.Now.Day.ToString();
                string allpath = path + "/" + upinge + "/" + day;

                //如果路径不存在就创建路径
                if (!Directory.Exists(allpath))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(allpath);
                    directoryInfo.Create();
                }

                Random ran = new Random();

                //图片名称
                string pathname = DateTime.Now.ToString("yyyyMMddhhmmss") + "-" + ran.Next(1000, 9999).ToString() + ".jpeg";

                string valtr = "/Image/" + upinge + "/" + day + "/" + pathname;
                //此处可以给上传的图片加水印效果
                using (Stream stre = new FileStream(allpath + "/" + pathname, FileMode.CreateNew))
                //using (Stream reder = new FileStream(@"E:\work\RongMeMeAPI\PuMan.BeautyFarmPOS.API.WebApi/one.png", FileMode.Open))
                using (Image bmp = Image.FromStream(st))
                using (Graphics g = Graphics.FromImage(bmp))//得到图片的画布
                {
                    //g.DrawString("马泽工", new Font(FontFamily.GenericSerif, 9), Brushes.Red, 302, 46);//Font应该被释放
                    bmp.Save(stre, ImageFormat.Jpeg);//图片保存到输出流            
                }

                //保存相对路径valtr
                string flg = _hmjMember.UpdateImageUrl(valtr, openid);

                if (flg == "0")
                {
                    return ResponseJson(false, "对不起不存在该粉丝信息");
                }

                return ResponseJson(true, "OK", AppConfig.BeautyChinaUrl + valtr);
            }
            catch (Exception ex)
            {
                WriteLog("上传图片", ex);
                return ResponseJsonError(false, error_message, ex);
            }
        }
        #endregion

        #region 判断是否绑定
        /// <summary>
        /// 判断是否绑定
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private dynamic ChckBind(dynamic arg)
        {
            try
            {
                //openid
                string openid = base.GetValue<string>("openid");
                bool isok = _hmjMember.ChckBind(openid);
                return ResponseJson(true, "OK", isok);
            }
            catch (Exception ex)
            {
                WriteLog("判断是否绑定", ex);
                return ResponseJsonError(false, error_message, ex);
            }
        }
        #endregion

        #region 得到会员详细信息
        /// <summary>
        /// 得到会员详细信息
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private dynamic GetMemberDetail(dynamic arg)
        {
            try
            {
                //得到请求参数
                MemberDetailReqDTO request = base.BindObject<MemberDetailReqDTO>();

                MemberDetailResDTO detail = _hmjMember.GetMemberDetail(request.OPENID, request.IS_UPDATE);

                if (detail == null)
                {
                    return ResponseJson(false, "对不起，无法查询到该会员信息", detail);
                }

                return ResponseJson(true, "OK", detail);
            }
            catch (Exception ex)
            {
                WriteLog("得到会员详细信息", ex);
                return ResponseJsonError(false, error_message, ex);
            }
        }
        #endregion

        #region 绑定会员
        /// <summary>
        /// 绑定会员
        /// </summary>
        /// <param name="arg"></param>
        /// <remarks>此操作会发送模板消息</remarks>
        private dynamic Binding(dynamic arg)
        {
            try
            {
                //得到请求参数
                BindReqDTO request = base.BindObject<BindReqDTO>();

                string flg = _hmjMember.Binding(request);

                if (flg == "3")
                {
                    return ResponseJson(false, "不存在粉丝信息，请重新关注本公众号", "3");
                }

                if (flg == "2")
                {
                    return ResponseJson(false, "您已经绑定过会员，请不要重复绑定。", "2");
                }

                if (flg == "4")
                {
                    return ResponseJson(false, "不存在会员信息，请先注册华美家会员。", "4");
                }

                if (flg == "5")
                {
                    //return ResponseJson(false, "crm有重复会员，请联系客服", "5");
                    return ResponseJson(false, "您已经注册成功了，请返回公众号重新进入", "5");
                }

                QueryMemberShipBindingResDTO DTO = _hmjMember.QueryMemberShipBinding(request.OPENID);

                int ok = 1;
                int isbind = 1;

                //如果无品牌
                if (DTO.BRAND_LIST == null || DTO.BRAND_LIST.Count() <= 0)
                {
                    ok = 0;
                    //如果是待激活状态那么就要激活
                    if (request.ZZAFLD000004 == "E0005")
                    {
                        //激活
                        _hmjMember.ChageSatus(request.MOBILE);
                    }
                }
                else
                {
                    foreach (QueryMemberBindDetailResDTO model in DTO.BRAND_LIST)
                    {
                        if (model.IF_BINDING == "0")
                        {
                            isbind = 0;
                        }
                    }
                }



                return ResponseJson(true, "OK", new { IS_POINT = isbind, IS_BRAND = ok });
            }
            catch (Exception ex)
            {
                WriteLog("绑定会员", ex);
                return ResponseJsonError(false, error_message, ex);
            }
        }
        #endregion

        #region CRM-绑定关系同步接口
        /// <summary>
        /// CRM-绑定关系同步接口
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private dynamic BindingRelShip(dynamic arg)
        {
            try
            {
                //得到请求参数
                string OPENID = base.GetValue<string>("OPENID");

                //得到code列表
                string BRAND_CODE = base.GetValue<string>("BRAND_CODE").Trim(',');

                //得到所有积分
                string POIT = base.GetValue<string>("POIT");

                string Str_REG_DATE = "";
                BindingRelShipResDTO flg = _hmjMember.BindingRelShip(OPENID, BRAND_CODE, POIT, ref Str_REG_DATE);

                if (flg == null)
                {
                    return ResponseJson(false, "对不起，无法查到您的会员信息");
                }
                bool isBcjMember = false;
                if (flg.ZRETURN == "Y")
                {
                    #region 招新应急
                    //DateTime StartDate = DateTime.MinValue;
                    //DateTime EndDate = DateTime.MinValue;
                    //if (!string.IsNullOrWhiteSpace(AppConfig.RecruitNewFansStartDate))
                    //{
                    //    try
                    //    {
                    //        StartDate = DateTime.Parse(AppConfig.RecruitNewFansStartDate + " 00:00:00");
                    //    }
                    //    catch { }
                    //}

                    //if (!string.IsNullOrWhiteSpace(AppConfig.RecruitNewFansEndDate))
                    //{
                    //    try
                    //    {
                    //        EndDate = DateTime.Parse(AppConfig.RecruitNewFansEndDate + " 23:59:59");
                    //    }
                    //    catch { }
                    //}

                    //if (StartDate <= DateTime.Now && EndDate >= DateTime.Now)
                    //{
                    //    HmjMemberDetail member = _hmjMember.GetMemberDetailByOpenId(OPENID);
                    //    //判断没有送过券
                    //    List<WXCouponGiveInfo> listWXCouponGiveInfo = _hmjMember.GetWXCouponGiveInfo(OPENID, AppConfig.CardId);
                    //    if (listWXCouponGiveInfo != null && listWXCouponGiveInfo.Count > 0)
                    //        _logdebug.Debug("招新应急-绑定-" + OPENID + "：客人已经领过券");
                    //    if (member != null && (listWXCouponGiveInfo == null || listWXCouponGiveInfo.Count <= 0))
                    //    {
                    //        DateTime HMJRegisterDate = DateTime.MinValue;
                    //        DateTime REG_DATE = DateTime.MinValue;
                    //        try
                    //        {
                    //            HMJRegisterDate = DateTime.Parse(AppConfig.HMJRegisterDate);
                    //        }
                    //        catch { }
                    //        try { REG_DATE = DateTime.Parse(Str_REG_DATE); } catch { }
                    //        if (!isBcjMember && REG_DATE > DateTime.MinValue && HMJRegisterDate > DateTime.MinValue && HMJRegisterDate <= REG_DATE)
                    //        {
                    //            //判断是否在佰草集新客

                    //            //品牌积分查询
                    //            QueryMemberShipBindingResDTO result = _hmjMember.QueryMemberShipBinding(OPENID);
                    //            if (result != null && result.BRAND_LIST.Count > 0)
                    //            {
                    //                for (int i = 0; i < result.BRAND_LIST.Count; i++)
                    //                {
                    //                    if (result.BRAND_LIST[i].DATA_SOURCE2 == "0001" || result.BRAND_LIST[i].DATA_SOURCE2 == "0002")
                    //                    {
                    //                        isBcjMember = true;
                    //                        _logdebug.Debug("招新应急-绑定-" + OPENID + "：为佰草集老客");
                    //                        break;
                    //                    }
                    //                }

                    //                if (!isBcjMember)
                    //                    _logdebug.Debug("招新应急-绑定-" + OPENID + "：为佰草集新客");
                    //            }
                    //            else
                    //                _logdebug.Debug("招新应急-绑定-" + OPENID + "：为佰草集新客");
                    //        }

                    //        if (!isBcjMember && REG_DATE != DateTime.MinValue && HMJRegisterDate != DateTime.MinValue && HMJRegisterDate <= REG_DATE)//满足发券资格
                    //        {
                    //            WXCouponGiveInfo WXCouponGiveInfo = new WXCouponGiveInfo();
                    //            WXCouponGiveInfo.Openid = OPENID;
                    //            WXCouponGiveInfo.CreateDate = DateTime.Now;
                    //            WXCouponGiveInfo.CardId = AppConfig.CardId;
                    //            _hmjMember.insertWXCouponGiveInfo(WXCouponGiveInfo);
                    //            //WriteLog("注册会员-满足发券资格", request.OPENID);
                    //            _logdebug.Debug("招新应急-绑定-" + OPENID + "：客人满足发券资格，注册日期=" + REG_DATE);

                    //        }
                    //        else
                    //            _logdebug.Debug("招新应急-绑定-" + OPENID + "：客人不满足发券资格，注册日期=" + REG_DATE);

                    //    }
                    //}



                    #endregion


                    return ResponseJson(true, "绑定成功");
                }

                return ResponseJson(false, flg.MESSAGE);
            }
            catch (Exception ex)
            {
                WriteLog("CRM-绑定关系同步接口", ex);
                return ResponseJsonError(false, error_message, ex);
            }
        }
        #endregion

        #region 品牌积分查询
        /// <summary>
        /// 品牌积分查询
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private dynamic QueryMemberShipBinding(dynamic arg)
        {
            try
            {
                //得到请求参数
                string OPENID = base.GetValue<string>("OPENID");

                QueryMemberShipBindingResDTO flg = _hmjMember.QueryMemberShipBinding(OPENID);

                if (flg == null)
                {
                    return ResponseJson(false, "失败");
                }

                return ResponseJson(true, "OK", flg);
            }
            catch (Exception ex)
            {
                WriteLog("品牌积分查询", ex);
                return ResponseJsonError(false, error_message, ex);
            }
        }
        #endregion

        #region 通过手机查询品牌绑定信息
        /// <summary>
        /// 品牌积分查询
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private dynamic QueryBindingByMobile(dynamic arg)
        {
            try
            {
                //得到请求参数
                string MOBILE = base.GetValue<string>("MOBILE");
                QueryMemberShipBindingResDTO flg = _hmjMember.QueryMemberShipBinding("", MOBILE);

                if (flg == null)
                {
                    return ResponseJson(false, "失败");
                }

                return ResponseJson(true, "OK", flg);
            }
            catch (Exception ex)
            {
                WriteLog("品牌积分查询", ex);
                return ResponseJsonError(false, error_message, ex);
            }
        }
        #endregion

        #region 修改会员兑礼密码
        private dynamic UpdateChangePwd(dynamic arg)
        {
            try
            {
                //得到请求参数
                //OpendID
                string OpendID = base.GetValue<string>("OpendID");

                //新手机号
                //string mobile = Request["mobile"];

                //旧的手机号
                //string oldmobile = Request["oldmobile"];

                //新密码
                string pwd = base.GetValue<string>("NewPwd");

                //旧密码
                string oldpwd = base.GetValue<string>("OldPwd");

                string msgno = string.Empty;

                //修该密码或者修该手机号（CRM端修改）
                int cout = _custMember.UpdateMobileOrPwd("", "", pwd, oldpwd,
                    OpendID, ref msgno);

                if (cout <= 0)
                {
                    return ResponseJson(false, "失败", "对不起，密码或者手机号未完整。");
                }
                if (cout == 2)
                {
                    return ResponseJson(false, "失败");
                }


                return ResponseJson(true, "OK", cout);
            }
            catch (Exception ex)
            {
                WriteLog("兑礼密码修改", ex);
                return ResponseJsonError(false, error_message, ex);
            }
        }
        #endregion

        #region 通过openid获取到用户信息 手机号，姓名
        private dynamic GetUserInfoByOpenid(dynamic arg)
        {
            try
            {
                string openid = base.GetValue<string>("openid");
                HmjMemberDetail member = _hmjMember.GetMemberDetailByOpenId(openid);

                if (member == null)
                {
                    return ResponseJson(false, "失败", "查无此人");
                }
                return ResponseJson(true, "OK", member);
            }
            catch (Exception ex)
            {
                return ResponseJsonError(false, error_message, ex);
            }
        }
        #endregion

        #region 是否可以领取
        private dynamic CanGetCoupon(dynamic arg)
        {
            try
            {
                //得到请求参数
                //OpendID

                DateTime StartDate = DateTime.MinValue;
                DateTime EndDate = DateTime.MinValue;
                if (!string.IsNullOrWhiteSpace(AppConfig.RecruitNewFansStartDate))
                {
                    try
                    {
                        StartDate = DateTime.Parse(AppConfig.RecruitNewFansStartDate + " 00:00:00");
                    }
                    catch { }
                }

                if (!string.IsNullOrWhiteSpace(AppConfig.RecruitNewFansEndDate))
                {
                    try
                    {
                        EndDate = DateTime.Parse(AppConfig.RecruitNewFansEndDate + " 23:59:59");
                    }
                    catch { }
                }

                if (StartDate <= DateTime.Now && EndDate >= DateTime.Now)
                {

                    string OpendID = base.GetValue<string>("openid");


                    WXCouponGiveInfo WXCouponGiveInfo = _hmjMember.CanGetCoupon(OpendID, AppConfig.CardId);
                    if (WXCouponGiveInfo != null)
                    {

                        // WXCouponGiveInfo WXCouponGiveInfo = _hmjMember.GetWXCouponGiveInfoByOpenid(OpendID);

                        var cardId = AppConfig.CardId;
                        string signature = "";
                        string timestamp = "";
                        string api_ticket = "";

                        timestamp = Utility.ConvertDateTimeInt(DateTime.Now).ToString();
                        string token = Token(AppConfig.FWHOriginalID);
                        api_ticket = GetCardApi(token);
                        string[] ArrTmp = { cardId, timestamp, api_ticket };
                        Array.Sort(ArrTmp);
                        string tmpStr = string.Join("", ArrTmp);
                        tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
                        signature = tmpStr;

                        CardExt cardExt = new CardExt();
                        cardExt.timestamp = timestamp;
                        cardExt.signature = signature;
                        var retcardExt = JsonConvert.SerializeObject(cardExt);// @"{""timestamp"":""" + timestamp + @""",""signature"":""" + signature + @"""}";

                        // return View(CardSDK);

                        CardInfo CardInfo = new CardInfo();
                        CardInfo.cardId = cardId;
                        CardInfo.cardExt = retcardExt;

                        return ResponseJson(true, "OK", CardInfo);
                    }
                    else
                    {
                        return ResponseJson(false, "失败", "不满足送券资格");
                    }
                }
                else
                {
                    return ResponseJson(false, "失败", "活动结束");
                }
            }
            catch (Exception ex)
            {
                return ResponseJsonError(false, ex.Message, ex);
            }
        }


        private dynamic IsNewCustForBCJ(dynamic arg)
        {
            try
            {

                DateTime StartDate = DateTime.MinValue;
                DateTime EndDate = DateTime.MinValue;
                if (!string.IsNullOrWhiteSpace(AppConfig.RecruitNewFansStartDate))
                {
                    try
                    {
                        StartDate = DateTime.Parse(AppConfig.RecruitNewFansStartDate + " 00:00:00");
                    }
                    catch { }
                }

                if (!string.IsNullOrWhiteSpace(AppConfig.RecruitNewFansEndDate))
                {
                    try
                    {
                        EndDate = DateTime.Parse(AppConfig.RecruitNewFansEndDate + " 23:59:59");
                    }
                    catch { }
                }

                if (StartDate <= DateTime.Now && EndDate >= DateTime.Now)
                {
                    //得到请求参数
                    string OPENID = base.GetValue<string>("OPENID");


                    HmjMemberDetail member = _hmjMember.GetMemberDetailByOpenId(OPENID);



                    //品牌积分查询
                    bool isBcjMember = false;
                    QueryMemberShipBindingResDTO result = _hmjMember.QueryMemberShipBinding(OPENID);
                    if (result != null && result.BRAND_LIST.Count > 0)
                    {
                        for (int i = 0; i < result.BRAND_LIST.Count; i++)
                        {
                            if (result.BRAND_LIST[i].DATA_SOURCE2 == "0001" || result.BRAND_LIST[i].DATA_SOURCE2 == "0002")
                            {
                                isBcjMember = true;
                                return ResponseJson(true, "招新应急-绑定-" + OPENID + "：为佰草集老客");
                            }
                        }

                        if (!isBcjMember)
                            return ResponseJson(true, "招新应急-绑定-" + OPENID + "：为佰草集新客");
                    }

                    return ResponseJson(true, "招新应急-绑定-" + OPENID + "：为佰草集新客");
                }
                else
                {
                    return ResponseJson(false, "活动结束");
                }





            }
            catch (Exception ex)
            {

                return ResponseJsonError(false, error_message, ex);
            }
        }

        public class CardExt
        {
            public string timestamp { get; set; }
            public string signature { get; set; }
        }

        public class CardInfo
        {
            public string cardId { get; set; }
            public string cardExt { get; set; }
        }
        #endregion

        #region 私有方法

        /// <summary>
        /// 修改会员扩展信息
        /// </summary>
        /// <param name="request"></param>
        private string UpdateMemberExtend(MemberExtendDTO request, string mobile, ref string flg)
        {
            dt_Dyn_UpdateMemberShip_req updates = new dt_Dyn_UpdateMemberShip_req();
            ZCRMT322_Dyn z = new ZCRMT322_Dyn
            {
                //大宝生日
                KID_BIRTHDAY = request.KID_BIRTHDAY,
                //二宝生日
                KID_BIRTHDAY2 = request.KID_BIRTHDAY2,
                //从事行业
                ZC019 = request.ZC019,
                //收入范围
                ZC004 = request.ZC004,
                ZC016 = request.ZC016,//婚姻状况
                BRAND_PREF = request.BRAND_PREF, //"01,02,03";//品牌编号
                ZA003 = request.ZA003, //"01";//皮肤特征
                ZA004 = request.ZA004, //"02,03";//皮肤问题
                CLASS_PREF = request.CLASS_PREF,//品类编号
                DATA_SOURCE = AppConfig.DATA_SOURCE,
                LOYALTY_BRAND = AppConfig.LOYALTY_BRAND,//忠诚度品牌
                SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM,//来源系统
                VGROUP = AppConfig.VGROUP, //销售组织
                REGION = request.REGION,//省编号
                TRANSPZONE = request.TRANSPZONE,//市编号
                INFO_WANTED = request.INFO_WANTED, //"001,002";//何处了解华美家
                ACCOUNT_ID = mobile, //"18952435467";
                PARTNER = request.PARTNER // "MCHM000000003";
            };
            updates.ZCRMT316 = new ZCRMT322_Dyn[] { z };

            dt_Dyn_UpdateMemberShip_res ups = WebHmjApiHelp.UpdateMemberShip(updates);
            flg = ups.WV_RETURN;

            #region 完善资料送积分 2018.03.29 23:59:59停止使用
            //if (flg == "Y")
            //{
            //    bool bo = _hmjMember.SendPoint(request);
            //    if (bo)
            //    {
            //        #region 微信模板消息 2018.03.29 23:59:59停止使用
            //        //接口查询会员主数据，用于模板消息中的内容填充
            //        //dt_Dyn_DispMember_req w = new dt_Dyn_DispMember_req();
            //        //w.DATA_SOURCE = AppConfig.DATA_SOURCE;
            //        //w.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
            //        //w.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
            //        //w.VGROUP = AppConfig.VGROUP; //销售组织
            //        //w.PARTNER = request.PARTNER;//会员唯一标示
            //        //dt_Dyn_DispMember_res dt = WebHmjApiHelp.DispMember(w);
            //        //if (dt.ZCRMT316 == null || dt.ZCRMT316.Length <= 0)
            //        //{
            //        //    return null;
            //        //}
            //        //if (dt.ZCRMT316.Length > 1)
            //        //{
            //        //    return null;
            //        //}
            //        //ZCRMT302_Dyn newmeber = dt.ZCRMT316[0];

            //        //var openid = request.OPENID;
            //        //var tempId = "nJqLvWytZJ2IUBdOapJ3RQcJjD3Zvt0UYdvQ1A-5IQ8";
            //        //var redirect_url = AppConfig.HmjWebApp + "assets/hmjweixin/html/hytq.html";
            //        //var p1 = "恭喜您完善资料成功！";
            //        //var p2 = DateTime.Now.ToString("yyyy年MM月dd日");
            //        //var p3 = AppConfig.MemberSend.Split('|')[0];
            //        //var p4 = "完善资料成功";
            //        //var p5 = newmeber.ZCCUR_POINT.ToString();
            //        //var p6 = "今后您可获得更多积分、享受更多优惠。了解更多会员权益，点击查看 >";
            //        //_hmjMember.SendTmpPublicFunc(true, openid, tempId, redirect_url,
            //        //    p1, p2, p3, p4, p5, p6);
            //        #endregion
            //    }
            //}
            #endregion

            return ups.WV_MESSAGE;
        }

        /// <summary>
        /// 得到会员的扩展信息
        /// </summary>
        /// <param name="pARTNER"></param>
        /// <returns></returns>
        private MemberExtendDTO GetMemberExt(string pARTNER)
        {
            dt_Dyn_DispMember_req w = new dt_Dyn_DispMember_req();

            w.DATA_SOURCE = AppConfig.DATA_SOURCE;
            w.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
            w.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
            w.VGROUP = AppConfig.VGROUP; //销售组织
            w.PARTNER = pARTNER;//2002652891
            dt_Dyn_DispMember_res dt = WebHmjApiHelp.DispMember(w);

            if (dt.ZCRMT316 == null || dt.ZCRMT316.Length <= 0)
            {
                return null;
            }

            if (dt.ZCRMT316.Length > 1)
            {
                return null;
            }

            ZCRMT302_Dyn newmeber = dt.ZCRMT316[0];


            return newmeber.MapTo<MemberExtendDTO>();
        }

        /// <summary>
        /// 根据手机号得到crm的会员信息
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public CrmMemberResDTO GetCrmMemberDto(string mobile)
        {
            dt_Dyn_DispMemQuick_req w = new dt_Dyn_DispMemQuick_req
            {
                DATA_SOURCE = AppConfig.DATA_SOURCE,
                LOYALTY_BRAND = AppConfig.LOYALTY_BRAND,//忠诚度品牌
                SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM,//来源系统
                VGROUP = AppConfig.VGROUP, //销售组织
                ACCOUNT_ID = mobile//2002652891
            };
            dt_Dyn_DispMemQuick_res dt = WebHmjApiHelp.DispMemQuick(w);

            if (dt.I_ZCRMT316 == null || dt.I_ZCRMT316.Length <= 0)
            {
                return null;
            }

            if (dt.I_ZCRMT316.Length > 1)
            {
                return null;
            }

            dt_Dyn_DispMemQuick_resITEM newmeber = dt.I_ZCRMT316[0];

            return new CrmMemberResDTO()
            {
                ZZAFLD000004 = newmeber.ZZAFLD000004,
                BIRTHDAY = newmeber.BIRTHDT,
                GENDER = newmeber.XSEX == "2" ? "1" : "0",
                NAME_FIRST = newmeber.NAME_FIRST,
                NAME_LAST = newmeber.NAME_LAST,
                REG_DATE = newmeber.REG_DATE,
                ACCOUNT_ID = newmeber.ACCOUNT_ID
            };
        }

        #endregion

        private void WriteLog(string funcName, Exception ex)
        {
            _logerror.Error(funcName + "报错\r\n时间" + DateTime.Now.ToString() + "\r\n" + ex.Message + "\r\n" + ex.ToString());
        }
    }
}