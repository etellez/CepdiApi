using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CepdiModel
{
    public class MedicamentoWithFormaFarmaceutica : Medicamento
    {
        public required string FormaFarmaceuticaNombre { get; set; }

        public required string FormaFarmaceuticaHabilitado { get; set; }
    }
}
