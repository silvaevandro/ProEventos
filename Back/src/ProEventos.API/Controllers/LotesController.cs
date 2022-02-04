using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.DomainService;
using ProEventos.Application.ViewModels;

namespace ProEventos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LotesController : ControllerBase
{
    public ILoteService loteService { get; set; }

    public LotesController(ILoteService loteService)
    {
        this.loteService = loteService;
    }

    [HttpGet("{eventoId}")]
    public async Task<IActionResult> Get(int eventoId)
    {
        try
        {
            var lotes = await loteService.GetLotesByEventoIdAsync(eventoId);
            if (lotes == null)
                return NoContent();
            return Ok(lotes);
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar Eventos: {e.Message}");
        }
    }


    [HttpPut("{eventoId}")]
    public async Task<IActionResult> Put(int eventoId, LoteViewModel[] models)
    {
        try
        {
            var lotes = await loteService.SaveLotes(eventoId, models);
            if (lotes == null)
                return BadRequest("Erro ao tentar salvar os Lotes.");
            return Ok(lotes);
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar salvar os Lotes {e.Message}");
        }
    }

    [HttpDelete("{eventoId}/{loteId}")]
    public async Task<IActionResult> Delete(int eventoId, int loteId)
    {
        try
        {
            var lote = await loteService.GetLoteByIdsAsync(eventoId, loteId);
            if (lote == null)
                return NoContent();
            return await loteService.DeleteLote(eventoId, loteId) ? Ok(new { message = "Lote Deletado" }) : BadRequest("Erro ao deletar o lote.");
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao deletar o Lote {e.Message}");
        }
    }
};