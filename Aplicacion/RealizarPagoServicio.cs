using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidad;
using Persistencia;

namespace Aplicacion
{
    public class RealizarPagoServicio
    {
        #region Singleton
        private static readonly RealizarPagoServicio _instancia = new RealizarPagoServicio();
        public static RealizarPagoServicio Instancia
        {
            get { return RealizarPagoServicio._instancia; }
        }

        #endregion Singleton

        #region ListarCuotas
        public List<Prestamo> ObtenerPrestamoAPagar(string dni)
        {
            try
            {
                return datPrestamo.Instancia.ObtenerPrestamoAPagar(dni);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion ListarCuotas

        #region VerificarCuotasAtrasadas
        public Prestamo VerificarCuotasAtrasadas(List<Prestamo> prestamos)
        {
            List<Cuota> cuotas = new List<Cuota>();
            int b = 0;
            Prestamo prestamo = new Prestamo();
            foreach (var prestam in prestamos)
            {
                foreach (var cuota in prestam.cuotas)
                {
                    if (cuota.estado == false)
                    {
                        b = 1;
                        if (DateTime.Now.Date == cuota.fechaPago.Date)
                        {
                            cuota.p = 1;
                        }
                    }
                }
                if (b == 1)
                {
                    prestamo = prestam;
                }
            }
            return prestamo;
        }
        #endregion VerificarCuotasAtrasadas

        #region ObtenerIdsDeCuotasAtrasadas
        public List<int> ObtenerIdsDeCuotasAtrasadas(Prestamo prestamo)
        {
            List<int> cuo = new List<int>();
            foreach (var cuotas in prestamo.cuotas)
            {
                if (!cuotas.estado)
                {
                    cuo.Add(cuotas.idCuota);
                    if (cuotas.p == 1)
                    {
                        break;
                    }
                }
            }
            return cuo;
        }
        #endregion ObtenerIdsDeCuotasAtrasadas

        #region totalPagoVariasCuotas
        public decimal totalPagoVariasCuotas(List<Pago> pagos)
        {
            decimal total = 0;
            foreach (var item in pagos)
            {
                total = total + item.total;
            }
            return total;
        }
        #endregion totalPagoVariasCuotas

        #region totalPagoMora
        public decimal totalPagoMora(List<Pago> pagos)
        {
            decimal mora = 0;
            foreach (var item in pagos)
            {
                mora = mora + item.mora;
            }
            return mora;
        }
        #endregion totalPagoMora

        #region GuardarPago
        public Boolean GuardarPago(Pago pago)
        {
            try
            {
                return datPago.Instancia.GuardarPago(pago);
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        #endregion GuardarPago
    }
}
