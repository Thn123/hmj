using System;

namespace VIPSystemV2.Entity
{
    public enum UseState : sbyte
    {
        Disable = 0,
        Enable = 1
    }
    [Flags]
    public enum GoodsTag : int
    {
        HOT = 1,
        DISCOUNT = 2,
        NEW = 4,
    }
    /// <summary>
    /// 商品渠道
    /// </summary>
    [Flags]
    public enum GoodsChannel : int
    {
        /// <summary>
        /// VIP网站
        /// </summary>
        VIPSITE = 1,
        /// <summary>
        /// G+App
        /// </summary>
        GPLUSAPP = 2,
        /// <summary>
        /// 彩虹网
        /// </summary>
        RAINBOW = 4,
    }


    [Flags]
    public enum VipLevel : int
    {
        VIP1 = 1,
        VIP2 = 2,
        VIP3 = 4,
        VIP4 = 8,
        VIP5 = 16,
        VIP6 = 32,
        VIP7 = 64,
        VIP8 = 128,
        VIP9 = 256,
    }

    public enum ApproveStatus : sbyte
    {
        //0 初始化  1 审批通过  2 审批不通过
        /// <summary>
        /// 初始化
        /// </summary>
        Init = 0,

        /// <summary>
        /// 审批通过
        /// </summary>
        Approve = 1,

        /// <summary>
        /// 审批不通过
        /// </summary>
        Disapprove = 2

    }


    public enum ResponseCode : int
    {
        Continue = 100,// 初始的请求已经接受，客户应当继续发送请求的其余部分。（HTTP 1.1新）
        Switching_Protocols = 101, //服务器将遵从客户的请求转换到另外一种协议（HTTP 1.1新）
        OK = 200, //一切正常，对GET和POST请求的应答文档跟在后面。
        Created = 201, // 服务器已经创建了文档，Location头给出了它的URL。
        Accepted = 202, //已经接受请求，但处理尚未完成。
        Moved_Permanently = 301, //客户请求的文档在其他地方，新的URL在Location头中给出，浏览器应该自动地访问新的URL。
        Bad_Request = 400, //Bad Request 请求出现语法错误。
        Unauthorized = 401, //客户试图未经授权访问受密码保护的页面。应答中会包含一个WWW-Authenticate头，浏览器据此显示用户名字/密码对话框，然后在填写合适的Authorization头后再次发出请求。
        Forbidden = 403, // 资源不可用。服务器理解客户的请求，但拒绝处理它。通常由于服务器上文件或目录的权限设置导致。
        Not_Found = 404, //无法找到指定位置的资源。这也是一个常用的应答。
        Request_Timeout = 408, //在服务器许可的等待时间内，客户一直没有发出任何请求。客户可以在以后重复同一请求。（HTTP 1.1新）
        Internal_Server_Error = 500, //Internal Server Error 服务器遇到了意料不到的情况，不能完成客户的请求。
        Not_Implemented = 501, //服务器不支持实现请求所需要的功能。例如，客户发出了一个服务器不支持的PUT请求。
        Bad_Gateway = 502, //服务器作为网关或者代理时，为了完成请求访问下一个服务器，但该服务器返回了非法的应答。
        Service_Unavailable = 503 //服务器由于维护或者负载过重未能应答。例如，Servlet可能在数据库连接池已满的情况下返回503。服务器返回503时可以提供一个Retry-After头。  
    }

    /// <summary>
    /// SYSTEM_LOG表code枚举值
    /// </summary>
    public enum SystemCode
    {
        /// <summary>
        /// 创建会员
        /// </summary>
        CreateMemberShip,
        /// <summary>
        /// 创建潜客
        /// </summary>
        CreateLead,
        /// <summary>
        /// 会员详情查询
        /// </summary>
        DispMember,
        /// <summary>
        /// 会员快速查询
        /// </summary>
        DispMemQuick,
        /// <summary>
        /// 会员实时修改
        /// </summary>
        UpdateMemberShip,
        /// <summary>
        /// 会员积分明细查询
        /// </summary>
        GetPointDetail,
        /// <summary>
        /// 会员绑定查询品牌会员接口
        /// </summary>
        QueryMemberShipBinding,
        /// <summary>
        /// 绑定关系同步
        /// </summary>
        DynMemberBunding,
        /// <summary>
        /// 积分加减接口
        /// </summary>
        ActCreateTel,
        /// <summary>
        /// 用户关注公众号状态信息传送
        /// </summary>
        WechatStateTran,
        /// <summary>
        /// 会员状态调整
        /// </summary>
        ChangeMemberStatus,
        /// <summary>
        /// 短信发送
        /// </summary>
        SMSInsert
    }
}
