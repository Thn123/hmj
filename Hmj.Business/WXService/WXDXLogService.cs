using Hmj.DataAccess.Repository;
using Hmj.Entity;

namespace Hmj.Business.WXService
{
    public class WXDXLogService
    {
        private WXDXLogReponsitory _WXDXLogReponsitory;
        private WXDXLogReponsitory WXDXLogReponsitory
        {
            get
            {
                if (_WXDXLogReponsitory == null)
                {
                    _WXDXLogReponsitory = new WXDXLogReponsitory();
                }
                return _WXDXLogReponsitory;
            }
        }

        /// <summary>
        /// 保存短信日志对象
        /// </summary>
        /// <param name="dl">WXDXLog对象</param>
        /// <returns></returns>
        public int Save(WXDXLog dl)
        {
            if (dl.ID <= 0)
            {
                return (int)WXDXLogReponsitory.Insert(dl);
            }
            else
            {
                return WXDXLogReponsitory.Update(dl);
            }
        }
    }
}
