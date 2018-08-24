using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Entity.PageSearch;
using System;
using System.Collections.Generic;

namespace Hmj.DataAccess.Repository
{
    public class OrgDictRepository : BaseRepository
    {
        /// <summary>
        /// 获取一个公司所有的数据字典
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public List<ORG_DICT> GetAllORG_DICT(int orgId)
        {
            string cols = @" SELECT distinct A.* FROM ORG_DICT A  ORDER BY A.DICT_SEQ";
            return base.Query<ORG_DICT>(cols, null);
        }

        /// <summary>
        /// 根据父级编号获得子数据字典
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public PagedList<ORG_DICT> GetORG_DICTByParentId(Guid pId, PageView view, int orgId)
        {
            string cols = @" A.* ";
            return base.PageGet<ORG_DICT>(view, cols,
               "[ORG_DICT] A "
               , " AND A.PARENT_ID='" + pId + "' "
               , "A.[DICT_SEQ]", "");
        }

        /// <summary>
        /// 根据父级编码获得子数据字典
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public List<ORG_DICT_EX> GetORG_DICTByPCode(string pCode, int orgId)
        {
            string cols = @"SELECT A.*,isnull(B.DICT_VALUE,'数据字典') 
      PARENT_NAME FROM ORG_DICT A LEFT JOIN ORG_DICT B ON A.PARENT_ID=B.ID
      WHERE A.PARENT_ID=(SELECT ID FROM ORG_DICT DD WHERE DD.DICT_CODE=@DICT_CODE ) ";
            return base.Query<ORG_DICT_EX>(cols, new { DICT_CODE = pCode });
        }

        /// <summary>
        /// 根据字典编号获得字典对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ORG_DICT_EX GetORG_DICTById(Guid id)
        {
            string cols = @"SELECT A.*,isnull(B.DICT_VALUE,'数据字典') PARENT_NAME FROM ORG_DICT A LEFT JOIN ORG_DICT B ON A.PARENT_ID=B.ID WHERE A.ID='" + id + "'";
            return base.Get<ORG_DICT_EX>(cols, null);
        }

        /// <summary>
        /// 检查字典的重复性:同一个公司同一个父级相同名称或值不能重复
        /// </summary>
        /// <param name="cuObj"></param>
        /// <returns></returns>
        public int SaveValidate(ORG_DICT cuObj)
        {
            string sql = @"SELECT count([ID]) FROM [ORG_DICT] where DICT_VALUE=@DICT_VALUE and ID<>@ID AND ORG_ID=@ORG_ID AND PARENT_ID=@PARENT_ID";
            var cuo = base.Get<int>(sql, new { DICT_VALUE = cuObj.DICT_VALUE, ID = cuObj.ID, ORG_ID = cuObj.ORG_ID, PARENT_ID = cuObj.PARENT_ID });
            if (cuo > 0)
                return -2;
            sql = @"SELECT count([ID]) FROM [ORG_DICT] where DICT_CODE=@DICT_CODE and ID<>@ID AND ORG_ID=@ORG_ID AND PARENT_ID=@PARENT_ID";
            cuo = base.Get<int>(sql, new { DICT_CODE = cuObj.DICT_CODE, ID = cuObj.ID, ORG_ID = cuObj.ORG_ID, PARENT_ID = cuObj.PARENT_ID });
            if (cuo > 0)
                return -3;
            return 0;
        }

        /// <summary>
        /// 删除的字典下面是否有子节点
        /// </summary>
        /// <param name="cuObj"></param>
        /// <returns></returns>
        public int DeleteORG_DICT(Guid id)
        {
            string sql = @"SELECT COUNT(0) FROM [ORG_DICT] where PARENT_ID=@PARENT_ID";
            var re = base.Get<int>(sql, new { PARENT_ID = id });
            if (re > 0)
                return -2;
            sql = "DELETE FROM ORG_DICT WHERE ID=@ID";

            //查询对象
            string sqlDc = @"SELECT * FROM [ORG_DICT] where ID=@ID";
            var dicObj = base.Get<ORG_DICT>(sqlDc, new { ID = id });
            if (dicObj == null)
                return 1;
            //查询父级的sql语句
            var selectSql = "SELECT CHECK_SQL FROM [ORG_DICT] WHERE [ID]=@ID";
            var checkSql = base.Get<string>(selectSql, new { ID = dicObj.PARENT_ID });
            if (string.IsNullOrEmpty(checkSql))
            {
                return base.Excute(sql, new { ID = id });
            }
            else
            {
                try
                {
                    var res = base.Get<int>(checkSql, new { DICT_CODE = dicObj.DICT_CODE, ORG_ID = dicObj.ORG_ID });
                    if (res > 0)
                        return -3;
                    else return base.Excute(sql, new { ID = id });
                }
                catch (Exception)
                {
                    return -4;
                }

            }

        }
    }
}
