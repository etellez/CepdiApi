using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CepdiModel
{
    public class CrearUsuarioDTO
    {
        public required string Nombre { get; set; }
        
        public required string Usuario { get; set; }
        public required string Password { get; set; }
        public int Estatus { get; set; }
        public int IdPerfil { get; set; }
        
    }
}
