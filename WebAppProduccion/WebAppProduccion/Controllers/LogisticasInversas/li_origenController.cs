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
    public class li_origenController : Controller
    {
        private DB_A3F19C_producccionEntities db = new DB_A3F19C_producccionEntities();

        // GET: li_origen
        public ActionResult Index()
        {
            return View(db.li_origen.ToList());
        }

        // GET: li_origen/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            li_origen li_origen = db.li_origen.Find(id);
            if (li_origen == null)
            {
                return HttpNotFound();
            }
            return View(li_origen);
        }

        // GET: li_origen/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: li_origen/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nomenclatura,descripcion")] li_origen li_origen)
        {
            if (ModelState.IsValid)
            {
                db.li_origen.Add(li_origen);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(li_origen);
        }

        // GET: li_origen/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            li_origen li_origen = db.li_origen.Find(id);
            if (li_origen == null)
            {
                return HttpNotFound();
            }
            return View(li_origen);
        }

        // POST: li_origen/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nomenclatura,descripcion")] li_origen li_origen)
        {
            if (ModelState.IsValid)
            {
                db.Entry(li_origen).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(li_origen);
        }

        // GET: li_origen/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            li_origen li_origen = db.li_origen.Find(id);
            if (li_origen == null)
            {
                return HttpNotFound();
            }
            return View(li_origen);
        }

        // POST: li_origen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            li_origen li_origen = db.li_origen.Find(id);
            db.li_origen.Remove(li_origen);
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
