using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProEventos.Application.ViewModels;
using ProEventos.Domain.Identity;

namespace ProEventos.Application.DomainService
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _iConfiguration;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration iConfiguration,
                            UserManager<User> userManager,
                            IMapper mapper)
        {
            this._mapper = mapper;
            this._userManager = userManager;
            this._iConfiguration = iConfiguration;
            this._key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(iConfiguration["TokenKey"]));
        }
        public async Task<string> CreateToken(UserUpdateViewModel userUpdateViewModel)
        {
            var user = _mapper.Map<User>(userUpdateViewModel);
            var claims = new List<Claim>{
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
            };

            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(token);

        }
    }
}