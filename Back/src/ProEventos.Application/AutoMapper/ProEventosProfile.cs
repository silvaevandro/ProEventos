using AutoMapper;
using ProEventos.Application.ViewModels;
using ProEventos.Domain.Entities;

namespace ProEventos.Application.AutoMapper
{
    public class ProEventosProfile : Profile
    {
        public ProEventosProfile()
        {
            CreateMap<Evento, EventoViewModel>().ReverseMap();
            CreateMap<Lote, LoteViewModel>().ReverseMap();
            CreateMap<Palestrante, PalestranteViewModel>().ReverseMap();
            CreateMap<RedeSocial, RedeSocialViewModel>().ReverseMap();
        }
    }
}