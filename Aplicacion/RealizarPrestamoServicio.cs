using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidad;
using Persistencia;

namespace Aplicacion
{
    public class RealizarPrestamoServicio
    {
        #region Singleton
        private static readonly RealizarPrestamoServicio _instancia = new RealizarPrestamoServicio();
        public static RealizarPrestamoServicio Instancia
        {
            get { return RealizarPrestamoServicio._instancia; }
        }

        #endregion Singleton

        #region validarPrestamo
        public int ValPrestamo(Cliente cli)
        {
            try
            {
                return datPrestamo.Instancia.ValPrestamo(cli.idCliente);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion validarPrestamo

        #region RegistrarPrestamoYCuotas
        public Boolean RegistrarPrestamoYCuotas(Prestamo prestamo)
        {
            try
            {
                prestamo.fechaInicio = DateTime.Now;
                prestamo.fechaTermino = DateTime.Now.AddMonths(prestamo.cantCuotas);
                return datPrestamo.Instancia.RegistrarPrestamoYCuotas(prestamo);
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        #endregion RegistrarPrestamoYCuotas

    }
}
