using System.ServiceModel.Channels;
using System;
using System.Diagnostics;
using System.ServiceModel;
using System.Transactions;
using System.Xml;
using System.IO;

namespace LoopbackTransport.Channels
{

    public class LoopbackRequestChannel : ChannelBase, IRequestChannel, IChannel, ICommunicationObject
    {
        // Fields
        private bool _copyMessageProperties;
        private EndpointAddress _remoteAddress;
        private SendMessage _request;
        private Uri _via;

        // Methods
        public LoopbackRequestChannel(ChannelManagerBase factory, EndpointAddress remoteAddress, Uri via, bool copyMessageProperties)
            : base(factory)
        {
            Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, string.Format("LoopbackRequestChannel:Constructor {0}", via.ToString()));
            this._remoteAddress = remoteAddress;
            this._via = via;
            this._copyMessageProperties = copyMessageProperties;
        }

        public IAsyncResult BeginRequest(Message message, AsyncCallback callback, object state)
        {
            base.ThrowIfDisposedOrNotOpen();
            Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, string.Format("LoopbackRequestChannel:BeginSend {0}", this._via.ToString()));
            this._request = new SendMessage(this.Request);
            return this._request.BeginInvoke(message, callback, state);
        }

        public IAsyncResult BeginRequest(Message message, TimeSpan timeout, AsyncCallback callback, object state)
        {
            Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, string.Format("LoopbackRequestChannel:BeginSend with timeout {0}", this._via.ToString()));
            return this.BeginRequest(message, callback, state);
        }

        public Message EndRequest(IAsyncResult result)
        {
            Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, string.Format("LoopbackRequestChannel:EndSend {0}", this._via.ToString()));
            return this._request.EndInvoke(result);
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
            Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, string.Format("LoopbackRequestChannel:OnAbort {0}", this._via.ToString()));
        }

        protected override IAsyncResult OnBeginClose(TimeSpan timeout, AsyncCallback callback, object state)
        {
            Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, string.Format("LoopbackRequestChannel:OnBeginClose {0}", this._via.ToString()));
            throw new NotImplementedException();
        }

        protected override IAsyncResult OnBeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
        {
            Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, string.Format("LoopbackRequestChannel:OnBeginOpen {0}", this._via.ToString()));
            throw new NotImplementedException();
        }

        protected override void OnClose(TimeSpan timeout)
        {
            Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, string.Format("LoopbackRequestChannel:OnClose {0}", this._via.ToString()));
        }

        protected override void OnEndClose(IAsyncResult result)
        {
            Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, string.Format("LoopbackRequestChannel:OnEndClose {0}", this._via.ToString()));
        }

        protected override void OnEndOpen(IAsyncResult result)
        {
            Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, string.Format("LoopbackRequestChannel:OnEndOpen {0}", this._via.ToString()));
        }

        protected override void OnOpen(TimeSpan timeout)
        {
            Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, string.Format("LoopbackRequestChannel:OnOpen {0}", this._via.ToString()));
        }

        private Message Request(Message msg)
        {
            base.ThrowIfDisposedOrNotOpen();

            MessageBuffer buffer = msg.CreateBufferedCopy(0x7fffffff);
            string body = buffer.CreateMessage().GetReaderAtBodyContents().ReadContentAsString();

            Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, string.Format("LoopbackRequestChannel:Request body-content: \n{0}", body));

            //when returning a copy of the incoming message (via a buffer) it is no good for BizTalk

            Message response = Message.CreateMessage(MessageVersion.Soap12, "Loopback", body);
            if (this._copyMessageProperties)
            {
                Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, string.Format("LoopbackRequestChannel:Request copying message props"));
                response.Properties.CopyProperties(buffer.CreateMessage().Properties);
            }

            return response;
        }

        private static string GetBody(Message request)
        {
            MemoryStream mem = new MemoryStream();
            XmlDictionaryWriter writer = XmlDictionaryWriter.CreateTextWriter(mem);
            request.WriteBodyContents(writer);
            writer.Flush();
            mem.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(mem);
            return sr.ReadToEnd();
        }

        private static string GetBodyAlt(Message request)
        {
            return request.GetReaderAtBodyContents().ReadContentAsString();
        }

        public Message Request(Message message, TimeSpan timeout)
        {
            Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, string.Format("LoopbackRequestChannel:Request with timeout {0}", timeout.ToString()));
            return this.Request(message);
        }

        // Properties
        public EndpointAddress RemoteAddress
        {
            get
            {
                Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, string.Format("LoopbackRequestChannel:RemoteAddress get {0}", this._remoteAddress));
                return this._remoteAddress;
            }
        }

        public Uri Via
        {
            get
            {
                Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, string.Format("LoopbackRequestChannel:Via get {0}", this._via));
                return this._via;
            }
        }

        // Nested Types
        private delegate Message SendMessage(Message msg);
    }



}

