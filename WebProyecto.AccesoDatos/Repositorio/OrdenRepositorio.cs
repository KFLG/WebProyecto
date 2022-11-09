using WebProyecto.Modelos;
using WebProyecto.AccesoDatos.Repositorio.IRepositorio;
using WebProyecto.Data;

namespace WebProyecto.AccesoDatos.Repositorio
{

    public class OrdenRepositorio : Repositorio<Orden>,
 IOrdenRepositorio
        {
            private readonly ApplicationDbContext _db;
            public OrdenRepositorio(ApplicationDbContext db) : base(db)
            {
                _db = db;
            }
            public void Actualizar(Orden orden)
            {
                _db.Update(orden);
            }
        }

    }

