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
                return null;    
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
                if (evento == null) 
                    return null;

                model.Id = evento.Id;

                geralRepository.Update(model);
                if (await geralRepository.SaveChangeAsync())
                {
                    return await eventoRepository.GetAllEventosByIdAsync(model.Id, false);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteEvento(int eventoId)
        {
            try
            {
                var evento = await eventoRepository.GetAllEventosByIdAsync(eventoId, false);
                if (evento == null) 
                    throw new Exception("Evento para delete não encontrado.");

                geralRepository.Update(evento);
                return await geralRepository.SaveChangeAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            try
            {
                 return await eventoRepository.GetAllEventosAsync(includePalestrantes);
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento> GetAllEventosByIdAsync(int eventoId, bool includePalestrantes = false)
        {
            try
            {
                 return await eventoRepository.GetAllEventosByIdAsync(eventoId, includePalestrantes);
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            try
            {
                 var eventos = await eventoRepository.GetAllEventosByTemaAsync(tema, includePalestrantes);
                 if (eventos == null)
                    return null;
                 return eventos;   
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}