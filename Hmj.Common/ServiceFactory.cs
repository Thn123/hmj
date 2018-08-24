using StructureMap;
using System;

namespace Hmj.Common
{
    public class ServiceFactory
    {
        /// <summary>
        /// 获取对象的实例，StructureMap中没有注册过的类型同样可以可以使用ObjectFactory创建
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns></returns>
        public static T GetInstance<T>()
        {
            return ObjectFactory.GetInstance<T>();
        }

        /// <summary>
        /// 根据类型获取类型的示例，StructureMap中没有注册过的类型同样可以可以使用ObjectFactory创建
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="pluginType">需要实例化的对象类型</param>
        /// <returns></returns>
        public static T GetInstance<T>(Type pluginType)
        {
            return (T)ObjectFactory.GetInstance(pluginType);
        }
    }
}
