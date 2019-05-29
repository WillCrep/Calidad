using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidad;

namespace Persistencia
{
    public class datPago
    {

        #region Singleton
        private static readonly datPago _instancia = new datPago();
        public static datPago Instancia
        {
            get { return datPago._instancia; }
        }

        #endregion Singleton
    }
}
