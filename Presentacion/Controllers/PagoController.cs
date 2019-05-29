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
            if (Session["cli"] !=null)
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
                String dni = frm["dni"].ToString();
                Cliente cli = new Cliente();
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
    }
}