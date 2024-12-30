using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CepdiModel
{
    public class UsuarioDTO
    {
        public int IdUsuario { get; set; }
        public required string Nombre { get; set; }
        public required string Usuario { get; set; }
        public int Estatus { get; set; }
    }
}
