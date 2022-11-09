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
    public class CategoriaPerfumeController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        public CategoriaPerfumeController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            CategoriaPerfume categoriaperfume = new CategoriaPerfume();
            if (id == null)
            {
                //Esto es para crear una nueva categoria (insert)
                return View(categoriaperfume);
            }
            //actualizamos un registro Categoria existente
            categoriaperfume = _unidadTrabajo.CategoriaPerfume.Obtener(id.GetValueOrDefault());
            if (categoriaperfume == null)
            {
                return NotFound();
            }
            return View(categoriaperfume);
        }
       

         [HttpPost]
 [ValidateAntiForgeryToken]
 public IActionResult Upsert(CategoriaPerfume categoriaperfume)
        {
            if (ModelState.IsValid)
            {
                if (categoriaperfume.id == 0) //nuevo registro
                {
                    TempData["crear"] = "Categoría de perfumes creada correctamente!!";
                    _unidadTrabajo.CategoriaPerfume.Agregar(categoriaperfume);
                }
                else
                {
                    TempData["actualizar"] = "Categoría de perfumes actualizada correctamente!!";
                    _unidadTrabajo.CategoriaPerfume.Actualizar(categoriaperfume);
                }
                _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            return View(categoriaperfume);
        }
        #region API
        [HttpGet]
        public IActionResult ObtenerTodos()
        {
            var todos = _unidadTrabajo.CategoriaPerfume.ObtenerTodos();
            return Json(new { data = todos });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var categoriaperfumeDb = _unidadTrabajo.CategoriaPerfume.Obtener(id);
            if (categoriaperfumeDb == null)
            {
                return Json(new { success = false, message = "Error al borrar la categoría de perfumes " });
            }
            else
                _unidadTrabajo.CategoriaPerfume.Remover(categoriaperfumeDb);
            _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Categoría de perfumes borrada exitosamente" });
        }
        #endregion
    }

}
