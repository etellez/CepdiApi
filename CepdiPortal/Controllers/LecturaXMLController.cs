using CepdiPortal.Models;
using CepdiPortal.Servicio;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.Xml;

namespace CepdiPortal.Controllers
{
    public class LecturaXMLController : Controller
    {
        private readonly PdfWebServiceClient _pdfService;

        public LecturaXMLController()
        {
            _pdfService = new PdfWebServiceClient();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProcesarXML(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No se cargó un archivo válido.");
            }

            try
            {
                // Leer el contenido del archivo XML
                using var stream = file.OpenReadStream();
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(stream);

                // Extraer el UUID del nodo TimbreFiscalDigital
                var namespaceManager = new XmlNamespaceManager(xmlDoc.NameTable);
                namespaceManager.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                var uuidNode = xmlDoc.SelectSingleNode("//tfd:TimbreFiscalDigital/@UUID", namespaceManager);

                if (uuidNode == null)
                {
                    return BadRequest("No se encontró el UUID en el archivo XML.");
                }

                var uuid = uuidNode.Value;

                // Consumir el servicio
                var pdfResponse = _pdfService.ConsumirObtenerPDF(uuid);

                if (pdfResponse == null || !pdfResponse.Exitoso)
                {
                    return StatusCode(500, $"Error al obtener el PDF: {pdfResponse?.MensajeError ?? "Error desconocido"}");
                }

                var pdfBase64 = Convert.ToBase64String(pdfResponse.PDF);
                return Ok(new { pdfBase64 });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al procesar el archivo: {ex.Message}");
            }
        }

    }
}
