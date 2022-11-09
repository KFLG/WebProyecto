using System.Linq;
using WebProyecto.Modelos;
using WebProyecto.AccesoDatos.Repositorio.IRepositorio;
using WebProyecto.Data;

namespace WebProyecto.AccesoDatos.Repositorio
{
    public class FlorRepositorio : Repositorio<Flor>, IFlorRepositorio
    {
        private readonly ApplicationDbContext _db;
        public FlorRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Actualizar(Flor flor)
        {
            var florDb = _db.Flores.FirstOrDefault(p => p.Id == flor.Id);
            if (florDb != null)
            {
                if (flor.ImageUrl != null)
                {
                    florDb.ImageUrl = flor.ImageUrl;
                }
            }
            florDb.NumeroSerie = flor.NumeroSerie;
            florDb.Descripcion = flor.Descripcion;
            florDb.Precio = flor.Precio;
            florDb.Costo = flor.Costo;
            florDb.CategoriaFlorId = flor.CategoriaFlorId;

        }
    }
}
