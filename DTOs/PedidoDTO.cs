using System;

namespace ControlInventariosAPI.DTOs
{
    public class PedidoDTO
    {
        public int IdPedido { get; set; }
        public DateTime FechaPedido { get; set; }
        public int IdCliente { get; set; }
        public string NombreCliente { get; set; }
        public decimal total { get; set; }
    }
}
