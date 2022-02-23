using ProEventos.Application.ViewModels;
using ProEventos.Infra.Data.Models;

namespace ProEventos.Application.DomainService
{
    public interface IPalestranteService
    {
        Task<PalestranteViewModel> AddPalestrantes(int userId, PalestranteAddViewModel model);
        Task<PalestranteViewModel> UpdatePalestrante(int userId, PalestranteUpdateViewModel model);
        Task<PageList<PalestranteViewModel>> GetAllPalestrantesAsync(PageParms pageParms, bool includeEventos = false);
        Task<PalestranteViewModel> GetPalestranteByUserIdAsync(int userId, bool includeEventos = false);
    }
}