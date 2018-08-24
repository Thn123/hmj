using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hmj.ExtendAPI.Geocoder
{
    public class GeocoderClient
    {

        private static readonly object obj = new object();
        private static GeocoderClient _instance = null;
        public static GeocoderClient Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new GeocoderClient();
                        }
                    }
                }
                return _instance;
            }
        }
        public string GetGeocoder(string Addr)
        {
            string result = "";
            try
            {
                string URL = "http://api.map.baidu.com/geocoder/v2/";
                string ak = "o1DpcVZBKxedvEVwDxLWEaQTY4jnfDax";
                string sk = "8wyI2aU73YxI3CglYFE2kqQcZdY9vOyB";
                string output = "json";
                string address = Addr;
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("address", address);
                dict.Add("output", output);
                dict.Add("ak", ak);
                string sn = AKSNCaculater.CaculateAKSN(ak, sk, "/geocoder/v2/", dict);
                string url = URL + "?address=" + address + "&output=" + output + "&ak=" + ak + "&sn=" + sn;
               
                result = InvokeHttpContext.DoJsonRequest(url, null);
                //{"status":0,"result":{"location":{"lng":121.48789948569473,"lat":31.24916171001514},"precise":0,"confidence":12,"level":"城市"}}
            }
            catch (Exception ex)
            {
            }
            return result;
        }
    }
}
