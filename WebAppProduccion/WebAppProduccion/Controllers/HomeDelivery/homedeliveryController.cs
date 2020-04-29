using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppProduccion.Entities.Graficas;
using WebAppProduccion.Entities.ModulosEscaneos;
using WebAppProduccion.Filters;

namespace WebAppProduccion.Controllers
{
    public class homedeliveryController : Controller
    {
        DB_A3F19C_producccionEntities db = new DB_A3F19C_producccionEntities();
        // GET: homedelivery
        [AuthorizeUser(IdOperacion: 31)]
        public ActionResult Index()
        {
            DateTime fechaActual = DateTime.Now.Date;

            var cajasabiertas = from w in db.wl_waldos
                                join dt in db.wl_detenvios on w.id equals dt.wl_waldos_Id
                                join c in db.wl_cajas on dt.wl_cajas_Id equals c.id
                                where c.wl_statuscajas_Id == 2 && w.fechaalta.Value > fechaActual
                                select c;

            var cajascerradas = from w in db.wl_waldos
                                join dt in db.wl_detenvios on w.id equals dt.wl_waldos_Id
                                join c in db.wl_cajas on dt.wl_cajas_Id equals c.id
                                where c.wl_statuscajas_Id == 1 && w.fechaalta.Value > fechaActual
                                select c;

            var paquetes = from w in db.wl_waldos
                                join dt in db.wl_detenvios on w.id equals dt.wl_waldos_Id
                                join c in db.wl_cajas on dt.wl_cajas_Id equals c.id
                                where w.fechaalta.Value > fechaActual
                                select c;

            var productividadwaldospickers = (from w in db.wl_waldos
                                             join dt in db.wl_detenvios on w.id equals dt.wl_waldos_Id
                                             join c in db.wl_cajas on dt.wl_cajas_Id equals c.id
                                             join p in db.pickers on c.Pickers_Id equals p.id
                                             join dtc in db.wl_detcajassku on c.id equals dtc.wl_cajas_Id   
                                             where w.fechaalta > fechaActual
                                             select new { dtc.Cantidad, p.nombres }).ToList();

            var productividadwaldosauditores = (from w in db.wl_waldos
                                              join dt in db.wl_detenvios on w.id equals dt.wl_waldos_Id
                                              join c in db.wl_cajas on dt.wl_cajas_Id equals c.id
                                              join a in db.auditores on c.Auditores_Id equals a.id
                                              join dtc in db.wl_detcajassku on c.id equals dtc.wl_cajas_Id
                                              where w.fechaalta > fechaActual
                                              select new { dtc.Cantidad, a.nombres }).ToList();

            List<DataPoint> dataPoints = new List<DataPoint>();
            List<ResumenKpis> listaResumenPickers = new List<ResumenKpis>();
            List<ResumenKpis> listaResumenAuditores = new List<ResumenKpis>();

            foreach (var item in productividadwaldospickers.GroupBy(x => x.nombres))
            {
                if (item.Key != null)
                {
                    //Grafica
                    dataPoints.Add(new DataPoint(item.Key, (double)productividadwaldospickers.Where(x => x.nombres.Equals(item.Key)).Sum(x => x.Cantidad)));
                    
                    //Tabla
                    ResumenKpis resumen = new ResumenKpis();
                    resumen.Nombre = item.Key;
                    resumen.CantidadPiezas = (int)productividadwaldospickers.Where(x => x.nombres.Equals(item.Key)).Sum(x => x.Cantidad);
                    resumen.CantidadPaquetes = (int)paquetes.Where(x => x.pickers.nombres.Equals(item.Key)).Sum(x => x.Paquetes);

                    listaResumenPickers.Add(resumen);
                }
            }

            foreach (var item in productividadwaldosauditores.GroupBy(x => x.nombres))
            {
                if (item.Key != null)
                {                    
                    ResumenKpis resumen = new ResumenKpis();
                    resumen.Nombre = item.Key;
                    resumen.CantidadPiezas = (int)productividadwaldosauditores.Where(x => x.nombres.Equals(item.Key)).Sum(x => x.Cantidad);
                    resumen.CantidadPaquetes = (int)paquetes.Where(x => x.auditores.nombres.Equals(item.Key)).Sum(x => x.Paquetes);

                    listaResumenAuditores.Add(resumen);
                }
            }

            ViewBag.ResumenAuditores = listaResumenAuditores;
            ViewBag.ResumenPickers = listaResumenPickers;
            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            ViewBag.FoliosWaldosAbiertos = cajasabiertas.Count();
            ViewBag.FoliosWaldosCerrados = cajascerradas.Count();
            ViewBag.TotalCajasWaldos = cajasabiertas.Count() + cajascerradas.Count();
            int? totalpaquetes = paquetes.Sum(x => x.Paquetes);

            ViewBag.TotalPaquetesWaldos = totalpaquetes == null ? 0 : totalpaquetes;

            return View();
        }

        [AuthorizeUser(IdOperacion: 32)]
        public ActionResult Ordenes() 
        {
            return View();
        }
    }
}