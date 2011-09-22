using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace LoopbackTransport.Channels
{
    public class LoopbackRequestSessionChannel : LoopbackRequestChannel, IRequestSessionChannel
    {
        public LoopbackRequestSessionChannel(ChannelManagerBase factory, EndpointAddress address, Uri via) 
        {
        }

        #region IRequestChannel Members

        public IAsyncResult BeginRequest(Message message, TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginRequest(Message message, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public Message EndRequest(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public EndpointAddress RemoteAddress
        {
            get { throw new NotImplementedException(); }
        }

        public Message Request(Message message, TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        public Message Request(Message message)
        {
            throw new NotImplementedException();
        }

        public Uri Via
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IChannel Members

        public T GetProperty<T>() where T : class
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ICommunicationObject Members

        public void Abort()
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginClose(TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginClose(AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginOpen(AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public void Close(TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public event EventHandler Closed;

        public event EventHandler Closing;

        public void EndClose(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public void EndOpen(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public event EventHandler Faulted;

        public void Open(TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        public void Open()
        {
            throw new NotImplementedException();
        }

        public event EventHandler Opened;

        public event EventHandler Opening;

        public CommunicationState State
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region ISessionChannel<IOutputSession> Members

        public IOutputSession Session
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
