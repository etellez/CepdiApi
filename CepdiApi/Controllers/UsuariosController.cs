using CepdiModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using AutoMapper;

namespace CepdiApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IDatabaseService _databaseService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UsuariosController(IDatabaseService databaseService, IConfiguration configuration, IMapper mapper)
        {
            _databaseService = databaseService;
            _configuration = configuration;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var sql = "SELECT * FROM Usuarios WHERE Usuario = @Usuario AND Password = @Password";

            try
            {
                var usuario = await _databaseService.QuerySingleAsync<Usuario>(sql, new { loginRequest.Usuario, Password = loginRequest.Password });

                if (usuario == null)
                {
                    return Unauthorized(new { Message = "Usuario o contraseña incorrectos." });
                }

                var token = GenerateJwtToken(usuario);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error al iniciar sesión.", Details = ex.Message });
            }
        }

        private string GenerateJwtToken(Usuario usuario)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.IdUsuario.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, usuario.Nombre ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Usuario ?? string.Empty),
                new Claim("role", usuario.IdPerfil.ToString())
            };

            var keyString = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured.");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CrearUsuario([FromBody] CrearUsuarioDTO crearUsuarioDto)
        {
            // Validar la contraseña
            var passwordValidationResult = ValidarPassword(crearUsuarioDto.Password);
            if (!passwordValidationResult.IsValid)
            {
                return BadRequest(new { Message = "La contraseña no cumple con los requisitos.", Details = passwordValidationResult.Message });
            }

            Usuario usuario = _mapper.Map<Usuario>(crearUsuarioDto);
            usuario.Estatus = 1;
            usuario.FechaCreacion = DateTime.Now;

            var sql = "INSERT INTO Usuarios (Nombre, FechaCreacion, Usuario, Password, IdPerfil, Estatus) VALUES (@Nombre, @FechaCreacion, @Usuario, @Password, @IdPerfil, @Estatus)";

            try
            {
                var rowsAffected = await _databaseService.ExecuteAsync(sql, usuario);
                return Ok(new { Message = "Usuario creado exitosamente.", RowsAffected = rowsAffected });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error al crear el usuario.", Details = ex.Message });
            }
        }


        [HttpGet("validate-password")]
        [AllowAnonymous]
        public IActionResult ValidarPasswordEndpoint([FromQuery] string password)
        {
            var result = ValidarPassword(password);
            if (!result.IsValid)
            {
                return BadRequest(new { Message = result.Message });
            }

            return Ok(new { Message = "La contraseña cumple con los requisitos." });
        }

        private (bool IsValid, string Message) ValidarPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return (false, "La contraseña no puede estar vacía.");
            }

            if (password.Length < 8)
            {
                return (false, "La contraseña debe tener al menos 8 caracteres.");
            }

            if (!Regex.IsMatch(password, "[A-Z]"))
            {
                return (false, "La contraseña debe contener al menos una letra mayúscula.");
            }

            if (!Regex.IsMatch(password, "[a-z]"))
            {
                return (false, "La contraseña debe contener al menos una letra minúscula.");
            }

            if (!Regex.IsMatch(password, "\\d"))
            {
                return (false, "La contraseña debe contener al menos un número.");
            }

            if (!Regex.IsMatch(password, "[!@#$%^&*(),.?\":{ }|<>]"))
            {
                return (false, "La contraseña debe contener al menos un carácter especial.");
            }

            return (true, "");
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            var sql = "DELETE FROM Usuarios WHERE IdUsuario = @Id";

            try
            {
                var rowsAffected = await _databaseService.ExecuteAsync(sql, new { Id = id });

                if (rowsAffected == 0)
                {
                    return NotFound(new { Message = "Usuario no encontrado." });
                }

                return Ok(new { Message = "Usuario eliminado exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error al eliminar el usuario.", Details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ModificarUsuario(int id, [FromBody] CrearUsuarioDTO crearUsuarioDto)
        {
            // Validar la contraseña
            var passwordValidationResult = ValidarPassword(crearUsuarioDto.Password);
            if (!passwordValidationResult.IsValid)
            {
                return BadRequest(new { Message = "La contraseña no cumple con los requisitos.", Details = passwordValidationResult.Message });
            }

            Usuario usuario = _mapper.Map<Usuario>(crearUsuarioDto);

            var sql = "UPDATE Usuarios SET Nombre = @Nombre, Usuario = @Usuario, Password = @Password, IdPerfil = @IdPerfil, Estatus = @Estatus WHERE IdUsuario = @Id";

            try
            {
                var rowsAffected = await _databaseService.ExecuteAsync(sql, new { usuario.Nombre, usuario.Usuario, usuario.Password, usuario.IdPerfil, usuario.Estatus, Id = id });

                if (rowsAffected == 0)
                {
                    return NotFound(new { Message = "Usuario no encontrado." });
                }

                return Ok(new { Message = "Usuario modificado exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error al modificar el usuario.", Details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ConsultarUsuario(int id)
        {
            var sql = "SELECT * FROM Usuarios WHERE IdUsuario = @Id";

            try
            {
                var usuario = await _databaseService.QuerySingleAsync<Usuario>(sql, new { Id = id });

                if (usuario == null)
                {
                    return NotFound(new { Message = "Usuario no encontrado." });
                }

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error al consultar el usuario.", Details = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ConsultarUsuarios([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? filtro = null)
        {
            var totalSql = "SELECT COUNT(*) FROM Usuarios WHERE (@Filtro IS NULL OR Nombre LIKE '%' + @Filtro + '%')";
            var dataSql = @"SELECT * FROM Usuarios 
                    WHERE (@Filtro IS NULL OR Nombre LIKE '%' + @Filtro + '%') 
                    ORDER BY IdUsuario OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            try
            {
                // Obtener el total de registros filtrados
                var totalCount = await _databaseService.QuerySingleAsync<int>(totalSql, new { Filtro = filtro });

                var usuarios = await _databaseService.QueryAsync<Usuario>(dataSql, new
                {
                    Filtro = filtro,
                    Offset = (page - 1) * pageSize,
                    PageSize = pageSize
                });

                var response = new
                {
                    totalCount = totalCount,
                    data = usuarios
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error al consultar los usuarios.", Details = ex.Message });
            }
        }

    }

}
