using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Infra.Data.Context;
using static ProEventos.API.Evento;

namespace ProEventos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class eventosController : ControllerBase
{
    private readonly DataContext context;

    public eventosController(DataContext context)
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