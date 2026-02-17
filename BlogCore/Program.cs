using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BlogCore.AccesoDatos.Data;
using BlogCore.AccesoDatos.Data.Repository;
using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models;
using BlogCore.AccesoDatos.Inicializador;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("ConexionSQL");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI();
builder.Services.AddControllersWithViews();

// Agregar contenedor de trabajo al contenedor IoC de inyecci�n de dependencias
// Cada vez que se solicite IContenedorTrabajo, se crear� una nueva instancia de ContenedorTrabajo
// El AddScoped significa que se crear� una nueva instancia de ContenedorTrabajo para cada solicitud HTTP, lo que es adecuado para la mayor�a de los casos de uso en aplicaciones web
// Se reutilizar� la misma instancia de ContenedorTrabajo durante toda la solicitud HTTP, lo que garantiza que todas las operaciones de base de datos realizadas durante esa solicitud se realicen en el mismo contexto de base de datos
// Se destruir� la instancia de ContenedorTrabajo al finalizar la solicitud HTTP, lo que ayuda a liberar recursos y evitar problemas de memoria
builder.Services.AddScoped<IContenedorTrabajo, ContenedorTrabajo>();

// Agregar el inicializador de la base de datos al contenedor IoC de inyecci�n de dependencias
builder.Services.AddScoped<IInicializadorBD, InicializadorBD>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

//// Metodo que ejecuta la siembra de datos
//SiembraDatos();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Cliente}/{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

void SiembraDatos()
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var inicializadorBD = services.GetRequiredService<IInicializadorBD>();
        inicializadorBD.Inicializar();
    }
}
