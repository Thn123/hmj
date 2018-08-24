using Hmj.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hmj.DataAccess.Repository
{
    public class ThdPlatformBaseRepository : Vulcan.Repository.BaseRepository
    {
        public ThdPlatformBaseRepository()
           : base(ConnectionStringHelper.GetValueByKey(AppConfig.ThirdPlatformDbKey))
        {
        }
        public ThdPlatformBaseRepository(string dbkey)
            : base(ConnectionStringHelper.GetValueByKey(dbkey))
        {
        }

    }
}
