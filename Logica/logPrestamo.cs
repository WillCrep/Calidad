using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidad;
using Persistencia;

namespace Logica
{
    public class logPrestamo
    {
        #region Singleton
        private static readonly logPrestamo _instancia = new logPrestamo();
        public static logPrestamo Instancia
        {
            get { return logPrestamo._instancia; }
        }

        #endregion Singleton

        #region validarPrestamo
        public int ValPrestamo(Cliente cli)
        {
            try
            {
                return datPrestamo.Instancia.ValPrestamo(cli.idCli);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion validarPrestamo

        #region registrarPrestamo
        public int RegistrarPrestamo(Prestamo prestamo)
        {
            try
            {
                prestamo.fechaIni = DateTime.Now;
                prestamo.fechaTerm = DateTime.Now.AddMonths(prestamo.cantCu);
                return datPrestamo.Instancia.RegistrarPrestamo(prestamo);
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        #endregion registrarPrestamo

    }
}
