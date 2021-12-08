using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain.Entities;
using ProEventos.Infra.Data;

namespace ProEventos.Application.DomainService
{
    public class EventoService : IEventoService
    {
        private readonly IGeralRepository geralRepository;
        private readonly IEventosRepository eventoRepository;

        public EventoService(IGeralRepository geralRepository, IEventosRepository eventoRepository)
        {
            this.eventoRepository = eventoRepository;
            this.geralRepository = geralRepository;
        }
        public async Task<Evento> AddEvento(Evento model)
        {
            try
            {
                geralRepository.Add<Evento>(model);
                if (await geralRepository.SaveChangeAsync())
                    return await eventoRepository.GetAllEventosByIdAsync(model.Id, false);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<Evento> UpdateEvento(int eventoId, Evento model)
        {
            try
            {
                var evento = await eventoRepository.GetAllEventosByIdAsync(eventoId, false);
                if (evento == null) return null;

                model.Id = evento.Id;

                geralRepository.Update<model>;
                if (geralPersist.SaveChangeAsync())
                {
                    return await eventoRepository.GetAllEventosByIdAsync(model.Id, false);
                }






            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public Task<bool> DeleteEvento(int eventoId)
        {
            throw new NotImplementedException();
        }

        public Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            throw new NotImplementedException();
        }

        public Task<Evento> GetAllEventosByIdAsync(int eventoId, bool includePalestrantes = false)
        {
            throw new NotImplementedException();
        }

        public Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            throw new NotImplementedException();
        }

    }
}