using System;
using System.Collections.Generic;

namespace MiradorB.Models;

public partial class EstadosReserva
{
    public int IdEstadoReserva { get; set; }

    public string NombreEstadoReserva { get; set; } = null!;

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
