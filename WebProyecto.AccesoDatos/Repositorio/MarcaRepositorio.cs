using System.Linq;
using WebProyecto.Modelos;
using WebProyecto.AccesoDatos.Repositorio.IRepositorio;
using WebProyecto.Data;

namespace WebProyecto.AccesoDatos.Repositorio
{
    public class MarcaRepositorio : Repositorio<Marca>, IMarcaRepositorio
    {
        private readonly ApplicationDbContext _db;
        public MarcaRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Actualizar(Marca marca)
        {
            var marcaDb = _db.Marcas.FirstOrDefault(b => b.id == marca.id);
            if (marcaDb != null)
            {
                marcaDb.Nombre = marca.Nombre;
                marcaDb.Estado = marca.Estado;
            }
        }
    }
}
