using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hmj.WebApi.Models
{
    public class AdministrativeDivisionResponse
    {
        [JsonProperty("region_id", Order = 0)]
        public int region_id { get; set; }
        [JsonProperty("parent_id", Order = 0)]
        public int parent_id { get; set; }
        [JsonProperty("region_name", Order = 0)]
        public string region_name { get; set; }
        [JsonProperty("region_type", Order = 0)]
        public int region_type { get; set; }
        [JsonProperty("agency_id", Order = 0)]
        public int agency_id { get; set; }
        [JsonProperty("region_sn", Order = 0)]
        public int region_sn { get; set; }
        [JsonProperty("buildin", Order = 0)]
        public int buildin { get; set; }
        [JsonProperty("lastchanged", Order = 0)]
        public DateTime lastchanged { get; set; }
        [JsonProperty("other_name", Order = 0)]
        public string other_name { get; set; }

    }

    public class AdministrativeDivision<T> : AdministrativeDivisionResponse
    {

        [JsonProperty("data", Order = 2)]
        public T Data
        {
            get;
            set;
        }

    }
}