using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CepdiModel
{
    public class FormaFarmaceutica
    {
        public int IdFormaFarmaceutica { get; set; }
        public required string Nombre { get; set; }
        public int Habilitado { get; set; }
    }
}
