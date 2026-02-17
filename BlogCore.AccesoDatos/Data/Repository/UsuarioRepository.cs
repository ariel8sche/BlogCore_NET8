using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;

        public UsuarioRepository(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _db = db;
        }

        public IEnumerable<ApplicationUser> ObtenerTodos(string idUsuarioActual)
        {
            return _db.Users.Where(u => u.Id != idUsuarioActual).ToList();
        }

        public ApplicationUser ObtenerUsuario(string idUsuario)
        {
            return _db.Users.FirstOrDefault(u => u.Id == idUsuario);
        }

        public void BloquearUsuario(string idUsuario) 
        {
            var usuario = _db.Users.FirstOrDefault(u => u.Id == idUsuario);
            if (usuario != null)
            {
                usuario.LockoutEnd = DateTime.Now.AddYears(100); // Bloquea el usuario por 100 años
                _db.SaveChanges();
            }
        }
        public void DesbloquearUsuario(string idUsuario)
        {
            var usuario = _db.Users.FirstOrDefault(u => u.Id == idUsuario);
            if (usuario != null)
            {
                usuario.LockoutEnd = DateTime.Now; // Bloquea el usuario por 100 años
                _db.SaveChanges();
            }
        }
    }
}
