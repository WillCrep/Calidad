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
                ViewBag.cliente = logCliente.Instancia.BusClienteDni(frm["dni"].ToString());
                if (ViewBag.cliente != null)
                {
                    ViewBag.exito = 1;
                    ViewBag.valido = logPrestamo.Instancia.ValPrestamo(ViewBag.cliente);
                    Session["cli"] = ViewBag.cliente;
                    return View("ReaPres");
                }
                ViewBag.exito = 2;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View("ReaPres");
        }


        public ActionResult Cronograma(Prestamo prestamo)
        {
            List<Cuota> Cuotas = prestamo.GenerarCuotas(prestamo);
            Session["cu"] = Cuotas;
            Session["pre"] = prestamo;
            return View(Cuotas);
        }

        public ActionResult GuardarCronograma()
        {
            try
            {
                Prestamo prestamo = (Prestamo)Session["pre"];
                Cliente cli = (Cliente)Session["cli"];
                prestamo.cliente = cli;
                List<Cuota> cuotas = new List<Cuota>();
                cuotas = (List<Cuota>)Session["cu"];
                int idPrestamo = logPrestamo.Instancia.RegistrarPrestamo(prestamo);
                Boolean valido = logCuota.Instancia.RegistrarCuotas(cuotas, prestamo, idPrestamo);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return RedirectToAction("ReaPres", "Prestamo");
        }
    }
}