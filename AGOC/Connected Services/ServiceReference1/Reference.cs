﻿
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServiceReference1
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://ducont.co.in/services/", ConfigurationName="ServiceReference1.SMSServiceSoap")]
    public interface SMSServiceSoap
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://ducont.co.in/services/SMSPush", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<ServiceReference1.SMS> SMSPushAsync(string ApplicationID, string Password, string MobileNumber, string MessageText, bool ConfirmDelivery, int Priority);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://ducont.co.in/services/SMSBulkPush", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<ServiceReference1.SMSBulk> SMSBulkPushAsync(string ApplicationID, string Password, string DistListName, string MessageText, bool ConfirmDelivery, int Priority);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://ducont.co.in/services/GenerateOTP2", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<ServiceReference1.OTP> GenerateOTP2Async(string ApplicationID, string Password, string MobileNumber, string langCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://ducont.co.in/services/GenerateOTP", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<ServiceReference1.OTP> GenerateOTPAsync(string ApplicationID, string Password, string MobileNumber);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://ducont.co.in/services/ValidateOTP", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<ServiceReference1.OTPCheck> ValidateOTPAsync(string ApplicationID, string Password, string OTPValue, string OTPReference);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://ducont.co.in/services/GetMessageStatus", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<ServiceReference1.DeliveryReport[]> GetMessageStatusAsync(string ApplicationID, string Password, string MobileNumber, string DurationOfRecords);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ducont.co.in/services/")]
    public partial class SMS
    {
        
        private string statusField;
        
        private string returnCodeField;
        
        private string errorMessageField;
        
        private string mobileNumberField;
        
        private string applicationIDField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string ReturnCode
        {
            get
            {
                return this.returnCodeField;
            }
            set
            {
                this.returnCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string ErrorMessage
        {
            get
            {
                return this.errorMessageField;
            }
            set
            {
                this.errorMessageField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public string MobileNumber
        {
            get
            {
                return this.mobileNumberField;
            }
            set
            {
                this.mobileNumberField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public string ApplicationID
        {
            get
            {
                return this.applicationIDField;
            }
            set
            {
                this.applicationIDField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ducont.co.in/services/")]
    public partial class DeliveryReport
    {
        
        private string messageTextField;
        
        private string messageIDField;
        
        private string recdDateTimeField;
        
        private string sendDateTimeField;
        
        private string deliveryDateTimeField;
        
        private string sendStatusField;
        
        private string deliveryStatusField;
        
        private string priorityLevelField;
        
        private string mobileNumberField;
        
        private string senderIDField;
        
        private string subscIDField;
        
        private string subscUserIDField;
        
        private string statusField;
        
        private string returnCodeField;
        
        private string errorMessageField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string MessageText
        {
            get
            {
                return this.messageTextField;
            }
            set
            {
                this.messageTextField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string MessageID
        {
            get
            {
                return this.messageIDField;
            }
            set
            {
                this.messageIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string RecdDateTime
        {
            get
            {
                return this.recdDateTimeField;
            }
            set
            {
                this.recdDateTimeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public string SendDateTime
        {
            get
            {
                return this.sendDateTimeField;
            }
            set
            {
                this.sendDateTimeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public string DeliveryDateTime
        {
            get
            {
                return this.deliveryDateTimeField;
            }
            set
            {
                this.deliveryDateTimeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public string SendStatus
        {
            get
            {
                return this.sendStatusField;
            }
            set
            {
                this.sendStatusField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=6)]
        public string DeliveryStatus
        {
            get
            {
                return this.deliveryStatusField;
            }
            set
            {
                this.deliveryStatusField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=7)]
        public string PriorityLevel
        {
            get
            {
                return this.priorityLevelField;
            }
            set
            {
                this.priorityLevelField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=8)]
        public string MobileNumber
        {
            get
            {
                return this.mobileNumberField;
            }
            set
            {
                this.mobileNumberField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=9)]
        public string SenderID
        {
            get
            {
                return this.senderIDField;
            }
            set
            {
                this.senderIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=10)]
        public string SubscID
        {
            get
            {
                return this.subscIDField;
            }
            set
            {
                this.subscIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=11)]
        public string SubscUserID
        {
            get
            {
                return this.subscUserIDField;
            }
            set
            {
                this.subscUserIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=12)]
        public string Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=13)]
        public string ReturnCode
        {
            get
            {
                return this.returnCodeField;
            }
            set
            {
                this.returnCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=14)]
        public string ErrorMessage
        {
            get
            {
                return this.errorMessageField;
            }
            set
            {
                this.errorMessageField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ducont.co.in/services/")]
    public partial class OTPCheck
    {
        
        private string statusField;
        
        private string returnCodeField;
        
        private string errorMessageField;
        
        private string oTPValueField;
        
        private string oTPReferenceField;
        
        private string applicationIDField;
        
        private int failedRequestsField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string ReturnCode
        {
            get
            {
                return this.returnCodeField;
            }
            set
            {
                this.returnCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string ErrorMessage
        {
            get
            {
                return this.errorMessageField;
            }
            set
            {
                this.errorMessageField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public string OTPValue
        {
            get
            {
                return this.oTPValueField;
            }
            set
            {
                this.oTPValueField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public string OTPReference
        {
            get
            {
                return this.oTPReferenceField;
            }
            set
            {
                this.oTPReferenceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public string ApplicationID
        {
            get
            {
                return this.applicationIDField;
            }
            set
            {
                this.applicationIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=6)]
        public int FailedRequests
        {
            get
            {
                return this.failedRequestsField;
            }
            set
            {
                this.failedRequestsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ducont.co.in/services/")]
    public partial class OTP
    {
        
        private string statusField;
        
        private string returnCodeField;
        
        private string errorMessageField;
        
        private string oTPValueField;
        
        private string oTPReferenceField;
        
        private string mobileNumberField;
        
        private string applicationIDField;
        
        private int totalOTPRequestsField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string ReturnCode
        {
            get
            {
                return this.returnCodeField;
            }
            set
            {
                this.returnCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string ErrorMessage
        {
            get
            {
                return this.errorMessageField;
            }
            set
            {
                this.errorMessageField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public string OTPValue
        {
            get
            {
                return this.oTPValueField;
            }
            set
            {
                this.oTPValueField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public string OTPReference
        {
            get
            {
                return this.oTPReferenceField;
            }
            set
            {
                this.oTPReferenceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public string MobileNumber
        {
            get
            {
                return this.mobileNumberField;
            }
            set
            {
                this.mobileNumberField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=6)]
        public string ApplicationID
        {
            get
            {
                return this.applicationIDField;
            }
            set
            {
                this.applicationIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=7)]
        public int TotalOTPRequests
        {
            get
            {
                return this.totalOTPRequestsField;
            }
            set
            {
                this.totalOTPRequestsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ducont.co.in/services/")]
    public partial class SMSBulk
    {
        
        private string statusField;
        
        private string returnCodeField;
        
        private string errorMessageField;
        
        private string mobileNumberCountField;
        
        private string dLNameField;
        
        private string applicationIDField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string ReturnCode
        {
            get
            {
                return this.returnCodeField;
            }
            set
            {
                this.returnCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string ErrorMessage
        {
            get
            {
                return this.errorMessageField;
            }
            set
            {
                this.errorMessageField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public string MobileNumberCount
        {
            get
            {
                return this.mobileNumberCountField;
            }
            set
            {
                this.mobileNumberCountField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public string DLName
        {
            get
            {
                return this.dLNameField;
            }
            set
            {
                this.dLNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public string ApplicationID
        {
            get
            {
                return this.applicationIDField;
            }
            set
            {
                this.applicationIDField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    public interface SMSServiceSoapChannel : ServiceReference1.SMSServiceSoap, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    public partial class SMSServiceSoapClient : System.ServiceModel.ClientBase<ServiceReference1.SMSServiceSoap>, ServiceReference1.SMSServiceSoap
    {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public SMSServiceSoapClient(EndpointConfiguration endpointConfiguration) : 
                base(SMSServiceSoapClient.GetBindingForEndpoint(endpointConfiguration), SMSServiceSoapClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public SMSServiceSoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(SMSServiceSoapClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public SMSServiceSoapClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(SMSServiceSoapClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public SMSServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public System.Threading.Tasks.Task<ServiceReference1.SMS> SMSPushAsync(string ApplicationID, string Password, string MobileNumber, string MessageText, bool ConfirmDelivery, int Priority)
        {
            return base.Channel.SMSPushAsync(ApplicationID, Password, MobileNumber, MessageText, ConfirmDelivery, Priority);
        }
        
        public System.Threading.Tasks.Task<ServiceReference1.SMSBulk> SMSBulkPushAsync(string ApplicationID, string Password, string DistListName, string MessageText, bool ConfirmDelivery, int Priority)
        {
            return base.Channel.SMSBulkPushAsync(ApplicationID, Password, DistListName, MessageText, ConfirmDelivery, Priority);
        }
        
        public System.Threading.Tasks.Task<ServiceReference1.OTP> GenerateOTP2Async(string ApplicationID, string Password, string MobileNumber, string langCode)
        {
            return base.Channel.GenerateOTP2Async(ApplicationID, Password, MobileNumber, langCode);
        }
        
        public System.Threading.Tasks.Task<ServiceReference1.OTP> GenerateOTPAsync(string ApplicationID, string Password, string MobileNumber)
        {
            return base.Channel.GenerateOTPAsync(ApplicationID, Password, MobileNumber);
        }
        
        public System.Threading.Tasks.Task<ServiceReference1.OTPCheck> ValidateOTPAsync(string ApplicationID, string Password, string OTPValue, string OTPReference)
        {
            return base.Channel.ValidateOTPAsync(ApplicationID, Password, OTPValue, OTPReference);
        }
        
        public System.Threading.Tasks.Task<ServiceReference1.DeliveryReport[]> GetMessageStatusAsync(string ApplicationID, string Password, string MobileNumber, string DurationOfRecords)
        {
            return base.Channel.GetMessageStatusAsync(ApplicationID, Password, MobileNumber, DurationOfRecords);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.SMSServiceSoap))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.SMSServiceSoap12))
            {
                System.ServiceModel.Channels.CustomBinding result = new System.ServiceModel.Channels.CustomBinding();
                System.ServiceModel.Channels.TextMessageEncodingBindingElement textBindingElement = new System.ServiceModel.Channels.TextMessageEncodingBindingElement();
                textBindingElement.MessageVersion = System.ServiceModel.Channels.MessageVersion.CreateVersion(System.ServiceModel.EnvelopeVersion.Soap12, System.ServiceModel.Channels.AddressingVersion.None);
                result.Elements.Add(textBindingElement);
                System.ServiceModel.Channels.HttpTransportBindingElement httpBindingElement = new System.ServiceModel.Channels.HttpTransportBindingElement();
                httpBindingElement.AllowCookies = true;
                httpBindingElement.MaxBufferSize = int.MaxValue;
                httpBindingElement.MaxReceivedMessageSize = int.MaxValue;
                result.Elements.Add(httpBindingElement);
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.SMSServiceSoap))
            {
                return new System.ServiceModel.EndpointAddress("http://10.17.1.50/smsservice/smsservice.asmx");
            }
            if ((endpointConfiguration == EndpointConfiguration.SMSServiceSoap12))
            {
                return new System.ServiceModel.EndpointAddress("http://10.17.1.50/smsservice/smsservice.asmx");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        public enum EndpointConfiguration
        {
            
            SMSServiceSoap,
            
            SMSServiceSoap12,
        }
    }
}
