using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidad;
using System.Data;
using System.Data.SqlClient;

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

        public Boolean GuardarPago(Pago pago)
        {
            SqlCommand cmd = null;
            Boolean valido = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("GuardarPago", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@prmtDateFechaPago", pago.fechaPago);
                cmd.Parameters.AddWithValue("@prmtDeciMora", pago.mora);
                cmd.Parameters.AddWithValue("@prmtDeciTotal", pago.total);
                cmd.Parameters.AddWithValue("@prmtIntidCli", pago.idClin.idCli);
                cmd.Parameters.AddWithValue("@prmtIntIdPres", pago.idPres.idPres);
                cmd.Parameters.AddWithValue("@prmtIntIdCuo", pago.idCuot.idCuo);
                cmd.Parameters.AddWithValue("@prmtDeciEfectivo", pago.detPago.efectivo);
                cmd.Parameters.AddWithValue("@prmtDeciVuelto", pago.detPago.vuelto);
                cmd.Parameters.AddWithValue("@prmtDeciIm", pago.detPago.iM);
                cmd.Parameters.AddWithValue("@prmtDeciIcv", pago.detPago.iCv);
                cn.Open();
                int i = cmd.ExecuteNonQuery();
                valido = i > 0 ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { cmd.Connection.Close(); }
            return valido;
        }
    }
}
