using HmjNew.Service;
using oldService = Hmj.WebService;
using Hmj.Common;
using Hmj.DataAccess;
using Hmj.DataAccess.Repository;
using Hmj.DTO;
using Hmj.Entity;
using Hmj.Entity.PageSearch;
using Hmj.Interface;
using System;
using System.Collections.Generic;
using Hmj.Entity.Entities;
using Newtonsoft.Json;
using Hmj.Entity.WxModel;
using WeChatCRM.Common.Utils;
using System.Text;
using Hmj.Common.Utils;
using log4net;
using System.Threading;

namespace Hmj.Business.ServiceImpl
{
    public class HmjMemberService : IHmjMemberService
    {

        private static ILog _logerror = LogManager.GetLogger("logerror");

        //新的业务
        HmjMemberRepository _hmjMember;

        //老的业务
        private CustMemberRepository _oldMember;

        public HmjMemberService()
        {
            _hmjMember = new HmjMemberRepository();
            _oldMember = new CustMemberRepository();
        }

        /// <summary>
        /// 更新会员基本信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string UpdateMember(MemberUpdateReqDTO request)
        {
            //查询会员主数据信息
            CUST_MEMBER member = _oldMember.GetMemberByOpenId(request.OPENID);

            dt_Dyn_UpdateMemberShip_req updates = new dt_Dyn_UpdateMemberShip_req();
            ZCRMT322_Dyn z = new ZCRMT322_Dyn();

            z.DATA_SOURCE = AppConfig.DATA_SOURCE;
            z.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
            z.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
            z.VGROUP = AppConfig.VGROUP; //销售组织

            z.NAME_LAST = request.NAME_LAST;
            z.NAME_FIRST = request.NAME_FIRST;
            z.NAME1_TEXT = request.NAME_LAST + request.NAME_FIRST;
            z.XSEX = request.GENDER == "0" ? "1" : "2";
            //z.PSTREET = address;

            if (!string.IsNullOrEmpty(request.MOBILE))
            {
                z.MOB_NUMBER = request.MOBILE;
            }

            z.BIRTHDT = request.BIRTHDAY;
            z.ACCOUNT_ID = member.MEMBERNO;
            z.PARTNER = member.PARTNER;
            updates.ZCRMT316 = new ZCRMT322_Dyn[] { z };

            dt_Dyn_UpdateMemberShip_res ups = WebHmjApiHelp.UpdateMemberShip(updates);

            //修改成功
            if (ups.WV_RETURN == "Y")
            {
                //
                _hmjMember.UpdateNick(request.OPENID, request.NICK_NAME);
                return "1";
            }
            else
            {
                return ups.WV_MESSAGE;
            }
        }

        /// <summary>
        /// 得到集合
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<MemberResDTO> GetMemberList(MemberSearch search)
        {
            List<CUST_MEMBER> members = _hmjMember.GetMemberList(search);

            return members.MapToList<MemberResDTO>();
        }

        /// <summary>
        /// 得到会员详情
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public HmjMemberDetail GetMemberDetailByOpenId(string openid)
        {
            return _hmjMember.GetMemberDetailByOpenId(openid);
        }

        /// <summary>
        /// 根据会员id得到会员详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MemberResDTO GetMemberModel(string id)
        {
            CUST_MEMBER member = _hmjMember.GetMemberModel(id);

            return member.MapTo<MemberResDTO>();
        }

        /// <summary>
        /// 华美家绑定会员
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public string Binding(BindReqDTO bindmodel)
        {
            using (TransScope scope = new TransScope())
            {
                try
                {
                    //第一步检查是否存在该粉丝
                    WXCUST_FANS fans = _oldMember.GetFans(bindmodel.OPENID);

                    //不存在该粉丝
                    if (fans == null)
                    {
                        return "3";
                    }

                    //第二步查询会员
                    CUST_MEMBER member = _oldMember.GetMemberByMobile(bindmodel.MOBILE);

                    //如果存在会员就说明已经存在绑定信息
                    if (member != null)
                    {
                        return "2";
                    }

                    //调用接口查询并插入数据库
                    string flg = InsertMember(fans.IS_REGISTER, fans.ID, bindmodel);
                    scope.Commit();
                    //成功返回1
                    return flg;
                }
                catch (Exception ex)
                {
                    scope.Rollback();
                    throw new Exception();
                }
            }
        }

        /// <summary>
        /// 是否绑定
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public bool ChckBind(string openid)
        {
            CUST_MEMBER member = _oldMember.GetMemberByOpenId(openid);

            return member != null;
        }

        /// <summary>
        /// 更新本地数据库信息并拿到会员信息
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public MemberDetailResDTO GetMemberDetail(string openid, string isupdate)
        {
            //查询会员主数据信息
            HmjMemberDetail member = _hmjMember.GetMemberDetailByOpenId(openid);

            if (member == null)
            {
                return null;
            }

            if (member.IMAGE != null && !member.IMAGE.StartsWith("http"))
            {
                member.IMAGE = AppConfig.BeautyChinaUrl + member.IMAGE;
            }

            int? AVA_POINTS = 0;
            string ZTIER = "";
            string NAME_LAST = member.NAME_LAST;
            string NAME_FIRST = member.NAME_FIRST;

            if (isupdate == "1")
            {
                #region 得到最新会员信息,并更新本地信息
                //接口查询会员主数据
                dt_Dyn_DispMember_req w = new dt_Dyn_DispMember_req();
                w.DATA_SOURCE = AppConfig.DATA_SOURCE;
                w.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
                w.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
                w.VGROUP = AppConfig.VGROUP; //销售组织
                w.PARTNER = member.PARTNER;//会员唯一标示
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
                NAME_FIRST = newmeber.NAME_FIRST;
                NAME_LAST = newmeber.NAME_LAST;
                AVA_POINTS = (int?)newmeber.ZCCUR_POINT;
                ZTIER = newmeber.ZTIER;

                var ZPASS = "";
                #region 已激活状态需查询兑礼密码
                if (newmeber.ZZAFLD000004 == "E0001")
                {
                    oldService.dt_Dyn_ChangeMemberStatus_req req = new oldService.dt_Dyn_ChangeMemberStatus_req
                    {
                        DATA_SOURCE = AppConfig.DATA_SOURCE,
                        ZVTWEG = "102",//来源渠道 102表示微信
                        ACCOUNT_ID = newmeber.MOB_NUMBER,
                        FLAG = "Q",//查询
                        LOYALTY_BRAND = AppConfig.LOYALTY_BRAND,//忠诚度品牌
                        SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM,//来源系统
                        VGROUP = AppConfig.VGROUP, //销售组织
                        PASS_FLAG = "Y",
                        REASON = "查询密码"
                    };
                    oldService.dt_Dyn_ChangeMemberStatus_res res = WebApiHelp.ChangeMemberStatus(req);
                    ZPASS = res.ZPASS;
                }
                #endregion

                CUST_MEMBER mebers = new CUST_MEMBER()
                {
                    ID = member.ID,
                    ADDRESS = newmeber.PSTREET,
                    AVA_POINTS = (int?)newmeber.ZCCUR_POINT,
                    BIRTHDAY = string.IsNullOrEmpty(newmeber.BIRTHDT) ? null :
                   (DateTime?)DateTime.Parse(newmeber.BIRTHDT),
                    GENDER = newmeber.XSEX == "2" ? true : false,
                    MEMBERNO = newmeber.ACCOUNT_ID,
                    MOBILE = newmeber.MOB_NUMBER,
                    MEM_LEVEL = newmeber.ZTIER,
                    NAME = newmeber.NAME1_TEXT,
                    PARTNER = newmeber.PARTNER,
                    STORE = newmeber.ZH003,
                    LOGINPASSON = ZPASS,
                    NAME_FIRST = newmeber.NAME_FIRST,
                    NAME_LAST = newmeber.NAME_LAST,
                    ZZAFLD000004 = newmeber.ZZAFLD000004
                };

                //更新本地会员信息
                int count = _oldMember.UpdateMember(mebers);
                #endregion
            }
            //会员等级（如果数据库信息有误，则读取程序中的配置文件）
            string m_level = "";
            MEMBER_LVL lvlmodel = _hmjMember.GetHmjSelfLvl(ZTIER);
            if (lvlmodel == null)
            {
                m_level = Utility.GetMemberLvl(ZTIER);
            }
            else
                m_level = lvlmodel.HMJ_NAME;

            return new MemberDetailResDTO()
            {
                AVA_POINTS = AVA_POINTS,
                NICK_NAME = member.NICK_NAME,
                MEM_LEVEL = m_level,
                IMAGE = member.IMAGE,
                MOBILE = member.MOBILE,
                NAME_LAST = NAME_LAST,
                NAME_FIRST = NAME_FIRST,
                GENDER = member.GENDER.HasValue ? member.GENDER.Value ? "1" : "0" : "1",
                BIRTHDAY = member.BIRTHDAY.HasValue ?
                member.BIRTHDAY.Value.ToString("yyyy-MM-dd") : "",
                PARTNER = member.PARTNER,
                IS_SEND = string.IsNullOrEmpty(member.IS_SEND) ? "0" : member.IS_SEND,
                LOGINPASSON = member.LOGINPASSON,
                ZZAFLD000004 = Utility.GetMemberState(member.ZZAFLD000004),
                MEMBERNO = member.MEMBERNO
            };
        }

        /// <summary>
        /// 更新粉丝头像
        /// </summary>
        /// <param name="valtr"></param>
        /// <param name="fans_id"></param>
        /// <returns></returns>
        public string UpdateImageUrl(string valtr, string openid)
        {
            return _hmjMember.UpdateImageUrl(valtr, openid);
        }

        /// <summary>
        /// 注册会员
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string RegisterMember(MemberRegisterReqDTO request, ref string msg)
        {
            WXCUST_FANS fans = _oldMember.GetFans(request.OPENID);

            //没有该粉丝
            if (fans == null)
            {
                msg = "对不起，查无该粉丝，请重新关注本公众号";
                return "-1";
            }


            #region 调用接口通知crm创建会员
            dt_Dyn_CreateHMJMemberShip_req req = new dt_Dyn_CreateHMJMemberShip_req();
            ZCRMT316_HMJ meber = new ZCRMT316_HMJ();
            meber.MOB_NUMBER = request.MOBILE;
            meber.OPENID = request.OPENID;
            meber.NAME1_TEXT = request.NAME_LAST.Trim() + request.NAME_FIRST.Trim();//全名
            meber.DATA_SOURCE = AppConfig.DATA_SOURCE;
            meber.ACCOUNT_ID = request.MOBILE;
            meber.NAME_LAST = request.NAME_LAST.Trim();
            meber.NAME_FIRST = request.NAME_FIRST.Trim();
            meber.XSEX = request.GENDER == "1" ? "2" : "1";
            meber.RE_BPEXT = request.REFEREE_MOBILE;
            meber.BIRTHDT = request.BIRTHDAY;
            meber.NAMCOUNTRY = "CN";
            meber.WECHATNAME = request.NICK_NAME;
            meber.WECHATFOLLOWSTATUS = "1";
            //meber.RE_BPEXT = "";
            //meber.LOGINPASS2 = "111111";//兑换密码默认123456
            meber.DATA_SOURCE = AppConfig.DATA_SOURCE;//数据来源
            meber.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
            meber.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
            meber.VGROUP = AppConfig.VGROUP; //销售组织
            meber.CHANNEL_SOURCE = AppConfig.CHANNEL_SOURCE;//渠道来源
            meber.BRAND_SOURCE = AppConfig.BRAND_SOURCE;//品牌来源

            //固定死
            meber.EMPID = AppConfig.EMPID;
            meber.DEPTID = AppConfig.DEPTID;

            //查询客人最近一次扫码门店编码
            WXCustScanRecordEx scanRecord = _oldMember.GetLastWXCustScanRecord(request.OPENID);
            if (scanRecord != null)
            {
                meber.DEPTID = scanRecord.EVENTKEY;//更新入会门店
                meber.VGROUP = scanRecord.vgroup;
                meber.BRAND_SOURCE = scanRecord.source;//"01"佰草集
            }


            req.ZCRMT316 = new ZCRMT316_HMJ[] { meber };

            //创建会员
            dt_Dyn_CreateHMJMemberShip_res res = WebHmjApiHelp.CreateMemberShip(req);
            #endregion

            if (res.WV_RETURN == "N")
            {
                msg = res.WV_MESSAGE;
                return "-1";
            }

            //创建成功
            if (res.WV_RETURN == "Y")
            {
                //创建本地会员
                CUST_MEMBER member = new CUST_MEMBER()
                {
                    BIRTHDAY = DateTime.Parse(request.BIRTHDAY),
                    FANS_ID = fans.ID,
                    MEMBERNO = res.ACCOUNT_ID,
                    MOBILE = request.MOBILE,
                    NAME = request.NAME_LAST + request.NAME_FIRST,
                    STATUS = 1,
                    STORE = AppConfig.DEPTID,
                    TYPE = 0,
                    //LOGINPASSON = "111111",
                    CREATE_DATE = DateTime.Now,
                    AVA_POINTS = 0,
                    PARTNER = res.PARTNER,
                    GENDER = false,
                    IS_SEND_TMP = 0
                    //REMARK = remak
                };

                if (scanRecord != null)
                    member.STORE = scanRecord.EVENTKEY;//更新入会门店

                double counts = _oldMember.Insert(member);

                if (counts <= 0)
                {
                    return $"请关掉页面并用手机号：{request.MOBILE}，绑定!";
                }
                else
                {
                    ThreadPool.QueueUserWorkItem(a =>
                    {
                        #region 注册送积分
                        bool _sendResult = Send(request.MOBILE, AppConfig.BindMemberSend);
                        #endregion

                        //更改tag
                        FansTag(request.OPENID);

                        #region 注册成功发送模板消息通知
                        if (_sendResult)
                        {
                            var openid = request.OPENID;
                            var tempId = "PC5Va36GwaMzhuWAf-ZZNlGBEmrTNLkVLiN-y6S6NwI";
                            var redirect_url = AppConfig.HmjWebApp + "assets/hmjweixin/html/hytq.html";
                            var p1 = "欢迎您加入华美家！";
                            var p2 = "首次注册奖励" + AppConfig.BindMemberSend.Split('|')[0] + "积分";
                            var p3 = DateTime.Now.ToString("yyyy年MM月dd日");
                            var p4 = "今后您可获得更多积分、享受更多优惠。了解更多会员权益，点击查看 >";
                            SendTmpPublicFunc(true, openid, tempId, redirect_url, p1, p2, p3, p4);
                        }
                        #endregion
                    });

                }
            }

            return "1";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isreal_time"></param>
        /// <param name="param">1openid,2templateId,3redirecturl,4first,5keywords1,6keywords2,....</param>
        public void SendTmpPublicFunc(bool isreal_time, params string[] param)
        {
            BCJ_TMP_DETAIL myrequest = new BCJ_TMP_DETAIL()
            {
                Contact_Information = param[0],
                Template_Code = param[1],
                Redirect_Url = param[2],
                IsRealTime = isreal_time,
                Invoke_Time = DateTime.Now
            };

            for (int i = 0; i < param.Length; i++)
            {
                switch (i)
                {
                    case 3:
                        myrequest.P_1 = param[3];
                        break;
                    case 4:
                        myrequest.P_2 = param[4];
                        break;
                    case 5:
                        myrequest.P_3 = param[5];
                        break;
                    case 6:
                        myrequest.P_4 = param[6];
                        break;
                    case 7:
                        myrequest.P_5 = param[7];
                        break;
                    case 8:
                        myrequest.P_6 = param[8];
                        break;
                }
            }


            string resmsg = NetHelper.HttpRequest(AppConfig.BeautyChinaWebApp, "", "GET", 2000,
                    Encoding.UTF8, "application/json");

            TokeRes toke = JsonHelper.DeserializeObject<TokeRes>(resmsg);

            SendTmp(myrequest, toke.Access_Token);
        }

        /// <summary>
        /// CRM-绑定关系同步接口
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public BindingRelShipResDTO BindingRelShip(string OPENID, string BRAND_CODE, string POIT, ref string REG_DATE)
        {
            using (TransScope scope = new TransScope())
            {
                try
                {
                    CUST_MEMBER MEMBER = _oldMember.GetMemberByOpenId(OPENID);

                    if (MEMBER == null)
                    {
                        return null;
                    }

                    dt_Dyn_DispMemQuick_req w = new dt_Dyn_DispMemQuick_req();
                    w.DATA_SOURCE = AppConfig.DATA_SOURCE;
                    w.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
                    w.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
                    w.VGROUP = AppConfig.VGROUP; //销售组织
                    w.BP = MEMBER.PARTNER;//2002652891
                    dt_Dyn_DispMemQuick_res dt = WebHmjApiHelp.DispMemQuick(w);
                    if (dt.I_ZCRMT316 == null || dt.I_ZCRMT316.Length <= 0)
                    {
                        return null;
                    }
                    if (dt.I_ZCRMT316.Length > 1)
                    {
                        return null;
                    }

                    #region 调用绑定关系同步接口
                    //调用
                    dt_DynMemberBunding_req req = new dt_DynMemberBunding_req();
                    req.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
                    req.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
                    req.VGROUP = AppConfig.VGROUP; //销售组织
                    req.DATA_SOURCE = AppConfig.DATA_SOURCE;//数据来源
                    req.MEMBER_ID = MEMBER.PARTNER;// "MCHM000000012";

                    string[] str = BRAND_CODE.Split(new char[] { ',' },
                        StringSplitOptions.RemoveEmptyEntries);

                    dt_DynMemberBunding_reqITEM[] arry = new dt_DynMemberBunding_reqITEM[str.Length];


                    for (int i = 0; i < arry.Length; i++)
                    {
                        string[] codes = str[i].Split(new char[] { '-' },
                        StringSplitOptions.RemoveEmptyEntries);

                        EXCHANGE_RATE rate = _hmjMember.GetRateByBreadCode(codes[0]);

                        dt_DynMemberBunding_reqITEM items = new dt_DynMemberBunding_reqITEM()
                        {
                            DATA_SOURCE2 = rate.DATA_SOURCE,
                            LOYALTY_BRAND2 = rate.LOYALTY_BRAND,
                            MEMBER_ID2 = codes[2],
                            VGROUP2 = codes[0]
                        };

                        arry[i] = items;
                    }

                    req.BRANDMEMBER = arry;

                    dt_DynMemberBunding_res res = WebHmjApiHelp.DynMemberBunding(req);
                    #endregion

                    //如果转换成功 就发 送积分
                    if (res.ZRETURN == "Y")
                    {
                        #region  转换成功，将状态更新为激活
                        dt_Dyn_DispMemQuick_resITEM newmeber = dt.I_ZCRMT316[0];
                        //如果是待激活状态那么就要激活
                        if (newmeber.ZZAFLD000004 == "E0005")
                        {
                            //激活
                            this.ChageSatus(newmeber.ACCOUNT_ID);
                        }
                        #endregion

                        var _sendResult = Send(newmeber.ACCOUNT_ID, AppConfig.BindBrandSend);

                        //接口查询会员主数据
                        dt_Dyn_DispMember_req ws = new dt_Dyn_DispMember_req();
                        ws.DATA_SOURCE = AppConfig.DATA_SOURCE;
                        ws.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
                        ws.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
                        ws.VGROUP = AppConfig.VGROUP; //销售组织
                        ws.PARTNER = newmeber.PARTNER;//会员唯一标示
                        dt_Dyn_DispMember_res dts = WebHmjApiHelp.DispMember(ws);

                        if (dts.ZCRMT316 == null || dts.ZCRMT316.Length <= 0)
                        {
                            return null;
                        }

                        if (dts.ZCRMT316.Length > 1)
                        {
                            return null;
                        }

                        ZCRMT302_Dyn newmebers = dts.ZCRMT316[0];
                        REG_DATE = newmebers.REG_DATE;
                        #region 绑定成功发送模板消息
                        if (_sendResult)
                        {
                            var openid = OPENID;
                            var tempId = "nJqLvWytZJ2IUBdOapJ3RQcJjD3Zvt0UYdvQ1A-5IQ8";
                            var redirect_url = AppConfig.HmjWebApp + "assets/hmjweixin/html/hytq.html";
                            var p1 = "恭喜您成为华美家" + Utility.GetMemberLvl(newmebers.ZTIER) + "，并成功转换积分！";
                            var p2 = DateTime.Now.ToString("yyyy年MM月dd日");
                            var p3 = AppConfig.BindBrandSend.Split('|')[0];
                            var p4 = "首次转换品牌积分";
                            var p5 = newmebers.ZCCUR_POINT.ToString();
                            var p6 = "今后您可获得更多积分、享受更多优惠。了解更多会员权益，点击查看 >";
                            SendTmpPublicFunc(true, openid, tempId, redirect_url, p1, p2, p3, p4, p5, p6);
                        }
                        #endregion
                    }

                    return res.MapTo<BindingRelShipResDTO>();
                }
                catch (Exception ex)
                {
                    scope.Rollback();
                    throw new Exception();
                }
            }
        }

        /// <summary>
        /// 积分操作
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        private static bool SendPoit(string LOYALTY_BRAND, string SOURCE_SYSTEM, string VGROUP,
           string DATA_SOURCE, string POINTS, string type, string mobile)
        {
            dt_Dyn_ActCreateTel_req Actreq = new dt_Dyn_ActCreateTel_req();
            //处理标识 
            Actreq.OBJECT_ID = DateTime.Now.ToString("yyyyMMddHHmmss") + CommonHelp.GetRand(10); ;
            //活动单据类型	
            Actreq.PROCESS_TYPE = type;// "ZJS";//减少积分
                                       //外部会员卡号	"会员卡号(如已传输手机号，可不用填写会员卡号)"
            Actreq.ACCOUNT_ID = mobile;
            Actreq.POSTING_DATE = DateTime.Today.ToString();
            //积分类型	
            Actreq.POINT_TYPE = "ZBCJF01";
            //积分数  要改
            Actreq.POINTS = decimal.Parse(POINTS);
            //单据全局活动ID
            Actreq.CAMPAIGN_HE_ID = "C-00001786";
            Actreq.LOYALTY_BRAND = LOYALTY_BRAND;//AppConfig.LOYALTY_BRAND;//忠诚度品牌
            Actreq.SOURCE_SYSTEM = SOURCE_SYSTEM;//AppConfig.SOURCE_SYSTEM;//来源系统
            Actreq.VGROUP = VGROUP;//AppConfig.VGROUP; //销售组织
            Actreq.TYPE = DATA_SOURCE;//AppConfig.DATA_SOURCE;// 数据源类型
            dt_Dyn_ActCreateTel_res res = WebHmjApiHelp.ActCreateTel(Actreq);

            dt_Dyn_ActCreateTel_resItem item = new dt_Dyn_ActCreateTel_resItem();
            if (res != null)
            {
                item = res.I_ZCRMT047_dyn[0];
            }

            return item.CHECK_FLAG == "Y";
        }

        /// <summary>
        /// CRM-会员绑定查询品牌会员接口
        /// </summary>
        /// <param name="mobile">不为空时，使用此字段查询会员信息</param>
        /// <returns></returns>
        public QueryMemberShipBindingResDTO QueryMemberShipBinding(string openID, string mobile = "")
        {
            using (TransScope scope = new TransScope())
            {
                try
                {
                    CUST_MEMBER MEMBER = new CUST_MEMBER();
                    if (!string.IsNullOrEmpty(mobile))
                    {
                        MEMBER = _oldMember.GetMemberByMobile(mobile);
                    }
                    else
                        MEMBER = _oldMember.GetMemberByOpenId(openID);

                    if (MEMBER == null)
                    {
                        return null;
                    }

                    dt_Dyn_QueryMemberShipBinding_req req = new dt_Dyn_QueryMemberShipBinding_req();
                    req.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
                    req.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
                    req.VGROUP = AppConfig.VGROUP; //销售组织
                    req.DATA_SOURCE = AppConfig.DATA_SOURCE;//数据来源
                    req.MOB_NUMBER = MEMBER.MOBILE;

                    dt_Dyn_QueryMemberShipBinding_reqITEM[] item = new dt_Dyn_QueryMemberShipBinding_reqITEM[]
                    {
                        //佰草集
                        new dt_Dyn_QueryMemberShipBinding_reqITEM()
                        {
                             DATA_SOURCE2="0002",
                             LOYALTY_BRAND2="28",
                             VGROUP2="C004"
                        },
                        //高夫
                        new dt_Dyn_QueryMemberShipBinding_reqITEM()
                        {
                             DATA_SOURCE2="0006",
                             LOYALTY_BRAND2="30",
                             VGROUP2="C003"
                        }
                    };
                    req.BRANDLIST = item;
                    QueryMemberShipBindingResDTO detail = new QueryMemberShipBindingResDTO
                    {
                        BRAND_LIST = new List<QueryMemberBindDetailResDTO>()
                    };

                    dt_Dyn_QueryMemberShipBinding_res res =
                        WebHmjApiHelp.QueryMemberShipBinding(req);

                    detail.IS_BRADN = res.BRANDLIST == null ? 0 : res.BRANDLIST.Length <= 0 ? 0 : 1;//当res为null时，出现异常

                    if (res != null && res.ZRETURN == "Y")
                    {
                        string lvl = string.Empty;
                        string codes = string.Empty;
                        foreach (dt_Dyn_QueryMemberShipBinding_resITEM model in res.BRANDLIST)
                        {
                            QueryMemberBindDetailResDTO mymodel =
                                model.MapTo<QueryMemberBindDetailResDTO>();

                            if (mymodel != null)
                            {
                                EXCHANGE_RATE rate = _hmjMember.GetRateByBreadCode(mymodel.VGROUP2);

                                if (rate != null)
                                {
                                    if (mymodel.IF_BINDING == "0")
                                    {
                                        detail.POINT_ALL += model.POINT_AVAILABLE * (rate.RATE.HasValue ? rate.RATE.Value : 1);
                                        mymodel.POINT_DETAIL = (model.POINT_AVAILABLE * (rate.RATE.HasValue ? rate.RATE.Value : 1)).ToString();
                                    }
                                    mymodel.BRAND_NAME = rate.BRAND_NAME;

                                    lvl += "'" + mymodel.VGROUP2 + "',";
                                    codes += "'" + mymodel.TIER_CODE + "',";
                                }

                                detail.BRAND_LIST.Add(mymodel);
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(lvl))
                        {
                            //计算得到等级
                            MEMBER_LVL lvlmodel = _hmjMember.GetHmjLvl(lvl.Trim(','), codes.Trim(','));

                            if (lvlmodel != null)
                            {
                                detail.LVL_CODE = lvlmodel.HMJ_LVL;
                                detail.LVL_NAME = lvlmodel.HMJ_NAME;
                            }
                        }
                    }
                    //else
                    //{
                    //     ILog logger = LogManager.GetLogger("loginfo");
                    //     logger.Info("会员绑定查询品牌会员接口si_Dyn_QueryMemberShipBinding_obService无响应,无法判断是否有品牌信息，手机号:" + MEMBER.MOBILE);
                    //}

                    return detail;
                }
                catch (Exception ex)
                {
                    scope.Rollback();
                    throw new Exception();
                }
            }
        }

        /// <summary>
        /// 完善信息送积分
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool SendPoint(MemberExtendDTO request)
        {
            bool bl = false;
            //如果信息都填充，送积分
            if (!string.IsNullOrEmpty(request.BRAND_PREF) &&
                !string.IsNullOrEmpty(request.CLASS_PREF) && !string.IsNullOrEmpty(request.INFO_WANTED)
                && !string.IsNullOrEmpty(request.PARTNER)
                && !string.IsNullOrEmpty(request.TRANSPZONE) &&
                !string.IsNullOrEmpty(request.ZA003) && !string.IsNullOrEmpty(request.ZA004)
                && !string.IsNullOrEmpty(request.ZC004) && !string.IsNullOrEmpty(request.ZC016) &&
                !string.IsNullOrEmpty(request.ZC019))
            {

                var needSend = false;

                //如果已婚已育，则大宝生日必填
                if (request.ZC016 == "02" && !string.IsNullOrEmpty(request.KID_BIRTHDAY))
                {
                    needSend = true;
                }
                else if (request.ZC016 != "02")
                {
                    needSend = true;
                }

                if (needSend)
                {
                    //得到会员信息
                    CUST_MEMBER member = _hmjMember.GetMemberByBP(request.PARTNER);

                    MEMBER_MSG msg = _hmjMember.GetMsgByBP(member.ID);

                    //未发送
                    if (msg == null)
                    {
                        bool ok = Send(member.MEMBERNO, AppConfig.MemberSend);
                        if (ok)
                        {
                            MEMBER_MSG models = new MEMBER_MSG()
                            {
                                IS_SEND = 1,
                                MEMBER_ID = member.ID,
                                STATUS = 1,
                                REMARK = "发送成功"

                            };
                            _hmjMember.Insert(models);
                        }
                        bl = true;
                    }
                }
            }

            return bl;
        }

        /// <summary>
        /// 得到会员详细信息
        /// </summary>
        /// <param name="oPENOD"></param>
        /// <returns></returns>
        public string GetMemberMobileByOpenId(string oPENOD)
        {
            CUST_MEMBER MEMBER = _oldMember.GetMemberByOpenId(oPENOD);

            if (MEMBER == null || string.IsNullOrEmpty(MEMBER.MEMBERNO))
            {
                return "";
            }

            return MEMBER.MEMBERNO;
        }

        /// <summary>
        /// 根据BP号得到会员手机号
        /// </summary>
        /// <param name="pARTNER"></param>
        /// <returns></returns>
        public string GetMemberMobileByBP(string pARTNER)
        {
            CUST_MEMBER MEMBER = _hmjMember.GetMemberByBP(pARTNER);

            if (MEMBER == null || string.IsNullOrEmpty(MEMBER.MEMBERNO))
            {
                return "";
            }

            return MEMBER.MEMBERNO;
        }

        #region 模板消息发送

        /// <summary>
        /// 模板消息发送(参数全部由外部传入)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="v"></param>
        /// <param name="isHmj">是否是华美家微信平台自己调用，是的话，部分参数将直接取值于配置文件</param>
        /// <returns></returns>
        public int SendTmp(BCJ_TMP_DETAIL request, string access_token, bool isHmj = true)
        {
            //得到配置文件
            List<WX_TMP_CONFIG> config = _hmjMember.GetTmps(request.Template_Code);

            if (config == null || config.Count <= 0)
            {
                return -1;
            }

            Dictionary<string, TemplateData> dic = CommonHelp.GetTmpPar(request, config);

            TemplateSend tmp = new TemplateSend()
            {
                Url = request.Redirect_Url,
                Template_Id = request.Template_Code,
                Touser = request.Contact_Information,
                Data = dic
            };

            string tmpStr = JsonConvert.SerializeObject(tmp);

            bool issend = false;
            bool isselect = false;
            string resot = string.Empty;
            //发送时间
            DateTime dt = DateTime.Now;
            //如果是实时接口就调用发送模板
            if (request.IsRealTime)
            {
                isselect = true;
                resot = NetHelper.HttpRequest("https://api.weixin.qq.com/cgi-bin/message/template/send?access_token="
                    + access_token, tmpStr, "POST", 2000, Encoding.UTF8, "application/json");

                dt = DateTime.Now;

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
                SEND_TIME = dt,
                VGROUP = request.Vgroup,
                SOURCE_SYSTEM = request.Data_source,
                TMP_ID = request.Template_Code,
                IS_SELECT = isselect,
                SEND_MSG = resot
            };

            if (isHmj)
            {
                log.VGROUP = AppConfig.Get("VGROUP");
                log.LOYALTY_BRAND = AppConfig.Get("LOYALTY_BRAND");
                log.DATA_SOURCE = AppConfig.Get("DATA_SOURCE");
                log.SOURCE_SYSTEM = AppConfig.Get("SOURCE_SYSTEM");

            }

            long count = _hmjMember.Insert(log);

            return 1;
        }


        /// <summary>
        /// 模板消息发送(参数全部由外部传入)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="v"></param>
        /// <param name="isHmj">是否是华美家微信平台自己调用，是的话，部分参数将直接取值于配置文件</param>
        /// <returns></returns>
        public int SendTmpEx(BCJ_TMP_DETAIL_EX request, string access_token, ref int successCnt, ref string errorMessage, bool isHmj = true)
        {

            foreach (Template template in request.Templates)
            {
                //得到配置文件
                List<WX_TMP_CONFIG> config = _hmjMember.GetTmps(template.Template_Code);

                if (config == null || config.Count <= 0)
                {
                    errorMessage = "没有该模板" + template.Template_Code + "，请查看模板ID";
                    return -1;
                }

                foreach (Template_Param temp_param in request.Template_Params)
                {

                    Dictionary<string, TemplateData> dic = CommonHelp.GetTmpParEx(temp_param, config, template.Template_Code);

                    TemplateSend tmp = new TemplateSend()
                    {
                        Url = temp_param.Redirect_Url,
                        Template_Id = template.Template_Code,
                        Touser = temp_param.Contact_Information,
                        Data = dic
                    };

                    string tmpStr = JsonConvert.SerializeObject(tmp);
                    tmpStr = tmpStr.Replace("\\\\", "\\");
                    bool issend = false;
                    bool isselect = false;
                    string resot = string.Empty;
                    //发送时间
                    DateTime dt = DateTime.Now;
                    //如果是实时接口就调用发送模板
                    if (request.IsRealTime == "1")
                    {
                        isselect = true;
                        resot = NetHelper.HttpRequest("https://api.weixin.qq.com/cgi-bin/message/template/send?access_token="
                            + access_token, tmpStr, "POST", 2000, Encoding.UTF8, "application/json");

                        dt = DateTime.Now;

                        //如果发送成功
                        if (resot.Contains("ok"))
                        {
                            issend = true;
                            successCnt++;
                        }
                    }

                    //记录发送的
                    WX_TMP_HIS log = new WX_TMP_HIS()
                    {
                        DETAIL = tmpStr,
                        OPENID = temp_param.Contact_Information,
                        CAMPAIGN_CODE = request.Campaign_Code,
                        DATA_SOURCE = request.Data_source,
                        CAMPAIGN_NAME = request.Campaign_Name,
                        INVOKE_TIME = string.IsNullOrWhiteSpace(request.Invoke_Time) ? DateTime.Now : DateTime.Parse(request.Invoke_Time),
                        ISREAL_TIME = request.IsRealTime == "1" ? true : false,
                        IS_SEND = issend,
                        LOYALTY_BRAND = request.Loyalty_Brand,
                        SEND_TIME = dt,
                        VGROUP = request.Vgroup,
                        SOURCE_SYSTEM = request.Data_source,
                        TMP_ID = template.Template_Code,
                        IS_SELECT = isselect,
                        SEND_MSG = resot
                    };

                    if (isHmj)
                    {
                        log.VGROUP = AppConfig.Get("VGROUP");
                        log.LOYALTY_BRAND = AppConfig.Get("LOYALTY_BRAND");
                        log.DATA_SOURCE = AppConfig.Get("DATA_SOURCE");
                        log.SOURCE_SYSTEM = AppConfig.Get("SOURCE_SYSTEM");

                    }

                    long count = _hmjMember.Insert(log);


                }

            }

            return 1;
        }

        #endregion

        #region 私有 方法

        /// <summary>
        /// 给粉丝增加Tag
        /// </summary>
        /// <param name="mobile"></param>
        private string FansTag(string openid)
        {
            string resmsg = NetHelper.HttpRequest(AppConfig.BeautyChinaWebApp, "", "GET", 2000,
                    Encoding.UTF8, "application/json");

            TokeRes toke = JsonHelper.DeserializeObject<TokeRes>(resmsg);

            //先给用户取消100标签
            string json = "{\"openid_list\":[\"" + openid + "\"],\"tagid\" : 100 }";
            string url = "https://api.weixin.qq.com/cgi-bin/tags/members/batchuntagging?access_token="
             + toke.Access_Token;
            var resMessage = NetHelper.HttpRequest(url, json, "POST", 2000,
                    Encoding.UTF8, "application/json");

            string resMessagetwo = string.Empty;
            //如果成功就给打赏101标签
            if (resMessage.Contains("ok"))
            {
                string jsontwo = "{\"openid_list\":[\"" + openid + "\"],\"tagid\" : 101 }";
                string urltwo = "https://api.weixin.qq.com/cgi-bin/tags/members/batchtagging?access_token="
                 + toke.Access_Token;
                resMessagetwo = NetHelper.HttpRequest(url, json, "POST", 2000,
                        Encoding.UTF8, "application/json");
            }

            _logerror.Error("Access_Token:" + toke.Access_Token + "。");
            _logerror.Error(resMessage);
            _logerror.Error(resMessagetwo);
            return "1";
        }

        /// <summary>
        /// 绑定新增会员
        /// </summary>
        /// <param name="mobile"></param>
        private string InsertMember(int? IS_REGISTER, int fans_id, BindReqDTO model)
        {
            dt_Dyn_DispMember_req w = new dt_Dyn_DispMember_req();
            w.DATA_SOURCE = AppConfig.DATA_SOURCE;
            w.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
            w.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
            w.VGROUP = AppConfig.VGROUP; //销售组织
            w.MOB_NUMBER = model.MOBILE;//2002652891
            //w.ACCOUNT_ID = model.MOBILE;//2002652891

            dt_Dyn_DispMember_res dt = WebHmjApiHelp.DispMember(w);

            if (dt.ZCRMT316 == null || dt.ZCRMT316.Length <= 0)
            {
                return "4";
            }

            if (dt.ZCRMT316.Length > 1)
            {
                return "5";
            }

            ZCRMT302_Dyn meber = dt.ZCRMT316[0];

            CUST_MEMBER mebers = new CUST_MEMBER()
            {
                ADDRESS = meber.PSTREET,
                AVA_POINTS = (int?)meber.ZCCUR_POINT,
                BIRTHDAY = string.IsNullOrEmpty(meber.BIRTHDT) ? null : (DateTime?)DateTime.Parse(meber.BIRTHDT),
                CITY = "",
                COUNTRY = "",
                CREATE_DATE = DateTime.Now,
                CREATE_USER = "system",
                EMAIL = meber.CSMTP_ADDR,
                FANS_ID = fans_id,
                GENDER = meber.XSEX == "2" ? true : false,
                MEMBERNO = meber.ACCOUNT_ID,
                MEM_LEVEL = meber.ZTIER,
                MOBILE = model.MOBILE,
                STATUS = 1,
                STORE = meber.ZH003,
                NAME = meber.NAME1_TEXT,
                PARTNER = meber.PARTNER,
                TYPE = meber.DATA_SOURCE == "0001" ? 0 : 1,
                IS_SEND_TMP = 0
            };

            double counts = _oldMember.Insert(mebers);

            if (counts > 0)
            {
                //绑定接口
                dt_Dyn_UpdateMemberShip_req updates = new dt_Dyn_UpdateMemberShip_req();
                ZCRMT322_Dyn z = new ZCRMT322_Dyn();

                z.DATA_SOURCE = AppConfig.DATA_SOURCE;
                z.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
                z.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
                z.VGROUP = AppConfig.VGROUP; //销售组织
                z.WECHATNAME = model.NICKNAME;
                z.WECHATFOLLOWSTATUS = "1";
                z.OPENID = model.OPENID;
                z.ACCOUNT_ID = dt.ZCRMT316[0].ACCOUNT_ID;
                z.PARTNER = dt.ZCRMT316[0].PARTNER;
                z.NAME_LAST = model.NAME_LAST;
                z.NAME_FIRST = model.NAME_FIRST;
                z.BIRTHDT = model.BIRTHDAY;
                z.XSEX = model.GENDER == "0" ? "1" : "2";

                updates.ZCRMT316 = new ZCRMT322_Dyn[] { z };

                dt_Dyn_UpdateMemberShip_res ups = WebHmjApiHelp.UpdateMemberShip(updates);

                FansTag(model.OPENID);

                ////如果是待激活状态那么就要激活
                //if (meber.ZZAFLD000004 == "E0005")
                //{
                //    //激活
                //    this.ChageSatus(meber.ACCOUNT_ID);
                //}

                //绑定成功送50积分
                if (meber.DEPTID != "HSGXN" && meber.DEPTID != "HWXXN")
                {
                    var _sendResult = Send(meber.ACCOUNT_ID, AppConfig.BindMemberSend);

                    #region 老会员绑定并赠送积分后发送模板消息通知

                    if (_sendResult)
                    {
                        var openid = model.OPENID;
                        var tempId = "PC5Va36GwaMzhuWAf-ZZNlGBEmrTNLkVLiN-y6S6NwI";
                        var redirect_url = AppConfig.HmjWebApp + "assets/hmjweixin/html/hytq.html";
                        var p1 = "欢迎您登录华美家！";
                        var p2 = "首次绑定奖励50积分";
                        var p3 = DateTime.Now.ToString("yyyy年MM月dd日");
                        var p4 = "今后您可获得更多积分、享受更多优惠。了解更多会员权益，点击查看 >";
                        SendTmpPublicFunc(true, openid, tempId, redirect_url, p1, p2, p3, p4);
                    }
                    #endregion
                }
            }

            return counts.ToString();
        }

        /// <summary>
        /// 激活
        /// </summary>
        /// <param name="mobile"></param>
        public void ChageSatus(string mobile)
        {
            //如果是待激活状态那么就要激活
            dt_Dyn_ChangeMemberStatus_req reqs = new dt_Dyn_ChangeMemberStatus_req
            {
                ZVTWEG = "102",
                STATUS_OLD = "E0005",
                STATUS_NEW = "E0001",
                ACCOUNT_ID = mobile,
                FLAG = "I",//激活

                DATA_SOURCE = AppConfig.DATA_SOURCE,
                LOYALTY_BRAND = AppConfig.LOYALTY_BRAND,//忠诚度品牌
                SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM,//来源系统
                VGROUP = AppConfig.VGROUP, //销售组织
                PASS_FLAG = "N",
                REASON = "激活"
            };

            LogService log = new LogService();
            dt_Dyn_ChangeMemberStatus_res resok = new dt_Dyn_ChangeMemberStatus_res();
            var _res = "失败";
            try
            {
                resok = WebHmjApiHelp.ChangeMemberStatus(reqs);
                //E代表查询或调整失败
                if (resok.ZTYPE != "E")
                    _res = "成功";
            }
            catch (Exception ex)
            {
                _res += ex.Message;
            }
            finally
            {
                if (_res != "成功")
                {
                    LogReponsitory _logservice = new LogReponsitory();
                    MEMBER_CHANGESTATUS_LOG record = new MEMBER_CHANGESTATUS_LOG
                    {
                        ZTYPE = resok==null?"E": resok.ZTYPE,
                        STATUS = resok == null ? "0" : resok.STATUS,
                        MESSAGE = resok == null ? "修改失败" : resok.MESSAGE,
                        ZPASS = resok == null ? "" : resok.ZPASS,
                        MOBILE = mobile,
                        CREATE_DATE = DateTime.Now
                    };
                    _logservice.InsertFailRecord(record);
                }
                _logerror.Error(mobile + "修改会员状态" + _res);
            }
        }

        /// <summary>
        /// 发送积分
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="pointAndCampaign_he_id">积分与活动代码</param>
        /// <returns></returns>
        public bool Send(string mobile, string pointAndCampaign_he_id)
        {
            string[] _pointAndCampaign_he_id = pointAndCampaign_he_id.Split('|');

            //如果注册会员成功送积分
            //si_Dyn_ActCreateTel_obService == si_ActCreateTel_obService
            dt_Dyn_ActCreateTel_req Actreq = new dt_Dyn_ActCreateTel_req();
            // 数据源类型 
            Actreq.TYPE = AppConfig.DATA_SOURCE;
            //处理标识 
            Actreq.OBJECT_ID = DateTime.Now.ToString("yyyyMMddHHmmss") + CommonHelp.GetRand(10);
            //活动单据类型	
            Actreq.PROCESS_TYPE = "ZXY";
            //外部会员卡号	"会员卡号(如已传输手机号，可不用填写会员卡号)"
            Actreq.ACCOUNT_ID = mobile;
            Actreq.POSTING_DATE = DateTime.Today.ToString();
            //积分类型	
            Actreq.POINT_TYPE = "ZBCJF01";

            //积分数  要改
            Actreq.POINTS = decimal.Parse(_pointAndCampaign_he_id[0]);
            //单据全局活动ID
            Actreq.CAMPAIGN_HE_ID = _pointAndCampaign_he_id[1];
            Actreq.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
            Actreq.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
            Actreq.VGROUP = AppConfig.VGROUP; //销售组织

            dt_Dyn_ActCreateTel_res Actres = WebHmjApiHelp.ActCreateTel(Actreq);





            return Actres.I_ZCRMT047_dyn[0].CHECK_FLAG == "Y";
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="funcName"></param>
        /// <param name="ex"></param>
        private void WriteLog(string funcName, Exception ex)
        {
            _logerror.Error(funcName + "报错\r\n时间" + DateTime.Now.ToString() + "\r\n" + ex.Message + "\r\n" + ex.ToString());
        }

        #endregion

        /// <summary>
        /// 查询模板发送记录
        /// </summary>
        /// <param name="IS_SEND"></param>
        /// <returns></returns>
        public List<WX_TMP_HIS> GetWxTmpHisByIsSend(int IS_SEND)
        {
            return _hmjMember.GetWxTmpHisByIsSend(IS_SEND);
        }

        /// <summary>
        /// 修改模板消息发送记录
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="IS_SEND"></param>
        /// <returns></returns>
        public int UpdateWxTmpHisIsSendByID(int ID, int IS_SEND, string result)
        {
            return _hmjMember.UpdateWxTmpHisIsSendByID(ID, IS_SEND, result);
        }

        public long insertWXCouponGiveInfo(WXCouponGiveInfo WXCouponGiveInfo)
        {
            return _hmjMember.insertWXCouponGiveInfo(WXCouponGiveInfo);
        }

        public WXCouponGiveInfo GetWXCouponGiveInfoByOpenid(string Openid)
        {
            return _hmjMember.GetWXCouponGiveInfoByOpenid(Openid);
        }


        public List<WXCouponNoInfo> QueryWXCouponNoInfo()
        {
            return _hmjMember.QueryWXCouponNoInfo();
        }
        public int UpdateWXCouponNoInfoIsImport(long id)
        {
            return _hmjMember.UpdateWXCouponNoInfoIsImport(id);
        }

        public WXCouponGiveInfo CanGetCoupon(string OpenId, string cardId)
        {
            return _hmjMember.CanGetCoupon(OpenId, cardId);
        }

        public int UpdateWXCouponGiveInfoIsHX(string CouponNo)
        {
            return _hmjMember.UpdateWXCouponGiveInfoIsHX(CouponNo);
        }

        public CardApiTicket GetModelCardApi()
        {
            return _hmjMember.GetModelCardApi();
        }

        public long AddCardApi(CardApiTicket model)
        {
            return _hmjMember.Insert(model);
        }

        /// <summary>
        /// 查询用户某卡券获取资格
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public List<WXCouponGiveInfo> GetWXCouponGiveInfo(string openid, string cardid)
        {
            return _hmjMember.GetWXCouponGiveInfo(openid, cardid);
        }
    }
}
