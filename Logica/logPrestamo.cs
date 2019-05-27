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

        public Boolean ValPrestamo(int idCli)
        {
            try
            {
                return datPrestamo.Instancia.ValPrestamo(idCli);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int RegistrarPrestamo(Prestamo prestamo)
        {
            try
            {
                return datPrestamo.Instancia.RegistrarPrestamo(prestamo);
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}
