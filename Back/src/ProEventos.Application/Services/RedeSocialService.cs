using ProEventos.Application.ViewModels;
using ProEventos.Infra.Data;
using ProEventos.Domain.Entities;
using AutoMapper;
using ProEventos.Infra.Data.Repository;

namespace ProEventos.Application.Services
{
    public class RedeSocialService : IRedeSocialService
    {
        private readonly IRedeSocialRepository _redeSocialRepository;
        private readonly IMapper _mapper;

        public RedeSocialService(IRedeSocialRepository redeSocialRepository, IMapper mapper)
        {
            _redeSocialRepository = redeSocialRepository;
            _mapper = mapper;
        }
        public async Task AddRedeSocial(int id, RedeSocialViewModel model, bool isEvento)
        {
            try
            {
                var redeSocial = _mapper.Map<RedeSocial>(model);
                if (isEvento)
                {
                    redeSocial.EventoId = id;
                    redeSocial.PalestranteId = null;
                }
                else
                {
                    redeSocial.EventoId = null;
                    redeSocial.PalestranteId = id;
                }

                _redeSocialRepository.Add<RedeSocial>(redeSocial);
                await _redeSocialRepository.SaveChangeAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<RedeSocialViewModel[]> SaveByEvento(int eventoId, RedeSocialViewModel[] models)
        {
            try
            {
                var redesSociais = await _redeSocialRepository.GetAllByEventoIdAsync(eventoId);
                if (redesSociais == null)
                    return null;

                foreach (var model in models)
                {
                    if (model.Id == 0)
                    {
                        await AddRedeSocial(eventoId, model, true);
                    }
                    else
                    {
                        var redeSocial = redesSociais.FirstOrDefault(RedeSocial => RedeSocial.Id == model.Id);
                        model.EventoId = eventoId;
                        _mapper.Map(model, redeSocial);

                        _redeSocialRepository.Update<RedeSocial>(redeSocial!);
                        await _redeSocialRepository.SaveChangeAsync();
                    }
                }

                var RedeSocial_atz = await _redeSocialRepository.GetAllByEventoIdAsync(eventoId);
                return _mapper.Map<RedeSocialViewModel[]>(RedeSocial_atz);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialViewModel[]> SaveByPalestrante(int palestranteId, RedeSocialViewModel[] models)
        {
            try
            {
                var redesSociais = await _redeSocialRepository.GetAllByPalestranteIdAsync(palestranteId);
                if (redesSociais == null)
                    return null;

                foreach (var model in models)
                {
                    if (model.Id == 0)
                    {
                        await AddRedeSocial(palestranteId, model, false);
                    }
                    else
                    {
                        var redeSocial = redesSociais.FirstOrDefault(RedeSocial => RedeSocial.Id == model.Id);
                        model.PalestranteId = palestranteId;
                        _mapper.Map(model, redeSocial);

                        _redeSocialRepository.Update<RedeSocial>(redeSocial!);
                        await _redeSocialRepository.SaveChangeAsync();
                    }
                }

                var RedeSocial_atz = await _redeSocialRepository.GetAllByPalestranteIdAsync(palestranteId);
                return _mapper.Map<RedeSocialViewModel[]>(RedeSocial_atz);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteByEvento(int eventoId, int idRedeSocial)
        {
            try
            {
                var redeSocial = await _redeSocialRepository.GetRedeSocialEventoByIdsAsync(eventoId, idRedeSocial);
                if (redeSocial == null)
                    throw new Exception("Rede Social por Evento para delete não encontrado.");

                _redeSocialRepository.Delete<RedeSocial>(redeSocial);
                return await _redeSocialRepository.SaveChangeAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteByPalestrante(int palestranteId, int idRedeSocial)
        {
            try
            {
                var redeSocial = await _redeSocialRepository.GetRedeSocialPalestranteByIdsAsync(palestranteId, idRedeSocial);
                if (redeSocial == null)
                    throw new Exception("Rede Social por Palestrante para delete não encontrado.");

                _redeSocialRepository.Delete<RedeSocial>(redeSocial);
                return await _redeSocialRepository.SaveChangeAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialViewModel[]> GetAllByEventoIdAsync(int eventoId)
        {
            try
            {
                var redesSociais = await _redeSocialRepository.GetAllByEventoIdAsync(eventoId);
                return _mapper.Map<RedeSocialViewModel[]>(redesSociais);
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialViewModel[]> GetAllByPalestranteIdAsync(int palestranteId)
        {
            try
            {
                var redesSociais = await _redeSocialRepository.GetAllByPalestranteIdAsync(palestranteId);
                return _mapper.Map<RedeSocialViewModel[]>(redesSociais);
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }        

        public async Task<RedeSocialViewModel> GetRedeSocialEventoByIdsAsync(int eventoId, int redeSocialId)
        {
            try
            {
                var redeSocial = await _redeSocialRepository.GetRedeSocialEventoByIdsAsync(eventoId, redeSocialId);
                if (redeSocial == null)
                    return null;
                return _mapper.Map<RedeSocialViewModel>(redeSocial);
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialViewModel> GetRedeSocialPalestranteByIdAsync(int palestranteId, int redeSocialId)
        {
            try
            {
                var redeSocial = await _redeSocialRepository.GetRedeSocialPalestranteByIdsAsync(palestranteId, redeSocialId);
                if (redeSocial == null)
                    return null;
                return _mapper.Map<RedeSocialViewModel>(redeSocial);
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}