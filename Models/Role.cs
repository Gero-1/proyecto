﻿using System;
using System.Collections.Generic;

namespace MiradorB.Models;

public partial class Role
{
    public int IdRol { get; set; }

    public string? NomRol { get; set; }

    public bool? Estado { get; set; }

    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();

    public virtual ICollection<PermisosRole> PermisosRoles { get; set; } = new List<PermisosRole>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
