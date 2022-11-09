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


    public class FlorController : Controller
    {

        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly IWebHostEnvironment _hostEnvironment;
        public FlorController(IUnidadTrabajo unidadTrabajo,
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
            FlorVM florVM = new FlorVM()
            {
                Flor = new Flor(),
                CategoriaFlorLista =
            _unidadTrabajo.CategoriaFlor.ObtenerTodos().Select(c => new SelectListItem
            {
                Text = c.Nombre,
            Value = c.id.ToString()
 }),

 
 PadreLista =
 _unidadTrabajo.Flor.ObtenerTodos().Select(p => new SelectListItem
 {
     Text = p.Descripcion,
     Value = p.Id.ToString()
 })
 };
 if (id == null)
 {
 // Esto es para Crear nuevo Registro
 return View(florVM);
    }
            // Esto es para Actualizar
     florVM.Flor = _unidadTrabajo.Flor.Obtener(id.GetValueOrDefault());
 if (florVM.Flor == null)
 {
 return NotFound();
}
return View(florVM);



 }
 [HttpPost]


[ValidateAntiForgeryToken]
public IActionResult Upsert(FlorVM florVM)
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
            var uploads = Path.Combine(webRootPath, @"img\flores");
            var extension = Path.GetExtension(files[0].FileName);
             Flor florDB =
             _unidadTrabajo.Flor.Obtener(florVM.Flor.Id);
            if (florDB != null)
               florVM.Flor.ImageUrl = florDB.ImageUrl;
            if (florVM.Flor.ImageUrl != null)
            {
                //Esto es para editar, necesitamos borrar la imagen anterior
                var imagenPath = Path.Combine(webRootPath,
                 florVM.Flor.ImageUrl.TrimStart('\\'));
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
               florVM.Flor.ImageUrl = @"\img\flores\"
             + filename + extension;
        }
        else
        {
            // Si en el Update el usuario no cambia la imagen
            if (florVM.Flor.Id != 0)
            {
                        Flor florDB =
                _unidadTrabajo.Flor.Obtener(florVM.Flor.Id);
                florVM.Flor.ImageUrl = florDB.ImageUrl;
            }
        }
        if (florVM.Flor.Id == 0)
        {
            _unidadTrabajo.Flor.Agregar(florVM.Flor);
        }
        else
        {
            _unidadTrabajo.Flor.Actualizar(florVM.Flor);
        }
        _unidadTrabajo.Guardar();
        return RedirectToAction(nameof(Index));
    }
    else
    {
                florVM.CategoriaFlorLista =
        _unidadTrabajo.CategoriaFlor.ObtenerTodos().Select(c => new SelectListItem
        {
            Text = c.Nombre,
            Value = c.id.ToString()
        });

                florVM.PadreLista = _unidadTrabajo.Flor.ObtenerTodos().Select(p =>
        new SelectListItem
        {
            Text = p.Descripcion,
            Value = p.Id.ToString()
        });
        if (florVM.Flor.Id != 0)
        {
                    florVM.Flor =
            _unidadTrabajo.Flor.Obtener(florVM.Flor.Id);
        }
    }
    return View(florVM.Flor);

}

        public IActionResult ImprimirFlores()
        {
            FlorVM florVM = new FlorVM();
            florVM.FlorLista =
            _unidadTrabajo.Flor.ObtenerTodos(IncluirPropiedades: "CategoriaFlor");
            return new ViewAsPdf("ImprimirFlores", florVM)
            {
                FileName = "ListaFlores" + ".pdf",
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
    _unidadTrabajo.Flor.ObtenerTodos(IncluirPropiedades: "CategoriaFlor");
    return Json(new { data = todos });
}
[HttpDelete]
public IActionResult Delete(int id)
{
    var florDb = _unidadTrabajo.Flor.Obtener(id);
    if (florDb == null)
    {
        return Json(new { success = false, message = "Error al Borrar" });
    }
    // Eliminar la Imagen relacionada al producto
    string webRootPath = _hostEnvironment.WebRootPath;
    var imagenPath = Path.Combine(webRootPath, florDb.ImageUrl.TrimStart('\\'));
    if (System.IO.File.Exists(imagenPath))
    {
        System.IO.File.Delete(imagenPath);
    }
    _unidadTrabajo.Flor.Remover(florDb);
    _unidadTrabajo.Guardar();
    return Json(new { success = true, message = "Flor Borrada Exitosamente" });
}
 #endregion
 }













}
