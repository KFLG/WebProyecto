using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebProyecto.AccesoDatos.Repositorio.IRepositorio
{
    
        public interface IUnidadTrabajo : IDisposable

        {
            //envolvemos todo lo necesario de todas las entidades del proyecto
            //wrapper
            IBodegaRepositorio Bodega { get;  }
            ICategoriaFlorRepositorio CategoriaFlor { get; }
            ICategoriaPerfumeRepositorio CategoriaPerfume { get; }
            IMarcaRepositorio Marca { get; }
            IFlorRepositorio Flor { get; }
            IPerfumeRepositorio Perfume { get; }
            IUsuarioAplicacionRepositorio UsuarioAplicacion { get; }
            ICompaniaRepositorio Compania { get; }
            IEspecieRepositorio Especie { get; }
        ICarroComprasRepositorio CarroCompras { get; }
            IOrdenRepositorio Orden { get; }
            IOrdenDetalleRepositorio OrdenDetalle { get; }
            void Guardar();
        }
}

