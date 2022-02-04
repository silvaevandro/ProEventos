using ProEventos.Domain.Entities;

namespace ProEventos.Infra.Data
{
    public interface IEventoRepository
    {
        //Eventos
        Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes);
        Task<Evento[]> GetAllEventosAsync(bool includePalestrantes);
        Task<Evento> GetAllEventosByIdAsync(int eventoId, bool includePalestrantes);
    }
}