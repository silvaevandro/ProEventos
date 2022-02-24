using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.API.Helpers;
using ProEventos.Application.DomainService;
using ProEventos.Application.ViewModels;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IUtil _util;
        private readonly string _destino = "perfil";

        public UserController(IUserService userService,
                              ITokenService tokenService,
                              IUtil util)
        {
            _userService = userService;
            _tokenService = tokenService;
            _util = util;
        }

        [HttpGet("GetUser")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var userName = User.GetUserName();
                if (userName == null)
                    return BadRequest("Falha ao obter o usuário");

                var user = await _userService.GetUserByUserNameAsync(userName);
                return Ok(user);
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar Usuário: {e.Message}");
            }
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserViewModel userViewModel)
        {
            try
            {
                if (await _userService.UserExists(userViewModel.UserName))
                    return BadRequest("Usuário já existe");

                var user = await _userService.CreateAccontAsync(userViewModel);
                if (user != null)
                    return Ok(new
                    {
                        UserName = user.UserName,
                        PrimeiroNome = user.PrimeiroNome,
                        UltimoNome = user.UltimoNome,
                        token = _tokenService.CreateToken(user).Result
                    });

                return BadRequest("Usuário não cadastrado");
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar Usuário: {e.Message}");
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginViewModel userLogin)
        {
            try
            {
                var user = await _userService.GetUserByUserNameAsync(userLogin.UserName);
                if (user == null)
                    return Unauthorized("Usuário ou senha inválido");

                var result = await _userService.CheckUserPasswordAsync(user, userLogin.Password);
                if (!result.Succeeded)
                    return Unauthorized("Usuário ou senha inválido");


                return Ok(new
                {
                    UserName = user.UserName,
                    PrimeiroNome = user.PrimeiroNome,
                    UltimoNome = user.UltimoNome,
                    token = _tokenService.CreateToken(user).Result
                }
                );
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar Usuário: {e.Message}");
            }
        }


        [HttpPut("Update")]
        public async Task<IActionResult> Update(UserUpdateViewModel userViewModel)
        {
            try
            {
                if (userViewModel.UserName != User.GetUserName())
                    return Unauthorized("Usuário inválido");

                var user = await _userService.GetUserByUserNameAsync(User.GetUserName()!);
                if (user == null)
                    return Unauthorized("Usuário inválido");

                var userUpdate = await _userService.UpdateUser(userViewModel);
                if (userUpdate == null)
                    return NoContent();
                return Ok(new
                {
                    UserName = userUpdate.UserName,
                    PrimeiroNome = userUpdate.PrimeiroNome,
                    UltimoNome = userUpdate.UltimoNome,
                    token = _tokenService.CreateToken(userUpdate).Result
                }
                );
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar atualizar Usuário: {e.Message}");
            }
        }

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage()
        {
            try
            {
                if (Request.Form.Files.Count == 0)
                    return NoContent();

                var user = await _userService.GetUserByUserNameAsync(User.GetUserName()!);
                if (user == null)
                    return NoContent();

                var file = Request.Form.Files[0];
                if (file.Length > 0)
                {
                    _util.DeleteImage(user.ImagemURL, _destino);
                    user.ImagemURL = await _util.SaveImage(file, _destino);
                }
                var userRetorno = await _userService.UpdateUser(user);
                if (userRetorno == null)
                    return BadRequest("Erro ao realizar o Upload de Foto do Usuário.");
                return Ok(userRetorno);
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao realizar o Upload de Foto do Usuário {e.Message}");
            }
        }        
    }
}