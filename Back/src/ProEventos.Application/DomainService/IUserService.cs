using Microsoft.AspNetCore.Identity;
using ProEventos.Application.ViewModels;

namespace ProEventos.Application.DomainService
{
    public interface IUserService
    {
        Task<bool> UserExists(string username);
        Task<UserUpdateViewModel> GetUserByUserNameAsync(string username);
        Task<SignInResult> CheckUserPasswordAsync(UserUpdateViewModel userUpdateViewModel, string password);
        Task<UserUpdateViewModel> CreateAccontAsync(UserViewModel userViewModel);
        Task<UserUpdateViewModel> UpdateUser(UserUpdateViewModel userUpdateViewModel);

    }
}