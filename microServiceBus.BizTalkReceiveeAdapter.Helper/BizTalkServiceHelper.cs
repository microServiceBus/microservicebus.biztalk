using Microsoft.BizTalk.ExplorerOM;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using microServiceBus.BizTalkReceiveeAdapter.Helper.Tools;
using System.Runtime.InteropServices;
using System.Reflection;
using microServiceBus.BizTalkReceiveeAdapter.RunTime;

namespace microServiceBus.BizTalkReceiveeAdapter.Helper
{
    public class BizTalkServiceHelper
    {
        #region Private members
        private const string ns = "https://microservicebus.com/biztalk/properties";
        private const string valiablesNs = "https://microservicebus.com/biztalk/properties/variables";
        private BizTalkMessaging _bizTalkMessaging;
        #endregion

        #region Test methods
        public void Test() {

            var explorer = new BtsCatalogExplorer();
            explorer.ConnectionString = ConnectionStringHelper.GetMgmtConnectionString();

            foreach (SendPort sp in explorer.SendPorts)
            {
                if (sp.Name == "zzzzzz")
                    Console.WriteLine(sp.Name);
            }

            createTransmitBindings("microservicebus.com", "mySendPort", "localhost", 3400, "application/xml");

        }
        public void InstallTest() {
            var payload = new
            {
                applicationName = "microservicebus.com",
                receivePort = "myReceivePort",
                receiveLocation = "myReceiveLocation",
                submitURI = @"microservicebus://onramp"
            };
            new BizTalkServiceHelper().InstallAdapter(payload);
        }
        public void UnInstallTest()
        {
            var payload = new { applicationName = "microServiceBus.com" };

            new BizTalkServiceHelper().UnInstallReceiveAdapter(payload);
        }
        #endregion

        #region Public methods
        [STAThread]
        public async Task<object> InstallAdapter(dynamic input)
        {
            try
            {
                if (!isAdapterInstalled())
                    installInternal(input);

                return null;
            }
            catch (Exception ex){
                return ex.Message;
            }
        }
        [STAThread]
        public async Task<object> ApplyReceiveBindings(dynamic input)
        {
            System.Diagnostics.Trace.WriteLine("ApplyTransmitBindings called", "microservicebus");
            try
            {
                createReceiveBindings((string)input.applicationName,
                    (string)input.receivePort,
                    (string)input.receiveLocation);
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        [STAThread]
        public async Task<object> ApplyTransmitBindings(dynamic input)
        {
            System.Diagnostics.Trace.WriteLine("ApplyTransmitBindings called", "microservicebus");
            try
            {
                createTransmitBindings((string)input.applicationName,
                    (string)input.sendPort,
                    (string)input.address,
                    (int)input.port,
                    (string)input.contentType);
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        [STAThread]
        public async Task<object> UnInstallReceiveAdapter(dynamic input)
        {
            uninstallInternal(input);
            return true;
        }
        [STAThread]
        public async Task<object> ProcessMessage(dynamic input)
        {
            try
            {
                dynamic message = input.message;
                dynamic context = input.context;
                var submitURI = string.Format("microservicebus://{0}", (string)input.location);
                // Create IBaseMessage messages
                if (_bizTalkMessaging ==null)
                    _bizTalkMessaging = new BizTalkMessaging();

                var btsMessage = _bizTalkMessaging.CreateMessageFromString(submitURI, (string)message);

                if (context.IntegrationId != null)
                    btsMessage.Context.Write("IntegrationId", ns, (string)context.IntegrationId);
                if (context.CorrelationId != null)
                    btsMessage.Context.Write("CorrelationId", ns, (string)context.CorrelationId);
                if (context.Created != null)
                    btsMessage.Context.Write("Created", ns, (string)context.Created);
                if (context.CreatedBy != null)
                    btsMessage.Context.Write("CreatedBy", ns, (string)context.CreatedBy);
                if (context.InterchangeId != null)
                    btsMessage.Context.Write("InterchangeId", ns, (string)context.InterchangeId);
                if (context.IsBinary != null)
                    btsMessage.Context.Write("IsBinary", ns, (bool)context.IsBinary);
                if (context.IsCorrelation != null)
                    btsMessage.Context.Write("IsCorrelation", ns, (bool)context.IsCorrelation);
                if (context.IsRequestResponse != null)
                    btsMessage.Context.Write("IsRequestResponse", ns, (bool)context.IsRequestResponse);
                if (context.ItineraryId != null)
                    btsMessage.Context.Write("ItineraryId", ns, (string)context.ItineraryId);
                if (context.Length != null)
                    btsMessage.Context.Write("Length", ns, (int)context.Length);
                if (context.OrganizationId != null)
                    btsMessage.Context.Write("OrganizationId", ns, (string)context.OrganizationId);
                if (context.Sender != null)
                    btsMessage.Context.Write("Sender", ns, (string)context.Sender);

                foreach (var variable in context.Variables)
                {
                    btsMessage.Context.Write(variable.Variable, valiablesNs, variable.Value);
                }

                // Submit the messages
                var result = _bizTalkMessaging.SubmitMessage(btsMessage);
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        #endregion

        #region Private methods
        public bool isAdapterInstalled()
        {
            var explorer = new BtsCatalogExplorer();

            try
            {
                bool installed = false;
                explorer.ConnectionString = ConnectionStringHelper.GetMgmtConnectionString();
                foreach (ProtocolType p in explorer.ProtocolTypes)
                {
                    installed = p.Name == "microServiceBus";
                    if (installed)
                        break;
                }

                return installed;
            }
            catch { return false; }

        }
        private void installInternal(dynamic input)
        {

            //Uninstall();

            RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\BizTalk Server\3.0");
            if (null == rk)
            {
                throw new Exception("Microsoft BizTalk Server is not installed on this machine.");
            }

            // Get install directory path
            string InstallPath = Directory.GetCurrentDirectory();
            //string InstallPath = Path.GetDirectoryName(location);
            string InboundAssemblyPath = Path.Combine(InstallPath, @"microServiceBus.BizTalk\microServiceBus.BizTalkReceiveeAdapter.RunTime.dll");
            string AdapterMgmtAssemblyPath = Path.Combine(InstallPath, @"microServiceBus.BizTalk\microServiceBus.BizTalkReceiveAdapter.Management.dll");

            // Check if the adapter has been already registered
            var baseKey = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.ClassesRoot, RegistryView.Registry32);
            rk = baseKey.OpenSubKey(@"CLSID\{132018E7-EA66-4A57-8D40-855F285694DC}");

            if (null == rk)
            {
                // Console.WriteLine("Updating the registry...");
                //rk = Registry.ClassesRoot.OpenSubKey("CLSID", true);
                rk = baseKey.OpenSubKey(@"CLSID", true);
                rk = rk.CreateSubKey("{132018E7-EA66-4A57-8D40-855F285694DC}");
                rk.SetValue("", "microServiceBus adapter");
                rk.SetValue("AppID", "{4BD57A60-161E-4F7F-B01A-B638ED970B6B}");

                RegistryKey rkBizTalk = rk.CreateSubKey("BizTalk");
                rk = rk.CreateSubKey("Implemented Categories");
                rk.CreateSubKey("{7F46FC3E-3C2C-405B-A47F-8D17942BA8F9}");

                rkBizTalk.SetValue("", "BizTalk");

                rkBizTalk.SetValue("TransportType", "microServiceBus");
                rkBizTalk.SetValue("Constraints", 0x00003C03);

                rkBizTalk.SetValue("InboundProtocol_PageProv", "{2DE93EE6-CB01-4007-93E9-C3D71689A281}");
                rkBizTalk.SetValue("ReceiveLocation_PageProv", "{2DE93EE6-CB01-4007-93E9-C3D71689A280}");

                rkBizTalk.SetValue("InboundEngineCLSID", "{11E62A28-D6C8-41ED-AF7F-369EFBFDA2BE}");
                rkBizTalk.SetValue("InboundTypeName", "microServiceBus.BizTalkReceiveeAdapter.RunTime.microServiceBusTransmitAdapter");
                rkBizTalk.SetValue("InboundAssemblyPath", InboundAssemblyPath);

                rkBizTalk.SetValue("OutboundProtocol_PageProv", "{2DE93EE6-CB01-4007-93E9-C3D71689A283}");
                rkBizTalk.SetValue("TransmitLocation_PageProv", "{2DE93EE6-CB01-4007-93E9-C3D71689A282}");
                rkBizTalk.SetValue("OutboundEngineCLSID", "{11E62A28-D6C8-41ED-AF7F-369EFBFDA2BE}");
                rkBizTalk.SetValue("OutboundTypeName", "microServiceBus.BizTalkReceiveeAdapter.RunTime.microServiceBusTransmitAdapter");
                rkBizTalk.SetValue("OutboundAssemblyPath", InboundAssemblyPath);

                rkBizTalk.SetValue("PropertyNameSpace", "http://microservicebus.biztalk");

                rkBizTalk.SetValue("AdapterMgmtTypeName", "microServiceBus.BizTalkReceiveAdapter.Management.AdapterManagement");
                rkBizTalk.SetValue("AdapterMgmtAssemblyPath", AdapterMgmtAssemblyPath);
                rkBizTalk.SetValue("AliasesXML", "<AdapterAliasList><AdapterAlias>microservicebus://</AdapterAlias></AdapterAliasList>");
                rkBizTalk.SetValue("SendHandlerPropertiesXML", "<CustomProps><AdapterConfig vt=\"8\"/></CustomProps>");
                rkBizTalk.SetValue("ReceiveLocationPropertiesXML", "<CustomProps><AdapterConfig vt=\"8\"/></CustomProps>");
                rkBizTalk.SetValue("SendLocationPropertiesXML", "<CustomProps><AdapterConfig vt=\"8\"/></CustomProps>");
                rkBizTalk.SetValue("ReceiveHandlerPropertiesXML", "<CustomProps><AdapterConfig vt=\"8\"/></CustomProps>");
            }
            else
            {
                // Console.WriteLine("Submit adapter has been already registered.");
            }

            // Add the adapter to the server.
            // Console.WriteLine("Adding the adapter to the server...");
            ManagementObject objInstance = null;

            try
            {
                var objClass = new ManagementClass(
                    @"root\MicrosoftBizTalkServer",
                    "MSBTS_AdapterSetting",
                    new ObjectGetOptions());

                objInstance = objClass.CreateInstance();

                objInstance["Name"] = "microServiceBus";
                objInstance["MgmtCLSID"] = "{132018E7-EA66-4A57-8D40-855F285694DC}";
                objInstance["Comment"] = "Programmatic submission adapter";

                objInstance.Put();
                createAdapterHandler("microServiceBus");
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to add adapter to the server, reason: {0}", e.Message);
            }
        }
        private void uninstallInternal(dynamic input)
        {
            // Check if the BizTalk Server is present on the machine
            RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\BizTalk Server\3.0");
            if (null == rk)
            {
                // Console.WriteLine("Microsoft BizTalk Server is not installed on this machine.");
                return;
            }

            removeBindings((string)input.applicationName);
            // Check if the adapter has been already registered
            var baseKey = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.ClassesRoot, RegistryView.Registry32);
            rk = baseKey.OpenSubKey(@"CLSID\{132018E7-EA66-4A57-8D40-855F285694DC}");

            //rk = Registry.ClassesRoot.OpenSubKey(@"CLSID\{132018E7-EA66-4A57-8D40-855F285694DC}");
            if (null != rk)
            {
                // Console.WriteLine("Removing the registry entery...");
                rk = Registry.ClassesRoot.OpenSubKey("CLSID", true);
                rk.DeleteSubKeyTree("{132018E7-EA66-4A57-8D40-855F285694DC}");
                // Console.WriteLine("Registry entery removed.");
            }
            else
            {
                // Console.WriteLine("Submit adapter is not registered.");
            }

            // Removing the adapter from the server.
            // Console.WriteLine("Removing the adapter from the server...");

            try
            {
                ManagementClass objClass = new ManagementClass(
                    @"root\MicrosoftBizTalkServer",
                    "MSBTS_AdapterSetting",
                    new ObjectGetOptions());

                foreach (ManagementObject objInstance in objClass.GetInstances())
                {
                    if ("microServiceBus" == objInstance["Name"].ToString())
                    {
                        objInstance.Delete();
                        break;
                    }
                }

                // Console.WriteLine("Submit adapter removed from server.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to remove adapter from the server, reason: {0}", e.Message);
            }
        }
        private static void createAdapterHandler(string adapterName)
        {
            string hostName = null;
            try
            {
                PutOptions options = new PutOptions();
                options.Type = PutType.CreateOnly;

                //create a ManagementClass object and spawn a ManagementObject instance
                ManagementClass objReceiveHandlerClass = new ManagementClass("root\\MicrosoftBizTalkServer", "MSBTS_ReceiveHandler", null);
                ManagementObject objReceiveHandler = objReceiveHandlerClass.CreateInstance();

                //set the properties for the Managementobject
                objReceiveHandler["AdapterName"] = adapterName;

                ManagementClass hostClass = new ManagementClass(
                   @"root\MicrosoftBizTalkServer",
                   "MSBTS_HostSetting",
                   new ObjectGetOptions());

                foreach (ManagementObject objInstance in hostClass.GetInstances())
                {
                    if ("2" == objInstance["HostType"].ToString())// "2" indicates isolated type
                    {
                        objReceiveHandler["HostName"] = objInstance["Name"];
                        break;
                    }
                }

                objReceiveHandler["CustomCfg"] = "<CustomProps><AdapterConfig vt=\"8\"/></CustomProps>";

                //create the Managementobject
                objReceiveHandler.Put(options);
                // Console.WriteLine("ReceiveHandler - " + adapterName + " " + hostName + " - has been created successfully");
            }
            catch (Exception excep)
            {
                // Console.WriteLine("CreateReceiveHandler - " + adapterName + " " + hostName + " - failed: " + excep.Message);
            }
        }
        private static void createReceiveBindings(string applicationName, 
            string receivePortName, 
            string receiveLocationName)
        {
            var explorer = new BtsCatalogExplorer();

            try
            {
                explorer.ConnectionString = ConnectionStringHelper.GetMgmtConnectionString();

                var application = explorer.Applications[applicationName];
                if (application == null)
                {
                    application = explorer.AddNewApplication();
                    application.Name = applicationName;
                    explorer.SaveChanges();
                }
                System.Diagnostics.Trace.WriteLine("application done", "microservicebus");

                var receivePort = application.ReceivePorts[receivePortName];
                if (receivePort == null)
                {
                    receivePort = application.AddNewReceivePort(false);
                    receivePort.Name = receivePortName;
                    explorer.SaveChanges();
                }
                System.Diagnostics.Trace.WriteLine("receive port done", "microservicebus");
                var receiveLocation = receivePort.ReceiveLocations[receiveLocationName];
                if (receiveLocation == null)
                {
                    var submitURI = string.Format("microservicebus://{0}", receiveLocationName);
                    var pipeline = explorer.Pipelines["Microsoft.BizTalk.DefaultPipelines.PassThruReceive"];
                    var transport = explorer.ProtocolTypes["microServiceBus"];
                    receiveLocation = receivePort.AddNewReceiveLocation();
                    receiveLocation.Address = submitURI;
                    receiveLocation.Description = "Automaticly created by the microServiceBus.com host.";
                    receiveLocation.Name = receiveLocationName;
                    receiveLocation.ReceivePipeline = pipeline;
                    receiveLocation.TransportType = transport;
                    receiveLocation.TransportTypeData = @"<CustomProps><AdapterConfig vt=""8"">&lt;CustomProps xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""&gt;&lt;uri&gt;"+submitURI+@"&lt;/uri&gt;&lt;/CustomProps&gt;</AdapterConfig></CustomProps>";
                    foreach (ReceiveHandler receiveHandler in explorer.ReceiveHandlers)
                    {
                        if (receiveHandler.Name == "BizTalkServerIsolatedHost" &&
                            receiveHandler.TransportType.Name == "microServiceBus")
                        {
                            receiveLocation.ReceiveHandler = receiveHandler;
                            break;
                        }

                    }
                    receiveLocation.Enable = true;
                    explorer.SaveChanges();
                    System.Diagnostics.Trace.WriteLine("all done", "microservicebus");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void createTransmitBindings(string applicationName,
            string sendPortName,
            string address,
            int port,
            string contentType)
        {
            var explorer = new BtsCatalogExplorer();

            try
            {
                explorer.ConnectionString = ConnectionStringHelper.GetMgmtConnectionString();

                var application = explorer.Applications[applicationName];
                if (application == null)
                {
                    application = explorer.AddNewApplication();
                    application.Name = applicationName;
                    explorer.SaveChanges();
                }


                var sendPort = application.SendPorts[sendPortName];
                if (sendPort == null)
                {
                    var uri = string.Format("microservicebus://{0}:{1}", address, port);
                    var pipeline = explorer.Pipelines["Microsoft.BizTalk.DefaultPipelines.PassThruTransmit"];
                    sendPort = application.AddNewSendPort(false, false);
                    sendPort.Name = sendPortName;
                    sendPort.SendPipeline = pipeline;
                    
                    var transport = explorer.ProtocolTypes["microServiceBus"];
                    sendPort.Description = "Automaticly created by the microServiceBus.com host.";
                    sendPort.PrimaryTransport.Address = uri;
                    sendPort.PrimaryTransport.TransportType = transport;
                    sendPort.PrimaryTransport.TransportTypeData = @"<CustomProps><AdapterConfig vt=""8"">&lt;CustomProps xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""&gt;&lt;uri&gt;" + uri + @"&lt;/uri&gt;&lt;address&gt;" + address + @"&lt;/address&gt;&lt;port&gt;" + port.ToString() + @"&lt;/port&gt;&lt;contentType&gt;"+ contentType + @"&lt;/contentType&gt;&lt;/CustomProps&gt;</AdapterConfig></CustomProps>";
                    foreach (SendHandler sendHandler in explorer.SendHandlers)
                    {
                        if (sendHandler.TransportType.Name == "microServiceBus")
                        {
                            sendPort.PrimaryTransport.SendHandler = sendHandler;
                            break;
                        }

                    }
                    sendPort.Status = PortStatus.Started;
                    explorer.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static void removeBindings(string applicationName)
        {
            var explorer = new BtsCatalogExplorer();

            try
            {
                explorer.ConnectionString = ConnectionStringHelper.GetMgmtConnectionString();

                var application = explorer.Applications[applicationName];
                if (application != null)
                {
                    application.Stop(ApplicationStopOption.StopAll);
                    explorer.SaveChanges();
                    foreach (ReceivePort receivePort in application.ReceivePorts)
                    {
                        foreach (ReceiveLocation location in receivePort.ReceiveLocations)
                        {
                            location.Enable = false;
                        }
                        explorer.RemoveReceivePort(receivePort);
                    }


                    explorer.RemoveApplication(application);
                    explorer.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to apply bindings.");
            }
        }
        #endregion
    }
}
