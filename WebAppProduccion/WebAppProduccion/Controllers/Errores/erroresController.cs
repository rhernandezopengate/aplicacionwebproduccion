using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppProduccion.Controllers.Errores
{
    public class erroresController : Controller
    {
        // GET: errores
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UnauthorizedOperation(string operacion, string modulo, string msjeErrorExcepcion)
        {
            ViewBag.operacion = operacion;
            ViewBag.modulo = modulo;
            ViewBag.mensaje = msjeErrorExcepcion;
            return View();
        }
    }
}