using System;

namespace Hmj.Common.Utils
{
    public class IdUtils
    {
        public static String NewGuid()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
