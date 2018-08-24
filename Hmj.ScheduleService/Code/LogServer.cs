using NLog;

namespace Hmj.ScheduleService.Code
{
    public class LogService
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
        private static Logger _logger = LogManager.GetLogger("Service");
        public static void Trace(string msg)
        {
            _logger.Trace(msg);
        }
        public static void Debug(string msg)
        {
            _logger.Debug(msg);
        }
        public static void Info(string msg)
        {
            _logger.Info(msg);
        }

        public static void Warn(string msg)
        {
            _logger.Warn(msg);
        }
        public static void Error(string msg)
        {
            _logger.Error(msg);
        }
        public static void Fatal(string msg)
        {
            _logger.Fatal(msg);
        }


    }
}
