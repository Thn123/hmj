using System.Collections.Generic;
using System.Text;

namespace Hmj.Entity.ORMapping
{
    public class EntitySQLBuilder
    {

        public static string EntityMetaToInsertSQL(EntityMeta meta)
        {                
            if (meta.Columns == null || meta.Columns.Count == 0)
                return string.Empty;

            StringBuilder sqlbuilder = new StringBuilder();
            sqlbuilder.AppendFormat("INSERT INTO [{0}] (", meta.TableName);
            for (int i = 0, j = 0; i < meta.Columns.Count; i++)
            {
                if (meta.Columns[i].Identity)
                {
                    continue;
                }
                if (j > 0)
                {
                    sqlbuilder.Append(",");
                }
                sqlbuilder.Append("[" + meta.Columns[i].ColumnName + "]");
                j++;
            }
            sqlbuilder.Append(") VALUES (");
            for (int i = 0, j = 0; i < meta.Columns.Count; i++)
            {
                if (meta.Columns[i].Identity)
                {
                    continue;
                }
                if (j > 0)
                {
                    sqlbuilder.Append(",");
                }
                sqlbuilder.Append("@" + meta.Columns[i].ColumnName + "");
                j++;

            }
            sqlbuilder.Append(");");

            if (meta.Columns.Exists(x=>x.Identity))
            {
                sqlbuilder.Append(" SELECT CAST(SCOPE_IDENTITY() as bigint) as Id;"); //sqlbuilder.Append("SELECT SCOPE_IDENTITY();");
            }
            else
            {
                sqlbuilder.Append(" SELECT  CAST(0 as bigint) as Id;");
            }

            return sqlbuilder.ToString();          
        }

        public static string EntityMetaToUpdateSQL(EntityMeta meta)
        {
            if (meta.Columns == null || meta.Columns.Count == 0)
                return string.Empty;


            var keys = meta.Columns.FindAll(_ => _.PrimaryKey);

            StringBuilder sqlbuilder = new StringBuilder();
            sqlbuilder.AppendFormat("UPDATE [{0}] SET ", meta.TableName);

            for (int i = 0, j = 0; i < meta.Columns.Count; i++)
            {               
                if (!meta.Columns[i].PrimaryKey)
                {
                    if (j > 0)
                    {
                        sqlbuilder.Append(",");
                    }
                    j++;
                    sqlbuilder.Append("[" + meta.Columns[i].ColumnName + "]=@" + meta.Columns[i].ColumnName + "");

                }
            }
            sqlbuilder.Append(" WHERE ");
            for (int i = 0; i < keys.Count; i++)
            {
                if (i > 0)
                {
                    sqlbuilder.Append(" AND ");
                }
                sqlbuilder.Append("[" + keys[i].ColumnName + "]=@" + keys[i].ColumnName);
            }

            return sqlbuilder.ToString();
        }   

        internal static string BuildInsertSql(EntityMeta meta, List<string> list)
        {
            if (list == null || list.Count == 0)
                return string.Empty;

            StringBuilder sqlbuilder = new StringBuilder();
            sqlbuilder.AppendFormat("INSERT INTO [{0}] (", meta.TableName);
            for (int i = 0, j = 0; i < meta.Columns.Count; i++)
            {
                if (meta.Columns[i].Identity || !list.Contains(meta.Columns[i].ColumnName))
                {
                    continue;
                }
                if (j > 0)
                {
                    sqlbuilder.Append(",");
                }
                sqlbuilder.Append("[" + meta.Columns[i].ColumnName + "]");
                j++;
            }
            sqlbuilder.Append(") VALUES (");
            for (int i = 0, j = 0; i < meta.Columns.Count; i++)
            {
                if (meta.Columns[i].Identity || !list.Contains(meta.Columns[i].ColumnName))
                {
                    continue;
                }
                if (j > 0)
                {
                    sqlbuilder.Append(",");
                }
                sqlbuilder.Append("@" + meta.Columns[i].ColumnName + "");
                j++;

            }
            sqlbuilder.Append(");");

            if (meta.Columns.Exists(x => x.Identity))
            {
                sqlbuilder.Append(" SELECT CAST(SCOPE_IDENTITY() as bigint) as Id;"); //sqlbuilder.Append("SELECT SCOPE_IDENTITY();");
            }
            else
            {
                sqlbuilder.Append(" SELECT  CAST(0 as bigint) as Id;");
            }

            return sqlbuilder.ToString();          
        }

        internal static string BuildUpdateSql(EntityMeta meta, List<string> list)
        {
            if (list == null || list.Count == 0)
                return string.Empty;


            var keys = meta.Columns.FindAll(_ => _.PrimaryKey);

            StringBuilder sqlbuilder = new StringBuilder();
            sqlbuilder.AppendFormat("UPDATE [{0}] SET ", meta.TableName);

            for (int i = 0, j = 0; i < meta.Columns.Count; i++)
            {
                if (!meta.Columns[i].PrimaryKey && list.Contains(meta.Columns[i].ColumnName))
                {
                    if (j > 0)
                    {
                        sqlbuilder.Append(",");
                    }
                    j++;
                    sqlbuilder.Append("[" + meta.Columns[i].ColumnName + "]=@" + meta.Columns[i].ColumnName + "");
                }
            }
            sqlbuilder.Append(" WHERE ");
            for (int i = 0; i < keys.Count; i++)
            {
                if (i > 0)
                {
                    sqlbuilder.Append(" AND ");
                }
                sqlbuilder.Append("[" + keys[i].ColumnName + "]=@" + keys[i].ColumnName);
            }

            return sqlbuilder.ToString();
        }
    }
}
