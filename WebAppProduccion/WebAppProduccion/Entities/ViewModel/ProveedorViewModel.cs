using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAppProduccion.Entities.ModulosAdministracion;

namespace WebAppProduccion.Entities.ViewModel
{
    public class ProveedorViewModel
    {
        public string pais { get; set; }

        public string estado { get; set; }

        public string municipio { get; set; }

        public string colonia { get; set; }

        public string calle { get; set; }

        public string cp { get; set; }

        public string nombrebanco { get; set; }

        public string cuentabancaria { get; set; }

        public string clabeinterbancaria { get; set; }

        public string razonsocial { get; set; }

        public string rfc { get; set; }
        
        public string nombrecomercial { get; set; }
        
        public string actividadempresarial { get; set; }

        public string representantelegal { get; set; }

        public int moneda_id { get; set; }

        public int credito_id { get; set; }

        public int categoria_id { get; set; }

        public contactosproveedores[] contactos { get; set; }
    }
}