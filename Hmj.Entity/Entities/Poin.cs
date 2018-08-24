using System;

namespace Hmj.Entity.Entities
{
    public class Poin
    {
        /// <param name="lat">纬度 X</param>    
        /// <param name="lng">经度 Y</param>    
        public Poin(double lat, double lng)
        {
            this.lat = lat;
            this.lng = lng;
        }

        //  纬度 X    
        private double lat;

        // 经度 Y    
        private double lng;

        /// <summary>    
        /// 代表纬度 X轴    
        /// </summary>    
        public double Lat { set; get; }

        /// <summary>    
        /// 代表经度 Y轴    
        /// </summary>    
        public double Lng { get; set; }

        public double RadLat { get { return lat * Math.PI / 180; } }

        public double RadLng { get { return lng * Math.PI / 180; } }
    }
}
