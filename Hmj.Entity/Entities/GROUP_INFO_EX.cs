using System.Collections.Generic;

namespace Hmj.Entity.Entities
{
    public class GROUP_INFO_EX : GROUP_INFO
    {
        public List<GROUP_INFO_EX> Children { get; set; }
        //public List<MDSearch> Stores { get; set; }
    }
}
