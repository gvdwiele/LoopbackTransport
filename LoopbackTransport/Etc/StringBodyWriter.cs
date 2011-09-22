using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;

namespace LoopbackTransport.Etc
{
    public class StringBodyWriter : BodyWriter
    {
        string _msg;
        public StringBodyWriter(string message)
            : base(false)
        {
            _msg = message;
        }

        protected override void OnWriteBodyContents(System.Xml.XmlDictionaryWriter writer)
        {
            writer.WriteRaw(_msg);
        }
    }
}