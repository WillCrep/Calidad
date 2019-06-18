using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidad;
using Persistencia;

namespace Logica
{
    public class logCuota
    {
        #region Singleton
        private static readonly logCuota _instancia = new logCuota();
        public static logCuota Instancia
        {
            get { return logCuota._instancia; }
        }

        #endregion Singleton

        #region VerificarCuota
        public List<int> VerificarCuota(List<Cuota> cuotas)
        {
            List<int> cuo = new List<int>();
            foreach (var item in cuotas)
            {
                if (!item.estado)
                {
                    cuo.Add(item.idCuo);
                    if (item.p == 1)
                    {
                        break;
                    }
                }
            }
            return cuo;
        }
        #endregion VerificarCuota


        #region cuotaUnitaria
        public Cuota cuotaUnitaria(List<Cuota> cuotas)
        {
            Cuota c = new Cuota();
            foreach (var item in cuotas)
            {
                if (item.p == 1)
                {
                    c = item;
                    break;
                }
            }
            return c;
        }
        #endregion cuotaUnitaria

        #region cuotasApagar
        public List<Cuota> cuotasApagar(List<Cuota> cuota)
        {
            List<Cuota> cuotas = new List<Cuota>();
            int id = 0;
            foreach (var item in cuota)
            {
                if (item.estado == false)
                {
                    id = item.prestamo.idPres;
                    if (DateTime.Now.Date == item.fechaPa.Date)
                    {
                        item.p = 1;
                    }
                }
            }
            foreach (var item in cuota)
            {
                if (item.prestamo.idPres == id)
                {
                    cuotas.Add(item);
                }
            }
            return cuotas;
        }
        #endregion cuotasApagar

        #region Listar Cuotas
        public List<Cuota> LsCuotas(string dni)
        {
            try
            {
                return datCuota.Instancia.LsCuotas(dni);
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        #endregion Listar Cuotas

        #region Registrar Cuotas
        public Boolean RegistrarCuotas(List<Cuota> cuotas, Prestamo prestamo, int id)
        {
            try
            {
                prestamo.idPres = id;
                foreach (var item in cuotas)
                {
                    item.prestamo = prestamo;
                }
                return datCuota.Instancia.RegistrarCuotas(cuotas);
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        #endregion Registrar Cuotas
    }
}
