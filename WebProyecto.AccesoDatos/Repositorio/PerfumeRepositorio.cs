using System.Linq;
using WebProyecto.Modelos;
using WebProyecto.AccesoDatos.Repositorio.IRepositorio;
using WebProyecto.Data;

namespace WebProyecto.AccesoDatos.Repositorio
{
    public class PerfumeRepositorio : Repositorio<Perfume>, IPerfumeRepositorio
    {
        private readonly ApplicationDbContext _db;
        public PerfumeRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Actualizar(Perfume perfume)
        {
            var perfumeDb = _db.Perfumes.FirstOrDefault(p => p.Id == perfume.Id);
            if (perfumeDb != null)
            {
                if (perfume.ImageUrl != null)
                {
                    perfumeDb.ImageUrl = perfume.ImageUrl;
                }
            }
            perfumeDb.NumeroSerie = perfume.NumeroSerie;
            perfumeDb.Descripcion = perfume.Descripcion;
            perfumeDb.Contenido = perfume.Contenido;
            perfumeDb.Concentracion = perfume.Concentracion;
            perfumeDb.Precio = perfume.Precio;
            perfumeDb.Costo = perfume.Costo;
            perfumeDb.CategoriaPerfumeId = perfume.CategoriaPerfumeId;
            perfumeDb.MarcaId = perfume.MarcaId;
        }
    }
}
