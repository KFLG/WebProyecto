using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProyecto.Modelos;

namespace WebProyecto.AccesoDatos.Repositorio.IRepositorio
{
   
        public interface IOrdenDetalleRepositorio : IRepositorio<OrdenDetalle>
        {
            void Actualizar(OrdenDetalle ordenDetalle);
        }
    }

