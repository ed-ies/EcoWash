using EcoWash.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EcoWash.Services
{
    /// <summary>
    /// Proporciona servicios de acceso a datos para la entidad <see cref="Lavadora"/>,
    /// implementando operaciones CRUD sobre la base de datos.
    /// </summary>
    public class ServiceLavadora : IDisposable
    {
        #region Variables_Interfaz
        bool disposed;

        /// <summary>
        /// Inicializa una nueva instancia del servicio <see cref="ServiceLavadora"/>.
        /// </summary>
        public ServiceLavadora()
        {
            disposed = false;
        }
        #endregion


        #region CRUD
        // METODOS CRUD

        /// <summary>
        /// Obtiene la lista completa de lavadoras registradas en el sistema.
        /// </summary>
        /// <returns>
        /// Una tarea asincrónica que devuelve una lista de objetos <see cref="Lavadora"/>
        /// ordenados por su identificador.
        /// </returns>
        public async Task<List<Lavadora>> Listar()
        {
            using (var _context = new EcoWashDbContext())
            {
                return await _context.Lavadoras
                    .AsNoTracking()
                    .OrderBy(u => u.Id)
                    .ToListAsync();
            }
        }

        /// <summary>
        /// Obtiene una lavadora específica a partir de su identificador.
        /// </summary>
        /// <param name="id">
        /// Identificador único de la lavadora que se desea consultar.
        /// </param>
        /// <returns>
        /// Una tarea asincrónica que devuelve la lavadora encontrada,
        /// o <c>null</c> si no existe una lavadora con el identificador indicado.
        /// </returns>
        public async Task<Lavadora?> Listar(int id)
        {
            using (var _context = new EcoWashDbContext())
            {
                return await _context.Lavadoras
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id);
            }
        }

        /// <summary>
        /// Inserta una nueva lavadora en la base de datos.
        /// </summary>
        /// <param name="lavadora">
        /// Objeto <see cref="Lavadora"/> que contiene la información de la nueva lavadora.
        /// </param>
        /// <returns>
        /// Una tarea asincrónica que devuelve la entidad <see cref="Lavadora"/> insertada,
        /// incluyendo su identificador generado.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Se produce cuando el parámetro <paramref name="lavadora"/> es <c>null</c>.
        /// </exception>
        public async Task<Lavadora> Insertar(Lavadora lavadora)
        {
            if (lavadora == null) throw new ArgumentNullException(nameof(lavadora));

            using (var _context = new EcoWashDbContext())
            {
                await _context.Lavadoras.AddAsync(lavadora); // Agrega un nuevo objeto User al DbSet y lo inserta de forma asincrona
                await _context.SaveChangesAsync(); // Ejecuta todas las operaciones pendientes en la base de datos
                return lavadora;
            }
        }

        /// <summary>
        /// Actualiza los datos de una lavadora existente.
        /// </summary>
        /// <param name="lavadora">
        /// Objeto <see cref="Lavadora"/> que contiene los datos actualizados.
        /// </param>
        /// <returns>
        /// Una tarea asincrónica que devuelve <c>true</c> si la actualización se realizó correctamente,
        /// o <c>false</c> si la lavadora no existe.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Se produce cuando el parámetro <paramref name="lavadora"/> es <c>null</c>.
        /// </exception>
        public async Task<bool> Actualizar(Lavadora lavadora)
        {
            if (lavadora == null) throw new ArgumentNullException(nameof(lavadora));

            using (var _context = new EcoWashDbContext())
            {
                var existing = await _context.Lavadoras.FirstOrDefaultAsync(l => l.Id == lavadora.Id);
                if (existing is null) return false;

                // Actualiza campos permitidos
                existing.Tipo = lavadora.Tipo;
                existing.IsOccupied = lavadora.IsOccupied;
                existing.TiendaId = lavadora.TiendaId;


                await _context.SaveChangesAsync();
                return true;
            }
        }

        /// <summary>
        /// Elimina una lavadora de la base de datos a partir de su identificador.
        /// </summary>
        /// <param name="id">
        /// Identificador único de la lavadora que se desea eliminar.
        /// </param>
        /// <returns>
        /// Una tarea asincrónica que devuelve <c>true</c> si la lavadora fue eliminada,
        /// o <c>false</c> si no se encontró la lavadora.
        /// </returns>
        public async Task<bool> Borrar(int id)
        {
            using (var _context = new EcoWashDbContext())
            {
                var entity = await _context.Lavadoras.FirstOrDefaultAsync(l => l.Id == id);
                if (entity is null) return false;

                _context.Lavadoras.Remove(entity); // Marca el objeto entity para ser eliminado en el siguiente SaveChangesAsync()
                await _context.SaveChangesAsync();
                return true;
            }
        }
        #endregion


        #region LiberacionRecursos
        // MÉTODOS DE LIBERACIÓN DE RECURSOS ----------

        /// <summary>
        /// Libera los recursos utilizados por la instancia actual.
        /// </summary>
        public void Dispose()
        {
            Dispose(true); // Método que libera los recursos cuando terminas de usar la clase
            GC.SuppressFinalize(this); // Evita llamar al finalizador dos veces
        }

        /// <summary>
        /// Libera los recursos administrados y no administrados.
        /// </summary>
        /// <param name="disposing">
        /// Indica si el método fue llamado explícitamente desde <see cref="Dispose()"/>.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                //Liberar recursos no manejados como ficheros, conexiones a bd, etc.
            }

            disposed = true;
        }

        /// <summary>
        /// Finalizador que asegura la liberación de recursos si no se llamó a <see cref="Dispose()"/>.
        /// </summary>
        ~ServiceLavadora()
        {
            Dispose(false);
        }
        #endregion
    }
}
