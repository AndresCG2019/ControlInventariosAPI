using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ControlInventariosAPI.Entidades
{
    public class Pedido
    {
        [Key]
        public int IdPedido { get; set; }
        public DateTime FechaPedido { get; set; }
        public Cliente Cliente { get; set; }
        public int IdCliente { get; set; }

        public List<ArticuloEgreso> Egresos { get; set; }
    }
}
