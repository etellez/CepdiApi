using AutoMapper;
using CepdiModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CepdiApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MedicamentosController : ControllerBase
    {
        private readonly IDatabaseService _databaseService;
        private readonly IMapper _mapper;

        public MedicamentosController(IDatabaseService databaseService, IMapper mapper)
        {
            _databaseService = databaseService;
            this._mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CrearMedicamento([FromBody] CrearMedicamentoDTO crearMedicamentoDTO)
        {
            var sql = "INSERT INTO Medicamentos (Nombre, Concentracion, IdFormaFarmaceutica, Precio, Stock, Presentacion, BHabilitado) VALUES (@Nombre, @Concentracion, @IdFormaFarmaceutica, @Precio, @Stock, @Presentacion, @BHabilitado)";

            try
            {
                Medicamento medicamento = _mapper.Map<Medicamento>(crearMedicamentoDTO);
                medicamento.BHabilitado = 1;
                
                var rowsAffected = await _databaseService.ExecuteAsync(sql, medicamento);
                return Ok(new { Message = "Medicamento creado exitosamente.", RowsAffected = rowsAffected });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error al crear el medicamento.", Details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarMedicamento(int id)
        {
            var sql = "DELETE FROM Medicamentos WHERE IdMedicamento = @Id";

            try
            {
                var rowsAffected = await _databaseService.ExecuteAsync(sql, new { Id = id });

                if (rowsAffected == 0)
                {
                    return NotFound(new { Message = "Medicamento no encontrado." });
                }

                return Ok(new { Message = "Medicamento eliminado exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error al eliminar el medicamento.", Details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ModificarMedicamento(int id, [FromBody] Medicamento medicamento)
        {
            var sql = "UPDATE Medicamentos SET Nombre = @Nombre, Concentracion = @Concentracion, IdFormaFarmaceutica = @IdFormaFarmaceutica, Precio = @Precio, Stock = @Stock, Presentacion = @Presentacion, BHabilitado = @BHabilitado WHERE IdMedicamento = @Id";

            try
            {
                var rowsAffected = await _databaseService.ExecuteAsync(sql, new { medicamento.Nombre, medicamento.Concentracion, medicamento.IdFormaFarmaceutica, medicamento.Precio, medicamento.Stock, medicamento.Presentacion, medicamento.BHabilitado, Id = id });

                if (rowsAffected == 0)
                {
                    return NotFound(new { Message = "Medicamento no encontrado." });
                }

                return Ok(new { Message = "Medicamento modificado exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error al modificar el medicamento.", Details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ConsultarMedicamento(int id)
        {
            var sql = @"SELECT m.*, ff.IdFormaFarmaceutica, ff.Nombre AS FormaFarmaceuticaNombre, ff.Habilitado 
                        FROM Medicamentos m 
                        JOIN FormasFarmaceuticas ff ON m.IdFormaFarmaceutica = ff.IdFormaFarmaceutica 
                        WHERE m.IdMedicamento = @Id";

            try
            {
                var medicamento = await _databaseService.QuerySingleAsync<MedicamentoWithFormaFarmaceutica>(sql, new { Id = id });

                if (medicamento == null)
                {
                    return NotFound(new { Message = "Medicamento no encontrado." });
                }

                return Ok(medicamento);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error al consultar el medicamento.", Details = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ConsultarMedicamentos([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? filtro = null)
        {
            var totalSql = @"SELECT COUNT(*) 
                     FROM Medicamentos m 
                     JOIN FormasFarmaceuticas ff ON m.IdFormaFarmaceutica = ff.IdFormaFarmaceutica 
                     WHERE (@Filtro IS NULL OR m.Nombre LIKE '%' + @Filtro + '%')";

            var dataSql = @"SELECT m.*, ff.Nombre AS FormaFarmaceuticaNombre, ff.Habilitado AS FormaFarmaceuticaHabilitado
                    FROM Medicamentos m 
                    JOIN FormasFarmaceuticas ff ON m.IdFormaFarmaceutica = ff.IdFormaFarmaceutica 
                    WHERE (@Filtro IS NULL OR m.Nombre LIKE '%' + @Filtro + '%') 
                    ORDER BY m.IdMedicamento 
                    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            try
            {
                // Obtener el total de registros filtrados
                var totalCount = await _databaseService.QuerySingleAsync<int>(totalSql, new { Filtro = filtro });

                // Obtener los registros para la página actual
                var medicamentos = await _databaseService.QueryAsync<MedicamentoWithFormaFarmaceutica>(dataSql, new
                {
                    Filtro = filtro,
                    Offset = (page - 1) * pageSize,
                    PageSize = pageSize
                });

                // Construir la respuesta con datos y paginación
                var response = new
                {
                    totalCount = totalCount,
                    page = page,
                    pageSize = pageSize,
                    data = medicamentos
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error al consultar los medicamentos.", Details = ex.Message });
            }
        }


        [HttpGet("formas-farmaceuticas")]
        public async Task<IActionResult> ObtenerFormasFarmaceuticas()
        {
            var sql = "SELECT idformafarmaceutica, nombre FROM FormasFarmaceuticas WHERE habilitado = 1";

            try
            {
                var formasFarmaceuticas = await _databaseService.QueryAsync<FormaFarmaceutica>(sql);
                return Ok(formasFarmaceuticas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error al consultar las formas farmacéuticas.", Details = ex.Message });
            }
        }
    }
}
