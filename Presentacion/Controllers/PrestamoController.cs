using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entidad;
using Logica;

namespace Presentacion.Controllers
{
    public class PrestamoController : Controller
    {
        // GET: Prestamo
        public ActionResult ReaPres()
        {

            return View();
        }

        [HttpPost]
        public ActionResult ReaPres(Prestamo prestamo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    prestamo.fechaIni = DateTime.Now;
                    prestamo.fechaTerm = DateTime.Now.AddMonths(prestamo.cantCu);
                    return RedirectToAction("Cronograma", "Prestamo", prestamo);
                }
                else
                {
                    return View("Error");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ActionResult BuscarCli(FormCollection frm)
        {
            try
            {
                Boolean val = false;
                String dni = frm["dni"].ToString();
                Cliente cli = new Cliente();
                cli = logCliente.Instancia.BusClienteDni(dni);
                if (cli != null)
                {
                    ViewBag.exito = 1;
                    val = logPrestamo.Instancia.ValPrestamo(cli.idCli);
                    ViewBag.valido = val ? 1 : 2;
                }
                else
                {
                    ViewBag.exito = 2;
                }
                ViewBag.cliente = cli;
                Session["cli"] = cli;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return View("ReaPres");
        }


        public ActionResult Cronograma(Prestamo prestamo)
        {
            List<Cuota> Cuotas = logCuota.Instancia.GenerarCuotas(prestamo);
            Session["cu"] = Cuotas;
            Session["pre"] = prestamo;
            return View(Cuotas);
        }

        public ActionResult GuardarCronograma()
        {
            Prestamo prestamo = (Prestamo)Session["pre"];
            Cliente cli = (Cliente)Session["cli"];
            prestamo.cliente = cli;
            int idPrestamo = logPrestamo.Instancia.RegistrarPrestamo(prestamo);
            prestamo.idPres = idPrestamo;
            List<Cuota> cuotas = new List<Cuota>();
            cuotas = (List<Cuota>)Session["cu"];
            foreach (var item in cuotas)
            {
                item.prestamo = prestamo;
            }
            Boolean valido = logCuota.Instancia.RegistrarCuotas(cuotas);
            return RedirectToAction("ReaPres", "Prestamo");
        }
    }
}