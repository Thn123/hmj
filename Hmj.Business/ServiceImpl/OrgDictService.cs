using Hmj.DataAccess.Repository;
using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Entity.PageSearch;
using Hmj.Interface;
using System;
using System.Collections.Generic;

namespace Hmj.Business.ServiceImpl
{
    public class OrgDictService : IOrgDictService
    {
        OrgDictRepository _repo;
        public OrgDictService()
        {
            _repo = new OrgDictRepository();
        }

        /// <summary>
        /// 获取一个公司所有的数据字典
        /// </summary>
        /// <returns></returns>
        public List<ORG_DICT> GetAllORG_DICT(int orgId)
        {
            return _repo.GetAllORG_DICT(orgId);
        }

        /// <summary>
        /// 根据父级代码获得子数据字典
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public PagedList<ORG_DICT> GetORG_DICTByParentId(Guid pId, PageView view, int orgId)
        {
            return _repo.GetORG_DICTByParentId(pId, view,orgId);
        }

        /// <summary>
        /// 根据字典代码获得字典对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ORG_DICT_EX GetORG_DICTById(Guid id)
        {
            return _repo.GetORG_DICTById(id);
        }

        /// <summary>
        /// 保存字典对象
        /// </summary>
        /// <param name="cuObj"></param>
        /// <returns></returns>
        public int SaveORG_DICT(ORG_DICT cuObj)
        {
            try
            {
                int re = _repo.SaveValidate(cuObj);
                if (re < 0)
                    return re;//重复
                else
                {
                    if (cuObj.ID == Guid.Empty) // 新增
                    {
                        cuObj.ID = Guid.NewGuid();
                        re = (int)_repo.Insert(cuObj);
                    }
                    else
                    {
                        re = _repo.Update(cuObj);
                    }
                }
                return re;
            }
            catch (Exception)
            {
                return -1;
            }
          
        }

        /// <summary>
        /// 删除字典对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteORG_DICT(Guid id)
        {
            return _repo.DeleteORG_DICT(id);
        }

        /// <summary>
        /// 根据父级代码获得子数据字典 有父级名称
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public List<ORG_DICT_EX> GetORG_DICTByPCode(string pCode, int orgId)
        {
            return _repo.GetORG_DICTByPCode(pCode, orgId);
        }
    }
}
