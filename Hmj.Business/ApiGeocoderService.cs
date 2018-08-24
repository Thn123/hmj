using Hmj.Common.Utils;
using Hmj.Entity.apiEntity;
using Hmj.ExtendAPI.Geocoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hmj.Business
{
    public class ApiGeocoderService
    {
        /// <summary>
        /// 根据地址获取经纬度
        /// </summary>
        /// <param name="Addr"></param>
        /// <returns></returns>
        public GeocoderResponse GetGeocoder(string Addr)
        {
            string result = GeocoderClient.Instance.GetGeocoder(Addr);
            var response = JsonHelper.DeserializeObject<GeocoderResponse>(result);
            return response;
        }
    }
}
