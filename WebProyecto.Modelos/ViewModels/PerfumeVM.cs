using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebProyecto.Modelos.ViewModels
{
    public class PerfumeVM
    {

        public Perfume Perfume { get; set; }
        public IEnumerable<SelectListItem> CategoriaPerfumeLista { get; set; }
        public IEnumerable<SelectListItem> MarcaLista { get; set; }
        public IEnumerable<SelectListItem> PadreLista { get; set; }
        public IEnumerable<Perfume> PerfumeLista { get; set; }


    }
}
