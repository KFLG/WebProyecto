using System.Linq;
using WebProyecto.Modelos;
using WebProyecto.AccesoDatos.Repositorio.IRepositorio;
using WebProyecto.Data;

namespace WebProyecto.AccesoDatos.Repositorio
{
    public class CategoriaFlorRepositorio : Repositorio<CategoriaFlor>,
  ICategoriaFlorRepositorio
    {
        private readonly ApplicationDbContext _db;
        public CategoriaFlorRepositorio (ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Actualizar(CategoriaFlor categoriaflor)
        {
            var categoriaflorDb = _db.CategoriasFlor.FirstOrDefault(b => b.id == categoriaflor.id);
            if (categoriaflorDb != null)
            {
                categoriaflorDb.Nombre = categoriaflor.Nombre;
                categoriaflorDb.Estado = categoriaflor.Estado;
            }
        }
    }

}
