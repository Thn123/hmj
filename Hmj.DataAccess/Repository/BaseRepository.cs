using Dapper;
using Hmj.Common;
using Hmj.Entity.PageSearch;
using System.Data;

namespace Hmj.DataAccess.Repository
{
    public abstract class BaseRepository : Vulcan.Repository.BaseRepository
    {        
        public BaseRepository()
            : base(ConnectionStringHelper.GetValueByKey(AppConfig.MainDbKey))
        {
        }
        public BaseRepository(string dbkey)
            : base(ConnectionStringHelper.GetValueByKey(dbkey))
        {
        }

        protected PagedList<T> PageGet<T>(PageView view, string sqlColumns, string sqlTable,
            string sqlCondition, string sqlPk, string sqlOrder)
        {
            if (string.IsNullOrEmpty(sqlOrder))
            {
                sqlOrder = " ORDER BY " + sqlPk;
            }
            //edit by xuanye 2014.9
            //else
            //{
            //    sqlOrder = " ORDER BY " + view.SortName + " " + view.SortOrder;
            //}           
            var p = new DynamicParameters();           
            int total = 0;
            p.Add("@SQLPARAMS", sqlCondition,DbType.String,ParameterDirection.Input,null);
            p.Add("@PAGESIZE", view.PageSize, DbType.Int32, ParameterDirection.Input, null);
            p.Add("@PAGEINDEX", view.PageIndex, DbType.Int32, ParameterDirection.Input, null);
            p.Add("@SQLTABLE", sqlTable, DbType.String, ParameterDirection.Input, null);
            p.Add("@SQLCOLUMNS", sqlColumns, DbType.String, ParameterDirection.Input, null);
            p.Add("@SQLPK", sqlPk, DbType.String, ParameterDirection.Input, null);
            p.Add("@SQLORDER", sqlOrder, DbType.String, ParameterDirection.Input, null);
            p.Add("@Count", total, DbType.Int32, ParameterDirection.Output, null);
            
            var list = base.SPQuery<T>("sp_pageselect", p);
           
            PagedList<T> pList = new PagedList<T>();

            if (view.PageIndex == 0)
            {
                pList.Total = p.Get<int>("@Count");
            }
            pList.DataList = list;
          
            pList.PageIndex = view.PageIndex;
            pList.PageSize = view.PageSize;
            return pList;
        }

        
    } 
}
