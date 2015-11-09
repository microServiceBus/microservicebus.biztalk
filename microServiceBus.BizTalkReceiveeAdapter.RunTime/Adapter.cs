using System;
using System.Diagnostics;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.TransportProxy.Interop;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microServiceBus.BizTalkReceiveeAdapter.RunTime
{
    public abstract class Adapter :
        IBTTransport,
        IBTTransportControl,
        IPersistPropertyBag
    {
        //  core member data
        private string propertyNamespace;
        private IBTTransportProxy transportProxy;
        private IPropertyBag handlerPropertyBag;
        private bool initialized;

        //  member data for implementing IBTTransport
        private string name;
        private string version;
        private string description;
        private string transportType;
        private Guid clsid;

        protected Adapter(
            string name,
            string version,
            string description,
            string transportType,
            Guid clsid,
            string propertyNamespace)
        {
            Trace.WriteLine(String.Format("Adapter.Adapter name: {0}", name));

            this.transportProxy = null;
            this.handlerPropertyBag = null;
            this.initialized = false;

            this.name = name;
            this.version = version;
            this.description = description;
            this.transportType = transportType;
            this.clsid = clsid;

            this.propertyNamespace = propertyNamespace;
        }

        protected string PropertyNamespace { get { return propertyNamespace; } }
        public IBTTransportProxy TransportProxy { get { return transportProxy; } }
        protected IPropertyBag HandlerPropertyBag { get { return handlerPropertyBag; } }
        protected bool Initialized { get { return initialized; } }

        //  IBTTransport
        public string Name { get { return name; } }
        public string Version { get { return version; } }
        public string Description { get { return description; } }
        public string TransportType { get { return transportType; } }
        public Guid ClassID { get { return clsid; } }

        //  IBTransportControl
        public virtual void Initialize(IBTTransportProxy transportProxy)
        {
            Trace.WriteLine("Adapter.Initialize");

            //  this is a Singleton and this should only ever be called once
            if (this.initialized)
                throw new AlreadyInitialized();

            this.transportProxy = transportProxy;
            this.initialized = true;
        }
        public virtual void Terminate()
        {
            Trace.WriteLine("Adapter.Terminate");

            if (!this.initialized)
                throw new NotInitialized();

            this.transportProxy = null;
        }

        protected virtual void HandlerPropertyBagLoaded()
        {
            // let any derived classes know the property bag has now been loaded
        }

        // IPersistPropertyBag
        public void GetClassID(out Guid classid) { classid = this.clsid; }
        public void InitNew() { }
        public void Load(IPropertyBag pb, int pErrorLog)
        {
            Trace.WriteLine("Adapter.Load");

            this.handlerPropertyBag = pb;
            HandlerPropertyBagLoaded();
        }
        public void Save(IPropertyBag pb, bool fClearDirty, bool fSaveAllProperties) { }
    }
}
