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
        public int idPrestamo { get; set; }
        [Display(Name = "Fecha de Inicio")]
        [Required(ErrorMessage = "Este campo es Obligatorio")]
        public DateTime fechaInicio { get; set; }
        [Display(Name = "Fecha de Termino")]
        [Required(ErrorMessage = "Este campo es Obligatorio")]
        public DateTime fechaTermino { get; set; }
        [Display(Name = "Monto a Prestar")]
        [Range(1000, 30000, ErrorMessage = "El monto debe estar entre 1000 y 30000")]
        [Required(ErrorMessage = "Este campo es Obligatorio")]
        public decimal monto { get; set; }
        [Display(Name = "TEA")]
        [Range(0.10, 0.20, ErrorMessage = "La TEA permitida esta entre 0.10 y 0.20")]
        [Required(ErrorMessage = "Este campo es Obligatorio")]
        public decimal tea { get; set; }
        [Display(Name = "Cantidad de Cuotas")]
        [Range(3, 24, ErrorMessage = "Los periodos permitidos estan entre 3 y 24")]
        [Required(ErrorMessage = "Este campo es Obligatorio")]
        public int cantCuotas { get; set; }
        public Cliente cliente { get; set; }
        public List<Cuota> cuotas { get; set; }

        public Prestamo()
        {
            this.cuotas = new List<Cuota>();
        }

        //METODOS

        #region calculoTasaEfectivaMensual
        public decimal calcularTasaEfectivaMensual(Prestamo prestamo)
        {
            decimal i = (decimal)Math.Pow((1 + (double)prestamo.tea), 0.083f) - 1;
            return i;
        }
        #endregion calculoTasaEfectivaMensual

        #region calculoCuotaFijaMensual
        public decimal calcularCuotaFijaMensual(Prestamo prestamo)
        {
            decimal i = calcularTasaEfectivaMensual(prestamo);
            decimal a = prestamo.monto / ((1 - (decimal)Math.Pow(1 + (double)i, -prestamo.cantCuotas)) / i);
            return a;
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
        public void GenerarCuotas(Prestamo prestamo)
        {
            decimal a = calcularCuotaFijaMensual(prestamo);
            decimal i = calcularTasaEfectivaMensual(prestamo);
            decimal s = prestamo.monto;
            for (int w = 0; w < prestamo.cantCuotas + 1; w++)
            {
                Cuota cuota = new Cuota();
                Cuota cuotaT = new Cuota();
                if (w == 0)
                {
                    cuota.periodo = w;
                    cuota.saldo = s;
                    cuota.cuota = 0;
                    cuota.interes = 0;
                    cuota.amortizacion = 0;
                    cuota.fechaPago = DateTime.Now;
                    prestamo.cuotas.Add(cuota);
                }
                else
                {

                    cuotaT.saldo = cuotas[w - 1].saldo;
                    cuotaT.interes = cuotas[w - 1].interes;
                    cuotaT.amortizacion = cuotas[w - 1].amortizacion;
                    cuotaT.fechaPago = cuotas[w - 1].fechaPago;

                    cuota.cuota = a;
                    cuota.periodo = w;
                    cuota.interes = calcularInteres(i, cuotaT.saldo);
                    cuota.amortizacion = calcularAmortizacion(a, cuota.interes);
                    cuota.saldo = calcularSaldo(cuota.amortizacion, cuotaT.saldo);
                    cuota.fechaPago = cuotaT.fechaPago.AddMonths(1);
                    prestamo.cuotas.Add(cuota);
                }
            }
        }
        #endregion GenerarCuotas
    }
}
