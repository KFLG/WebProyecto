using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProyecto.AccesoDatos.Repositorio.IRepositorio;
using WebProyecto.Modelos;


namespace WebProyecto.AccesoDatos.Repositorio.IRepositorio
{
    
        public interface ICategoriaPerfumeRepositorio : IRepositorio<CategoriaPerfume>
        {
            void Actualizar(CategoriaPerfume categoriaperfume);

        }
    }

