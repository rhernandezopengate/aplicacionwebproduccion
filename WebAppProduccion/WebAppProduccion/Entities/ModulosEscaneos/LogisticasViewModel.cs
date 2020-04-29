using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppProduccion.Entities.ModulosEscaneos
{
    public class LogisticasViewModel
    {
    }

    public class DetTarimasCajasSkus
    {
        public string foliotarima { get; set; }

        public string foliocaja { get; set; }

        public string sku { get; set; }

        public int cantidad { get; set; }
    }

    public partial class logisticainversa 
    {
        public string origen { get; set; }
        
        public string status { get; set; }
    }
}