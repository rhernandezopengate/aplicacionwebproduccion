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
    public class pickersController : Controller
    {
        private DB_A3F19C_producccionEntities db = new DB_A3F19C_producccionEntities();

        // GET: pickers
        [AuthorizeUser(IdOperacion: 17)]
        public ActionResult Index()
        {
            return View(db.pickers.ToList());
        }

        // GET: pickers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pickers pickers = db.pickers.Find(id);
            if (pickers == null)
            {
                return HttpNotFound();
            }
            return View(pickers);
        }

        // GET: pickers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: pickers/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(IdOperacion: 18)]
        public ActionResult Create([Bind(Include = "id,nombres")] pickers pickers)
        {
            if (ModelState.IsValid)
            {
                db.pickers.Add(pickers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pickers);
        }

        // GET: pickers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pickers pickers = db.pickers.Find(id);
            if (pickers == null)
            {
                return HttpNotFound();
            }
            return View(pickers);
        }

        // POST: pickers/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(IdOperacion: 19)]
        public ActionResult Edit([Bind(Include = "id,nombres")] pickers pickers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pickers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pickers);
        }

        // GET: pickers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pickers pickers = db.pickers.Find(id);
            if (pickers == null)
            {
                return HttpNotFound();
            }
            return View(pickers);
        }

        // POST: pickers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(IdOperacion: 20)]
        public ActionResult DeleteConfirmed(int id)
        {
            pickers pickers = db.pickers.Find(id);
            db.pickers.Remove(pickers);
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
