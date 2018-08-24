using System;

namespace Hmj.ExtendAPI.WeiXin.Models
{
    [Serializable]
    [System.Xml.Serialization.XmlRoot("xml")]
    public class RequestEventMessage : MessageBase
    {
        public string Event { get; set; }
    }
}
