using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppProduccion.Entities.ModulosEscaneos;

namespace WebAppProduccion.Controllers.LogisticasInversas
{
    public class li_statusController : Controller
    {
        private DB_A3F19C_producccionEntities db = new DB_A3F19C_producccionEntities();

        // GET: li_status
        public ActionResult Index()
        {
            return View(db.li_status.ToList());
        }

        // GET: li_status/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            li_status li_status = db.li_status.Find(id);
            if (li_status == null)
            {
                return HttpNotFound();
            }
            return View(li_status);
        }

        // GET: li_status/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: li_status/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,descripcion")] li_status li_status)
        {
            if (ModelState.IsValid)
            {
                db.li_status.Add(li_status);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(li_status);
        }

        // GET: li_status/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            li_status li_status = db.li_status.Find(id);
            if (li_status == null)
            {
                return HttpNotFound();
            }
            return View(li_status);
        }

        // POST: li_status/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,descripcion")] li_status li_status)
        {
            if (ModelState.IsValid)
            {
                db.Entry(li_status).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(li_status);
        }

        // GET: li_status/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            li_status li_status = db.li_status.Find(id);
            if (li_status == null)
            {
                return HttpNotFound();
            }
            return View(li_status);
        }

        // POST: li_status/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            li_status li_status = db.li_status.Find(id);
            db.li_status.Remove(li_status);
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
