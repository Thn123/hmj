using Hmj.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Hmj.Extension
{
    public static class ResourceExtensions
    {
        public static string GetResourceNameByValue(
          this IList<IResource> rList, string value)
        {
            var res = rList.Where(o => o.Value == value);
            if (res.Count() == 0)
                return null;
            return res.First().Name;
        }
    }
}
