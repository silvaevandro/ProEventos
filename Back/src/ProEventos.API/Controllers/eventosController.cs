using Microsoft.AspNetCore.Mvc;
using ProEventos.Infra.Data.Context;

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
        return context.eventos;
    }

    [HttpGet("{id}")]
    public Evento? Get(int id)
    {
        return context.eventos.FirstOrDefault(e => e.EventoId == id);
    }
}