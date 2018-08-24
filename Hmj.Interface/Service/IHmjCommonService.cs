using Hmj.DTO;
using Hmj.Entity;
using Hmj.Entity.Entities;
using System.Collections.Generic;

namespace Hmj.Interface
{
    public interface IHmjCommonService
    {
        /// <summary>
        /// 得到城市
        /// </summary>
        /// <returns></returns>
        List<CityResDTO> GetAdministrativeDivision();

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        long InsertLog(Tb_log log);

        /// <summary>
        /// 得到城市列表
        /// </summary>
        /// <returns></returns>
        List<BCJ_CITY> GetCity();

        /// <summary>
        /// 得到门店数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        List<BCJ_STORES_EX> GetStoresByCityCode(string code);
    }
}
