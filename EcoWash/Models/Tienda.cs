using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EcoWash.Models
{
    public class Tienda
    {
        [Key]
        public int Id { get; set; }

        // Nombre de la tienda
        [Required, MaxLength(200)]
        public string Nombre { get; set; } = string.Empty;

        // Direccion de la tienda
        [MaxLength(200)]
        public string Direccion { get; set; } = string.Empty;

        // Indicador de si esta activa o no
        public bool IsActive { get; set; } = true;

        // Potencia contratada de la tienda en KWh
        public double Potencia { get; set; }

        // Entidad relacion
        // Lista de lavadoras asociadas a la tienda
        [InverseProperty("TiendaAsociada")]
        public virtual ICollection<Lavadora> Lavadoras { get; set; } = new ObservableCollection<Lavadora>();
    }
}
