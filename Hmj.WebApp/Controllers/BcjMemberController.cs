using BarcodeLib;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using Hmj.Business;
using Hmj.Entity.Entities;
using Hmj.Entity.Jsons;
using Hmj.Entity.WxModel;
using Hmj.Interface;
using Hmj.WebService;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Mvc;

namespace Hmj.WebApp.Controllers
{
    public class BcjMemberController : Controller
    {
        private ICustMemberService _custMember;

        public BcjMemberController(ICustMemberService custMember)
        {
            _custMember = custMember;
        }

        /// <summary>
        /// 显示客服页面
        /// </summary>
        /// <returns></returns>
        [Outh(true)]
        //[IsLogin(true)]
        public ActionResult ShowPer()
        {
            FansInfo Fans = Session["FansInfo"] as FansInfo;

            Fans = Fans == null ? new FansInfo() { Nickname = "当前粉丝异常" } : Fans;

            return Redirect($@"https://www.sobot.com/chat/h5/index.html?sysNum=f6736c79af394a31b88fe19b59e589e7&source=1&uname={Fans.Nickname}&face={Fans.Headimgurl}");
        }

        /// <summary>
        /// 绑定会员
        /// </summary>
        /// <returns></returns>
        [Outh(true)]
        public ActionResult Index()
        {
            FansInfo Fans = Session["FansInfo"] as FansInfo;

            Fans = Fans == null ? new FansInfo() { Nickname = "当前粉丝异常" } : Fans;

            if (_custMember.ChckBind(Fans.Openid))
            {
                //如果该粉丝已经绑定了会员，则直接进入到会员详情界面
                return Redirect("ShowMemberInfo.do");
            }

            return View(Fans);
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <returns></returns>
        public ActionResult SendMsg()
        {
            JsonSMsg msg = new JsonSMsg();
            string mobile = Request["mobile"];

            Random r = new Random();
            int num = r.Next(100000, 999999);
            //string message = string.Format("本次微信平台获取的验证码是：" + num);
            string message = string.Format("本次微信平台获取的验证码是："
                + num + "。如非本人操作，请致电400-821-6188");

            dt_SMSInsert_req req = new dt_SMSInsert_req();
            req.SMS_ITEM = new SMS_ITEM[] { new SMS_ITEM() { CONTENT = message,
                MESSAGEID = "0000001", MESSAGETYPE = "BC_WX_SMS",
                MOBILE = mobile, MSGFORMAT = "8", SRCNUMBER = "106900291033" } };

            dt_SMSInsert_res res = WebApiHelp.OldSMSInsert(req);
            if (res.zstatus == "1")
            {
                msg.Data = num;
                msg.Status = 1;
                msg.Message = "发送成功";
                return Json(msg);
            }
            msg.Data = 0;
            msg.Status = 0;
            msg.Message = "发送失败";
            return Json(msg);
        }

        /// <summary>
        /// 绑定
        /// </summary>
        /// <returns></returns>
        public ActionResult BindMember()
        {
            JsonSMsg msg = new JsonSMsg();
            try
            {
                string mobile = Request["mobile"];
                FansInfo fansinfo = Session["FansInfo"] as FansInfo;

                if (fansinfo == null)
                {
                    msg.Status = 0;
                    msg.Message = "请重新加载该页面";
                    return Json(msg);
                }

                string str = _custMember.BindMember(mobile, fansinfo.Openid, fansinfo.Nickname);

                if (str == "2")
                {
                    msg.Status = 0;
                    msg.Message = "对不起，您要绑定的手机号已经存在！";
                    return Json(msg);
                }

                if (str == "3")
                {
                    msg.Status = 0;
                    msg.Message = "系统未记录您的信息，请重新关注该公众号！";
                    return Json(msg);
                }

                if (str == "4")
                {
                    msg.Status = 4;
                    msg.Message = "您还没有注册，请前往注册会员！";
                    return Json(msg);
                }

                if (str == "5")
                {
                    msg.Status = 0;
                    msg.Message = "您好，您的手机号对应多个会员账号，请联系客服！";
                    return Json(msg);
                }

                msg.Status = 1;
                msg.Message = "绑定成功";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Status = 0;
                msg.Message = "对不起网络有问题，请关闭当前页面重新进入："+ex.Message;
                return Json(msg);
            }
        }

        /// <summary>
        /// 显示注册页面
        /// </summary>
        /// <returns></returns>
        [Outh(true)]
        public ActionResult ShowRegister()
        {
            FansInfo Fans = Session["FansInfo"] as FansInfo;

            //手机号
            string mobile = Request["mobile"];

            ViewBag.Mobile = mobile;
            ViewBag.Openid = Fans.Openid;
            ViewBag.NickName = Fans.Nickname;
            ViewBag.Gender = Fans.Gender;
            return View();
        }

        /// <summary>
        /// 显示会员详细信息
        /// </summary>
        /// <returns></returns>
        [Outh(true)]
        public ActionResult ShowMemberInfo()
        {
            FansInfo Fans = Session["FansInfo"] as FansInfo;

            //该方法先获取接口里面会员信息，然后更新本地会员信息（这样后续的会员更新等就不需要连续调用会员主数据接口了）
            MemberInfo meberinfo = _custMember.GetLoadMember(Fans.Openid);

            meberinfo.Headimgurl = Fans.Headimgurl;
            meberinfo.Nickname = Fans.Nickname;

            return View(meberinfo);
        }

        /// <summary>
        /// 显示二维码
        /// </summary>
        /// <returns></returns>
        public void ShowQrCode()
        {
            try
            {
                string code = Request["code"];

                QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
                QrCode qrCode = new QrCode();
                qrEncoder.TryEncode(code, out qrCode);

                GraphicsRenderer renderer = new GraphicsRenderer(new FixedModuleSize(5, QuietZoneModules.Two),
                    Brushes.Black, Brushes.White);

                using (MemoryStream ms = new MemoryStream())
                {
                    renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, ms);
                    Image img = Image.FromStream(ms);
                    img.Save(Response.OutputStream, ImageFormat.Jpeg);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// 显示条形码
        /// </summary>
        /// <returns></returns>
        public void ShowBarCode()
        {
            try
            {
                string code = Request["code"];

                using (var barcode = new Barcode()
                {
                    IncludeLabel = true,
                    Alignment = AlignmentPositions.CENTER,
                    Width = 400,
                    Height = 120,
                    RotateFlipType = RotateFlipType.RotateNoneFlipNone,
                    BackColor = Color.White,
                    ForeColor = Color.Black,
                })
                {
                    barcode.Encode(TYPE.CODE128B, code).
                        Save(Response.OutputStream, ImageFormat.Jpeg);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// 注册会员
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            JsonSMsg msgjson = new JsonSMsg();
            string mobile = Request["mobile"];
            string openid = Request["openid"];
            string nameo = Request["nameo"];
            string namek = Request["namek"];

            string nickname = Request["nickname"];
            string brithday = Request["brithday"];
            string gender = Request["gender"];

            if (string.IsNullOrEmpty(nameo))
            {
                msgjson.Status = 0;
                msgjson.Message = "姓氏不能是空";
                return Json(msgjson);
            }

            if (string.IsNullOrEmpty(namek))
            {
                msgjson.Status = 0;
                msgjson.Message = "名称不能是空";
                return Json(msgjson);
            }

            if (string.IsNullOrEmpty(brithday))
            {
                msgjson.Status = 0;
                msgjson.Message = "生日不能是空";
                return Json(msgjson);
            }

            if (DateTime.Now < DateTime.Parse(brithday))
            {
                msgjson.Status = 0;
                msgjson.Message = "对不起，生日不要超过当前时间";
                return Json(msgjson);
            }

            string msg = _custMember.RegisterMember(mobile, openid,
                nameo, namek, brithday, nickname, gender);

            //创建成功
            if (msg == "1")
            {
                msgjson.Status = 1;
                msgjson.Message = "创建成功";
                return Json(msgjson);
            }

            msgjson.Status = 0;
            msgjson.Message = msg;
            return Json(msgjson);
        }

        /// <summary>
        /// 显示会员
        /// </summary>
        /// <returns></returns>
        [Outh(true)]
        public ActionResult ShowMemberDetail()
        {
            FansInfo Fans = Session["FansInfo"] as FansInfo;

            MemberInfo meberinfo = _custMember.GetMemberInfo(Fans.Openid);

            meberinfo.Nickname = Fans.Nickname;
            meberinfo.Headimgurl = Fans.Headimgurl;

            return View(meberinfo);
        }

        /// <summary>
        /// 显示修该会员
        /// </summary>
        /// <returns></returns>
        [Outh(true)]
        public ActionResult UpdateMemberDetail()
        {
            FansInfo Fans = Session["FansInfo"] as FansInfo;

            MemberInfo meberinfo = _custMember.GetMemberInfo(Fans.Openid);

            meberinfo.Nickname = Fans.Nickname;
            meberinfo.Headimgurl = Fans.Headimgurl;
            meberinfo.OpenID = Fans.Openid;

            return View(meberinfo);
        }

        /// <summary>
        /// 修该密码或者修该手机号
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateMobileOrPwd()
        {
            JsonSMsg msg = new JsonSMsg();
            //OpendID
            string OpendID = Request["OpendID"];

            //新手机号
            string mobile = Request["mobile"];

            //旧的手机号
            string oldmobile = Request["oldmobile"];

            //新密码
            string pwd = Request["pwd"];

            //旧密码
            string oldpwd = Request["oldpwd"];

            string msgno = string.Empty;

            //修该密码或者修该手机号
            int cout = _custMember.UpdateMobileOrPwd(mobile, oldmobile, pwd, oldpwd,
                OpendID, ref msgno);

            if (cout <= 0)
            {
                msg.Status = 0;
                msg.Message = "对不起，密码或者手机号未完整。";
                return Json(msg);
            }
            if (cout == 2)
            {
                msg.Status = 0;
                msg.Message = msgno.TrimEnd(';').TrimEnd('；');
                return Json(msg);
            }

            msg.Status = 1;
            msg.Message = "成功";
            return Json(msg);
        }

        /// <summary>
        /// 修该会员常规信息
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateMember()
        {
            JsonSMsg msg = new JsonSMsg();
            //OpendID
            string OpendID = Request["OpendID"];

            //性
            string NAME_LAST = Request["NAME_LAST"];

            //名称
            string NAME_FIRST = Request["NAME_FIRST"];

            //性别
            string Gender = Request["Gender"];

            //地址
            string Address = Request["Address"];

            //修该密码或者修该手机号
            int cout = _custMember.UpdateMember(NAME_LAST, NAME_FIRST, Gender, Address, OpendID);

            if (cout == 2)
            {
                msg.Status = 0;
                msg.Message = "修该失败，查看日志";
                return Json(msg);
            }

            msg.Status = 1;
            msg.Message = "成功";
            return Json(msg);
        }

        /// <summary>
        /// 显示会员制度页面
        /// </summary>
        /// <returns></returns>
        [Outh(true)]
        public ActionResult ShowMemberRule()
        {
            FansInfo Fans = Session["FansInfo"] as FansInfo;

            if (!_custMember.ChckBind(Fans.Openid))
            {
                //如果该粉丝已经绑定了会员，则直接进入到会员详情界面
                return Redirect("Index.do");
            }

            return View();
        }

        /// <summary>
        /// 显示积分兑礼页面
        /// </summary>
        /// <returns></returns>
        [Outh(true)]
        public ActionResult ShowPointToGift()
        {
            FansInfo Fans = Session["FansInfo"] as FansInfo;

            if (!_custMember.ChckBind(Fans.Openid))
            {
                //如果该粉丝已经绑定了会员，则直接进入到会员详情界面
                return Redirect("Index.do");
            }

            return View();
        }

        /// <summary>
        /// 显示积分历史记录
        /// </summary>
        /// <returns></returns>
        [Outh(true)]
        public ActionResult ShowPointHistory()
        {
            FansInfo Fans = Session["FansInfo"] as FansInfo;

            PointHistory dyn = _custMember.GetPointHistory(Fans.Openid);
            

            ViewBag.History = dyn;
            return View();
        }

        /// <summary>
        /// 显示优惠券信息
        /// </summary>
        /// <returns></returns>
        [Outh(true)]
        public ActionResult ShowCoupon()
        {
            FansInfo Fans = Session["FansInfo"] as FansInfo;

            if (!_custMember.ChckBind(Fans.Openid))
            {
                //如果该粉丝已经绑定了会员，则直接进入到会员详情界面
                return Redirect("Index.do");
            }

            MEMBER_COUPON dyn = _custMember.GetCoupon(Fans.Openid);

            return View(dyn);
        }

        /// <summary>
        /// 显示核销信息
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowQrHx()
        {
            string ZCP_NUM = Request["ZCP_NUM"];

            string ZCP_YHQDES = Request["ZCP_YHQDES"];

            string BPEXT = Request["BPEXT"];

            string ZCP_EDATE = Request["ZCP_EDATE"];

            string ZCP_YHQ = Request["ZCP_YHQ"];

            string ZCP_BDATE = Request["ZCP_BDATE"];

            ViewBag.ZCP_NUM = ZCP_NUM;
            ViewBag.ZCP_YHQDES = ZCP_YHQDES;
            ViewBag.BPEXT = BPEXT;
            ViewBag.ZCP_EDATE = ZCP_EDATE;

            ViewBag.ZCP_BDATE = ZCP_BDATE;
            ViewBag.ZCP_YHQ = ZCP_YHQ;

            return View();
        }

        /// <summary>
        /// 显示会员权益
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowMemberLvlDetail()
        {
            string lvl = Request["lvl"];

            string str = string.Empty;

            switch (lvl)
            {
                case "普通会员":
                    str = @"倾心卡会员、知心卡会员降级后，都会直接成为普通会员。<br/>
	普通会员无限定的等级效期，当普通会员再次购买，即可重新激活倾心卡会员等级。";
                    break;

                case "倾心卡会员":
                    str = @"在佰草集品牌线下专卖店或专柜购买任意产品，并使用本人有效信息在门店或微信完成注册，即可成为倾心卡会员。<br/>
	倾心卡会员的等级有效期为6个月，效期内未升级到知心卡即降级为普通会员。<br/>
	普通会员再次购买任意产品，即可再次成为倾心卡会员。";
                    break;

                case "臻心卡会员":
                    str = @"	知心卡会员在其等级效期内，累计购买任意产品满3000元，且购物2次以上，即可成为臻心卡会员。<br/>
	臻心卡会员的等级效期为12个月，从升级成为臻心卡之日开始计算，至12个月后的月底。<br/>
	臻心卡会员在其等级效期内累计购买金额满3000元，且购物2次以上，可自动保持其臻心卡会员资格12个月。<br/>
	臻心卡会员在其等级效期内，累计购买金额不满3000元，或累计购买金额满3000元，但购物次数小于2次，在效期结束时直接降级为知心卡会员。";
                    break;

                case "知心卡会员":
                    str = @"	倾心卡会员自首笔购买后，在其等级效期内累计购买任意产品满800元，即可成为知心卡会员。（包含成为倾心卡会员的首笔购买）<br/> 
	知心卡会员的等级效期为12个月，从升级至知心卡之日开始计算，至12个月后的月底。<br/>
	知心卡会员在其等级效期内，再次累计购买金额满800元，则自动续会12个月。<br/>
	知心卡会员在其等级效期内，累计购买金额不满800元，在效期结束时直接降级为普通册会员。";
                    break;

                case "注册会员":
                    str = @"&nbsp;&nbsp;&nbsp;&nbsp;倾心卡会员、知心卡会员降级后，都会直接成为注册会员。<br/>
	&nbsp;&nbsp;&nbsp;&nbsp;注册会员无限定的等级效期，当注册会员再次入店产生首笔购买，即可重新激活倾心卡会员等级。";
                    break;

                default:
                    str = "未知卡类型";
                    break;
            }


            ViewBag.LVL = str;
            return View();
        }

    }
}
