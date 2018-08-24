using Hmj.Entity.PageSearch;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Hmj.Entity.Jsons
{
    public class JsonQTable
    {
        public JsonQTable()
        {
        }

         public JsonQTable(
            int pageIndex, int totalCount, IList<JsonQRow> data)
        {
            page = pageIndex;
            total = totalCount;
            rows = data;
        }
        public int page { get; set; }
        public int total { get; set; }
        public IList<JsonQRow> rows { get; set; }

        public static JsonQTable ConvertFromList<T>(List<T> list, string key,string[] cols) where T : class
        {
            JsonQTable data = new JsonQTable();
            data.page = 1;
            if (list != null)
            {
                data.total = list.Count;
                data.rows = new List<JsonQRow>();
                foreach (T t in list)
                {
                    JsonQRow row = new JsonQRow();
                    row.id = getValue<T>(t, key);
                    row.cell = new List<string>();
                    foreach (string col in cols)
                    {
                        row.cell.Add(getValue<T>(t, col));
                    }
                    data.rows.Add(row);
                }
            }
            else
            {
                data.total = 0;
            }
            return data;
        }
        public static JsonQTable ConvertFromList<T>(List<T> list, string key, string[] cols,Func<T,int> checkStateHandler) where T : class
        {
            JsonQTable data = new JsonQTable();
            data.page = 1;
            if (list != null)
            {
                data.total = list.Count;
                data.rows = new List<JsonQRow>();
                foreach (T t in list)
                {
                    JsonQRow row = new JsonQRow();
                    row.id = getValue<T>(t, key);
                    row.check_state = checkStateHandler(t);
                    row.cell = new List<string>();
                    foreach (string col in cols)
                    {
                        row.cell.Add(getValue<T>(t, col));
                    }
                    data.rows.Add(row);
                }
            }
            else
            {
                data.total = 0;
            }
            return data;
        }

        public static JsonQTable ConvertFromList<T>(List<T> list, string key, string[] cols, Func<string, bool> checkExtP, Func<string, T, string> exciteExtP,Func<T,int> checkStateHandler) where T : class
        {
            JsonQTable data = new JsonQTable();
            data.page = 1;
            if (list != null)
            {
                data.total = list.Count;
                data.rows = new List<JsonQRow>();
                foreach (T t in list)
                {
                    JsonQRow row = new JsonQRow();
                    row.id = getValue<T>(t, key);
                    row.check_state = checkStateHandler(t);
                    row.cell = new List<string>();
                    foreach (string col in cols)
                    {
                        if (checkExtP(col))
                        {
                            row.cell.Add(exciteExtP(col, t));
                        }
                        else
                        {
                            row.cell.Add(getValue<T>(t, col));
                        }
                    }
                    data.rows.Add(row);
                }
            }
            else
            {
                data.total = 0;
            }
            return data;
        }

        private static string getValue<T>(T t, string pname) where T : class
        {
            Type type = t.GetType();
            PropertyInfo pinfo = type.GetProperty(pname);
            if (pinfo != null)
            {
                object v = pinfo.GetValue(t, null);
                return v != null ? v.ToString() : "";
            }
            return "";
        }
        public static JsonQTable ConvertFromPagedList<T>(PagedList<T> pagelist, string key, string[] cols, Func<string, bool> checkExtP, Func<string, T, string> excuteExtP, Func<T, int> checkStateHandler) where T : class
        {
            JsonQTable data = new JsonQTable();
            data.page = pagelist.PageIndex + 1;
            if (pagelist.PageIndex == 0)
            {
                data.total = pagelist.Total;
            }
            else
            {
                data.total = -1;
            }
            data.rows = new List<JsonQRow>();
            foreach (T t in pagelist.DataList)
            {
                JsonQRow row = new JsonQRow();
                row.id = getValue<T>(t, key);
                row.check_state = checkStateHandler(t);
                row.cell = new List<string>();
                foreach (string col in cols)
                {
                    if (checkExtP(col))
                    {
                        row.cell.Add(excuteExtP(col, t));
                    }
                    else
                    {
                        row.cell.Add(getValue<T>(t, col));
                    }
                }
                data.rows.Add(row);
            }
            return data;
        }
        public static JsonQTable ConvertFromPagedList<T>(PagedList<T> pagelist, string key, string[] cols, Func<T, int> checkStateHandler) where T : class
        {
            JsonQTable data = new JsonQTable();
            data.page = pagelist.PageIndex + 1;
            if (pagelist.PageIndex == 0)
            {
                data.total = pagelist.Total;
            }
            else
            {
                data.total = -1;
            }
            data.rows = new List<JsonQRow>();
            foreach (T t in pagelist.DataList)
            {
                JsonQRow row = new JsonQRow();
                row.id = getValue<T>(t, key);
                row.check_state = checkStateHandler(t);
                row.cell = new List<string>();
                foreach (string col in cols)
                {
                    row.cell.Add(getValue<T>(t, col));
                }
                data.rows.Add(row);
            }
            return data;
        }
        public static JsonQTable ConvertFromPagedList<T>(PagedList<T> pagelist,string key, string[] cols) where T : class
        {
            JsonQTable data = new JsonQTable();
            data.page = pagelist.PageIndex + 1;
            if (pagelist.PageIndex == 0)
            {
                data.total = pagelist.Total;
            }
            else
            {
                data.total = -1;
            }
            data.rows = new List<JsonQRow>();
            foreach (T t in pagelist.DataList)
            {
                JsonQRow row = new JsonQRow();
                row.id = getValue<T>(t, key);
                row.cell = new List<string>();
                foreach (string col in cols)
                {
                    row.cell.Add(getValue<T>(t, col));
                }
                data.rows.Add(row);
            }
            return data;
        }



    }

    public class JsonQRow
    {
        public string id { get; set; }
        public int check_state {get;set;}
        public List<string> cell { get; set; }
    }
}
