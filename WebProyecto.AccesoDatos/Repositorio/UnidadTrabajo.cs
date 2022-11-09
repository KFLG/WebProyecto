using WebProyecto.AccesoDatos.Repositorio.IRepositorio;
using WebProyecto.Data;
using WebProyecto.Modelos;

namespace WebProyecto.AccesoDatos.Repositorio
{
    public class UnidadTrabajo : IUnidadTrabajo
    {
        private readonly ApplicationDbContext _db;
        public IBodegaRepositorio Bodega { get; private set; }
        public ICategoriaFlorRepositorio CategoriaFlor { get; set; }
        public ICategoriaPerfumeRepositorio CategoriaPerfume { get; set; }
        public IMarcaRepositorio Marca { get; set; }
        public IFlorRepositorio Flor { get; set; }
        public IPerfumeRepositorio Perfume { get; set; }
        public IUsuarioAplicacionRepositorio UsuarioAplicacion { get; private set; }
        public ICarroComprasRepositorio CarroCompras { get; private set; }
        public IOrdenRepositorio Orden { get; private set; }
        public IOrdenDetalleRepositorio OrdenDetalle { get; private set; }

        public ICompaniaRepositorio Compania => throw new System.NotImplementedException();

        public IEspecieRepositorio Especie => throw new System.NotImplementedException();

        public UnidadTrabajo(ApplicationDbContext db)
        {
            _db = db;
            Bodega = new BodegaRepositorio(_db); //inicializamos unidad Bodega
            CategoriaFlor = new CategoriaFlorRepositorio(_db); //inicializamos Categoria
            CategoriaPerfume = new CategoriaPerfumeRepositorio(_db); //inicializamos Categoria
            Marca = new MarcaRepositorio(_db); //inicializamos Marca
            Flor = new FlorRepositorio(_db); //inicializamos Producto
            Perfume = new PerfumeRepositorio(_db); //inicializamos Producto
            UsuarioAplicacion = new UsuarioAplicacionRepositorio(_db);
            CarroCompras = new CarroComprasRepositorio(_db);
            Orden = new OrdenRepositorio(_db);
            OrdenDetalle = new OrdenDetalleRepositorio(_db);
        }
        public void Guardar()
        {
            _db.SaveChanges();
        }
        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
