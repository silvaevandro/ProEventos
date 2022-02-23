using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Entities;
using ProEventos.Infra.Data.Context;
using ProEventos.Infra.Data.Models;

namespace ProEventos.Infra.Data.Repository
{
    public class PalestranteRepository : GeralRepository, IPalestranteRepository
    {
        private readonly ProEventosContext _context;

        public PalestranteRepository(ProEventosContext context) : base(context)
        {
            this._context = context;
            this._context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public async Task<PageList<Palestrante>> GetAllPalestrantesAsync(PageParms pageParms, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.palestrantes
                    .Include(p => p.RedesSociais)
                    .Include(p => p.User);
            if (includeEventos){
                query = query.Include(p => p.PalestrantesEventos!).ThenInclude(pe => pe.Evento);
            }
            query = query.AsNoTracking()
                         .Where(p => (p.MiniCurriculo!.ToLower().Contains(pageParms.Term.ToLower())
                                  || p.User.PrimeiroNome!.ToLower().Contains(pageParms.Term.ToLower())
                                  || p.User.UltimoNome!.ToLower().Contains(pageParms.Term.ToLower()))
                                  && p.User.Funcao == Domain.Enum.Funcao.Palestrante
                                ) 
                         .OrderBy(p => p.Id);
            return await PageList<Palestrante>.CreateAsync(query, pageParms.PageNumber, pageParms.pageSize);
        }

        // public async Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos = false)
        // {
        //     IQueryable<Palestrante> query = _context.palestrantes
        //                     .Include(p => p.RedesSociais);
        //     if (includeEventos)
        //         query = query.Include(p => p.PalestrantesEventos).ThenInclude(pe => pe.Evento);


        //     query = query.OrderBy(p => p.Id).Where(p => p.User.PrimeiroNome.ToLower().Contains(nome.ToLower()));
        //     return await query.ToArrayAsync();
        // }

        public async Task<Palestrante> GetPalestranteByUserIdAsync(int userId, bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.palestrantes
                                        .Include(p => p.User)
                                        .Include(p => p.RedesSociais);
            if (includeEventos)
                query = query.Include(p => p.PalestrantesEventos).ThenInclude(pe => pe.Evento);


            query = query.OrderBy(e => e.Id).Where(e => e.Id.Equals(userId));
            return await query.FirstOrDefaultAsync();
        }

    }
}