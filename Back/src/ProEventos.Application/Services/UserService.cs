using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProEventos.Application.ViewModels;
using ProEventos.Domain.Identity;
using ProEventos.Infra.Data;

namespace ProEventos.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UserService(UserManager<User> userManager,
                           SignInManager<User> signInManager,
                           IMapper mapper,
                           IUserRepository userRepository)
        {
            this._mapper = mapper;
            this._userRepository = userRepository;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }
        public async Task<SignInResult> CheckUserPasswordAsync(UserUpdateViewModel userUpdateViewModel, string password)
        {
            try
            {
                var user = await _userManager.Users
                                             .SingleOrDefaultAsync(user => user.UserName == userUpdateViewModel.UserName.ToLower());
                return await _signInManager.CheckPasswordSignInAsync(user, password, false);
            }
            catch (System.Exception ex)
            {

                throw new Exception($"Erro ao tentar verificar password. Erro: {ex.Message}");
            }
        }

        public async Task<UserUpdateViewModel> CreateAccontAsync(UserViewModel userViewModel)
        {
            try
            {
                var user = _mapper.Map<User>(userViewModel);
                var result = await _userManager.CreateAsync(user, userViewModel.Password);
                if (result.Succeeded)
                    return _mapper.Map<UserUpdateViewModel>(user);
                return null;
            }
            catch (System.Exception ex)
            {

                throw new Exception($"Erro ao tentar criar o usuário. Erro: {ex.Message}");
            }
        }

        public async Task<UserUpdateViewModel> GetUserByUserNameAsync(string username)
        {
            try
            {
                var user = await _userRepository.GetUsersByNameAsync(username);
                if (user == null)
                    return null;
                return _mapper.Map<UserUpdateViewModel>(user);
            }
            catch (System.Exception ex)
            {

                throw new Exception($"Erro ao tentar obter o usuário por UserName. Erro: {ex.Message}");
            }
        }

        public async Task<UserUpdateViewModel> UpdateUser(UserUpdateViewModel userUpdateViewModel)
        {
            try
            {
                var user = await _userRepository.GetUsersByNameAsync(userUpdateViewModel.UserName!);
                if (user == null)
                    return null;
                userUpdateViewModel.Id = user.Id;
                _mapper.Map(userUpdateViewModel, user);
                if (userUpdateViewModel.Password != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    await _userManager.ResetPasswordAsync(user, token, userUpdateViewModel.Password);
                }
                _userRepository.Update<User>(user);
                if (await _userRepository.SaveChangeAsync())
                {
                    var userRetorno = await _userRepository.GetUsersByNameAsync(user.UserName);
                    return _mapper.Map<UserUpdateViewModel>(userRetorno);
                }
                return null;
            }
            catch (System.Exception ex)
            {

                throw new Exception($"Erro ao tentar atualizar o usuário. Erro: {ex.Message}");
            }
        }

        public async Task<bool> UserExists(string username)
        {
            try
            {
                return await _userManager.Users.AnyAsync(user => user.UserName == username.ToLower());
            }
            catch (System.Exception ex)
            {

                throw new Exception($"Erro ao tentar verificar se o usuário existe. Erro: {ex.Message}");
            }
        }
    }
}