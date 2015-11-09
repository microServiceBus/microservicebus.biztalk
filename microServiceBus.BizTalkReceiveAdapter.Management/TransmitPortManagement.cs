using System;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Adapter.Framework;

namespace microServiceBus.BizTalkReceiveAdapter.Management
{

    public class TransmitPortManagement : IAdapterConfig,
                                    IStaticAdapterConfig
    {
        public string GetConfigSchema(ConfigType type)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>"
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

            if (type == 0) return result;

            return null;
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