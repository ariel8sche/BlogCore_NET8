using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models
{
    public class Categoria
    {
        public Categoria()
        {
            FechaCreacion = DateTime.Now;
        }
        
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="El nombre de categoría debe ser obligatorio")]
        [MaxLength(60, ErrorMessage ="El nombre de categoría no debe exceder los 60 caracteres")]
        [Display(Name ="Nombre de Categoría")]
        public string? Nombre { get; set; }

        [Display(Name ="Orden de Visualización")]
        [Range(0, 10, ErrorMessage ="El valor debe estar entre 1 y 10")]
        public int Orden { get; set; }

        [Display(Name ="Fecha de Creación")]
        public DateTime FechaCreacion { get; set; }

    }
}
