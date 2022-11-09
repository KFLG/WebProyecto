using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebProyecto.AccesoDatos.Repositorio.IRepositorio;
using WebProyecto.Modelos;
using WebProyecto.Modelos.ViewModels;
using WebProyecto.Utilities;

namespace WebProyecto.Areas.Admin.Controllers
{

    [Area("Admin")]


    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Inventario)]


    public class PerfumeController : Controller
    {

        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly IWebHostEnvironment _hostEnvironment;
        public PerfumeController(IUnidadTrabajo unidadTrabajo,
        IWebHostEnvironment hostEnvironment)
        {
            _unidadTrabajo = unidadTrabajo;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            PerfumeVM perfumeVM = new PerfumeVM()
            {
                Perfume = new Perfume(),
                CategoriaPerfumeLista =
            _unidadTrabajo.CategoriaPerfume.ObtenerTodos().Select(c => new SelectListItem
            {
                Text = c.Nombre,
            Value = c.id.ToString()
 }),
 MarcaLista = _unidadTrabajo.Marca.ObtenerTodos().Select(m => new SelectListItem
 {
     Text = m.Nombre,
     Value = m.id.ToString()
 }),
 PadreLista =
 _unidadTrabajo.Perfume.ObtenerTodos().Select(p => new SelectListItem
 {
     Text = p.Descripcion,
     Value = p.Id.ToString()
 })
 };
 if (id == null)
 {
 // Esto es para Crear nuevo Registro
 return View(perfumeVM);
    }
            // Esto es para Actualizar
     perfumeVM.Perfume = _unidadTrabajo.Perfume.Obtener(id.GetValueOrDefault());
 if (perfumeVM.Perfume == null)
 {
 return NotFound();
}
return View(perfumeVM);



 }
 [HttpPost]


[ValidateAntiForgeryToken]
public IActionResult Upsert(PerfumeVM perfumeVM)
{
    if (ModelState.IsValid)
    {
        // Cargar Imagenes
        string webRootPath = _hostEnvironment.WebRootPath;
        var files = HttpContext.Request.Form.Files;
        if (files.Count > 0)
        {
            /*
            * El método Guid (Identificador único global) puede crear nuevos objetos 
           * como identificadores, mediante la propiedad NewGuid 
            * GUID (o UUID) es un acrónimo de 'Identificador global único' (o 
           'Identificador universal único'). 
            * Es un número entero de 128 bits que se utiliza para identificar recursos.
            */
            string filename = Guid.NewGuid().ToString();
            var uploads = Path.Combine(webRootPath, @"img\perfumes");
            var extension = Path.GetExtension(files[0].FileName);
            Perfume perfumeDB =
             _unidadTrabajo.Perfume.Obtener(perfumeVM.Perfume.Id);
            if (perfumeDB != null)
                perfumeVM.Perfume.ImageUrl = perfumeDB.ImageUrl;
            if (perfumeVM.Perfume.ImageUrl != null)
            {
                //Esto es para editar, necesitamos borrar la imagen anterior
                var imagenPath = Path.Combine(webRootPath,
                 perfumeVM.Perfume.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(imagenPath))
                {
                    System.IO.File.Delete(imagenPath);
                }
            }
            using (var filesStreams = new FileStream(Path.Combine(uploads,
            filename + extension), FileMode.Create))
            {
                files[0].CopyTo(filesStreams);
            }
                    perfumeVM.Perfume.ImageUrl = @"\img\perfumes\"
             + filename + extension;
        }
        else
        {
            // Si en el Update el usuario no cambia la imagen
            if (perfumeVM.Perfume.Id != 0)
            {
                        Perfume perfumeDB =
                _unidadTrabajo.Perfume.Obtener(perfumeVM.Perfume.Id);
                        perfumeVM.Perfume.ImageUrl = perfumeDB.ImageUrl;
            }
        }
        if (perfumeVM.Perfume.Id == 0)
        {
            _unidadTrabajo.Perfume.Agregar(perfumeVM.Perfume);
        }
        else
        {
            _unidadTrabajo.Perfume.Actualizar(perfumeVM.Perfume);
        }
        _unidadTrabajo.Guardar();
        return RedirectToAction(nameof(Index));
    }
    else
    {
                perfumeVM.CategoriaPerfumeLista =
        _unidadTrabajo.CategoriaPerfume.ObtenerTodos().Select(c => new SelectListItem
        {
            Text = c.Nombre,
            Value = c.id.ToString()
        });
                perfumeVM.MarcaLista = _unidadTrabajo.Marca.ObtenerTodos().Select(m =>
        new SelectListItem
        {
            Text = m.Nombre,
            Value = m.id.ToString()
        });
                perfumeVM.PadreLista = _unidadTrabajo.Perfume.ObtenerTodos().Select(p =>
        new SelectListItem
        {
            Text = p.Descripcion,
            Value = p.Id.ToString()
        });
        if (perfumeVM.Perfume.Id != 0)
        {
                    perfumeVM.Perfume =
            _unidadTrabajo.Perfume.Obtener(perfumeVM.Perfume.Id);
        }
    }
    return View(perfumeVM.Perfume);

}


        public IActionResult ImprimirProductos()
        {
            PerfumeVM perfumeVM = new PerfumeVM();
            perfumeVM.PerfumeLista =
            _unidadTrabajo.Perfume.ObtenerTodos(IncluirPropiedades: "CategoriaPerfume,Marca");
            return new ViewAsPdf("ImprimirPerfumes", perfumeVM)
            {
                FileName = "ListaPerfumes" + ".pdf",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = "--page-offset 9 --footer-center [page] --footer-font-size 12"
            };
        }


        #region API
        [HttpGet]
public IActionResult ObtenerTodos()
{
    var todos =
    _unidadTrabajo.Perfume.ObtenerTodos(IncluirPropiedades: "CategoriaPerfume,Marca");
    return Json(new { data = todos });
}
[HttpDelete]
public IActionResult Delete(int id)
{
    var perfumeDb = _unidadTrabajo.Perfume.Obtener(id);
    if (perfumeDb == null)
    {
        return Json(new { success = false, message = "Error al Borrar" });
    }
    // Eliminar la Imagen relacionada al producto
    string webRootPath = _hostEnvironment.WebRootPath;
    var imagenPath = Path.Combine(webRootPath, perfumeDb.ImageUrl.TrimStart('\\'));
    if (System.IO.File.Exists(imagenPath))
    {
        System.IO.File.Delete(imagenPath);
    }
    _unidadTrabajo.Perfume.Remover(perfumeDb);
    _unidadTrabajo.Guardar();
    return Json(new { success = true, message = "Perfume Borrado Exitosamente" });
}
 #endregion
 }













}
