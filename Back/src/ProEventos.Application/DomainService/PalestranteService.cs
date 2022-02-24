using ProEventos.Application.ViewModels;
using ProEventos.Infra.Data;
using ProEventos.Domain.Entities;
using AutoMapper;
using ProEventos.Infra.Data.Models;

namespace ProEventos.Application.DomainService
{
    public class PalestranteService : IPalestranteService
    {
        private readonly IPalestranteRepository _palestranteRepository;
        private readonly IMapper _mapper;

        public PalestranteService(IGeralRepository geralRepository, IPalestranteRepository palestranteRepository, IMapper mapper)
        {
            _palestranteRepository = palestranteRepository;
            _mapper = mapper;
        }
        public async Task<PalestranteViewModel> AddPalestrantes(int userId, PalestranteAddViewModel model)
        {
            try
            {
                var Palestrante = _mapper.Map<Palestrante>(model);
                Palestrante.UserId = userId;
                _palestranteRepository.Add<Palestrante>(Palestrante);
                if (await _palestranteRepository.SaveChangeAsync())
                {
                    var palestranteRetorno = await _palestranteRepository.GetPalestranteByUserIdAsync(userId, false);
                    return _mapper.Map<PalestranteViewModel>(palestranteRetorno);
                }
                return null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<PalestranteViewModel> UpdatePalestrante(int userId, PalestranteUpdateViewModel model)
        {
            try
            {
                var palestrante = await _palestranteRepository.GetPalestranteByUserIdAsync(userId, false);
                if (palestrante == null)
                    return null;

                model.Id = palestrante.Id;
                model.UserId = userId;

                var palestranteRet = _mapper.Map<Palestrante>(model);
                _palestranteRepository.Update<Palestrante>(palestranteRet);

                if (await _palestranteRepository.SaveChangeAsync())
                {
                    var palestranteAtz = await _palestranteRepository.GetPalestranteByUserIdAsync(userId, false);
                    return _mapper.Map<PalestranteViewModel>(palestranteAtz);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(message: ex.InnerException?.Message);
            }
        }

        public async Task<PageList<PalestranteViewModel>> GetAllPalestrantesAsync(PageParms pageParms, bool includeEventos = false)
        {
            try
            {
                var palestrantes = await _palestranteRepository.GetAllPalestrantesAsync(pageParms, includeEventos);
                var result = _mapper.Map<PageList<PalestranteViewModel>>(palestrantes);
                result.CurrentPage = palestrantes.CurrentPage;
                result.TotalPages = palestrantes.TotalPages;
                result.PageSize = palestrantes.PageSize;
                result.TotalCount = palestrantes.TotalCount;
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PalestranteViewModel> GetPalestranteByUserIdAsync(int userId, bool includeEventos = false)
        {
            try
            {
                var palestrante = await _palestranteRepository.GetPalestranteByUserIdAsync(userId, includeEventos);
                return _mapper.Map<PalestranteViewModel>(palestrante);
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}