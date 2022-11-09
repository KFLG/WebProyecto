using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebProyecto.Modelos.ViewModels
{
    public class FlorVM
    {

        public Flor Flor { get; set; }
        public IEnumerable<SelectListItem> CategoriaFlorLista { get; set; }
        public IEnumerable<SelectListItem> PadreLista { get; set; }
        public IEnumerable<Flor> FlorLista { get; set; }


    }
}
