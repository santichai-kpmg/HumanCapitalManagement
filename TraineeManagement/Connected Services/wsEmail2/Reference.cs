﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TraineeManagement.wsEmail2 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://th.kpmg.intranet/", ConfigurationName="wsEmail2.EMail_NewSoap")]
    public interface EMail_NewSoap {
        
        // CODEGEN: Generating message contract since the wrapper name (ws_MailServer) of message ws_MailServer does not match the default value (MailServer)
        [System.ServiceModel.OperationContractAttribute(Action="http://th.kpmg.intranet/ws_MailServer", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        TraineeManagement.wsEmail2.ws_MailServer1 MailServer(TraineeManagement.wsEmail2.ws_MailServer request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://th.kpmg.intranet/ws_MailServer", ReplyAction="*")]
        System.Threading.Tasks.Task<TraineeManagement.wsEmail2.ws_MailServer1> MailServerAsync(TraineeManagement.wsEmail2.ws_MailServer request);
        
        // CODEGEN: Generating message contract since the wrapper name (ws_SendMail) of message ws_SendMail does not match the default value (SendMail)
        [System.ServiceModel.OperationContractAttribute(Action="http://th.kpmg.intranet/ws_SendMail", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        TraineeManagement.wsEmail2.ws_SendMail1 SendMail(TraineeManagement.wsEmail2.ws_SendMail request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://th.kpmg.intranet/ws_SendMail", ReplyAction="*")]
        System.Threading.Tasks.Task<TraineeManagement.wsEmail2.ws_SendMail1> SendMailAsync(TraineeManagement.wsEmail2.ws_SendMail request);
        
        // CODEGEN: Generating message contract since the wrapper name (ws_SendMail_Advance) of message ws_SendMail_Advance does not match the default value (SendMail_Advance)
        [System.ServiceModel.OperationContractAttribute(Action="http://th.kpmg.intranet/ws_SendMail_Advance", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        TraineeManagement.wsEmail2.ws_SendMail_Advance1 SendMail_Advance(TraineeManagement.wsEmail2.ws_SendMail_Advance request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://th.kpmg.intranet/ws_SendMail_Advance", ReplyAction="*")]
        System.Threading.Tasks.Task<TraineeManagement.wsEmail2.ws_SendMail_Advance1> SendMail_AdvanceAsync(TraineeManagement.wsEmail2.ws_SendMail_Advance request);
        
        // CODEGEN: Generating message contract since the wrapper name (ws_GetScriptAdminMail4AutoScript) of message ws_GetScriptAdminMail4AutoScript does not match the default value (GetScriptAdminMail4AutoScript)
        [System.ServiceModel.OperationContractAttribute(Action="http://th.kpmg.intranet/ws_GetScriptAdminMail4AutoScript", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        TraineeManagement.wsEmail2.ws_GetScriptAdminMail4AutoScript1 GetScriptAdminMail4AutoScript(TraineeManagement.wsEmail2.ws_GetScriptAdminMail4AutoScript request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://th.kpmg.intranet/ws_GetScriptAdminMail4AutoScript", ReplyAction="*")]
        System.Threading.Tasks.Task<TraineeManagement.wsEmail2.ws_GetScriptAdminMail4AutoScript1> GetScriptAdminMail4AutoScriptAsync(TraineeManagement.wsEmail2.ws_GetScriptAdminMail4AutoScript request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ws_MailServer", WrapperNamespace="http://th.kpmg.intranet/", IsWrapped=true)]
    public partial class ws_MailServer {
        
        public ws_MailServer() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ws_MailServerResponse", WrapperNamespace="http://th.kpmg.intranet/", IsWrapped=true)]
    public partial class ws_MailServer1 {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://th.kpmg.intranet/", Order=0)]
        public string ws_MailServerResult;
        
        public ws_MailServer1() {
        }
        
        public ws_MailServer1(string ws_MailServerResult) {
            this.ws_MailServerResult = ws_MailServerResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ws_SendMail", WrapperNamespace="http://th.kpmg.intranet/", IsWrapped=true)]
    public partial class ws_SendMail {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://th.kpmg.intranet/", Order=0)]
        public string mFrom;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://th.kpmg.intranet/", Order=1)]
        public string nFrom;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://th.kpmg.intranet/", Order=2)]
        public string mTo;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://th.kpmg.intranet/", Order=3)]
        public string mCC;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://th.kpmg.intranet/", Order=4)]
        public string mBCC;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://th.kpmg.intranet/", Order=5)]
        public string mSbj;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://th.kpmg.intranet/", Order=6)]
        public string mMsg;
        
        public ws_SendMail() {
        }
        
        public ws_SendMail(string mFrom, string nFrom, string mTo, string mCC, string mBCC, string mSbj, string mMsg) {
            this.mFrom = mFrom;
            this.nFrom = nFrom;
            this.mTo = mTo;
            this.mCC = mCC;
            this.mBCC = mBCC;
            this.mSbj = mSbj;
            this.mMsg = mMsg;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ws_SendMailResponse", WrapperNamespace="http://th.kpmg.intranet/", IsWrapped=true)]
    public partial class ws_SendMail1 {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://th.kpmg.intranet/", Order=0)]
        public bool ws_SendMailResult;
        
        public ws_SendMail1() {
        }
        
        public ws_SendMail1(bool ws_SendMailResult) {
            this.ws_SendMailResult = ws_SendMailResult;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://th.kpmg.intranet/")]
    public enum MailPriority {
        
        /// <remarks/>
        Normal,
        
        /// <remarks/>
        Low,
        
        /// <remarks/>
        High,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ws_SendMail_Advance", WrapperNamespace="http://th.kpmg.intranet/", IsWrapped=true)]
    public partial class ws_SendMail_Advance {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://th.kpmg.intranet/", Order=0)]
        public string mFrom;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://th.kpmg.intranet/", Order=1)]
        public string nFrom;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://th.kpmg.intranet/", Order=2)]
        public string mTo;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://th.kpmg.intranet/", Order=3)]
        public string mCC;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://th.kpmg.intranet/", Order=4)]
        public string mBCC;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://th.kpmg.intranet/", Order=5)]
        public string mSbj;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://th.kpmg.intranet/", Order=6)]
        public string mMsg;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://th.kpmg.intranet/", Order=7)]
        public bool isTest;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://th.kpmg.intranet/", Order=8)]
        public string tTo;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://th.kpmg.intranet/", Order=9)]
        public TraineeManagement.wsEmail2.MailPriority priority;
        
        public ws_SendMail_Advance() {
        }
        
        public ws_SendMail_Advance(string mFrom, string nFrom, string mTo, string mCC, string mBCC, string mSbj, string mMsg, bool isTest, string tTo, TraineeManagement.wsEmail2.MailPriority priority) {
            this.mFrom = mFrom;
            this.nFrom = nFrom;
            this.mTo = mTo;
            this.mCC = mCC;
            this.mBCC = mBCC;
            this.mSbj = mSbj;
            this.mMsg = mMsg;
            this.isTest = isTest;
            this.tTo = tTo;
            this.priority = priority;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ws_SendMail_AdvanceResponse", WrapperNamespace="http://th.kpmg.intranet/", IsWrapped=true)]
    public partial class ws_SendMail_Advance1 {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://th.kpmg.intranet/", Order=0)]
        public bool ws_SendMail_AdvanceResult;
        
        public ws_SendMail_Advance1() {
        }
        
        public ws_SendMail_Advance1(bool ws_SendMail_AdvanceResult) {
            this.ws_SendMail_AdvanceResult = ws_SendMail_AdvanceResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ws_GetScriptAdminMail4AutoScript", WrapperNamespace="http://th.kpmg.intranet/", IsWrapped=true)]
    public partial class ws_GetScriptAdminMail4AutoScript {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://th.kpmg.intranet/", Order=0)]
        public string ScriptKey;
        
        public ws_GetScriptAdminMail4AutoScript() {
        }
        
        public ws_GetScriptAdminMail4AutoScript(string ScriptKey) {
            this.ScriptKey = ScriptKey;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ws_GetScriptAdminMail4AutoScriptResponse", WrapperNamespace="http://th.kpmg.intranet/", IsWrapped=true)]
    public partial class ws_GetScriptAdminMail4AutoScript1 {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://th.kpmg.intranet/", Order=0)]
        public string ws_GetScriptAdminMail4AutoScriptResult;
        
        public ws_GetScriptAdminMail4AutoScript1() {
        }
        
        public ws_GetScriptAdminMail4AutoScript1(string ws_GetScriptAdminMail4AutoScriptResult) {
            this.ws_GetScriptAdminMail4AutoScriptResult = ws_GetScriptAdminMail4AutoScriptResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface EMail_NewSoapChannel : TraineeManagement.wsEmail2.EMail_NewSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class EMail_NewSoapClient : System.ServiceModel.ClientBase<TraineeManagement.wsEmail2.EMail_NewSoap>, TraineeManagement.wsEmail2.EMail_NewSoap {
        
        public EMail_NewSoapClient() {
        }
        
        public EMail_NewSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public EMail_NewSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public EMail_NewSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public EMail_NewSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        TraineeManagement.wsEmail2.ws_MailServer1 TraineeManagement.wsEmail2.EMail_NewSoap.MailServer(TraineeManagement.wsEmail2.ws_MailServer request) {
            return base.Channel.MailServer(request);
        }
        
        public string MailServer() {
            TraineeManagement.wsEmail2.ws_MailServer inValue = new TraineeManagement.wsEmail2.ws_MailServer();
            TraineeManagement.wsEmail2.ws_MailServer1 retVal = ((TraineeManagement.wsEmail2.EMail_NewSoap)(this)).MailServer(inValue);
            return retVal.ws_MailServerResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<TraineeManagement.wsEmail2.ws_MailServer1> TraineeManagement.wsEmail2.EMail_NewSoap.MailServerAsync(TraineeManagement.wsEmail2.ws_MailServer request) {
            return base.Channel.MailServerAsync(request);
        }
        
        public System.Threading.Tasks.Task<TraineeManagement.wsEmail2.ws_MailServer1> MailServerAsync() {
            TraineeManagement.wsEmail2.ws_MailServer inValue = new TraineeManagement.wsEmail2.ws_MailServer();
            return ((TraineeManagement.wsEmail2.EMail_NewSoap)(this)).MailServerAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        TraineeManagement.wsEmail2.ws_SendMail1 TraineeManagement.wsEmail2.EMail_NewSoap.SendMail(TraineeManagement.wsEmail2.ws_SendMail request) {
            return base.Channel.SendMail(request);
        }
        
        public bool SendMail(string mFrom, string nFrom, string mTo, string mCC, string mBCC, string mSbj, string mMsg) {
            TraineeManagement.wsEmail2.ws_SendMail inValue = new TraineeManagement.wsEmail2.ws_SendMail();
            inValue.mFrom = mFrom;
            inValue.nFrom = nFrom;
            inValue.mTo = mTo;
            inValue.mCC = mCC;
            inValue.mBCC = mBCC;
            inValue.mSbj = mSbj;
            inValue.mMsg = mMsg;
            TraineeManagement.wsEmail2.ws_SendMail1 retVal = ((TraineeManagement.wsEmail2.EMail_NewSoap)(this)).SendMail(inValue);
            return retVal.ws_SendMailResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<TraineeManagement.wsEmail2.ws_SendMail1> TraineeManagement.wsEmail2.EMail_NewSoap.SendMailAsync(TraineeManagement.wsEmail2.ws_SendMail request) {
            return base.Channel.SendMailAsync(request);
        }
        
        public System.Threading.Tasks.Task<TraineeManagement.wsEmail2.ws_SendMail1> SendMailAsync(string mFrom, string nFrom, string mTo, string mCC, string mBCC, string mSbj, string mMsg) {
            TraineeManagement.wsEmail2.ws_SendMail inValue = new TraineeManagement.wsEmail2.ws_SendMail();
            inValue.mFrom = mFrom;
            inValue.nFrom = nFrom;
            inValue.mTo = mTo;
            inValue.mCC = mCC;
            inValue.mBCC = mBCC;
            inValue.mSbj = mSbj;
            inValue.mMsg = mMsg;
            return ((TraineeManagement.wsEmail2.EMail_NewSoap)(this)).SendMailAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        TraineeManagement.wsEmail2.ws_SendMail_Advance1 TraineeManagement.wsEmail2.EMail_NewSoap.SendMail_Advance(TraineeManagement.wsEmail2.ws_SendMail_Advance request) {
            return base.Channel.SendMail_Advance(request);
        }
        
        public bool SendMail_Advance(string mFrom, string nFrom, string mTo, string mCC, string mBCC, string mSbj, string mMsg, bool isTest, string tTo, TraineeManagement.wsEmail2.MailPriority priority) {
            TraineeManagement.wsEmail2.ws_SendMail_Advance inValue = new TraineeManagement.wsEmail2.ws_SendMail_Advance();
            inValue.mFrom = mFrom;
            inValue.nFrom = nFrom;
            inValue.mTo = mTo;
            inValue.mCC = mCC;
            inValue.mBCC = mBCC;
            inValue.mSbj = mSbj;
            inValue.mMsg = mMsg;
            inValue.isTest = isTest;
            inValue.tTo = tTo;
            inValue.priority = priority;
            TraineeManagement.wsEmail2.ws_SendMail_Advance1 retVal = ((TraineeManagement.wsEmail2.EMail_NewSoap)(this)).SendMail_Advance(inValue);
            return retVal.ws_SendMail_AdvanceResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<TraineeManagement.wsEmail2.ws_SendMail_Advance1> TraineeManagement.wsEmail2.EMail_NewSoap.SendMail_AdvanceAsync(TraineeManagement.wsEmail2.ws_SendMail_Advance request) {
            return base.Channel.SendMail_AdvanceAsync(request);
        }
        
        public System.Threading.Tasks.Task<TraineeManagement.wsEmail2.ws_SendMail_Advance1> SendMail_AdvanceAsync(string mFrom, string nFrom, string mTo, string mCC, string mBCC, string mSbj, string mMsg, bool isTest, string tTo, TraineeManagement.wsEmail2.MailPriority priority) {
            TraineeManagement.wsEmail2.ws_SendMail_Advance inValue = new TraineeManagement.wsEmail2.ws_SendMail_Advance();
            inValue.mFrom = mFrom;
            inValue.nFrom = nFrom;
            inValue.mTo = mTo;
            inValue.mCC = mCC;
            inValue.mBCC = mBCC;
            inValue.mSbj = mSbj;
            inValue.mMsg = mMsg;
            inValue.isTest = isTest;
            inValue.tTo = tTo;
            inValue.priority = priority;
            return ((TraineeManagement.wsEmail2.EMail_NewSoap)(this)).SendMail_AdvanceAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        TraineeManagement.wsEmail2.ws_GetScriptAdminMail4AutoScript1 TraineeManagement.wsEmail2.EMail_NewSoap.GetScriptAdminMail4AutoScript(TraineeManagement.wsEmail2.ws_GetScriptAdminMail4AutoScript request) {
            return base.Channel.GetScriptAdminMail4AutoScript(request);
        }
        
        public string GetScriptAdminMail4AutoScript(string ScriptKey) {
            TraineeManagement.wsEmail2.ws_GetScriptAdminMail4AutoScript inValue = new TraineeManagement.wsEmail2.ws_GetScriptAdminMail4AutoScript();
            inValue.ScriptKey = ScriptKey;
            TraineeManagement.wsEmail2.ws_GetScriptAdminMail4AutoScript1 retVal = ((TraineeManagement.wsEmail2.EMail_NewSoap)(this)).GetScriptAdminMail4AutoScript(inValue);
            return retVal.ws_GetScriptAdminMail4AutoScriptResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<TraineeManagement.wsEmail2.ws_GetScriptAdminMail4AutoScript1> TraineeManagement.wsEmail2.EMail_NewSoap.GetScriptAdminMail4AutoScriptAsync(TraineeManagement.wsEmail2.ws_GetScriptAdminMail4AutoScript request) {
            return base.Channel.GetScriptAdminMail4AutoScriptAsync(request);
        }
        
        public System.Threading.Tasks.Task<TraineeManagement.wsEmail2.ws_GetScriptAdminMail4AutoScript1> GetScriptAdminMail4AutoScriptAsync(string ScriptKey) {
            TraineeManagement.wsEmail2.ws_GetScriptAdminMail4AutoScript inValue = new TraineeManagement.wsEmail2.ws_GetScriptAdminMail4AutoScript();
            inValue.ScriptKey = ScriptKey;
            return ((TraineeManagement.wsEmail2.EMail_NewSoap)(this)).GetScriptAdminMail4AutoScriptAsync(inValue);
        }
    }
}