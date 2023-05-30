using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ControlInventariosAPI.Entidades
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }
        [StringLength(maximumLength: 50)]
        [Required]
        public string Descripcion { get; set; }
        public List<Articulo> Articulos { get; set; }
    }
}
