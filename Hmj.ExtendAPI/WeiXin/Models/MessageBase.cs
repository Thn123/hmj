namespace Hmj.ExtendAPI.WeiXin.Models
{
    public class MessageBase : IMessageBase
    {
        public string ToUserName { get; set; }
        public string FromUserName { get; set; }
        public long CreateTime { get; set; }
        //public DateTime CreateTime
        //{
        //    get
        //    {
        //        return DateTimeHelper.GetDateTimeFromFromUnixTime(this.CreateTimeStamp);
        //    }
        //}

        public override string ToString()
        {
            //TODO:直接输出XML


            return base.ToString();
        }
    }
}
