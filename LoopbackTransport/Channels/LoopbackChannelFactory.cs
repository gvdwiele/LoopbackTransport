using System.ServiceModel.Channels;
using System.Diagnostics;
using LoopbackTransport;
using System;
using LoopbackTransport.Channels;
using System.ServiceModel;

namespace LoopbackTransport.Channels
{

    public class LoopbackChannelFactory<TChannel> : ChannelFactoryBase<TChannel> 
    {
        // Fields
        private BindingContext _context;
        private LoopbackTransportBindingElement _element;
        private TryOpen _onOpen;

        // Methods
        public LoopbackChannelFactory()
        {
            Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, string.Format("LoopbackChannelFactory:Constructor", new object[0]));
        }

        public LoopbackChannelFactory(LoopbackTransportBindingElement element, BindingContext context)
            : base(context.Binding)
        {
            Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, string.Format("LoopbackChannelFactory:Constructor", new object[0]));
            this._element = element;
            this._context = context;
        }

        protected override IAsyncResult OnBeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
        {
            _onOpen = new TryOpen(this.OnOpen);
            return _onOpen.BeginInvoke(timeout,null,null);
        }

        protected override TChannel OnCreateChannel(EndpointAddress address, Uri via)
        {
            Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, string.Format("LoopbackChannelFactory:OnCreateChannel", new object[0]));
            if (typeof(TChannel) == typeof(IRequestChannel))
            {
                LoopbackRequestChannel channel = new LoopbackRequestChannel(this, address, via, this._element.CopyMessageProperties);
                channel.Open();
                return (TChannel)(object)channel;
            }
            if (typeof(TChannel) != typeof(IOutputChannel))
            {
                throw new NotSupportedException(typeof(TChannel).Name);
            }
            LoopbackOutputChannel channel2 = new LoopbackOutputChannel(this, address, via);
            channel2.Open();
            return (TChannel)(object)channel2;
        }

        protected override void OnEndOpen(IAsyncResult result)
        {
            Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, string.Format("LoopbackChannelFactory:OnEndOpen", new object[0]));
        }

        protected override void OnOpen(TimeSpan timeout)
        {
            Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, string.Format("LoopbackChannelFactory:OnOpen", new object[0]));
        }

        delegate void TryOpen(TimeSpan timeout);
    }

 
}

