using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hmj.WebApp.WebService
{
    [Serializable]
    public class AccessResponse
    {
        /// <summary>
        /// //接受成功与否标志（Y/N）
        /// </summary>
        public string Wv_Return { get; set; }

        public string Message { get; set; }

        public string Member_Group { get; set; }

    }
}