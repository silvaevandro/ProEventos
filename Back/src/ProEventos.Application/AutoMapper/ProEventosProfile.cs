using AutoMapper;
using ProEventos.Application.ViewModels;
using ProEventos.Domain.Entities;
using ProEventos.Domain.Identity;

namespace ProEventos.Application.AutoMapper
{
    public class ProEventosProfile : Profile
    {
        public ProEventosProfile()
        {
            CreateMap<Evento, EventoViewModel>().ReverseMap();

            CreateMap<Lote, LoteViewModel>().ReverseMap();

            CreateMap<Palestrante, PalestranteViewModel>().ReverseMap();
            CreateMap<Palestrante, PalestranteAddViewModel>().ReverseMap();
            CreateMap<Palestrante, PalestranteUpdateViewModel>().ReverseMap();

            CreateMap<RedeSocial, RedeSocialViewModel>().ReverseMap();

            CreateMap<User, UserViewModel>().ReverseMap();
            CreateMap<User, UserLoginViewModel>().ReverseMap();
            CreateMap<User, UserUpdateViewModel>().ReverseMap();

        }
    }
}