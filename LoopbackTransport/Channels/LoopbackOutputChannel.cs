using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Diagnostics;

namespace LoopbackTransport.Channels
{

    public class LoopbackOutputChannel : ChannelBase, IOutputChannel, IChannel, ICommunicationObject
    {
        // Fields
        private EndpointAddress _remoteAddress;
        private Action<Message> _send;
        private Uri _via;

        // Methods
        public LoopbackOutputChannel(ChannelManagerBase factory, EndpointAddress remoteAddress, Uri via)
            : base(factory)
        {
            Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, string.Format("LoopbackOutputChannel:Constructor {0}", via.ToString()));
            this._remoteAddress = remoteAddress;
            this._via = via;
        }

        public IAsyncResult BeginSend(Message message, AsyncCallback callback, object state)
        {
            base.ThrowIfDisposedOrNotOpen();
            Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, string.Format("LoopbackOutputChannel:BeginSend {0}", this._via.ToString()));
            this._send = new Action<Message>(this.Send);
            return this._send.BeginInvoke(message, callback, state);
        }

        public IAsyncResult BeginSend(Message message, TimeSpan timeout, AsyncCallback callback, object state)
        {
            Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, string.Format("LoopbackOutputChannel:BeginSend with timeout {0}", this._via.ToString()));
            return this.BeginSend(message, callback, state);
        }

        public void EndSend(IAsyncResult result)
        {
            Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, string.Format("LoopbackOutputChannel:EndSend {0}", this._via.ToString()));
            this._send.EndInvoke(result);
        }

        public override T GetProperty<T>()
        {
            if (typeof(T) == typeof(FaultConverter))
            {
                return (FaultConverter.GetDefaultFaultConverter(MessageVersion.Soap12WSAddressing10) as T);
            }
            return base.GetProperty<T>();
        }

        protected override void OnAbort()
        {
            Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, string.Format("LoopbackOutputChannel:OnAbort {0}", this._via.ToString()));
        }

        protected override IAsyncResult OnBeginClose(TimeSpan timeout, AsyncCallback callback, object state)
        {
            Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, string.Format("LoopbackOutputChannel:OnBeginClose {0}", this._via.ToString()));
            throw new NotImplementedException();
        }

        protected override IAsyncResult OnBeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
        {
            Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, string.Format("LoopbackOutputChannel:OnBeginOpen {0}", this._via.ToString()));
            throw new NotImplementedException();
        }

        protected override void OnClose(TimeSpan timeout)
        {
            Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, string.Format("LoopbackOutputChannel:OnClose {0}", this._via.ToString()));
        }

        protected override void OnEndClose(IAsyncResult result)
        {
            Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, string.Format("LoopbackOutputChannel:OnEndClose {0}", this._via.ToString()));
        }

        protected override void OnEndOpen(IAsyncResult result)
        {
            Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, string.Format("LoopbackOutputChannel:OnEndOpen {0}", this._via.ToString()));
        }

        protected override void OnOpen(TimeSpan timeout)
        {
            Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, string.Format("LoopbackOutputChannel:OnOpen {0}", this._via.ToString()));
        }

        public void Send(Message request)
        {
            base.ThrowIfDisposedOrNotOpen();

            MessageBuffer buffer = request.CreateBufferedCopy(0x7fffffff);
            string body = buffer.CreateMessage().GetReaderAtBodyContents().ReadContentAsString();

            Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, string.Format("LoopbackOutputChannel:Send {0}", this._via.ToString()));
            Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, "Sending message: " + body);
        }

        public void Send(Message message, TimeSpan timeout)
        {
            Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, string.Format("LoopbackOutputChannel:Send with timeout {0}", this._via.ToString()));
            this.Send(message);
        }

        // Properties
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

