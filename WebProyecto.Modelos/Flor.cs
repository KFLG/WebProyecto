using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebProyecto.Modelos
{
    public class Flor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        [Display(Name = "Número de Serie")]
        public string NumeroSerie { get; set; }

        [Required]
        [MaxLength(60)]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
       
        [Required]
        [Range(1, 1000000)]
        [Display(Name = "Precio")]
        public double Precio { get; set; }

        [Required]
        [Range(1, 9000000)]
        [Display(Name = "Costo")]
        public double Costo { get; set; }

        public string ImageUrl { get; set; }

        //foreign keys

        [Required]
        public int CategoriaFlorId { get; set; }
        [ForeignKey("CategoriaFlorId")]
        public CategoriaFlor CategoriaFlor { get; set; }

        [Required]
        public int EspecieId { get; set; }
        [ForeignKey("EspecieId")]
        public Especie Especie { get; set; }


    }
}
