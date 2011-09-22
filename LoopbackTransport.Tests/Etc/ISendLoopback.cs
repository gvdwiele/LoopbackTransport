using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace LoopbackTransport.Tests
{
    [ServiceContract]
    interface ISendLoopback
    {
        [OperationContract(Action="*",ReplyAction="*")]
        Message Send(Message msg);
    }
}
