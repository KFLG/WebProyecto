using WebProyecto.Modelos;
using WebProyecto.AccesoDatos.Repositorio.IRepositorio;
using WebProyecto.Data;

namespace WebProyecto.AccesoDatos.Repositorio
{
    public class CarroComprasRepositorio : Repositorio<CarroCompras>,
 ICarroComprasRepositorio
    {
        private readonly ApplicationDbContext _db;
        public CarroComprasRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Actualizar(CarroCompras carroCompras)
        {
            _db.Update(carroCompras);
        }
    }
}
