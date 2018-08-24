using Hmj.DTO;
using Hmj.Entity.PageSearch;
using System.Collections.Generic;
using Hmj.Entity.Entities;
using Hmj.Entity;

namespace Hmj.Interface
{
    public interface IHmjMemberService
    {

        /// <summary>
        /// 根据id得到会员详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MemberResDTO GetMemberModel(string id);

        /// <summary>
        /// 得到集合
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<MemberResDTO> GetMemberList(MemberSearch search);

        /// <summary>
        /// 得到会员详情
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        HmjMemberDetail GetMemberDetailByOpenId(string openid);

        /// <summary>
        /// 华美家登录
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        string Binding(BindReqDTO bindmodel);

        /// <summary>
        /// 是否绑定
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        bool ChckBind(string openid);

        /// <summary>
        /// 得到会员的详细信息
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="Is_Update">是否执行更新操作 0：不执行 1：执行</param>
        /// <returns></returns>
        MemberDetailResDTO GetMemberDetail(string openid, string Is_Update);
        int SendTmp(BCJ_TMP_DETAIL request, string v, bool isHmj = true);

        /// <summary>
        /// 更新头像
        /// </summary>
        /// <param name="valtr"></param>
        /// <param name="fans_id"></param>
        /// <returns></returns>
        string UpdateImageUrl(string valtr, string openid);
        string RegisterMember(MemberRegisterReqDTO request, ref string msg);

        /// <summary>
        /// 修改会员基本信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        string UpdateMember(MemberUpdateReqDTO request);
        string GetMemberMobileByOpenId(string oPENOD);

        /// <summary>
        /// 会员品牌积分查询接口
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        QueryMemberShipBindingResDTO QueryMemberShipBinding(string openid, string mobile = "");

        /// <summary>
        /// 会员品牌积分转换绑定接口
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        BindingRelShipResDTO BindingRelShip(string mobile, string codes, string POIT,ref string  REG_DATE);

        /// <summary>
        /// 完善信息送积分
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        bool SendPoint(MemberExtendDTO request);
        string GetMemberMobileByBP(string pARTNER);
        void SendTmpPublicFunc(bool isreal_time, params string[] param);
        void ChageSatus(string mobile);

        /// <summary>
        /// 查询模板发送记录
        /// </summary>
        /// <param name="IS_SEND"></param>
        /// <returns></returns>
        List<WX_TMP_HIS> GetWxTmpHisByIsSend(int IS_SEND);

        /// <summary>
        /// 修改模板消息发送记录
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="IS_SEND"></param>
        /// <returns></returns>
        int UpdateWxTmpHisIsSendByID(int ID, int IS_SEND, string result);

        long insertWXCouponGiveInfo(WXCouponGiveInfo WXCouponGiveInfo);

        WXCouponGiveInfo GetWXCouponGiveInfoByOpenid(string Openid);
        
        List<WXCouponNoInfo> QueryWXCouponNoInfo();

        int UpdateWXCouponNoInfoIsImport(long id);

        WXCouponGiveInfo CanGetCoupon(string OpenId, string cardId);
        int UpdateWXCouponGiveInfoIsHX(string CouponNo);

        CardApiTicket GetModelCardApi();
        long AddCardApi(CardApiTicket model);

        /// <summary>
        /// 查询用户某卡券获取资格
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        List<WXCouponGiveInfo> GetWXCouponGiveInfo(string openid, string cardid);

    }
}
