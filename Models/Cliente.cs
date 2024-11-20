using System;
using System.Collections.Generic;

namespace MiradorB.Models;

public partial class Cliente
{
    public int NroDocumento { get; set; }

    public int IdTipoDocumento { get; set; }

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;


    public string Celular { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public bool Estado { get; set; }

    public int? IdRol { get; set; }

    public virtual Role? IdRolNavigation { get; set; }

    public virtual TipoDocumento IdTipoDocumentoNavigation { get; set; } = null!;

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
