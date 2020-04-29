using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppProduccion.Entities.ModulosEscaneos;
using WebAppProduccion.Filters;

namespace WebAppProduccion.Controllers.Sistemas
{
    public class auditoresController : Controller
    {
        private DB_A3F19C_producccionEntities db = new DB_A3F19C_producccionEntities();

        // GET: auditores
        [AuthorizeUser(IdOperacion: 21)]
        public ActionResult Index()
        {
            return View(db.auditores.ToList());
        }

        // GET: auditores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            auditores auditores = db.auditores.Find(id);
            if (auditores == null)
            {
                return HttpNotFound();
            }
            return View(auditores);
        }

        // GET: auditores/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: auditores/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(IdOperacion: 22)]
        public ActionResult Create([Bind(Include = "id,nombres")] auditores auditores)
        {
            if (ModelState.IsValid)
            {
                db.auditores.Add(auditores);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(auditores);
        }

        // GET: auditores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            auditores auditores = db.auditores.Find(id);
            if (auditores == null)
            {
                return HttpNotFound();
            }
            return View(auditores);
        }

        // POST: auditores/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(IdOperacion: 23)]
        public ActionResult Edit([Bind(Include = "id,nombres")] auditores auditores)
        {
            if (ModelState.IsValid)
            {
                db.Entry(auditores).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(auditores);
        }

        // GET: auditores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            auditores auditores = db.auditores.Find(id);
            if (auditores == null)
            {
                return HttpNotFound();
            }
            return View(auditores);
        }

        // POST: auditores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(IdOperacion: 24)]
        public ActionResult DeleteConfirmed(int id)
        {
            auditores auditores = db.auditores.Find(id);
            db.auditores.Remove(auditores);
            db.SaveChanges();
            return RedirectToAction("Index");
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
