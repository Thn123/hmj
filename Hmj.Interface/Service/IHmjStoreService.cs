using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Entity.PageSearch;
using Hmj.Entity.SearchEntity;
using System.Collections.Generic;

namespace Hmj.Interface
{
    public interface IBcjStoreService
    {
        /// <summary>
        /// 推送模板插入日志
        /// </summary>
        /// <param name="template_Id"></param>
        /// <param name="url"></param>
        /// <param name="touser"></param>
        /// <param name="tmpStr"></param>
        /// <returns></returns>
        int InsertLog(WX_TMP_HIS his);

        /// <summary>
        /// 验证是否有该编号门店
        /// </summary>
        /// <param name="storeCode"></param>
        /// <returns></returns>
        int ChckStore(string storeCode);

        /// <summary>
        /// 登录门店
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="storecode"></param>
        /// <returns></returns>
        long StoreLogin(string mobile, string storecode,string pwd);

        /// <summary>
        /// 得到门店数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        List<BCJ_STORES_EX> GetStoresByCityCode(string code);
        void UpdateOk(int iD);

        /// <summary>
        /// 发送模板
        /// </summary>
        /// <param name="request"></param>
        /// <param name="access_token"></param>
        /// <returns></returns>
        int SendTmp(BCJ_TMP_DETAIL request,string access_token);

        /// <summary>
        /// 得到门店的详细信息
        /// </summary>
        /// <param name="storeCode"></param>
        /// <returns></returns>
        BCJ_STORES_EX GetStoreEntity(string storeCode);

        /// <summary>
        /// 启动批量发送模板
        /// </summary>
        void StartSendTmp();
        void GoToSendPoint(int fansid, string openid);
        PagedList<STORES_EXCEL> QueryExcels(ExcelSearch search, PageView view);

        bool QueryBcjStores();
    }
}
