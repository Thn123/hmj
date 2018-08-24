using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Entity.PageSearch;
using System.Collections.Generic;

namespace Hmj.Interface
{
    public interface IMySmallShopService
    {
        /// <summary>
        /// 根据商户ID获取门店列表
        /// </summary>
        /// <param name="Mid"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        PagedList<WD_STORE> GetStoreByMid(int Mid, PageView view);

        ///// <summary>
        ///// 获取价目表
        ///// </summary>
        ///// <param name="Mid"></param>
        ///// <param name="view"></param>
        ///// <returns></returns>
        //PagedList<Pro> GetPriceListByMid(int Mid, PageView view);

        ///// <summary>
        ///// 获取价目表
        ///// </summary>
        ///// <param name="Mid"></param>
        ///// <returns></returns>
        //List<Pro> GetPriceListByMid(int Mid);
        List<WXCUST_MSG_HIS> GetWXCUST_MSG_HIS(string strwhere);
        /// <summary>
        /// 获取价目表明细
        /// </summary>
        /// <param name="Mid"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        PagedList<ProDetail> GetPriceDetailListByPid(int Mid, PageView view);

        /// <summary>
        /// 获取价目表明细
        /// </summary>
        /// <param name="Mid"></param>
        /// <returns></returns>
        List<ProDetail> GetPriceDetailListByPid(int Mid);

        /// <summary>
        /// 获取门店列表
        /// </summary>
        /// <param name="Mid"></param>
        /// <returns></returns>
        List<WD_STORE> GetStoreByMid(int Mid);

        /// <summary>
        /// 获取门店列表，是否适用优惠券
        /// </summary>
        /// <param name="Mid">商户id</param>
        /// <param name="Tid">优惠券id</param>
        /// <returns></returns>
        List<WD_STORE_EX> GetStoreByMid(int Mid, int Tid);

        /// <summary>
        ///  根据商户ID获取预约列表
        /// </summary>
        /// <param name="Mid"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        PagedList<WD_BOOKING> GetBookingListByMid(int Mid, PageView view);

        /// <summary>
        /// 保存门店
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        int SaveStore(WD_STORE s);

        /// <summary>
        /// 保存价目表
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        //int SavePrice(Pro p);

        /// <summary>
        /// 保存价目表明细
        /// </summary>
        /// <param name="pd"></param>
        /// <returns></returns>
        int SavePriceDetail(ProDetail pd);

        /// <summary>
        /// 获取门店
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        WD_STORE GetStore(int id);

        /// <summary>
        /// 获取价目表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //Pro GetPrice(int id);

        /// <summary>
        /// 获取价目表明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ProDetail GetProDetail(int id);

        /// <summary>
        /// 保存临时二维码
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        int SaveLSQrcode(MJ_Qrcode_ls ls);

        /// <summary>
        /// 获取二维码扫描记录
        /// </summary>
        /// <param name="qid"></param>
        /// <returns></returns>
        MJ_Qrcode_Log GetQrcodeLog(int qid);

        /// <summary>
        /// 获取门店配置列表
        /// </summary>
        /// <param name="Sid"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        PagedList<WD_STORE_SET_EX> GetStoreSetList(int Sid, PageView view);

        /// <summary>
        /// 根据id获取配置信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        WD_STORE_SET GetStoreSet(int id);

        /// <summary>
        /// 根据openid和门店ID获取配置信息
        /// </summary>
        /// <param name="FromUserName"></param>
        /// <param name="StoreID"></param>
        /// <returns></returns>
        WD_STORE_SET GetStoreSet(string FromUserName, int StoreID);

        /// <summary>
        /// 保存门店配置信息
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        int SaveStoreSet(WD_STORE_SET set);

        /// <summary>
        /// 删除配置信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DeleteStoreSet(int id);

        /// <summary>
        /// 获取评价列表
        /// </summary>
        /// <param name="Mid"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        PagedList<WD_Evaluation_EX> GetEvaluationList(string tousername, PageView view);

        /// <summary>
        /// 获取评价
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        WD_Evaluation_EX GetEvaluation(int id);

        /// <summary>
        /// 删除所有门店信息
        /// </summary>
        /// <returns></returns>
        int deleteWD_STORE();
    }
}
