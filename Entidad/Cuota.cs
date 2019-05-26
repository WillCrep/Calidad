using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class Cuota
    {
        public int idCuo { get; set; }
        [Required(ErrorMessage = "Este campo es Obligatorio")]
        public int periodo { get; set; }
        [Required(ErrorMessage = "Este campo es Obligatorio")]
        public decimal saldo { get; set; }
        [Required(ErrorMessage = "Este campo es Obligatorio")]
        public decimal amortizacion { get; set; }
        [Required(ErrorMessage = "Este campo es Obligatorio")]
        public decimal interes { get; set; }
        [Required(ErrorMessage = "Este campo es Obligatorio")]
        public decimal cuota { get; set; }
        public Boolean estado { get; set; }
        public DateTime fechPa { get; set; }
        public Prestamo prestamo { get; set; }
    }
}
