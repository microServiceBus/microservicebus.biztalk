using System;
using System.Xml;
using Microsoft.BizTalk.Component.Interop;

namespace microServiceBus.BizTalkReceiveeAdapter.RunTime
{
    public class ConfigProperties
    {
        // Various useful helper functions
        public static XmlDocument ExtractConfigDomImpl(IPropertyBag pConfig, bool required)
        {
            object obj = null;
            pConfig.Read("AdapterConfig", out obj, 0);
            if (!required && null == obj)
                return null;
            if (null == obj)
                throw new NoAdapterConfig();

            XmlDocument configDom = new XmlDocument();

            string adapterConfig = (string)obj;
            configDom.LoadXml(adapterConfig);

            return configDom;
        }

        public static XmlDocument ExtractConfigDom(IPropertyBag pConfig)
        {
            return ExtractConfigDomImpl(pConfig, true);
        }

        public static XmlDocument IfExistsExtractConfigDom(IPropertyBag pConfig)
        {
            return ExtractConfigDomImpl(pConfig, false);
        }

        public static string ExtractImpl(XmlDocument document, string path, bool required, string alt)
        {
            XmlNode node = document.SelectSingleNode(path);
            if (!required && null == node)
                return alt;
            if (null == node)
                throw new NoSuchProperty(path);
            return node.InnerText;
        }

        public static string IfNotEmptyExtract(XmlDocument document, string path, bool required, string alt)
        {
            XmlNode node = document.SelectSingleNode(path);
            if (!required && (null == node || 0 == node.InnerText.Length))
                return alt;
            if (null == node)
                throw new NoSuchProperty(path);
            return node.InnerText;
        }

        public static string Extract(XmlDocument document, string path, string alt)
        {
            return ExtractImpl(document, path, true, alt);
        }

        public static string IfExistsExtract(XmlDocument document, string path, string alt)
        {
            return ExtractImpl(document, path, false, alt);
        }

        public static int ExtractInt(XmlDocument document, string path)
        {
            string s = Extract(document, path, String.Empty);
            return int.Parse(s);
        }

        public static int IfExistsExtractInt(XmlDocument document, string path, int alt)
        {
            string s = IfExistsExtract(document, path, String.Empty);
            if (0 == s.Length)
                return alt;
            return int.Parse(s);
        }

        public static long ExtractLong(XmlDocument document, string path)
        {
            string s = Extract(document, path, String.Empty);
            return long.Parse(s);
        }

        public static long IfExistsExtractLong(XmlDocument document, string path, long alt)
        {
            string s = IfExistsExtract(document, path, String.Empty);
            if (0 == s.Length)
                return alt;
            return long.Parse(s);
        }

        public static bool ExtractBool(XmlDocument document, string path)
        {
            string s = Extract(document, path, String.Empty);
            return Boolean.Parse(s);
        }

        public static bool IfExistsExtractBool(XmlDocument document, string path, bool alt)
        {
            string s = IfExistsExtract(document, path, String.Empty);
            if (0 == s.Length)
                return alt;
            return Boolean.Parse(s);
        }
        
    }
}
