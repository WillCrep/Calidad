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

        public int ValPrestamo(int idCli)
        {
            SqlCommand cmd = null;
            int valido = 2;
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("veri_prests", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@prmintCliId", idCli);
                cmd.Parameters.AddWithValue("@valido", 0).Direction = ParameterDirection.Output;
                cn.Open();
                cmd.ExecuteNonQuery();
                valido = Convert.ToInt32(cmd.Parameters["@valido"].Value);
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

        public Boolean RegistrarPrestamoYCuotas(Prestamo prestamo)
        {
            SqlCommand cmd = null;
            Boolean valido = false;
            int id = 0;
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("RegistrarPrestamo", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@prmtDtFechaIni", prestamo.fechaInicio);
                cmd.Parameters.AddWithValue("@prmtDtFechaTerm", prestamo.fechaTermino);
                cmd.Parameters.AddWithValue("@prmtFloatMonto", prestamo.monto);
                cmd.Parameters.AddWithValue("@prmtFloatTea", prestamo.tea);
                cmd.Parameters.AddWithValue("@prmtIntCantCu", prestamo.cantCuotas);
                cmd.Parameters.AddWithValue("@prmtCliIdCli", prestamo.cliente.idCliente);
                cmd.Parameters.AddWithValue("@idPres", 0).Direction = ParameterDirection.Output;
                cn.Open();
                cmd.ExecuteNonQuery();
                id = Convert.ToInt32(cmd.Parameters["@idPres"].Value);

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { cmd.Connection.Close(); }

            foreach (var cuota in prestamo.cuotas)
            {
                try
                {
                    SqlConnection cn = Conexion.Instancia.conectar();
                    cmd = new SqlCommand("RegistrarCuota", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@prmtIntPeriodo", cuota.periodo);
                    cmd.Parameters.AddWithValue("@prmtFloatSaldo", cuota.saldo);
                    cmd.Parameters.AddWithValue("@prmtFloatAmortizacion", cuota.amortizacion);
                    cmd.Parameters.AddWithValue("@prmtFloatInteres", cuota.interes);
                    cmd.Parameters.AddWithValue("@prmtFloatCuota", cuota.cuota);
                    cmd.Parameters.AddWithValue("@prmtDTFechPa", cuota.fechaPago);
                    cmd.Parameters.AddWithValue("@prmtIntPrestamoId", id);
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

        #endregion Guardar Prestamo

        #region ObtenerPrestamoAPagar
        public List<Prestamo> ObtenerPrestamoAPagar(string dni)
        {
            SqlCommand cmd = null;
            List<Prestamo> prestamos = new List<Prestamo>();
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("LsCuotas", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@prmintDni", dni);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                Cliente cli = new Cliente();
                while (dr.Read())
                {
                    int periodo = Convert.ToInt32(dr["periodo"]);
                    int ban = 0;
                    Cuota cuota = new Cuota();
                    Prestamo prestamo = new Prestamo();
                    if (periodo != 0)
                    {
                        int id = Convert.ToInt32(dr["idPrest"]);
                        foreach (var prestam in prestamos)
                        {
                            if (id == prestam.idPrestamo)
                            {
                                cuota.idCuota = Convert.ToInt32(dr["idCuo"]);
                                cuota.interes = Convert.ToDecimal(dr["interes"]);
                                cuota.amortizacion = Convert.ToDecimal(dr["amortizacion"]);
                                cuota.cuota = Convert.ToDecimal(dr["cuota"]);
                                cuota.estado = Convert.ToBoolean(dr["estado"]);
                                cuota.fechaPago = Convert.ToDateTime(dr["fechaPa"]);
                                cuota.periodo = periodo;
                                cuota.saldo = Convert.ToDecimal(dr["saldo"]);
                                prestam.cuotas.Add(cuota);
                                ban = 1;
                            }
                        }
                        if (ban != 1)
                        {
                            prestamo.idPrestamo = id;
                            prestamo.cantCuotas = Convert.ToInt32(dr["cantCut"]);
                            prestamo.fechaInicio = Convert.ToDateTime(dr["fechaIni"]);
                            prestamo.fechaTermino = Convert.ToDateTime(dr["fechaTerm"]);
                            prestamo.monto = Convert.ToDecimal(dr["monto"]);
                            prestamo.tea = Convert.ToDecimal(dr["tea"]);
                            cli.idCliente = Convert.ToInt32(dr["idClie"]);
                            prestamo.cliente = cli;
                            cuota.idCuota = Convert.ToInt32(dr["idCuo"]);
                            cuota.interes = Convert.ToDecimal(dr["interes"]);
                            cuota.amortizacion = Convert.ToDecimal(dr["amortizacion"]);
                            cuota.cuota = Convert.ToDecimal(dr["cuota"]);
                            cuota.estado = Convert.ToBoolean(dr["estado"]);
                            cuota.fechaPago = Convert.ToDateTime(dr["fechaPa"]);
                            cuota.periodo = periodo;
                            cuota.saldo = Convert.ToDecimal(dr["saldo"]);
                            prestamo.cuotas.Add(cuota);
                            prestamos.Add(prestamo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { cmd.Connection.Close(); }
            return prestamos;
        }
        #endregion ObtenerPrestamoAPagar
    }
}
