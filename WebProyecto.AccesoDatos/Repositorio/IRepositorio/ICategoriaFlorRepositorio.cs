using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProyecto.AccesoDatos.Repositorio.IRepositorio;
using WebProyecto.Modelos;


namespace WebProyecto.AccesoDatos.Repositorio.IRepositorio
{
    
        public interface ICategoriaFlorRepositorio : IRepositorio<CategoriaFlor>
        {
            void Actualizar(CategoriaFlor categoriaflor);

        }
    }

