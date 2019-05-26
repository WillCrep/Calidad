using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class Cliente
    {
        public int idCli { get; set; }
        [Required]
        public String nombres { get; set; }
        [Required]
        public String apellidos { get; set; }
        [Required]
        public String dni { get; set; }
        public String direccion { get; set; }
        [Required]
        public DateTime fechaRegistro { get; set; }
    }
}
