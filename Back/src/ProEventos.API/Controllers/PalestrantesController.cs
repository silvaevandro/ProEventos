using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.Application.Services;
using ProEventos.Application.ViewModels;
using ProEventos.Infra.Data.Models;

namespace ProEventos.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PalestrantesController : ControllerBase
{
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly IUserService _userService;

    public IPalestranteService _palestranteService { get; set; }

    public PalestrantesController(IPalestranteService PalestranteService,
                             IWebHostEnvironment hostEnvironment,
                             IUserService userService)
    {
        _palestranteService = PalestranteService;
        _hostEnvironment = hostEnvironment;
        _userService = userService;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll([FromQuery]PageParms pageParams)
    {
        try
        {
            var palestrantes = await _palestranteService.GetAllPalestrantesAsync(pageParams, true);
            if (palestrantes == null)            
                return NoContent();

            Response.AddPagination(palestrantes.CurrentPage, palestrantes.PageSize, palestrantes.TotalCount, palestrantes.TotalPages);
            return Ok(palestrantes);
        }
        catch (System.Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar Palestrantes: {e.Message}");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetPalestrantes()
    {
        try
        {
            var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId(), true);
            if (palestrante == null)
                return NoContent();
            return Ok(palestrante);
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar Palestrantes {e.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post(PalestranteAddViewModel model)
    {
        try
        {
            var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId());
            if (palestrante == null)
                palestrante = await _palestranteService.AddPalestrantes(User.GetUserId(), model);
            if (palestrante == null)
                return BadRequest("Erro ao tentar adicionar Palestrante.");
            return Ok(palestrante);
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar adicionar o Palestrante {e.Message}");
        }
    }

    [HttpPut]
    public async Task<IActionResult> Put(PalestranteUpdateViewModel model)
    {
        try
        {
            var palestrante = await _palestranteService.UpdatePalestrante(User.GetUserId(), model);
            if (palestrante == null)
                return BadRequest("Erro ao tentar atualizar o Palestrante.");
            return Ok(palestrante);
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar atualizar o Palestrante {e.Message}");
        }
    }
}