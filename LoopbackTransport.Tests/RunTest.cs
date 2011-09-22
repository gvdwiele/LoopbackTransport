using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.ServiceModel.Channels;
using System.Xml;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Diagnostics;
using LoopbackTransport.Tests;
using System.IO;
using LoopbackTransport.Installation;
using LoopbackTransport.Channels;

namespace LoopbackTransportTests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class RunTest
    {
        [TestInitialize]
        public void LoopbackIntaller()
        {
            LoopbackTransportInstaller.Install();
        }

        [TestCleanup]
        public void LoopbackUnintaller()
        {
            //LoopbackTransportInstaller.Uninstall();
        }

        [TestMethod]
        public void FromConfigRun()
        {
            Message msg = CreateTestMessage();
            using (LoopbackClient client = new LoopbackClient())
            {
                client.Send(msg);
            }
        }

        [TestMethod]
        public void IRequestChannelPullsStream()
        {
            Message msg = CreateTestMessage();
            var channel = GetLoopbackChannel<IRequestChannel>(true);
            channel.Request(msg);
        }

        [TestMethod]
        public void IRequestChannelCopiesStream()
        {
            Message request = CreateTestMessage("123456");
            var channel = GetLoopbackChannel<IRequestChannel>(true);
            Message response = channel.Request(request);
            
        }

        [TestMethod]
        public void IOutputChannelPullsStream()
        {
            Message msg = CreateTestMessage();
            var channel = GetLoopbackChannel<IOutputChannel>(true);
            channel.Send(msg);
        }

        [TestMethod]
        public void CanCopyProperties()
        {
            Message msg = CreateTestMessage();
            msg.Properties.Add("prop", 0);

            var channel = GetLoopbackChannel<IRequestChannel>(true);
            Message response = channel.Request(msg);
            Assert.IsTrue(response.Properties.ContainsKey("prop"));
        }

        [TestMethod]
        public void CanRemoveProperties()
        {
            Message msg = CreateTestMessage();
            msg.Properties.Add("prop", 0);

            var channel = GetLoopbackChannel<IRequestChannel>(false);
            Message response = channel.Request(msg);
            Assert.IsFalse(response.Properties.ContainsKey("prop"));
        }

        private static Message CreateTestMessage()
        {
            return CreateTestMessage("Hello I'm the default bodystring");
        }

        private static Message CreateTestMessage(string body)
        {
            return Message.CreateMessage(MessageVersion.None, "testrun", body);
        }

        private static Message CreateTestMessage(Stream body)
        {
            Message msg;
            string bodyString = "Hello I'm the bodystring";
            msg = Message.CreateMessage(MessageVersion.None, "testrun", bodyString);
            return msg;
        }

        private static T GetLoopbackChannel<T>(bool CopyMessageProperties)
        {
            CustomBinding binding = new CustomBinding();
            LoopbackTransportBindingElement loopback = new LoopbackTransportBindingElement();
            loopback.CopyMessageProperties = CopyMessageProperties;
            binding.Elements.Add(loopback);
            var factory = binding.BuildChannelFactory<T>(new BindingParameterCollection());
            factory.Open();
            T channel = factory.CreateChannel(new EndpointAddress("net.loop://whatever"));
            return channel;
        }

    }
}
