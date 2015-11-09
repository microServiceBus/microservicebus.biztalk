using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace microServiceBus.BizTalkReceiveeAdapter.Helper.Tools
{
    public class ConnectionStringHelper
    {
        
        private string bamConnectionString;

        public string BAMConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(bamConnectionString))
                {
                    bamConnectionString = GetBAMConnectionString();
                }

                return bamConnectionString;
            }
            private set { bamConnectionString = value; }
        }

        private string mgmtConnectionString;

        public string MgmtConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(mgmtConnectionString))
                {
                    mgmtConnectionString = GetMgmtConnectionString();
                }

                return mgmtConnectionString;
            }
            private set { mgmtConnectionString = value; }
        }

        public static string GetBAMConnectionString()
        {
            GroupSetting.GroupSettingCollection settings = GroupSetting.GetInstances();
            IEnumerator e = settings.GetEnumerator();

            e.MoveNext();

            GroupSetting gs = e.Current as GroupSetting;

            return string.Format("Integrated Security=SSPI;Data Source={0};Initial Catalog={1}", gs.BamDBServerName, gs.BamDBName);

        }

        public static string GetMgmtConnectionString()
        {
            GroupSetting.GroupSettingCollection settings = GroupSetting.GetInstances();
            IEnumerator e = settings.GetEnumerator();

            e.MoveNext();

            GroupSetting gs = e.Current as GroupSetting;
            
            return string.Format("Integrated Security=SSPI;Data Source={0};Initial Catalog={1}", gs.MgmtDbServerName, gs.MgmtDbName);

        }
    }
}