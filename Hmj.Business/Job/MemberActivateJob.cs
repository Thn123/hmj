using FluentScheduler;
using Hmj.Business.ServiceImpl;
using Hmj.DataAccess.Repository;
using Hmj.Entity;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hmj.Business.Job
{
    public class MemberActivateJob : Registry
    {
        private object obj = new object();
        static LogReponsitory _logservice = new LogReponsitory();
        static HmjMemberService mymember = new HmjMemberService();

        private static ILog logger = LogManager.GetLogger("loginfo");
        private static ILog _logdebug = LogManager.GetLogger("logdebug");

        public MemberActivateJob()
        {
            Registry registry = new Registry();
            registry.Schedule(() => ScheduleJob()).WithName("MemberActivateJob")
                .ToRunNow().AndEvery(30).Minutes();
            
            JobManager.Initialize(registry);
        }

        
      
        private void ScheduleJob()
        {
            List<MEMBER_CHANGESTATUS_LOG> wxTmp = _logservice.QueryMemberActivateList();

            int mylen = wxTmp.Count;

            int len = (int)Math.Ceiling(mylen * 1.0m / 1000);

            _logdebug.Info("执行会员激活定时任务，当前时间：" + DateTime.Now + "；发送的数量" + mylen + "；");

            for (int i = 0; i < len; i++)
            {
                List<MEMBER_CHANGESTATUS_LOG> arry = wxTmp.OrderBy(a => a.ID).Skip(i * 1000).Take(1000).ToList();

                SendTmpsByMember(arry);
            }
        }

        /// <summary>
        /// 自己封装耗时的方法
        /// </summary>
        /// <returns></returns>
        public static Task SendTmpsByMember(List<MEMBER_CHANGESTATUS_LOG> his)
        {
            return Task.Run(() =>
            {
                foreach (MEMBER_CHANGESTATUS_LOG item in his)
                {
                    mymember.ChageSatus(item.MOBILE);
                    _logservice.UpdateMemberActivateUse(item.ID);//不管是否成功，都将used改成true，因为在ChageSatus方法中，如果失败了会插入一条新的记录
                }
            });
        }
    }
}
