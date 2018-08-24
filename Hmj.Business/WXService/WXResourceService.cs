using Hmj.DataAccess.Repository;
using Hmj.Entity;
using System;
using System.Collections.Generic;

namespace Hmj.Business.WXService
{
    public class WXResourceService
    {
        private WXResourcereRepository _wrr;
        public WXResourceService()
        {
            _wrr = new WXResourcereRepository();
        }

        /// <summary>
        /// 查询所有在微信端显示的门店
        /// </summary>
        /// <param name="orgID">商户ID</param>
        /// <returns>服务大类IList</returns>
        public List<ORG_STORE> GetStores(int orgID)
        {
            return _wrr.GetStores(orgID);
        }

        /// <summary>
        /// 查询所有服务大类
        /// </summary>
        /// <param name="orgID">商户ID</param>
        /// <returns>服务大类IList</returns>
        public List<PROD_CATEGORY> GetServices(int orgID)
        {
            return _wrr.GetServices(orgID, Guid.Empty, 2);
        }

        /// <summary>
        /// 查询空闲技师列表，考虑预约冲突和排班的因素
        /// </summary>
        /// <param name="storId">门店ID</param>
        /// <param name="beginTime">预约开始时间</param>
        /// <param name="endTime">预约结束时间</param>
        /// <param name="gender">是否指定技师男女，可为空</param>
        /// <returns>员工IList</returns>
        public List<ORG_EMPLOYEE> GetFreeEmployees(int storId, DateTime beginTime, DateTime endTime, bool? gender)
        {
            return _wrr.GetFreeEmployees(storId, beginTime, endTime, gender);
        }
    }
}
