using System;
using System.Collections.Generic;

namespace MiradorB.Models;

public partial class DetalleReservaServicio
{
    public int IdDetalleReservaServicio { get; set; }

    public int IdServicio { get; set; }

    public int IdReserva { get; set; }

    public int Cantidad { get; set; }

    public double Precio { get; set; }

    public virtual Reserva IdReservaNavigation { get; set; } = null!;

    public virtual Servicio IdServicioNavigation { get; set; } = null!;
}
