using Microsoft.AspNetCore.Mvc;

namespace Victory.Network.Api.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
