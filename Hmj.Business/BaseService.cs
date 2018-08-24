using Hmj.Common;
using Hmj.Interface;

namespace Hmj.Business
{
    public class BaseService
    {

        public ILogService LogService
        {
            get
            {
                return ServiceFactory.GetInstance<ILogService>();
            }
        }
    }
}
