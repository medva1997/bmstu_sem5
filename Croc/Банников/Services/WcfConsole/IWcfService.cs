using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfConsole
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IWcfService" in both code and config file together.
    [ServiceContract(Namespace = "http://www.orioner.ru")]
    public interface IWcfService
    {
        [OperationContract()]
        [WebGet(ResponseFormat = WebMessageFormat.Xml, 
            UriTemplate ="Factorial/{s}", 
            BodyStyle =  WebMessageBodyStyle.Bare)]
        Result Factorial(string s);
    }
}
