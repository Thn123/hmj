namespace Hmj.DTO
{
    public class JsonDTO<T>
    {
        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public T Data { get; set; }
    }
}
