using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entidad;
using Logica;

namespace Presentacion.Controllers
{
    public class PagoController : Controller
    {
        // GET: Pago
        public ActionResult ReaPago()
        {
            if (TempData["dni"]!=null)
            {
                List<Cuota> cuotas = new List<Cuota>();
                cuotas = logCuota.Instancia.LsCuotas(TempData["dni"].ToString());
                cuotas = logCuota.Instancia.cuotasApagar(cuotas);
                ViewBag.exito = 1;
                if (cuotas != null)
                {
                    TempData["cuotas"] = cuotas;
                }
                else
                {
                    ViewBag.v = 1;
                }
                return View(cuotas);
            }
            else
            {
                return View();
            }
        }

        public ActionResult BuscarCli(FormCollection frm)
        {
            try
            {
                Cliente cli = new Cliente();
                String dni = frm["dni"].ToString();
                cli = logCliente.Instancia.BusClienteDni(dni);
                if (cli != null)
                {
                    TempData["dni"] = dni;
                }
                else
                {
                    ViewBag.exito = 2;
                    return View("Reapago");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return RedirectToAction("ReaPago");
        }

        public ActionResult VerificarDeudas(int idCu)
        {
            List<Cuota> cuotas = (List<Cuota>)TempData["cuotas"];
            TempData["cuotas"] = cuotas;
            List<int> cuo = logCuota.Instancia.VerificarCuota(cuotas);
            if(cuo.Count > 1)
            {

                TempData["pa"] = cuo.Count;
                TempData["cuos"] = cuo;
            } 
            else
            {
                TempData["pa"] = 0;
            }
            return RedirectToAction("FormPago");
        }

        public ActionResult FormPago ()
        {
            List<Cuota> cuotas = (List<Cuota>)TempData["cuotas"];
            List<Pago> pagos = new List<Pago>();
            if (Convert.ToInt32(TempData["pa"]) == 0)
            {
                Cuota c = logCuota.Instancia.cuotaUnitaria(cuotas);
                Pago p = new Pago();
                p = p.GenerarPago(c);
                pagos.Add(p);
                ViewBag.total = c.cuota;
            }
            else
            {
                List<int> cuos = (List<int>)TempData["cuos"];
                int c = Convert.ToInt32(TempData["pa"]);
                for (int i =0; i < c; i++)
                {
                    Pago p = new Pago();
                    p = p.GenerarPagoMora(cuos[i], cuotas);
                    pagos.Add(p);
                }
                ViewBag.total = logPago.Instancia.totalPagoVariasCuotas(pagos);
                ViewBag.mora = logPago.Instancia.totalPagoMora(pagos);
            }
            TempData["pag"] = pagos;
            return View();
        }


        public ActionResult PagarDeuda(String efec, String vuelt)
        {
            List<Pago> pagos = (List<Pago>)TempData["pag"];
            Pago p = new Pago();
            try
            {
                if (pagos.Count > 1)
                {
                    foreach (var item in pagos)
                    {
                        p = item;
                        p.detPago.efectivo = Convert.ToDecimal(efec);
                        p.detPago.vuelto = Convert.ToDecimal(vuelt);
                        Boolean val = logPago.Instancia.GuardarPago(p);
                    }
                }
                else
                {
                    p = pagos[0];
                    p.detPago.efectivo = Convert.ToDecimal(efec);
                    p.detPago.vuelto = Convert.ToDecimal(vuelt);
                    Boolean val = logPago.Instancia.GuardarPago(p);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            TempData["pag"] = pagos;
            return RedirectToAction("ReportePago");
        }

        public ActionResult ReportePago()
        {
            List<Pago> pagos = new List<Pago>();
            pagos = (List<Pago>)TempData["pag"];
            return View(pagos);
        }
    }
}