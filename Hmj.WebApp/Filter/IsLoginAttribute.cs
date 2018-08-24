using System;

namespace Hmj.WebApp
{
    public class IsLoginAttribute : Attribute
    {
        /// <summary>
        /// 默认false
        /// </summary>
        public bool IS_LOGIN { get; set; } = false;

        public IsLoginAttribute(bool Power)
        {
            this.IS_LOGIN = Power;
        }
    }
}