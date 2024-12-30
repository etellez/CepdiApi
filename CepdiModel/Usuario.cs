using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CepdiModel
{
    public class Usuario : CrearUsuarioDTO
    {
        public int IdUsuario { get; set; }
        public DateTime FechaCreacion { get; set; }        
    }
}
