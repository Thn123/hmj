using System;
using System.IO;
using System.Text;

namespace Hmj.ScheduleService.Code
{
    /// <summary>
    /// 辅助类
    /// </summary>
    public class Helper
    {
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="content"></param>
        public static void WriteLog(string content)
        {
            if (string.IsNullOrEmpty(content.Trim()))
            {
                return;
            }

            string filepath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            if (!Directory.Exists(filepath + "Log"))
            {
                Directory.CreateDirectory(filepath + "Log");
            }

            FileStream fs = new FileStream(filepath + @"Log\Log" + DateTime.Now.ToString("yyyyMMdd") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);

            StreamWriter m_streamWriter = new StreamWriter(fs, Encoding.UTF8);

            m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);

            m_streamWriter.WriteLine(DateTime.Now.ToString() + "\t--->\t\r" + content + "\r\n");
            m_streamWriter.WriteLine("-----------------------------------------------------------------");
            m_streamWriter.Flush();
            m_streamWriter.Close();
            fs.Close();
        }

    }
}
