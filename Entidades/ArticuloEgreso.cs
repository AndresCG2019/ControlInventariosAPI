using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlInventariosAPI.Entidades
{
    public class ArticuloEgreso
    {
        [Key]
        public int IdEgreso { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioVenta { get; set; }
        public int Cantidad { get; set; }
        public Articulo Articulo { get; set; }
        public string ClaveArticulo { get; set; }
        public Pedido Pedido { get; set; }
        public int IdPedido { get; set; }
    }
}
