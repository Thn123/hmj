using System;

namespace Hmj.Entity.SearchEntity
{
    public class BookingSearch
    {
        public DateTime SearchDate { get; set; }
        public string RoomNo { get; set; }
        public string MasseurNO { get; set; }
        public string StoreID { get; set; }
        public int ORG_ID { get; set; }
        public int REGION_ID { get; set; }
        public string CUSTNAME { get; set; }
        public string CUSTMOBLIE { get; set; }
    }
}
