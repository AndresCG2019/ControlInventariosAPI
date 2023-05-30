using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlInventariosAPI.Entidades
{
    public class Articulo
    {
        [Key]
        [StringLength(maximumLength: 100)]
        public string ClaveArticulo { get; set; }
        [StringLength(maximumLength: 80)]
        public string Descripcion { get; set; }
        [StringLength(maximumLength: 50)]
        public string Unidad { get; set; }
        public int Existencia { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioVenta { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioCompra { get; set; }
        public Categoria Categoria { get; set; }
        public int IdCategoria { get; set; }

        public List<ArticuloIngreso> Ingresos { get; set; }
        public List<ArticuloEgreso> Egresos { get; set; }

    }
}
