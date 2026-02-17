using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models
{
    public class Articulo
    {
        public Articulo()
        {
            FechaCreacion = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="El nombre del artículo es obligatorio")]
        [Display(Name ="Nombre del Artículo")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción del artículo es obligatorio")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Display(Name = "Fecha de Creación")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Imagen")]
        public string urlImagen { get; set; }

        [Required(ErrorMessage = "La categoría del artículo es obligatoria")]
        public int CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set; }
    }

}
