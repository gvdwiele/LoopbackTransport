namespace LoopbackTransport.Runtime
{
    using LoopbackTransport;
    using System;
    using System.Diagnostics;
    using System.ServiceModel;
    using System.ServiceModel.Channels;

    public class LoopbackOutputChannel : ChannelBase, IOutputChannel, IChannel, ICommunicationObject
    {
        private EndpointAddress _remoteAddress;
        private Action<Message> _send;
        private Uri _via;

        public LoopbackOutputChannel(ChannelManagerBase factory, EndpointAddress remoteAddress, Uri via) : base(factory)
        {
            Trace.WriteLineIf(Tracing.TraceSwitch.TraceVerbose, string.Format("LoopbackOutputChannel:Constructor {0}", via.ToString()));
            this._remoteAddress = remoteAddress;
            this._via = via;
        }

        public IAsyncResult BeginSend(Message message, AsyncCallback callback, object state)
        {
            base.ThrowIfDisposedOrNotOpen();
            Trace.WriteLineIf(Tracing.TraceSwitch.TraceVerbose, string.Format("LoopbackOutputChannel:BeginSend {0}", this._via.ToString()));
            this._send = new Action<Message>(this.Send);
            return this._send.BeginInvoke(message, callback, state);
        }

        public IAsyncResult BeginSend(Message message, TimeSpan timeout, AsyncCallback callback, object state)
        {
            Trace.WriteLineIf(Tracing.TraceSwitch.TraceVerbose, string.Format("LoopbackOutputChannel:BeginSend with timeout {0}", this._via.ToString()));
            return this.BeginSend(message, callback, state);
        }

        public void EndSend(IAsyncResult result)
        {
            Trace.WriteLineIf(Tracing.TraceSwitch.TraceVerbose, string.Format("LoopbackOutputChannel:EndSend {0}", this._via.ToString()));
            this._send.EndInvoke(result);
        }

        public override T GetProperty<T>() where T: class
        {
            if (typeof(T) == typeof(FaultConverter))
            {
                return (FaultConverter.GetDefaultFaultConverter(MessageVersion.Soap12WSAddressing10) as T);
            }
            return base.GetProperty<T>();
        }

        protected override void OnAbort()
        {
            Trace.WriteLineIf(Tracing.TraceSwitch.TraceVerbose, string.Format("LoopbackOutputChannel:OnAbort {0}", this._via.ToString()));
        }

        protected override IAsyncResult OnBeginClose(TimeSpan timeout, AsyncCallback callback, object state)
        {
            Trace.WriteLineIf(Tracing.TraceSwitch.TraceVerbose, string.Format("LoopbackOutputChannel:OnBeginClose {0}", this._via.ToString()));
            throw new NotImplementedException();
        }

        protected override IAsyncResult OnBeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
        {
            Trace.WriteLineIf(Tracing.TraceSwitch.TraceVerbose, string.Format("LoopbackOutputChannel:OnBeginOpen {0}", this._via.ToString()));
            throw new NotImplementedException();
        }

        protected override void OnClose(TimeSpan timeout)
        {
            Trace.WriteLineIf(Tracing.TraceSwitch.TraceVerbose, string.Format("LoopbackOutputChannel:OnClose {0}", this._via.ToString()));
        }

        protected override void OnEndClose(IAsyncResult result)
        {
            Trace.WriteLineIf(Tracing.TraceSwitch.TraceVerbose, string.Format("LoopbackOutputChannel:OnEndClose {0}", this._via.ToString()));
        }

        protected override void OnEndOpen(IAsyncResult result)
        {
            Trace.WriteLineIf(Tracing.TraceSwitch.TraceVerbose, string.Format("LoopbackOutputChannel:OnEndOpen {0}", this._via.ToString()));
        }

        protected override void OnOpen(TimeSpan timeout)
        {
            Trace.WriteLineIf(Tracing.TraceSwitch.TraceVerbose, string.Format("LoopbackOutputChannel:OnOpen {0}", this._via.ToString()));
        }

        public void Send(Message message)
        {
            base.ThrowIfDisposedOrNotOpen();
            Trace.WriteLineIf(Tracing.TraceSwitch.TraceVerbose, string.Format("LoopbackOutputChannel:Send {0}", this._via.ToString()));
            Trace.WriteLine("Sending message: " + message.ToString());
        }

        public void Send(Message message, TimeSpan timeout)
        {
            Trace.WriteLineIf(Tracing.TraceSwitch.TraceVerbose, string.Format("LoopbackOutputChannel:Send with timeout {0}", this._via.ToString()));
            this.Send(message);
        }

        public EndpointAddress RemoteAddress
        {
            get
            {
                return this._remoteAddress;
            }
        }

        public Uri Via
        {
            get
            {
                return this._via;
            }
        }
    }
}

