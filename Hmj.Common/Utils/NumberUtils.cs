namespace Hmj.Common.Utils
{
    public class NumberUtils
    {

        public static int ToInt(object obj, int defaultValue)
        {
            int result;
            if (obj != null && int.TryParse(obj.ToString(), out result))
            {
                return result;
            }
            else
            {
                return defaultValue;
            }
        }

        public static int ToInt(object obj)
        {
            return ToInt(obj, 0);
        }

        public static int? ToIntNullable(object obj)
        {
            int result;
            if (obj != null && int.TryParse(obj.ToString(), out result))
            {
                return result;
            }
            return null;
        }

        public static decimal ToDecimal(object obj, decimal defaultValue)
        {
            decimal result;
            if (obj != null && decimal.TryParse(obj.ToString(), out result))
            {
                return result;
            }
            else
            {
                return defaultValue;
            }
        }

        public static decimal ToDecimal(object obj)
        {
            return ToDecimal(obj, 0.0m);
        }

        public static double ToDouble(object obj, double defaultValue)
        {
            double result;
            if (obj != null && double.TryParse(obj.ToString(), out result))
            {
                return result;
            }
            else
            {
                return defaultValue;
            }
        }

        public static double ToDouble(object obj)
        {
            return ToDouble(obj, 0.0);
        }

        public static sbyte ToBoolSbyte(object obj)
        {
            int intValue = ToInt(obj);
            return intValue > 0 ? (sbyte)1 : (sbyte)0;
        }

    }
}
