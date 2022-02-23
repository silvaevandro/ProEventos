using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Application.ViewModels;

namespace ProEventos.Application.DomainService
{
    public interface IRedeSocialService
    {
        Task<RedeSocialViewModel[]> SaveByEvento(int eventoId, RedeSocialViewModel[] models);
        Task<bool> DeleteByEvento(int eventoId, int redeSocialId);
        Task<RedeSocialViewModel[]> SaveByPalestrante(int palestranteId, RedeSocialViewModel[] models);
        Task<bool> DeleteByPalestrante(int palestranteId, int redeSocialId);

        Task<RedeSocialViewModel[]> GetAllByEventoIdAsync(int eventoId);
        Task<RedeSocialViewModel[]> GetAllByPalestranteIdAsync(int palestranteId);

        Task<RedeSocialViewModel> GetRedeSocialEventoByIdsAsync(int eventoId, int redeSocialId);
        Task<RedeSocialViewModel> GetRedeSocialPalestranteByIdAsync(int palestranteId, int redeSocialId);
    }
}