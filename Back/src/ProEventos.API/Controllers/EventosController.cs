using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.API.Helpers;
using ProEventos.Application.Services;
using ProEventos.Application.ViewModels;
using ProEventos.Infra.Data.Models;

namespace ProEventos.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EventosController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IUtil _util;
    private readonly IEventoService _eventoService;

    private readonly string _destino = "images";

    public EventosController(IEventoService eventoService,
                             IUserService userService,
                             IUtil util)
    {
        _eventoService = eventoService;
        _userService = userService;
        _util = util;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PageParms pageParams)
    {
        try
        {
            var eventos = await _eventoService.GetAllEventosAsync(User.GetUserId(), pageParams, true);
            if (eventos == null)
                return NoContent();

            Response.AddPagination(eventos.CurrentPage, eventos.PageSize, eventos.TotalCount, eventos.TotalPages);
            return Ok(eventos);
        }
        catch (System.Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar Eventos: {e.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(), id, true);
            if (evento == null)
                return NoContent();
            return Ok(evento);
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
            var evento = await _eventoService.AddEvento(User.GetUserId(), model);
            if (evento == null)
                return BadRequest("Erro ao tentar adicionar evento.");
            return Ok(evento);
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar adicionar o Evento {e.Message}");
        }
    }

    [HttpPost("upload-image/{eventoId}")]
    public async Task<IActionResult> UploadImage(int eventoId)
    {
        try
        {
            if (Request.Form.Files.Count == 0)
                return NoContent();

            var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(), eventoId, false);
            if (evento == null)
                return NoContent();

            var file = Request.Form.Files[0];
            if (file.Length > 0)
            {
                _util.DeleteImage(evento.ImagemURL, _destino);
                evento.ImagemURL = await _util.SaveImage(file, _destino);
            }
            var eventoRetorno = await _eventoService.UpdateEvento(User.GetUserId(), eventoId, evento);
            if (eventoRetorno == null)
                return BadRequest("Erro ao realizar o Upload da Imagem.");
            return Ok(eventoRetorno);
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao realizar o Upload da Imagem {e.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, EventoViewModel model)
    {
        try
        {
            var evento = await _eventoService.UpdateEvento(User.GetUserId(), id, model);
            if (evento == null)
                return BadRequest("Erro ao tentar atualizar o evento.");
            return Ok(evento);
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar atualizar o Evento {e.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int eventoId)
    {
        try
        {

            var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(), eventoId, false);
            if (evento == null)
                return NoContent();

            if (await _eventoService.DeleteEvento(User.GetUserId(), eventoId))
            {
                _util.DeleteImage(evento.ImagemURL!, _destino);
                return Ok(new { message = "Deletado" });
            }

            return BadRequest("Erro ao deletar o evento.");
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao deletar o Evento {e.Message}");
        }
    }
}