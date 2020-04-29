using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppProduccion.Entities.ModulosAdministracion;

namespace WebAppProduccion.Controllers.Administracion
{
    public class monedafacturacionsController : Controller
    {
        private DB_A3F19C_producccionEntities2 db = new DB_A3F19C_producccionEntities2();

        // GET: monedafacturacions
        public ActionResult Index()
        {
            return View(db.monedafacturacion.ToList());
        }

        // GET: monedafacturacions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            monedafacturacion monedafacturacion = db.monedafacturacion.Find(id);
            if (monedafacturacion == null)
            {
                return HttpNotFound();
            }
            return View(monedafacturacion);
        }

        // GET: monedafacturacions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: monedafacturacions/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,descripcion")] monedafacturacion monedafacturacion)
        {
            if (ModelState.IsValid)
            {
                db.monedafacturacion.Add(monedafacturacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(monedafacturacion);
        }

        // GET: monedafacturacions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            monedafacturacion monedafacturacion = db.monedafacturacion.Find(id);
            if (monedafacturacion == null)
            {
                return HttpNotFound();
            }
            return View(monedafacturacion);
        }

        // POST: monedafacturacions/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,descripcion")] monedafacturacion monedafacturacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(monedafacturacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(monedafacturacion);
        }

        // GET: monedafacturacions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            monedafacturacion monedafacturacion = db.monedafacturacion.Find(id);
            if (monedafacturacion == null)
            {
                return HttpNotFound();
            }
            return View(monedafacturacion);
        }

        // POST: monedafacturacions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            monedafacturacion monedafacturacion = db.monedafacturacion.Find(id);
            db.monedafacturacion.Remove(monedafacturacion);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
