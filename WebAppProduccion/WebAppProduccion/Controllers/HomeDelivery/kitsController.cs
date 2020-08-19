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

namespace WebAppProduccion.Controllers.HomeDelivery
{
    public class kitsController : Controller
    {
        private DB_A3F19C_producccionEntities db = new DB_A3F19C_producccionEntities();

        // GET: kits
        [AuthorizeUser(IdOperacion: 25)]
        public ActionResult Index()
        {
            return View(db.kits.ToList());
        }

        [HttpPost]
        public ActionResult CrearKitDetalle(string descripcion, string codigobarras, kitskus[] detalleKit) 
        {
            try
            {
                int idkit = AgregarKit(descripcion, codigobarras);
                if (idkit > 0)
                {
                    List<kitskus> elementosAgregar = new List<kitskus>();                   

                    foreach (var item in detalleKit)
                    {
                        kitskus detalleKitTemp = new kitskus();
                        detalleKitTemp.Cantidad = item.Cantidad;
                        detalleKitTemp.skus_Id = db.skus.Where(x => x.codigobarras.Equals(item.sku)).FirstOrDefault().id;
                        detalleKitTemp.kits_Id = idkit;
                        elementosAgregar.Add(detalleKitTemp);
                    }

                    db.kitskus.AddRange(elementosAgregar);
                    db.SaveChanges();
                    return Json(new { respuesta = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { respuesta = false }, JsonRequestBehavior.AllowGet);
                }          
            }
            catch (Exception)
            {
                return Json(new { respuesta = false }, JsonRequestBehavior.AllowGet);
            }            
        }

        public int AgregarKit(string descripcion, string codigobarras) 
        {
            try
            {
                kits kit = new kits();
                kit.descripcion = descripcion;
                kit.codigobarras = codigobarras;

                db.kits.Add(kit);
                db.SaveChanges();

                return kit.id;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public ActionResult EditarKit(int? id) 
        {
            kits kit = db.kits.Where(x => x.id == id).FirstOrDefault();
            ViewBag.Id = kit.id;
            ViewBag.Descripcion = kit.descripcion;
            ViewBag.CodigoBarras = kit.codigobarras;

            return View();        
        }

        [AuthorizeUser(IdOperacion: 27)]
        public ActionResult EditarDetalleKit(int idkit)
        {
            ViewBag.IdKit = idkit;
            return PartialView();
        }

        // GET: kits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            kits kits = db.kits.Find(id);
            if (kits == null)
            {
                return HttpNotFound();
            }
            return View(kits);
        }

        [AuthorizeUser(IdOperacion: 26)]
        public ActionResult CrearKit()
        {
            return View();
        }

        // GET: kits/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: kits/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(IdOperacion: 26)]
        public ActionResult Create([Bind(Include = "id,descripcion,codigobarras")] kits kits)
        {
            if (ModelState.IsValid)
            {
                db.kits.Add(kits);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(kits);
        }

        // GET: kits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            kits kits = db.kits.Find(id);
            if (kits == null)
            {
                return HttpNotFound();
            }
            return View(kits);
        }

        // POST: kits/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(IdOperacion: 27)]
        public ActionResult Edit(int idkit, string descripcion, string codigobarras)
        {            
            return Json(JsonRequestBehavior.AllowGet);
        }

        // GET: kits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            kits kits = db.kits.Find(id);
            if (kits == null)
            {
                return HttpNotFound();
            }
            return View(kits);
        }

        // POST: kits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(IdOperacion: 28)]
        public ActionResult DeleteConfirmed(int id)
        {
            kits kits = db.kits.Find(id);
            db.kits.Remove(kits);
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
