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
        public decimal total { get; set; }
        public Cliente idClin { get; set; }
        public Prestamo idPres { get; set; }
        public Cuota idCuot { get; set; }
        public DetPago detPago { get; set; }

        public Pago GenerarPago(Cuota c)
        {
            Pago p = new Pago();
            p.fechaPago = DateTime.Now.Date;
            p.total = c.cuota;
            p.idCuot = c;
            p.idPres = c.prestamo;
            p.idClin = c.prestamo.cliente;
            DetPago d = new DetPago();
            p.detPago = d;
            return p; 
        }

        public Pago GenerarPagoMora (int id, List<Cuota> cuotas)
        {
            Pago p = new Pago();
            foreach (var item in cuotas)
            {
                if(item.idCuo == id)
                {
                    p.fechaPago = DateTime.Now.Date;
                    p.idCuot = item;
                    p.idPres = item.prestamo;
                    p.idClin = item.prestamo.cliente;
                    DetPago d = new DetPago();
                    d.iCv = item.calculoIcv(item);
                    d.iM = item.calculoDeIm(item);
                    p.detPago = d;
                    p.mora = d.iM + d.iCv;
                    p.total = item.cuota + p.mora;
                    break;
                }
            }
            return p;
        }
    }
}
