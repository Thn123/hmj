using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hmj.WebApi.Models
{
    public class AccessResponse
    {
        [JsonProperty("status", Order = 0)]
        public int Status
        {
            get;
            set;
        }

        [JsonProperty("message", Order = 1)]
        public string Message
        {
            get;
            set;
        }

        [JsonIgnore]
        public virtual string ContentType
        {
            get
            {
                return "application/json; charset=utf-8";
            }
        }
    }

    public class AccessResponse<T> : AccessResponse  
    {

        [JsonProperty("data", Order = 2)]
        public T Data
        {
            get;
            set;
        }

    }
    public class XmlResponse<T> : AccessResponse
    {
        [JsonIgnore]
        public override string ContentType
        {
            get
            {
                return "application/xml; charset=utf-8";
            }
        }
    }
}