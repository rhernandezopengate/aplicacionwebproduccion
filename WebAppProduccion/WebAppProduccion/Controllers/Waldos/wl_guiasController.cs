using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppProduccion.Entities.ModulosEscaneos;
using System.Linq.Dynamic;
using WebAppProduccion.Filters;

namespace WebAppProduccion.Controllers.Waldos
{
    public class wl_guiasController : Controller
    {
        private DB_A3F19C_producccionEntities db = new DB_A3F19C_producccionEntities();

        // GET: wl_guias
        [AuthorizeUser(IdOperacion: 13)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ObtenerGuias()
        {
            try
            {
                var Draw = Request.Form.GetValues("draw").FirstOrDefault();
                var Start = Request.Form.GetValues("start").FirstOrDefault();
                var Length = Request.Form.GetValues("length").FirstOrDefault();
                var SortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][data]").FirstOrDefault();
                var SortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

                var guia = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();

                int PageSize = Length != null ? Convert.ToInt32(Length) : 0;
                int Skip = Start != null ? Convert.ToInt32(Start) : 0;
                int TotalRecords = 0;

                List<wl_guias> listaRetorno = new List<wl_guias>();

                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionGlobal"].ToString()))
                {
                    con.Open();

                    string sql = "exec [SP_GuiasWaldos_Index_ParametrosOpcionales] @guia";
                    var query = new SqlCommand(sql, con);

                    if (guia != "")
                    {
                        query.Parameters.AddWithValue("@guia", guia);
                    }
                    else
                    {
                        query.Parameters.AddWithValue("@guia", DBNull.Value);
                    }

                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            // facturas
                            var wl_guias = new wl_guias();

                            wl_guias.id = Convert.ToInt32(dr["id"]);
                            wl_guias.guia = dr["guia"].ToString();                            

                            listaRetorno.Add(wl_guias);
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

        // GET: wl_guias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            wl_guias wl_guias = db.wl_guias.Find(id);
            if (wl_guias == null)
            {
                return HttpNotFound();
            }
            return View(wl_guias);
        }

        // GET: wl_guias/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: wl_guias/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(IdOperacion: 14)]
        public ActionResult Create([Bind(Include = "id,guia")] wl_guias wl_guias)
        {
            if (ModelState.IsValid)
            {
                db.wl_guias.Add(wl_guias);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(wl_guias);
        }

        // GET: wl_guias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            wl_guias wl_guias = db.wl_guias.Find(id);
            if (wl_guias == null)
            {
                return HttpNotFound();
            }
            return View(wl_guias);
        }

        // POST: wl_guias/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(IdOperacion: 15)]
        public ActionResult Edit([Bind(Include = "id,guia")] wl_guias wl_guias)
        {
            if (ModelState.IsValid)
            {
                db.Entry(wl_guias).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { respuesta = true }, JsonRequestBehavior.AllowGet);
            }
            return View(wl_guias);
        }

        // GET: wl_guias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            wl_guias wl_guias = db.wl_guias.Find(id);
            if (wl_guias == null)
            {
                return HttpNotFound();
            }
            return View(wl_guias);
        }

        // POST: wl_guias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(IdOperacion: 16)]
        public ActionResult DeleteConfirmed(int id)
        {
            wl_guias wl_guias = db.wl_guias.Find(id);
            db.wl_guias.Remove(wl_guias);
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
