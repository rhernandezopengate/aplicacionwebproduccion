using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppProduccion.Entities.ModulosEscaneos;
using System.Linq.Dynamic;

namespace WebAppProduccion.Controllers.LogisticasInversas
{
    public class logisticasController : Controller
    {
        DB_A3F19C_producccionEntities db = new DB_A3F19C_producccionEntities();
        // GET: logisticas
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ObtenerLogisticas()
        {
            var Draw = Request.Form.GetValues("draw").FirstOrDefault();
            var Start = Request.Form.GetValues("start").FirstOrDefault();
            var Length = Request.Form.GetValues("length").FirstOrDefault();
            var SortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][data]").FirstOrDefault();
            var SortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            var logistica = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();

            int PageSize = Length != null ? Convert.ToInt32(Length) : 0;
            int Skip = Start != null ? Convert.ToInt32(Start) : 0;
            int TotalRecords = 0;

            List<logisticainversa> lista = new List<logisticainversa>();

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionGlobal"].ToString()))
            {
                con.Open();

                string sql = "exec SP_Logisticas_Index_ParametrosOpcionales @folio";
                var query = new SqlCommand(sql, con);

                if (logistica != "")
                {
                    query.Parameters.AddWithValue("@folio", logistica);
                }
                else
                {
                    query.Parameters.AddWithValue("@folio", DBNull.Value);
                }

                using (var dr = query.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        // facturas
                        var logisticasTemp = new logisticainversa();

                        logisticasTemp.id = Convert.ToInt32(dr["id"]);
                        logisticasTemp.Folio = dr["Folio"].ToString();
                        logisticasTemp.FechaAlta = Convert.ToDateTime(dr["FechaAlta"]);
                        logisticasTemp.status = dr["Estado"].ToString();
                        logisticasTemp.origen = dr["Origen"].ToString();

                        lista.Add(logisticasTemp);
                    }
                }
            }

            if (!(string.IsNullOrEmpty(SortColumn) && string.IsNullOrEmpty(SortColumnDir)))
            {
                lista = lista.OrderBy(SortColumn + " " + SortColumnDir).ToList();
            }

            TotalRecords = lista.ToList().Count();
            var NewItems = lista.Skip(Skip).Take(PageSize == -1 ? TotalRecords : PageSize).ToList();

            return Json(new { draw = Draw, recordsFiltered = TotalRecords, recordsTotal = TotalRecords, data = NewItems }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VistaDetalles(int id) 
        {
            logisticainversa logisticainversa = db.logisticainversa.Find(id);
            return View(logisticainversa);
        }

        public ActionResult DetallesLogistica(int idlogistica) 
        {
            var Draw = Request.Form.GetValues("draw").FirstOrDefault();
            var Start = Request.Form.GetValues("start").FirstOrDefault();
            var Length = Request.Form.GetValues("length").FirstOrDefault();
            var SortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][data]").FirstOrDefault();
            var SortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            var sku = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();

            int PageSize = Length != null ? Convert.ToInt32(Length) : 0;
            int Skip = Start != null ? Convert.ToInt32(Start) : 0;
            int TotalRecords = 0;

            List<DetTarimasCajasSkus> lista = new List<DetTarimasCajasSkus>();

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionGlobal"].ToString()))
            {
                con.Open();

                string sql = "exec SP_Logisticas_Detalle_ParametrosOpcionales @idlogistica, @sku";
                var query = new SqlCommand(sql, con);

                query.Parameters.AddWithValue("@idlogistica", idlogistica);

                if (sku != "")
                {
                    query.Parameters.AddWithValue("@sku", sku);
                }
                else
                {
                    query.Parameters.AddWithValue("@sku", DBNull.Value);
                }

                using (var dr = query.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        // facturas
                        var detalle = new DetTarimasCajasSkus();

                        detalle.foliotarima = dr["FolioTarima"].ToString();
                        detalle.foliocaja = dr["FolioCaja"].ToString();
                        detalle.sku = dr["SKU"].ToString();
                        detalle.cantidad = Convert.ToInt32(dr["Cantidad"]);
                        

                        lista.Add(detalle);
                    }
                }
            }

            if (!(string.IsNullOrEmpty(SortColumn) && string.IsNullOrEmpty(SortColumnDir)))
            {
                lista = lista.OrderBy(SortColumn + " " + SortColumnDir).ToList();
            }

            TotalRecords = lista.ToList().Count();
            var NewItems = lista.Skip(Skip).Take(PageSize == -1 ? TotalRecords : PageSize).ToList();

            return Json(new { draw = Draw, recordsFiltered = TotalRecords, recordsTotal = TotalRecords, data = NewItems }, JsonRequestBehavior.AllowGet);
        }
    }
}