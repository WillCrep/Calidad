using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidad;
using Persistencia;

namespace Logica
{
    public class logPago
    {

        #region Singleton
        private static readonly logPago _instancia = new logPago();
        public static logPago Instancia
        {
            get { return logPago._instancia; }
        }

        #endregion Singleton

        #region VerificarMora
        public decimal VerificarMora(List<Cuota> cuotas, int idCu)
        {
            decimal deu = 0;
            decimal cont = 0;
            decimal mora = 0;
            List<int> dias = new List<int>();
            List<decimal> moras = new List<decimal>();
            foreach (var item in cuotas)
            {
                if (item.estado == false)
                {
                    if (item.idCuo == idCu)
                    {
                        break;
                    }
                    else
                    {
                        TimeSpan d = DateTime.Now.Date - item.fechaPa.Date;
                        dias.Add(d.Days);
                        deu = item.cuota;
                        cont = cont + 1;
                    }
                }
            }

            if (cont > 0)
            {
                moras = GenerarMora(dias,deu);
            }
            return mora;
        }
        #endregion VerificarMora
        public decimal calcularIm()
        {
            decimal im = 0;
            return im;
        }
        public decimal calcularIcv()
        {
            decimal icm = 0;
            return icm;
        }
        #region GenerarMora
        public List<decimal> GenerarMora(List<int> dias, decimal deu)
        {
            List<decimal> mora =new List<decimal>();
            decimal tm = (decimal)1.991;
            decimal tea = (decimal)0.4;
            foreach (var item in dias)
            {
                decimal ad = 0;
                ad = ((decimal)Math.Pow(1+(double)tm,(double)item/360)-1)*deu;
                mora.Add(ad);
            }
            return mora;
        }
        #endregion GenerarMora

        #region calcularNuevoPago
        public decimal calcularNuevoPago(decimal mora, List<Cuota> cuotas)
        {
            decimal nuevoPago = 0;
            decimal deu = 0;
            foreach (var item in cuotas)
            {
                deu = item.cuota;
                break; 
            }
            if(mora != 0)
            {
                nuevoPago = mora + deu + (mora / (decimal)0.25);
                return nuevoPago;
            }
            else
            {
                return deu;
            }
        }
        #endregion calcularNuevoPago

        public Boolean GuardarPago(Pago pago)
        {
            try
            {
                return datPago.Instancia.GuardarPago(pago);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public List<Pago> GenerarPagos(Pago pago, List<Cuota> cuotas)
        {
            List<Pago> pagos = new List<Pago>();
            int id = pago.idCuot.idCuo;
            pagos.Add(pago);
            foreach (var item in cuotas)
            {
                if (item.estado == false)
                {
                    if (item.idCuo == id)
                    {
                        break;
                    }
                    else
                    {
                        Pago pag = new Pago();
                        pag.idClin = pago.idClin;
                        pag.fechaPago = pago.fechaPago;
                        pag.idPres = pago.idPres;
                        pag.idCuot = item;
                        pag.mora = item.cuota * (decimal)0.25;
                        pag.total = item.cuota;
                        pagos.Add(pag);
                    }
                }
            }

            return pagos;
        }
    }
}
