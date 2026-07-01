using System;
using System.Collections.Generic;

namespace LaTiendaAPI.Models;

public partial class Producto
{
    public int idProducto { get; set; }

    public int idCategoria { get; set; }

    public string? nombre { get; set; }

    public decimal? precio { get; set; }

    public int? stock { get; set; }

    public bool? estado { get; set; }

    public virtual Categoria? objCategoria { get; set; }
}
