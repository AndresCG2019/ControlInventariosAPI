using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ControlInventariosAPI.Entidades
{
    public class Cliente
    {
        [Key]
        public int IdCliente { get; set; }
        [StringLength(maximumLength: 100)]
        [Required]
        public string NombreCompleto { get; set; }
        [StringLength(maximumLength: 100)]
        [Required]
        public string Domicilio { get; set; }
        [StringLength(maximumLength: 20)]
        [Required]
        public string Email { get; set; }
        [StringLength(maximumLength: 20)]
        public string Telefono { get; set; }
        public int Edad { get; set; }
        public List<Pedido> Pedidos { get; set; }
    }
}
