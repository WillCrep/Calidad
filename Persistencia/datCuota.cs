using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Entidad;

namespace Persistencia
{
    public class datCuota
    {
        #region Singleton

        private static readonly datCuota _instacia = new datCuota();
        public static datCuota Instancia
        {
            get { return datCuota._instacia; }
        }

        #endregion Singleton


        
    }
}
