using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Persistencia
{
    public class Conexion
    {
        private static readonly Conexion _instancia = new Conexion();
        public static Conexion Instancia
        {
            get { return Conexion._instancia; }
        }

        public SqlConnection conectar()
        {
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = "Data Source=DESKTOP-74GDALA; Initial Catalog=prestamo5; Integrated Security=true";

            return cn;
        }
    }
}
