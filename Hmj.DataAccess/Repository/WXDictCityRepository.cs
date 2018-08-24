using Hmj.Entity;
using System.Collections.Generic;

namespace Hmj.DataAccess.Repository
{
    public class WXDictCityRepository : BaseRepository
    {
        public List<WXDictProvince> Query()
        {
            string sql = @"SELECT * FROM [WXDictProvince]";
            return base.Query<WXDictProvince>(sql, null);
        }
    }
}
