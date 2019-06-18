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
        //ENTIDADES
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

        //METODOS

        #region calculoTasaEfectivaMensual
        public decimal calcularTasaEfectivaMensual(Prestamo prestamo)
        {
            decimal i = (decimal)Math.Pow((1 + (double)prestamo.tea), 0.083f) - 1;
            return Math.Round(i, 2, MidpointRounding.AwayFromZero);
        }
        #endregion calculoTasaEfectivaMensual

        #region calculoCuotaFijaMensual
        public decimal calcularCuotaFijaMensual(Prestamo prestamo)
        {
            decimal i = calcularTasaEfectivaMensual(prestamo);
            decimal a = prestamo.monto / ((1 - (decimal)Math.Pow(1 + (double)i, -prestamo.cantCu)) / i);
            return Math.Round(a, 2, MidpointRounding.AwayFromZero);
        }
        #endregion calculoCuotaFijaMensual

        #region calcularSaldo
        public decimal calcularSaldo(decimal amortizacion, decimal s)
        {
            return s - amortizacion;
        }
        #endregion calcularSaldo

        #region calcularInteres
        public decimal calcularInteres(decimal i, decimal saldo)
        {
            return saldo * i;
        }
        #endregion calcularInteres

        #region calcularAmortizacion
        public decimal calcularAmortizacion(decimal a, decimal interes)
        {
            return a - interes;
        }
        #endregion calcularAmortizacion

        #region GenerarCuotas
        public List<Cuota> GenerarCuotas(Prestamo prestamo)
        {
            decimal a = calcularCuotaFijaMensual(prestamo);
            decimal i = calcularTasaEfectivaMensual(prestamo);
            decimal s = prestamo.monto;
            List<Cuota> cuotas = new List<Cuota>();
            for (int w = 0; w < prestamo.cantCu + 1; w++)
            {
                Cuota cuota = new Cuota();
                Cuota cuotaT = new Cuota();
                if (w == 0)
                {
                    cuota.periodo = w;
                    cuota.saldo = Math.Round(s, 2, MidpointRounding.AwayFromZero);
                    cuota.cuota = 0;
                    cuota.interes = 0;
                    cuota.amortizacion = 0;
                    cuota.fechaPa = DateTime.Now;
                    cuotas.Add(cuota);
                }
                else
                {

                    cuotaT.saldo = cuotas[w - 1].saldo;
                    cuotaT.interes = cuotas[w - 1].interes;
                    cuotaT.amortizacion = cuotas[w - 1].amortizacion;
                    cuotaT.fechaPa = cuotas[w - 1].fechaPa;

                    cuota.cuota = Math.Round(a, 3, MidpointRounding.AwayFromZero);
                    cuota.periodo = w;
                    cuota.interes = Math.Round(calcularInteres(i, cuotaT.saldo), 2, MidpointRounding.AwayFromZero);
                    cuota.amortizacion = Math.Round(calcularAmortizacion(a, cuota.interes), 2, MidpointRounding.AwayFromZero);
                    cuota.saldo = Math.Round(calcularSaldo(cuota.amortizacion, cuotaT.saldo), 2, MidpointRounding.AwayFromZero);
                    cuota.fechaPa = cuotaT.fechaPa.AddMonths(1);
                    cuotas.Add(cuota);
                }
            }
            return cuotas;
        }
        #endregion GenerarCuotas
    }
}
