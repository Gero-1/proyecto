﻿using System;
using System.Collections.Generic;

namespace MiradorB.Models;

public partial class Reserva
{
    public int IdReserva { get; set; }

    public int NroDocumentoCliente { get; set; }

    public int NroDocumentoUsuario { get; set; }

    public DateTime FechaReserva { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime FechaFinalizacion { get; set; }

    public double SubTotal { get; set; }

    public double Descuento { get; set; }

    public double Iva { get; set; }

    public double MontoTotal { get; set; }

    public int MetodoPago { get; set; }

    public int NroPersonas { get; set; }

    public int IdEstadoReserva { get; set; }

    public virtual ICollection<Abono> Abonos { get; set; } = new List<Abono>();

    public virtual ICollection<DetalleReservaPaquete> DetalleReservaPaquetes { get; set; } = new List<DetalleReservaPaquete>();

    public virtual ICollection<DetalleReservaServicio> DetalleReservaServicios { get; set; } = new List<DetalleReservaServicio>();

    public virtual EstadosReserva IdEstadoReservaNavigation { get; set; } = null!;

    public virtual MetodoPago MetodoPagoNavigation { get; set; } = null!;

    public virtual Cliente NroDocumentoClienteNavigation { get; set; } = null!;

    public virtual Usuario NroDocumentoUsuarioNavigation { get; set; } = null!;
}
