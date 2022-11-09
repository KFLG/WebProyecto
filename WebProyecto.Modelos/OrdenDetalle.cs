using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebProyecto.Modelos
{
    public class OrdenDetalle
    {

        [Key]
        public int Id { get; set; }
        public int OrdenId { get; set; }
        [ForeignKey("OrdenId")]
        public Orden Orden { get; set; }


        [Required]
        public int FlorId { get; set; }
        [ForeignKey("FlorId")]
        public Flor Flor { get; set; }


        [Required]
        public int PerfumeId { get; set; }
        [ForeignKey("PerfumeId")]
        public Perfume Perfume { get; set; }


        [Required]
        public int Cantidad { get; set; }
        [Required]
        public double Precio { get; set; }



    }
}
