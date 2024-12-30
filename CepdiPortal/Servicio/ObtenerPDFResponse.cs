using System.ServiceModel;

namespace CepdiPortal.Servicio
{
    //[MessageContract(IsWrapped = true, WrapperName = "ObtenerPDFResponse", WrapperNamespace = "http://WebService/")]
    //public class ObtenerPDFResponse
    //{
    //    [MessageBodyMember(Namespace = "http://WebService/", Order = 0)]
    //    public RespuestaPDF ObtenerPDFResult { get; set; }
    //}

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "ObtenerPDFResponse", WrapperNamespace = "http://WebService/", IsWrapped = true)]
    public partial class ObtenerPDFResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://WebService/", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public respuestaPDF @return;

        public ObtenerPDFResponse()
        {
        }

        public ObtenerPDFResponse(respuestaPDF @return)
        {
            this.@return = @return;
        }
    }
}
