using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LaTiendaAPI.Models;

public partial class Role
{
    [Key]
    public int RolId { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<UsuarioRole> UsuarioRoles { get; set; } = new List<UsuarioRole>(); 

}
