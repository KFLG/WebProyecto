using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebProyecto.Modelos.ViewModels
{
    public class InventarioVM
    {

        public Inventario Inventario { get; set; }
        public InventarioDetalle InventarioDetalle { get; set; }
        public List<InventarioDetalle> InventarioDetalles { get; set; }
        public IEnumerable<SelectListItem> BodegaLista { get; set; }
        public IEnumerable<SelectListItem> PerfumeLista { get; set; }
        public IEnumerable<SelectListItem> FlorLista { get; set; }



    }
}
