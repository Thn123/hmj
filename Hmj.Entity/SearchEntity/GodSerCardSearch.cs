using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPACRM.Entity.SearchEntity
{
   public class GodSerCardSearch
    {
       //产品/服务/卡 ID
       public int ID { get; set; }
       //名称
       public string NAME { get; set; }
       //价格
       public string PRICE { get; set; }
       //来源TABLE
       public string TABLE { get; set; }
    }
}
