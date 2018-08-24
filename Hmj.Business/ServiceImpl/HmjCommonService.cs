using Hmj.Common;
using Hmj.DataAccess.Repository;
using Hmj.DTO;
using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Interface;
using StructureMap.Attributes;
using System.Collections.Generic;

namespace Hmj.Business.ServiceImpl
{
    public class HmjCommonService : IHmjCommonService
    {
        [SetterProperty]
        public ILogService LogService { get; set; }


        //新的业务
        HmjCommonRepository _common;

        public HmjCommonService()
        {
            _common = new HmjCommonRepository();
        }

        /// <summary>
        /// 得到集合
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<CityResDTO> GetAdministrativeDivision()
        {
            List<HMJ_CITY> members = _common.GetAdministrativeDivision();
            List<CityResDTO> NewMembers = new List<CityResDTO>();
            //一级
            members.FindAll(x => x.parent_id == 1).ForEach(x =>
            {
                NewMembers.Add(new CityResDTO()
                {
                    CODE = x.region_id,
                    NAME = x.region_name
                });
            });
            //二级
            NewMembers.ForEach(x =>
            {
                x.CHILD = new List<CityResDTO>();
                members.FindAll(y => y.parent_id > 1).ForEach(y =>
                {
                    if (y.parent_id == x.CODE)
                    {
                        x.CHILD.Add(new CityResDTO()
                        {
                            CODE = y.region_id,
                            NAME = y.region_name,
                            CHILD = new List<CityResDTO>()
                        });
                    }
                });
            });
            ////三级
            //NewMembers.FindAll(x => x.CHILD.Count > 0).ForEach(x =>
            //    {
            //        x.CHILD.ForEach(y =>
            //        {
            //            y.CHILD = new List<CityResDTO>();
            //            members.FindAll(z => z.parent_id > 0).ForEach(z =>
            //            {
            //                if (z.parent_id == y.CODE)
            //                {
            //                    y.CHILD.Add(new CityResDTO()
            //                    {
            //                        CODE = z.region_id,
            //                        NAME = z.region_name
            //                    });
            //                }
            //            });
            //        });
            //    });

            return NewMembers;
        }


        public long InsertLog(Tb_log log)
        {
            return _common.Insert(log);
        }

        /// <summary>
        /// 得到城市列表
        /// </summary>
        /// <returns></returns>
        public List<BCJ_CITY> GetCity()
        {
            return _common.GetCity();
        }

        /// <summary>
        /// 得到门店数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public List<BCJ_STORES_EX> GetStoresByCityCode(string code)
        {
            return _common.GetStoresByCityCode(code);
        }
    }
}
