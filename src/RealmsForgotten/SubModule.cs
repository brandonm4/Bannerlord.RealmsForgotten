using Bannerlord.ButterLib.MBSubModuleBaseExtended;
using Utility;
using HarmonyLib;

using System;
using System.Linq;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

using TaleWorlds.Library;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade;

using Microsoft.Extensions.Logging;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using Serilog.Events;
using Bannerlord.ButterLib.Common.Extensions;
using TaleWorlds.Localization;
using RealmsForgotten.Managers;

public partial class SubModule : MBSubModuleBaseEx
{
    public readonly static string ModuleId = "RealmsForgotten";
    public readonly static string DisplayName = "Realms Forgotten";
    public readonly static string HarmonyDomain = "com.realmsforgotten.coreoh -";
    internal readonly static Color StdTextColor = Color.FromUint(15822118);
    internal static SubModule Instance { get; set; } = default!;
    internal static ILogger Log { get; set; } = default!;

    public static string Version
    {
        get
        {
            return ($"{ModuleHelper.GetModuleInfo(ModuleId).Version}");
        }
    }


    #region Taleworlds Sub Mod Callbacks       
    protected override void OnSubModuleLoad()
    {
        
        Instance = this;

        //var extender = new UIExtender(Name);
        //extender.Register(typeof(SubModule).Assembly);
        //extender.Enable();

        LogEventLevel logEventLevel = LogEventLevel.Information;
        if (Settings.Instance.DebugMode)
        {
            logEventLevel = LogEventLevel.Verbose;
        }

        this.AddSerilogLoggerProvider($"{ModuleId}.log", new[] { $"{ModuleId}.*" }, config => config.MinimumLevel.Is(Settings.Instance.DebugLogLevel.SelectedValue));
        Log = LogFactory.Get<SubModule>();

        Module.CurrentModule.AddInitialStateOption(new InitialStateOption("RT", new TextObject("Realms Forgotten", null), 3, () => MBGameManager.StartNewGame(new RFCampaignManager()), () => new ValueTuple<bool, TextObject>(Module.CurrentModule.IsOnlyCoreContentEnabled, new TextObject("Disabled during installation.", null))));

        ApplyPatches(
                null,
                typeof(SubModule),
                Settings.Instance.DebugMode,
                true
            );

        base.OnSubModuleLoad();
    }

    protected override void InitializeGameStarter(Game game, IGameStarter starterObject)
    {
        CharacterCreationManager.Instance.OnInitializeGameStarter(game, starterObject);
    }

    public override void OnCampaignStart(Game game, object starterObject)
    {
        base.OnCampaignStart(game, starterObject);
    }

    protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
    {
        base.OnGameStart(game, gameStarterObject);

        if (game.GameType is Campaign)
        {
            ApplyPatches(
                game,
                typeof(SubModule),
                Settings.Instance.DebugMode
            );

            if (gameStarterObject is CampaignGameStarter campaignGameStarter)
            {
               
            }

            Log.LogInformation($"{DisplayName} {Version} Game Started");
        }

    }

    protected override void OnBeforeInitialModuleScreenSetAsRoot()
    {
        base.OnBeforeInitialModuleScreenSetAsRoot();

        try
        {
            if (Settings.Instance.DebugMode)
            {
                MessageHelper.ShowMessage($"{DisplayName} {Version}- DEBUG", Colors.Yellow);                

            }
            else
            {
                MessageHelper.ShowMessage($"{DisplayName} {Version} Loaded", Colors.Cyan);
            }
        }
        catch (Exception ex)
        {

        }
    }
    public override void OnGameInitializationFinished(Game game)
    {

    }

    public override void OnBeforeMissionBehaviorInitialize(Mission mission)
    {

    }

    protected override void OnSubModuleUnloaded()
    {
        base.OnSubModuleUnloaded();
    }

    #endregion
}
