using System;

namespace Hmj.Entity.Entities
{
    public class Graphic_Detail_EX : WXGraphicDetail
    {
        /// <summary>
        /// 图文名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 图文
        /// </summary>
        public string Tuwen { get; set; }

        public int Listcount { get; set; }

        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 带服务器地址的图片链接
        /// </summary>
        public string FullPicUrl { get; set; }
    }
}
