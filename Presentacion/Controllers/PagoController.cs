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
            if (Session["cli"] != null)
            {
                ViewBag.exito = 1;
                List<Cuota> cuotas = new List<Cuota>();
                cuotas = (List<Cuota>)Session["cuotas"];
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
                List<Cuota> cuotas = new List<Cuota>();
                Cliente cli = new Cliente();
                String dni = frm["dni"].ToString();
                cli = logCliente.Instancia.BusClienteDni(dni);
                if (cli != null)
                {
                    
                    cuotas = logCuota.Instancia.LsCuotas(dni);
                    Session["cuotas"] = cuotas;
                }
                else
                {
                    ViewBag.exito = 0;
                }
                ViewBag.cliente = cli;
                Session["cli"] = cli;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return RedirectToAction("ReaPago");
        }

        public ActionResult VerificarDeudas(int idCu)
        {

            List<Cuota> cuotas = new List<Cuota>();
            cuotas = (List<Cuota>)Session["cuotas"];
            decimal PagoTotal = 0;
            decimal mora = logPago.Instancia.VerificarMora(cuotas, idCu);
            if(mora != 0)
            {
                PagoTotal = logPago.Instancia.calcularNuevoPago(mora, cuotas);
                return RedirectToAction("FormPago", new { pago= PagoTotal, idCuo = idCu});
            } 
            else
            {
                PagoTotal = logPago.Instancia.calcularNuevoPago(mora, cuotas);
                return RedirectToAction("FormPago", new { pago = PagoTotal, idCuo = idCu });
            }
        }

        public ActionResult FormPago (decimal pago, int idCuo)
        {
            List<Cuota> cuotas = new List<Cuota>();
            cuotas = (List<Cuota>)Session["cuotas"];
            Cliente cli = (Cliente)Session["cli"];
            Cuota cuota = new Cuota();
            Pago pagob = new Pago();
            Prestamo prestamo = new Prestamo();
            decimal mora = logPago.Instancia.VerificarMora(cuotas, idCuo);
            foreach (var item in cuotas)
            {
                if(item.idCuo == idCuo)
                {
                    cuota.amortizacion = item.amortizacion;
                    cuota.cuota = item.cuota;
                    cuota.estado = item.estado;
                    cuota.fechaPa = item.fechaPa;
                    cuota.idCuo = item.idCuo;
                    cuota.interes = item.interes;
                    cuota.periodo = item.periodo;
                    cuota.prestamo = item.prestamo;
                    cuota.saldo = item.saldo;
                    break;
                } 
            }
            prestamo = cuota.prestamo;
            pagob.idCuot = cuota;
            pagob.idPres = prestamo;
            pagob.idClin = cli;
            pagob.mora = mora;
            pagob.total = pago;
            pagob.fechaPago = DateTime.Now;
            ViewBag.total = pagob.total;
            ViewBag.mora = pagob.mora;
            Session["pag"] = pagob;
            return View();
        }


        public ActionResult PagarDeuda(String efec, String vuelt)
        {
            Pago pago = new Pago();
            pago = (Pago)Session["pag"];
            List<Pago> pagos = new List<Pago>();
            Boolean valido = false;
            if(pago.mora != 0)
            {
                List<Cuota> cuotas = new List<Cuota>();
                cuotas = (List<Cuota>)Session["cuotas"];
                pagos = logPago.Instancia.GenerarPagos(pago, cuotas);
                foreach (var item in pagos)
                {
                    valido = logPago.Instancia.GuardarPago(item);
                }

            }
            else
            {
                pagos.Add(pago);
                valido = logPago.Instancia.GuardarPago(pago);
            }
            Session["pagos"] = pagos;
            return RedirectToAction("ReportePago");
        }

        public ActionResult ReportePago()
        {
            List<Pago> pagos = new List<Pago>();
            pagos = (List<Pago>)Session["pagos"];
            return View(pagos);
        }
    }
}