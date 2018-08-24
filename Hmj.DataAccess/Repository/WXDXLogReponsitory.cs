using Hmj.Entity;

namespace Hmj.DataAccess.Repository
{
    public class WXDXLogReponsitory : BaseRepository
    {
        public int Save(WXDXLog log)
        {
            return (int)Insert(log);
        }
    }
}
