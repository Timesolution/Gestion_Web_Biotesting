﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Gestion_Web.SmsMasivoService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://servicio.smsmasivos.com.ar/ws/", ConfigurationName="SmsMasivoService.SMSMasivosAPISoap")]
    public interface SMSMasivosAPISoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://servicio.smsmasivos.com.ar/ws/EnviarSMS", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string EnviarSMS(string usuario, string clave, long numero, string texto, bool test);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://servicio.smsmasivos.com.ar/ws/EnviarSMS", ReplyAction="*")]
        System.Threading.Tasks.Task<string> EnviarSMSAsync(string usuario, string clave, long numero, string texto, bool test);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://servicio.smsmasivos.com.ar/ws/RecibirSMS", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        Gestion_Web.SmsMasivoService.clsRespuesta[] RecibirSMS(string usuario, string clave, string origen, bool solonoleidos, bool marcarcomoleidos);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://servicio.smsmasivos.com.ar/ws/RecibirSMS", ReplyAction="*")]
        System.Threading.Tasks.Task<Gestion_Web.SmsMasivoService.clsRespuesta[]> RecibirSMSAsync(string usuario, string clave, string origen, bool solonoleidos, bool marcarcomoleidos);
    }
    
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://servicio.smsmasivos.com.ar/ws/")]
    public partial class clsRespuesta : object, System.ComponentModel.INotifyPropertyChanged {
        
        private long numeroField;
        
        private string textoField;
        
        private System.DateTime fechaField;
        
        private long idsmsField;
        
        private string idinternoField;
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public long numero {
            get {
                return this.numeroField;
            }
            set {
                this.numeroField = value;
                this.RaisePropertyChanged("numero");
            }
        }
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string texto {
            get {
                return this.textoField;
            }
            set {
                this.textoField = value;
                this.RaisePropertyChanged("texto");
            }
        }
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public System.DateTime fecha {
            get {
                return this.fechaField;
            }
            set {
                this.fechaField = value;
                this.RaisePropertyChanged("fecha");
            }
        }
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public long idsms {
            get {
                return this.idsmsField;
            }
            set {
                this.idsmsField = value;
                this.RaisePropertyChanged("idsms");
            }
        }
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public string idinterno {
            get {
                return this.idinternoField;
            }
            set {
                this.idinternoField = value;
                this.RaisePropertyChanged("idinterno");
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface SMSMasivosAPISoapChannel : Gestion_Web.SmsMasivoService.SMSMasivosAPISoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SMSMasivosAPISoapClient : System.ServiceModel.ClientBase<Gestion_Web.SmsMasivoService.SMSMasivosAPISoap>, Gestion_Web.SmsMasivoService.SMSMasivosAPISoap {
        
        public SMSMasivosAPISoapClient() {
        }
        
        public SMSMasivosAPISoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SMSMasivosAPISoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SMSMasivosAPISoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SMSMasivosAPISoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string EnviarSMS(string usuario, string clave, long numero, string texto, bool test) {
            return base.Channel.EnviarSMS(usuario, clave, numero, texto, test);
        }
        
        public System.Threading.Tasks.Task<string> EnviarSMSAsync(string usuario, string clave, long numero, string texto, bool test) {
            return base.Channel.EnviarSMSAsync(usuario, clave, numero, texto, test);
        }
        
        public Gestion_Web.SmsMasivoService.clsRespuesta[] RecibirSMS(string usuario, string clave, string origen, bool solonoleidos, bool marcarcomoleidos) {
            return base.Channel.RecibirSMS(usuario, clave, origen, solonoleidos, marcarcomoleidos);
        }
        
        public System.Threading.Tasks.Task<Gestion_Web.SmsMasivoService.clsRespuesta[]> RecibirSMSAsync(string usuario, string clave, string origen, bool solonoleidos, bool marcarcomoleidos) {
            return base.Channel.RecibirSMSAsync(usuario, clave, origen, solonoleidos, marcarcomoleidos);
        }
    }
}
