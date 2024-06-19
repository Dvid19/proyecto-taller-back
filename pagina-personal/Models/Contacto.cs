using System;
using System.Collections.Generic;

namespace pagina_personal.Models;

public partial class Contacto
{
    public int IdContacto { get; set; }

    public string? Foto { get; set; }

    public string? Titulo { get; set; }

    public string? Subtitulo { get; set; }
}
