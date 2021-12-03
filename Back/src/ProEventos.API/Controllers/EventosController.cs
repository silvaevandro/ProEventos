using Microsoft.AspNetCore.Mvc;
using static ProEventos.API.Evento;

namespace ProEventos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventosController : ControllerBase
{
    public EventosController()
    {
    }

    private IEnumerable<Evento> _eventos = new Evento[] {
        new Evento(){
            EventoId = 1,
            Tema = "Angular 11 e .NET 6.0",
            Local = "Guariba",
            Lote = "1º e 2º Lote",
            QtdPessoas = 250,
            DataEvento = DateTime.Now.AddDays(2).ToString()
        },
        new Evento(){
            EventoId = 2,
            Tema = "Evento 2 Angular 11 e .NET 6.0",
            Local = "Guariba",
            Lote = "1º e 2º Lote",
            QtdPessoas = 250,
            DataEvento = DateTime.Now.AddDays(2).ToString()
        },
        new Evento(){
            EventoId = 3,
            Tema = "Evento 3 Angular 11 e .NET 6.0",
            Local = "Guariba",
            Lote = "1º e 2º Lote",
            QtdPessoas = 250,
            DataEvento = DateTime.Now.AddDays(2).ToString()
            },
    };

    [HttpGet]
    public IEnumerable<Evento> Get()
    {
        return _eventos;
    }

    [HttpGet("{id}")]
    public IEnumerable<Evento> Get(int id)
    {
        return _eventos.Where(e => e.EventoId == id);
    }
}
