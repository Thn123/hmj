using System;
using System.Collections.Generic;
using System.Text;

namespace Hmj.Common.Utils
{
    public static class SqlWhereUtils
    {
        public static String WhereInValues(IList<String> ids)
        {
            StringBuilder result = new StringBuilder();
            if (ids != null && ids.Count > 0)
            {
                foreach (String id in ids)
                {
                    result.AppendFormat(",'{0}'", StrToSqlStr(id));
                }
                if (result.Length > 0)
                {
                    result.Remove(0, 1);
                }
            }
            else
            {
                result.AppendFormat("'temp_emptyInData'");
            }
            return result.ToString();
        }

        public static String WhereInValues(IList<int> ids)
        {
            StringBuilder result = new StringBuilder();
            if (ids != null && ids.Count > 0)
            {
                foreach (long id in ids)
                {
                    result.AppendFormat(",{0}", id);
                }
            }
            if (result.Length > 0)
            {
                result.Remove(0, 1);
            }
            else
            {
                result.AppendFormat("'0'");
            }
            return result.ToString();
        }

        public static String WhereLikeRight(String value)
        {
            return value + "%";
        }

        public static String WhereLikeLeft(String value)
        {
            return "%" + value;
        }

        public static String WhereLike(String value)
        {
            return "%" + value + "%";
        }

        public static String StrToSqlStr(String str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            return str.Replace("'", "''");
        }
    }
}
