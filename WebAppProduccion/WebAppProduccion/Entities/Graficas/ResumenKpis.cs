using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppProduccion.Entities.Graficas
{
    public class ResumenKpis
    {
        public string Nombre { get; set; }
        public int CantidadPiezas { get; set; }

        public int CantidadPaquetes { get; set; }
    }
}