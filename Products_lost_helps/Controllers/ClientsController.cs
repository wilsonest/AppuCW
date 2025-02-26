using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Products_lost_helps.Logica;
using Products_lost_helps.Models;
using System.Data;

namespace Products_lost_helps.Controllers
{
    public class ClientsController : Controller
    {
        private readonly Lo_Clients _loClients;

        public ClientsController(Lo_Clients loClients)
        {
            _loClients = loClients;
        }

        public IActionResult Clients()
        {
            
            IEnumerable<Clients> clientes = _loClients.GetAllClients();
            return View(clientes);
        }
    }
}
