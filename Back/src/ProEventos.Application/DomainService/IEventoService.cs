using ProEventos.Application.ViewModels;

namespace ProEventos.Application.DomainService
{
    public interface IEventoService
    {
        Task<EventoViewModel> AddEvento(EventoViewModel model);
        Task<EventoViewModel> UpdateEvento(int eventoId, EventoViewModel model);
        Task<bool> DeleteEvento(int eventoId);
        Task<EventoViewModel[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false);
        Task<EventoViewModel[]> GetAllEventosAsync(bool includePalestrantes = false);
        Task<EventoViewModel> GetAllEventosByIdAsync(int eventoId, bool includePalestrantes = false);
    }
}