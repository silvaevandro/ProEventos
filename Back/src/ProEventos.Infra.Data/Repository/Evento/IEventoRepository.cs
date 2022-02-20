using ProEventos.Domain.Entities;

namespace ProEventos.Infra.Data
{
    public interface IEventoRepository
    {
        //Eventos
        Task<Evento[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes);
        Task<Evento[]> GetAllEventosAsync(int userId, bool includePalestrantes);
        Task<Evento> GetAllEventosByIdAsync(int userId, int eventoId, bool includePalestrantes);
    }
}