using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Entity.PageSearch;
using System;
using System.Collections.Generic;

namespace Hmj.Interface
{
    public interface IOrgDictService
    {
        /// <summary>
        /// 获取一个公司所有的数据字典
        /// </summary>
        /// <returns></returns>
        List<ORG_DICT> GetAllORG_DICT(int orgId);

        /// <summary>
        /// 根据父级代码获得子数据字典
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        PagedList<ORG_DICT> GetORG_DICTByParentId(Guid pId, PageView view,int orgId);

        /// <summary>
        /// 根据字典代码获得字典对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ORG_DICT_EX GetORG_DICTById(Guid id);

        /// <summary>
        /// 保存字典对象
        /// </summary>
        /// <param name="cuObj"></param>
        /// <returns></returns>
        int SaveORG_DICT(ORG_DICT cuObj);

        /// <summary>
        /// 删除字典对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DeleteORG_DICT(Guid id);

        /// <summary>
        /// 根据父级代码获得子数据字典
        /// </summary>
        /// <param name="pCode"></param>
        /// <param name="orgId"></param>
        /// <returns></returns>
        List<ORG_DICT_EX> GetORG_DICTByPCode(string pCode, int orgId);

    }
}
