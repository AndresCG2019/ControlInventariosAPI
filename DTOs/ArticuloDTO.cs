using ControlInventariosAPI.Entidades;

namespace ControlInventariosAPI.DTOs
{
    public class ArticuloDTO
    {
        public string ClaveArticulo { get; set; }
        public string Descripcion { get; set; }
        public string Unidad { get; set; }
        public int Existencia { get; set; }
        public decimal PrecioVenta { get; set; }
        public decimal PrecioCompra { get; set; }
        public int IdCategoria { get; set; }
        public string descripcionCategoria { get; set; }
        public decimal TotalVendido { get; set; }
        public decimal TotalComprado { get; set; }
    }
}
