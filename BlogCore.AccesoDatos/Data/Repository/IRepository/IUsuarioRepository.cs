using BlogCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repository.IRepository
{
    public interface IUsuarioRepository
    {
        ApplicationUser ObtenerUsuario(string idUsuario);
        IEnumerable<ApplicationUser> ObtenerTodos(string idUsuarioActual);
        void BloquearUsuario(string idUsuario);
        void DesbloquearUsuario(string idUsuario);
    }
}
