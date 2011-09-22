using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using System.ServiceModel.Configuration;

namespace LoopbackTransport.Installation
{
    public class WcfMachineConfiguration
    {
        System.Configuration.Configuration _config;
        object _syncLock = new object();

        public WcfMachineConfiguration(string filepath)
        {
            _config = ConfigurationManager.OpenMappedMachineConfiguration(new ConfigurationFileMap(filepath));

            if (_config == null)
                throw new Exception(string.Format("OpenMappedMachineConfiguration returned null for path '{0}'.", filepath));

            //check if WCF is present
            ServiceModelSectionGroup serviceModelGroup = ServiceModelSectionGroup.GetSectionGroup(_config);

        }

        public WcfMachineConfiguration()
            : this(ConfigurationManager.OpenMachineConfiguration().FilePath)
        {
        }

        public string Path
        {
            get
            {
                return _config.FilePath;
            }
        }

        /// <summary>
        /// When not present the extensionElement will be added to the configuration.
        /// When present it will be replaced.
        /// </summary>
        /// <param name="extensionElement"></param>
        public void UpdateBindingElementExtension(ExtensionElement extensionElement)
        {
            ServiceModelSectionGroup serviceModel = ServiceModelSectionGroup.GetSectionGroup(_config);
            ExtensionElementCollection bindingElementExtensions = serviceModel.Extensions.BindingElementExtensions;
            lock (_syncLock)
            {
                if (bindingElementExtensions.ContainsKey(extensionElement.Name))
                {
                    bindingElementExtensions.RemoveAt(extensionElement.Name);
                }
                bindingElementExtensions.Add(extensionElement);
                _config.Save();
            }
            
        }

        public void RemoveBindingElementExtension(string name)
        {
            Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, "WcfMachineConfiguration:RemoveBindingElementExtension for " + name);
            ServiceModelSectionGroup serviceModel = ServiceModelSectionGroup.GetSectionGroup(_config);
            ExtensionElementCollection bindingElementExtensions = serviceModel.Extensions.BindingElementExtensions;
            lock (_syncLock)
            {
                if (bindingElementExtensions.ContainsKey(name))
                {
                    bindingElementExtensions.RemoveAt(name);
                }
                _config.Save();
            }
        }

        public void UpdateBindingExtension(ExtensionElement extensionElement)
        {
            Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, "WcfMachineConfiguration:UpdateBindingExtension for " + extensionElement.Name);
            ServiceModelSectionGroup serviceModel = ServiceModelSectionGroup.GetSectionGroup(_config);
            ExtensionElementCollection bindingExtensions = serviceModel.Extensions.BindingExtensions;
            lock (_syncLock)
            {
                if (bindingExtensions.ContainsKey(extensionElement.Name))
                {
                    bindingExtensions.RemoveAt(extensionElement.Name);
                }
                bindingExtensions.Add(extensionElement);
                _config.Save();
            }
        }

        public void RemoveBindingExtension(string name)
        {
            Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, "WcfMachineConfiguration:RemoveBindingExtension for " + name);
            ServiceModelSectionGroup serviceModel = ServiceModelSectionGroup.GetSectionGroup(_config);
            ExtensionElementCollection bindingExtensions = serviceModel.Extensions.BindingExtensions;
            lock (_syncLock)
            {
                if (bindingExtensions.ContainsKey(name))
                {
                    bindingExtensions.Remove(bindingExtensions[name]);
                }
                _config.Save();
            }
        }

        public bool TryGetBindingElementExtension(string name, out ExtensionElement bindingElement)
        {
            Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, "WcfMachineConfiguration:TryGetBindingElementExtension for " + name);
            ServiceModelSectionGroup serviceModel = ServiceModelSectionGroup.GetSectionGroup(_config);
            ExtensionElementCollection elementExtensions = serviceModel.Extensions.BindingElementExtensions;
            lock (_syncLock)
            {
                if (elementExtensions.ContainsKey(name))
                {
                    bindingElement = elementExtensions[name];
                    return true;
                }
            }
            bindingElement = null;
            return false;
        }

        public bool TryGetBindingExtension(string name, out ExtensionElement element)
        {
            Trace.WriteLineIf(Tracing.LBSwitch.TraceVerbose, "WcfMachineConfiguration:TryGetBindingExtension for " + name);
            ServiceModelSectionGroup serviceModel = ServiceModelSectionGroup.GetSectionGroup(_config);
            ExtensionElementCollection bindingExtensions = serviceModel.Extensions.BindingExtensions;
            
            if (bindingExtensions.ContainsKey(name))
            {
                element = bindingExtensions[name];
                return true;
            }

            element = null;
            return false;
        }

        //the channel binding
        public IEnumerable<Type> BindingElementTypes()
        {
            ServiceModelSectionGroup serviceModel = ServiceModelSectionGroup.GetSectionGroup(_config);
            ExtensionElementCollection bindingElementExtensions = serviceModel.Extensions.BindingElementExtensions;

            foreach (ExtensionElement bindingElementExtension in bindingElementExtensions)
            {
                yield return Type.GetType(bindingElementExtension.Type);
            }
        }

    }
}