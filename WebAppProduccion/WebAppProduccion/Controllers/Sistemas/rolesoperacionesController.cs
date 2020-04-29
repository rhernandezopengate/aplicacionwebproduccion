using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppProduccion.Entities.ModulosSistemas;

namespace WebAppProduccion.Controllers.Sistemas
{
    public class rolesoperacionesController : Controller
    {
        private DB_A3F19C_producccionEntities1 db = new DB_A3F19C_producccionEntities1();

        // GET: rolesoperaciones
        public ActionResult Index()
        {
            var rolesoperaciones = db.rolesoperaciones.Include(r => r.operaciones).Include(r => r.rol);
            return View(rolesoperaciones.ToList());
        }

        // GET: rolesoperaciones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rolesoperaciones rolesoperaciones = db.rolesoperaciones.Find(id);
            if (rolesoperaciones == null)
            {
                return HttpNotFound();
            }
            return View(rolesoperaciones);
        }

        // GET: rolesoperaciones/Create
        public ActionResult Create()
        {
            ViewBag.Operaciones_Id = new SelectList(db.operaciones, "id", "Nombre");
            ViewBag.Rol_Id = new SelectList(db.rol, "id", "Nombre");
            return View();
        }

        // POST: rolesoperaciones/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Rol_Id,Operaciones_Id")] rolesoperaciones rolesoperaciones)
        {
            if (ModelState.IsValid)
            {
                db.rolesoperaciones.Add(rolesoperaciones);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Operaciones_Id = new SelectList(db.operaciones, "id", "Nombre", rolesoperaciones.Operaciones_Id);
            ViewBag.Rol_Id = new SelectList(db.rol, "id", "Nombre", rolesoperaciones.Rol_Id);
            return View(rolesoperaciones);
        }

        // GET: rolesoperaciones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rolesoperaciones rolesoperaciones = db.rolesoperaciones.Find(id);
            if (rolesoperaciones == null)
            {
                return HttpNotFound();
            }
            ViewBag.Operaciones_Id = new SelectList(db.operaciones, "id", "Nombre", rolesoperaciones.Operaciones_Id);
            ViewBag.Rol_Id = new SelectList(db.rol, "id", "Nombre", rolesoperaciones.Rol_Id);
            return View(rolesoperaciones);
        }

        // POST: rolesoperaciones/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Rol_Id,Operaciones_Id")] rolesoperaciones rolesoperaciones)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rolesoperaciones).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Operaciones_Id = new SelectList(db.operaciones, "id", "Nombre", rolesoperaciones.Operaciones_Id);
            ViewBag.Rol_Id = new SelectList(db.rol, "id", "Nombre", rolesoperaciones.Rol_Id);
            return View(rolesoperaciones);
        }

        // GET: rolesoperaciones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rolesoperaciones rolesoperaciones = db.rolesoperaciones.Find(id);
            if (rolesoperaciones == null)
            {
                return HttpNotFound();
            }
            return View(rolesoperaciones);
        }

        // POST: rolesoperaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            rolesoperaciones rolesoperaciones = db.rolesoperaciones.Find(id);
            db.rolesoperaciones.Remove(rolesoperaciones);
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
