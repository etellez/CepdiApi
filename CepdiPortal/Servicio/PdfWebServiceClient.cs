using System.ServiceModel;
using System.ServiceModel.Description;

namespace CepdiPortal.Servicio
{
    public class PdfWebServiceClient
    {
        private readonly string _endpointUrl = "https://timbrador.cepdi.mx:8443/WSDemo/WS";
        private readonly string _usuario = "demo1@mail.com";
        private readonly string _password = "Demo123#";

        public respuestaPDF ConsumirObtenerPDF(string uuid)
        {
            var binding = new BasicHttpBinding
            {
                Security = new BasicHttpSecurity
                {
                    Mode = BasicHttpSecurityMode.Transport
                },
                MaxReceivedMessageSize = 10485760, // Permite hasta 10 MB
                ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas
                {
                    MaxDepth = 32,
                    MaxStringContentLength = 10485760,
                    MaxArrayLength = 10485760,
                    MaxBytesPerRead = 4096,
                    MaxNameTableCharCount = 16384
                }
            };


            var endpoint = new EndpointAddress("https://timbrador.cepdi.mx:8443/WSDemo/WS");
            //var endpoint = new EndpointAddress("https://timbrador.cepdi.mx:8443/WSProduccion/WS?WSDL");

            var factory = new ChannelFactory<IWSPDFClient>(binding, endpoint);

            factory.Endpoint.EndpointBehaviors.Add(new LoggingBehavior());

            var client = factory.CreateChannel();

            var request = new ObtenerPDFRequest
            {
                Usuario = _usuario,
                Password = _password,
                uuid = uuid
            };

            try
            {
                var response = client.ObtenerPDF(request);

                if (response == null)
                {
                    throw new Exception("El servicio devolvió una respuesta nula.");
                }

                //if (response.ObtenerPDFResult == null)
                //{
                //    throw new Exception("El servicio devolvió un ObtenerPDFResult nulo.");
                //}

                //return response.ObtenerPDFResult;
                return response.@return;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al consumir el Web Service: {ex.Message}");
            }
            finally
            {
                var communicationObject = (ICommunicationObject)client;
                if (communicationObject.State == CommunicationState.Opened)
                {
                    communicationObject.Close();
                }
            }
        }
    }


}
