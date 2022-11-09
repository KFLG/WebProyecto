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

    public class MarcaController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        public MarcaController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            Marca marca = new Marca();
            if (id == null)
            {
                //Esto es para crear una nueva marca (insert)
                return View(marca);
            }
            //actualizamos un registro marca existente
            marca = _unidadTrabajo.Marca.Obtener(id.GetValueOrDefault());
            if (marca == null)
            {
                return NotFound();
                
            }
            return View(marca);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Marca marca)
        {
            if (ModelState.IsValid)
            {
                if (marca.id == 0) //nuevo registro
                {
                    _unidadTrabajo.Marca.Agregar(marca);
                }
                else
                {
                    _unidadTrabajo.Marca.Actualizar(marca);
                }
                _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            return View(marca);
        }
        #region API
        [HttpGet]
        public IActionResult ObtenerTodos()
        {
            var todos = _unidadTrabajo.Marca.ObtenerTodos();
            return Json(new { data = todos });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var marcaDb = _unidadTrabajo.Marca.Obtener(id);
            if (marcaDb == null)
            {
                return Json(new { success = false, message = "Error al borrar la marca " });
            }
            else
                _unidadTrabajo.Marca.Remover(marcaDb);
            _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Marca borrada exitosamente" });
        }
        #endregion
    }

}
