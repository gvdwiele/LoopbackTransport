using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace LoopbackTransport.Tests
{
    class LoopbackClient :  ClientBase<ISendLoopback>, ISendLoopback
    {

        public System.ServiceModel.Channels.Message Send(System.ServiceModel.Channels.Message msg)
        {
            return base.Channel.Send(msg);
        }

    }
}
