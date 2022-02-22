using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Entities;
using ProEventos.Infra.Data.Context;
using ProEventos.Infra.Data.Models;

namespace ProEventos.Infra.Data.Repository
{
    public class EventoRepository : IEventoRepository
    {
        private readonly ProEventosContext _context;

        public EventoRepository(ProEventosContext context)
        {
            this._context = context;
            this._context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        // public async Task<PageList<Evento>> GetAllEventosByTemaAsync(int userId, PageParms pageParms, string tema, bool includePalestrantes)
        // {
        //     IQueryable<Evento> query = _context.eventos
        //                                 .Include(e => e.Lotes)
        //                                 .Include(e => e.RedesSociais);
        //     if (includePalestrantes)
        //         query = query.Include(e => e.PalestrantesEventos!).ThenInclude(pe => pe.Palestrante);


        //     query = query.OrderBy(e => e.Id)
        //                  .Where(e => e.UserId == userId && e.Tema!.ToLower().Contains(tema.ToLower()));
        //     return await query.ToArrayAsync();
        // }

        public async Task<Evento> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.eventos
                                        .Include(e => e.Lotes)
                                        .Include(e => e.RedesSociais);
            if (includePalestrantes)
                query = query.Include(e => e.PalestrantesEventos).ThenInclude(pe => pe.Palestrante);


            query = query.OrderBy(e => e.Id).Where(e => e.UserId == userId && e.Id.Equals(eventoId));
            return await query.FirstOrDefaultAsync();        
        }

        public async Task<PageList<Evento>> GetAllEventosAsync(int userId, PageParms pageParms, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.eventos
                                        .Include(e => e.Lotes)
                                        .Include(e => e.RedesSociais);
            if (includePalestrantes)
                query = query.Include(e => e.PalestrantesEventos).ThenInclude(pe => pe.Palestrante);


            query = query.AsNoTracking()
                         .Where(e => e.UserId == userId && (e.Tema!.ToLower().Contains(pageParms.Term.ToLower()) || e.Local!.ToLower().Contains(pageParms.Term.ToLower())))
                         .OrderBy(e => e.Id);
            return await PageList<Evento>.CreateAsync(query, pageParms.PageNumber, pageParms.pageSize);
        }
    }
}