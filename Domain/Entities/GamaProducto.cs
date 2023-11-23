using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class GamaProducto
    {
        public string Gama { get; set; }
        public string DescripcionTexto { get; set; }
        public string DescripcionHtml { get; set; }
        public string Imagen { get; set; }
        public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}