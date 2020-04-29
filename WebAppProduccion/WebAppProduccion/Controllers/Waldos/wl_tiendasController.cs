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
using System.Data.SqlClient;
using System.Configuration;
using WebAppProduccion.Filters;

namespace WebAppProduccion.Controllers.Waldos
{
    public class wl_tiendasController : Controller
    {
        private DB_A3F19C_producccionEntities db = new DB_A3F19C_producccionEntities();

        // GET: wl_tiendas
        [AuthorizeUser(IdOperacion: 9)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ObtenerTienda()
        {
            try
            {
                var Draw = Request.Form.GetValues("draw").FirstOrDefault();
                var Start = Request.Form.GetValues("start").FirstOrDefault();
                var Length = Request.Form.GetValues("length").FirstOrDefault();
                var SortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][data]").FirstOrDefault();
                var SortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

                var codigo = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();

                int PageSize = Length != null ? Convert.ToInt32(Length) : 0;
                int Skip = Start != null ? Convert.ToInt32(Start) : 0;
                int TotalRecords = 0;

                List<wl_tiendas> listaRetorno = new List<wl_tiendas>();

                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionGlobal"].ToString()))
                {
                    con.Open();

                    string sql = "exec [SP_TiendasWaldos_Index_ParametrosOpcionales] @codigotienda";
                    var query = new SqlCommand(sql, con);

                    if (codigo != "")
                    {
                        query.Parameters.AddWithValue("@codigotienda", codigo);
                    }
                    else
                    {
                        query.Parameters.AddWithValue("@codigotienda", DBNull.Value);
                    }

                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            // facturas
                            var wl_tiendas = new wl_tiendas();

                            wl_tiendas.id = Convert.ToInt32(dr["id"]);
                            wl_tiendas.codigo = dr["codigo"].ToString();
                            wl_tiendas.descripcion = dr["descripcion"].ToString();

                            listaRetorno.Add(wl_tiendas);
                        }
                    }
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

        // GET: wl_tiendas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            wl_tiendas wl_tiendas = db.wl_tiendas.Find(id);
            if (wl_tiendas == null)
            {
                return HttpNotFound();
            }
            return View(wl_tiendas);
        }

        // GET: wl_tiendas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: wl_tiendas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(IdOperacion: 10)]
        public ActionResult Create([Bind(Include = "id,codigo,descripcion")] wl_tiendas wl_tiendas)
        {
            if (ModelState.IsValid)
            {
                db.wl_tiendas.Add(wl_tiendas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(wl_tiendas);
        }

        // GET: wl_tiendas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            wl_tiendas wl_tiendas = db.wl_tiendas.Find(id);
            if (wl_tiendas == null)
            {
                return HttpNotFound();
            }
            return View(wl_tiendas);
        }

        // POST: wl_tiendas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(IdOperacion: 11)]
        public ActionResult Edit([Bind(Include = "id,codigo,descripcion")] wl_tiendas wl_tiendas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(wl_tiendas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(wl_tiendas);
        }

        // GET: wl_tiendas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            wl_tiendas wl_tiendas = db.wl_tiendas.Find(id);
            if (wl_tiendas == null)
            {
                return HttpNotFound();
            }
            return View(wl_tiendas);
        }

        // POST: wl_tiendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(IdOperacion: 12)]
        public ActionResult DeleteConfirmed(int id)
        {
            wl_tiendas wl_tiendas = db.wl_tiendas.Find(id);
            db.wl_tiendas.Remove(wl_tiendas);
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
