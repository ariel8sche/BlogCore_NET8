using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BlogCore.Models;
using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models.ViewModels;

namespace BlogCore.Areas.Cliente.Controllers;

[Area("Cliente")]
public class HomeController : Controller
{
    
    private readonly IContenedorTrabajo _contenedorTrabajo;

    public HomeController(IContenedorTrabajo contenedorTrabajo)
    {
        _contenedorTrabajo = contenedorTrabajo;
    }

    // Primera versión del método Index, sin paginación ni búsqueda
    //[HttpGet]
    //public IActionResult Index()
    //{
    //    HomeVM homeVM = new HomeVM()
    //    {
    //        Sliders = _contenedorTrabajo.Slider.GetAll(),
    //        ListArticulos = _contenedorTrabajo.Articulo.GetAll()
    //    };

    //    ViewBag.IsHome = true;

    //    return View(homeVM);
    //}

    // Segunda versión del método Index, con paginación y búsqueda
    [HttpGet]
    public IActionResult Index(int page = 1, int pageSize = 6)
    {
        // Obtener todos los artículos como una consulta IQueryable
        var articulos = _contenedorTrabajo.Articulo.AsQueryable();

        // Paginar los resultados
        var pagedArticulos = articulos.Skip((page - 1) * pageSize).Take(pageSize);

        HomeVM homeVM = new HomeVM()
        {
            Sliders = _contenedorTrabajo.Slider.GetAll(),
            ListArticulos = pagedArticulos.ToList(),
            PageIndex = page,
            TotalPages = (int)Math.Ceiling(articulos.Count() / (double)pageSize)
        };

        // Esta variable se puede usar en la vista para mostrar u ocultar elementos específicos de la página de inicio
        ViewBag.IsHome = true;

        return View(homeVM);
    }

    [HttpGet]
    public IActionResult Detalle(int id)
    {
        var articulo = _contenedorTrabajo.Articulo.Get(id);
        return View(articulo);
    }




    [HttpGet]
    public IActionResult ResultadoBusqueda(string searchString, int page = 1, int pageSize = 3)
    {
        var articulos = _contenedorTrabajo.Articulo.AsQueryable();

        // Filtrar por el término de búsqueda
        if (!string.IsNullOrEmpty(searchString))
        {
            articulos = articulos.Where(e => e.Nombre.Contains(searchString));
        }

        // Paginar los resultados
        var pagedArticulos = articulos.Skip((page - 1) * pageSize).Take(pageSize);

        // Crear el modelo de vista para pasar a la vista
        var model = new ListaPaginada<Articulo>(pagedArticulos.ToList(), 
            articulos.Count(), page, pageSize, searchString);

        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
