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
public class LivrosController : ControllerBase
{
    private readonly AppDbContext _context;

    public LivrosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LivroResponseDto>>> GetAll()
    {
        var livros = await _context.Livros
            .Include(l => l.Autor)
            .AsNoTracking()
            .ToListAsync();

        return Ok(livros.Select(ToResponse));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<LivroResponseDto>> GetById(int id)
    {
        var livro = await _context.Livros
            .Include(l => l.Autor)
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == id);

        if (livro is null)
            return NotFound(new { mensagem = $"Livro {id} não encontrado." });

        return Ok(ToResponse(livro));
    }

    [HttpPost]
    public async Task<ActionResult<LivroResponseDto>> Create(LivroRequestDto dto)
    {
        var autor = await _context.Autores.FindAsync(dto.AutorId);

        if (autor is null)
            return BadRequest(new { mensagem = $"Autor {dto.AutorId} não existe." });

        var livro = new Livro
        {
            Titulo = dto.Titulo,
            AnoPublicacao = dto.AnoPublicacao,
            Isbn = dto.Isbn,
            AutorId = dto.AutorId,
            Autor = autor
        };

        _context.Livros.Add(livro);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = livro.Id, version = "1.0" }, ToResponse(livro));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, LivroRequestDto dto)
    {
        var livro = await _context.Livros.FindAsync(id);

        if (livro is null)
            return NotFound(new { mensagem = $"Livro {id} não encontrado." });

        var autorExiste = await _context.Autores.AnyAsync(a => a.Id == dto.AutorId);

        if (!autorExiste)
            return BadRequest(new { mensagem = $"Autor {dto.AutorId} não existe." });

        livro.Titulo = dto.Titulo;
        livro.AnoPublicacao = dto.AnoPublicacao;
        livro.Isbn = dto.Isbn;
        livro.AutorId = dto.AutorId;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var livro = await _context.Livros.FindAsync(id);

        if (livro is null)
            return NotFound(new { mensagem = $"Livro {id} não encontrado." });

        _context.Livros.Remove(livro);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static LivroResponseDto ToResponse(Livro livro) =>
        new(livro.Id, livro.Titulo, livro.AnoPublicacao, livro.Isbn, livro.AutorId, livro.Autor?.Nome ?? string.Empty);
}