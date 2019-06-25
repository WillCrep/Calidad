using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entidad;
using Aplicacion;

namespace Presentacion.Controllers
{
    public class PagoController : Controller
    {
        // GET: Pago
        public ActionResult ReaPago()
        {
            if (TempData["dni"]!=null)
            {
                List<Prestamo> prestamos = RealizarPagoServicio.Instancia.ObtenerPrestamoAPagar(TempData["dni"].ToString());
                Prestamo prestamo = RealizarPagoServicio.Instancia.VerificarCuotasAtrasadas(prestamos);
                ViewBag.exito = 1;
                if (prestamo != null)
                {
                    TempData["prestamo"] = prestamo;
                }
                else
                {
                    ViewBag.v = 1;
                }
                return View(prestamo);
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
                cli = ClienteServicio.Instancia.BusClienteDni(dni);
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
            Prestamo prestamo = (Prestamo)TempData["prestamo"];
            TempData["prestamo"] = prestamo;
            List<int> cuo = RealizarPagoServicio.Instancia.ObtenerIdsDeCuotasAtrasadas(prestamo);
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
            Prestamo prestamo = (Prestamo)TempData["prestamo"];
            List<Pago> pagos = new List<Pago>();
            if (Convert.ToInt32(TempData["pa"]) == 0)
            {
                Pago p = new Pago();
                p = p.GenerarPago(prestamo);
                pagos.Add(p);
                ViewBag.total = prestamo.cuotas[0].cuota;
            }
            else
            {
                List<int> cuos = (List<int>)TempData["cuos"];
                int c = Convert.ToInt32(TempData["pa"]);
                for (int i =0; i < c; i++)
                {
                    Pago p = new Pago();
                    p = p.GenerarPagoMora(cuos[i], prestamo);
                    pagos.Add(p);
                }
                ViewBag.total = Math.Round(RealizarPagoServicio.Instancia.totalPagoVariasCuotas(pagos),2);
                ViewBag.mora = Math.Round(RealizarPagoServicio.Instancia.totalPagoMora(pagos),2);
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
                        p.detallePago.efectivo = Convert.ToDecimal(efec);
                        p.detallePago.vuelto = Convert.ToDecimal(vuelt);
                        Boolean val = RealizarPagoServicio.Instancia.GuardarPago(p);
                    }
                }
                else
                {
                    p = pagos[0];
                    p.detallePago.efectivo = Convert.ToDecimal(efec);
                    p.detallePago.vuelto = Convert.ToDecimal(vuelt);
                    Boolean val = RealizarPagoServicio.Instancia.GuardarPago(p);
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