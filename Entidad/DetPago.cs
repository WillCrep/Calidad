using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class DetPago
    {
        public int idDetPag { get; set; }
        public decimal efectivo { get; set; }
        public decimal vuelto { get; set; }
        public decimal iM { get; set; }
        public decimal iCv { get; set; }
    }
}
