using CepdiPortal.Servicio;
using System.ServiceModel;

namespace CepdiPortal.Models
{
    [ServiceContract]
    public interface IWebServiceClient
    {
        [OperationContract]
        Task<ObtenerPDFResponse> ObtenerPDFAsync(ObtenerPDFRequest request);
    }
}
