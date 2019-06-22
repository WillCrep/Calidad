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
        public int idCuota { get; set; }
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
        public DateTime fechaPago { get; set; }
        public int p { get; set; }

        //METODOS


        #region calculoDeIm
        public decimal calculoDeIm(Cuota c)
        {
            decimal im = 0;
            double dias = c.conteoDeDias(c);
            im = ((decimal)Math.Pow(2.991, dias / 360) - 1) * c.cuota;
            return im;
        }
        #endregion calculoDeIm

        #region calculoDeIcv
        public decimal calculoIcv (Cuota c)
        {
            decimal icv = 0;
            double dias = c.conteoDeDias(c);
            icv = ((decimal)Math.Pow(1.4, (dias / 360)) - 1) * c.cuota;
            return icv;
        }
        #endregion calculoDeIcv

        #region conteoDeDias
        public Double conteoDeDias (Cuota cuota)
        {
            double dias = 0;
            TimeSpan d = DateTime.Now.Date - cuota.fechaPago.Date;
            dias = d.Days;
            return dias;
        }
        #endregion conteoDeDias
    }
}
