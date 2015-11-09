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

namespace microServiceBus.BizTalkReceiveeAdapter.Helper.Tools
{
    public class RegisterAdapter
    {
        [STAThread]
        public static void Install(string applicationName, string receivePort, string receiveLocation, string submitURI)
        {
            //Uninstall();
            // Check if the BizTalk Server is present on the machine
            RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\BizTalk Server\3.0");
            if (null == rk)
            {
                throw new Exception("Microsoft BizTalk Server is not installed on this machine.");
            }

            // Get install directory path
            string InstallPath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            string InboundAssemblyPath = Path.Combine(InstallPath, "microServiceBus.BizTalkReceiveeAdapter.RunTime.dll");
            string AdapterMgmtAssemblyPath = Path.Combine(InstallPath, "microServiceBus.BizTalkReceiveAdapter.Management.dll");

            // Check if the adapter has been already registered
            rk = Registry.ClassesRoot.OpenSubKey(@"CLSID\{0DB0FB02-4F7F-4B08-BAE4-BDC3C301EBF4}");
            if (null == rk)
            {
                // Console.WriteLine("Updating the registry...");
                rk = Registry.ClassesRoot.OpenSubKey("CLSID", true);
                rk = rk.CreateSubKey("{0DB0FB02-4F7F-4B08-BAE4-BDC3C301EBF4}");
                rk.SetValue("", "microServiceBus Adapter Class");
                rk.SetValue("AppID", "{4BD57A60-161E-4F7F-B01A-B638ED970B6B}");

                RegistryKey rkBizTalk = rk.CreateSubKey("BizTalk");
                rk = rk.CreateSubKey("Implemented Categories");
                rk.CreateSubKey("{7F46FC3E-3C2C-405B-A47F-8D17942BA8F9}");

                rkBizTalk.SetValue("", "BizTalk");

                rkBizTalk.SetValue("TransportType", "microServiceBus");
                rkBizTalk.SetValue("Constraints", 0x00000081);
                rkBizTalk.SetValue("ReceiveLocation_PageProv", "{2DE93EE6-CB01-4007-93E9-C3D71689A280}");
                rkBizTalk.SetValue("InboundEngineCLSID", "{11E62A28-D6C8-41ed-AF7F-369EFBFDA2BE}");
                rkBizTalk.SetValue("InboundTypeName", "microServiceBus.BizTalkReceiveeAdapter.RunTime.BizTalkMessaging");
                rkBizTalk.SetValue("InboundAssemblyPath", InboundAssemblyPath);

                rkBizTalk.SetValue("PropertyNameSpace", "http://schemas.microsoft.com/BizTalk/2003/SDK_Samples/Messaging/Transports/submit-properties");

                rkBizTalk.SetValue("AdapterMgmtTypeName", "microServiceBus.BizTalkReceiveAdapter.Management.TPUtilsManagement");
                rkBizTalk.SetValue("AdapterMgmtAssemblyPath", AdapterMgmtAssemblyPath);
                rkBizTalk.SetValue("AliasesXML", "<AdapterAliasList><AdapterAlias>microservicebus://</AdapterAlias></AdapterAliasList>");
                rkBizTalk.SetValue("ReceiveLocationPropertiesXML", "<CustomProps><AdapterConfig vt=\"8\"/></CustomProps>");
                rkBizTalk.SetValue("ReceiveHandlerPropertiesXML", "<CustomProps><AdapterConfig vt=\"8\"/></CustomProps>");

                // Console.WriteLine("Registry has been updated.");

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
                ManagementClass objClass = new ManagementClass(
                    @"root\MicrosoftBizTalkServer",
                    "MSBTS_AdapterSetting",
                    new ObjectGetOptions());

                objInstance = objClass.CreateInstance();

                objInstance["Name"] = "microServiceBus";
                objInstance["MgmtCLSID"] = "{0DB0FB02-4F7F-4B08-BAE4-BDC3C301EBF4}";
                objInstance["Comment"] = "Programmatic submission adapter";

                objInstance.Put();
                CreateAdapterHandler("microServiceBus");
                // Console.WriteLine("Adapter has been added.");

                CreateBindings(applicationName, receivePort, receiveLocation, submitURI);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to add adapter to the server, reason: {0}", e.Message);
            }
        }

        static void CreateAdapterHandler(string adapterName)
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

        public static void CreateBindings(string applicationName, string receivePortName, string receiveLocationName, string submitURI)
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

                var receivePort = application.ReceivePorts[receivePortName];
                if (receivePort == null)
                {
                    receivePort = application.AddNewReceivePort(false);
                    receivePort.Name = receivePortName;
                    explorer.SaveChanges();
                }

                var receiveLocation = receivePort.ReceiveLocations[receiveLocationName];
                if (receiveLocation == null)
                {
                    var pipeline = explorer.Pipelines["Microsoft.BizTalk.DefaultPipelines.PassThruReceive"];
                    var transport = explorer.ProtocolTypes["microServiceBus"];
                    receiveLocation = receivePort.AddNewReceiveLocation();
                    receiveLocation.Address = submitURI;
                    receiveLocation.Description = "Automaticly created by the microServiceBus.com host.";
                    receiveLocation.Name = receiveLocationName;
                    receiveLocation.ReceivePipeline = pipeline;
                    receiveLocation.TransportType = transport;
                    receiveLocation.TransportTypeData = @"<CustomProps><AdapterConfig vt=""8"">&lt;CustomProps xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""&gt;&lt;uri&gt;microservicebus://onramp&lt;/uri&gt;&lt;/CustomProps&gt;</AdapterConfig></CustomProps>";
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
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to apply bindings.");
            }
        }

        static void Uninstall()
        {
            // Check if the BizTalk Server is present on the machine
            RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\BizTalk Server\3.0");
            if (null == rk)
            {
                // Console.WriteLine("Microsoft BizTalk Server is not installed on this machine.");
                return;
            }

            // Check if the adapter has been already registered
            rk = Registry.ClassesRoot.OpenSubKey(@"CLSID\{0DB0FB02-4F7F-4B08-BAE4-BDC3C301EBF4}");
            if (null != rk)
            {
                // Console.WriteLine("Removing the registry entery...");
                rk = Registry.ClassesRoot.OpenSubKey("CLSID", true);
                rk.DeleteSubKeyTree("{0DB0FB02-4F7F-4B08-BAE4-BDC3C301EBF4}");
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
    }
}
