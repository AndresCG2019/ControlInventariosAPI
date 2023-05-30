using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ControlInventariosAPI.Entidades
{
    public class Proveedor
    {
        [Key]
        public int IdProveedor { get; set; }
        [StringLength(maximumLength: 100)]
        [Required]
        public string nombre { get; set; }
        [StringLength(maximumLength: 100)]
        [Required]
        public string Direccion { get; set; }
        [StringLength(maximumLength: 20)]
        [Required]
        public string Telefono { get; set; }
        [StringLength(maximumLength: 50)]
        [Required]
        public string Email { get; set; }
        [StringLength(maximumLength: 13)]
        [Required]
        public string RFC { get; set; }

        public List<ArticuloIngreso> Ingresos { get; set; }
    }
}
