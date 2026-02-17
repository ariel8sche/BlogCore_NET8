using BlogCore.AccesoDatos.Data;
using BlogCore.Models;
using BlogCore.Utilidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Inicializador
{
    public class InicializadorBD : IInicializadorBD
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public InicializadorBD(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Inicializar()
        {
            try
            {
                if (_db.Database.GetAppliedMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception)
            {
            }
            if (_db.Roles.Any(ro => ro.Name == CNT.Administrador)) return;

            // Creacion de los roles
            _roleManager.CreateAsync(new IdentityRole(CNT.Administrador)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(CNT.Registrado)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(CNT.Cliente)).GetAwaiter().GetResult();

            // Creacion del usuario administrador o inicial
            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "arielesd@gmail.com",
                Email = "arielesd@gmail.com",
                EmailConfirmed = true,
                Nombre = "Ariel Eduardo",
                Direccion = "Calle Falsa 123",
                Ciudad = "Springfield",
                Pais = "USA",
            }, "Admin123*").GetAwaiter().GetResult();

            var usuario = _userManager.FindByEmailAsync("arielesd@gmail.com").GetAwaiter().GetResult();

            if (usuario != null)
            {
                _userManager.AddToRoleAsync(usuario, CNT.Administrador).GetAwaiter().GetResult();
            }
        }
    }
}
