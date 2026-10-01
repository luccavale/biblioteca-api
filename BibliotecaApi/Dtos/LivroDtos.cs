using System.ComponentModel.DataAnnotations;

namespace BibliotecaApi.Dtos;

public class LivroRequestDto
{
    [Required(ErrorMessage = "O título é obrigatório.")]
    [StringLength(150, ErrorMessage = "O título deve ter no máximo 150 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    [Range(1000, 2100, ErrorMessage = "O ano de publicação deve estar entre 1000 e 2100.")]
    public int AnoPublicacao { get; set; }

    [Required(ErrorMessage = "O ISBN é obrigatório.")]
    [StringLength(20, ErrorMessage = "O ISBN deve ter no máximo 20 caracteres.")]
    public string Isbn { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Informe um AutorId válido.")]
    public int AutorId { get; set; }
}

public record LivroResponseDto(int Id, string Titulo, int AnoPublicacao, string Isbn, int AutorId, string AutorNome);