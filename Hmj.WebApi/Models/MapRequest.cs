using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hmj.WebApi.Models
{
    public class MapRequest
    {
        [JsonProperty("LO")]
        public string LO
        {
            get;
            set;
        }

        [JsonProperty("LN")]
        public string LN
        {
            get;
            set;
        }

        [JsonProperty("CODE")]
        public string CODE
        {
            get;
            set;
        }
    }
}