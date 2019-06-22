using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Persistencia;
using Entidad;

namespace Aplicacion
{
    public class ClienteServicio
    {
        #region Singleton
        private static readonly ClienteServicio _instancia = new ClienteServicio();
        public static ClienteServicio Instancia
        {
            get { return ClienteServicio._instancia; }
        }

        #endregion Singleton

        #region BuscarClienteDni
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
        #endregion BuscarClienteDni
    }
}
