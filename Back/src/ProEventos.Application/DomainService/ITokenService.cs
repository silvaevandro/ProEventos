using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Application.ViewModels;

namespace ProEventos.Application.DomainService
{
    public interface ITokenService
    {
        Task<string> CreateToken(UserUpdateViewModel userUpdateViewModel);
    }
}