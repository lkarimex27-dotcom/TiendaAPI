using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;  // ← agrega este using

namespace LaTiendaAPI.Models;

public partial class UsuarioRole
{
    public int UsuarioRolId { get; set; }

    public int UsuarioId { get; set; }

    public int RolId { get; set; }

    [ForeignKey("RolId")]      // ← agrega esto
    public virtual Role Rol { get; set; } = null!;

    [ForeignKey("UsuarioId")]  // ← agrega esto
    public virtual Usuario Usuario { get; set; } = null!;
}