using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Entity.PageSearch;
using Hmj.Entity.SearchEntity;
using Hmj.Entity.WxModel;
using Hmj.WebService;

namespace Hmj.Interface
{
    public interface ICustMemberService
    {
        /// <summary>
        /// 绑定会员
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        string BindMember(string mobile, string openid, string Nickname);

        /// <summary>
        /// 判断时候绑定会员
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        bool ChckBind(string openid);

        /// <summary>
        /// 注册会员
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="openid"></param>
        /// <param name="nameo"></param>
        /// <param name="namek"></param>
        /// <returns></returns>
        string RegisterMember(string mobile, string openid, string nameo,
            string namek, string brithday, string nickname, string gender);
        string GetOldMember(string openid);
        ZCRMT302_Dyn GetMemberModelByBp(string bp);

        /// <summary>
        /// 更新会员信息并获取会员详情
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        MemberInfo GetLoadMember(string openid);
        void InserMeber(CUST_MEMBER model);
        PagedList<SURREY_GROUP_EX> QueryGetGroups(GroupSearch search, PageView view);
        void UpdateFans(int fansid);
        void ChageSatus(string aCCOUNT_ID);
        string SendPiod(string mobile,string point);
        string ReduceOrAddPiod(string mobile,string point,string type);

        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <returns></returns>
        MemberInfo GetMemberInfo(string openid, string isnew = "1");

        /// <summary>
        /// 修改会员主数据信息，
        /// </summary>
        /// <param name="mobile">新手机号</param>
        /// <param name="oldmobile">旧的手机号</param>
        /// <param name="pwd">新密码</param>
        /// <param name="oldpwd">旧密码</param>
        /// <returns></returns>
        int UpdateMobileOrPwd(string mobile, string oldmobile,
            string pwd, string oldpwd, string OpendID, ref string msg);

        /// <summary>
        /// 修改会员常规信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="gender"></param>
        /// <param name="address"></param>
        /// <param name="opendID"></param>
        /// <returns></returns>
        int UpdateMember(string name, string NAME_FIRST, string gender, string address, string opendID);

        /// <summary>
        /// 获取历史记录
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        PointHistory GetPointHistory(string openid);

        /// <summary>
        /// 获取会员的优惠券信息
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        MEMBER_COUPON GetCoupon(string openid);
        bool DeleteMember(string fROMUSERNAME);

        /// <summary>
        /// 根据openid获取会员普通信息
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        CUST_MEMBER GetMemberByOpenId(string openid);
    }
}
