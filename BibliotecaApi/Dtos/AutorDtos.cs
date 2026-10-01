using System.ComponentModel.DataAnnotations;

namespace BibliotecaApi.Dtos;

public class AutorRequestDto
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [StringLength(60, ErrorMessage = "A nacionalidade deve ter no máximo 60 caracteres.")]
    public string Nacionalidade { get; set; } = string.Empty;
}

public record AutorResponseDto(int Id, string Nome, string Nacionalidade);