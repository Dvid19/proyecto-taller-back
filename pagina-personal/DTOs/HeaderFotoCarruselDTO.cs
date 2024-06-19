namespace pagina_personal.DTOs
{
    public class HeaderFotoCarruselDTO
    {
        public int? IdHeaderFotoCarrusel { get; set; }

        public string? Foto { get; set; }
    }

    public class HeaderFotoCarruselPostDTO
    {
        public int? IdHeaderFotoCarrusel { get; set; }

        public IFormFile? Foto { get; set; }
    }
}
