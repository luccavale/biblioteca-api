namespace BibliotecaApi.Models;

public class Livro
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public int AnoPublicacao { get; set; }
    public string Isbn { get; set; } = string.Empty;
    public int AutorId { get; set; }
    public Autor? Autor { get; set; }
}