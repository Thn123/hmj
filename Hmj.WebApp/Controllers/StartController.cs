using Hmj.Interface;
using System.Web.Mvc;

namespace Hmj.WebApp.Controllers
{
    public class StartController : TtBaseController
    {
        public StartController(ISystemService sbo) :
            base(sbo)
        {
        }

        //
        // GET: /Start/

        public ActionResult Index()
        {
            Base();

            return View();
        }

    }
}
