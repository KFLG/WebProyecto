using System.Linq;
using WebProyecto.Modelos;
using WebProyecto.AccesoDatos.Repositorio.IRepositorio;
using WebProyecto.Data;

namespace WebProyecto.AccesoDatos.Repositorio
{

    public class BodegaRepositorio : Repositorio<Bodega>, IBodegaRepositorio
        {
            private readonly ApplicationDbContext _db;
            public BodegaRepositorio(ApplicationDbContext db) : base(db)
            {
                _db = db;
            }
            public void Actualizar(Bodega bodega)
            {
                var bodegaDb = _db.Bodegas.FirstOrDefault(b => b.id == bodega.id);
                if (bodegaDb != null)
                {
                    bodegaDb.Nombre = bodega.Nombre;
                    bodegaDb.Descripcion = bodega.Descripcion;

                }
                bodegaDb.Estado = bodega.Estado;
            }
        }
    }

