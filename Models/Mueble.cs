using System;
using System.Collections.Generic;

namespace MiradorB.Models;

public partial class Mueble
{
    public int IdMueble { get; set; }

    public string Nombre { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public decimal? Valor { get; set; }

    public bool Estado { get; set; }

    public byte[]? Imagen { get; set; }

    public virtual ICollection<HabitacionMueble> HabitacionMuebles { get; set; } = new List<HabitacionMueble>();
}
