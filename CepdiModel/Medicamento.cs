using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CepdiModel
{
    public class Medicamento : CrearMedicamentoDTO
    {
        public int IdMedicamento { get; set; }
        public int BHabilitado { get; set; }
    }
}
