using EcoWash.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoWash.Services
{
    public class ServiceTienda : IDisposable
    {
        #region Variables_Interfaz
        bool disposed;
        public ServiceTienda()
        {
            disposed = false;
        }
        #endregion


        #region CRUD
        // METODOS CRUD

        // LISTAR
        public async Task<List<Tienda>> Listar()
        {
            using (var _context = new EcoWashDbContext())
            {
                return await _context.Tiendas
                    .Include(t => t.Lavadoras)
                    .AsNoTracking()
                    .OrderBy(t => t.Id)
                    .ToListAsync();
            }
        }

        // LISTAR ID
        public async Task<Tienda?> Listar(int id)
        {
            using (var _context = new EcoWashDbContext())
            {
                return await _context.Tiendas
                .Include(t => t.Lavadoras)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
            }
        }

        // INSERTAR
        public async Task<Tienda> Insertar(Tienda tienda)
        {
            if (tienda == null) throw new ArgumentNullException(nameof(tienda));

            using (var _context = new EcoWashDbContext())
            {
                await _context.Tiendas.AddAsync(tienda); // Agrega un nuevo objeto User al DbSet y lo inserta de forma asincrona
                await _context.SaveChangesAsync(); // Ejecuta todas las operaciones pendientes en la base de datos
                return tienda;
            }
        }

        // ACTUALIZAR
        public async Task<bool> Actualizar(Tienda tienda)
        {
            if (tienda == null) throw new ArgumentNullException(nameof(tienda));

            using (var _context = new EcoWashDbContext())
            {
                var existing = await _context.Tiendas.FirstOrDefaultAsync(t => t.Id == tienda.Id);
                if (existing is null) return false;

                // Actualiza campos permitidos
                existing.Nombre = tienda.Nombre;
                existing.Direccion = tienda.Direccion;
                existing.IsActive = tienda.IsActive;
                existing.Potencia = tienda.Potencia;


                await _context.SaveChangesAsync();
                return true;
            }
        }

        // BORRAR
        public async Task<bool> Borrar(int id)
        {
            using (var _context = new EcoWashDbContext())
            {
                var entity = await _context.Tiendas.FirstOrDefaultAsync(t => t.Id == id);
                if (entity is null) return false;

                _context.Tiendas.Remove(entity); // Marca el objeto entity para ser eliminado en el siguiente SaveChangesAsync()
                await _context.SaveChangesAsync();
                return true;
            }
        }
        #endregion


        #region LiberacionRecursos
        // MÉTODOS DE LIBERACIÓN DE RECURSOS ----------

        // Método público de IDisposable
        public void Dispose()
        {
            Dispose(true); // Método que libera los recursos cuando terminas de usar la clase
            GC.SuppressFinalize(this); // Evita llamar al finalizador dos veces
        }

        // Método protegido: libera los recursos
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

        // Finalizador (por si se olvida llamar a Dispose), tambien conocido como Destructor
        ~ServiceTienda()
        {
            Dispose(false);
        }
        #endregion
    }
}
