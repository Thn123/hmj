using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Hmj.ScheduleService.Code
{
    public class ConvertHelper
    {
        #region DataTable 转换 List
        public static List<T> ConvertToModel<T>(DataTable dt) where T : new()
        {

            List<T> ts = new List<T>();// 定义集合
            Type type = typeof(T); // 获得此模型的类型
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                PropertyInfo[] propertys = t.GetType().GetProperties();// 获得此模型的公共属性
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;
                    if (dt.Columns.Contains(tempName))
                    {
                        if (!pi.CanWrite) continue;
                        object value = dr[tempName];
                        if (value != DBNull.Value)
                            pi.SetValue(t, value, null);
                    }
                }
                ts.Add(t);
            }
            return ts;
        }

        public static object To<T>(object value)
        {
            Type type = typeof(T);
            Type sType = typeof(string);
            if (type.FullName == sType.FullName)
                return value;
            Type t = type.Assembly.GetType(type.FullName);
            object data = default(T);
            try
            {
                data = t.GetMethod("Parse", new[] { sType }).Invoke(null, new object[] { value });
            }
            catch (Exception)
            {
            }
            return data;
        }
        #endregion
    }
}
