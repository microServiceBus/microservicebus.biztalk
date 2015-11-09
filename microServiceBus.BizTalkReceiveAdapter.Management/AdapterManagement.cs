using System;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Adapter.Framework;

namespace microServiceBus.BizTalkReceiveAdapter.Management
{
    class AdapterManagement : IAdapterConfig, IStaticAdapterConfig
    {
        public string GetConfigSchema(ConfigType type)
        {
            switch (type)
            {
                case ConfigType.ReceiveHandler:
                case ConfigType.ReceiveLocation:

                    string receiveConfig = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>"
                                    + "<xs:schema targetNamespace=\"http://microservicebus.biztalk\""
                                    + "           elementFormDefault=\"qualified\""
                                    + "           xmlns=\"http://microservicebus.biztalk\""
                                    + "           xmlns:mstns=\"http://tempuri.org/XMLSchema.xsd\""
                                    + "           xmlns:xs=\"http://www.w3.org/2001/XMLSchema\">"
                                    + "    <xs:element name=\"CustomProps\">"
                                    + "        <xs:complexType>"
                                    + "            <xs:sequence>"
                                    + "                <xs:element name=\"uri\" type=\"xs:string\" />"
                                    + "            </xs:sequence>"
                                    + "        </xs:complexType>"
                                    + "    </xs:element>"
                                    + "</xs:schema>";
                    return receiveConfig;
                case ConfigType.TransmitHandler:
                case ConfigType.TransmitLocation:
                    string transmitConfig = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>"
                                    + "<xs:schema targetNamespace=\"http://microservicebus.biztalk\""
                                    + "           elementFormDefault=\"qualified\""
                                    + "           xmlns=\"http://microservicebus.biztalk\""
                                    + "           xmlns:mstns=\"http://tempuri.org/XMLSchema.xsd\""
                                    + "           xmlns:xs=\"http://www.w3.org/2001/XMLSchema\">"
                                    + "    <xs:element name=\"CustomProps\">"
                                    + "        <xs:complexType>"
                                    + "            <xs:sequence>"
                                    + "                <xs:element name=\"uri\" type=\"xs:string\" />"
                                    + "                <xs:element name=\"address\" type=\"xs:string\" />"
                                    + "                <xs:element name=\"port\" type=\"xs:integer\" />"
                                    + "                <xs:element name=\"contentType\" type=\"xs:string\" />"
                                    + "            </xs:sequence>"
                                    + "        </xs:complexType>"
                                    + "    </xs:element>"
                                    + "</xs:schema>";
                    return transmitConfig;
                default:
                    return null;
            }
        }
        public string[] GetServiceDescription(string[] wsdls)
        {
            return null;
        }

        public string GetServiceOrganization(IPropertyBag endPointConfiguration, string node)
        {
            return null;
        }


        public Result GetSchema(string xsdLocation, string xsdNamespace, out string XSDFileName)
        {
            XSDFileName = null;
            return Result.Continue;
        }
    }
}
