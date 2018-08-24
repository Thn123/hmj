using System;

namespace Hmj.Common.Utils
{
    public  class DateUtils
    {
        public static DateTime ToDateTime(object obj)
        {
            DateTime result;
            if (obj != null && DateTime.TryParse(obj.ToString(), out result))
            {
                return result;
            }
            else
            {
                return DateTime.MinValue;
            }

        }
        public static DateTime? ToDateTimeNullable(object obj)
        {

            DateTime result;
            if (obj != null && DateTime.TryParse(obj.ToString(), out result))
            {
                return result;
            }
            else
            {
                return null;
            }

        }
    }
}
