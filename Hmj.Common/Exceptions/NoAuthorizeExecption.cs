using System;

namespace Hmj.Common.Exceptions
{
    /// <summary>
    /// 没有登录的异常
    /// </summary>
    [Serializable]
    public class NoAuthorizeExecption : Exception
    {
        public NoAuthorizeExecption()
            : base("你没有登录，该页面必须登录后访问！")
        {

        }
    }
}
