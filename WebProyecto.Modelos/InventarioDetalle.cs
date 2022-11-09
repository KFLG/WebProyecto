using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebProyecto.Modelos
{
    public class InventarioDetalle
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public int InventarioId { get; set; }

        [ForeignKey("InventarioId")]
        public Inventario Inventario { get; set; }


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
        [Display(Name = "Stock Anterior")]
        public int StockAnterior { get; set; }

        [Required]
        [Display(Name = "Cantidad")]
        public int Cantidad { get; set; }





    }
}
