namespace microServiceBus.BizTalkReceiveeAdapter.Helper.Tools
{
    using System;
    using System.ComponentModel;
    using System.Management;
    using System.Collections;
    using System.Globalization;
    using System.ComponentModel.Design.Serialization;
    using System.Reflection;
    
    
    // Functions ShouldSerialize<PropertyName> are functions used by VS property browser to check if a particular property has to be serialized. These functions are added for all ValueType properties ( properties of type Int32, BOOL etc.. which cannot be set to null). These functions use Is<PropertyName>Null function. These functions are also used in the TypeConverter implementation for the properties to check for NULL value of property so that an empty value can be shown in Property browser in case of Drag and Drop in Visual studio.
    // Functions Is<PropertyName>Null() are used to check if a property is NULL.
    // Functions Reset<PropertyName> are added for Nullable Read/Write properties. These functions are used by VS designer in property browser to set a property to NULL.
    // Every property added to the class for WMI property has attributes set to define its behavior in Visual Studio designer and also to define a TypeConverter to be used.
    // An Early Bound class generated for the WMI class.MSBTS_GroupSetting
    public class GroupSetting : System.ComponentModel.Component {
        
        // Private property to hold the WMI namespace in which the class resides.
        private static string CreatedWmiNamespace = "ROOT\\MicrosoftBizTalkServer";
        
        // Private property to hold the name of WMI class which created this class.
        private static string CreatedClassName = "MSBTS_GroupSetting";
        
        // Private member variable to hold the ManagementScope which is used by the various methods.
        private static System.Management.ManagementScope statMgmtScope = null;
        
        private ManagementSystemProperties PrivateSystemProperties;
        
        // Underlying lateBound WMI object.
        private System.Management.ManagementObject PrivateLateBoundObject;
        
        // Member variable to store the 'automatic commit' behavior for the class.
        private bool AutoCommitProp;
        
        // Private variable to hold the embedded property representing the instance.
        private System.Management.ManagementBaseObject embeddedObj;
        
        // The current WMI object used
        private System.Management.ManagementBaseObject curObj;
        
        // Flag to indicate if the instance is an embedded object.
        private bool isEmbedded;
        
        // Below are different overloads of constructors to initialize an instance of the class with a WMI object.
        public GroupSetting() {
            this.InitializeObject(null, null, null);
        }
        
        public GroupSetting(string keyMgmtDbName, string keyMgmtDbServerName) {
            this.InitializeObject(null, new System.Management.ManagementPath(GroupSetting.ConstructPath(keyMgmtDbName, keyMgmtDbServerName)), null);
        }
        
        public GroupSetting(System.Management.ManagementScope mgmtScope, string keyMgmtDbName, string keyMgmtDbServerName) {
            this.InitializeObject(((System.Management.ManagementScope)(mgmtScope)), new System.Management.ManagementPath(GroupSetting.ConstructPath(keyMgmtDbName, keyMgmtDbServerName)), null);
        }
        
        public GroupSetting(System.Management.ManagementPath path, System.Management.ObjectGetOptions getOptions) {
            this.InitializeObject(null, path, getOptions);
        }
        
        public GroupSetting(System.Management.ManagementScope mgmtScope, System.Management.ManagementPath path) {
            this.InitializeObject(mgmtScope, path, null);
        }
        
        public GroupSetting(System.Management.ManagementPath path) {
            this.InitializeObject(null, path, null);
        }
        
        public GroupSetting(System.Management.ManagementScope mgmtScope, System.Management.ManagementPath path, System.Management.ObjectGetOptions getOptions) {
            this.InitializeObject(mgmtScope, path, getOptions);
        }
        
        public GroupSetting(System.Management.ManagementObject theObject) {
            Initialize();
            if ((CheckIfProperClass(theObject) == true)) {
                PrivateLateBoundObject = theObject;
                PrivateSystemProperties = new ManagementSystemProperties(PrivateLateBoundObject);
                curObj = PrivateLateBoundObject;
            }
            else {
                throw new System.ArgumentException("Class name does not match.");
            }
        }
        
        public GroupSetting(System.Management.ManagementBaseObject theObject) {
            Initialize();
            if ((CheckIfProperClass(theObject) == true)) {
                embeddedObj = theObject;
                PrivateSystemProperties = new ManagementSystemProperties(theObject);
                curObj = embeddedObj;
                isEmbedded = true;
            }
            else {
                throw new System.ArgumentException("Class name does not match.");
            }
        }
        
        // Property returns the namespace of the WMI class.
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string OriginatingNamespace {
            get {
                return "ROOT\\MicrosoftBizTalkServer";
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ManagementClassName {
            get {
                string strRet = CreatedClassName;
                if ((curObj != null)) {
                    if ((curObj.ClassPath != null)) {
                        strRet = ((string)(curObj["__CLASS"]));
                        if (((strRet == null) 
                                    || (strRet == string.Empty))) {
                            strRet = CreatedClassName;
                        }
                    }
                }
                return strRet;
            }
        }
        
        // Property pointing to an embedded object to get System properties of the WMI object.
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ManagementSystemProperties SystemProperties {
            get {
                return PrivateSystemProperties;
            }
        }
        
        // Property returning the underlying lateBound object.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Management.ManagementBaseObject LateBoundObject {
            get {
                return curObj;
            }
        }
        
        // ManagementScope of the object.
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Management.ManagementScope Scope {
            get {
                if ((isEmbedded == false)) {
                    return PrivateLateBoundObject.Scope;
                }
                else {
                    return null;
                }
            }
            set {
                if ((isEmbedded == false)) {
                    PrivateLateBoundObject.Scope = value;
                }
            }
        }
        
        // Property to show the commit behavior for the WMI object. If true, WMI object will be automatically saved after each property modification.(ie. Put() is called after modification of a property).
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool AutoCommit {
            get {
                return AutoCommitProp;
            }
            set {
                AutoCommitProp = value;
            }
        }
        
        // The ManagementPath of the underlying WMI object.
        [Browsable(true)]
        public System.Management.ManagementPath Path {
            get {
                if ((isEmbedded == false)) {
                    return PrivateLateBoundObject.Path;
                }
                else {
                    return null;
                }
            }
            set {
                if ((isEmbedded == false)) {
                    if ((CheckIfProperClass(null, value, null) != true)) {
                        throw new System.ArgumentException("Class name does not match.");
                    }
                    PrivateLateBoundObject.Path = value;
                }
            }
        }
        
        // Public static scope property which is used by the various methods.
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static System.Management.ManagementScope StaticScope {
            get {
                return statMgmtScope;
            }
            set {
                statMgmtScope = value;
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Name of the BAM SQL database. Max length for this property is 123 characters.")]
        public string BamDBName {
            get {
                return ((string)(curObj["BamDBName"]));
            }
            set {
                curObj["BamDBName"] = value;
                if (((isEmbedded == false) 
                            && (AutoCommitProp == true))) {
                    PrivateLateBoundObject.Put();
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Name of the SQL server where BAM database is located. Max length for this propert" +
            "y is 80 characters.")]
        public string BamDBServerName {
            get {
                return ((string)(curObj["BamDBServerName"]));
            }
            set {
                curObj["BamDBServerName"] = value;
                if (((isEmbedded == false) 
                            && (AutoCommitProp == true))) {
                    PrivateLateBoundObject.Put();
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"This property is the name of the BizTalk Administrator Windows Group.  Members of this Windows Group are granted permission to access most of the BizTalk Administration functions. Certain functions require additional Windows or SQL privileges; refer to BizTalk Server 2010 Help for more details. This property is required for instance creation. Max length for this property is 128 characters.")]
        public string BizTalkAdministratorGroup {
            get {
                return ((string)(curObj["BizTalkAdministratorGroup"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"This property is the name of the BizTalk B2B Operators Windows Group.  Members of this Windows Group are granted permission to access BizTalk party/partners related functions. Certain functions require additional Windows or SQL privileges; refer to BizTalk Server 2010 Help for more details. Max length for this property is 128 characters.")]
        public string BizTalkB2BOperatorGroup {
            get {
                return ((string)(curObj["BizTalkB2BOperatorGroup"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"This property is the name of the BizTalk Operators Windows Group.  Members of this Windows Group are granted permission to access BizTalk Monitoring functions. Certain functions require additional Windows or SQL privileges; refer to BizTalk Server 2010 Help for more details. Max length for this property is 128 characters.")]
        public string BizTalkOperatorGroup {
            get {
                return ((string)(curObj["BizTalkOperatorGroup"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("A short description (one-line string) of the CIM_Setting object.")]
        public string Caption {
            get {
                return ((string)(curObj["Caption"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsConfigurationCacheRefreshIntervalNull {
            get {
                if ((curObj["ConfigurationCacheRefreshInterval"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("This property indicates how often the server refreshes the cache of the BizTalk M" +
            "essaging Configuration objects (in seconds). Max value for this property is 4320" +
            "0 and mininum value is 1.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint ConfigurationCacheRefreshInterval {
            get {
                if ((curObj["ConfigurationCacheRefreshInterval"] == null)) {
                    return System.Convert.ToUInt32(0);
                }
                return ((uint)(curObj["ConfigurationCacheRefreshInterval"]));
            }
            set {
                curObj["ConfigurationCacheRefreshInterval"] = value;
                if (((isEmbedded == false) 
                            && (AutoCommitProp == true))) {
                    PrivateLateBoundObject.Put();
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("A description of the CIM_Setting object.")]
        public string Description {
            get {
                return ((string)(curObj["Description"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsGlobalTrackingOptionNull {
            get {
                if ((curObj["GlobalTrackingOption"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("This property indicates the level of tracking which BizTalk server should perform" +
            ".")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public GlobalTrackingOptionValues GlobalTrackingOption {
            get {
                if ((curObj["GlobalTrackingOption"] == null)) {
                    return ((GlobalTrackingOptionValues)(System.Convert.ToInt32(2)));
                }
                return ((GlobalTrackingOptionValues)(System.Convert.ToInt32(curObj["GlobalTrackingOption"])));
            }
            set {
                if ((GlobalTrackingOptionValues.NULL_ENUM_VALUE == value)) {
                    curObj["GlobalTrackingOption"] = null;
                }
                else {
                    curObj["GlobalTrackingOption"] = value;
                }
                if (((isEmbedded == false) 
                            && (AutoCommitProp == true))) {
                    PrivateLateBoundObject.Put();
                }
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsLMSFragmentSizeNull {
            get {
                if ((curObj["LMSFragmentSize"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("This property is the message size below which message will be handled in memory. " +
            "Any message with a size above this wull be buffered to the file system to reduce" +
            " memory requirements.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint LMSFragmentSize {
            get {
                if ((curObj["LMSFragmentSize"] == null)) {
                    return System.Convert.ToUInt32(0);
                }
                return ((uint)(curObj["LMSFragmentSize"]));
            }
            set {
                curObj["LMSFragmentSize"] = value;
                if (((isEmbedded == false) 
                            && (AutoCommitProp == true))) {
                    PrivateLateBoundObject.Put();
                }
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsLMSThresholdNull {
            get {
                if ((curObj["LMSThreshold"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"This property is the Threshold size (in bytes) for large message support. As a message batch is processed, if the in-memory size of a message batch reaches the number of bytes specified for Large message threshold then the portion of the message batch that has been processed is written to the MessageBox before the remainder of the message batch is processed.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint LMSThreshold {
            get {
                if ((curObj["LMSThreshold"] == null)) {
                    return System.Convert.ToUInt32(0);
                }
                return ((uint)(curObj["LMSThreshold"]));
            }
            set {
                curObj["LMSThreshold"] = value;
                if (((isEmbedded == false) 
                            && (AutoCommitProp == true))) {
                    PrivateLateBoundObject.Put();
                }
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsMajorVersionNull {
            get {
                if ((curObj["MajorVersion"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("This property is the Major Version Of BizTalk Server Installed.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint MajorVersion {
            get {
                if ((curObj["MajorVersion"] == null)) {
                    return System.Convert.ToUInt32(0);
                }
                return ((uint)(curObj["MajorVersion"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("This property contains the initial catalog part of the BizTalk Management databas" +
            "e connect string, and represents the database name.  This property is required f" +
            "or instance creation. Max length for this property is 123 characters.")]
        public string MgmtDbName {
            get {
                return ((string)(curObj["MgmtDbName"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("This property contains the data source part of the BizTalk database connect strin" +
            "g.  This property is required for instance creation. Max length for this propert" +
            "y is 80 characters.")]
        public string MgmtDbServerName {
            get {
                return ((string)(curObj["MgmtDbServerName"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsMinorVersionNull {
            get {
                if ((curObj["MinorVersion"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("This property is the Minor Version Of BizTalk Server Installed")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint MinorVersion {
            get {
                if ((curObj["MinorVersion"] == null)) {
                    return System.Convert.ToUInt32(0);
                }
                return ((uint)(curObj["MinorVersion"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("This property contains the name of the BizTalk group. This property is required f" +
            "or instance creation. Max length for this property is 128 characters.")]
        public string Name {
            get {
                return ((string)(curObj["Name"]));
            }
            set {
                curObj["Name"] = value;
                if (((isEmbedded == false) 
                            && (AutoCommitProp == true))) {
                    PrivateLateBoundObject.Put();
                }
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsPerfCounterCacheRefreshIntervalNull {
            get {
                if ((curObj["PerfCounterCacheRefreshInterval"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("This property indicates interval at which performance counters are refreshed. Ran" +
            "ge for this property is 1 to 43200.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint PerfCounterCacheRefreshInterval {
            get {
                if ((curObj["PerfCounterCacheRefreshInterval"] == null)) {
                    return System.Convert.ToUInt32(0);
                }
                return ((uint)(curObj["PerfCounterCacheRefreshInterval"]));
            }
            set {
                curObj["PerfCounterCacheRefreshInterval"] = value;
                if (((isEmbedded == false) 
                            && (AutoCommitProp == true))) {
                    PrivateLateBoundObject.Put();
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Name of the RuleEngine SQL database. Max length for this property is 123 characte" +
            "rs.")]
        public string RuleEngineDBName {
            get {
                return ((string)(curObj["RuleEngineDBName"]));
            }
            set {
                curObj["RuleEngineDBName"] = value;
                if (((isEmbedded == false) 
                            && (AutoCommitProp == true))) {
                    PrivateLateBoundObject.Put();
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Name of the SQL server where RuleEngine database is located. Max length for this " +
            "property is 80 characters.")]
        public string RuleEngineDBServerName {
            get {
                return ((string)(curObj["RuleEngineDBServerName"]));
            }
            set {
                curObj["RuleEngineDBServerName"] = value;
                if (((isEmbedded == false) 
                            && (AutoCommitProp == true))) {
                    PrivateLateBoundObject.Put();
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The identifier by which the CIM_Setting object is known.")]
        public string SettingID {
            get {
                return ((string)(curObj["SettingID"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("This is a comment field that allows you to associate a friendly name with a signi" +
            "ng certificate. Max length for this property is 256 characters.")]
        public string SignCertComment {
            get {
                return ((string)(curObj["SignCertComment"]));
            }
            set {
                curObj["SignCertComment"] = value;
                if (((isEmbedded == false) 
                            && (AutoCommitProp == true))) {
                    PrivateLateBoundObject.Put();
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"Thumbprint of the signing certificate to be used to sign outbound documents sent by any BizTalk Host instance in the BizTalk group. Certificate Thumbprint is a digest of the certificate data and is found in the Certificate Details, and is expressed as a hexadecimal value. Example: 'FD36 90F0 EB49 F7B8 D3AB 1C69 8E02 ED84 5738 7868'. Max length for this property is 80 characters.")]
        public string SignCertThumbprint {
            get {
                return ((string)(curObj["SignCertThumbprint"]));
            }
            set {
                curObj["SignCertThumbprint"] = value;
                if (((isEmbedded == false) 
                            && (AutoCommitProp == true))) {
                    PrivateLateBoundObject.Put();
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Name of the machine where Single Sign On (SSO) server resides on. Max length for " +
            "this property is 80 characters.")]
        public string SSOServerName {
            get {
                return ((string)(curObj["SSOServerName"]));
            }
            set {
                curObj["SSOServerName"] = value;
                if (((isEmbedded == false) 
                            && (AutoCommitProp == true))) {
                    PrivateLateBoundObject.Put();
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Name of the master subscription SQL database.  This property is required for inst" +
            "ance creation. Max length for this property is 100 characters.")]
        public string SubscriptionDBName {
            get {
                return ((string)(curObj["SubscriptionDBName"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Name of the SQL server where the master subscription database is located.  This p" +
            "roperty is required for instance creation. Max length for this property is 80 ch" +
            "aracters.")]
        public string SubscriptionDBServerName {
            get {
                return ((string)(curObj["SubscriptionDBServerName"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Name of the tracking SQL analysis database. Max length for this property is 123 c" +
            "haracters.")]
        public string TrackingAnalysisDBName {
            get {
                return ((string)(curObj["TrackingAnalysisDBName"]));
            }
            set {
                curObj["TrackingAnalysisDBName"] = value;
                if (((isEmbedded == false) 
                            && (AutoCommitProp == true))) {
                    PrivateLateBoundObject.Put();
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Name of the tracking SQL analysis server. Max length for this property is 80 char" +
            "acters.")]
        public string TrackingAnalysisDBServerName {
            get {
                return ((string)(curObj["TrackingAnalysisDBServerName"]));
            }
            set {
                curObj["TrackingAnalysisDBServerName"] = value;
                if (((isEmbedded == false) 
                            && (AutoCommitProp == true))) {
                    PrivateLateBoundObject.Put();
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Name of the tracking SQL database.  This property is required for instance creati" +
            "on. Max length for this property is 123 characters.")]
        public string TrackingDBName {
            get {
                return ((string)(curObj["TrackingDBName"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Name of the SQL server where the tracking database is located.  This property is " +
            "required for instance creation. Max length for this property is 80 characters.")]
        public string TrackingDBServerName {
            get {
                return ((string)(curObj["TrackingDBServerName"]));
            }
        }
        
        private bool CheckIfProperClass(System.Management.ManagementScope mgmtScope, System.Management.ManagementPath path, System.Management.ObjectGetOptions OptionsParam) {
            if (((path != null) 
                        && (string.Compare(path.ClassName, this.ManagementClassName, true, System.Globalization.CultureInfo.InvariantCulture) == 0))) {
                return true;
            }
            else {
                return CheckIfProperClass(new System.Management.ManagementObject(mgmtScope, path, OptionsParam));
            }
        }
        
        private bool CheckIfProperClass(System.Management.ManagementBaseObject theObj) {
            if (((theObj != null) 
                        && (string.Compare(((string)(theObj["__CLASS"])), this.ManagementClassName, true, System.Globalization.CultureInfo.InvariantCulture) == 0))) {
                return true;
            }
            else {
                System.Array parentClasses = ((System.Array)(theObj["__DERIVATION"]));
                if ((parentClasses != null)) {
                    int count = 0;
                    for (count = 0; (count < parentClasses.Length); count = (count + 1)) {
                        if ((string.Compare(((string)(parentClasses.GetValue(count))), this.ManagementClassName, true, System.Globalization.CultureInfo.InvariantCulture) == 0)) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        
        private void ResetBamDBName() {
            curObj["BamDBName"] = null;
            if (((isEmbedded == false) 
                        && (AutoCommitProp == true))) {
                PrivateLateBoundObject.Put();
            }
        }
        
        private void ResetBamDBServerName() {
            curObj["BamDBServerName"] = null;
            if (((isEmbedded == false) 
                        && (AutoCommitProp == true))) {
                PrivateLateBoundObject.Put();
            }
        }
        
        private bool ShouldSerializeConfigurationCacheRefreshInterval() {
            if ((this.IsConfigurationCacheRefreshIntervalNull == false)) {
                return true;
            }
            return false;
        }
        
        private void ResetConfigurationCacheRefreshInterval() {
            curObj["ConfigurationCacheRefreshInterval"] = null;
            if (((isEmbedded == false) 
                        && (AutoCommitProp == true))) {
                PrivateLateBoundObject.Put();
            }
        }
        
        private bool ShouldSerializeGlobalTrackingOption() {
            if ((this.IsGlobalTrackingOptionNull == false)) {
                return true;
            }
            return false;
        }
        
        private void ResetGlobalTrackingOption() {
            curObj["GlobalTrackingOption"] = null;
            if (((isEmbedded == false) 
                        && (AutoCommitProp == true))) {
                PrivateLateBoundObject.Put();
            }
        }
        
        private bool ShouldSerializeLMSFragmentSize() {
            if ((this.IsLMSFragmentSizeNull == false)) {
                return true;
            }
            return false;
        }
        
        private void ResetLMSFragmentSize() {
            curObj["LMSFragmentSize"] = null;
            if (((isEmbedded == false) 
                        && (AutoCommitProp == true))) {
                PrivateLateBoundObject.Put();
            }
        }
        
        private bool ShouldSerializeLMSThreshold() {
            if ((this.IsLMSThresholdNull == false)) {
                return true;
            }
            return false;
        }
        
        private void ResetLMSThreshold() {
            curObj["LMSThreshold"] = null;
            if (((isEmbedded == false) 
                        && (AutoCommitProp == true))) {
                PrivateLateBoundObject.Put();
            }
        }
        
        private bool ShouldSerializeMajorVersion() {
            if ((this.IsMajorVersionNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeMinorVersion() {
            if ((this.IsMinorVersionNull == false)) {
                return true;
            }
            return false;
        }
        
        private void ResetName() {
            curObj["Name"] = null;
            if (((isEmbedded == false) 
                        && (AutoCommitProp == true))) {
                PrivateLateBoundObject.Put();
            }
        }
        
        private bool ShouldSerializePerfCounterCacheRefreshInterval() {
            if ((this.IsPerfCounterCacheRefreshIntervalNull == false)) {
                return true;
            }
            return false;
        }
        
        private void ResetPerfCounterCacheRefreshInterval() {
            curObj["PerfCounterCacheRefreshInterval"] = null;
            if (((isEmbedded == false) 
                        && (AutoCommitProp == true))) {
                PrivateLateBoundObject.Put();
            }
        }
        
        private void ResetRuleEngineDBName() {
            curObj["RuleEngineDBName"] = null;
            if (((isEmbedded == false) 
                        && (AutoCommitProp == true))) {
                PrivateLateBoundObject.Put();
            }
        }
        
        private void ResetRuleEngineDBServerName() {
            curObj["RuleEngineDBServerName"] = null;
            if (((isEmbedded == false) 
                        && (AutoCommitProp == true))) {
                PrivateLateBoundObject.Put();
            }
        }
        
        private void ResetSignCertComment() {
            curObj["SignCertComment"] = null;
            if (((isEmbedded == false) 
                        && (AutoCommitProp == true))) {
                PrivateLateBoundObject.Put();
            }
        }
        
        private void ResetSignCertThumbprint() {
            curObj["SignCertThumbprint"] = null;
            if (((isEmbedded == false) 
                        && (AutoCommitProp == true))) {
                PrivateLateBoundObject.Put();
            }
        }
        
        private void ResetSSOServerName() {
            curObj["SSOServerName"] = null;
            if (((isEmbedded == false) 
                        && (AutoCommitProp == true))) {
                PrivateLateBoundObject.Put();
            }
        }
        
        private void ResetTrackingAnalysisDBName() {
            curObj["TrackingAnalysisDBName"] = null;
            if (((isEmbedded == false) 
                        && (AutoCommitProp == true))) {
                PrivateLateBoundObject.Put();
            }
        }
        
        private void ResetTrackingAnalysisDBServerName() {
            curObj["TrackingAnalysisDBServerName"] = null;
            if (((isEmbedded == false) 
                        && (AutoCommitProp == true))) {
                PrivateLateBoundObject.Put();
            }
        }
        
        [Browsable(true)]
        public void CommitObject() {
            if ((isEmbedded == false)) {
                PrivateLateBoundObject.Put();
            }
        }
        
        [Browsable(true)]
        public void CommitObject(System.Management.PutOptions putOptions) {
            if ((isEmbedded == false)) {
                PrivateLateBoundObject.Put(putOptions);
            }
        }
        
        private void Initialize() {
            AutoCommitProp = true;
            isEmbedded = false;
        }
        
        private static string ConstructPath(string keyMgmtDbName, string keyMgmtDbServerName) {
            string strPath = "ROOT\\MicrosoftBizTalkServer:MSBTS_GroupSetting";
            strPath = string.Concat(strPath, string.Concat(".MgmtDbName=", string.Concat("\"", string.Concat(keyMgmtDbName, "\""))));
            strPath = string.Concat(strPath, string.Concat(",MgmtDbServerName=", string.Concat("\"", string.Concat(keyMgmtDbServerName, "\""))));
            return strPath;
        }
        
        private void InitializeObject(System.Management.ManagementScope mgmtScope, System.Management.ManagementPath path, System.Management.ObjectGetOptions getOptions) {
            Initialize();
            if ((path != null)) {
                if ((CheckIfProperClass(mgmtScope, path, getOptions) != true)) {
                    throw new System.ArgumentException("Class name does not match.");
                }
            }
            PrivateLateBoundObject = new System.Management.ManagementObject(mgmtScope, path, getOptions);
            PrivateSystemProperties = new ManagementSystemProperties(PrivateLateBoundObject);
            curObj = PrivateLateBoundObject;
        }
        
        // Different overloads of GetInstances() help in enumerating instances of the WMI class.
        public static GroupSettingCollection GetInstances() {
            return GetInstances(null, null, null);
        }
        
        public static GroupSettingCollection GetInstances(string condition) {
            return GetInstances(null, condition, null);
        }
        
        public static GroupSettingCollection GetInstances(System.String [] selectedProperties) {
            return GetInstances(null, null, selectedProperties);
        }
        
        public static GroupSettingCollection GetInstances(string condition, System.String [] selectedProperties) {
            return GetInstances(null, condition, selectedProperties);
        }
        
        public static GroupSettingCollection GetInstances(System.Management.ManagementScope mgmtScope, System.Management.EnumerationOptions enumOptions) {
            if ((mgmtScope == null)) {
                if ((statMgmtScope == null)) {
                    mgmtScope = new System.Management.ManagementScope();
                    mgmtScope.Path.NamespacePath = "root\\MicrosoftBizTalkServer";
                }
                else {
                    mgmtScope = statMgmtScope;
                }
            }
            System.Management.ManagementPath pathObj = new System.Management.ManagementPath();
            pathObj.ClassName = "MSBTS_GroupSetting";
            pathObj.NamespacePath = "root\\MicrosoftBizTalkServer";
            System.Management.ManagementClass clsObject = new System.Management.ManagementClass(mgmtScope, pathObj, null);
            if ((enumOptions == null)) {
                enumOptions = new System.Management.EnumerationOptions();
                enumOptions.EnsureLocatable = true;
            }
            return new GroupSettingCollection(clsObject.GetInstances(enumOptions));
        }
        
        public static GroupSettingCollection GetInstances(System.Management.ManagementScope mgmtScope, string condition) {
            return GetInstances(mgmtScope, condition, null);
        }
        
        public static GroupSettingCollection GetInstances(System.Management.ManagementScope mgmtScope, System.String [] selectedProperties) {
            return GetInstances(mgmtScope, null, selectedProperties);
        }
        
        public static GroupSettingCollection GetInstances(System.Management.ManagementScope mgmtScope, string condition, System.String [] selectedProperties) {
            if ((mgmtScope == null)) {
                if ((statMgmtScope == null)) {
                    mgmtScope = new System.Management.ManagementScope();
                    mgmtScope.Path.NamespacePath = "root\\MicrosoftBizTalkServer";
                }
                else {
                    mgmtScope = statMgmtScope;
                }
            }
            System.Management.ManagementObjectSearcher ObjectSearcher = new System.Management.ManagementObjectSearcher(mgmtScope, new SelectQuery("MSBTS_GroupSetting", condition, selectedProperties));
            System.Management.EnumerationOptions enumOptions = new System.Management.EnumerationOptions();
            enumOptions.EnsureLocatable = true;
            ObjectSearcher.Options = enumOptions;
            return new GroupSettingCollection(ObjectSearcher.Get());
        }
        
        [Browsable(true)]
        public static GroupSetting CreateInstance() {
            System.Management.ManagementScope mgmtScope = null;
            if ((statMgmtScope == null)) {
                mgmtScope = new System.Management.ManagementScope();
                mgmtScope.Path.NamespacePath = CreatedWmiNamespace;
            }
            else {
                mgmtScope = statMgmtScope;
            }
            System.Management.ManagementPath mgmtPath = new System.Management.ManagementPath(CreatedClassName);
            System.Management.ManagementClass tmpMgmtClass = new System.Management.ManagementClass(mgmtScope, mgmtPath, null);
            return new GroupSetting(tmpMgmtClass.CreateInstance());
        }
        
        [Browsable(true)]
        public void Delete() {
            PrivateLateBoundObject.Delete();
        }
        
        public uint RegisterLocalServer() {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("RegisterLocalServer", inParams, null);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint UnregisterLocalServer() {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("UnregisterLocalServer", inParams, null);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                return System.Convert.ToUInt32(0);
            }
        }
        
        public enum GlobalTrackingOptionValues {
            
            Disable_tracking = 0,
            
            Normal_tracking = 1,
            
            NULL_ENUM_VALUE = 2,
        }
        
        // Enumerator implementation for enumerating instances of the class.
        public class GroupSettingCollection : object, ICollection {
            
            private ManagementObjectCollection privColObj;
            
            public GroupSettingCollection(ManagementObjectCollection objCollection) {
                privColObj = objCollection;
            }
            
            public virtual int Count {
                get {
                    return privColObj.Count;
                }
            }
            
            public virtual bool IsSynchronized {
                get {
                    return privColObj.IsSynchronized;
                }
            }
            
            public virtual object SyncRoot {
                get {
                    return this;
                }
            }
            
            public virtual void CopyTo(System.Array array, int index) {
                privColObj.CopyTo(array, index);
                int nCtr;
                for (nCtr = 0; (nCtr < array.Length); nCtr = (nCtr + 1)) {
                    array.SetValue(new GroupSetting(((System.Management.ManagementObject)(array.GetValue(nCtr)))), nCtr);
                }
            }
            
            public virtual System.Collections.IEnumerator GetEnumerator() {
                return new GroupSettingEnumerator(privColObj.GetEnumerator());
            }
            
            public class GroupSettingEnumerator : object, System.Collections.IEnumerator {
                
                private ManagementObjectCollection.ManagementObjectEnumerator privObjEnum;
                
                public GroupSettingEnumerator(ManagementObjectCollection.ManagementObjectEnumerator objEnum) {
                    privObjEnum = objEnum;
                }
                
                public virtual object Current {
                    get {
                        return new GroupSetting(((System.Management.ManagementObject)(privObjEnum.Current)));
                    }
                }
                
                public virtual bool MoveNext() {
                    return privObjEnum.MoveNext();
                }
                
                public virtual void Reset() {
                    privObjEnum.Reset();
                }
            }
        }
        
        // TypeConverter to handle null values for ValueType properties
        public class WMIValueTypeConverter : TypeConverter {
            
            private TypeConverter baseConverter;
            
            private System.Type baseType;
            
            public WMIValueTypeConverter(System.Type inBaseType) {
                baseConverter = TypeDescriptor.GetConverter(inBaseType);
                baseType = inBaseType;
            }
            
            public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Type srcType) {
                return baseConverter.CanConvertFrom(context, srcType);
            }
            
            public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Type destinationType) {
                return baseConverter.CanConvertTo(context, destinationType);
            }
            
            public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value) {
                return baseConverter.ConvertFrom(context, culture, value);
            }
            
            public override object CreateInstance(System.ComponentModel.ITypeDescriptorContext context, System.Collections.IDictionary dictionary) {
                return baseConverter.CreateInstance(context, dictionary);
            }
            
            public override bool GetCreateInstanceSupported(System.ComponentModel.ITypeDescriptorContext context) {
                return baseConverter.GetCreateInstanceSupported(context);
            }
            
            public override PropertyDescriptorCollection GetProperties(System.ComponentModel.ITypeDescriptorContext context, object value, System.Attribute[] attributeVar) {
                return baseConverter.GetProperties(context, value, attributeVar);
            }
            
            public override bool GetPropertiesSupported(System.ComponentModel.ITypeDescriptorContext context) {
                return baseConverter.GetPropertiesSupported(context);
            }
            
            public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(System.ComponentModel.ITypeDescriptorContext context) {
                return baseConverter.GetStandardValues(context);
            }
            
            public override bool GetStandardValuesExclusive(System.ComponentModel.ITypeDescriptorContext context) {
                return baseConverter.GetStandardValuesExclusive(context);
            }
            
            public override bool GetStandardValuesSupported(System.ComponentModel.ITypeDescriptorContext context) {
                return baseConverter.GetStandardValuesSupported(context);
            }
            
            public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, System.Type destinationType) {
                if ((baseType.BaseType == typeof(System.Enum))) {
                    if ((value.GetType() == destinationType)) {
                        return value;
                    }
                    if ((((value == null) 
                                && (context != null)) 
                                && (context.PropertyDescriptor.ShouldSerializeValue(context.Instance) == false))) {
                        return  "NULL_ENUM_VALUE" ;
                    }
                    return baseConverter.ConvertTo(context, culture, value, destinationType);
                }
                if (((baseType == typeof(bool)) 
                            && (baseType.BaseType == typeof(System.ValueType)))) {
                    if ((((value == null) 
                                && (context != null)) 
                                && (context.PropertyDescriptor.ShouldSerializeValue(context.Instance) == false))) {
                        return "";
                    }
                    return baseConverter.ConvertTo(context, culture, value, destinationType);
                }
                if (((context != null) 
                            && (context.PropertyDescriptor.ShouldSerializeValue(context.Instance) == false))) {
                    return "";
                }
                return baseConverter.ConvertTo(context, culture, value, destinationType);
            }
        }
        
        // Embedded class to represent WMI system Properties.
        [TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
        public class ManagementSystemProperties {
            
            private System.Management.ManagementBaseObject PrivateLateBoundObject;
            
            public ManagementSystemProperties(System.Management.ManagementBaseObject ManagedObject) {
                PrivateLateBoundObject = ManagedObject;
            }
            
            [Browsable(true)]
            public int GENUS {
                get {
                    return ((int)(PrivateLateBoundObject["__GENUS"]));
                }
            }
            
            [Browsable(true)]
            public string CLASS {
                get {
                    return ((string)(PrivateLateBoundObject["__CLASS"]));
                }
            }
            
            [Browsable(true)]
            public string SUPERCLASS {
                get {
                    return ((string)(PrivateLateBoundObject["__SUPERCLASS"]));
                }
            }
            
            [Browsable(true)]
            public string DYNASTY {
                get {
                    return ((string)(PrivateLateBoundObject["__DYNASTY"]));
                }
            }
            
            [Browsable(true)]
            public string RELPATH {
                get {
                    return ((string)(PrivateLateBoundObject["__RELPATH"]));
                }
            }
            
            [Browsable(true)]
            public int PROPERTY_COUNT {
                get {
                    return ((int)(PrivateLateBoundObject["__PROPERTY_COUNT"]));
                }
            }
            
            [Browsable(true)]
            public string[] DERIVATION {
                get {
                    return ((string[])(PrivateLateBoundObject["__DERIVATION"]));
                }
            }
            
            [Browsable(true)]
            public string SERVER {
                get {
                    return ((string)(PrivateLateBoundObject["__SERVER"]));
                }
            }
            
            [Browsable(true)]
            public string NAMESPACE {
                get {
                    return ((string)(PrivateLateBoundObject["__NAMESPACE"]));
                }
            }
            
            [Browsable(true)]
            public string PATH {
                get {
                    return ((string)(PrivateLateBoundObject["__PATH"]));
                }
            }
        }
    }
}
