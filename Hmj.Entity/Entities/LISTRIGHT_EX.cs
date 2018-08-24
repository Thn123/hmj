using Hmj.Entity.ORMapping;
using System;
using System.Collections.Generic;

namespace Hmj.Entity.Entities
{
    public class LISTRIGHT_EX
    {
        private int _RIGHT_ID;
        /// <summary>
        /// 
        ///  int(10)
        /// </summary>
        [Identity, PrimaryKey(1)]
        public int RIGHT_ID { get; set; }
        /// <summary>
        /// 
        ///  varchar(50)
        /// </summary>

        public string RIGHT_NAME { get; set; }
        /// <summary>
        /// 
        ///  varchar(50)
        /// </summary>

        public string RIGHT_DSC { get; set; }
        /// <summary>
        /// 
        ///  char(1)
        /// </summary>

        public bool IS_RIGHT { get; set; }
        /// <summary>
        /// 
        ///  char(1)
        /// </summary>

        public bool IS_MENU { get; set; }
        /// <summary>
        /// 
        ///  varchar(20)
        /// </summary>
        [Nullable]
        public string MENU_CODE { get; set; }
        /// <summary>
        /// 
        ///  varchar(80)
        /// </summary>
        [Nullable]
        public string URL_LINK_TO { get; set; }
        /// <summary>
        /// 
        ///  varchar(20)
        /// </summary>
        [Nullable]
        public string TARGET { get; set; }
        /// <summary>
        /// 
        ///  int(10)
        /// </summary>
        [Nullable]
        public int? PARENT_ID { get; set; }

        public List<SYS_RIGHT> LISTRIGHT { get; set; }
    }
}
