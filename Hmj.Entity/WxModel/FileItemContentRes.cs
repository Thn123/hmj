using Newtonsoft.Json;
using System.Collections.Generic;

namespace Hmj.Entity.Entities
{
    public class FileItemContentRes
    {
        /// <summary>
        ///  图文列表
        /// </summary>
        [JsonProperty("news_item")]
        public List<FileItemNewsRes> News_Item { get; set; }
    }
}
