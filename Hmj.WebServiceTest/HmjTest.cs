using HmjNew.Service;
using Hmj.Business;
using Hmj.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hmj.WebServiceTest
{
    public partial class HmjTest : Form
    {
        public HmjTest()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 主数据查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            dt_Dyn_DispMember_req w = new dt_Dyn_DispMember_req();

            w.DATA_SOURCE = AppConfig.DATA_SOURCE;
            w.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
            w.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
            w.VGROUP = AppConfig.VGROUP; //销售组织
            w.PARTNER = "MCHM000000003";//2002652891
            dt_Dyn_DispMember_res dt = WebHmjApiHelp.DispMember(w, true);
        }

        /// <summary>
        /// 会员快速查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            dt_Dyn_DispMemQuick_req w2 = new dt_Dyn_DispMemQuick_req();
            w2.BP = "MCHM000000137";

            w2.DATA_SOURCE = AppConfig.DATA_SOURCE;
            w2.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
            w2.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
            w2.VGROUP = AppConfig.VGROUP; //销售组织

            dt_Dyn_DispMemQuick_res dt2 = WebHmjApiHelp.DispMemQuick(w2, true);
        }

        /// <summary>
        /// 创建会员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            dt_Dyn_CreateHMJMemberShip_req req = new dt_Dyn_CreateHMJMemberShip_req();
            ZCRMT316_HMJ meber = new ZCRMT316_HMJ();
            meber.MOB_NUMBER = "13681885249";//18952435467
            meber.OPENID = "testopenid4";
            meber.NAME1_TEXT = "苗玉凯";//全名

            meber.ACCOUNT_ID = "13681885249";
            meber.NAME_LAST = "苗";
            meber.NAME_FIRST = "玉凯";
            meber.XSEX = "1";
            meber.REGION = "";
            meber.BIRTHDT = DateTime.Now.ToString();
            meber.NAMCOUNTRY = "CN";
            meber.WECHATNAME = "飞翔";
            meber.WECHATFOLLOWSTATUS = "1";
            meber.RE_BPEXT = "13681885234";//推荐人手机号 （需要快速查询验证）
            meber.LOGINPASS2 = "";//兑换密码默认123456

            meber.DATA_SOURCE = AppConfig.DATA_SOURCE;//数据来源
            meber.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
            meber.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
            meber.VGROUP = AppConfig.VGROUP; //销售组织
            meber.CHANNEL_SOURCE = AppConfig.CHANNEL_SOURCE;//渠道来源
            meber.BRAND_SOURCE = AppConfig.BRAND_SOURCE;//品牌来源

            //固定死
            meber.EMPID = AppConfig.EMPID;
            meber.DEPTID = AppConfig.DEPTID;

            req.ZCRMT316 = new ZCRMT316_HMJ[] { meber };

            //创建会员
            dt_Dyn_CreateHMJMemberShip_res res = WebHmjApiHelp.CreateMemberShip(req, true);

        }

        /// <summary>
        /// 创建潜客
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            dt_Dyn_CreateLead_req crreq = new dt_Dyn_CreateLead_req();
            ZCRMT342_Dyn meber = new ZCRMT342_Dyn();

            meber.WECHAT = "openidtest4";
            meber.NAME1_TEXT = "华美家潜客";//全名
            meber.WECHATNAME = "飞";//昵称

            meber.DATA_SOURCE = AppConfig.DATA_SOURCE;
            meber.DEPTID = AppConfig.DEPTID;//入会门店
            meber.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
            meber.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
            meber.VGROUP = AppConfig.VGROUP; //销售组织
            meber.BRAND_SOURCE = AppConfig.BRAND_SOURCE;//品牌
            meber.CHANNEL_SOURCE = AppConfig.CHANNEL_SOURCE;//来源渠道
            meber.IN_CHANNEL = "11";//收集渠道
            meber.IN_DATE = DateTime.Now.ToString("yyyy-MM-dd");//收集日期

            crreq.INFO_QK = new ZCRMT342_Dyn[] { meber };
            dt_Dyn_CreateLead_res qianke = WebHmjApiHelp.CreateLead(crreq, true);
        }

        /// <summary>
        /// 修改会员详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            dt_Dyn_UpdateMemberShip_req updates = new dt_Dyn_UpdateMemberShip_req();
            ZCRMT322_Dyn z = new ZCRMT322_Dyn();
            z.DATA_SOURCE = AppConfig.DATA_SOURCE;

            z.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
            z.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
            z.VGROUP = AppConfig.VGROUP; //销售组织

            z.NAME_LAST = "苗*";
            z.NAME_FIRST = "玉凯*";
            z.NAME1_TEXT = "苗+玉凯";
            z.XSEX = "2";
            z.PSTREET = "修改的地址";

            z.ACCOUNT_ID = "18952435467";
            z.PARTNER = "MCHM000000003";
            updates.ZCRMT316 = new ZCRMT322_Dyn[] { z };

            dt_Dyn_UpdateMemberShip_res ups = WebHmjApiHelp.UpdateMemberShip(updates, true);
        }

        /// <summary>
        /// 绑定成功修改状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            dt_Dyn_UpdateMemberShip_req updates = new dt_Dyn_UpdateMemberShip_req();
            ZCRMT322_Dyn z = new ZCRMT322_Dyn();
            z.DATA_SOURCE = AppConfig.DATA_SOURCE;

            z.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
            z.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
            z.VGROUP = AppConfig.VGROUP; //销售组织
            z.WECHATNAME = "昵称";
            z.WECHATFOLLOWSTATUS = "1";
            z.OPENID = "sdjflajslkfd";
            z.ACCOUNT_ID = "18952435467";
            z.PARTNER = "MCHM000000003";
            updates.ZCRMT316 = new ZCRMT322_Dyn[] { z };

            dt_Dyn_UpdateMemberShip_res ups = WebHmjApiHelp.UpdateMemberShip(updates, true);
        }

        /// <summary>
        /// 修改手机号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            dt_Dyn_UpdateMemberShip_req updates = new dt_Dyn_UpdateMemberShip_req();
            ZCRMT322_Dyn z = new ZCRMT322_Dyn();
            z.DATA_SOURCE = AppConfig.DATA_SOURCE;

            z.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
            z.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
            z.VGROUP = AppConfig.VGROUP; //销售组织

            z.MOB_NUMBER = "18954152811";
            z.ACCOUNT_ID = "13681885237";
            z.PARTNER = "MCHM000000036";
            updates.ZCRMT316 = new ZCRMT322_Dyn[] { z };

            dt_Dyn_UpdateMemberShip_res ups = WebHmjApiHelp.UpdateMemberShip(updates, true);
        }

        /// <summary>
        /// 会员积分明细查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e)
        {
            dt_Dyn_GetPointDetail_req req = new dt_Dyn_GetPointDetail_req();
            req.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
            req.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
            req.VGROUP = AppConfig.VGROUP; //销售组织

            req.DATA_SOURCE = AppConfig.DATA_SOURCE;

            //得到一个月之内的历史记录
            req.ZSTART_DATE = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            req.ZEND_DATE = DateTime.Now.ToString("yyyy-MM-dd");
            //req.POINT_TYPE = "ZHMJF01";
            req.ACCOUNT_ID = "18952435467";

            ZCRMT402_Dyn[] lists = WebHmjApiHelp.GetPointDetail(req, true).ZCRMT402;
        }

        /// <summary>
        /// 会员绑定查询品牌会员接口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button9_Click(object sender, EventArgs e)
        {
            dt_Dyn_QueryMemberShipBinding_req req = new dt_Dyn_QueryMemberShipBinding_req();

            req.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
            req.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
            req.VGROUP = AppConfig.VGROUP; //销售组织
            req.DATA_SOURCE = AppConfig.DATA_SOURCE;//数据来源
            req.MOB_NUMBER = "15222196580";

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

            dt_Dyn_QueryMemberShipBinding_res res = WebHmjApiHelp.QueryMemberShipBinding(req, true);
        }

        /// <summary>
        /// 绑定关系同步接口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button10_Click(object sender, EventArgs e)
        {
            dt_DynMemberBunding_req req = new dt_DynMemberBunding_req();
            req.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
            req.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
            req.VGROUP = AppConfig.VGROUP; //销售组织
            req.DATA_SOURCE = AppConfig.DATA_SOURCE;//数据来源

            req.MEMBER_ID = "MCHM000000012";

            req.BRANDMEMBER = new dt_DynMemberBunding_reqITEM[]
            {
                new dt_DynMemberBunding_reqITEM()
                {
                    DATA_SOURCE2 ="0006",
                    LOYALTY_BRAND2 ="30",
                    MEMBER_ID2 ="MC30000166414",
                    VGROUP2 ="C003"
                },
                new dt_DynMemberBunding_reqITEM()
                {
                    DATA_SOURCE2 ="0002",
                    LOYALTY_BRAND2 ="28",
                    MEMBER_ID2 ="MC28002504008",
                    VGROUP2 ="C004"
                }
            };

            dt_DynMemberBunding_res res = WebHmjApiHelp.DynMemberBunding(req, true);
        }

        /// <summary>
        /// 完善信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button11_Click(object sender, EventArgs e)
        {
            dt_Dyn_UpdateMemberShip_req updates = new dt_Dyn_UpdateMemberShip_req();
            ZCRMT322_Dyn z = new ZCRMT322_Dyn();

            //大宝生日
            z.KID_BIRTHDAY = "1992-12-23";
            //二宝生日
            z.KID_BIRTHDAY2 = "1992-12-10";
            //从事行业
            z.ZC019 = "4";
            //收入范围
            z.ZC004 = "06";
            //何处了解华美家
            z.INFO_WANTED = "001,002";
            //婚姻状况
            z.ZC016 = "01";
            //品牌编号
            z.BRAND_PREF = "01,02,03";
            //皮肤特征
            z.ZA003 = "01";
            //皮肤问题
            z.ZA004 = "02,03";
            //品牌 编号
            z.BRAND_PREF = "";
            //品类编号
            z.CLASS_PREF = "";


            z.DATA_SOURCE = AppConfig.DATA_SOURCE;
            z.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
            z.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
            z.VGROUP = AppConfig.VGROUP; //销售组织

            z.ACCOUNT_ID = "18952435467";
            z.PARTNER = "MCHM000000003";
            updates.ZCRMT316 = new ZCRMT322_Dyn[] { z };

            dt_Dyn_UpdateMemberShip_res ups = WebHmjApiHelp.UpdateMemberShip(updates, true);
        }

        /// <summary>
        /// 积分增减
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button12_Click(object sender, EventArgs e)
        {
            //如果注册会员成功送积分
            //si_Dyn_ActCreateTel_obService == si_ActCreateTel_obService
            dt_Dyn_ActCreateTel_req Actreq = new dt_Dyn_ActCreateTel_req();
            // 数据源类型 
            Actreq.TYPE = "0000";
            //处理标识 
            Actreq.OBJECT_ID = DateTime.Now.ToString("yyyyMMddHHmmss") + CommonHelp.GetRand(10);
            //活动单据类型	
            Actreq.PROCESS_TYPE = "ZXY";
            //外部会员卡号	"会员卡号(如已传输手机号，可不用填写会员卡号)"
            Actreq.ACCOUNT_ID = "18952435467";
            Actreq.POSTING_DATE = DateTime.Today.ToString();
            //积分类型	
            Actreq.POINT_TYPE = "ZHMJF01";
            //积分数  要改
            Actreq.POINTS = -1;
            //单据全局活动ID
            //Actreq.CAMPAIGN_HE_ID = "CMP2820171023005";
            Actreq.CAMPAIGN_HE_ID = "";
            Actreq.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
            Actreq.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
            Actreq.VGROUP = AppConfig.VGROUP; //销售组织

            dt_Dyn_ActCreateTel_res Actres = WebHmjApiHelp.ActCreateTel(Actreq, true);
        }

        /// <summary>
        /// 微信关注传输
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button13_Click(object sender, EventArgs e)
        {
            dt_Dyn_WechatStateTran_req req = new dt_Dyn_WechatStateTran_req();
            req.OPENID = "openidtest";
            req.NEW_STATE = "2";//1：已关注 2：取消关注 0：:未关注

            req.DATA_SOURCE = AppConfig.DATA_SOURCE;
            req.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
            req.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
            req.VGROUP = AppConfig.VGROUP; //销售组织
            dt_Dyn_WechatStateTran_res res = WebHmjApiHelp.WechatStateTran(req, true);
        }

        /// <summary>
        /// 会员状态调整
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button14_Click(object sender, EventArgs e)
        {

            ////如果是待激活状态那么就要激活
            dt_Dyn_ChangeMemberStatus_req reqs = new dt_Dyn_ChangeMemberStatus_req();

            reqs.ZVTWEG = "102";
            reqs.STATUS_OLD = "E0005";
            reqs.STATUS_NEW = "E0001";
            reqs.ACCOUNT_ID = "15222196580";
            reqs.FLAG = "I";//激活

            reqs.DATA_SOURCE = AppConfig.DATA_SOURCE;
            reqs.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
            reqs.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
            reqs.VGROUP = AppConfig.VGROUP; //销售组织
            reqs.PASS_FLAG = "N";
            reqs.REASON = "激活";

            //dt_Dyn_ChangeMemberStatus_req req = new dt_Dyn_ChangeMemberStatus_req();
            //req.ZVTWEG = "102";
            ////req.STATUS_OLD = "E0000";
            ////req.STATUS_NEW = "E0001";
            //req.ACCOUNT_ID = "15222196580";
            //req.FLAG = "Q";//查询

            //req.DATA_SOURCE = AppConfig.DATA_SOURCE;
            //req.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
            //req.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
            //req.VGROUP = AppConfig.VGROUP; //销售组织
            //req.PASS_FLAG = "Y";
            //req.REASON = "查询密码";

            dt_Dyn_ChangeMemberStatus_res res = WebHmjApiHelp.ChangeMemberStatus(reqs, true);
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button15_Click(object sender, EventArgs e)
        {
            Random r = new Random();
            int num = r.Next(100000, 999999);
            //string message = string.Format("本次微信平台获取的验证码是：" + num);
            string message = string.Format("本次微信平台获取的验证码是："
                + num + "。如非本人操作，请致电400-821-6188");

            dt_SMSInsert_req req = new dt_SMSInsert_req();
            req.SMS_ITEM = new SMS_ITEM[] { new SMS_ITEM() { CONTENT = message,
                MESSAGEID = "0000001", MESSAGETYPE = "BC_WX_SMS",
                MOBILE = "18721170080", MSGFORMAT = "8", SRCNUMBER = "1069048560003" } };

            dt_SMSInsert_res res = WebHmjApiHelp.SMSInsert(req, true);
        }
    }
}
