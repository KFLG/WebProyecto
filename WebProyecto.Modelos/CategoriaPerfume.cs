using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebProyecto.Modelos
{
    public class CategoriaPerfume
    {
        [Key]
        public int id { get; set; }
        [Required]
        [MaxLength(50)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }
        [Required]
        public bool Estado { get; set; }
    }
}
