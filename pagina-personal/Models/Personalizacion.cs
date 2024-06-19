using System;
using System.Collections.Generic;

namespace pagina_personal.Models;

public partial class Personalizacion
{
    public int IdPersonalizacion { get; set; }

    public string? Nav { get; set; }

    public string? Footer { get; set; }
}
