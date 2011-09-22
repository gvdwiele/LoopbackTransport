using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Diagnostics;

namespace LoopbackTransport.Runtime
{
    public class LoopbackChannelFactory<TChannel> : ChannelFactoryBase<TChannel>
    {
        private BindingContext _context;
        private LoopbackTransportBindingElement _element;

        public LoopbackChannelFactory()
        {
            Trace.WriteLineIf(Tracing.TraceSwitch.TraceVerbose, string.Format("LoopbackChannelFactory:Constructor", new object[0]));
        }

        public LoopbackChannelFactory(LoopbackTransportBindingElement element, BindingContext context) : base(context.Binding)
        {
            Trace.WriteLineIf(Tracing.TraceSwitch.TraceVerbose, string.Format("LoopbackChannelFactory:Constructor", new object[0]));
            this._element = element;
            this._context = context;
        }

        protected override IAsyncResult OnBeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
        {
            Trace.WriteLineIf(Tracing.TraceSwitch.TraceVerbose, string.Format("LoopbackChannelFactory:OnBeginOpen", new object[0]));
            return delegate (TimeSpan _timeout) {
                Debug.WriteLine("OnBeginOpen");
            }.BeginInvoke(timeout, null, null);
        }

        protected override TChannel OnCreateChannel(EndpointAddress address, Uri via)
        {
            Trace.WriteLineIf(Tracing.TraceSwitch.TraceVerbose, string.Format("LoopbackChannelFactory:OnCreateChannel", new object[0]));
            if (typeof(TChannel) == typeof(IRequestChannel))
            {
                LoopbackRequestChannel channel = new LoopbackRequestChannel(this, address, via, this._element.CopyMessageProperties);
                channel.Open();
                return (TChannel) channel;
            }
            if (typeof(TChannel) != typeof(IOutputChannel))
            {
                throw new NotSupportedException(typeof(TChannel).Name);
            }
            LoopbackOutputChannel channel2 = new LoopbackOutputChannel(this, address, via);
            channel2.Open();
            return (TChannel) channel2;
        }

    }
}

