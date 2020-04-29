using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppProduccion.Controllers.Sistemas
{
    public class sistemasController : Controller
    {
        // GET: sistemas
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IndexSkus() 
        {
            return View();        
        }


        public ActionResult Plain()
        {
            return View();
        }

        public ActionResult Form() 
        {
            return View();
        }

        public ActionResult Tablas()
        {
            return View();
        }
    }
}