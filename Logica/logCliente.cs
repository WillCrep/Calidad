using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidad;
using Persistencia;

namespace Logica
{
    public class logCliente
    {
        #region Singleton
        private static readonly logCliente _instancia = new logCliente();
        public static logCliente Instancia
        {
            get { return logCliente._instancia; }
        }

        #endregion Singleton

        #region logica

        public Cliente BusClienteDni(String dni)
        {
            try
            {
                return datCliente.Instancia.BusClienteDni(dni);
            }
            catch (Exception e)
            {

                throw e;
            }
        }



        #endregion logica
    }
}
