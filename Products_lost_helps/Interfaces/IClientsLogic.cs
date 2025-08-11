using Products_lost_helps.Models;

namespace Products_lost_helps.Interfaces
{
    public interface IClientsLogic
    {
        IEnumerable<Clients> GetAllClients();
    }
}
