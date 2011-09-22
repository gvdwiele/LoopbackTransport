using System.Configuration;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using LoopbackTransport.Tests;
using System.ServiceModel.Configuration;
using LoopbackTransport.Installation;
using LoopbackTransport;

namespace LoopbackTransportTests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class WcfMachineConfigurationTests
    {
        

        [TestMethod]
        public void CanGetDefaultMachineConfig()
        {
            WcfMachineConfiguration config = new WcfMachineConfiguration();
            Assert.IsNotNull(config);
        }

        [TestMethod]
        public void CanGetMachineConfigFromPath()
        {
            string filepath = DocLoader.CreateTempMachineConfig();
            WcfMachineConfiguration config = new WcfMachineConfiguration(filepath);
            Assert.IsNotNull(config);
        }

        [TestMethod]
        public void TryGetBindingElementExtension()
        {
            string filepath = DocLoader.CreateTempMachineConfig();
            WcfMachineConfiguration config = new WcfMachineConfiguration(filepath);

            ExtensionElement element;
            Assert.IsTrue(config.TryGetBindingElementExtension("loopbackTransport", out element));
        }

        [TestMethod]
        public void AddBindingElementExtension()
        {
            string filepath = DocLoader.CreateTempMachineConfig();
            WcfMachineConfiguration config = new WcfMachineConfiguration(filepath);

            string name = new Random().Next().ToString();
            ExtensionElement extension1;
            extension1 = new ExtensionElement(name, this.GetType().AssemblyQualifiedName);
            Assert.IsFalse(config.TryGetBindingElementExtension(name, out extension1));
            extension1 = new ExtensionElement(name, this.GetType().AssemblyQualifiedName);
            config.UpdateBindingElementExtension(extension1);
            Assert.IsTrue(config.TryGetBindingElementExtension(name, out extension1));

        }

        [TestMethod]
        public void RemoveBindingElementExtension()
        {
            string filepath = DocLoader.CreateTempMachineConfig();
            WcfMachineConfiguration config = new WcfMachineConfiguration(filepath);

            string name = new Random().Next().ToString();
            ExtensionElement extension1 = new ExtensionElement(name, this.GetType().AssemblyQualifiedName);

            config.UpdateBindingElementExtension(extension1);

            Assert.IsTrue(config.TryGetBindingElementExtension(name, out extension1));

            config.RemoveBindingElementExtension(name);

            Assert.IsFalse(config.TryGetBindingElementExtension(name, out extension1));

        }

    }
}
