namespace LoopbackTransport.Runtime
{
    using LoopbackTransport;
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.ServiceModel;
    using System.ServiceModel.Channels;

    public class LoopbackRequestChannel : ChannelBase, IRequestChannel, IChannel, ICommunicationObject
    {
        private bool _copyMessageProperties;
        private EndpointAddress _remoteAddress;
        private SendMessage _request;
        private Uri _via;

        public LoopbackRequestChannel(ChannelManagerBase factory, EndpointAddress remoteAddress, Uri via, bool copyMessageProperties) : base(factory)
        {
            Trace.WriteLineIf(Tracing.TraceSwitch.TraceVerbose, string.Format("LoopbackRequestChannel:Constructor {0}", via.ToString()));
            this._remoteAddress = remoteAddress;
            this._via = via;
            this._copyMessageProperties = copyMessageProperties;
        }

        public IAsyncResult BeginRequest(Message message, AsyncCallback callback, object state)
        {
            base.ThrowIfDisposedOrNotOpen();
            Trace.WriteLineIf(Tracing.TraceSwitch.TraceVerbose, string.Format("LoopbackRequestChannel:BeginSend {0}", this._via.ToString()));
            this._request = new SendMessage(this.Request);
            return this._request.BeginInvoke(message, callback, state);
        }

        public IAsyncResult BeginRequest(Message message, TimeSpan timeout, AsyncCallback callback, object state)
        {
            Trace.WriteLineIf(Tracing.TraceSwitch.TraceVerbose, string.Format("LoopbackRequestChannel:BeginSend with timeout {0}", this._via.ToString()));
            return this.BeginRequest(message, callback, state);
        }

        public Message EndRequest(IAsyncResult result)
        {
            Trace.WriteLineIf(Tracing.TraceSwitch.TraceVerbose, string.Format("LoopbackRequestChannel:EndSend {0}", this._via.ToString()));
            return this._request.EndInvoke(result);
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
            Trace.WriteLineIf(Tracing.TraceSwitch.TraceVerbose, string.Format("LoopbackRequestChannel:OnAbort {0}", this._via.ToString()));
        }

        protected override IAsyncResult OnBeginClose(TimeSpan timeout, AsyncCallback callback, object state)
        {
            Trace.WriteLineIf(Tracing.TraceSwitch.TraceVerbose, string.Format("LoopbackRequestChannel:OnBeginClose {0}", this._via.ToString()));
            throw new NotImplementedException();
        }

        protected override IAsyncResult OnBeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
        {
            Trace.WriteLineIf(Tracing.TraceSwitch.TraceVerbose, string.Format("LoopbackRequestChannel:OnBeginOpen {0}", this._via.ToString()));
            throw new NotImplementedException();
        }

        protected override void OnClose(TimeSpan timeout)
        {
            Trace.WriteLineIf(Tracing.TraceSwitch.TraceVerbose, string.Format("LoopbackRequestChannel:OnClose {0}", this._via.ToString()));
        }

        protected override void OnEndClose(IAsyncResult result)
        {
            Trace.WriteLineIf(Tracing.TraceSwitch.TraceVerbose, string.Format("LoopbackRequestChannel:OnEndClose {0}", this._via.ToString()));
        }

        protected override void OnEndOpen(IAsyncResult result)
        {
            Trace.WriteLineIf(Tracing.TraceSwitch.TraceVerbose, string.Format("LoopbackRequestChannel:OnEndOpen {0}", this._via.ToString()));
        }

        protected override void OnOpen(TimeSpan timeout)
        {
            Trace.WriteLineIf(Tracing.TraceSwitch.TraceVerbose, string.Format("LoopbackRequestChannel:OnOpen {0}", this._via.ToString()));
        }

        public Message Request(Message msg)
        {
            base.ThrowIfDisposedOrNotOpen();
            bool flag = true;
            MessageBuffer buffer = msg.CreateBufferedCopy(0x7fffffff);
            if (flag)
            {
                Trace.WriteLineIf(Tracing.TraceSwitch.TraceVerbose, string.Format("Message body in:\n{0}", buffer.CreateMessage().ToString()));
            }
            Message message = buffer.CreateMessage();
            string body = message.GetReaderAtBodyContents().ReadContentAsString();
            Message message2 = Message.CreateMessage(MessageVersion.Soap12, "Loopback", body);
            if (this._copyMessageProperties)
            {
                message2.Properties.CopyProperties(message.Properties);
            }
            buffer = message2.CreateBufferedCopy(0x7fffffff);
            if (flag)
            {
                Trace.WriteLineIf(Tracing.TraceSwitch.TraceVerbose, string.Format("Message body out:\n{0}", buffer.CreateMessage().ToString()));
            }
            return buffer.CreateMessage();
        }

        public Message Request(Message message, TimeSpan timeout)
        {
            Trace.WriteLineIf(Tracing.TraceSwitch.TraceVerbose, string.Format("LoopbackRequestChannel:Request with timeout {0}", this._via.ToString()));
            return this.Request(message);
        }

        public EndpointAddress RemoteAddress
        {
            get
            {
                Trace.WriteLineIf(Tracing.TraceSwitch.TraceVerbose, string.Format("LoopbackRequestChannel:EndpointAddress Get", new object[0]));
                return this._remoteAddress;
            }
        }

        public Uri Via
        {
            get
            {
                Trace.WriteLineIf(Tracing.TraceSwitch.TraceVerbose, string.Format("LoopbackRequestChannel:Uri Get", new object[0]));
                return this._via;
            }
        }

        private delegate Message SendMessage(Message msg);
    }
}

