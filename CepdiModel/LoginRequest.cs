using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CepdiModel
{
    public class LoginRequest
    {
        public required string Usuario { get; set; }
        public required string Password { get; set; }
    }
}
