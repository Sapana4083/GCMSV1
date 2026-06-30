using Microsoft.AspNetCore.Mvc;

namespace GCMS.Controllers
{
    public class UserMasterController : Controller
    {
        public IActionResult UserLogin()
        {
            return View();
        }
    }
}
