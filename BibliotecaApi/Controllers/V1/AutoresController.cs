using Asp.Versioning;
using BibliotecaApi.Data;
using BibliotecaApi.Dtos;
using BibliotecaApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaApi.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AutoresController : ControllerBase
{
    private readonly AppDbContext _context;

    public AutoresController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AutorResponseDto>>> GetAll()
    {
        var autores = await _context.Autores
            .AsNoTracking()
            .Select(a => new AutorResponseDto(a.Id, a.Nome, a.Nacionalidade))
            .ToListAsync();

        return Ok(autores);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AutorResponseDto>> GetById(int id)
    {
        var autor = await _context.Autores.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);

        if (autor is null)
            return NotFound(new { mensagem = $"Autor {id} não encontrado." });

        return Ok(ToResponse(autor));
    }

    [HttpPost]
    public async Task<ActionResult<AutorResponseDto>> Create(AutorRequestDto dto)
    {
        var autor = new Autor { Nome = dto.Nome, Nacionalidade = dto.Nacionalidade };

        _context.Autores.Add(autor);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = autor.Id, version = "1.0" }, ToResponse(autor));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, AutorRequestDto dto)
    {
        var autor = await _context.Autores.FindAsync(id);

        if (autor is null)
            return NotFound(new { mensagem = $"Autor {id} não encontrado." });

        autor.Nome = dto.Nome;
        autor.Nacionalidade = dto.Nacionalidade;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var autor = await _context.Autores.FindAsync(id);

        if (autor is null)
            return NotFound(new { mensagem = $"Autor {id} não encontrado." });

        _context.Autores.Remove(autor);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static AutorResponseDto ToResponse(Autor autor) =>
        new(autor.Id, autor.Nome, autor.Nacionalidade);
}