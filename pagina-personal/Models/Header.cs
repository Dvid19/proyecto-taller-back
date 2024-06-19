using System;
using System.Collections.Generic;

namespace pagina_personal.Models;

public partial class Header
{
    public int IdHeader { get; set; }

    public string? Titulo { get; set; }

    public string? Subtitulo { get; set; }

    public string? DocumentoCv { get; set; }

    public string? FotoFondo { get; set; }
}
