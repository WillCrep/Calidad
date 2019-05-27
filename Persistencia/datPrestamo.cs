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
    public class datPrestamo
    {
        #region Singleton

        private static readonly datPrestamo _instancia = new datPrestamo();
        public static datPrestamo Instancia
        {
            get { return datPrestamo._instancia; }
        }

        #endregion Singleton

        #region Validar Prestamo

        public Boolean ValPrestamo(int idCli)
        {
            SqlCommand cmd = null;
            Boolean valido = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("veri_prests", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@prmintCliId", idCli);
                cmd.Parameters.AddWithValue("@valido", 0).Direction = ParameterDirection.Output;
                cn.Open();
                cmd.ExecuteNonQuery();
                valido = Convert.ToBoolean(cmd.Parameters["@valido"].Value);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { cmd.Connection.Close(); }
            return valido;
        }

        #endregion Validar Prestamo

        #region Guardar Prestamo

        public int RegistrarPrestamo(Prestamo prestamo)
        {
            SqlCommand cmd = null;
            int valido = 0;
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("RegistrarPrestamo", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@prmtDtFechaIni", prestamo.fechaIni);
                cmd.Parameters.AddWithValue("@prmtDtFechaTerm", prestamo.fechaTerm);
                cmd.Parameters.AddWithValue("@prmtFloatMonto", prestamo.monto);
                cmd.Parameters.AddWithValue("@prmtFloatTea", prestamo.tea);
                cmd.Parameters.AddWithValue("@prmtIntCantCu", prestamo.cantCu);
                cmd.Parameters.AddWithValue("@prmtCliIdCli", prestamo.cliente.idCli);
                cmd.Parameters.AddWithValue("@idPres", 0).Direction = ParameterDirection.Output;
                cn.Open();
                cmd.ExecuteNonQuery();
                valido = Convert.ToInt32(cmd.Parameters["@idPres"].Value);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { cmd.Connection.Close(); }
            return valido;
        }

        #endregion Guardar Prestamo
    }
}
