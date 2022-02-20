using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.Application.DomainService;
using ProEventos.Application.ViewModels;

namespace ProEventos.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EventosController : ControllerBase
{
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly IUserService _userService;

    public IEventoService _eventoService { get; set; }

    public EventosController(IEventoService eventoService,
                             IWebHostEnvironment hostEnvironment,
                             IUserService userService)
    {
        _eventoService = eventoService;
        _hostEnvironment = hostEnvironment;
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var eventos = await _eventoService.GetAllEventosAsync(User.GetUserId(), true);
            if (eventos == null)
                return NoContent();
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

    [HttpGet("{tema}/tema")]
    public async Task<IActionResult> Get(string tema)
    {
        try
        {
            var eventos = await _eventoService.GetAllEventosByTemaAsync(User.GetUserId(), tema, true);
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
            if (file.Length > 0 && evento.ImagemURL != null)
            {
                DeleteImage(evento.ImagemURL!);
                evento.ImagemURL = await SaveImage(file);
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
                DeleteImage(evento.ImagemURL!);
                return Ok(new { message = "Deletado" });
            }

            return BadRequest("Erro ao deletar o evento.");
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao deletar o Evento {e.Message}");
        }
    }


    [NonAction]
    private async Task<string> SaveImage(IFormFile imageFile)
    {
        string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName)
                                          .Take(10)
                                          .ToArray()
                                         ).Replace(' ', '-');
        imageName = $"{imageName}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(imageFile.FileName)}";
        var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources/Images", imageName);

        using (var fileStream = new FileStream(imagePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(fileStream);
        }
        return imageName;
    }

    [NonAction]
    private void DeleteImage(string imageName)
    {
        var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources/Images", imageName);
        if (System.IO.File.Exists(imagePath))
        {
            System.IO.File.Delete(imagePath);
        }
    }
}