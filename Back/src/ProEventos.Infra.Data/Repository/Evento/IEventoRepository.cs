using ProEventos.Domain.Entities;
using ProEventos.Infra.Data.Models;

namespace ProEventos.Infra.Data
{
    public interface IEventoRepository : IGeralRepository
    {
        Task<PageList<Evento>> GetAllEventosAsync(int userId, PageParms pageParms, bool includePalestrantes);
        Task<Evento> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes);
    }
}