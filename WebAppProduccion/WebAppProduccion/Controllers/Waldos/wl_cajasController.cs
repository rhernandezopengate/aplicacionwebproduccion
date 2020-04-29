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
    public class wl_cajasController : Controller
    {
        private DB_A3F19C_producccionEntities db = new DB_A3F19C_producccionEntities();

        // GET: wl_cajas
        [AuthorizeUser(IdOperacion: 5)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ObtenerCajas()
        {
            try
            {
                var Draw = Request.Form.GetValues("draw").FirstOrDefault();
                var Start = Request.Form.GetValues("start").FirstOrDefault();
                var Length = Request.Form.GetValues("length").FirstOrDefault();
                var SortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][data]").FirstOrDefault();
                var SortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

                var numerocaja = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
                var idstatus = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();

                int PageSize = Length != null ? Convert.ToInt32(Length) : 0;
                int Skip = Start != null ? Convert.ToInt32(Start) : 0;
                int TotalRecords = 0;

                List<wl_cajas> listaRetorno = new List<wl_cajas>();

                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionGlobal"].ToString()))
                {
                    con.Open();

                    string sql = "exec [SP_CajasWaldos_Index_ParametrosOpcionales] @numerocaja, @idstatus";
                    var query = new SqlCommand(sql, con);

                    if (numerocaja != "")
                    {
                        query.Parameters.AddWithValue("@numerocaja", numerocaja);
                    }
                    else
                    {
                        query.Parameters.AddWithValue("@numerocaja", DBNull.Value);
                    }

                    if (idstatus == "" || idstatus == "0")
                    {
                        query.Parameters.AddWithValue("@idstatus", DBNull.Value);
                    }
                    else
                    {
                        query.Parameters.AddWithValue("@idstatus", idstatus);
                    }

                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            // facturas
                            var wl_cajas = new wl_cajas();

                            wl_cajas.id = Convert.ToInt32(dr["id"]);
                            wl_cajas.Codigo = dr["Codigo"].ToString();
                            wl_cajas.Status = dr["Estado"].ToString();
                            wl_cajas.Auditor = dr["Auditor"].ToString();
                            wl_cajas.Picker = dr["Picker"].ToString();
                            wl_cajas.Paquetes = Convert.ToInt32(dr["Paquetes"]);

                            listaRetorno.Add(wl_cajas);
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

        [HttpPost]
        public JsonResult ListaStatus()
        {
            List<SelectListItem> lista = new List<SelectListItem>();

            foreach (var item in db.wl_statuscajas.ToList())
            {
                lista.Add(new SelectListItem
                {
                    Value = item.id.ToString(),
                    Text = item.descripcion
                });
            }

            return Json(lista);
        }

        public ActionResult DetalleCaja(int? id)
        {
            List<DetalleCajaViewModel> lista = new List<DetalleCajaViewModel>();
            List<skus> listSKUS = db.skus.ToList();

            ViewBag.Guia = db.wl_detenvios.Where(x => x.wl_cajas_Id == id).FirstOrDefault().wl_guias.guia;
            ViewBag.Tienda = db.wl_detenvios.Where(x => x.wl_cajas_Id == id).FirstOrDefault().wl_tiendas.codigo;
            ViewBag.Caja = db.wl_detenvios.Where(x => x.wl_cajas_Id == id).FirstOrDefault().wl_cajas.Codigo;
            ViewBag.StatusCaja = db.wl_detenvios.Where(x => x.wl_cajas_Id == id).FirstOrDefault().wl_cajas.wl_statuscajas.descripcion;
            ViewBag.Paquetes = db.wl_cajas.Where(x => x.id ==  id).FirstOrDefault().Paquetes;

            foreach (var item in db.wl_detcajassku.Where(x => x.wl_cajas_Id == id).ToList())
            {
                DetalleCajaViewModel detalle = new DetalleCajaViewModel();
                detalle.sku = item.skus.codigobarras;
                detalle.cantidad = (int)item.Cantidad;

                lista.Add(detalle);
            }

            return View(lista);
        }

        // GET: wl_cajas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            wl_cajas wl_cajas = db.wl_cajas.Find(id);
            if (wl_cajas == null)
            {
                return HttpNotFound();
            }
            return View(wl_cajas);
        }

        // GET: wl_cajas/Create
        public ActionResult Create()
        {
            ViewBag.wl_statuscajas_Id = new SelectList(db.wl_statuscajas, "id", "descripcion");
            return View();
        }

        // POST: wl_cajas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(IdOperacion: 6)]
        public ActionResult Create([Bind(Include = "id,Codigo,wl_statuscajas_Id")] wl_cajas wl_cajas)
        {
            if (ModelState.IsValid)
            {
                db.wl_cajas.Add(wl_cajas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.wl_statuscajas_Id = new SelectList(db.wl_statuscajas, "id", "descripcion", wl_cajas.wl_statuscajas_Id);
            return View(wl_cajas);
        }

        // GET: wl_cajas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            wl_cajas wl_cajas = db.wl_cajas.Find(id);
            if (wl_cajas == null)
            {
                return HttpNotFound();
            }
            ViewBag.wl_statuscajas_Id = new SelectList(db.wl_statuscajas, "id", "descripcion", wl_cajas.wl_statuscajas_Id);
            ViewBag.Pickers_Id = new SelectList(db.pickers, "id", "nombres", wl_cajas.Pickers_Id);
            ViewBag.Auditores_Id = new SelectList(db.auditores, "id", "nombres", wl_cajas.Auditores_Id);

            return View(wl_cajas);
        }

        // POST: wl_cajas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(IdOperacion: 7)]
        public ActionResult Edit([Bind(Include = "id,Codigo,wl_statuscajas_Id,Pickers_Id,Auditores_Id,Paquetes")] wl_cajas wl_cajas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(wl_cajas).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { respuesta = true }, JsonRequestBehavior.AllowGet);
            }
            ViewBag.wl_statuscajas_Id = new SelectList(db.wl_statuscajas, "id", "descripcion", wl_cajas.wl_statuscajas_Id);
            return View(wl_cajas);
        }

        // GET: wl_cajas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            wl_cajas wl_cajas = db.wl_cajas.Find(id);
            if (wl_cajas == null)
            {
                return HttpNotFound();
            }
            return View(wl_cajas);
        }

        // POST: wl_cajas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(IdOperacion: 8)]
        public ActionResult DeleteConfirmed(int id)
        {
            wl_cajas wl_cajas = db.wl_cajas.Find(id);
            db.wl_cajas.Remove(wl_cajas);
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
