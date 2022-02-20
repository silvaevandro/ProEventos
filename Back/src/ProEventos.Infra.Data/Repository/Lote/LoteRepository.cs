using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Entities;
using ProEventos.Infra.Data.Context;

namespace ProEventos.Infra.Data.Repository
{
    public class LoteRepository : ILoteRepository
    {
        private readonly ProEventosContext _context;

        public LoteRepository(ProEventosContext context)
        {
            this._context = context;
            this._context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public async Task<Lote> GetLoteByIdsAsync(int eventoId, int loteId)
        {
            IQueryable<Lote> query = _context.lotes;
            query = query.AsNoTracking()
                         .Where(lote => lote.EventoId == eventoId && lote.Id == loteId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Lote[]> GetLotesByEventoIdAsync(int eventoId)
        {
            IQueryable<Lote> query = _context.lotes;
            query = query.AsNoTracking()
                         .Where(lote => lote.EventoId == eventoId);

            return await query.ToArrayAsync();
        }
    }
}