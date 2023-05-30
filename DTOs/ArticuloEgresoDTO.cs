namespace ControlInventariosAPI.DTOs
{
    public class ArticuloEgresoDTO
    {
        public int IdEgreso { get; set; }
        public decimal PrecioVenta { get; set; }
        public int Cantidad { get; set; }
        public string ClaveArticulo { get; set; }
        public int IdPedido { get; set; }
    }
}
