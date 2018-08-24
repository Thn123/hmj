using Hmj.DataAccess.Repository;
using Hmj.Entity;
using System.Collections.Generic;

namespace Hmj.Business.WXService
{
    public class WXDictCityService
    {
        private WXDictCityRepository _WXDictCityRepository;
        public WXDictCityRepository WXDictCityRepository
        {
            get
            {
                if (_WXDictCityRepository == null)
                {
                    _WXDictCityRepository = new WXDictCityRepository();
                }
                return _WXDictCityRepository;
            }
        }

        /// <summary>
        /// 查询所有省
        /// </summary>
        /// <returns></returns>
        public List<WXDictProvince> Query()
        {
            return WXDictCityRepository.Query();
        }
    }
}
