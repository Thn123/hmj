using Hmj.DataAccess.Repository;
using Hmj.Entity;
using Hmj.Interface;
using NLog;
using System;
using System.Collections.Generic;

namespace Hmj.Business.ServiceImpl
{
    public class LogService : ILogService
    {
        /*
            logger.Trace("Sample trace message");
            logger.Debug("Sample debug message");
            logger.Info("Sample informational message");
            logger.Warn("Sample warning message");
            logger.Error("Sample error message");
            logger.Fatal("Sample fatal error message");

            // alternatively you can call the Log() method 
            // and pass log level as the parameter.
            logger.Log(LogLevel.Info, "Sample fatal error message");
        */
        private static Logger _logger = LogManager.GetLogger("SPACRM");
        LogReponsitory log;
        public LogService()
        {
            log = new LogReponsitory();
        }


        public void Trace(string msg)
        {
            _logger.Trace(msg);
        }
        public void Debug(string msg)
        {
            _logger.Debug(msg);
        }
        public void Info(string msg)
        {
            _logger.Info(msg);
        }

        public void Warn(string msg)
        {
            _logger.Warn(msg);
        }
        public void Error(string msg)
        {
            _logger.Error(msg);
        }
        public void Fatal(string msg)
        {
            _logger.Fatal(msg);
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="reqStr"></param>
        /// <param name="resStr"></param>
        /// <param name="timeStart"></param>
        /// <param name="timeEnd"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        public void DataInfo(string reqStr, string resStr, DateTime? timeStart, DateTime? timeEnd, int type, string code, string code_name)
        {
            SYSTEM_LOG logs = new SYSTEM_LOG()
            {
                CODE = code,
                CODE_NAME = code_name,
                BEGIN_TIME = timeStart,
                END_TIME = timeEnd,
                TYPE = type,
                LOG_MEG_RETURN = resStr,
                LOG_MSG_SEND = reqStr
            };

            log.Insert(logs);
        }

        /// <summary>
        /// 将会员状态更改为“激活”时失败的信息记录下来
        /// </summary>
        /// <param name="ztype"></param>
        /// <param name="status"></param>
        /// <param name="message"></param>
        /// <param name="zpass"></param>
        /// <param name="mobile"></param>
        public void ChangeStatusLog(string ztype, string status, string message, string zpass, string mobile)
        {
            MEMBER_CHANGESTATUS_LOG logs = new MEMBER_CHANGESTATUS_LOG()
            {
                ZTYPE = ztype,
                STATUS = status,
                MESSAGE = message,
                ZPASS = zpass,
                MOBILE = mobile,
                CREATE_DATE = DateTime.Now
            };

            log.Insert(logs);
        }

        public List<MEMBER_CHANGESTATUS_LOG> QueryMemberActivateList()
        {
            return log.QueryMemberActivateList();
        }
    }
}
