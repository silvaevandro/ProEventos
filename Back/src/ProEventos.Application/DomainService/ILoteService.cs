using ProEventos.Application.ViewModels;

namespace ProEventos.Application.DomainService
{
    public interface ILoteService
    {
        Task<LoteViewModel[]> SaveLotes(int eventoId, LoteViewModel[] model);
        Task<bool> DeleteLote(int eventoId, int loteId);
        Task<LoteViewModel[]> GetLotesByEventoIdAsync(int eventoId);
        Task<LoteViewModel> GetLoteByIdsAsync(int eventoId, int loteId);
    }
}