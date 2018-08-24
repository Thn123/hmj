using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hmj.Entity.apiEntity
{
    public class GeocoderResponse
    {
        public int status { get; set; }//返回结果状态值， 成功返回0
        public GeoModel result { get; set; }
    }

    public class GeoModel
    {
        public GeoLocation location { get; set; }//经纬度坐标
        public int precise { get; set; }//位置的附加信息，是否精确查找。1为精确查找，即准确打点；0为不精确，即模糊打点
        public int confidence { get; set; }//可信度，描述打点准确度
        public string level { get; set; }//地址类型
    }

    public class GeoLocation
    {
        public decimal lng { get; set; }//经度值
        public decimal lat { get; set; }//纬度值
    }
}
