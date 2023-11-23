using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Empleado
    {
        public int CodigoEmpleado { get; set; }
        public string Nombre { get; set; }
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }
        public string Extension { get; set; }
        public string Email { get; set; }
        public string CodigoOficina { get; set; }
        public int? CodigoJefe { get; set; }
        public string Puesto { get; set; }
        public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
        public virtual Empleado CodigoJefeNavigation { get; set; }
        public virtual Oficina CodigoOficinaNavigation { get; set; }
        public virtual ICollection<Empleado> InverseCodigoJefeNavigation { get; set; } = new List<Empleado>();
    }
}