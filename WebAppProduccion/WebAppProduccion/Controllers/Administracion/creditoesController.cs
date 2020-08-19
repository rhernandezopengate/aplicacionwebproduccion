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
    public class creditoesController : Controller
    {
        private DB_A3F19C_producccionEntities2 db = new DB_A3F19C_producccionEntities2();

        // GET: creditoes
        public ActionResult Index()
        {
            return View(db.credito.ToList());
        }

        [HttpPost]
        public JsonResult ListaCredito()
        {
            List<SelectListItem> lista = new List<SelectListItem>();

            foreach (var item in db.credito.OrderByDescending(x => x.dias).ToList())
            {
                lista.Add(new SelectListItem
                {
                    Value = item.id.ToString(),
                    Text = item.descripcion
                });
            }

            return Json(lista);
        }

        // GET: creditoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            credito credito = db.credito.Find(id);
            if (credito == null)
            {
                return HttpNotFound();
            }
            return View(credito);
        }

        // GET: creditoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: creditoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,descripcion,dias")] credito credito)
        {
            if (ModelState.IsValid)
            {
                db.credito.Add(credito);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(credito);
        }

        // GET: creditoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            credito credito = db.credito.Find(id);
            if (credito == null)
            {
                return HttpNotFound();
            }
            return View(credito);
        }

        // POST: creditoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,descripcion,dias")] credito credito)
        {
            if (ModelState.IsValid)
            {
                db.Entry(credito).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(credito);
        }

        // GET: creditoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            credito credito = db.credito.Find(id);
            if (credito == null)
            {
                return HttpNotFound();
            }
            return View(credito);
        }

        // POST: creditoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            credito credito = db.credito.Find(id);
            db.credito.Remove(credito);
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
