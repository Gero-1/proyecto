using System;
using System.Collections.Generic;

namespace MiradorB.Models;

public partial class MetodoPago
{
    public int IdMetodoPago { get; set; }

    public string NomMetodoPago { get; set; } = null!;

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
