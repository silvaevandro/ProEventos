using ProEventos.Domain.Entities;
using ProEventos.Infra.Data.Models;

namespace ProEventos.Infra.Data
{
    public interface IPalestranteRepository: IGeralRepository
    {
        Task<PageList<Palestrante>> GetAllPalestrantesAsync(PageParms pageParms0, bool includeEventos);
        Task<Palestrante> GetPalestranteByUserIdAsync(int userId, bool includeEventos);
    }
}