using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Infra.Data.Context;
using static ProEventos.API.Evento;

namespace ProEventos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventosController : ControllerBase
{
    private readonly DataContext context;

    public EventosController(DataContext context)
    {
        this.context = context;
    }

    [HttpGet]
    public IEnumerable<Evento> Get()
    {
        return context.Eventos;
    }

    [HttpGet("{id}")]
    public Evento? Get(int id)
    {
        return context.Eventos.FirstOrDefault(e => e.EventoId == id);
    }
}



// private IEnumerable<Evento> _eventos = new Evento[] {
//         new Evento(){
//             EventoId = 1,
//             Tema = "Angular 11 e .NET 6.0",
//             Local = "Guariba",
//             Lote = "1º e 2º Lote",
//             QtdPessoas = 250,
//             DataEvento = DateTime.Now.AddDays(2).ToString()
//         },
//         new Evento(){
//             EventoId = 2,
//             Tema = "Evento 2 Angular 11 e .NET 6.0",
//             Local = "Guariba",
//             Lote = "1º e 2º Lote",
//             QtdPessoas = 250,
//             DataEvento = DateTime.Now.AddDays(2).ToString()
//         },
//         new Evento(){
//             EventoId = 3,
//             Tema = "Evento 3 Angular 11 e .NET 6.0",
//             Local = "Guariba",
//             Lote = "1º e 2º Lote",
//             QtdPessoas = 250,
//             DataEvento = DateTime.Now.AddDays(2).ToString()
//             },
//     };
