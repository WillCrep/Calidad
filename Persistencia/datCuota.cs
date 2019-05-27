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

        #region Registrar Cuotas

        public Boolean RegistrarCuotas(List<Cuota> cuotas)
        {
            SqlCommand cmd = null;
            Boolean valido = false;
            foreach (var item in cuotas)
            {
                try
                {
                    SqlConnection cn = Conexion.Instancia.conectar();
                    cmd = new SqlCommand("RegistrarCuota", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@prmtIntPeriodo", item.periodo);
                    cmd.Parameters.AddWithValue("@prmtFloatSaldo", item.saldo);
                    cmd.Parameters.AddWithValue("@prmtFloatAmortizacion", item.amortizacion);
                    cmd.Parameters.AddWithValue("@prmtFloatInteres", item.interes);
                    cmd.Parameters.AddWithValue("@prmtFloatCuota", item.cuota);
                    cmd.Parameters.AddWithValue("@prmtDTFechPa", item.fechPa);
                    cmd.Parameters.AddWithValue("@prmtIntPrestamoId", item.prestamo.idPres);
                    cn.Open();
                    int i = cmd.ExecuteNonQuery();
                    valido = i > 0 ? true : false;
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally { cmd.Connection.Close(); }
            }

            return valido;
        }

        #endregion Registrar Cuotas
    }
}
