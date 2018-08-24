using Hmj.Common;
using Hmj.DataAccess.Repository;
using Hmj.Entity;
using FluentScheduler;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using StructureMap;
using Hmj.Interface;
using HmjNew.Service;
using Hmj.Business.ServiceImpl;
using Hmj.Entity.SearchEntity;
using System.Diagnostics;

namespace Hmj.Business
{
    public class MySendTmpList : Registry
    {
        private object obj = new object();
        BcjStoreRepository _bcjstor = new BcjStoreRepository();
        static HmjMemberService mymember = new HmjMemberService();
        private static ILog logger = LogManager.GetLogger("loginfo");
        private static ILog _logdebug = LogManager.GetLogger("logdebug");

        public MySendTmpList()
        {
            Registry registry = new Registry();
            //registry.Schedule(() =>
            //Moethd()).WithName("Moethd").ToRunNow().AndEvery(100).Seconds();

            registry.Schedule(() => ScheduleJob()).WithName("ScheduleJob")
                .ToRunEvery(1).Days().At(10, 00);

            //JobManager.Initialize(registry);
            JobManager.Initialize(registry);
            //Schedule<NewJobSend>().ToRunNow().AndEvery(2).Seconds();
        }

        /// <summary>
        /// 每天早上10点，对注册/绑定会员未进行过品牌转换的用户发送模板消息
        /// </summary>
        private void ScheduleJob()
        {
            List<MEMBER_EX> wxTmp = _bcjstor.GetMembers();

            int mylen = wxTmp.Count;

            int len = (int)Math.Ceiling(mylen * 1.0m / 1000);

            _logdebug.Info("执行定时任务，当前时间：" + DateTime.Now + "；发送的数量" + mylen + "；");

            for (int i = 0; i < len; i++)
            {
                List<MEMBER_EX> arry = wxTmp.OrderBy(a => a.ID).Skip(i * 1000).Take(1000).ToList();

                SendTmpsByMember(arry);
            }
        }

        /// <summary>
        /// 自己封装耗时的方法
        /// </summary>
        /// <returns></returns>
        public static Task SendTmpsByMember(List<MEMBER_EX> his)
        {
            return Task.Run(() =>
            {
                foreach (MEMBER_EX item in his)
                {
                    dt_Dyn_QueryMemberShipBinding_req req = new dt_Dyn_QueryMemberShipBinding_req();
                    req.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
                    req.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
                    req.VGROUP = AppConfig.VGROUP; //销售组织
                    req.DATA_SOURCE = AppConfig.DATA_SOURCE;//数据来源
                    req.MOB_NUMBER = item.MEMBERNO;

                    dt_Dyn_QueryMemberShipBinding_reqITEM[] items = new dt_Dyn_QueryMemberShipBinding_reqITEM[]
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
                    req.BRANDLIST = items;

                    dt_Dyn_QueryMemberShipBinding_res res = WebHmjApiHelp.QueryMemberShipBinding(req);

                    bool isbingd = false;

                    if (res != null && res.ZRETURN == "Y")
                    {
                        foreach (dt_Dyn_QueryMemberShipBinding_resITEM model in res.BRANDLIST)
                        {
                            if (model.IF_BINDING == "0")
                            {
                                isbingd = true;
                            }
                        }
                    }

                    //发送模板
                    if (isbingd)
                    {
                        dt_Dyn_DispMemQuick_req w = new dt_Dyn_DispMemQuick_req();
                        w.DATA_SOURCE = AppConfig.DATA_SOURCE;
                        w.LOYALTY_BRAND = AppConfig.LOYALTY_BRAND;//忠诚度品牌
                        w.SOURCE_SYSTEM = AppConfig.SOURCE_SYSTEM;//来源系统
                        w.VGROUP = AppConfig.VGROUP; //销售组织
                        w.ACCOUNT_ID = item.MEMBERNO;//2002652891
                        dt_Dyn_DispMemQuick_res dt = WebHmjApiHelp.DispMemQuick(w);

                        dt_Dyn_DispMemQuick_resITEM newmeber = new dt_Dyn_DispMemQuick_resITEM();

                        if (dt.I_ZCRMT316 != null || dt.I_ZCRMT316.Length > 0)
                        {
                            newmeber = dt.I_ZCRMT316[0];
                        }

                        var openid = item.OPENID;
                        var tempId = "6D5qBE3AxWyGeiAcMmK_NDMtiCbIDq79Ap98gZ358IQ";
                        var redirect_url = AppConfig.HmjWebApp + "assets/hmjweixin/html/hytq.html";
                        var p1 = "恭喜您成为华美家会员，您有以下权益可领取！";
                        var p2 = Utility.GetMemberLvl(newmeber.ZTIER);
                        var p3 = "升级至等级" + Utility.GetMemberLvl(newmeber.ZTIER);// + "会员";  20180417注释，因为Utility.GetMemberLvl(newmeber.ZTIER)已含会员两字
                        var p4 = DateTime.Now.ToString("yyyy年MM月dd日");
                        var p5 = "转换品牌积分至华美家，即可享受权益。如不转换，可能会影响您的积分正常使用。了解更多会员权益，点击查看>";

                        mymember.SendTmpPublicFunc(true, openid, tempId, redirect_url, p1, p2, p3, p4, p5);

                    }

                }
            });
        }
    }
}