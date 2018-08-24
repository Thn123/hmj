using FluentScheduler;
using Hmj.Business.ServiceImpl;
using Hmj.Common;
using Hmj.Common.Utils;
using Hmj.DataAccess.Repository;
using Hmj.Entity;
using Hmj.Entity.Entities;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatCRM.Common.Utils;

namespace Hmj.Business.Job
{
    public class SendTmpMessageJob: Registry
    {
        private object obj = new object();
        static LogReponsitory _logservice = new LogReponsitory();
        static HmjMemberService mymember = new HmjMemberService();

        private static ILog logger = LogManager.GetLogger("loginfo");
        private static ILog _logdebug = LogManager.GetLogger("logdebug");

        public SendTmpMessageJob()
        {
            Registry registry = new Registry();
            registry.Schedule(() => ScheduleJob()).WithName("SendTmpMessageJob")
                 .ToRunEvery(1).Days().At(10, 00);

            JobManager.Initialize(registry);
        }



        private void ScheduleJob()
        {
            List<WX_TMP_HIS> wxTmp = mymember.GetWxTmpHisByIsSend(0);

            int mylen = wxTmp.Count;

            int len = (int)Math.Ceiling(mylen * 1.0m / 1000);

            _logdebug.Info("执行模板消息发送定时任务，当前时间：" + DateTime.Now + "；发送的数量" + mylen + "；");

            for (int i = 0; i < len; i++)
            {
                List<WX_TMP_HIS> arry = wxTmp.OrderBy(a => a.ID).Skip(i * 1000).Take(1000).ToList();

                SendTmpsByMember(arry);
            }
        }

        /// <summary>
        /// 自己封装耗时的方法
        /// </summary>
        /// <returns></returns>
        public static Task SendTmpsByMember(List<WX_TMP_HIS> his)
        {
            return Task.Run(() =>
            {
                foreach (WX_TMP_HIS item in his)
                {
                    string resmsg = NetHelper.HttpRequest(AppConfig.BeautyChinaWebApp, "", "GET", 2000,
                      Encoding.UTF8, "application/json");
                    string tmpStr = item.DETAIL;
                    TokeRes toke = JsonHelper.DeserializeObject<TokeRes>(resmsg);
                    string resot = string.Empty;
                    resot = NetHelper.HttpRequest("https://api.weixin.qq.com/cgi-bin/message/template/send?access_token="
                  + toke.Access_Token, tmpStr, "POST", 2000, Encoding.UTF8, "application/json");


                    //如果发送成功
                    if (resot.Contains("ok"))
                    {
                        _logdebug.Info("执行模板消息发送定时任务，当前时间：" + DateTime.Now + "；发送结果：成功，发送内容：" + tmpStr);
                        mymember.UpdateWxTmpHisIsSendByID(item.ID, 1, resot);
                    }
                    else
                    {
                        _logdebug.Info("执行模板消息发送定时任务，当前时间：" + DateTime.Now + "；发送结果：失败，发送内容：" + tmpStr);
                        mymember.UpdateWxTmpHisIsSendByID(item.ID, 1, resot);
                    }

                }
            });
        }
    }
}
