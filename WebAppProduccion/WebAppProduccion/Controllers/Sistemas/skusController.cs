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
    public class skusController : Controller
    {
        private DB_A3F19C_producccionEntities db = new DB_A3F19C_producccionEntities();

        // GET: skus
        public ActionResult Index()
        {
            var skus = db.skus.Include(s => s.uom);
            return View(skus.ToList());
        }

        [HttpPost]
        public JsonResult ListaSkus()
        {
            List<SelectListItem> lista = new List<SelectListItem>();

            foreach (var item in db.skus.ToList().OrderByDescending(x => x.id))
            {
                lista.Add(new SelectListItem
                {
                    Value = item.id.ToString(),
                    Text = item.codigobarras
                });
            }

            return Json(lista);
        }

        [HttpPost]
        public JsonResult AutoComplete(string prefix)
        {
            var skus = (from s in db.skus
                               where s.codigobarras.Contains(prefix)
                               orderby s.codigobarras ascending
                               select new
                               {
                                   label = s.codigobarras,
                                   val = s.id
                               }).ToList();

            return Json(skus);
        }

        // GET: skus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            skus skus = db.skus.Find(id);
            if (skus == null)
            {
                return HttpNotFound();
            }
            return View(skus);
        }

        // GET: skus/Create
        public ActionResult Create()
        {
            ViewBag.uom_id = new SelectList(db.uom, "id", "descripcion");
            return View();
        }

        // POST: skus/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,SKU,Descripcion,codigobarras,uom_id")] skus skus)
        {
            if (ModelState.IsValid)
            {
                db.skus.Add(skus);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.uom_id = new SelectList(db.uom, "id", "descripcion", skus.uom_id);
            return View(skus);
        }

        // GET: skus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            skus skus = db.skus.Find(id);
            if (skus == null)
            {
                return HttpNotFound();
            }
            ViewBag.uom_id = new SelectList(db.uom, "id", "descripcion", skus.uom_id);
            return View(skus);
        }

        // POST: skus/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,SKU,Descripcion,codigobarras,uom_id")] skus skus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(skus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.uom_id = new SelectList(db.uom, "id", "descripcion", skus.uom_id);
            return View(skus);
        }

        // GET: skus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            skus skus = db.skus.Find(id);
            if (skus == null)
            {
                return HttpNotFound();
            }
            return View(skus);
        }

        // POST: skus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            skus skus = db.skus.Find(id);
            db.skus.Remove(skus);
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
