using Hmj.Business.ServiceImpl;
using Hmj.Interface;
using Hmj.WebService;
using log4net;
using System;
using System.Web.Script.Serialization;

namespace Hmj.Business
{
    /// <summary>
    /// 调用web service
    /// </summary>
    public class WebApiHelp
    {
        //static Stopwatch sw = new Stopwatch();
        static JavaScriptSerializer js = new JavaScriptSerializer();
        private static ILog logger = LogManager.GetLogger("loginfo");

        private static ILogService datalog = new LogService();

        //private static ILog _logwarn = LogManager.GetLogger("logwarn");
        //private static ILog _logerror = LogManager.GetLogger("logerror");
        //private static ILog _logfatal = LogManager.GetLogger("logfatal");

        /// <summary>
        /// 会员快速查询
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static dt_Dyn_DispMemQuick_res DispMemQuick(dt_Dyn_DispMemQuick_req req)
        {
            try
            {
                si_Dyn_DispMemQuick_obService s2 = new si_Dyn_DispMemQuick_obService();

                string reqStr = js.Serialize(req);
                DateTime timeStart = DateTime.Now;

                logger.Info("会员信息快速查询si_Dyn_DispMemQuick_ob请求信息：" + reqStr);

                dt_Dyn_DispMemQuick_res dt2 = s2.si_Dyn_DispMemQuick_ob(req);
                string resStr = js.Serialize(dt2);
                DateTime timeEnd = DateTime.Now;

                logger.Info("响应信息" + resStr + "\r\n");


                datalog.DataInfo(reqStr, resStr, timeStart, timeEnd, 0, "si_Dyn_DispMemQuick_ob", "会员信息快速查询");
                return dt2;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 会员主数据查询
        ///  w.DATA_SOURCE = "0002";
        ///  w.VGROUP = "C004";//销售组织
        ///  w.LOYALTY_BRAND = "28";//忠诚度品牌
        ///  w.SOURCE_SYSTEM = "0003";//来源系统
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static dt_Dyn_DispMember_res DispMember(dt_Dyn_DispMember_req req)
        {
            try
            {
                si_Dyn_DispMember_obService web = new si_Dyn_DispMember_obService();

                string reqStr = js.Serialize(req);
                DateTime timeStart = DateTime.Now;

                logger.Info("会员主数据查询接口si_Dyn_DispMember_ob请求信息：" + reqStr);
                dt_Dyn_DispMember_res res = web.si_Dyn_DispMember_ob(req);

                string resStr = js.Serialize(res);
                DateTime timeEnd = DateTime.Now;

                logger.Info("响应信息" + resStr + "\r\n");


                datalog.DataInfo(reqStr, resStr, timeStart, timeEnd, 0, "si_Dyn_DispMember_ob", "会员主数据查询接口");

                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// (old)发送短信接口
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static dt_SMSInsert_res OldSMSInsert(dt_SMSInsert_req req)
        {
            try
            {
                string reqStr = js.Serialize(req);
                DateTime timeStart = DateTime.Now;
                logger.Info("si_SMSInsert_real_obService请求信息new：" + reqStr + "\r\n");

                si_SMSInsert_real_obService web = new si_SMSInsert_real_obService();

                dt_SMSInsert_res res = web.si_SMSInsert_real_ob(req);

                string resStr = js.Serialize(res);
                DateTime timeEnd = DateTime.Now;
                logger.Info("响应信息" + resStr + "\r\n");


                datalog.DataInfo(reqStr, resStr, timeStart, timeEnd, 0, "si_SMSInsert_real_obService", "发送短信接口");
                return res;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        /// <summary>
        /// 会员时时修改
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static dt_Dyn_UpdateMemberShip_res UpdateMemberShip(dt_Dyn_UpdateMemberShip_req req)
        {
            try
            {
                si_Dyn_UpdateMemberShip_obService web = new si_Dyn_UpdateMemberShip_obService();

                string reqStr = js.Serialize(req);
                DateTime timeStart = DateTime.Now;
                logger.Info("会员实时修改接口si_Dyn_UpdateMemberShip_obService请求信息：" + reqStr);

                dt_Dyn_UpdateMemberShip_res res = web.si_Dyn_UpdateMemberShip_ob(req);

                string resStr = js.Serialize(res);
                DateTime timeEnd = DateTime.Now;
                logger.Info("响应信息" + resStr + "\r\n");

                datalog.DataInfo(reqStr, resStr, timeStart, timeEnd, 0, "si_Dyn_UpdateMemberShip_obService", "会员实时修改接口");
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// 关注/取关
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static dt_Dyn_WechatStateTran_res WechatStateTran(dt_Dyn_WechatStateTran_req req)
        {
            try
            {
                string reqStr = js.Serialize(req);
                DateTime timeStart = DateTime.Now;
                logger.Info("微信关注数据传输si_Dyn_WechatStateTran_obService请求信息：" + reqStr);
                si_Dyn_WechatStateTran_obService web = new si_Dyn_WechatStateTran_obService();

                dt_Dyn_WechatStateTran_res res = web.si_Dyn_WechatStateTran_ob(req);

                string resStr = js.Serialize(res);
                DateTime timeEnd = DateTime.Now;
                logger.Info("响应信息" + resStr + "\r\n");

                datalog.DataInfo(reqStr, resStr, timeStart, timeEnd, 0, "si_Dyn_WechatStateTran_obService", "微信关注数据传输");
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// 创建会员
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static dt_Dyn_UploadMemberShip_res CreateMemberShip(dt_Dyn_UploadMemberShip_req req)
        {
            try
            {
                string reqStr = js.Serialize(req);
                DateTime timeStart = DateTime.Now;
                logger.Info("微信注册会员si_Dyn_CreateMemberShip_obService请求信息：" + reqStr);
                si_Dyn_CreateMemberShip_obService web = new si_Dyn_CreateMemberShip_obService();

                dt_Dyn_UploadMemberShip_res res = web.si_Dyn_CreateMemberShip_ob(req);

                string resStr = js.Serialize(res);
                DateTime timeEnd = DateTime.Now;
                logger.Info("响应信息" + resStr + "\r\n");

                datalog.DataInfo(reqStr, resStr, timeStart, timeEnd, 0, "si_Dyn_CreateMemberShip_obService", "微信注册会员");
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// 创建潜客
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static dt_Dyn_CreateLead_res CreateLead(dt_Dyn_CreateLead_req req)
        {
            try
            {
                string reqStr = js.Serialize(req);
                DateTime timeStart = DateTime.Now;
                logger.Info("微信潜客创建si_Dyn_CreateLead_obService请求信息：" + reqStr);
                si_Dyn_CreateLead_obService web = new si_Dyn_CreateLead_obService();

                dt_Dyn_CreateLead_res res = web.si_Dyn_CreateLead_ob(req);

                string resStr = js.Serialize(res);
                DateTime timeEnd = DateTime.Now;
                logger.Info("响应信息" + resStr + "\r\n");

                datalog.DataInfo(reqStr, resStr, timeStart, timeEnd, 0, "si_Dyn_CreateLead_obService", "微信潜客创建");
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// 会员状态调整
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static dt_Dyn_ChangeMemberStatus_res ChangeMemberStatus(dt_Dyn_ChangeMemberStatus_req req)
        {
            try
            {
                string reqStr = js.Serialize(req);
                DateTime timeStart = DateTime.Now;
                logger.Info("会员状态调整si_Dyn_ChangeMemberStatus_obService请求信息：" + reqStr);
                si_Dyn_ChangeMemberStatus_obService web = new si_Dyn_ChangeMemberStatus_obService();

                dt_Dyn_ChangeMemberStatus_res res = web.si_Dyn_ChangeMemberStatus_ob(req);

                string resStr = js.Serialize(res);
                DateTime timeEnd = DateTime.Now;
                logger.Info("响应信息" + resStr + "\r\n");

                datalog.DataInfo(reqStr, resStr, timeStart, timeEnd, 0, "si_Dyn_ChangeMemberStatus_obService", "会员状态调整");
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// 积分加减接口
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static dt_Dyn_ActCreateTel_res ActCreateTel(dt_Dyn_ActCreateTel_req req)
        {
            try
            {
                string reqStr = js.Serialize(req);
                DateTime timeStart = DateTime.Now;
                logger.Info("积分加减接口si_Dyn_ActCreateTel_obService请求信息：" + reqStr);
                si_Dyn_ActCreateTel_obService web = new si_Dyn_ActCreateTel_obService();

                dt_Dyn_ActCreateTel_res res = web.si_ActCreateTel_ob(req);

                string resStr = js.Serialize(res);
                DateTime timeEnd = DateTime.Now;
                logger.Info("响应信息" + resStr + "\r\n");

                datalog.DataInfo(reqStr, resStr, timeStart, timeEnd, 0, "si_Dyn_ActCreateTel_obService", "积分加减接口");
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// 会员密码变更
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static dt_Dyn_ChangePassword_res ChangePassword(dt_Dyn_ChangePassword_req req)
        {
            try
            {
                si_Dyn_ChangePassword_obService pi = new si_Dyn_ChangePassword_obService();

                string reqStr = js.Serialize(req);
                DateTime timeStart = DateTime.Now;
                logger.Info("会员密码变更si_Dyn_ChangePassword_obService请求信息：" + reqStr);

                dt_Dyn_ChangePassword_res pis = pi.si_Dyn_ChangePassword_ob(req);

                string resStr = js.Serialize(pis);
                DateTime timeEnd = DateTime.Now;
                logger.Info("响应信息" + resStr + "\r\n");

                datalog.DataInfo(reqStr, resStr, timeStart, timeEnd, 0, "si_Dyn_ChangePassword_obService", "会员密码变更");
                return pis;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 会员积分明细查询接口
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static dt_Dyn_GetPointDetail_res GetPointDetail(dt_Dyn_GetPointDetail_req req)
        {
            try
            {
                si_Dyn_GetPointDetail_obService pi = new si_Dyn_GetPointDetail_obService();

                string reqStr = js.Serialize(req);
                DateTime timeStart = DateTime.Now;
                logger.Info("会员积分明细查询接口si_Dyn_GetPointDetail_obService请求信息：" + reqStr);

                dt_Dyn_GetPointDetail_res pis = pi.si_Dyn_GetPointDetail_ob(req);

                string resStr = js.Serialize(pis);
                DateTime timeEnd = DateTime.Now;
                logger.Info("响应信息" + resStr + "\r\n");

                datalog.DataInfo(reqStr, resStr, timeStart, timeEnd, 0, "si_Dyn_GetPointDetail_obService", "会员积分明细查询接口");
                return pis;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 会员优惠券账户查询接口
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static dt_Dyn_PosCpSearch_res PosCpSearch(dt_Dyn_PosCpSearch_req req)
        {
            string reqStr = "";
            string resStr = "";
            DateTime? timeStart = null;
            DateTime? timeEnd = null;
            try
            {
                si_Dyn_PosCpSearch_obService dyn = new si_Dyn_PosCpSearch_obService();

                reqStr = js.Serialize(req);
                timeStart = DateTime.Now;
                logger.Info("会员优惠券账户查询接口si_Dyn_PosCpSearch_obService请求信息：" + reqStr);

                dt_Dyn_PosCpSearch_res pis = dyn.si_PosCpSearch_ob(req);

                 resStr = js.Serialize(pis);
                timeEnd = DateTime.Now;
                logger.Info("响应信息" + resStr + "\r\n");


                return pis;
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                resStr = ex.Message;
                logger.Info("响应信息" + resStr + "\r\n");
                return null;

            }
            finally {
                datalog.DataInfo(reqStr, resStr, timeStart, timeEnd, 0, "si_Dyn_PosCpSearch_obService", "会员优惠券账户查询接口");
            }
        }

        /// <summary>
        /// 门店同步
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static dt_GetMd_res GetMd(dt_GetMd_req req)
        {
            try
            {
                si_GetMd_obService dyn = new si_GetMd_obService();

                string reqStr = js.Serialize(req);
                DateTime timeStart = DateTime.Now;
                logger.Info("门店同步接口si_GetMd_obService请求信息：" + reqStr);

                dt_GetMd_res pis = dyn.si_GetMd_ob(req);

                string resStr = js.Serialize(pis);
                DateTime timeEnd = DateTime.Now;
                logger.Info("响应信息" + resStr + "\r\n");

                datalog.DataInfo(reqStr, resStr, timeStart, timeEnd, 0, "si_GetMd_obService", "门店同步接口");
                return pis;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
