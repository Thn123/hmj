using Hmj.DataAccess.Repository;
using Hmj.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hmj.DataAccess.MiniProgram
{
    public class MiniProgramRepository : BaseRepository
    {
        public Mini_sessionkey GetMini_Sessionkey(string minikey)
        {
            string sql = @"select 
                            id,
                            minikey,
                            openid,
                            session_key,
                            unionid,
                            account_id,
                            create_date
                            from mini_sessionkey with(nolock) where minikey=@minikey";
            return base.Get<Mini_sessionkey>(sql, new { minikey= minikey });
        }
    }
}
