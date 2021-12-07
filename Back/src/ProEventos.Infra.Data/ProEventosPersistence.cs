using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Entities;
using ProEventos.Infra.Data.Context;

namespace ProEventos.Infra.Data
{
    public class ProEventosPersistence : IProEventosPersistence
    {
        private readonly ProEventosContext _context;

        public ProEventosPersistence(ProEventosContext context)
        {
            this._context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }
        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public void DeleteRange<T>(T[] entities) where T : class
        {
            _context.RemoveRange(entities);
        }
        public async Task<bool> SaveChangeAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes)
        {
            IQueryable<Evento> query = _context.eventos
                                        .Include(e => e.Lotes)
                                        .Include(e => e.RedesSociais);
            if (includePalestrantes)                                        
                query = query.Include(e => e.PalestrantesEventos).ThenInclude(pe => pe.Palestrante);


            query = query.OrderBy(e => e.Id).Where(e => e.Tema.ToLower().Contains(tema.ToLower())) ;
            return await query.ToArrayAsync();
        }

        public async Task<Evento> GetAllEventosByIdAsync(int eventoId, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.eventos
                                        .Include(e => e.Lotes)
                                        .Include(e => e.RedesSociais);
            if (includePalestrantes)                                        
                query = query.Include(e => e.PalestrantesEventos).ThenInclude(pe => pe.Palestrante);


            query = query.OrderBy(e => e.Id).Where(e => e.Id.Equals(eventoId)) ;
            return await query.FirstOrDefaultAsync();
        }
        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            throw new NotImplementedException();
        }

        public Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos= false)
        {
            throw new NotImplementedException();
        }


        public async Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.palestrantes
                            .Include(p => p.RedesSociais);
            if (includeEventos)                                        
                query = query.Include(p => p.PalestrantesEventos).ThenInclude(pe => pe.Evento);


            query = query.OrderBy(p => p.Id).Where(p => p.Nome.ToLower().Contains(nome.ToLower())) ;
            return await query.ToArrayAsync();
        }

        public async Task<Palestrante> GetAllPalestranteByIdAsync(int palestranteId, bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.palestrantes
                                        .Include(e => e.RedesSociais);
            if (includeEventos)                                        
                query = query.Include(p => p.PalestrantesEventos).ThenInclude(pe => pe.Evento);


            query = query.OrderBy(e => e.Id).Where(e => e.Id.Equals(palestranteId)) ;
            return await query.FirstOrDefaultAsync();
        }

    }
}