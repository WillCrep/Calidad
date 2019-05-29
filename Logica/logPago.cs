using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidad;
using Persistencia;

namespace Logica
{
    public class logPago
    {

        #region Singleton
        private static readonly logPago _instancia = new logPago();
        public static logPago Instancia
        {
            get { return logPago._instancia; }
        }

        #endregion Singleton
    }
}
