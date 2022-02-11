using ProEventos.Application.ViewModels;
using ProEventos.Infra.Data;
using ProEventos.Domain.Entities;
using AutoMapper;

namespace ProEventos.Application.DomainService
{
    public class EventoService : IEventoService
    {
        private readonly IGeralRepository geralRepository;
        private readonly IEventoRepository eventoRepository;
        private readonly IMapper mapper;

        public EventoService(IGeralRepository geralRepository, IEventoRepository eventoRepository, IMapper mapper)
        {
            this.eventoRepository = eventoRepository;
            this.geralRepository = geralRepository;
            this.mapper = mapper;
        }
        public async Task<EventoViewModel> AddEvento(EventoViewModel model)
        {
            try
            {
                var evento = mapper.Map<Evento>(model);
                geralRepository.Add<Evento>(evento);
                if (await geralRepository.SaveChangeAsync())
                {
                    var evento_ins = await eventoRepository.GetAllEventosByIdAsync(evento.Id, false);
                    return mapper.Map<EventoViewModel>(evento_ins);
                }
                return null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<EventoViewModel> UpdateEvento(int eventoId, EventoViewModel model)
        {
            try
            {
                var evento = await eventoRepository.GetAllEventosByIdAsync(eventoId, false);
                if (evento == null)
                    return null;

                model.Id = evento.Id;

                var evento_upd = mapper.Map<Evento>(model);
                geralRepository.Update<Evento>(evento_upd);

                if (await geralRepository.SaveChangeAsync())
                {
                    var evento_atz = await eventoRepository.GetAllEventosByIdAsync(evento_upd.Id, false);
                    return mapper.Map<EventoViewModel>(evento_atz);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);


            }
        }

        public async Task<bool> DeleteEvento(int eventoId)
        {
            try
            {
                var evento = await eventoRepository.GetAllEventosByIdAsync(eventoId, false);
                if (evento == null)
                    throw new Exception("Evento para delete não encontrado.");

                geralRepository.Delete(evento);
                return await geralRepository.SaveChangeAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoViewModel[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            try
            {
                var eventos = await eventoRepository.GetAllEventosAsync(includePalestrantes);
                return mapper.Map<EventoViewModel[]>(eventos);
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoViewModel> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
        {
            try
            {
                var evento = await eventoRepository.GetAllEventosByIdAsync(eventoId, includePalestrantes);
                return mapper.Map<EventoViewModel>(evento);
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoViewModel[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await eventoRepository.GetAllEventosByTemaAsync(tema, includePalestrantes);
                if (eventos == null)
                    return null;
                return mapper.Map<EventoViewModel[]>(eventos);
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}