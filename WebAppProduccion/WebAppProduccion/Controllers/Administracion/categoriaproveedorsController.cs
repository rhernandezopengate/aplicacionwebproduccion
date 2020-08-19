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
    public class categoriaproveedorsController : Controller
    {
        private DB_A3F19C_producccionEntities2 db = new DB_A3F19C_producccionEntities2();

        // GET: categoriaproveedors
        public ActionResult Index()
        {
            return View(db.categoriaproveedor.ToList());
        }

        [HttpPost]
        public JsonResult ListaCategoria()
        {
            List<SelectListItem> lista = new List<SelectListItem>();

            foreach (var item in db.categoriaproveedor.OrderBy(x => x.descripcion).ToList())
            {
                lista.Add(new SelectListItem
                {
                    Value = item.id.ToString(),
                    Text = item.descripcion
                });
            }

            return Json(lista);
        }

        // GET: categoriaproveedors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            categoriaproveedor categoriaproveedor = db.categoriaproveedor.Find(id);
            if (categoriaproveedor == null)
            {
                return HttpNotFound();
            }
            return View(categoriaproveedor);
        }

        // GET: categoriaproveedors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: categoriaproveedors/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,descripcion")] categoriaproveedor categoriaproveedor)
        {
            if (ModelState.IsValid)
            {
                db.categoriaproveedor.Add(categoriaproveedor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(categoriaproveedor);
        }

        // GET: categoriaproveedors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            categoriaproveedor categoriaproveedor = db.categoriaproveedor.Find(id);
            if (categoriaproveedor == null)
            {
                return HttpNotFound();
            }
            return View(categoriaproveedor);
        }

        // POST: categoriaproveedors/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,descripcion")] categoriaproveedor categoriaproveedor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(categoriaproveedor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(categoriaproveedor);
        }

        // GET: categoriaproveedors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            categoriaproveedor categoriaproveedor = db.categoriaproveedor.Find(id);
            if (categoriaproveedor == null)
            {
                return HttpNotFound();
            }
            return View(categoriaproveedor);
        }

        // POST: categoriaproveedors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            categoriaproveedor categoriaproveedor = db.categoriaproveedor.Find(id);
            db.categoriaproveedor.Remove(categoriaproveedor);
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
