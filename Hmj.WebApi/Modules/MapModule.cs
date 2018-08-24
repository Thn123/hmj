using Hmj.Common;
using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Interface;
using Hmj.WebApi.Models;
using log4net;
using StructureMap.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hmj.WebApi.Modules
{
    public class MapModule:BaseModule
    {
        private static ILog logger = LogManager.GetLogger("logwarn");
        [SetterProperty]
        public IHmjCommonService _hmjCommonService { get; set; }
        public MapModule() : base("/Map")
        {
            Post["/GetLocalStore"] = GetLocalStore;
        }
        /// <summary>
        /// 获得经纬度
        /// </summary>
        /// <returns></returns>
        public dynamic GetLocalStore(dynamic arg)
        {
            MapRequest request = base.BindObject<MapRequest>();
            string lo = request.LO;
            string ln = request.LN; 
            string code = request.CODE; 

            List<BCJ_CITY> citys = new List<BCJ_CITY>();


            //如果城市编码是空表示顾客自己定位刚进入页面需要加载城市等信息
            if (string.IsNullOrEmpty(code))
            {
                TenAPI model = Utility.GetLocal(lo, ln);
                string citymodel = model.result.formatted_address;

                logger.Info(citymodel + "====" + lo + "===" + ln);

                citys.AddRange(_hmjCommonService.GetCity());

                BCJ_CITY one = citys.Where(a => citymodel.Contains(a.CITY_NAME)).ToList().FirstOrDefault();

                code = one == null ? "110100" : one.CITY_CODE;

            }

            //加载门店列表
            List<BCJ_STORES_EX> stores = _hmjCommonService.GetStoresByCityCode(code);


            //如果不是定位则计算距离城市中心点的位置
            foreach (BCJ_STORES_EX storeM in stores)
            {
                Poin beigin = new Poin(double.Parse(ln), double.Parse(lo));
                Poin end = new Poin(double.Parse(storeM.LONGITUDE), double.Parse(storeM.LATITUDE));
                storeM.Distance = Utility.GetDistance(beigin, end).ToString();
                storeM.NAME = storeM.NAME.Trim();
            }
            return ResponseJson(false, "添加失败", new
            {
                CIRY_CODE = code,
                CIRYS = citys,
                STORES = stores.OrderBy(a => a.Distance)
            });
        }

    }
}