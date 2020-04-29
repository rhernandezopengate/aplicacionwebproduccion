using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebAppProduccion.Entities.ModulosEscaneos;
using WebAppProduccion.Filters;

namespace WebAppProduccion.Controllers.Waldos
{
    public class waldosController : Controller
    {
        DB_A3F19C_producccionEntities db = new DB_A3F19C_producccionEntities();

        // GET: waldos
        [AuthorizeUser(IdOperacion: 29)]
        public ActionResult Index()
        {
            return View(db.wl_waldos.ToList());
        }

        [AuthorizeUser(IdOperacion: 30)]
        public ActionResult CargarArchivo()
        {
            return View();
        }

        public ActionResult DetalleEnvios(int? id) 
        {
            List<wl_waldos> lista = new List<wl_waldos>();
            
            ViewBag.Folio = db.wl_waldos.Where(x => x.id == id).FirstOrDefault().folio;
            ViewBag.Fecha = db.wl_waldos.Where(x => x.id == id).FirstOrDefault().fechaalta;
            
            //ViewBag.Tienda = db.wl_detenvios.Where(x => x.wl_cajas_Id == id).FirstOrDefault().wl_tiendas.codigo;
            //ViewBag.Caja = db.wl_detenvios.Where(x => x.wl_cajas_Id == id).FirstOrDefault().wl_cajas.Codigo;
            //ViewBag.StatusCaja = db.wl_detenvios.Where(x => x.wl_cajas_Id == id).FirstOrDefault().wl_cajas.wl_statuscajas.descripcion;

            int cajas = 0;
            foreach (var item in db.wl_detenvios.Where(x => x.wl_waldos_Id == id).ToList())
            {
                wl_waldos detalle = new wl_waldos();
                detalle.id = item.id;
                detalle.caja = item.wl_cajas.Codigo;
                detalle.guia = item.wl_guias.guia;
                detalle.tienda = item.wl_tiendas.codigo;
                cajas++;

                lista.Add(detalle);
            }

            ViewBag.CantidadCajas = cajas;

            return View(lista);
        }

        [HttpPost]        
        public async Task<ActionResult> Cargar(HttpPostedFileBase postedFileBase)
        {
            try
            {
                string filePath = string.Empty;

                if (postedFileBase != null)
                {
                    string path = Server.MapPath("~/Uploads/");

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    filePath = path + Path.GetFileName(postedFileBase.FileName);
                    string extension = Path.GetExtension(postedFileBase.FileName);
                    postedFileBase.SaveAs(filePath);

                    DataTable dtCharge = new DataTable();

                    dtCharge.Columns.AddRange(new DataColumn[9] {
                        //0
                        new DataColumn("codigotienda", typeof(string)),
                        //1                        
                        new DataColumn("nombretienda", typeof(string)),
                        //2
                        new DataColumn("fecha", typeof(string)),
                        //3
                        new DataColumn("numerocaja", typeof(string)),
                        //4
                        new DataColumn("peso", typeof(decimal)),
                        //5
                        new DataColumn("espaciolibre", typeof(decimal)),
                        //6
                        new DataColumn("guia", typeof(string)),
                        //7
                        new DataColumn("sku", typeof(string)),
                        //8
                        new DataColumn("enviados", typeof(int)),
                    });

                    string csvData = System.IO.File.ReadAllText(filePath);

                    foreach (string row in csvData.Split('\n'))
                    {
                        if (!string.IsNullOrEmpty(row))
                        {
                            dtCharge.Rows.Add();
                            int i = 0;

                            //Execute a loop over the columns.
                            foreach (string cell in row.Split(','))
                            {
                                dtCharge.Rows[dtCharge.Rows.Count - 1][i] = cell;

                                i++;
                            }
                        }
                    }

                    bool validacion = await ValidacionArchivo(dtCharge);
                    bool insercion = await InsercionDetalleCajas(dtCharge);
                    bool insercionenvios = await InsercionDetalleEnvios(dtCharge);
                }

                return RedirectToAction("Index");
            }
            catch (Exception _ex)
            {
                string error = _ex.Message.ToString();
                return RedirectToAction("Error500", "Errores", new { error = error });
            }
        }

        public async Task<bool> ValidacionArchivo(DataTable dtCharge)
        {
            try
            {
                //Preparacion de listas para consulta
                //SKUS
                List<skus> listaSKUSBusqueda = await ListaSKUSAsync();
                //Tiendas
                List<wl_tiendas> listaTiendasBusqueda = await ListaTiendasAsync();
                //Cajas
                List<wl_cajas> listaCajasBusqueda = await ListaCajasAsync();
                //Guias
                List<wl_guias> listaGuiasBusqueda = await ListaGuiasAsync();


                //Validacion de Skus
                List<skus> listaSKUS = new List<skus>();
                List<wl_tiendas> listaTiendas = new List<wl_tiendas>();
                List<wl_cajas> listaCajas = new List<wl_cajas>();
                List<wl_guias> listaGuias = new List<wl_guias>();

                //Validacion de Datos en Archivo
                foreach (DataRow row in dtCharge.Rows)
                {
                    string sku = row[7].ToString().Trim().ToUpper();
                    var skuexistente = listaSKUSBusqueda.Where(x => x.codigobarras.Equals(sku)).FirstOrDefault();
                    var skuenlista = listaSKUS.Where(x => x.codigobarras.Equals(sku)).FirstOrDefault();

                    if (skuexistente == null)
                    {
                        if (skuenlista == null)
                        {
                            skus skuTemp = new skus();
                            skuTemp.codigobarras = sku;
                            skuTemp.SKU = sku;
                            skuTemp.uom_id = 1;

                            listaSKUS.Add(skuTemp);
                        }
                    }

                    string tiendacodigo = row[0].ToString().Trim().ToUpper();
                    var tiendaexistente = listaTiendasBusqueda.Where(x => x.codigo.Equals(tiendacodigo)).FirstOrDefault();
                    var tiendaenlista = listaTiendas.Where(x => x.codigo.Equals(tiendacodigo)).FirstOrDefault();

                    if (tiendaexistente == null)
                    {
                        if (tiendaenlista == null)
                        {
                            wl_tiendas tienda = new wl_tiendas();
                            tienda.codigo = tiendacodigo;
                            tienda.descripcion = row[1].ToString().Trim().ToUpper();

                            listaTiendas.Add(tienda);
                        }
                    }

                    string cajacodigo = row[3].ToString().Trim().ToUpper();
                    var cajaexistente = listaCajasBusqueda.Where(x => x.Codigo.Equals(cajacodigo)).FirstOrDefault();
                    var cajaenlista = listaCajas.Where(x => x.Codigo.Equals(cajacodigo)).FirstOrDefault();

                    if (cajaexistente == null)
                    {
                        if (cajaenlista == null)
                        {
                            wl_cajas caja = new wl_cajas();
                            caja.Codigo = cajacodigo;
                            caja.wl_statuscajas_Id = 1;
                            caja.Pickers_Id = 1;
                            caja.Auditores_Id = 1;
                            caja.Paquetes = 1;

                            listaCajas.Add(caja);
                        }
                    }

                    string guia = row[6].ToString().Trim().ToUpper();
                    var guiaexistente = listaGuiasBusqueda.Where(x => x.guia.Equals(guia)).FirstOrDefault();
                    var guiaenlista = listaGuias.Where(x => x.guia.Equals(guia)).FirstOrDefault();

                    if (guiaexistente == null)
                    {
                        if (guiaenlista == null)
                        {
                            wl_guias guiaTemp = new wl_guias();
                            guiaTemp.guia = guia;

                            listaGuias.Add(guiaTemp);
                        }
                    }
                }

                if (listaSKUS.Count > 0)
                {
                    db.skus.AddRange(listaSKUS);
                    await db.SaveChangesAsync();
                }

                if (listaTiendas.Count > 0)
                {
                    db.wl_tiendas.AddRange(listaTiendas);
                    await db.SaveChangesAsync();
                }

                if (listaGuias.Count > 0)
                {
                    db.wl_guias.AddRange(listaGuias);
                    await db.SaveChangesAsync();
                }

                if (listaCajas.Count > 0)
                {
                    db.wl_cajas.AddRange(listaCajas);
                    await db.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> InsercionDetalleCajas(DataTable dtCharge)
        {
            try
            {
                //SKUS
                List<skus> listaSKUSBusqueda = await ListaSKUSAsync();
                //Cajas
                List<wl_cajas> listaCajasBusqueda = await ListaCajasAsync();
                //Lista Para Agregar Datos
                List<wl_detcajassku> listaDetalleCajas = new List<wl_detcajassku>();

                foreach (DataRow row in dtCharge.Rows)
                {
                    wl_detcajassku detalle = new wl_detcajassku();
                    string sku = row[7].ToString().Trim().ToUpper();
                    string cajacodigo = row[3].ToString().Trim().ToUpper();
                    int cantidad = Convert.ToInt32(row[8]);
                    int idsku = listaSKUSBusqueda.Where(x => x.codigobarras.Equals(sku)).FirstOrDefault().id;
                    int idcaja = listaCajasBusqueda.Where(x => x.Codigo.Equals(cajacodigo)).FirstOrDefault().id;

                    detalle.skus_Id = idsku;
                    detalle.wl_cajas_Id = idcaja;
                    detalle.Cantidad = cantidad;                    

                    listaDetalleCajas.Add(detalle);
                }

                db.wl_detcajassku.AddRange(listaDetalleCajas);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> InsercionDetalleEnvios(DataTable dtCharge)
        {
            try
            {
                List<wl_guias> listaGuiasBusqueda = await ListaGuiasAsync();
                List<wl_cajas> listaCajasBusqueda = await ListaCajasAsync();
                List<wl_tiendas> listaTiendasBusqueda = await ListaTiendasAsync();

                string folioenvio = "OGL-WLD-" + (db.wl_waldos.Count() + 1);
                wl_waldos waldos = new wl_waldos();
                waldos.folio = folioenvio;
                waldos.fechaalta = DateTime.Now;

                db.wl_waldos.Add(waldos);
                await db.SaveChangesAsync();

                int idenvio = waldos.id;

                List<wl_detenvios> listaEnvios = new List<wl_detenvios>();

                foreach (DataRow row in dtCharge.Rows)
                {
                    string cajacodigo = row[3].ToString().Trim().ToUpper();
                    string guia = row[6].ToString().Trim().ToUpper();
                    string codigotienda = row[0].ToString().Trim().ToUpper();

                    wl_detenvios detenvios = new wl_detenvios();
                    detenvios.wl_cajas_Id = listaCajasBusqueda.Where(x => x.Codigo.Equals(cajacodigo)).FirstOrDefault().id;
                    detenvios.wl_guias_Id = listaGuiasBusqueda.Where(x => x.guia.Equals(guia)).FirstOrDefault().id;
                    detenvios.wl_tiendas_Id = listaTiendasBusqueda.Where(x => x.codigo.Equals(codigotienda)).FirstOrDefault().id;

                    listaEnvios.Add(detenvios);
                }

                var listaAgrupada = listaEnvios.GroupBy(x => x.wl_cajas_Id);

                List<wl_detenvios> listaEnviosAgregar = new List<wl_detenvios>();

                foreach (var item in listaAgrupada)
                {
                    wl_detenvios detenvios = new wl_detenvios();
                    detenvios.wl_cajas_Id = item.Key;
                    detenvios.wl_guias_Id = listaEnvios.Where(x => x.wl_cajas_Id.Equals(item.Key)).FirstOrDefault().wl_guias_Id;
                    detenvios.wl_tiendas_Id = listaEnvios.Where(x => x.wl_cajas_Id.Equals(item.Key)).FirstOrDefault().wl_tiendas_Id;
                    detenvios.wl_waldos_Id = idenvio;

                    listaEnviosAgregar.Add(detenvios);
                }

                db.wl_detenvios.AddRange(listaEnviosAgregar);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private Task<List<wl_guias>> ListaGuiasAsync()
        {
            return Task.Run(() => db.wl_guias.ToList());
        }

        private Task<List<wl_cajas>> ListaCajasAsync()
        {
            return Task.Run(() => db.wl_cajas.ToList());
        }

        private Task<List<wl_tiendas>> ListaTiendasAsync()
        {
            return Task.Run(() => db.wl_tiendas.ToList());
        }

        private Task<List<skus>> ListaSKUSAsync()
        {
            return Task.Run(() => db.skus.ToList());
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