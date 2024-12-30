using System.ServiceModel;

namespace CepdiPortal.Servicio
{
    //[MessageContract(IsWrapped = true, WrapperName = "ObtenerPDFValidacionParam", WrapperNamespace = "http://WebService/")]
    //public class ObtenerPDFRequest
    //{
    //    [MessageBodyMember(Namespace = "http://WebService/", Order = 0)]
    //    public string Usuario { get; set; }

    //    [MessageBodyMember(Namespace = "http://WebService/", Order = 1)]
    //    public string Password { get; set; }

    //    [MessageBodyMember(Namespace = "http://WebService/", Order = 2)]
    //    public string UUID { get; set; }

    //    [MessageBodyMember(Namespace = "http://WebService/", Order = 3)]
    //    public string Parametro { get; set; }
    //}

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "ObtenerPDF", WrapperNamespace = "http://WebService/", IsWrapped = true)]
    public partial class ObtenerPDFRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://WebService/", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Usuario;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://WebService/", Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Password;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://WebService/", Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string uuid;

        public ObtenerPDFRequest()
        {
        }

        public ObtenerPDFRequest(string Usuario, string Password, string uuid)
        {
            this.Usuario = Usuario;
            this.Password = Password;
            this.uuid = uuid;
        }
    }

}
