using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppProduccion.Entities.ModulosAdministracion;
using WebAppProduccion.Entities.ViewModel;
using System.Linq.Dynamic;

namespace WebAppProduccion.Controllers.Administracion
{
    public class proveedoresController : Controller
    {
        DB_A3F19C_producccionEntities2 db = new DB_A3F19C_producccionEntities2();
        // GET: proveedores
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ObtenerProveedores()
        {
            try
            {
                var Draw = Request.Form.GetValues("draw").FirstOrDefault();
                var Start = Request.Form.GetValues("start").FirstOrDefault();
                var Length = Request.Form.GetValues("length").FirstOrDefault();
                var SortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][data]").FirstOrDefault();
                var SortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

                var razonsocial = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();                

                int PageSize = Length != null ? Convert.ToInt32(Length) : 0;
                int Skip = Start != null ? Convert.ToInt32(Start) : 0;
                int TotalRecords = 0;

                List<proveedores> listaRetorno = new List<proveedores>();

                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionGlobal"].ToString()))
                {
                    con.Open();

                    string sql = "exec [SP_Proveedores_Index_ParametrosOpcionales] @razonsocial";
                    var query = new SqlCommand(sql, con);

                    if (razonsocial != "")
                    {
                        query.Parameters.AddWithValue("@razonsocial", razonsocial);
                    }
                    else
                    {
                        query.Parameters.AddWithValue("@razonsocial", DBNull.Value);
                    }                   

                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            // facturas
                            var proveedores = new proveedores();

                            proveedores.id = Convert.ToInt32(dr["id"]);
                            proveedores.RazonSocial = dr["RazonSocial"].ToString();
                            proveedores.NombreComercial = dr["NombreComercial"].ToString();

                            listaRetorno.Add(proveedores);
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

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateProveedor(ProveedorViewModel data)
        {
            direccionproveedor direccionproveedor = new direccionproveedor();
            direccionproveedor.pais = data.pais.ToUpper().Trim();
            direccionproveedor.estado = data.estado.ToUpper().Trim();
            direccionproveedor.municipio = data.municipio.ToUpper().Trim();
            direccionproveedor.colonia = data.colonia.ToUpper().Trim();
            direccionproveedor.calle = data.calle.ToUpper().Trim();
            direccionproveedor.cp = data.cp.ToUpper().Trim();

            int iddireccionagregada = CrearDireccion(direccionproveedor);
            
            informacionbancaria informacionbancaria = new informacionbancaria();
            informacionbancaria.nombrebanco = data.nombrebanco.ToUpper().Trim();
            informacionbancaria.cuentabancaria = data.cuentabancaria;
            informacionbancaria.claveinterbancaria = data.clabeinterbancaria;

            int idinformacionbancariaagregada = CrearInformacionBancaria(informacionbancaria);

            proveedores proveedor = new proveedores();

            proveedor.FechaAlta = DateTime.Now.AddHours(2);
            proveedor.RazonSocial = data.razonsocial.ToUpper().Trim();
            proveedor.RFC = data.rfc.ToUpper().Trim();
            proveedor.NombreComercial = data.nombrecomercial.ToUpper().Trim();
            proveedor.ActividadEmpresarial = data.actividadempresarial.ToUpper().Trim();
            proveedor.RepresentanteLegal = data.representantelegal.ToUpper().Trim();            
            proveedor.InformacionBancaria_Id = idinformacionbancariaagregada;
            proveedor.DireccionProveedor_Id = iddireccionagregada;
            proveedor.StatusProveedor_Id = 1;
            proveedor.MonedaFacturacion_Id = data.moneda_id;
            proveedor.CategoriaProveedor_Id = data.categoria_id;
            proveedor.Credito_Id = data.credito_id;

            int idproveedoragregado = CrearProveedor(proveedor);

            CrearContactosProveedor(data.contactos, idproveedoragregado);

            return Json(new { respuesta = true }, JsonRequestBehavior.AllowGet );
        }

        public void CrearContactosProveedor(contactosproveedores [] _contactos, int _idproveedor) 
        {
            try
            {
                List<contactosproveedores> lista = new List<contactosproveedores>();

                if (_contactos != null)
                {
                    foreach (var item in _contactos)
                    {
                        contactosproveedores contacto = new contactosproveedores();

                        contacto.Proveedores_Id = _idproveedor;
                        contacto.Nombres = item.Nombres.ToUpper().Trim();
                        contacto.Email = item.Email;
                        contacto.Puesto = item.Puesto.ToUpper().Trim();
                        contacto.Telefono = item.Telefono;
                        lista.Add(contacto);
                    }

                    db.contactosproveedores.AddRange(lista);
                    db.SaveChanges();
                }
            }
            catch (Exception _ex)
            {
                Console.WriteLine(_ex.Message.ToString());
                throw;
            }
                   
        }

        public int CrearProveedor(proveedores _proveedor) 
        {
            try
            {
                proveedores proveedores = _proveedor;
                db.proveedores.Add(proveedores);
                db.SaveChanges();
                return proveedores.id;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int CrearInformacionBancaria(informacionbancaria _informacionbancaria)
        {
            try
            {
                informacionbancaria informacionbancaria = _informacionbancaria;
                db.informacionbancaria.Add(informacionbancaria);
                db.SaveChanges();

                return informacionbancaria.id;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int CrearDireccion(direccionproveedor _direccionproveedor) 
        {
            try
            {
                direccionproveedor direccionproveedor = _direccionproveedor;
                db.direccionproveedor.Add(direccionproveedor);
                db.SaveChanges();

                return direccionproveedor.id;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public ActionResult Edit(int? id)
        {
            return View();
        }

        [HttpPost]
        public JsonResult ListaPuestos()
        {
            List<SelectListItem> lista = new List<SelectListItem>();

            SelectListItem listItem = new SelectListItem();
            listItem.Value = "FACTURACION";
            listItem.Text = "FACTURACION";

            SelectListItem listItem2 = new SelectListItem();
            listItem2.Value = "COBRANZA";
            listItem2.Text = "COBRANZA";

            SelectListItem listItem3 = new SelectListItem();
            listItem3.Value = "FACTURACION Y COBRANZA";
            listItem3.Text = "FACTURACION Y COBRANZA";

            SelectListItem listItem4 = new SelectListItem();
            listItem4.Value = "PROPIETARIO";
            listItem4.Text = "PROPIETARIO";

            SelectListItem listItem5 = new SelectListItem();
            listItem5.Value = "ADMINISTRACION";
            listItem5.Text = "ADMINISTRACION";

            SelectListItem listItem6 = new SelectListItem();
            listItem6.Value = "VENTAS";
            listItem6.Text = "VENTAS";

            SelectListItem listItem7 = new SelectListItem();
            listItem7.Value = "CONTADURIA";
            listItem7.Text = "CONTADURIA";

            SelectListItem listItem8 = new SelectListItem();
            listItem8.Value = "GERENCIA DE FINANZAS";
            listItem8.Text = "GERENCIA DE FINANZAS";

            SelectListItem listItem9 = new SelectListItem();
            listItem9.Value = "JEFE DE SERVICIO POSTVENTA";
            listItem9.Text = "JEFE DE SERVICIO POSTVENTA";

            lista.Add(listItem);
            lista.Add(listItem2);
            lista.Add(listItem3);
            lista.Add(listItem4);
            lista.Add(listItem5);
            lista.Add(listItem6);
            lista.Add(listItem7);
            lista.Add(listItem8);
            lista.Add(listItem9);

            var listaretorno = lista.OrderBy(x => x.Text);

            return Json(listaretorno);
        }
    }
}