using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.Application.DomainService;
using ProEventos.Application.ViewModels;

namespace ProEventos.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RedesSociaisController : ControllerBase
{
    private readonly IEventoService _eventoService;
    private readonly IPalestranteService _palestranteService;
    private readonly IRedeSocialService _redeSocialService;

    public RedesSociaisController(IRedeSocialService redeSocialService,
                                  IEventoService eventoService,
                                  IPalestranteService palestranteService)
    {
        _redeSocialService = redeSocialService;
        _eventoService = eventoService;
        _palestranteService = palestranteService;
    }

    [HttpGet("evento/{eventoId}")]
    public async Task<IActionResult> GetByEvento(int eventoId)
    {
        try
        {
            if (!(await AutorEvento(eventoId)))
                return Unauthorized("Usuário sem permissão no evento informado.");

            var redesSociais = await _redeSocialService.GetAllByEventoIdAsync(eventoId);
            if (redesSociais == null)
                return NoContent();
            return Ok(redesSociais);
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar Redes Sociais: {e.Message}");
        }
    }

    [HttpGet("palestrante")]
    public async Task<IActionResult> GetByPalestrante()
    {
        try
        {
            var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId());
            if (palestrante == null)
                return Unauthorized();
            
            var redeSocial = await _redeSocialService.GetAllByPalestranteIdAsync(palestrante.Id);
            if (redeSocial == null)
                return NoContent();
            return Ok(redeSocial);
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar Redes Sociais: {e.Message}");
        }
    }

    [HttpPut("evento/{eventoId}")]
    public async Task<IActionResult> SaveByEvento(int eventoId, RedeSocialViewModel[] models)
    {
        try
        {
            if (!(await AutorEvento(eventoId)))
                return Unauthorized("Usuário sem permissão no evento informado.");

            var redeSocial = await _redeSocialService.SaveByEvento(eventoId, models);
            if (redeSocial == null)
                return BadRequest("Erro ao tentar salvar a Rede Social por Evento.");
            return Ok(redeSocial);
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar salvar a Rede Social por Evento: {e.Message}");
        }
    }

    [HttpPut("palestrante")]
    public async Task<IActionResult> SaveByPalestrante(RedeSocialViewModel[] models)
    {
        try
        {
            var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId());
            if (palestrante == null)
                return Unauthorized();

            var redeSocial = await _redeSocialService.SaveByPalestrante(palestrante.Id, models);
            if (redeSocial == null)
                return BadRequest("Erro ao tentar salvar a Rede Social por Palestrante.");
            return Ok(redeSocial);
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar salvar a Rede Social por Palestrante {e.Message}");
        }
    }    

    [HttpDelete("evento/{eventoId}/{redeSocialId}")]
    public async Task<IActionResult> DeleteByEvento(int eventoId, int redeSocialId)
    {
        try
        {
            if (!(await AutorEvento(eventoId)))
                return Unauthorized("Usuário sem permissão no evento informado.");

            var redeSocial = await _redeSocialService.GetRedeSocialEventoByIdsAsync(eventoId, redeSocialId);
            if (redeSocial == null)
                return NoContent();

            return await _redeSocialService.DeleteByEvento(eventoId, redeSocialId) ? Ok(new { message = "Rede Social Deletada" }) : BadRequest("Erro ao deletar a Rede Social por Evento.");
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao deletar a Rede Social por Evento{e.Message}");
        }
    }

    [HttpDelete("palestrante/{redeSocialId}")]
    public async Task<IActionResult> DeleteByPalestrante(int redeSocialId)
    {
        try
        {
            var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId());
            if (palestrante == null)
                return Unauthorized();

            var redeSocial = await _redeSocialService.GetRedeSocialPalestranteByIdAsync(palestrante.Id, redeSocialId);
            if (redeSocial == null)
                return NoContent();

            return await _redeSocialService.DeleteByPalestrante(palestrante.Id, redeSocialId) ? Ok(new { message = "Rede Social Deletada" }) : BadRequest("Erro ao deletar a Rede Social por Palestrante.");
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao deletar a Rede Social por Palestrante{e.Message}");
        }
    }


    [NonAction]
    private async Task<bool> AutorEvento(int eventoId){
        var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(), eventoId);
        return evento != null;
    }
}