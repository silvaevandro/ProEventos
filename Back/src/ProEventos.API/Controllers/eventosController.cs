using Microsoft.AspNetCore.Mvc;
using ProEventos.Application;
using ProEventos.Domain.Entities;
using ProEventos.Infra.Data.Context;

namespace ProEventos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class eventosController : ControllerBase
{
    public IEventoService eventoService { get; set; }

    public eventosController(IEventoService eventoService)
    {
        this.eventoService = eventoService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var eventos = await eventoService.GetAllEventosAsync(true);
            if (eventos == null) 
                return NotFound("Nenhum evento encontrado.");
            return Ok(eventos);
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar Eventos {e.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var evento = await eventoService.GetAllEventosByIdAsync(id, true);
            if (evento == null) 
                return NotFound("Evento por ID encontrado.");
            return Ok(evento);
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar Eventos {e.Message}");
        }        
    }

    [HttpGet("{tema}/tema")]
    public async Task<IActionResult> Get(string tema)
    {
        try
        {
            var evento = await eventoService.GetAllEventosByTemaAsync(tema, true);
            if (evento == null) 
                return NotFound("Evento por Tema encontrado.");
            return Ok(evento);
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar Eventos {e.Message}");
        }        
    }

    [HttpPost]
    public async Task<IActionResult> Post(Evento model){
        try{
            var evento = await eventoService.AddEvento(model);
            if (evento == null) 
                return BadRequest("Erro ao tentar adicionar evento.");
            return Ok(evento);
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar adicionar o Evento {e.Message}");
        }        
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Evento model){
        try{
            var evento = await eventoService.UpdateEvento(id, model);
            if (evento == null) 
                return BadRequest("Erro ao tentar adicionar evento.");
            return Ok(evento);
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar atualizar o Evento {e.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id){
        try{
            var evento = await eventoService.UpdateEvento(id, model);
            if (evento == null) 
                return BadRequest("Erro ao tentar adicionar evento.");
            return Ok(evento);
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar Eventos {e.Message}");
        }
    }    
}