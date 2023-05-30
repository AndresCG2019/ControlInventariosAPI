using System;

namespace ControlInventariosAPI.DTOs
{
    public class MovimientoDTO
    {
        public string ClaveArticulo { get; set; }
        public string Fecha { get; set; }
        public decimal Monto { get; set; }
        public int Cantidad { get; set; }
        public string TipoMovimiento { get; set; }
        public string NombreCliente { get; set; }
        public string NombreProveedor { get; set; }
    }
}
