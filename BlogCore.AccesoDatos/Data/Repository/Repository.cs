using BlogCore.AccesoDatos.Data.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        /// <summary>
        /// Contexto tipado como DbContext para mantener este repositorio genérico y desacoplado:
        /// permite inyectar ApplicationDbContext, un DbContext de pruebas u otro derivado.
        /// Cambia a ApplicationDbContext solo si necesitas miembros específicos de esa implementación.
        /// </summary>
        protected readonly DbContext Context;

        /// <summary>
        /// Conjunto de entidades del tipo T gestionado por EF Core.
        /// Representa la colección (tabla) en la base de datos; guarda las entidades que están siendo rastreadas por el Contexto
        /// y se usa para consultar, añadir, actualizar y eliminar registros (LINQ / operaciones CRUD).
        /// Se inicializa con Context.Set<T>() en el constructor y es internal para uso dentro del ensamblado.
        /// </summary>
        internal DbSet<T> dbSet;

        public Repository(DbContext context)
        {
            Context = context;
            this.dbSet = Context.Set<T>();
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(int id)
        {
            return dbSet.FirstOrDefault(e => EF.Property<int>(e, "Id") == id);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string? includeProperties = null)
        {
            // Se crea la consulta IQueryable a partir del DbSet del contexto
            IQueryable<T> query = dbSet;

            // Si se proporciona un filtro, se aplica a la consulta
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Si se incluyen propiedades de navegación, se agregan a la consulta
            if (includeProperties != null) {                 
                foreach (var includeProperty in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            // Si se proporciona una función de ordenación, se aplica a la consulta
            if (orderBy != null)
            {
                // Se ejecuta la consulta ordenada y se devuelve la lista resultante
                return orderBy(query).ToList();
            }

            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            // Se crea la consulta IQueryable a partir del DbSet del contexto
            IQueryable<T> query = dbSet;

            // Si se proporciona un filtro, se aplica a la consulta
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Si se incluyen propiedades de navegación, se agregan a la consulta
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return query.FirstOrDefault();
        }

        public void Remove(int id)
        {
            T? entityToRemove = dbSet.Find(id);
            if (entityToRemove != null)
            {
                dbSet.Remove(entityToRemove);
            }
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }
    }
}
