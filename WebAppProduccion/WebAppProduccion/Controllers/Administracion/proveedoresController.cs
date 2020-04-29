using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppProduccion.Controllers.Administracion
{
    public class proveedoresController : Controller
    {
        // GET: proveedores
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(int? id)
        {
            return View();
        }
        
        public ActionResult Edit(int? id)
        {
            return View();
        }
    }
}