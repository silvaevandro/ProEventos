using ProEventos.Application.ViewModels;
using ProEventos.Infra.Data.Models;

namespace ProEventos.Application.Services
{
    public interface IEventoService
    {
        Task<EventoViewModel> AddEvento(int userId, EventoViewModel model);
        Task<EventoViewModel> UpdateEvento(int userId, int eventoId, EventoViewModel model);
        Task<bool> DeleteEvento(int userId, int eventoId);
        Task<PageList<EventoViewModel>> GetAllEventosAsync(int userId, PageParms pageParms, bool includePalestrantes = false);
        Task<EventoViewModel> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false);
    }
}