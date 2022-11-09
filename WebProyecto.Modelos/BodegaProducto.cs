using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebProyecto.Modelos
{
    public class BodegaProducto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Bodega")]

        public int BodegaId { get; set; }
        [ForeignKey("BodegaId")]
        public Bodega Bodega { get; set; }

        [Required]
        [Display(Name = "Flor")]

        public int FlorId { get; set; }
        [ForeignKey("FlorId")]
        public Flor Flor { get; set; }

        [Required]
        [Display(Name = "Perfume")]

        public int PerfumeId { get; set; }
        [ForeignKey("PerfumeId")]
        public Perfume Perfume { get; set; }

        [Required]
        [Display(Name = "Cantidad")]
        public int Cantidad { get; set; }
    }
}
