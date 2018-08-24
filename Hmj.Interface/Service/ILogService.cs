using System;

namespace Hmj.Interface
{
    public interface ILogService
    {
        /// <summary>
        /// Debugs the specified MSG.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        void Debug(string msg);
        /// <summary>
        /// Traces the specified MSG.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        void Trace(string msg);

        void Info(string msg);
        /// <summary>
        /// Warns the specified MSG.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        void Warn(string msg);
        /// <summary>
        /// Errors the specified MSG.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        void Error(string msg);
        /// <summary>
        /// Fatals the specified MSG.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        void Fatal(string msg);
        void DataInfo(string reqStr, string resStr, DateTime? timeStart, DateTime? timeEnd, int v1, string v2, string v3);
    }
}
