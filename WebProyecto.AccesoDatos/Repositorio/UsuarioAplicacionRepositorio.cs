using WebProyecto.AccesoDatos.Repositorio.IRepositorio;
using WebProyecto.Data;
using WebProyecto.Modelos;

namespace WebProyecto.AccesoDatos.Repositorio
{
    public class UsuarioAplicacionRepositorio : Repositorio<UsuarioAplicacion>,
 IUsuarioAplicacionRepositorio
    {
        private readonly ApplicationDbContext _db;
        public UsuarioAplicacionRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }

}
