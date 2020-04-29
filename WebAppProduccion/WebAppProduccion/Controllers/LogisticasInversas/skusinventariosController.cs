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
    public class skusinventariosController : Controller
    {
        private DB_A3F19C_producccionEntities db = new DB_A3F19C_producccionEntities();

        // GET: skusinventarios
        public ActionResult Index()
        {
            var skusinventarios = db.skusinventarios.Include(s => s.skus);
            return View(skusinventarios.ToList());
        }

        // GET: skusinventarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            skusinventarios skusinventarios = db.skusinventarios.Find(id);
            if (skusinventarios == null)
            {
                return HttpNotFound();
            }
            return View(skusinventarios);
        }

        // GET: skusinventarios/Create
        public ActionResult Create()
        {
            ViewBag.skus_Id = new SelectList(db.skus.OrderByDescending(x => x.id), "id", "SKU");
            return View();
        }

        // POST: skusinventarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,qtymanual,skus_Id")] skusinventarios skusinventarios)
        {
            if (ModelState.IsValid)
            {
                db.skusinventarios.Add(skusinventarios);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.skus_Id = new SelectList(db.skus, "id", "SKU", skusinventarios.skus_Id);
            return View(skusinventarios);
        }

        // GET: skusinventarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            skusinventarios skusinventarios = db.skusinventarios.Find(id);
            if (skusinventarios == null)
            {
                return HttpNotFound();
            }
            ViewBag.skus_Id = new SelectList(db.skus, "id", "SKU", skusinventarios.skus_Id);
            return View(skusinventarios);
        }

        // POST: skusinventarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,qtymanual,skus_Id")] skusinventarios skusinventarios)
        {
            if (ModelState.IsValid)
            {                
                db.Entry(skusinventarios).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.skus_Id = new SelectList(db.skus, "id", "SKU", skusinventarios.skus_Id);
            return View(skusinventarios);
        }

        // GET: skusinventarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            skusinventarios skusinventarios = db.skusinventarios.Find(id);
            if (skusinventarios == null)
            {
                return HttpNotFound();
            }
            return View(skusinventarios);
        }

        // POST: skusinventarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            skusinventarios skusinventarios = db.skusinventarios.Find(id);
            db.skusinventarios.Remove(skusinventarios);
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
