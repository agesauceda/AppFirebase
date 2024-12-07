using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFirebase.Models
{
    public class Producto
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Descripción { get; set; }
        public double? Precio { get; set; }
        public string Foto { get; set; }
    }
}
