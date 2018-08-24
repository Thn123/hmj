using Hmj.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hmj.DataAccess.WXRespository
{
    public class CouponBaseRepository : Vulcan.Repository.BaseRepository
    {
        public CouponBaseRepository()
            : base(ConnectionStringHelper.GetValueByKey(AppConfig.CouponDbKey))
        {
        }
        public CouponBaseRepository(string dbkey)
            : base(ConnectionStringHelper.GetValueByKey(dbkey))
        {
        }

    }
}
