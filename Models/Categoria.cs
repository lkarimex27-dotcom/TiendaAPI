using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LaTiendaAPI.Models;

public partial class Categoria
{
    public int idCategoria { get; set; }

    public string nombre { get; set; } = null!;

    public bool? estado { get; set; }

    [JsonIgnore]
    public virtual ICollection<Producto> productos { get; set; } = new List<Producto>();
}
