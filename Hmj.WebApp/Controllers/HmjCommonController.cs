using Hmj.Common;
using Hmj.DTO;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Hmj.WebApp.Controllers
{
    public class HmjCommonController : Controller
    {
        /// <summary>
        /// 显示页面
        /// </summary>
        /// <returns></returns>

        public async Task<ActionResult> Index()
        {
            JsonDTO<CityResDTO> result = await RequestHelp.RequestGet<CityResDTO>("Test/PostTest", null);
            return Json(result);
        }
    }
}
