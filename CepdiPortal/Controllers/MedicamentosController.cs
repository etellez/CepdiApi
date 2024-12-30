using Microsoft.AspNetCore.Mvc;

namespace CepdiPortal.Controllers
{
    public class MedicamentosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
