using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SoapService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISecondService" in both code and config file together.
    [ServiceContract(Namespace ="http://www.orioner.ru")]
    public interface ISecondService
    {
        [OperationContract()]
        Result Factorial(string s);
    }
}
