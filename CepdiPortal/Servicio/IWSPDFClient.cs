using System.ServiceModel;

namespace CepdiPortal.Servicio
{
    //[ServiceContract(Namespace = "http://WebService/")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://WebService/", ConfigurationName = "WS")]
    public interface IWSPDFClient
    {
        //[OperationContract(Action = "http://WebService/WS/ObtenerPDFRequest", ReplyAction = "http://WebService/WS/ObtenerPDFResponse")]
        //ObtenerPDFResponse ObtenerPDF(ObtenerPDFRequest request);

        // CODEGEN: Parameter 'return' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action = "http://WebService/WS/ObtenerPDFRequest", ReplyAction = "http://WebService/WS/ObtenerPDFResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "return")]
        ObtenerPDFResponse ObtenerPDF(ObtenerPDFRequest request);
    }
}
