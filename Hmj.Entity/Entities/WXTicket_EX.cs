namespace Hmj.Entity.Entities
{
    public class WXTicket_EX : WXTicket
    {
        /// <summary>
        /// 是否已领取
        /// </summary>
        public bool IsHav { get; set; }

        /// <summary>
        /// 商户名称
        /// </summary>
        public string MERCHANT_NAME { get; set; }
    }
}
