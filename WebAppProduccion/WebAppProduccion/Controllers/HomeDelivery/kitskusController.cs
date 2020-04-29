using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppProduccion.Entities.ModulosEscaneos;

namespace WebAppProduccion.Controllers.HomeDelivery
{
    public class kitskusController : Controller
    {
        private DB_A3F19C_producccionEntities db = new DB_A3F19C_producccionEntities();

        // GET: kitskus
        public ActionResult Index()
        {
            var kitskus = db.kitskus.Include(k => k.kits).Include(k => k.skus);
            return View(kitskus.ToList());
        }

        // GET: kitskus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            kitskus kitskus = db.kitskus.Find(id);
            if (kitskus == null)
            {
                return HttpNotFound();
            }
            return View(kitskus);
        }

        // GET: kitskus/Create
        public ActionResult Create()
        {
            ViewBag.kits_Id = new SelectList(db.kits, "id", "descripcion");
            ViewBag.skus_Id = new SelectList(db.skus, "id", "SKU");
            return View();
        }

        // POST: kitskus/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Cantidad,kits_Id,skus_Id")] kitskus kitskus)
        {
            if (ModelState.IsValid)
            {
                db.kitskus.Add(kitskus);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.kits_Id = new SelectList(db.kits, "id", "descripcion", kitskus.kits_Id);
            ViewBag.skus_Id = new SelectList(db.skus, "id", "SKU", kitskus.skus_Id);
            return View(kitskus);
        }

        // GET: kitskus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            kitskus kitskus = db.kitskus.Find(id);
            if (kitskus == null)
            {
                return HttpNotFound();
            }
            ViewBag.kits_Id = new SelectList(db.kits, "id", "descripcion", kitskus.kits_Id);
            ViewBag.skus_Id = new SelectList(db.skus, "id", "SKU", kitskus.skus_Id);
            return View(kitskus);
        }

        // POST: kitskus/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Cantidad,kits_Id,skus_Id")] kitskus kitskus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kitskus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.kits_Id = new SelectList(db.kits, "id", "descripcion", kitskus.kits_Id);
            ViewBag.skus_Id = new SelectList(db.skus, "id", "SKU", kitskus.skus_Id);
            return View(kitskus);
        }

        // GET: kitskus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            kitskus kitskus = db.kitskus.Find(id);
            if (kitskus == null)
            {
                return HttpNotFound();
            }
            return View(kitskus);
        }

        // POST: kitskus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            kitskus kitskus = db.kitskus.Find(id);
            db.kitskus.Remove(kitskus);
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
