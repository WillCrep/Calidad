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
        public int idPago { get; set; }
        [Required(ErrorMessage = "Este campo es Obligatorio")]
        public DateTime fechaPago { get; set; }
        public decimal mora { get; set; }
        [Required(ErrorMessage = "Este campo es Obligatorio")]
        public decimal total { get; set; }
        public Cliente cliente { get; set; }
        public Prestamo prestamo { get; set; }
        public Cuota cuota { get; set; }
        public DetallePago detallePago { get; set; }

        public Pago GenerarPago(Prestamo prestamo)
        {
            Cuota c = new Cuota();
            foreach (var cuota in prestamo.cuotas)
            {
                if (cuota.p == 1)
                {
                    c = cuota;
                    break;
                }
            }
            Pago p = new Pago();
            p.fechaPago = DateTime.Now.Date;
            p.total = c.cuota;
            p.cuota = c;
            p.prestamo = prestamo;
            p.cliente = prestamo.cliente;
            DetallePago d = new DetallePago();
            p.detallePago = d;
            return p; 
        }

        public Pago GenerarPagoMora (int id, Prestamo prestamo)
        {
            Pago p = new Pago();
            foreach (var cuota in prestamo.cuotas)
            {
                if(cuota.idCuota == id)
                {
                    p.fechaPago = DateTime.Now.Date;
                    p.cuota = cuota;
                    p.prestamo = prestamo;
                    p.cliente = prestamo.cliente;
                    DetallePago d = new DetallePago();
                    d.iCv = cuota.calculoIcv(cuota);
                    d.iM = cuota.calculoDeIm(cuota);
                    p.detallePago = d;
                    p.mora = d.iM + d.iCv;
                    p.total = cuota.cuota + p.mora;
                    break;
                }
            }
            return p;
        }
    }
}
