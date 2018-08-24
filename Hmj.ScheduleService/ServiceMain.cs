using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using Hmj.ScheduleService.Code;
using Newtonsoft.Json;

namespace WindowsService
{
    public partial class ServiceMain : ServiceBase
    {
        /// <summary>
        /// 主线程
        /// </summary>
        private Thread _mainThread;
        /// <summary>
        /// 监听是否跑过
        /// </summary>
        private bool _dayIsRun;

        /// <summary>
        /// 开始抓取时间 小时数值
        /// </summary>
        private string _StartHour;
        public ServiceMain()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            _mainThread = new Thread(new ThreadStart(QueryData))
            {
                IsBackground = true
            };
            _mainThread.Start();

            _dayIsRun = false;
            _StartHour = ConfigurationSettings.AppSettings["StartHour"];

        }
        private void QueryData()
        {
            while (true)
            {
                try
                {

                    DateTime now = DateTime.Now;
                    DateTime day = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + _StartHour);

                    if (_dayIsRun && now>=day.AddMinutes(3))
                    {
                        _dayIsRun = false;
                    }
                    if (now >= day && day.AddMinutes(3) >= now && !_dayIsRun)
                    {

                        LogService.Info("时间已到，开启匹配商品信息");
                        //获取数据源
                        List<ProductInfo> data = ConvertHelper.ConvertToModel<ProductInfo>(SqlDbHelper.ExecuteDataTable("SELECT ID,SKU FROM dbo.PROD_INFO ORDER BY ID ASC"));

                        int num = data.Count % 50 == 0 ? data.Count / 50 : data.Count / 50 + 1;
                        for (int i = 1; i <= num; i++)
                        {
                            string codes = string.Join(",", data.Select(c => c.sku).Skip((i - 1) * 50).Take(50).ToArray());

                            string str = CenshERPApiClient.Create().GetProductDetail(codes);

                            LogService.Info("批次 "+i.ToString()+"："+codes);

                            var jobj = JObject.Parse(str);

                            LogService.Info("返回结果：" + str);
                            if (jobj["MESSAGE"]["MESSAGE_CODE"].ToString() == "1")
                            {
                                //成功信息
                                var jar = (JArray)jobj["RESULT"]["PRODUCT_DETAILS"];

                                StringBuilder sb = new StringBuilder();
                                for (int j = 0; j < jar.Count; j++)
                                {
                                    string temp = jar[j]["CODE"].ToString();
                                    ProductInfo product = data.Where(c => c.sku == temp).FirstOrDefault();
                                    if (product != null)
                                    {
                                        sb.AppendFormat(@"UPDATE dbo.PROD_INFO SET [NAME]='{0}',
                                                                            SALE_PRICE={1},
                                                                        [MARKET_PRICE]={2},
                                                                        [SALE_STATUS]={3},
                                                                        [COMMISSION_RATE]={4},
                                                                        [ATTR_DATA]={5} WHERE ID={6};",
                                                                            jar[j]["NAME"].ToString(),
                                                                            jar[j]["SALE_PRICE"].ToString(),
                                                                            jar[j]["MARKET_PRICE"].ToString(),
                                                                            jar[j]["ON_SALE"].ToString() == "T" ? 1 : 0,
                                                                            jar[j]["COMMISSION_RATE"].ToString(),
                                                                            jar[j].ToString(),
                                                                           product.id);

                                    }
                                }
                                if (!string.IsNullOrEmpty(sb.ToString()))
                                {
                                    SqlDbHelper.ExecuteNonQuery(sb.ToString());

                                    LogService.Info("更新成功 "+i.ToString()+"：" +sb.ToString());
                                }
                                sb.Clear();

                            }

                        }


                        //分批查询修改接口
                        _dayIsRun = true;
                    }


                    //else if (day.Hour != _StartHour)
                    //{
                    //    _dayIsRun = false;
                    //}

                    //if (_dayIsRun)
                    //{
                    //    Thread.Sleep(new TimeSpan(24, 0, 0));
                    //}
                }
                catch (Exception ex)
                {
                    LogService.Error("服务异常：" + ex.Message + "\n位置：" + ex.StackTrace);
                    Thread.Sleep(1000 * 5);//五秒执行一次
                }

            }
        }

        protected override void OnStart(string[] args)
        {
            Init();//初始化 及触发 线程读取配置
            Helper.WriteLog("服务启动...");

        }

        protected override void OnStop()
        {
            this.Dispose();

        }
    }
}
