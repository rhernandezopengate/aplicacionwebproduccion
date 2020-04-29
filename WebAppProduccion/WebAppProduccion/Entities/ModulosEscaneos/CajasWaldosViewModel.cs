using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppProduccion.Entities.ModulosEscaneos
{
    public class CajasWaldosViewModel
    {
    }

    public partial class wl_cajas 
    {
        public string Status { get; set; }

        public string Auditor { get; set; }

        public string Picker { get; set; }        
    }
}