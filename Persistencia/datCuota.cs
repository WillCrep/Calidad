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
                    cmd.Parameters.AddWithValue("@prmtDTFechPa", item.fechaPa);
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

        #region Listar Cuotas
        public List<Cuota> LsCuotas(string dni)
        {
            SqlCommand cmd = null;
            List<Cuota> lista = new List<Cuota>();
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("LsCuotas", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@prmintDni", dni);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int ban = Convert.ToInt32(dr["periodo"]);
                    if (ban != 0){
                        Cuota cuota = new Cuota();
                        Cliente cli = new Cliente();
                        Prestamo prestamo = new Prestamo();
                        cuota.idCuo = Convert.ToInt32(dr["idCuo"]);
                        prestamo.idPres = Convert.ToInt32(dr["idPrest"]);
                        prestamo.cantCu = Convert.ToInt32(dr["cantCut"]);
                        prestamo.fechaIni = Convert.ToDateTime(dr["fechaIni"]);
                        prestamo.fechaTerm = Convert.ToDateTime(dr["fechaTerm"]);
                        prestamo.monto = Convert.ToDecimal(dr["monto"]);
                        prestamo.tea = Convert.ToDecimal(dr["tea"]);
                        cli.idCli = Convert.ToInt32(dr["idClie"]);
                        prestamo.cliente = cli;
                        cuota.prestamo = prestamo;
                        cuota.interes = Convert.ToDecimal(dr["interes"]);
                        cuota.amortizacion = Convert.ToDecimal(dr["amortizacion"]);
                        cuota.cuota = Convert.ToDecimal(dr["cuota"]);
                        cuota.estado = Convert.ToBoolean(dr["estado"]);
                        cuota.fechaPa = Convert.ToDateTime(dr["fechaPa"]);
                        cuota.periodo = ban;
                        cuota.saldo = Convert.ToDecimal(dr["saldo"]);
                        lista.Add(cuota);
                    }
                    
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { cmd.Connection.Close(); }
            return lista;
        }
        #endregion Listar Cuotas
    }
}
