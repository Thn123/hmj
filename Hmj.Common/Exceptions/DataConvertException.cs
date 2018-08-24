using System;

namespace Hmj.Common.Exceptions
{
    [Serializable]
    public class DataConvertException : Exception
    {
        public DataConvertException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoPermissionExecption"/> class.
        /// </summary>
        /// <param name="message">异常消息.</param>
        public DataConvertException(string message)
            : base(message)
        { }
        /// <summary>
        /// Initializes a new instance of the <see cref="NoPermissionExecption"/> class.
        /// </summary>
        /// <param name="message">异常消息.</param>
        /// <param name="inner">内部异常.</param>
        public DataConvertException(string message, Exception inner)
            : base(message, inner)
        { }
    }
}
