using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlInventariosAPI.Entidades
{
    public class ArticuloIngreso
    {
        [Key]
        public int IdIngreso { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioCompra { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaIngreso { get; set; }
        public Articulo Articulo { get; set; }
        public string ClaveArticulo { get; set; }
        public Proveedor Proveedor { get; set; }
        public int IdProveedor { get; set; }
    }
}
