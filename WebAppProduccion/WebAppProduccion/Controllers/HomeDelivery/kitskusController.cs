using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppProduccion.Entities.ModulosEscaneos;
using System.Linq.Dynamic;

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

        public ActionResult CrearDetalleKit(int? id)
        {
            ViewBag.IdKit = id;
            return PartialView();
        }

        [HttpPost]
        public ActionResult CrearDetKit(int? idkit, int idsku, int cantidad)
        {
            try
            {
                kitskus kit = new kitskus();
                kit.kits_Id = (int)idkit;
                kit.skus_Id = idsku;
                kit.Cantidad = cantidad;

                db.kitskus.Add(kit);
                db.SaveChanges();

                return Json(new { respuesta = true, mensaje = "Operacion Correcta." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception _ex)
            {
                return Json(new { respuesta = true, mensaje = _ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }            
        }

        public ActionResult EditarDetalleKit(int? idkit) 
        {
            ViewBag.IdKit = idkit;
            return PartialView();
        }

        [HttpPost]
        public ActionResult ObtenerDetalleKit(int idkit)
        {
            try
            {
                var Draw = Request.Form.GetValues("draw").FirstOrDefault();
                var Start = Request.Form.GetValues("start").FirstOrDefault();
                var Length = Request.Form.GetValues("length").FirstOrDefault();
                var SortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][data]").FirstOrDefault();
                var SortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                
                int PageSize = Length != null ? Convert.ToInt32(Length) : 0;
                int Skip = Start != null ? Convert.ToInt32(Start) : 0;
                int TotalRecords = 0;

                List<kitskus> listaRetorno = new List<kitskus>();

                foreach (var item in db.kitskus.Where(x => x.kits_Id == idkit).ToList())
                {
                    kitskus kitskus = new kitskus();
                    kitskus.id = item.id;
                    kitskus.sku = item.skus.codigobarras;
                    kitskus.Cantidad = item.Cantidad;

                    listaRetorno.Add(kitskus);
                }

                if (!(string.IsNullOrEmpty(SortColumn) && string.IsNullOrEmpty(SortColumnDir)))
                {
                    listaRetorno = listaRetorno.OrderBy(SortColumn + " " + SortColumnDir).ToList();
                }

                TotalRecords = listaRetorno.ToList().Count();
                var NewItems = listaRetorno.Skip(Skip).Take(PageSize == -1 ? TotalRecords : PageSize).ToList();

                return Json(new { draw = Draw, recordsFiltered = TotalRecords, recordsTotal = TotalRecords, data = NewItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception _ex)
            {
                Console.WriteLine(_ex.Message.ToString());
                return null;
            }
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
                return Json(new { respuesta = true, mensaje = "Operacion Correcta." }, JsonRequestBehavior.AllowGet);
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
            return Json(new { respuesta = true, mensaje = "Operacion Correcta." }, JsonRequestBehavior.AllowGet);
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
