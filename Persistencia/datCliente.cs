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
    public class datCliente
    {
        #region singleton
        private static readonly datCliente _instancia = new datCliente();
        public static datCliente Instancia
        {
            get { return datCliente._instancia; }
        }
        #endregion singleton

        #region BuscarDni

        public Cliente BusClienteDni(String dni)
        {
            SqlCommand cmd = null;
            Cliente cli = null;
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("bus_cliente_dni", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@prmintDni", dni);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    cli = new Cliente();
                    cli.idCli = Convert.ToInt32(dr["idCli"]);
                    cli.nombres = dr["nombres"].ToString();
                    cli.apellidos = dr["apellidos"].ToString();
                    cli.dni = dr["dni"].ToString();
                    cli.direccion = dr["direccion"].ToString();
                    cli.fechaRegistro = Convert.ToDateTime(dr["fechaRegistro"]);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { cmd.Connection.Close(); }
            return cli;
        }

        #endregion BuscarDni
    }
}
