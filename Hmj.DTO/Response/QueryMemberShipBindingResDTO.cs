using System.Collections.Generic;

namespace Hmj.DTO
{
    public class QueryMemberShipBindingResDTO
    {
        public int IS_BRADN { get; set; }
        public decimal POINT_ALL { get; set; }

        public string LVL_CODE { get; set; }

        public string LVL_NAME { get; set; }

        public List<QueryMemberBindDetailResDTO> BRAND_LIST { get; set; }
    }

}
