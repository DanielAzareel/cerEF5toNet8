﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServiciosWeb.ServicioIEMS {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://ws.web.mec.sep.mx/schemas", ConfigurationName="ServicioIEMS.CertificadosIEMSPortType")]
    public interface CertificadosIEMSPortType {
        
        // CODEGEN: Se está generando un contrato de mensaje, ya que la operación cargaCertificadosIEMS no es RPC ni está encapsulada en un documento.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        ServiciosWeb.ServicioIEMS.cargaCertificadosIEMSResponse1 cargaCertificadosIEMS(ServiciosWeb.ServicioIEMS.cargaCertificadosIEMSRequest1 request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiciosWeb.ServicioIEMS.cargaCertificadosIEMSResponse1> cargaCertificadosIEMSAsync(ServiciosWeb.ServicioIEMS.cargaCertificadosIEMSRequest1 request);
        
        // CODEGEN: Se está generando un contrato de mensaje, ya que la operación consultaCertificadosIEMS no es RPC ni está encapsulada en un documento.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        ServiciosWeb.ServicioIEMS.consultaCertificadosIEMSResponse1 consultaCertificadosIEMS(ServiciosWeb.ServicioIEMS.consultaCertificadosIEMSRequest1 request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiciosWeb.ServicioIEMS.consultaCertificadosIEMSResponse1> consultaCertificadosIEMSAsync(ServiciosWeb.ServicioIEMS.consultaCertificadosIEMSRequest1 request);
        
        // CODEGEN: Se está generando un contrato de mensaje, ya que la operación descargaCertificadosIEMS no es RPC ni está encapsulada en un documento.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        ServiciosWeb.ServicioIEMS.descargaCertificadosIEMSResponse1 descargaCertificadosIEMS(ServiciosWeb.ServicioIEMS.descargaCertificadosIEMSRequest1 request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiciosWeb.ServicioIEMS.descargaCertificadosIEMSResponse1> descargaCertificadosIEMSAsync(ServiciosWeb.ServicioIEMS.descargaCertificadosIEMSRequest1 request);
        
        // CODEGEN: Se está generando un contrato de mensaje, ya que la operación cancelarCertificadosIEMS no es RPC ni está encapsulada en un documento.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        ServiciosWeb.ServicioIEMS.cancelarCertificadosIEMSResponse1 cancelarCertificadosIEMS(ServiciosWeb.ServicioIEMS.cancelarCertificadosIEMSRequest1 request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiciosWeb.ServicioIEMS.cancelarCertificadosIEMSResponse1> cancelarCertificadosIEMSAsync(ServiciosWeb.ServicioIEMS.cancelarCertificadosIEMSRequest1 request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.9037.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://ws.web.mec.sep.mx/schemas")]
    public partial class cargaCertificadosIEMSRequest : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string nombreArchivoField;
        
        private byte[] archivoBase64Field;
        
        private autenticacionType autenticacionField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string nombreArchivo {
            get {
                return this.nombreArchivoField;
            }
            set {
                this.nombreArchivoField = value;
                this.RaisePropertyChanged("nombreArchivo");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary", Order=1)]
        public byte[] archivoBase64 {
            get {
                return this.archivoBase64Field;
            }
            set {
                this.archivoBase64Field = value;
                this.RaisePropertyChanged("archivoBase64");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public autenticacionType autenticacion {
            get {
                return this.autenticacionField;
            }
            set {
                this.autenticacionField = value;
                this.RaisePropertyChanged("autenticacion");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.9037.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ws.web.mec.sep.mx/schemas")]
    public partial class autenticacionType : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string usuarioField;
        
        private string passwordField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string usuario {
            get {
                return this.usuarioField;
            }
            set {
                this.usuarioField = value;
                this.RaisePropertyChanged("usuario");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string password {
            get {
                return this.passwordField;
            }
            set {
                this.passwordField = value;
                this.RaisePropertyChanged("password");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.9037.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://ws.web.mec.sep.mx/schemas")]
    public partial class cargaCertificadosIEMSResponse : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string numeroLoteField;
        
        private string mensajeField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer", Order=0)]
        public string numeroLote {
            get {
                return this.numeroLoteField;
            }
            set {
                this.numeroLoteField = value;
                this.RaisePropertyChanged("numeroLote");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string mensaje {
            get {
                return this.mensajeField;
            }
            set {
                this.mensajeField = value;
                this.RaisePropertyChanged("mensaje");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class cargaCertificadosIEMSRequest1 {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.web.mec.sep.mx/schemas", Order=0)]
        public ServiciosWeb.ServicioIEMS.cargaCertificadosIEMSRequest cargaCertificadosIEMSRequest;
        
        public cargaCertificadosIEMSRequest1() {
        }
        
        public cargaCertificadosIEMSRequest1(ServiciosWeb.ServicioIEMS.cargaCertificadosIEMSRequest cargaCertificadosIEMSRequest) {
            this.cargaCertificadosIEMSRequest = cargaCertificadosIEMSRequest;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class cargaCertificadosIEMSResponse1 {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.web.mec.sep.mx/schemas", Order=0)]
        public ServiciosWeb.ServicioIEMS.cargaCertificadosIEMSResponse cargaCertificadosIEMSResponse;
        
        public cargaCertificadosIEMSResponse1() {
        }
        
        public cargaCertificadosIEMSResponse1(ServiciosWeb.ServicioIEMS.cargaCertificadosIEMSResponse cargaCertificadosIEMSResponse) {
            this.cargaCertificadosIEMSResponse = cargaCertificadosIEMSResponse;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.9037.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://ws.web.mec.sep.mx/schemas")]
    public partial class consultaCertificadosIEMSRequest : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string numeroLoteField;
        
        private autenticacionType autenticacionField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer", Order=0)]
        public string numeroLote {
            get {
                return this.numeroLoteField;
            }
            set {
                this.numeroLoteField = value;
                this.RaisePropertyChanged("numeroLote");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public autenticacionType autenticacion {
            get {
                return this.autenticacionField;
            }
            set {
                this.autenticacionField = value;
                this.RaisePropertyChanged("autenticacion");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.9037.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://ws.web.mec.sep.mx/schemas")]
    public partial class consultaCertificadosIEMSResponse : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string numeroLoteField;
        
        private short estatusLoteField;
        
        private string mensajeField;
        
        private byte[] excelBase64Field;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer", Order=0)]
        public string numeroLote {
            get {
                return this.numeroLoteField;
            }
            set {
                this.numeroLoteField = value;
                this.RaisePropertyChanged("numeroLote");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public short estatusLote {
            get {
                return this.estatusLoteField;
            }
            set {
                this.estatusLoteField = value;
                this.RaisePropertyChanged("estatusLote");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string mensaje {
            get {
                return this.mensajeField;
            }
            set {
                this.mensajeField = value;
                this.RaisePropertyChanged("mensaje");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary", Order=3)]
        public byte[] excelBase64 {
            get {
                return this.excelBase64Field;
            }
            set {
                this.excelBase64Field = value;
                this.RaisePropertyChanged("excelBase64");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class consultaCertificadosIEMSRequest1 {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.web.mec.sep.mx/schemas", Order=0)]
        public ServiciosWeb.ServicioIEMS.consultaCertificadosIEMSRequest consultaCertificadosIEMSRequest;
        
        public consultaCertificadosIEMSRequest1() {
        }
        
        public consultaCertificadosIEMSRequest1(ServiciosWeb.ServicioIEMS.consultaCertificadosIEMSRequest consultaCertificadosIEMSRequest) {
            this.consultaCertificadosIEMSRequest = consultaCertificadosIEMSRequest;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class consultaCertificadosIEMSResponse1 {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.web.mec.sep.mx/schemas", Order=0)]
        public ServiciosWeb.ServicioIEMS.consultaCertificadosIEMSResponse consultaCertificadosIEMSResponse;
        
        public consultaCertificadosIEMSResponse1() {
        }
        
        public consultaCertificadosIEMSResponse1(ServiciosWeb.ServicioIEMS.consultaCertificadosIEMSResponse consultaCertificadosIEMSResponse) {
            this.consultaCertificadosIEMSResponse = consultaCertificadosIEMSResponse;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.9037.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://ws.web.mec.sep.mx/schemas")]
    public partial class descargaCertificadosIEMSRequest : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string numeroLoteField;
        
        private autenticacionType autenticacionField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer", Order=0)]
        public string numeroLote {
            get {
                return this.numeroLoteField;
            }
            set {
                this.numeroLoteField = value;
                this.RaisePropertyChanged("numeroLote");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public autenticacionType autenticacion {
            get {
                return this.autenticacionField;
            }
            set {
                this.autenticacionField = value;
                this.RaisePropertyChanged("autenticacion");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.9037.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://ws.web.mec.sep.mx/schemas")]
    public partial class descargaCertificadosIEMSResponse : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string numeroLoteField;
        
        private string mensajeField;
        
        private byte[] certificadosBase64Field;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer", Order=0)]
        public string numeroLote {
            get {
                return this.numeroLoteField;
            }
            set {
                this.numeroLoteField = value;
                this.RaisePropertyChanged("numeroLote");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string mensaje {
            get {
                return this.mensajeField;
            }
            set {
                this.mensajeField = value;
                this.RaisePropertyChanged("mensaje");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary", Order=2)]
        public byte[] certificadosBase64 {
            get {
                return this.certificadosBase64Field;
            }
            set {
                this.certificadosBase64Field = value;
                this.RaisePropertyChanged("certificadosBase64");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class descargaCertificadosIEMSRequest1 {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.web.mec.sep.mx/schemas", Order=0)]
        public ServiciosWeb.ServicioIEMS.descargaCertificadosIEMSRequest descargaCertificadosIEMSRequest;
        
        public descargaCertificadosIEMSRequest1() {
        }
        
        public descargaCertificadosIEMSRequest1(ServiciosWeb.ServicioIEMS.descargaCertificadosIEMSRequest descargaCertificadosIEMSRequest) {
            this.descargaCertificadosIEMSRequest = descargaCertificadosIEMSRequest;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class descargaCertificadosIEMSResponse1 {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.web.mec.sep.mx/schemas", Order=0)]
        public ServiciosWeb.ServicioIEMS.descargaCertificadosIEMSResponse descargaCertificadosIEMSResponse;
        
        public descargaCertificadosIEMSResponse1() {
        }
        
        public descargaCertificadosIEMSResponse1(ServiciosWeb.ServicioIEMS.descargaCertificadosIEMSResponse descargaCertificadosIEMSResponse) {
            this.descargaCertificadosIEMSResponse = descargaCertificadosIEMSResponse;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.9037.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://ws.web.mec.sep.mx/schemas")]
    public partial class cancelarCertificadosIEMSRequest : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string folioCertificadoField;
        
        private autenticacionType autenticacionField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string folioCertificado {
            get {
                return this.folioCertificadoField;
            }
            set {
                this.folioCertificadoField = value;
                this.RaisePropertyChanged("folioCertificado");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public autenticacionType autenticacion {
            get {
                return this.autenticacionField;
            }
            set {
                this.autenticacionField = value;
                this.RaisePropertyChanged("autenticacion");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.9037.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://ws.web.mec.sep.mx/schemas")]
    public partial class cancelarCertificadosIEMSResponse : object, System.ComponentModel.INotifyPropertyChanged {
        
        private int codigoField;
        
        private string mensajeField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public int codigo {
            get {
                return this.codigoField;
            }
            set {
                this.codigoField = value;
                this.RaisePropertyChanged("codigo");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string mensaje {
            get {
                return this.mensajeField;
            }
            set {
                this.mensajeField = value;
                this.RaisePropertyChanged("mensaje");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class cancelarCertificadosIEMSRequest1 {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.web.mec.sep.mx/schemas", Order=0)]
        public ServiciosWeb.ServicioIEMS.cancelarCertificadosIEMSRequest cancelarCertificadosIEMSRequest;
        
        public cancelarCertificadosIEMSRequest1() {
        }
        
        public cancelarCertificadosIEMSRequest1(ServiciosWeb.ServicioIEMS.cancelarCertificadosIEMSRequest cancelarCertificadosIEMSRequest) {
            this.cancelarCertificadosIEMSRequest = cancelarCertificadosIEMSRequest;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class cancelarCertificadosIEMSResponse1 {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.web.mec.sep.mx/schemas", Order=0)]
        public ServiciosWeb.ServicioIEMS.cancelarCertificadosIEMSResponse cancelarCertificadosIEMSResponse;
        
        public cancelarCertificadosIEMSResponse1() {
        }
        
        public cancelarCertificadosIEMSResponse1(ServiciosWeb.ServicioIEMS.cancelarCertificadosIEMSResponse cancelarCertificadosIEMSResponse) {
            this.cancelarCertificadosIEMSResponse = cancelarCertificadosIEMSResponse;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface CertificadosIEMSPortTypeChannel : ServiciosWeb.ServicioIEMS.CertificadosIEMSPortType, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CertificadosIEMSPortTypeClient : System.ServiceModel.ClientBase<ServiciosWeb.ServicioIEMS.CertificadosIEMSPortType>, ServiciosWeb.ServicioIEMS.CertificadosIEMSPortType {
        
        public CertificadosIEMSPortTypeClient() {
        }
        
        public CertificadosIEMSPortTypeClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public CertificadosIEMSPortTypeClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CertificadosIEMSPortTypeClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CertificadosIEMSPortTypeClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        ServiciosWeb.ServicioIEMS.cargaCertificadosIEMSResponse1 ServiciosWeb.ServicioIEMS.CertificadosIEMSPortType.cargaCertificadosIEMS(ServiciosWeb.ServicioIEMS.cargaCertificadosIEMSRequest1 request) {
            return base.Channel.cargaCertificadosIEMS(request);
        }
        
        public ServiciosWeb.ServicioIEMS.cargaCertificadosIEMSResponse cargaCertificadosIEMS(ServiciosWeb.ServicioIEMS.cargaCertificadosIEMSRequest cargaCertificadosIEMSRequest) {
            ServiciosWeb.ServicioIEMS.cargaCertificadosIEMSRequest1 inValue = new ServiciosWeb.ServicioIEMS.cargaCertificadosIEMSRequest1();
            inValue.cargaCertificadosIEMSRequest = cargaCertificadosIEMSRequest;
            ServiciosWeb.ServicioIEMS.cargaCertificadosIEMSResponse1 retVal = ((ServiciosWeb.ServicioIEMS.CertificadosIEMSPortType)(this)).cargaCertificadosIEMS(inValue);
            return retVal.cargaCertificadosIEMSResponse;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiciosWeb.ServicioIEMS.cargaCertificadosIEMSResponse1> ServiciosWeb.ServicioIEMS.CertificadosIEMSPortType.cargaCertificadosIEMSAsync(ServiciosWeb.ServicioIEMS.cargaCertificadosIEMSRequest1 request) {
            return base.Channel.cargaCertificadosIEMSAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiciosWeb.ServicioIEMS.cargaCertificadosIEMSResponse1> cargaCertificadosIEMSAsync(ServiciosWeb.ServicioIEMS.cargaCertificadosIEMSRequest cargaCertificadosIEMSRequest) {
            ServiciosWeb.ServicioIEMS.cargaCertificadosIEMSRequest1 inValue = new ServiciosWeb.ServicioIEMS.cargaCertificadosIEMSRequest1();
            inValue.cargaCertificadosIEMSRequest = cargaCertificadosIEMSRequest;
            return ((ServiciosWeb.ServicioIEMS.CertificadosIEMSPortType)(this)).cargaCertificadosIEMSAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        ServiciosWeb.ServicioIEMS.consultaCertificadosIEMSResponse1 ServiciosWeb.ServicioIEMS.CertificadosIEMSPortType.consultaCertificadosIEMS(ServiciosWeb.ServicioIEMS.consultaCertificadosIEMSRequest1 request) {
            return base.Channel.consultaCertificadosIEMS(request);
        }
        
        public ServiciosWeb.ServicioIEMS.consultaCertificadosIEMSResponse consultaCertificadosIEMS(ServiciosWeb.ServicioIEMS.consultaCertificadosIEMSRequest consultaCertificadosIEMSRequest) {
            ServiciosWeb.ServicioIEMS.consultaCertificadosIEMSRequest1 inValue = new ServiciosWeb.ServicioIEMS.consultaCertificadosIEMSRequest1();
            inValue.consultaCertificadosIEMSRequest = consultaCertificadosIEMSRequest;
            ServiciosWeb.ServicioIEMS.consultaCertificadosIEMSResponse1 retVal = ((ServiciosWeb.ServicioIEMS.CertificadosIEMSPortType)(this)).consultaCertificadosIEMS(inValue);
            return retVal.consultaCertificadosIEMSResponse;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiciosWeb.ServicioIEMS.consultaCertificadosIEMSResponse1> ServiciosWeb.ServicioIEMS.CertificadosIEMSPortType.consultaCertificadosIEMSAsync(ServiciosWeb.ServicioIEMS.consultaCertificadosIEMSRequest1 request) {
            return base.Channel.consultaCertificadosIEMSAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiciosWeb.ServicioIEMS.consultaCertificadosIEMSResponse1> consultaCertificadosIEMSAsync(ServiciosWeb.ServicioIEMS.consultaCertificadosIEMSRequest consultaCertificadosIEMSRequest) {
            ServiciosWeb.ServicioIEMS.consultaCertificadosIEMSRequest1 inValue = new ServiciosWeb.ServicioIEMS.consultaCertificadosIEMSRequest1();
            inValue.consultaCertificadosIEMSRequest = consultaCertificadosIEMSRequest;
            return ((ServiciosWeb.ServicioIEMS.CertificadosIEMSPortType)(this)).consultaCertificadosIEMSAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        ServiciosWeb.ServicioIEMS.descargaCertificadosIEMSResponse1 ServiciosWeb.ServicioIEMS.CertificadosIEMSPortType.descargaCertificadosIEMS(ServiciosWeb.ServicioIEMS.descargaCertificadosIEMSRequest1 request) {
            return base.Channel.descargaCertificadosIEMS(request);
        }
        
        public ServiciosWeb.ServicioIEMS.descargaCertificadosIEMSResponse descargaCertificadosIEMS(ServiciosWeb.ServicioIEMS.descargaCertificadosIEMSRequest descargaCertificadosIEMSRequest) {
            ServiciosWeb.ServicioIEMS.descargaCertificadosIEMSRequest1 inValue = new ServiciosWeb.ServicioIEMS.descargaCertificadosIEMSRequest1();
            inValue.descargaCertificadosIEMSRequest = descargaCertificadosIEMSRequest;
            ServiciosWeb.ServicioIEMS.descargaCertificadosIEMSResponse1 retVal = ((ServiciosWeb.ServicioIEMS.CertificadosIEMSPortType)(this)).descargaCertificadosIEMS(inValue);
            return retVal.descargaCertificadosIEMSResponse;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiciosWeb.ServicioIEMS.descargaCertificadosIEMSResponse1> ServiciosWeb.ServicioIEMS.CertificadosIEMSPortType.descargaCertificadosIEMSAsync(ServiciosWeb.ServicioIEMS.descargaCertificadosIEMSRequest1 request) {
            return base.Channel.descargaCertificadosIEMSAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiciosWeb.ServicioIEMS.descargaCertificadosIEMSResponse1> descargaCertificadosIEMSAsync(ServiciosWeb.ServicioIEMS.descargaCertificadosIEMSRequest descargaCertificadosIEMSRequest) {
            ServiciosWeb.ServicioIEMS.descargaCertificadosIEMSRequest1 inValue = new ServiciosWeb.ServicioIEMS.descargaCertificadosIEMSRequest1();
            inValue.descargaCertificadosIEMSRequest = descargaCertificadosIEMSRequest;
            return ((ServiciosWeb.ServicioIEMS.CertificadosIEMSPortType)(this)).descargaCertificadosIEMSAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        ServiciosWeb.ServicioIEMS.cancelarCertificadosIEMSResponse1 ServiciosWeb.ServicioIEMS.CertificadosIEMSPortType.cancelarCertificadosIEMS(ServiciosWeb.ServicioIEMS.cancelarCertificadosIEMSRequest1 request) {
            return base.Channel.cancelarCertificadosIEMS(request);
        }
        
        public ServiciosWeb.ServicioIEMS.cancelarCertificadosIEMSResponse cancelarCertificadosIEMS(ServiciosWeb.ServicioIEMS.cancelarCertificadosIEMSRequest cancelarCertificadosIEMSRequest) {
            ServiciosWeb.ServicioIEMS.cancelarCertificadosIEMSRequest1 inValue = new ServiciosWeb.ServicioIEMS.cancelarCertificadosIEMSRequest1();
            inValue.cancelarCertificadosIEMSRequest = cancelarCertificadosIEMSRequest;
            ServiciosWeb.ServicioIEMS.cancelarCertificadosIEMSResponse1 retVal = ((ServiciosWeb.ServicioIEMS.CertificadosIEMSPortType)(this)).cancelarCertificadosIEMS(inValue);
            return retVal.cancelarCertificadosIEMSResponse;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiciosWeb.ServicioIEMS.cancelarCertificadosIEMSResponse1> ServiciosWeb.ServicioIEMS.CertificadosIEMSPortType.cancelarCertificadosIEMSAsync(ServiciosWeb.ServicioIEMS.cancelarCertificadosIEMSRequest1 request) {
            return base.Channel.cancelarCertificadosIEMSAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiciosWeb.ServicioIEMS.cancelarCertificadosIEMSResponse1> cancelarCertificadosIEMSAsync(ServiciosWeb.ServicioIEMS.cancelarCertificadosIEMSRequest cancelarCertificadosIEMSRequest) {
            ServiciosWeb.ServicioIEMS.cancelarCertificadosIEMSRequest1 inValue = new ServiciosWeb.ServicioIEMS.cancelarCertificadosIEMSRequest1();
            inValue.cancelarCertificadosIEMSRequest = cancelarCertificadosIEMSRequest;
            return ((ServiciosWeb.ServicioIEMS.CertificadosIEMSPortType)(this)).cancelarCertificadosIEMSAsync(inValue);
        }
    }
}
