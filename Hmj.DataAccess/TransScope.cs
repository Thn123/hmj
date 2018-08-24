namespace Hmj.DataAccess
{
    public class TransScope : Vulcan.Framework.DBConnectionManager.TransScope
    {
        public TransScope()
            : base(ConnectionStringHelper.GetValueByKey(Hmj.Common.AppConfig.MainDbKey))
        {
        }
    }
}
