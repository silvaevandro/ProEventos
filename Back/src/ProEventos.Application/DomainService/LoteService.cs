using ProEventos.Application.ViewModels;
using ProEventos.Infra.Data;
using ProEventos.Domain.Entities;
using AutoMapper;

namespace ProEventos.Application.DomainService
{
    public class LoteService : ILoteService
    {
        private readonly IGeralRepository geralRepository;
        private readonly ILoteRepository loteRepository;
        private readonly IMapper mapper;

        public LoteService(IGeralRepository geralRepository, ILoteRepository loteRepository, IMapper mapper)
        {
            this.loteRepository = loteRepository;
            this.geralRepository = geralRepository;
            this.mapper = mapper;
        }
        public async Task AddLote(int eventoId, LoteViewModel model)
        {
            try
            {
                var lote = mapper.Map<Lote>(model);
                lote.EventoId = eventoId;
                geralRepository.Add<Lote>(lote);
                await geralRepository.SaveChangeAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<LoteViewModel[]> SaveLotes(int eventoId, LoteViewModel[] models)
        {
            try
            {
                var lotes = await loteRepository.GetLotesByEventoIdAsync(eventoId);
                if (lotes == null)
                    return null;

                foreach (var model in models)
                {
                    if (model.Id == 0)
                    {
                        await AddLote(eventoId, model);
                    }
                    else
                    {
                        var lote = lotes.FirstOrDefault(lote => lote.Id == model.Id);
                        model.EventoId = eventoId;
                        mapper.Map(model, lote);

                        geralRepository.Update<Lote>(lote);
                        await geralRepository.SaveChangeAsync();
                    }
                }

                var lote_atz = await loteRepository.GetLotesByEventoIdAsync(eventoId);
                return mapper.Map<LoteViewModel[]>(lote_atz);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteLote(int eventoId, int loteId)
        {
            try
            {
                var lote = await loteRepository.GetLoteByIdsAsync(eventoId, loteId);
                if (lote == null)
                    throw new Exception("Lote para delete não encontrado.");

                geralRepository.Delete<Lote>(lote);
                return await geralRepository.SaveChangeAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteViewModel[]> GetLotesByEventoIdAsync(int eventoId)
        {
            try
            {
                var lotes = await loteRepository.GetLotesByEventoIdAsync(eventoId);
                return mapper.Map<LoteViewModel[]>(lotes);
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteViewModel> GetLoteByIdsAsync(int eventoId, int loteId)
        {
            try
            {
                var lote = await loteRepository.GetLoteByIdsAsync(eventoId, loteId);
                if (lote == null)
                    return null;
                return mapper.Map<LoteViewModel>(lote);
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}