using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Area("Admin")]
    public class CategoriasController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;

        public CategoriasController(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Categoria categoria)
        {
            // ModelState.IsValid se utiliza para verificar si los datos enviados por el formulario cumplen con las reglas de validación definidas en el modelo Categoria. Si los datos son válidos, se procede a agregar la nueva categoría a la base de datos utilizando el contenedor de trabajo (_contenedorTrabajo) y luego se guarda. Finalmente, se redirige al usuario a la acción Index para mostrar la lista actualizada de categorías. Si los datos no son válidos, se devuelve la vista con el modelo para que el usuario pueda corregir los errores.
            if (ModelState.IsValid)
            {
                // Logica para agregar la nueva categoría a la base de datos
                _contenedorTrabajo.Categoria.Add(categoria);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {

            Categoria categoria = new Categoria();
            categoria = _contenedorTrabajo.Categoria.Get(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                // Logica para editar la categoría
                _contenedorTrabajo.Categoria.Update(categoria);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        #region Llamadas a la API
        public IActionResult GetAll()
        {
            var lista = _contenedorTrabajo.Categoria.GetAll();
            return Json(new { data = lista });
        }


        
        
        
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _contenedorTrabajo.Categoria.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error al eliminar" });
            }

            _contenedorTrabajo.Categoria.Remove(objFromDb);
            _contenedorTrabajo.Save();
            return Json(new { success = true, message = "Eliminado correctamente" });
        }

        #endregion
    }
}
