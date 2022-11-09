using System.Linq;
using WebProyecto.Modelos;
using WebProyecto.AccesoDatos.Repositorio.IRepositorio;
using WebProyecto.Data;

namespace WebProyecto.AccesoDatos.Repositorio
{
    public class CategoriaPerfumeRepositorio : Repositorio<CategoriaPerfume>,
  ICategoriaPerfumeRepositorio
    {
        private readonly ApplicationDbContext _db;
        public CategoriaPerfumeRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Actualizar(CategoriaPerfume categoriaperfume)
        {
            var categoriaperfumeDb = _db.CategoriasPerfume.FirstOrDefault(b => b.id == categoriaperfume.id);
            if (categoriaperfumeDb != null)
            {
                categoriaperfumeDb.Nombre = categoriaperfume.Nombre;
                categoriaperfumeDb.Estado = categoriaperfume.Estado;
            }
        }
    }

}
