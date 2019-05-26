using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class Prestamo
    {
        public int idPres { get; set; }
        [Display(Name = "Fecha de Inicio")]
        [Required(ErrorMessage = "Este campo es Obligatorio")]
        public DateTime fechaIni { get; set; }
        [Display(Name = "Fecha de Termino")]
        [Required(ErrorMessage = "Este campo es Obligatorio")]
        public DateTime fechaTerm { get; set; }
        [Display(Name = "Monto a Prestar")]
        [Range(1000, 30000, ErrorMessage = "EL monto debe estar entre 1000 y 30000")]
        [Required(ErrorMessage = "Este campo es Obligatorio")]
        public decimal monto { get; set; }
        [Display(Name = "TEA")]
        [Range(0.10, 0.21, ErrorMessage = "La TEA permitida esta entre 0.10 y 0.20")]
        [Required(ErrorMessage = "Este campo es Obligatorio")]
        public decimal tea { get; set; }
        [Display(Name = "Cantidad de Cuotas")]
        [Range(3, 24, ErrorMessage = "Los periodos permitidos estan entre 3 y 24")]
        [Required(ErrorMessage = "Este campo es Obligatorio")]
        public int cantCu { get; set; }
        public Cliente cliente { get; set; }
    }
}
