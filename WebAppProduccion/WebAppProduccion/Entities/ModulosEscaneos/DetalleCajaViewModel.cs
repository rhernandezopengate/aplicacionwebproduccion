using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppProduccion.Entities.ModulosEscaneos
{
    public class DetalleCajaViewModel
    {
        public string sku { get; set; }
                
        public string status { get; set; }

        public string guia { get; set; }

        public string codigotienda { get; set; }

        public int cantidad { get; set; }
    }
}