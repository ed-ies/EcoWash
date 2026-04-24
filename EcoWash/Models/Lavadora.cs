using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoWash.Models
{
    public class Lavadora
    {
        [Key]
        public int Id { get; set; }

        // Tipo de lavadora la equivalencia será la siguiente
        // 0 => Lavadora
        // 1 => Secadora
        [Required, Range(0, 1)]
        public int Tipo { get; set; }

        // Indicador de si esta ocupado o no
        public bool IsOccupied { get; set; } = false;

        // Entidad relacion
        // Tienda en la que se encuentra
        public int TiendaId { get; set; }
        [ForeignKey("TiendaId")]
        public virtual Tienda TiendaAsociada { get; set; }
    }
}
