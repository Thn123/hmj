using System;

namespace Hmj.Entity.Entities
{
    public class BaseDeptInfo : ICloneable
    {
        public int ID { get; set; }

        public string DeptCode { get; set; }

        public string DeptName { get; set; }

        public string MagentoGroupID { get; set; }

        public int Type { get; set; }

        public int Order { get; set; }

        public int WXGroupID { get; set; }

        public int ParentID { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    public class DeptInfo : BaseDeptInfo
    {

        public int GroupID { get; set; }

        public int StoreType { get; set; }

        public string Address { get; set; }

        public string Telephone { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public string BelongsAreaNo { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string Brand { get; set; }

        public bool IsPickUp { get; set; }
    }
}
