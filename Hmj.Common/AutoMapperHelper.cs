using AutoMapper;
using System.Collections;
using System.Collections.Generic;

namespace Hmj.Common
{
    /// <summary>
    /// dto
    /// </summary>
    public static class AutoMapperHelper
    {
        /// <summary>
        ///  类型映射
        /// </summary>
        public static T MapTo<T>(this object obj)
        {
            var configuration = new MapperConfiguration(cfg => cfg.CreateMap(obj.GetType(), typeof(T)));
            T model = configuration.CreateMapper().Map<T>(obj);
            return model;

            //if (obj == null) return default(T);
            //Mapper.CreateMap(obj.GetType(), typeof(T));
            //return Mapper.Map<T>(obj);
        }

        /// <summary>
        /// 集合列表类型映射
        /// </summary>
        public static List<T> MapToList<T>(this IEnumerable source)
        {
            List<T> list = new List<T>();

            foreach (var first in source)
            {
                var configuration = new MapperConfiguration(cfg => cfg.CreateMap(first.GetType(), typeof(T)));
                T model = configuration.CreateMapper().Map<T>(first);
                list.Add(model);
            }

            return list;
        }

        /// <summary>
        /// 集合列表类型映射
        /// </summary>
        public static List<T> MapToList<T, V>(this IEnumerable<T> source)
        {
            var configuration = new MapperConfiguration(cfg => cfg.CreateMap<T, V>());
            List<T> model = configuration.CreateMapper().Map<List<T>>(source);
            return model;
        }
    }
}
