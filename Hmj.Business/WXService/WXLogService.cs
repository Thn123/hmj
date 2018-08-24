using Hmj.DataAccess.Repository;
using Hmj.Entity;

namespace Hmj.Business.WXService
{
    public class WXLogService
    {
        private WXLogReponsitory _WXLogReponsitory;
        private WXLogReponsitory WXLogReponsitory
        {
            get
            {
                if (_WXLogReponsitory == null)
                {
                    _WXLogReponsitory = new WXLogReponsitory();
                }
                return _WXLogReponsitory;
            }
        }

        /// <summary>
        /// 保存短信日志对象
        /// </summary>
        /// <param name="l">WXDXLog对象</param>
        /// <returns></returns>
        public int Save(WXLOG l)
        {
            if (l.ID <= 0)
            {
                return (int)WXLogReponsitory.Insert(l);
            }
            else
            {
                return WXLogReponsitory.Update(l);
            }
        }

    }
}
