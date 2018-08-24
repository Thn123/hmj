namespace Hmj.ExtendAPI.WeiXin.Models
{

    public interface IMessageBase
    {
        string ToUserName { get; set; }
        string FromUserName { get; set; }
        long CreateTime { get; set; }
    }

}
