using CepdiModel;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace CepdiPortal.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly HttpClient _httpClient;

        public UsuariosController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7074/api/Usuarios?page=1"); 
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetUsuarios()
        {
            return View();
        }
    }
}
