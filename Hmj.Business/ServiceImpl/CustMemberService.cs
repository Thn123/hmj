using Hmj.Common;
using Hmj.Common.Utils;
using Hmj.DataAccess;
using Hmj.DataAccess.Repository;
using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Entity.PageSearch;
using Hmj.Entity.SearchEntity;
using Hmj.Entity.WxModel;
using Hmj.Interface;
using Hmj.WebService;
using log4net;
using StructureMap.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeChatCRM.Common.Utils;

namespace Hmj.Business.ServiceImpl
{
    public class CustMemberService : ICustMemberService
    {
        private CustMemberRepository _repo;
        private BcjBookRepository _book;
        private static ILog _logerror = LogManager.GetLogger("logerror");

        [SetterProperty]
        public IHmjMemberService _hmjMember { get; set; }

        public CustMemberService()
        {
            _repo = new CustMemberRepository();
            _book = new BcjBookRepository();
        }

        /// <summary>
        /// 绑定会员
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public string BindMember(string mobile, string openid, string Nickname)
        {
            using (TransScope scope = new TransScope())
            {
                WXCUST_FANS fans = _repo.GetFans(openid);

                if (fans == null)
                {
                    return "3";
                }

                CUST_MEMBER member = _repo.GetMemberByMobile(mobile, fans.ID);

                if (member != null)
                {
                    return "2";
                }

                #region 记录会员
                dt_Dyn_DispMember_req w = new dt_Dyn_DispMember_req();
                w.DATA_SOURCE = "0002";
                w.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
                w.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
                w.VGROUP = AppConfig.VGROUP; //销售组织
                w.MOB_NUMBER = mobile;//2002652891
                dt_Dyn_DispMember_res dt = WebApiHelp.DispMember(w);

                if (dt.ZCRMT316 == null || dt.ZCRMT316.Count() <= 0)
                {
                    return "4";
                }

                if (dt.ZCRMT316.Count() > 1)
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
                    FANS_ID = fans.ID,
                    GENDER = meber.XSEX == "2" ? true : false,
                    MEMBERNO = meber.ACCOUNT_ID,
                    MEM_LEVEL = meber.ZTIER,
                    MOBILE = mobile,
                    STATUS = 1,
                    STORE = meber.ZH003,
                    NAME = meber.NAME1_TEXT,
                    PARTNER = meber.PARTNER,
                    TYPE = meber.DATA_SOURCE == "0001" ? 0 : 1
                };

                double counts = _repo.Insert(mebers);
                #endregion
                //如果绑定成功需要告诉crm已经绑定
                if (counts > 0)
                {
                    //绑定接口
                    dt_Dyn_UpdateMemberShip_req updates = new dt_Dyn_UpdateMemberShip_req();
                    ZCRMT322_Dyn z = new ZCRMT322_Dyn();
                    z.DATA_SOURCE = "0002";

                    z.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
                    z.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
                    z.VGROUP = AppConfig.VGROUP; //销售组织
                    z.WECHATNAME = Nickname;
                    z.WECHATFOLLOWSTATUS = "1";
                    z.OPENID = openid;
                    z.ACCOUNT_ID = dt.ZCRMT316[0].ACCOUNT_ID;
                    z.PARTNER = dt.ZCRMT316[0].PARTNER;
                    updates.ZCRMT316 = new ZCRMT322_Dyn[] { z };

                    dt_Dyn_UpdateMemberShip_res ups = WebApiHelp.UpdateMemberShip(updates);

                    //如果是待激活状态那么就要激活
                    if (meber.ZZAFLD000004 == "E0005")
                    {
                        //激活
                        this.ChageSatus(meber.ACCOUNT_ID);
                    }

                    //绑定成功送三百积分
                    if (fans.IS_REGISTER == null || fans.IS_REGISTER == 0)
                    {
                        #region 注册送积分
                        //如果注册会员成功送积分
                        //si_Dyn_ActCreateTel_obService == si_ActCreateTel_obService
                        dt_Dyn_ActCreateTel_req Actreq = new dt_Dyn_ActCreateTel_req();
                        // 数据源类型 
                        Actreq.TYPE = "0002";
                        //处理标识 
                        Actreq.OBJECT_ID = DateTime.Now.ToString("yyyyMMddHHmmss") + CommonHelp.GetRand(10);
                        //活动单据类型	
                        Actreq.PROCESS_TYPE = "ZXY";
                        //外部会员卡号	"会员卡号(如已传输手机号，可不用填写会员卡号)"
                        Actreq.ACCOUNT_ID = meber.ACCOUNT_ID;
                        Actreq.POSTING_DATE = DateTime.Today.ToString();
                        //积分类型	
                        Actreq.POINT_TYPE = "ZBCJF01";

                        //积分数  要改
                        Actreq.POINTS = decimal.Parse(AppConfig.POINTS);
                        //单据全局活动ID
                        Actreq.CAMPAIGN_HE_ID = "CMP2820171023005";
                        Actreq.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
                        Actreq.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
                        Actreq.VGROUP = AppConfig.VGROUP; //销售组织

                        dt_Dyn_ActCreateTel_res Actres = WebApiHelp.ActCreateTel(Actreq);

                        string remak = string.Empty;

                        if (Actres.I_ZCRMT047_dyn[0].CHECK_FLAG == "N")
                        {
                            remak = "新注册会员送积分失败，原因：" + Actres.I_ZCRMT047_dyn[0].MESSAGE;
                        }
                        #endregion

                        string str = _repo.UpdateIsRegion(fans.ID);
                    }
                }
                scope.Commit();
                return "1";
            }
        }

        /// <summary>
        /// 判断该粉丝是否已经绑定会员
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public bool ChckBind(string openid)
        {
            CUST_MEMBER member = _repo.GetMemberByOpenId(openid);

            return member != null;
        }

        /// <summary>
        /// 获取并更新会员信息
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public MemberInfo GetLoadMember(string openid)
        {
            //查询会员主数据信息
            CUST_MEMBER member = _repo.GetMemberByOpenId(openid);

            #region 得到最新会员信息
            //接口查询会员主数据
            dt_Dyn_DispMember_req w = new dt_Dyn_DispMember_req();
            w.DATA_SOURCE = "0002";
            w.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
            w.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
            w.VGROUP = AppConfig.VGROUP; //销售组织
            w.PARTNER = member.PARTNER;//会员唯一标示
            dt_Dyn_DispMember_res dt = WebApiHelp.DispMember(w);

            if (dt.ZCRMT316 == null || dt.ZCRMT316.Count() <= 0)
            {
                return null;
            }

            if (dt.ZCRMT316.Count() > 1)
            {
                return null;
            }

            ZCRMT302_Dyn newmeber = dt.ZCRMT316[0];

            #region 接口查询兑礼密码
            dt_Dyn_ChangeMemberStatus_req req = new dt_Dyn_ChangeMemberStatus_req();
            req.DATA_SOURCE = AppConfig.DATA_SOURCE;
            req.ZVTWEG = "102";//来源渠道
            req.ACCOUNT_ID = newmeber.ACCOUNT_ID;
            req.FLAG = "Q";//查询
            req.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
            req.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
            req.VGROUP = AppConfig.VGROUP; //销售组织
            req.PASS_FLAG = "Y";
            req.REASON = "查询密码";
            dt_Dyn_ChangeMemberStatus_res res = WebApiHelp.ChangeMemberStatus(req);
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
                LOGINPASSON = res.ZPASS,
                NAME_FIRST = newmeber.NAME_FIRST,
                NAME_LAST = newmeber.NAME_LAST,
                ZZAFLD000004 = newmeber.ZZAFLD000004
            };
            #endregion

            dt_Dyn_PosCpSearch_req reqs = new dt_Dyn_PosCpSearch_req();
            reqs.ACCOUNT_ID = member.MEMBERNO;//会员账号和MOB_NUMBER至少输入一个
            reqs.DATA_SOURCE = AppConfig.DATA_SOURCE;

            reqs.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
            reqs.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
            reqs.VGROUP = AppConfig.VGROUP; //销售组织

            dt_Dyn_PosCpSearch_res resok = WebApiHelp.PosCpSearch(reqs);

            int allcount = 0;

            foreach (ZPARTERN_CP2 item in resok.ZPARTERN_CP2)
            {
                //未使用
                if (string.IsNullOrEmpty(item.ZCP_USE_FLAG))
                {
                    //查询另外一个集合的数据
                    dt_Dyn_PosCpSearch_resITEM resout = resok.ZCPQ_RESULT.Where(a => a.ZCP_NUM == item.ZCP_NUM).
                        ToList().FirstOrDefault();

                    DateTime endTime = resout.ZCP_EDATE;// DateTime.Parse(resout.ZCP_EDATE);
                    if (endTime >= DateTime.Now)
                    {
                        allcount++;
                    }
                }
            }

            //更新本地会员信息
            int count = _repo.UpdateMember(mebers);

            return new MemberInfo()
            {
                AVA_POINTS = (int?)newmeber.ZCCUR_POINT,
                MOBILE = newmeber.MOB_NUMBER,
                SEX = newmeber.XSEX == "2" ? 1 : 0,
                COUPON_COUNT = allcount.ToString(),
                MEM_LEVEL = Utility.GetMemberLvl(newmeber.ZTIER)
            };
        }

        /// <summary>
        /// 注册会员
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="openid"></param>
        /// <param name="nameo"></param>
        /// <param name="namek"></param>
        /// <returns></returns>
        public string RegisterMember(string mobile, string openid,
            string nameo, string namek, string brithday, string nickname, string gender)
        {
            WXCUST_FANS fans = _repo.GetFans(openid);
            //没有该粉丝
            if (fans == null)
            {
                return "-1";
            }

            #region 调用接口通知crm创建会员
            dt_Dyn_UploadMemberShip_req req = new dt_Dyn_UploadMemberShip_req();
            ZCRMT316_Dyn meber = new ZCRMT316_Dyn();
            meber.MOB_NUMBER = mobile;
            meber.OPENID = openid;
            meber.NAME1_TEXT = nameo + namek;//全名
            meber.DATA_SOURCE = "0002";
            meber.ACCOUNT_ID = mobile;
            meber.NAME_LAST = nameo;
            meber.NAME_FIRST = namek;
            meber.XSEX = gender == "1" ? "2" : "1";
            meber.REGION = "";
            meber.BIRTHDT = brithday;
            meber.NAMCOUNTRY = "CN";
            meber.WECHATNAME = nickname;
            meber.WECHATFOLLOWSTATUS = "1";
            //meber.LOGINPASS2 = "111111";//兑换密码默认123456
            meber.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
            meber.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
            meber.VGROUP = AppConfig.VGROUP; //销售组织

            //固定死
            meber.EMPID = AppConfig.EMPID;
            meber.DEPTID = AppConfig.DEPTID;

            req.ZCRMT316 = new ZCRMT316_Dyn[] { meber };

            //创建会员
            dt_Dyn_UploadMemberShip_res res = WebApiHelp.CreateMemberShip(req);
            #endregion

            if (res.WV_RETURN == "N")
            {
                return res.WV_MESSAGE;
            }

            //创建成功
            if (res.WV_RETURN == "Y")
            {
                //创建本地会员
                CUST_MEMBER member = new CUST_MEMBER()
                {
                    BIRTHDAY = DateTime.Parse(brithday),
                    FANS_ID = fans.ID,
                    MEMBERNO = res.ACCOUNT_ID,
                    MOBILE = mobile,
                    NAME = nameo + namek,
                    STATUS = 1,
                    STORE = AppConfig.DEPTID,
                    TYPE = 0,
                    //LOGINPASSON = "111111",
                    CREATE_DATE = DateTime.Now,
                    AVA_POINTS = 0,
                    PARTNER = res.PARTNER,
                    GENDER = false
                    //REMARK = remak
                };

                double counts = _repo.Insert(member);

                if (counts <= 0)
                {
                    return $"请关掉页面并用手机号：{mobile}，绑定!";
                }
                else
                {
                    #region 注册送积分
                    //如果注册会员成功送积分
                    //si_Dyn_ActCreateTel_obService == si_ActCreateTel_obService
                    dt_Dyn_ActCreateTel_req Actreq = new dt_Dyn_ActCreateTel_req
                    {
                        // 数据源类型 
                        TYPE = "0002",
                        //处理标识 
                        OBJECT_ID = DateTime.Now.ToString("yyyyMMddHHmmss") + CommonHelp.GetRand(10),
                        //活动单据类型	
                        PROCESS_TYPE = "ZXY",
                        //外部会员卡号	"会员卡号(如已传输手机号，可不用填写会员卡号)"
                        ACCOUNT_ID = res.ACCOUNT_ID,
                        POSTING_DATE = DateTime.Today.ToString(),
                        //积分类型	
                        POINT_TYPE = "ZBCJF01",
                        //积分数  要改
                        POINTS = decimal.Parse(AppConfig.POINTS),
                        //单据全局活动ID
                        CAMPAIGN_HE_ID = "C-00001786",
                        LOYALTY_BRAND = AppConfig.LOYALTY_BRAND,//忠诚度品牌
                        SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM,//来源系统
                        VGROUP = AppConfig.VGROUP //销售组织
                    };

                    dt_Dyn_ActCreateTel_res Actres = WebApiHelp.ActCreateTel(Actreq);

                    //if (Actres.I_ZCRMT047_dyn[0].CHECK_FLAG == "N")
                    //{
                    //    remak = "新注册会员送积分失败，原因：" + Actres.I_ZCRMT047_dyn[0].MESSAGE;
                    //}
                    #endregion


                    #region 注册成功就要激活会员
                    ////如果是待激活状态那么就要激活
                    //dt_Dyn_ChangeMemberStatus_req reqs = new dt_Dyn_ChangeMemberStatus_req();
                    //reqs.DATA_SOURCE = "0002";
                    //reqs.ZVTWEG = "102";
                    ////req.STATUS_OLD = "E0000";
                    ////req.STATUS_NEW = "E0001";
                    //reqs.ACCOUNT_ID = meber.ACCOUNT_ID;
                    //reqs.FLAG = "I";//激活
                    //reqs.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
                    //reqs.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
                    //reqs.VGROUP = AppConfig.VGROUP; //销售组织
                    //reqs.PASS_FLAG = "N";
                    //reqs.REASON = "激活";
                    //dt_Dyn_ChangeMemberStatus_res resok = WebApiHelp.ChangeMemberStatus(reqs); 
                    #endregion
                }
            }

            return "1";
        }

        /// <summary>
        /// 获取会员详细信息
        /// </summary>
        /// <returns></returns>
        public MemberInfo GetMemberInfo(string openid, string isnew = "1")
        {
            //查询会员信息
            CUST_MEMBER member = _repo.GetMemberByOpenId(openid);

            MemberInfo info = new MemberInfo();

            string storeName = "";

            if (isnew == "1")
            {
                dt_Dyn_DispMemQuick_req w2 = new dt_Dyn_DispMemQuick_req();
                w2.DATA_SOURCE = "0002";//固定
                w2.BP = member.PARTNER;

                w2.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
                w2.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
                w2.VGROUP = AppConfig.VGROUP; //销售组织
                dt_Dyn_DispMemQuick_res dt2 = WebApiHelp.DispMemQuick(w2);
                storeName = dt2.I_ZCRMT316[0].DEPT_T;
            }
            info.AVA_POINTS = member.AVA_POINTS;
            info.MOBILE = member.MOBILE;
            info.SEX = member.GENDER == true ? 1 : 0;
            info.NAME = member.NAME;
            info.ADDRESS = member.ADDRESS;
            info.MEM_LEVEL = Utility.GetMemberLvl(member.MEM_LEVEL);
            info.BIRTHDAY = member.BIRTHDAY;
            info.STORE = member.STORE;
            info.PWD = member.LOGINPASSON;
            info.STORE_NAME = storeName;
            info.NAME_FIRST = member.NAME_FIRST;
            info.NAME_LAST = member.NAME_LAST;
            info.Member_Id = member.ID.ToString();

            return info;
        }

        /// <summary>
        /// 修改会员手机或兑礼密码，
        /// </summary>
        /// <param name="mobile">新手机号</param>
        /// <param name="oldmobile">旧的手机号</param>
        /// <param name="pwd">新密码</param>
        /// <param name="oldpwd">旧密码</param>
        /// <returns></returns>
        public int UpdateMobileOrPwd(string mobile, string oldmobile, string pwd,
            string oldpwd, string OpendID, ref string msg)
        {
            //查询会员主数据信息
            CUST_MEMBER member = _repo.GetMemberByOpenId(OpendID);
            //修改手机号
            if (!string.IsNullOrEmpty(mobile) && !string.IsNullOrEmpty(oldmobile))
            {
                //修改手机号
                dt_Dyn_UpdateMemberShip_req updates = new dt_Dyn_UpdateMemberShip_req();
                ZCRMT322_Dyn z = new ZCRMT322_Dyn
                {
                    DATA_SOURCE = AppConfig.DATA_SOURCE,
                    LOYALTY_BRAND = AppConfig.LOYALTY_BRAND,//忠诚度品牌
                    SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM,//来源系统
                    VGROUP = AppConfig.VGROUP, //销售组织
                    MOB_NUMBER = mobile,
                    ACCOUNT_ID = member.MEMBERNO,
                    PARTNER = member.PARTNER
                };
                updates.ZCRMT316 = new ZCRMT322_Dyn[] { z };

                dt_Dyn_UpdateMemberShip_res ups = WebApiHelp.UpdateMemberShip(updates);

                if (ups.WV_RETURN == "Y")
                {
                    return 1;
                }
                else
                {
                    msg = ups.WV_MESSAGE;
                    return 2;
                }
            }

            //修改密码
            if (!string.IsNullOrEmpty(pwd) && !string.IsNullOrEmpty(oldpwd))
            {
                dt_Dyn_ChangePassword_req piq = new dt_Dyn_ChangePassword_req
                {
                    DATA_SOURCE = AppConfig.DATA_SOURCE,//SOURCE
                    ZVTWEG = "102",
                    ACCOUNT_ID = member.MEMBERNO,//EXTERNAL_CARD_NU

                    EXCHANGEPASS = oldpwd,
                    EXCHANGEPASS2 = pwd,
                    PASS_TYPE = "2",
                    REASON = "兑礼密码修改",
                    INIT_FLAG = "",

                    LOYALTY_BRAND = AppConfig.LOYALTY_BRAND,//忠诚度品牌
                    SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM,//来源系统
                    VGROUP = AppConfig.VGROUP //销售组织
                };
                //修改CRM端密码
                dt_Dyn_ChangePassword_res res = WebApiHelp.ChangePassword(piq);

                if (res.ZRETURN == "Y")
                {
                    //修改本地密码
                    CUST_MEMBER mebers = new CUST_MEMBER()
                    {
                        ID = member.ID,
                        LOGINPASSON = pwd
                    };
                    //更新本地会员信息
                    int count = _repo.UpdateMember(mebers);

                    return 1;
                }
                else
                {
                    return 2;
                }
            }

            return 0;
        }

        /// <summary>
        /// 修改会员常规信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="gender"></param>
        /// <param name="address"></param>
        /// <param name="opendID"></param>
        /// <returns></returns>
        public int UpdateMember(string NAME_LAST, string NAME_FIRST, string gender, string address, string opendID)
        {
            //查询会员主数据信息
            CUST_MEMBER member = _repo.GetMemberByOpenId(opendID);

            dt_Dyn_UpdateMemberShip_req updates = new dt_Dyn_UpdateMemberShip_req();
            ZCRMT322_Dyn z = new ZCRMT322_Dyn();
            z.DATA_SOURCE = "0002";

            z.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
            z.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
            z.VGROUP = AppConfig.VGROUP; //销售组织

            z.NAME_LAST = NAME_LAST;
            z.NAME_FIRST = NAME_FIRST;
            z.NAME1_TEXT = NAME_LAST + NAME_FIRST;
            z.XSEX = gender == "0" ? "1" : "2"; //1女2男
            z.PSTREET = address;

            //z.MOB_NUMBER = mobile;
            z.ACCOUNT_ID = member.MEMBERNO;
            z.PARTNER = member.PARTNER;
            updates.ZCRMT316 = new ZCRMT322_Dyn[] { z };

            dt_Dyn_UpdateMemberShip_res ups = WebApiHelp.UpdateMemberShip(updates);

            if (ups.WV_RETURN == "Y")
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }

        /// <summary>
        /// 显示历史记录
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public PointHistory GetPointHistory(string openid)
        {
            //查询会员主数据信息
            CUST_MEMBER member = _repo.GetMemberByOpenId(openid);

            dt_Dyn_GetPointDetail_req req = new dt_Dyn_GetPointDetail_req();
            req.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
            req.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
            req.VGROUP = AppConfig.VGROUP; //销售组织
            req.DATA_SOURCE = "0002";

            //得到一年之内的历史记录
            req.ZSTART_DATE = DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd");
            req.ZEND_DATE = DateTime.Now.ToString("yyyy-MM-dd");

            req.ACCOUNT_ID = member.MEMBERNO;

            ZCRMT402_Dyn[] lists = WebApiHelp.GetPointDetail(req).ZCRMT402;
            PointHistory datas = new PointHistory();
            List<PointHisList> arry = new List<PointHisList>();

            foreach (ZCRMT402_Dyn dyn in lists)
            {
                PointHisList his = new PointHisList()
                {
                    CATEGORY = dyn.CATEGORY,
                    CREATED_TIME = dyn.CREATED_TIME,
                    EXPIRE_DATE = dyn.EXPIRE_DATE,
                    ORDER_TYPE = Utility.GetDetailDis(dyn.ORDER_TYPE),
                    POINTS = dyn.POINTS.ToString(),
                    CREATED_TIME_EX = DateTime.Parse(dyn.CREATED_TIME),
                };

                arry.Add(his);
            }

            datas.Hise = arry.OrderByDescending(a => a.CREATED_TIME_EX).ToList();
            datas.Point = member.AVA_POINTS.ToString();
            return datas;
        }

        /// <summary>
        /// 获取优惠券
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public MEMBER_COUPON GetCoupon(string openid)
        {
            MEMBER_COUPON coupon = new MEMBER_COUPON();

            //查询会员主数据信息
            CUST_MEMBER member = _repo.GetMemberByOpenId(openid);

            dt_Dyn_PosCpSearch_req reqs = new dt_Dyn_PosCpSearch_req();
            reqs.ACCOUNT_ID = member.MEMBERNO;//会员账号和MOB_NUMBER至少输入一个
            reqs.DATA_SOURCE = "0002";

            reqs.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
            reqs.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
            reqs.VGROUP = AppConfig.VGROUP; //销售组织

            dt_Dyn_PosCpSearch_res resok = WebApiHelp.PosCpSearch(reqs);

            if (resok.ZTYPE != "Y")
            {
                return null;
            }

            //已使用
            List<COUPONS> shiyong = new List<COUPONS>();

            //未使用
            List<COUPONS> meiyou = new List<COUPONS>();

            //已过期
            List<COUPONS> guoqi = new List<COUPONS>();

            if (resok.ZPARTERN_CP2.Count() > 0)
            {
                foreach (ZPARTERN_CP2 item in resok.ZPARTERN_CP2)
                {
                    //查询另外一个集合的数据
                    dt_Dyn_PosCpSearch_resITEM resout = resok.ZCPQ_RESULT.Where(a => a.ZCP_NUM == item.ZCP_NUM).
                        ToList().FirstOrDefault();

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
                        IS_BOOK = 0
                    };

                   
                        one.ZCPUDATE = item.ZCPUDATE;
                    

                    //已使用
                    if (item.ZCP_USE_FLAG == "X")
                    {
                        shiyong.Add(one);
                    }

                    //未使用
                    if (string.IsNullOrEmpty(item.ZCP_USE_FLAG))
                    {
                        DateTime endTime = one.ZCP_EDATE;
                        if (endTime > DateTime.Now)
                        {
                            HMJ_BOOK_EX book = _book.GetBookByNo(one.ZCP_NUM);

                            if (book != null)
                            {
                                if (book.STATUS == 1)
                                {
                                    shiyong.Add(one);
                                }
                                else if (book.STATUS == 0)
                                {
                                    one.IS_BOOK = 1;
                                    meiyou.Add(one);
                                }
                            }
                            else
                            {
                                meiyou.Add(one);
                            }

                        }
                        else
                        {
                            guoqi.Add(one);
                        }
                    }
                }
            }

            coupon.ALREADY_USE = shiyong.OrderByDescending(a => a.ZCPUDATE).ToList();
            coupon.NOT_USE = meiyou.OrderBy(a => a.ZCP_EDATE_EX).ToList();
            coupon.OBSOLETE = guoqi.OrderByDescending(a => a.ZCP_EDATE_EX).ToList();
            return coupon;
        }

        /// <summary>
        /// 得到列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public PagedList<SURREY_GROUP_EX> QueryGetGroups(GroupSearch search, PageView view)
        {
            return _repo.QueryGetGroups(search, view);
        }

        /// <summary>
        /// 删除会员
        /// </summary>
        /// <param name="fROMUSERNAME"></param>
        /// <returns></returns>
        public bool DeleteMember(string fROMUSERNAME)
        {
            return _repo.DeleteMember(fROMUSERNAME) > 0;
        }

        /// <summary>
        /// 发送积分
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public string SendPiod(string mobile, string piont)
        {
            dt_Dyn_ActCreateTel_req Actreq = new dt_Dyn_ActCreateTel_req();
            // 数据源类型 
            Actreq.TYPE = "0002";
            //处理标识 
            Actreq.OBJECT_ID = DateTime.Now.ToString("yyyyMMddHHmmss") + CommonHelp.GetRand(10); ;
            //活动单据类型	
            Actreq.PROCESS_TYPE = "ZXY";
            //外部会员卡号	"会员卡号(如已传输手机号，可不用填写会员卡号)"
            Actreq.ACCOUNT_ID = mobile;
            Actreq.POSTING_DATE = DateTime.Today.ToString();
            //积分类型	
            Actreq.POINT_TYPE = "ZBCJF01";
            //积分数  要改
            Actreq.POINTS = decimal.Parse(piont);
            //单据全局活动ID
            Actreq.CAMPAIGN_HE_ID = "C-00001786";
            Actreq.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
            Actreq.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
            Actreq.VGROUP = AppConfig.VGROUP; //销售组织

            dt_Dyn_ActCreateTel_res Actres = WebApiHelp.ActCreateTel(Actreq);

            return "";
        }

        /// <summary>
        /// 减少或者增加积分
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public string ReduceOrAddPiod(string mobile, string piont, string type)
        {
            string pont_type = "ZXY";
            if (type == "reduce")
            {
                pont_type = "PRODUCT_REDEEM";
            }

            if (type == "add")
            {
                pont_type = "ZXY";
            }

            dt_Dyn_ActCreateTel_req Actreq = new dt_Dyn_ActCreateTel_req();
            // 数据源类型 
            Actreq.TYPE = "0002";
            //处理标识 
            Actreq.OBJECT_ID = DateTime.Now.ToString("yyyyMMddHHmmss") + CommonHelp.GetRand(10); ;
            //活动单据类型	
            Actreq.PROCESS_TYPE = pont_type;
            //外部会员卡号	"会员卡号(如已传输手机号，可不用填写会员卡号)"
            Actreq.ACCOUNT_ID = mobile;
            Actreq.POSTING_DATE = DateTime.Today.ToString();
            //积分类型	
            Actreq.POINT_TYPE = "ZBCJF01";
            //积分数  要改
            Actreq.POINTS = decimal.Parse(piont);
            //单据全局活动ID
            Actreq.CAMPAIGN_HE_ID = "C-00001786";
            Actreq.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
            Actreq.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
            Actreq.VGROUP = AppConfig.VGROUP; //销售组织

            dt_Dyn_ActCreateTel_res Actres = WebApiHelp.ActCreateTel(Actreq);

            #region 查询会员详情
            dt_Dyn_DispMember_req w = new dt_Dyn_DispMember_req();
            w.DATA_SOURCE = "0002";
            w.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
            w.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
            w.VGROUP = AppConfig.VGROUP; //销售组织
            w.MOB_NUMBER = mobile;//2002652891
            dt_Dyn_DispMember_res dt = WebApiHelp.DispMember(w);

            if (dt.ZCRMT316 == null || dt.ZCRMT316.Count() <= 0)
            {
                return "未查询到该会员";
            }

            if (dt.ZCRMT316.Count() > 1)
            {
                return "查到多条该信息，请联系开发";
            }
            #endregion

            ZCRMT302_Dyn meber = dt.ZCRMT316[0];

            return meber.PARTNER;
        }

        /// <summary>
        /// 得到bp号
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public string GetOldMember(string openid)
        {
            return _repo.GetOldMember(openid);
        }

        /// <summary>
        /// 得到时时的会员数据
        /// </summary>
        /// <param name="bp"></param>
        /// <returns></returns>
        public ZCRMT302_Dyn GetMemberModelByBp(string bp)
        {
            //接口查询会员主数据
            dt_Dyn_DispMember_req w = new dt_Dyn_DispMember_req();
            w.DATA_SOURCE = "0002";
            w.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
            w.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
            w.VGROUP = AppConfig.VGROUP; //销售组织
            w.PARTNER = bp;//会员唯一标示
            dt_Dyn_DispMember_res dt = WebApiHelp.DispMember(w);

            if (dt.ZCRMT316 == null || dt.ZCRMT316.Count() <= 0)
            {
                return null;
            }

            if (dt.ZCRMT316.Count() > 1)
            {
                return null;
            }

            return dt.ZCRMT316[0];
        }

        /// <summary>
        /// 插入数据库
        /// </summary>
        /// <param name="model"></param>
        public void InserMeber(CUST_MEMBER model)
        {
            _repo.Insert(model);
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="fansid"></param>
        public void UpdateFans(int fansid)
        {
            string str = _repo.UpdateIsRegion(fansid);
        }

        /// <summary>
        /// 激活
        /// </summary>
        /// <param name="mobile"></param>
        public void ChageSatus(string mobile)
        {
            //如果是待激活状态那么就要激活
            dt_Dyn_ChangeMemberStatus_req reqs = new dt_Dyn_ChangeMemberStatus_req();
            reqs.DATA_SOURCE = "0002";
            reqs.ZVTWEG = "102";
            reqs.STATUS_OLD = "E0005";
            reqs.STATUS_NEW = "E0001";
            reqs.ACCOUNT_ID = mobile;
            reqs.FLAG = "I";//激活
            reqs.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
            reqs.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
            reqs.VGROUP = AppConfig.VGROUP; //销售组织
            reqs.PASS_FLAG = "N";
            reqs.REASON = "激活";
            dt_Dyn_ChangeMemberStatus_res resok = WebApiHelp.ChangeMemberStatus(reqs);
        }

        /// <summary>
        /// 写错误日志
        /// </summary>
        /// <param name="funcName"></param>
        /// <param name="ex"></param>
        private void WriteLog(string funcName, Exception ex)
        {
            _logerror.Error(funcName + "报错\r\n时间" + DateTime.Now.ToString() + "\r\n" + ex.Message + "\r\n" + ex.ToString());
        }

        private void WriteLog(string funcName)
        {
            _logerror.Error(funcName + "\r\n时间" + DateTime.Now.ToString());
        }

        /// <summary>
        /// 根据openid获取会员普通信息
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public CUST_MEMBER GetMemberByOpenId(string openid)
        {
            return _repo.GetMemberByOpenId(openid);
        }

    }
}
