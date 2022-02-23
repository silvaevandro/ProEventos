using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain.Entities;
using ProEventos.Infra.Data.Context;

namespace ProEventos.Infra.Data.Repository
{
    public interface IRedeSocialRepository : IGeralRepository
    {
        Task<RedeSocial> GetRedeSocialEventoByIdsAsync(int eventoId, int id);
        Task<RedeSocial> GetRedeSocialPalestranteByIdsAsync(int palestranteId, int id);
        Task<RedeSocial[]> GetAllByEventoIdAsync(int eventoId);
        Task<RedeSocial[]> GetAllByPalestranteIdAsync(int palestranteId);
    }
}