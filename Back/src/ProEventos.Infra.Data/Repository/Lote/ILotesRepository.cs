using ProEventos.Domain.Entities;

namespace ProEventos.Infra.Data
{
    public interface ILoteRepository
    {
        /// <summary>
        /// Métodd get que retornará uma lista de lotes por eventoId
        /// </summary>
        /// <param name="eventoId">ID do Evento</param>
        /// <returns>Lista de Lotes</returns>
        Task<Lote[]> GetLotesByEventoIdAsync(int eventoId);

        /// <summary>
        /// Método get que retornará apenas 1 lote
        /// </summary>
        /// <param name="eventoId">ID do evento</param>
        /// <param name="loteId">ID do Lote</param>
        /// <returns>Apenas 1 Lote</returns>
        Task<Lote> GetLoteByIdsAsync(int eventoId, int loteId);
    }
}