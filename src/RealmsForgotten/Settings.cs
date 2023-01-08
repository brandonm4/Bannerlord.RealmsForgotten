using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;
using MCM.Common;

using Serilog.Events;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealmsForgotten;
internal class Settings
{
    public static SettingsMCM Instance
    {
        get
        {
            if (SettingsMCM.Instance != null)
                return SettingsMCM.Instance;
            return new();
        }
    }
}
internal class SettingsMCM : AttributeGlobalSettings<SettingsMCM>
    {
        public override string DisplayName
        {
            get { return SubModule.DisplayName; }
        }

        public override string FolderName
        {
            get { return SubModule.ModuleId; }
        }

        public override string FormatType
        {
            get { return "json2"; }
        }
        public override string Id
        {
            get { return SubModule.ModuleId; }
        }
        




        [SettingPropertyBool(
            "Experimental Features",
            HintText = "Settings in here are under development.",
            Order = 1,
            RequireRestart = false,
            IsToggle = true
        )]
        [SettingPropertyGroup("Experimental Features", GroupOrder = 50)]
        public bool EnableExperimentalFeatures { get; set; } = false;

    #region Debug
    [SettingPropertyGroup("{=txpg0013}Diagnostics", GroupOrder = 99)]
    [SettingPropertyBool("{=txpd0095}Enable Debug", RequireRestart = false, Order = 1)]
    public bool DebugMode { get; set; } = false;

    [SettingPropertyGroup("{=txpg0013}Diagnostics", GroupOrder = 99)]
    [SettingPropertyDropdown("Log Level", Order = 2, RequireRestart = true)]
    public Dropdown<LogEventLevel> DebugLogLevel { get; set; } = new Dropdown<LogEventLevel>(Enum.GetValues(typeof(LogEventLevel)).Cast<LogEventLevel>(), 0);
    #endregion

}

