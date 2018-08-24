using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hmj.Entity.Entities
{
    public class WXCustScanRecordEx: WXCustScanRecord
    {
        public string vgroup { get; set; }
        public string source { get; set; }
    }
}
