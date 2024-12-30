using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CepdiModel
{
    public class CrearMedicamentoDTO
    {
        public required string Nombre { get; set; }
        public required string Concentracion { get; set; }
        public int IdFormaFarmaceutica { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public required string Presentacion { get; set; }
    }
}
