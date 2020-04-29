using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppProduccion.Entities.ModulosSistemas;
using WebAppProduccion.Filters;

namespace WebAppProduccion.Controllers.Sistemas
{
    [Authorize]
    public class empleadosController : Controller
    {
        private DB_A3F19C_producccionEntities1 db = new DB_A3F19C_producccionEntities1();

        // GET: empleados        
        public ActionResult Index()
        {
            var empleados = db.empleados.Include(e => e.rol);           

            return View(empleados.ToList());
        }

        // GET: empleados/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            empleados empleados = db.empleados.Find(id);
            if (empleados == null)
            {
                return HttpNotFound();
            }
            return View(empleados);
        }

        // GET: empleados/Create
        public ActionResult Create()
        {
            ViewBag.Puestos_Id = new SelectList(db.rol, "id", "Nombre");
            return View();
        }

        // POST: empleados/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(IdOperacion: 2)]
        public ActionResult Create([Bind(Include = "id,FechaAlta,Nombres,ApellidoPaterno,ApellidoMaterno,Email,FechaEntrada,Puestos_Id")] empleados empleados)
        {
            empleados.FechaAlta = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.empleados.Add(empleados);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Puestos_Id = new SelectList(db.rol, "id", "Nombre", empleados.Puestos_Id);
            return View(empleados);
        }

        // GET: empleados/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            empleados empleados = db.empleados.Find(id);
            if (empleados == null)
            {
                return HttpNotFound();
            }
            ViewBag.Puestos_Id = new SelectList(db.rol, "id", "Nombre", empleados.Puestos_Id);
            return View(empleados);
        }

        // POST: empleados/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(IdOperacion: 3)]
        public ActionResult Edit([Bind(Include = "id,FechaAlta,Nombres,ApellidoPaterno,ApellidoMaterno,Email,FechaEntrada,Puestos_Id")] empleados empleados)
        {
            if (ModelState.IsValid)
            {
                db.Entry(empleados).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Puestos_Id = new SelectList(db.rol, "id", "Nombre", empleados.Puestos_Id);
            return View(empleados);
        }

        // GET: empleados/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            empleados empleados = db.empleados.Find(id);
            if (empleados == null)
            {
                return HttpNotFound();
            }
            return View(empleados);
        }

        // POST: empleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(IdOperacion: 4)]
        public ActionResult DeleteConfirmed(int id)
        {
            empleados empleados = db.empleados.Find(id);
            db.empleados.Remove(empleados);
            db.SaveChanges();
            return Json(new { Respuesta = true, Mensaje = "Eliminacion Exitosa" } , JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                string sa = User.Identity.Name;

                if (Session["ua"] == null)
                {
                    Session["ua"] = sa;
                }

                db.Dispose();
            }
            base.Dispose(disposing);            
        }
    }
}
