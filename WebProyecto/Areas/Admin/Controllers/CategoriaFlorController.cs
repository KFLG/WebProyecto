using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebProyecto.AccesoDatos.Repositorio.IRepositorio;
using WebProyecto.Modelos;
using WebProyecto.Utilities;

namespace WebProyecto.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin)]
    public class CategoriaFlorController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        public CategoriaFlorController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            CategoriaFlor categoriaflor = new CategoriaFlor();
            if (id == null)
            {
                //Esto es para crear una nueva categoria (insert)
                return View(categoriaflor);
            }
            //actualizamos un registro Categoria existente
            categoriaflor = _unidadTrabajo.CategoriaFlor.Obtener(id.GetValueOrDefault());
            if (categoriaflor == null)
            {
                return NotFound();
            }
            return View(categoriaflor);
        }
       

         [HttpPost]
 [ValidateAntiForgeryToken]
 public IActionResult Upsert(CategoriaFlor categoriaflor)
        {
            if (ModelState.IsValid)
            {
                if (categoriaflor.id == 0) //nuevo registro
                {
                    TempData["crear"] = "Categoría de flores creada correctamente!!";
                    _unidadTrabajo.CategoriaFlor.Agregar(categoriaflor);
                }
                else
                {
                    TempData["actualizar"] = "Categoría de flores actualizada correctamente!!";
                    _unidadTrabajo.CategoriaFlor.Actualizar(categoriaflor);
                }
                _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            return View(categoriaflor);
        }
        #region API
        [HttpGet]
        public IActionResult ObtenerTodos()
        {
            var todos = _unidadTrabajo.CategoriaFlor.ObtenerTodos();
            return Json(new { data = todos });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var categorflorDb = _unidadTrabajo.CategoriaFlor.Obtener(id);
            if (categorflorDb == null)
            {
                return Json(new { success = false, message = "Error al borrar la categoría de flores " });
            }
            else
                _unidadTrabajo.CategoriaFlor.Remover(categorflorDb);
            _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Categoría de flores borrada exitosamente" });
        }
        #endregion
    }

}
