using Hmj.Entity.Jsons;
using System.Collections.Generic;

namespace Hmj.WebApp.Helper
{
    public class DataSourceHelper
    {
        public static List<CodeValue> GetHours()
        {
            var list = new List<CodeValue>();
            for (int i = 0; i <= 23; i++)
            {
                list.Add(new CodeValue { Code = i.ToString(), Value = i.ToString() });
            }
            return list;
        }
    }
}