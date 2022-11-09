using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebProyecto.Modelos
{
    public class CarroCompras
    {

        [Key]
        public int Id { get; set; }
        [Required]
        public string UsuarioAplicacionId { get; set; }
        [ForeignKey("UsuarioAplicacionId")]
        public UsuarioAplicacion UsuarioAplicacion { get; set; }

        [Required]
        public int FlorId { get; set; }
        [ForeignKey("FlorId")]
        public Flor Flor { get; set; }

        [Required]
        public int PerfumeId { get; set; }
        [ForeignKey("PerfumeId")]
        public Perfume Perfume { get; set; }

        [Required]
        [Range(1, 1000, ErrorMessage = "Ingrese un valor entre 1 y 1000")]
        public int Cantidad { get; set; }
        [NotMapped]
        public double Precio { get; set; }



    }
}
