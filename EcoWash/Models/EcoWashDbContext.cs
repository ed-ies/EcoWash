using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EcoWash.Models
{
    public partial class EcoWashDbContext : DbContext
    {
        // MODELOS
        public virtual DbSet<Tienda> Tiendas { get; set; }
        public virtual DbSet<Lavadora> Lavadoras { get; set; }

        public EcoWashDbContext() { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EcoWash");
        }
    }
}
