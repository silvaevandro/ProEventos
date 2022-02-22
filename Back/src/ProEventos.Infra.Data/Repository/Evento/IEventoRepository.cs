using ProEventos.Domain.Entities;
using ProEventos.Infra.Data.Models;

namespace ProEventos.Infra.Data
{
    public interface IEventoRepository
    {
        //Eventos
        //Task<PageList<Evento>> GetAllEventosByTemaAsync(int userId, PageParms pageParms, string tema, bool includePalestrantes);
        Task<PageList<Evento>> GetAllEventosAsync(int userId, PageParms pageParms, bool includePalestrantes);
        Task<Evento> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes);
    }
}