using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TorrentLibrary;

namespace WCFService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISignInService" in both code and config file together.
    [ServiceContract]
    public interface ISignInService
    {
        [OperationContract]
        bool login(string user, string password);

        [OperationContract]
        bool sendUserData(string userData); //Including list of files

        [OperationContract]
        List<string> getPeers(string fileName);
    }
}
