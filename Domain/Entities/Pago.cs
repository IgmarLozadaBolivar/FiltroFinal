using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Pago
    {
        public int CodigoCliente { get; set; }
        public string FormaPago { get; set; }
        public string IdTransaccion { get; set; }
        public DateOnly FechaPago { get; set; }
        public decimal Total { get; set; }
        public virtual Cliente CodigoClienteNavigation { get; set; }
    }
}