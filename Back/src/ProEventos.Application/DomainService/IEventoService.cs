using ProEventos.Application.ViewModels;

namespace ProEventos.Application.DomainService
{
    public interface IEventoService
    {
        Task<EventoViewModel> AddEvento(int userId, EventoViewModel model);
        Task<EventoViewModel> UpdateEvento(int userId, int eventoId, EventoViewModel model);
        Task<bool> DeleteEvento(int userId, int eventoId);
        Task<EventoViewModel[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes = false);
        Task<EventoViewModel[]> GetAllEventosAsync(int userId, bool includePalestrantes = false);
        Task<EventoViewModel> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false);
    }
}