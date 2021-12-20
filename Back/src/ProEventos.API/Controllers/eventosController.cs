using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.DomainService;
using ProEventos.Application.ViewModels;

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
                return NoContent();
            return Ok(eventos);
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar Eventos: {e.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var evento = await eventoService.GetAllEventosByIdAsync(id, true);
            if (evento == null)
                return NoContent();
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
            var eventos = await eventoService.GetAllEventosByTemaAsync(tema, true);
            if (eventos == null)
                return NoContent();

            return Ok(eventos);
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar Eventos {e.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post(EventoViewModel model)
    {
        try
        {
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
    public async Task<IActionResult> Put(int id, EventoViewModel model)
    {
        try
        {
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
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            return await eventoService.DeleteEvento(id) ? Ok("Deletado") : BadRequest("Erro ao deletar o evento.");
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao deletar o Evento {e.Message}");
        }
    }
};