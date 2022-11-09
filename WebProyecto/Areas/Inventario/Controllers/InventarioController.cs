﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using WebProyecto.AccesoDatos.Repositorio.IRepositorio;
using WebProyecto.Data;
using WebProyecto.Modelos;
using WebProyecto.Modelos.ViewModels;

namespace WebProyecto.Areas.Inventario.Controllers
{
    [Area("Inventario")]
    public class InventarioController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IUnidadTrabajo _unidadTrabajo;

        [BindProperty]
        public InventarioVM inventarioVM { get; set; }

        public InventarioController(ApplicationDbContext db, IUnidadTrabajo unidadTrabajo)
        {
            _db = db;
            _unidadTrabajo = unidadTrabajo;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult NuevoInventario(int? inventarioId)
        {
            inventarioVM = new InventarioVM();
            inventarioVM.BodegaLista = _db.Bodegas.ToList().Select(b =>
                   new SelectListItem
                   {
                       Text = b.Nombre,
                       Value = b.id.ToString()
                   });
            inventarioVM.PerfumeLista = _db.Perfumes.ToList().Select(p =>
                                new SelectListItem
                                {
                                    Text = p.Descripcion,
                                    Value = p.Id.ToString()
                                });
            inventarioVM.InventarioDetalles = new List<InventarioDetalle>();
            if (inventarioId != null)
            {
                inventarioVM.Inventario =
                    _db.Inventario.SingleOrDefault(i => i.Id == inventarioId);
                inventarioVM.InventarioDetalles =
                    _db.InventarioDetalle.Include(p => p.Perfume).Include(m =>
                             m.Perfume.Marca).
                             Where(d => d.InventarioId == inventarioId).ToList();
            }
            return View(inventarioVM);
        }
        [HttpPost]
        public IActionResult AgregarProductoPost(int perfume, int cantidad, int inventarioId)
        {
            inventarioVM.Inventario.Id = inventarioId;
            if (inventarioVM.Inventario.Id == 0) // Graba el Registro en inventario
            {
                //se incluye el registro en la tabla Inventario, si no existe el mismo.
                inventarioVM.Inventario.Estado = false;
                inventarioVM.Inventario.FechaInicial = DateTime.Now;
                // Capturar el Id del usuario conectado
                var claimIdentidad = (ClaimsIdentity)User.Identity;
                var claim = claimIdentidad.FindFirst(ClaimTypes.NameIdentifier);
                inventarioVM.Inventario.UsuarioAplicacionId = claim.Value;
                _db.Inventario.Add(inventarioVM.Inventario);
                _db.SaveChanges();
            }
            else
            {
                //si se recibe inventarioId =! 0 entonces se procede a buscar el registro
                //devolviendo el id del inventario que ya se encuentra creado
                inventarioVM.Inventario = _db.Inventario.SingleOrDefault(i => i.Id == inventarioId);
            }
            //Creamos una instancia de bodegaProducto para trabajar con los datos que se tiene
            //actualmente (BodegaProducto y su relación con Inventario y producto)
            var bodegaProducto = _db.BodegaProductos.Include(b =>
                                b.Perfume).FirstOrDefault(b => b.PerfumeId == perfume &&
                                b.BodegaId == inventarioVM.Inventario.BodegaId);

            //Se establece la relacion entre el inventario y el detalle de inventario
            //relacionando Inventario.Detalle con el producto que se debe ingresar
            //si el detalle existe para ese inventario y para ese producto
            var detalle = _db.InventarioDetalle.Include(p =>
                           p.Perfume).FirstOrDefault(d => d.PerfumeId == perfume &&
                           d.InventarioId == inventarioVM.Inventario.Id);
            //si no existe el detalle para ese producto en ese inventario 
            if (detalle == null)
            {
                //actualiza el inventario para ese producto en ese inventario
                inventarioVM.InventarioDetalle = new InventarioDetalle();
                inventarioVM.InventarioDetalle.PerfumeId = perfume;
                inventarioVM.InventarioDetalle.InventarioId = inventarioVM.Inventario.Id;
                //Si hay registro de bodegaProducto (stock en bodegaProducto)
                if (bodegaProducto != null)
                {
                    //le asignamos en stock anterior lo que hay actualmente en cantidad
                    //para agregar la nueva cantidad
                    inventarioVM.InventarioDetalle.StockAnterior = bodegaProducto.Cantidad;
                }
                else
                {
                    //si no existe entonces dejar el stock anterior en cero
                    inventarioVM.InventarioDetalle.StockAnterior = 0;
                }
                inventarioVM.InventarioDetalle.Cantidad = cantidad;
                _db.InventarioDetalle.Add(inventarioVM.InventarioDetalle);
                _db.SaveChanges();
            }
            else
            {
                //si el detalle ya existe se aumenta la cantidad existente
                detalle.Cantidad += cantidad;
                _db.SaveChanges();
            }
            return RedirectToAction("NuevoInventario", new { inventarioId = inventarioVM.Inventario.Id });
        }
        public IActionResult Mas(int Id)
        {
            inventarioVM = new InventarioVM();
            var detalle = _db.InventarioDetalle.FirstOrDefault(d => d.Id == Id);
            inventarioVM.Inventario =
                _db.Inventario.FirstOrDefault(i => i.Id == detalle.InventarioId);

            detalle.Cantidad += 1;
            _db.SaveChanges();
            return RedirectToAction("NuevoInventario",
                    new { inventarioId = inventarioVM.Inventario.Id });
        }
        public IActionResult Menos(int Id)
        {
            inventarioVM = new InventarioVM();
            var detalle = _db.InventarioDetalle.FirstOrDefault(d => d.Id == Id);
            inventarioVM.Inventario = _db.Inventario.FirstOrDefault(i => i.Id == detalle.InventarioId);
            if (detalle.Cantidad == 1)
            {
                _db.InventarioDetalle.Remove(detalle);
                _db.SaveChanges();
            }
            else
            {
                detalle.Cantidad -= 1;
                _db.SaveChanges();
            }
            return RedirectToAction("NuevoInventario", new { inventarioId = inventarioVM.Inventario.Id });
        }
        public IActionResult GenerarStock(int Id)
        {
            var inventario = _db.Inventario.FirstOrDefault(i => i.Id == Id);
            //filtra los detalles de la entrada de inventario dado un id 
            //en la tabla InventarioDetalle que viene de Inventario
            var detalleLista = _db.InventarioDetalle.Where(d => d.InventarioId == Id);
            foreach (var item in detalleLista)
            {
                var bodegaProductos = _db.BodegaProductos.Include(p
                           => p.Perfume).FirstOrDefault(b
                           => b.PerfumeId == item.PerfumeId
                           && b.BodegaId == inventario.BodegaId);
                if (bodegaProductos != null)
                {
                    bodegaProductos.Cantidad += item.Cantidad;
                }
                else
                {
                    bodegaProductos = new BodegaProducto();
                    bodegaProductos.BodegaId = inventario.BodegaId;
                    bodegaProductos.PerfumeId = item.PerfumeId;
                    bodegaProductos.Cantidad = item.Cantidad;
                    _db.BodegaProductos.Add(bodegaProductos);
                }
            }
            // Actualizar la Cabecera de Inventario
            inventario.Estado = true;
            inventario.FechaFinal = DateTime.Now;
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Historial()
        {
            return View();
        }
        public IActionResult DetalleHistorial(int id)
        {
            var detalleHistorial =
                _db.InventarioDetalle.Include(p => p.Perfume)
                .Include(m => m.Perfume.Marca).Where(d => d.InventarioId == id);


            var aux = (from inventario in _db.Inventario
                       join inventariodetalle in _db.InventarioDetalle
                       on inventario.Id equals id
                       join bodega in _db.Bodegas
                       on inventario.BodegaId equals bodega.id
                       join usuarios in _db.UsuarioAplicacion
                       on inventario.UsuarioAplicacionId equals usuarios.Id
                       select new { inventario, inventariodetalle, bodega, usuarios }).ToList();

            ViewBag.Bodega = aux[0].bodega.Nombre;
            ViewBag.Usuario = aux[0].usuarios.Nombres + " " + aux[0].usuarios.Apellidos;
            ViewBag.FechaInicial = aux[0].inventario.FechaInicial;
            ViewBag.FechaFinal = aux[0].inventario.FechaFinal;
            return View(detalleHistorial);
        }
        [HttpPost]
        public object ImprimirInventario(string info)
        {
            inventarioVM = new InventarioVM();
            if (info == null)
            {
                inventarioVM.InventarioDetalles =
                  _db.InventarioDetalle.Include(p => p.Perfume)
                  .Include(m => m.Perfume.Marca)
                  .Include(c => c.Perfume.CategoriaPerfume)
                  .OrderBy(c => c.Perfume.CategoriaPerfume.Nombre).ToList();
            }
            else
            {
                inventarioVM.InventarioDetalles =
               _db.InventarioDetalle.Include(p => p.Perfume).Include(m =>
                m.Perfume.Marca).Include(c => c.Perfume.CategoriaPerfume)
                .Where(p => p.Perfume.Descripcion.Contains(info))
               .OrderBy(c => c.Perfume.CategoriaPerfume.Nombre).ToList();
                if (inventarioVM.InventarioDetalles == null)
                {
                    inventarioVM.InventarioDetalles =
                   _db.InventarioDetalle.Include(p => p.Perfume).Include(m =>
                    m.Perfume.Marca).Include(c => c.Perfume.CategoriaPerfume)
                    .Where(p => p.Perfume.CategoriaPerfume.Nombre.Contains(info))
                   .OrderBy(c => c.Perfume.CategoriaPerfume.Nombre).ToList();
                }
            }

            var compania = _unidadTrabajo.Compania.ObtenerTodos();
            foreach (var item in compania)
            {
                HttpContext.Session.SetString("Logo", item.LogoUrl);
                HttpContext.Session.SetString("Nombre", item.Nombre);
                HttpContext.Session.SetString("Telefono", item.Telefono);
                HttpContext.Session.SetString("Descripcion", item.Descripcion);
                HttpContext.Session.SetString("Pais", item.Pais);
                HttpContext.Session.SetString("Ciudad", item.Ciudad);
            }

            return new ViewAsPdf("ImprimirInventario", inventarioVM.InventarioDetalles)
            {
                FileName = "ListaInventario" + ".pdf",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = "--page-offset 9 --footer-center [page] --footer-font-size 12"
            };
        }
        #region API
        [HttpGet]
        public IActionResult ObtenerTodos()
        {
            var ListaBodegaProductos = _db.BodegaProductos.Include(b =>
                        b.Bodega).Include(p => p.Perfume).ToList();
            return Json(new { data = ListaBodegaProductos });
        }
        [HttpGet]
        public IActionResult ObtenerHistorial()
        {
            var todos = _db.Inventario.Include(b => b.Bodega)
                 .Include(u => u.UsuarioAplicacion)
                 .Where(i => i.Estado == true).ToList();
            return Json(new { data = todos });
        }
        #endregion
    }

}
//https://csharp.hotexamples.com/es/examples/Rotativa/ViewAsPdf/-/php-viewaspdf-class-examples.html

