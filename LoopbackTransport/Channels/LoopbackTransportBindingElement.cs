using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace LoopbackTransport.Channels
{
    public class LoopbackTransportBindingElement : TransportBindingElement
    {
        private bool _copyMessageProperties;

        public bool CopyMessageProperties
        {
            get { return _copyMessageProperties; }
            set {  _copyMessageProperties = value; }
        }

        public LoopbackTransportBindingElement()
        {
        }

        public LoopbackTransportBindingElement(LoopbackTransportBindingElement elementToBeCloned)
            : base(elementToBeCloned)
        {
            //copy value properties and deep copy reference types
            _copyMessageProperties = elementToBeCloned.CopyMessageProperties;
        }

        public override string Scheme
        {
            get
            {
                return "net.loop";
            }
        }

        public override BindingElement Clone()
        {
            return new LoopbackTransportBindingElement(this);
        }

        public override bool CanBuildChannelFactory<TChannel>(BindingContext context)
        {
            return typeof(TChannel) == typeof(IRequestChannel) ||
                //typeof(TChannel) == typeof(IRequestSessionChannel) ||
                typeof(TChannel) == typeof(IOutputChannel) ;
        }

        public override bool CanBuildChannelListener<TChannel>(BindingContext context)
        {
            //only supposed to be used for server-side
            return false;
        }

        public override IChannelFactory<TChannel> BuildChannelFactory<TChannel>(BindingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (!CanBuildChannelFactory<TChannel>(context))
            {
                throw new ArgumentOutOfRangeException(string.Format("Unsupported channel type {0}.", typeof(TChannel)));
            }
            return new LoopbackChannelFactory<TChannel>(this, context);
        }

        public override IChannelListener<TChannel> BuildChannelListener<TChannel>(BindingContext context)
        {
            throw new NotSupportedException("This is a client-side transport.");
        }

        public override T GetProperty<T>(BindingContext context)
        {
            return base.GetProperty<T>(context);
        }
    }
}
