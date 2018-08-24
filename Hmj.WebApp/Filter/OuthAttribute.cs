using System;

namespace Hmj.WebApp
{
    public class OuthAttribute : Attribute
    {
        /// <summary>
        /// 默认false
        /// </summary>
        public bool IS_OUTH { get; set; } = false;

        public OuthAttribute(bool Power)
        {
            this.IS_OUTH = Power;
        }
    }
}