namespace CepdiPortal.Servicio
{
    //public class RespuestaPDF
    //{
    //    public bool Exitoso { get; set; }
    //    public byte[] PDF { get; set; }
    //    public string MensajeError { get; set; }
    //    public int CodigoError { get; set; }
    //}

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://WebService/")]
    public partial class respuestaPDF
    {

        private bool exitosoField;

        private byte[] pDFField;

        private int codigoErrorField;

        private string mensajeErrorField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public bool Exitoso
        {
            get
            {
                return this.exitosoField;
            }
            set
            {
                this.exitosoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "base64Binary", Order = 1)]
        public byte[] PDF
        {
            get
            {
                return this.pDFField;
            }
            set
            {
                this.pDFField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
        public int CodigoError
        {
            get
            {
                return this.codigoErrorField;
            }
            set
            {
                this.codigoErrorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 3)]
        public string MensajeError
        {
            get
            {
                return this.mensajeErrorField;
            }
            set
            {
                this.mensajeErrorField = value;
            }
        }
    }
}
