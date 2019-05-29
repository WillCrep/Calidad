using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidad;
using Persistencia;

namespace Logica
{
    public class logCuota
    {
        #region Singleton
        private static readonly logCuota _instancia = new logCuota();
        public static logCuota Instancia
        {
            get { return logCuota._instancia; }
        }

        #endregion Singleton

        #region Listar Cuotas
        public List<Cuota> LsCuotas(string dni)
        {
            try
            {
                return datCuota.Instancia.LsCuotas(dni);
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        #endregion Listar Cuotas

        #region Calculos de Cuotas
        private decimal calcularTasaEfectivaMensual(Prestamo prestamo)
        {
            decimal i = (decimal)Math.Pow((1 + (double)prestamo.tea), 0.083f) - 1;
            return Math.Round(i, 2, MidpointRounding.AwayFromZero);
        }

        private decimal calcularCuotaFijaMensual(Prestamo prestamo)
        {
            decimal i = calcularTasaEfectivaMensual(prestamo);
            decimal a = prestamo.monto / ((1 - (decimal)Math.Pow(1 + (double)i, -prestamo.cantCu)) / i);
            return Math.Round(a, 2, MidpointRounding.AwayFromZero);
        }

        private decimal calcularSaldo(decimal amortizacion, decimal s)
        {
            return s - amortizacion;
        }

        private decimal calcularInteres(decimal i, decimal saldo)
        {
            return saldo * i;
        }

        private decimal calcularAmortizacion(decimal a, decimal interes)
        {
            return a - interes;
        }

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
        #endregion Calculos de Cuotas

        #region Registrar Cuotas
        public Boolean RegistrarCuotas(List<Cuota> cuotas)
        {
            try
            {
                return datCuota.Instancia.RegistrarCuotas(cuotas);
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        #endregion Registrar Cuotas
    }
}
