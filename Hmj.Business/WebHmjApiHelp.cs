using Hmj.Business.ServiceImpl;
using Hmj.Interface;
using HmjNew.Service;
using log4net;
using System;
using System.Web.Script.Serialization;
using VIPSystemV2.Entity;

namespace Hmj.Business
{
    /// <summary>
    /// 调用web service
    /// </summary>
    public class WebHmjApiHelp
    {
        //static Stopwatch sw = new Stopwatch();
        static JavaScriptSerializer js = new JavaScriptSerializer();
        private static ILog logger = LogManager.GetLogger("loginfo");

        private static ILogService datalog = new LogService();

        //private static ILog _logwarn = LogManager.GetLogger("logwarn");
        //private static ILog _logerror = LogManager.GetLogger("logerror");
        //private static ILog _logfatal = LogManager.GetLogger("logfatal");

        /// <summary>
        /// 创建会员
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static dt_Dyn_CreateHMJMemberShip_res CreateMemberShip(dt_Dyn_CreateHMJMemberShip_req req,
            bool isdebug = false)
        {
            string reqStr = js.Serialize(req);
            string resStr = string.Empty;
            DateTime? timeStart = null;
            DateTime? timeEnd = null;
            string isok = "OK";

            try
            {
                timeStart = DateTime.Now;
                logger.Info("微信注册会员si_Dyn_CreateMemberShip_obService请求信息：" + reqStr);
                si_Dyn_CreateHMJMemberShip_obService web = new si_Dyn_CreateHMJMemberShip_obService();

                dt_Dyn_CreateHMJMemberShip_res res = web.si_Dyn_CreateHMJMemberShip_ob(req);

                resStr = js.Serialize(res);
                timeEnd = DateTime.Now;
                logger.Info("响应信息" + resStr + "\r\n");

                if (res.WV_RETURN == "N")
                {
                    isok = "NO";
                }

                return res;
            }
            catch (Exception ex)
            {
                resStr = ex.Message;
                isok = "NO";
                return null;
            }
            finally
            {
                if (!isdebug)
                {
                    datalog.DataInfo(reqStr, resStr, timeStart, timeEnd, 0, SystemCode.CreateMemberShip.ToString(), isok);
                }
            }
        }

        /// <summary>
        /// 潜客创建
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static dt_Dyn_CreateLead_res CreateLead(dt_Dyn_CreateLead_req req,
            bool isdebug = false)
        {
            string reqStr = js.Serialize(req);
            string resStr = string.Empty;
            DateTime? timeStart = null;
            DateTime? timeEnd = null;
            string isok = "OK";

            try
            {
                timeStart = DateTime.Now;
                logger.Info("潜客创建si_Dyn_CreateLead_obService请求信息：" + reqStr);
                si_Dyn_CreateLead_obService web = new si_Dyn_CreateLead_obService();

                dt_Dyn_CreateLead_res res = web.si_Dyn_CreateLead_ob(req);

                resStr = js.Serialize(res);
                timeEnd = DateTime.Now;
                logger.Info("响应信息" + resStr + "\r\n");

                if (res.WV_RETURN == "N")
                {
                    isok = "NO";
                }

                return res;
            }
            catch (Exception ex)
            {
                resStr = ex.Message;
                isok = "NO";
                return null;
            }
            finally
            {
                if (!isdebug)
                {
                    datalog.DataInfo(reqStr, resStr, timeStart, timeEnd, 0, SystemCode.CreateLead.ToString(), isok);
                }
            }

        }

        /// <summary>
        /// 主数据查询
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static dt_Dyn_DispMember_res DispMember(dt_Dyn_DispMember_req req,
            bool isdebug = false)
        {
            string reqStr = js.Serialize(req);
            string resStr = string.Empty;
            DateTime? timeStart = null;
            DateTime? timeEnd = null;
            string isok = "OK";

            try
            {
                timeStart = DateTime.Now;
                logger.Info("会员主数据查询si_Dyn_DispMember_obService请求信息：" + reqStr);
                si_Dyn_DispMember_obService web = new si_Dyn_DispMember_obService();

                dt_Dyn_DispMember_res res = web.si_Dyn_DispMember_ob(req);

                resStr = js.Serialize(res);
                timeEnd = DateTime.Now;
                logger.Info("响应信息" + resStr + "\r\n");

                if (res == null)
                {
                    isok = "NO";
                }
                return res;
            }
            catch (Exception ex)
            {
                resStr = ex.Message;
                isok = "NO";
                return null;
            }
            finally
            {
                if (!isdebug)
                {
                    datalog.DataInfo(reqStr, resStr, timeStart, timeEnd, 0, SystemCode.DispMember.ToString(), isok);
                }
            }

        }

        /// <summary>
        /// 会员快速查询
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static dt_Dyn_DispMemQuick_res DispMemQuick(dt_Dyn_DispMemQuick_req req,
            bool isdebug = false)
        {
            string reqStr = js.Serialize(req);
            string resStr = string.Empty;
            DateTime? timeStart = null;
            DateTime? timeEnd = null;
            string isok = "OK";

            try
            {
                timeStart = DateTime.Now;
                logger.Info("会员快速查询si_Dyn_DispMemQuick_obService请求信息：" + reqStr);
                si_Dyn_DispMemQuick_obService web = new si_Dyn_DispMemQuick_obService();

                dt_Dyn_DispMemQuick_res res = web.si_Dyn_DispMemQuick_ob(req);

                resStr = js.Serialize(res);
                timeEnd = DateTime.Now;
                logger.Info("响应信息" + resStr + "\r\n");

                if (res.ZTYPE == "N")
                {
                    isok = "NO";
                }

                return res;
            }
            catch (Exception ex)
            {
                resStr = ex.Message;
                isok = "NO";
                return null;
            }
            finally
            {
                if (!isdebug)
                {
                    datalog.DataInfo(reqStr, resStr, timeStart, timeEnd, 0, SystemCode.DispMemQuick.ToString(), isok);
                }
            }

        }

        /// <summary>
        /// 会员实时修改
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static dt_Dyn_UpdateMemberShip_res UpdateMemberShip(dt_Dyn_UpdateMemberShip_req req,
            bool isdebug = false)
        {
            string reqStr = js.Serialize(req);
            string resStr = string.Empty;
            DateTime? timeStart = null;
            DateTime? timeEnd = null;
            string isok = "OK";

            try
            {
                timeStart = DateTime.Now;
                logger.Info("会员实时修改si_Dyn_UpdateMemberShip_obService请求信息：" + reqStr);
                si_Dyn_UpdateMemberShip_obService web = new si_Dyn_UpdateMemberShip_obService();

                dt_Dyn_UpdateMemberShip_res res = web.si_Dyn_UpdateMemberShip_ob(req);

                resStr = js.Serialize(res);
                timeEnd = DateTime.Now;
                logger.Info("响应信息" + resStr + "\r\n");

                if (res.WV_RETURN == "N")
                {
                    isok = "NO";
                }

                return res;
            }
            catch (Exception ex)
            {
                resStr = ex.Message;
                isok = "NO";
                return null;
            }
            finally
            {
                if (!isdebug)
                {
                    datalog.DataInfo(reqStr, resStr, timeStart, timeEnd, 0, SystemCode.UpdateMemberShip.ToString(), isok);
                }
            }

        }

        /// <summary>
        /// 会员积分明细查询
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static dt_Dyn_GetPointDetail_res GetPointDetail(dt_Dyn_GetPointDetail_req req,
            bool isdebug = false)
        {
            string reqStr = js.Serialize(req);
            string resStr = string.Empty;
            DateTime? timeStart = null;
            DateTime? timeEnd = null;
            string isok = "OK";

            try
            {
                timeStart = DateTime.Now;
                logger.Info("会员积分明细查询si_Dyn_GetPointDetail_obService请求信息：" + reqStr);
                si_Dyn_GetPointDetail_obService web = new si_Dyn_GetPointDetail_obService();

                dt_Dyn_GetPointDetail_res res = web.si_Dyn_GetPointDetail_ob(req);

                resStr = js.Serialize(res);
                timeEnd = DateTime.Now;
                logger.Info("响应信息" + resStr + "\r\n");

                if (res == null)
                {
                    isok = "NO";
                }

                return res;
            }
            catch (Exception ex)
            {
                resStr = ex.Message;
                isok = "NO";
                return null;
            }
            finally
            {
                if (!isdebug)
                {
                    datalog.DataInfo(reqStr, resStr, timeStart, timeEnd, 0, SystemCode.GetPointDetail.ToString(), isok);
                }
            }
        }

        /// <summary>
        /// 会员绑定查询品牌会员接口
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static dt_Dyn_QueryMemberShipBinding_res QueryMemberShipBinding(dt_Dyn_QueryMemberShipBinding_req req,
            bool isdebug = false)
        {
            string reqStr = js.Serialize(req);
            string resStr = string.Empty;
            DateTime? timeStart = null;
            DateTime? timeEnd = null;
            string isok = "OK";

            try
            {
                timeStart = DateTime.Now;
                logger.Info("会员绑定查询品牌会员接口si_Dyn_QueryMemberShipBinding_obService请求信息：" + reqStr);
                si_Dyn_QueryMemberShipBinding_obService web = new si_Dyn_QueryMemberShipBinding_obService();

                dt_Dyn_QueryMemberShipBinding_res res = web.si_Dyn_QueryMemberShipBinding_ob(req);

                resStr = js.Serialize(res);
                timeEnd = DateTime.Now;
                logger.Info("响应信息" + resStr + "\r\n");

                if (res == null)
                {
                    isok = "NO";
                }

                return res;
            }
            catch (Exception ex)
            {
                resStr = ex.Message;
                isok = "NO";
                return null;
            }
            finally
            {
                if (!isdebug)
                {
                    datalog.DataInfo(reqStr, resStr, timeStart, timeEnd, 0, SystemCode.QueryMemberShipBinding.ToString(), isok);
                }
            }
        }

        /// <summary>
        /// 绑定关系同步接口
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static dt_DynMemberBunding_res DynMemberBunding(dt_DynMemberBunding_req req,
            bool isdebug = false)
        {
            string reqStr = js.Serialize(req);
            string resStr = string.Empty;
            DateTime? timeStart = null;
            DateTime? timeEnd = null;
            string isok = "OK";

            try
            {
                timeStart = DateTime.Now;
                logger.Info("绑定关系同步接口si_DynMemberBunding_obService请求信息：" + reqStr);
                si_DynMemberBunding_obService web = new si_DynMemberBunding_obService();

                dt_DynMemberBunding_res res = web.si_DynMemberBunding_ob(req);

                resStr = js.Serialize(res);
                timeEnd = DateTime.Now;
                logger.Info("响应信息" + resStr + "\r\n");

                if (res.ZRETURN == "N")
                {
                    isok = "NO";
                }

                return res;
            }
            catch (Exception ex)
            {
                resStr = ex.Message;
                isok = "NO";
                return null;
            }
            finally
            {
                if (!isdebug)
                {
                    datalog.DataInfo(reqStr, resStr, timeStart, timeEnd, 0, SystemCode.DynMemberBunding.ToString(), isok);
                }
            }
        }

        /// <summary>
        /// 积分加减接口
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static dt_Dyn_ActCreateTel_res ActCreateTel(dt_Dyn_ActCreateTel_req req,
            bool isdebug = false)
        {
            string reqStr = js.Serialize(req);
            string resStr = string.Empty;
            DateTime? timeStart = null;
            DateTime? timeEnd = null;
            string isok = "OK";

            try
            {
                timeStart = DateTime.Now;
                logger.Info("积分加减接口si_Dyn_ActCreateTel_obService请求信息：" + reqStr);
                si_Dyn_ActCreateTel_obService web = new si_Dyn_ActCreateTel_obService();

                dt_Dyn_ActCreateTel_res res = web.si_ActCreateTel_ob(req);

                resStr = js.Serialize(res);
                timeEnd = DateTime.Now;
                logger.Info("响应信息" + resStr + "\r\n");

                if (res == null)
                {
                    isok = "NO";
                }

                return res;
            }
            catch (Exception ex)
            {
                resStr = ex.Message;
                isok = "NO";
                return null;
            }
            finally
            {
                if (!isdebug)
                {
                    datalog.DataInfo(reqStr, resStr, timeStart, timeEnd, 0, SystemCode.ActCreateTel.ToString(), isok);
                }
            }

        }

        /// <summary>
        /// 关注信息传输
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static dt_Dyn_WechatStateTran_res WechatStateTran(dt_Dyn_WechatStateTran_req req,
            bool isdebug = false)
        {
            string reqStr = js.Serialize(req);
            string resStr = string.Empty;
            DateTime? timeStart = null;
            DateTime? timeEnd = null;
            string isok = "OK";

            try
            {
                timeStart = DateTime.Now;
                logger.Info("关注信息传输si_Dyn_WechatStateTran_obService请求信息：" + reqStr);
                si_Dyn_WechatStateTran_obService web = new si_Dyn_WechatStateTran_obService();

                dt_Dyn_WechatStateTran_res res = web.si_Dyn_WechatStateTran_ob(req);

                resStr = js.Serialize(res);
                timeEnd = DateTime.Now;
                logger.Info("响应信息" + resStr + "\r\n");

                if (res == null)
                {
                    isok = "NO";
                }

                return res;
            }
            catch (Exception ex)
            {
                resStr = ex.Message;
                isok = "NO";
                return null;
            }
            finally
            {
                if (!isdebug)
                {
                    datalog.DataInfo(reqStr, resStr, timeStart, timeEnd, 0, SystemCode.WechatStateTran.ToString(), isok);
                }
            }

        }

        /// <summary>
        /// 会员状态调整
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static dt_Dyn_ChangeMemberStatus_res ChangeMemberStatus(dt_Dyn_ChangeMemberStatus_req req,
            bool isdebug = false)
        {
            string reqStr = js.Serialize(req);
            string resStr = string.Empty;
            DateTime? timeStart = null;
            DateTime? timeEnd = null;
            string isok = "OK";

            try
            {
                timeStart = DateTime.Now;
                logger.Info("会员状态调整si_Dyn_ChangeMemberStatus_obService请求信息：" + reqStr);
                si_Dyn_ChangeMemberStatus_obService web = new si_Dyn_ChangeMemberStatus_obService();

                dt_Dyn_ChangeMemberStatus_res res = web.si_Dyn_ChangeMemberStatus_ob(req);

                resStr = js.Serialize(res);
                timeEnd = DateTime.Now;
                logger.Info("响应信息" + resStr + "\r\n");

                if (res == null)
                {
                    isok = "NO";
                }

                return res;
            }
            catch (Exception ex)
            {
                resStr = ex.Message;
                isok = "NO";
                return null;
            }
            finally
            {
                if (!isdebug)
                {
                    datalog.DataInfo(reqStr, resStr, timeStart, timeEnd, 0, SystemCode.ChangeMemberStatus.ToString(), isok);
                }
            }

        }

        /// <summary>
        /// 会员状态调整
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static dt_SMSInsert_res SMSInsert(dt_SMSInsert_req req,
            bool isdebug = false)
        {
            string reqStr = js.Serialize(req);
            string resStr = string.Empty;
            DateTime? timeStart = null;
            DateTime? timeEnd = null;
            string isok = "OK";

            try
            {
                timeStart = DateTime.Now;
                logger.Info("发送短信si_SMSInsert_real_obService请求信息：" + reqStr);
                si_SMSInsert_real_obService web = new si_SMSInsert_real_obService();

                dt_SMSInsert_res res = web.si_SMSInsert_real_ob(req);

                resStr = js.Serialize(res);
                timeEnd = DateTime.Now;
                logger.Info("响应信息" + resStr + "\r\n");

                if (res == null)
                {
                    isok = "NO";
                }

                return res;
            }
            catch (Exception ex)
            {
                resStr = ex.Message;
                isok = "NO";
                return null;
            }
            finally
            {
                if (!isdebug)
                {
                    datalog.DataInfo(reqStr, resStr, timeStart, timeEnd, 0, SystemCode.SMSInsert.ToString(), isok);
                }
            }

        }
    }
}
