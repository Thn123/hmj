using System;
using System.Runtime.Serialization;

namespace Hmj.WebApp.ViewModels
{
    public class BookIndexModel
    {      
        public int StartHour { get; set; }
        public int EndHour { get; set; }
        public int SubBranchId { get; set; }
        public int ParentSubId { get; set; }
        public int StoreId { get; set; }
    
        /// <summary>
        /// 1 按技师  2多技师
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public int Type { get; set; }
        public bool AdminEdit { get; set; }
        public DateTime CurrentDate { get; set; }

        //public List<DTUBooking> BookingList { get; set; }
    }
    [DataContract]
    public class ColModel
    {        
        [DataMember(Name = "id")]
        public string Id{get;set;}
        [DataMember(Name = "display")]
        public string Dispaly{get;set;}
        [DataMember(Name = "timeline")]
        public bool TimeLine { get; set; }
        [DataMember(Name = "shm")]
        public string StartHM { get; set; }
        [DataMember(Name = "ehm")]
        public string EndHM { get; set; }
    }
}