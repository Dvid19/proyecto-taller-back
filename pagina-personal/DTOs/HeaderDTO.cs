namespace pagina_personal.DTOs
{
    public class HeaderDTO
    {
        public int? IdHeader { get; set; }

        public string? Titulo { get; set; }

        public string? Subtitulo { get; set; }

        public string? DocumentoCv { get; set; }

        public List<HeaderFotoCarruselDTO>? Carrusel { get; set; }

        public string? FotoFondo { get; set; }
    }

    public class HeaderTitularDTO
    {
        public string? Titulo { get; set; }
        public string? Subtitulo { get; set; }
    }

    public class HeaderCvDTO
    {
        public IFormFile? Documento { get; set; }
    }
}
