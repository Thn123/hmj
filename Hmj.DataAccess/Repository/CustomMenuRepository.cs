using Hmj.Entity;
using Hmj.Entity.Entities;
using System.Collections.Generic;

namespace Hmj.DataAccess.Repository
{
    public class CustomMenuRepository : BaseRepository
    {
        public WXCustomMenu GetCustomMenu(int id)
        {
            var getRootMenuSql = @"select top 1 * from WXCustomMenu where ID=@ID";
            return base.Get<WXCustomMenu>(getRootMenuSql, new { ID = id });
        }

        public WXCustomMenu GetRootMenu(int merchants_ID)
        {
            var getRootMenuSql = @"select top 1 * from WXCustomMenu where ParentID is null and Merchants_ID=@Merchants_ID";
            return base.Get<WXCustomMenu>(getRootMenuSql, new { Merchants_ID = merchants_ID });
        }

        public List<WXCustomMenu_EX> GetCustomMenuList(int merchants_ID)
        {
            var sql = @"SELECT A.*,B.Media_ID FROM WXCustomMenu A
LEFT JOIN WXGraphicList B ON A.Graphic_ID=B.ID
where A.Merchants_ID=@Merchants_ID ORDER BY OrderNum asc";
            return base.Query<WXCustomMenu_EX>(sql, new { Merchants_ID = merchants_ID });
        }

        public int DeleteMenu(int id)
        {
            var sql = @"DELETE FROM [WXCustomMenu] WHERE ID=@ID";
            return base.Excute(sql, new { ID = id });
        }

        public int DeleteMenuByParentID(int parentID)
        {
            var sql = @"DELETE FROM [WXCustomMenu] WHERE ParentID=@ParentID";
            return base.Excute(sql, new { ParentID = parentID });
        }

        public int GetChildrenCount(int parentID)
        {
            var sql = @"SELECT COUNT(*) FROM [WXCustomMenu] where ParentID =@ParentID";
            return base.Get<int>(sql, new { ParentID = parentID });
        }

        public WXCustomMenu ExistType7Menu(int merchants_ID)
        {
            return base.Get<WXCustomMenu>(@"SELECT TOP 1 * FROM [WXCRM].[dbo].[WXCustomMenu] WHERE TYPE=7 AND Merchants_ID=@Merchants_ID", new { Merchants_ID = merchants_ID });
        }
    }
}
