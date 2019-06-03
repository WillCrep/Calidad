using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class Pago
    {
        public int idPag { get; set; }
        [Required(ErrorMessage = "Este campo es Obligatorio")]
        public DateTime fechaPago { get; set; }
        public decimal mora { get; set; }
        [Required(ErrorMessage = "Este campo es Obligatorio")]
        [Range(47.70, 30000, ErrorMessage = "EL monto minimo debe ser 47.70")]
        public decimal total { get; set; }
        public Cliente idClin { get; set; }
        public Prestamo idPres { get; set; }
        public Cuota idCuot { get; set; }
    }
}
