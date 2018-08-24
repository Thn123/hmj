using Hmj.Entity.ORMapping;
using System;
using System.Collections.Generic;
using Vulcan.Framework.ORMapping;

namespace Hmj.Entity
{
    public abstract class BaseEntity:IEntity
    {
        private static Dictionary<Type, string> _InsertSqlCache = new Dictionary<Type, string>();
        private static Dictionary<Type, string> _UpdateSqlCache = new Dictionary<Type, string>();

        private static object lockobject = new object();
        private List<string> _PropertyChangedList = new List<string>();

        #region 属性
        [Ignore]
        public bool FullUpdate { get; set; }
        #endregion
        #region 公开方法
        public string GetInsertSQL()
        {
            if (FullUpdate)
            {
                return GetInsertFullSql();
            }
            return GetInsertChangeColumnsSql();
        }
     
        public string GetUpdateSQL()
        {
            if (FullUpdate)
            {
                return GetUpdateFullSql();
            }
            return GetUpdateChangeColumnsSql();
        }

      
        public void RemoveUpdateColumn(string ColumnName)
        {
            lock (lockobject)
            {
                if (_PropertyChangedList.Contains(ColumnName))
                {
                    _PropertyChangedList.Remove(ColumnName);
                }
            }
        }
        #endregion

        protected void Clear()
        {
            lock (lockobject)
            {
                _PropertyChangedList.Clear();
            }
        }

        protected void OnPropertyChanged(string pName)
        {
            lock(lockobject)
            {
                if (!_PropertyChangedList.Contains(pName))
                {
                    _PropertyChangedList.Add(pName);
                }
            }
        }

        #region 私有方法
        private string GetInsertFullSql()
        {
            Type t = this.GetType();
            if (!_InsertSqlCache.ContainsKey(t))
            {
                EntityMeta metadeta = EntityReflect.GetDefineInfoFromType(t);
                string sql = EntitySQLBuilder.EntityMetaToInsertSQL(metadeta);
                lock (lockobject)
                {                 
                    if (!_InsertSqlCache.ContainsKey(t))
                    {                      
                        _InsertSqlCache.Add(t, sql);
                    }
                }

            }
            return _InsertSqlCache[t];
        }
        private string GetUpdateFullSql()
        {
            Type t = this.GetType();
            if (!_UpdateSqlCache.ContainsKey(t))
            {
                EntityMeta metadeta = EntityReflect.GetDefineInfoFromType(t);
                string sql = EntitySQLBuilder.EntityMetaToUpdateSQL(metadeta);
                lock (lockobject)
                {
                    if (!_UpdateSqlCache.ContainsKey(t))
                    {
                        _UpdateSqlCache.Add(t, sql);
                    }
                }

            }
            return _UpdateSqlCache[t];
        }
        private string GetInsertChangeColumnsSql()
        {
            EntityMeta metadeta = EntityReflect.GetDefineInfoFromType(this.GetType());
            return EntitySQLBuilder.BuildInsertSql(metadeta,this._PropertyChangedList);
        }
        private string GetUpdateChangeColumnsSql()
        {
            EntityMeta metadeta = EntityReflect.GetDefineInfoFromType(this.GetType());
            return EntitySQLBuilder.BuildUpdateSql(metadeta, this._PropertyChangedList);
        }
        #endregion
    }
}
