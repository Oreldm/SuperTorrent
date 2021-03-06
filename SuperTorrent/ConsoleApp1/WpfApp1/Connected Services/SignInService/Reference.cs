﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WpfApp1.SignInService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SignInService.ISignInService")]
    public interface ISignInService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignInService/login", ReplyAction="http://tempuri.org/ISignInService/loginResponse")]
        bool login(string user, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignInService/login", ReplyAction="http://tempuri.org/ISignInService/loginResponse")]
        System.Threading.Tasks.Task<bool> loginAsync(string user, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignInService/sendUserData", ReplyAction="http://tempuri.org/ISignInService/sendUserDataResponse")]
        bool sendUserData(string userData);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignInService/sendUserData", ReplyAction="http://tempuri.org/ISignInService/sendUserDataResponse")]
        System.Threading.Tasks.Task<bool> sendUserDataAsync(string userData);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignInService/getPeers", ReplyAction="http://tempuri.org/ISignInService/getPeersResponse")]
        string[] getPeers(string fileName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISignInService/getPeers", ReplyAction="http://tempuri.org/ISignInService/getPeersResponse")]
        System.Threading.Tasks.Task<string[]> getPeersAsync(string fileName);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ISignInServiceChannel : WpfApp1.SignInService.ISignInService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SignInServiceClient : System.ServiceModel.ClientBase<WpfApp1.SignInService.ISignInService>, WpfApp1.SignInService.ISignInService {
        
        public SignInServiceClient() {
        }
        
        public SignInServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SignInServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SignInServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SignInServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public bool login(string user, string password) {
            return base.Channel.login(user, password);
        }
        
        public System.Threading.Tasks.Task<bool> loginAsync(string user, string password) {
            return base.Channel.loginAsync(user, password);
        }
        
        public bool sendUserData(string userData) {
            return base.Channel.sendUserData(userData);
        }
        
        public System.Threading.Tasks.Task<bool> sendUserDataAsync(string userData) {
            return base.Channel.sendUserDataAsync(userData);
        }
        
        public string[] getPeers(string fileName) {
            return base.Channel.getPeers(fileName);
        }
        
        public System.Threading.Tasks.Task<string[]> getPeersAsync(string fileName) {
            return base.Channel.getPeersAsync(fileName);
        }
    }
}
