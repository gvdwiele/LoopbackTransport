using System.Configuration;
using System;
using System.ServiceModel.Configuration;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Deployment.WindowsInstaller;
using LoopbackTransport.Configuration;
using System.IO;

namespace LoopbackTransport.Installation
{
    public static class LoopbackTransportInstaller
    {
        static ExtensionElement _loopbackExtensionElement = new ExtensionElement("loopbackTransport", typeof(LoopbackTransportBindingElementExtensionElement).FullName + "," + typeof(LoopbackTransportBindingElementExtensionElement).Assembly.FullName);

        const string _v4framework32MachineConfig = @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\CONFIG\machine.config";
        const string _v4framework64MachineConfig = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\CONFIG\machine.config";

        [CustomAction("RegisterLoopbackTransport")]
        public static ActionResult Install(Session session)
        {
            try
            {
                Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, "LoopbackTransportInstaller:RegisterLoopbackTransport start");
                Install();
                return ActionResult.Success;
            }
            catch (Exception x)
            {
                Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, "LoopbackTransportInstaller:RegisterLoopbackTransport exception:" + x.ToString());
                return ActionResult.Failure;
            }

        }

        [CustomAction("UnregisterLoopbackTransport")]
        public static ActionResult Uninstall(Session session)
        {
            try
            {
                Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, "LoopbackTransportInstaller:UnregisterLoopbackTransport start");
                Uninstall();
            }
            catch (Exception x)
            {
                Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, "LoopbackTransportInstaller:UnregisterLoopbackTransport exception:" + x.ToString());
            }

            return ActionResult.Success;
        }

        /// <summary>
        /// Adds the LOOPBACK adapter's WCF configuration elements to the machine.config
        /// </summary>
        public static void Install()
        {
            if (File.Exists(_v4framework32MachineConfig))
                Install(_v4framework32MachineConfig);
            if (File.Exists(_v4framework64MachineConfig))
                Install(_v4framework64MachineConfig);
        }

        /// <summary>
        /// Removes the LOOPBACK adapter's WCF configuration elements from the machine.config
        /// </summary>
        public static void Uninstall()
        {
            if (File.Exists(_v4framework32MachineConfig))
                Uninstall(_v4framework32MachineConfig);
            if (File.Exists(_v4framework64MachineConfig))
                Uninstall(_v4framework64MachineConfig);
        }

        public static void Install(string filepath)
        {
            Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, "LoopbackTransportInstaller:Install for " + filepath);
            WcfMachineConfiguration config = new WcfMachineConfiguration(filepath);
            config.UpdateBindingElementExtension(_loopbackExtensionElement);
        }

        public static void Uninstall(string filepath)
        {
            Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, "LoopbackTransportInstaller:Uninstall for " + filepath);
            WcfMachineConfiguration config = new WcfMachineConfiguration(filepath);
            config.RemoveBindingElementExtension(_loopbackExtensionElement.Name);
        }

    }
}
