using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        T Get(int id);

        /// <summary>
        /// Recupera una colección de entidades del tipo T aplicando opcionalmente un filtro,
        /// un orden y la inclusión de propiedades de navegación.
        /// </summary>
        /// <param name="filter">
        /// Expresión booleana opcional para filtrar los elementos (ej. entidad => entidad.Prop == valor).
        /// Se usa para limitar el conjunto devuelto por condiciones específicas sin exponer la
        /// lógica de consulta al llamador.
        /// </param>
        /// <param name="orderBy">
        /// Función opcional que recibe un IQueryable<T> y devuelve un IOrderedQueryable<T> para
        /// aplicar ordenación (ej. q => q.OrderBy(e => e.Prop)). Se usa para controlar el
        /// orden de los resultados sin acoplar la implementación a un criterio fijo.
        /// </param>
        /// <param name="includeProperties">
        /// Cadena opcional con nombres de propiedades de navegación separadas por comas
        /// (ej. "Categoria,Autor"). Se usa para realizar eager loading de relaciones necesarias
        /// en la consulta y evitar múltiples viajes a la base de datos.
        /// </param>
        /// <returns>
        /// IEnumerable<T> con las entidades que cumplen las condiciones. Puede estar vacío si no hay coincidencias.
        /// </returns>
        IEnumerable<T> GetAll(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string? includeProperties = null
        );

        /// <summary>
        /// Recupera la primera entidad que cumple el filtro especificado o el valor por defecto (null)
        /// si no existe ninguna coincidencia. Permite incluir propiedades de navegación.
        /// </summary>
        /// <param name="filter">
        /// Expresión opcional para localizar una única entidad según alguna condición.
        /// Se usa cuando se espera una entidad concreta (por ejemplo, por slug, nombre único, etc.).
        /// </param>
        /// <param name="includeProperties">
        /// Cadena con nombres de propiedades de navegación separadas por comas para incluir datos relacionados
        /// en una sola consulta (eager loading). Útil cuando se necesita acceso inmediato a relaciones
        /// (ej. obtener un post con su autor y comentarios).
        /// </param>
        /// <returns>
        /// La primera entidad que cumple el filtro o null si no existe. Ideal para búsquedas de una sola entidad.
        /// </returns>
        T GetFirstOrDefault(
            Expression<Func<T, bool>>? filter = null,
            string? includeProperties = null
        );

        void Add(T entity);
        void Remove(int id);
        void Remove(T entity);
    }
}
