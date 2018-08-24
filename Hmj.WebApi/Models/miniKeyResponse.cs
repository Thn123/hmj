using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hmj.WebApi.Models
{
    public class miniKeyResponse
    {
        public string openid { get; set; }
        public string session_key { get; set; }
        public string unionid { get; set; }

    }
}