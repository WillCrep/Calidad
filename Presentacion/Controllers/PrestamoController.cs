using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entidad;
using Aplicacion;

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
                ViewBag.cliente = ClienteServicio.Instancia.BusClienteDni(frm["dni"].ToString());
                if (ViewBag.cliente != null)
                {
                    ViewBag.exito = 1;
                    ViewBag.valido = RealizarPrestamoServicio.Instancia.ValPrestamo(ViewBag.cliente);
                    TempData["cli"] = ViewBag.cliente;
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
            prestamo.GenerarCuotas(prestamo);
            TempData["pre"] = prestamo;
            return View(prestamo);
        }

        public ActionResult GuardarCronograma()
        {
            try
            {
                Prestamo prestamo = (Prestamo)TempData["pre"];
                Cliente cli = (Cliente)TempData["cli"];
                prestamo.cliente = cli;
                Boolean valido = RealizarPrestamoServicio.Instancia.RegistrarPrestamoYCuotas(prestamo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("ReaPres", "Prestamo");
        }
    }
}