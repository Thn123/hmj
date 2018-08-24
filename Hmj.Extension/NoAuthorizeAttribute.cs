using System;

namespace Hmj.Extension
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class NoAuthorizeAttribute : Attribute
    {
    }
}