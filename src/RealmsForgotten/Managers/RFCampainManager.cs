using RealmsForgotten.CampaignSystem;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.SaveSystem.Load;

namespace RealmsForgotten.Managers
{
    public class RFCampaignManager : MBGameManager
    {
        private bool _loadingSavedGame;

        private LoadResult _loadedGameResult;

        private int _seed = 1234;

        public RFCampaignManager()
        {
            this._loadingSavedGame = false;
            this._seed = (int)DateTime.Now.Ticks & 65535;
        }

        protected override void DoLoadingForGameManager(GameManagerLoadingSteps gameManagerLoadingStep, out GameManagerLoadingSteps nextStep)
        {
            nextStep = GameManagerLoadingSteps.None;
            switch (gameManagerLoadingStep)
            {
                case GameManagerLoadingSteps.PreInitializeZerothStep:
                    {
                        nextStep = GameManagerLoadingSteps.FirstInitializeFirstStep;
                        break;
                    }
                case GameManagerLoadingSteps.FirstInitializeFirstStep:
                    {
                        MBGameManager.LoadModuleData(this._loadingSavedGame);
                        nextStep = GameManagerLoadingSteps.WaitSecondStep;
                        break;
                    }
                case GameManagerLoadingSteps.WaitSecondStep:
                    {
                        if (!this._loadingSavedGame)
                        {
                            MBGameManager.StartNewGame();
                        }
                        nextStep = GameManagerLoadingSteps.SecondInitializeThirdState;
                        break;
                    }
                case GameManagerLoadingSteps.SecondInitializeThirdState:
                    {
                        MBGlobals.InitializeReferences();
                        if (this._loadingSavedGame)
                        {
                            MBDebug.Print("Initializing saved game begin...", 0, Debug.DebugColor.White, 17592186044416L);
                            ((Campaign)Game.LoadSaveGame(this._loadedGameResult, this).GameType).SetLoadingParameters(Campaign.GameLoadingType.SavedCampaign);
                            this._loadedGameResult = null;
                            Common.MemoryCleanupGC(false);
                            MBDebug.Print("Initializing saved game end...", 0, Debug.DebugColor.White, 17592186044416L);
                        }
                        else
                        {
                            MBDebug.Print("Initializing new game begin...", 0, Debug.DebugColor.White, 17592186044416L);
                            Campaign campaign = new Campaign(CampaignGameMode.Campaign);
                            Game.CreateGame(campaign, this);
                            campaign.SetLoadingParameters(Campaign.GameLoadingType.NewCampaign);
                            MBDebug.Print("Initializing new game end...", 0, Debug.DebugColor.White, 17592186044416L);
                        }
                        Game.Current.DoLoading();
                        nextStep = GameManagerLoadingSteps.PostInitializeFourthState;
                        break;
                    }
                case GameManagerLoadingSteps.PostInitializeFourthState:
                    {
                        bool flag = true;
                        foreach (MBSubModuleBase subModule in Module.CurrentModule.SubModules)
                        {
                            flag = (!flag ? false : subModule.DoLoading(Game.Current));
                        }
                        nextStep = (flag ? GameManagerLoadingSteps.FinishLoadingFifthStep : GameManagerLoadingSteps.PostInitializeFourthState);
                        break;
                    }
                case GameManagerLoadingSteps.FinishLoadingFifthStep:
                    {
                        nextStep = (Game.Current.DoLoading() ? GameManagerLoadingSteps.None : GameManagerLoadingSteps.FinishLoadingFifthStep);
                        break;
                    }
            }
        }

        private void LaunchSandboxCharacterCreation()
        {
            CharacterCreationState characterCreationState = Game.Current.GameStateManager.CreateState<CharacterCreationState>(new object[] { new RFCharacterCreationContent() });
            Game.Current.GameStateManager.CleanAndPushState(characterCreationState, 0);
        }

        public override void OnGameEnd(Game game)
        {
            MBDebug.SetErrorReportScene(null);
            base.OnGameEnd(game);
        }

        public override void OnLoadFinished()
        {
            string gameMenuId;
            if (this._loadingSavedGame)
            {
                if (CampaignSiegeTestStatic.IsSiegeTestBuild)
                {
                    CampaignSiegeTestStatic.DisableSiegeTest();
                }
                Game.Current.GameStateManager.OnSavedGameLoadFinished();
                Game.Current.GameStateManager.CleanAndPushState(Game.Current.GameStateManager.CreateState<MapState>(), 0);
                MapState activeState = Game.Current.GameStateManager.ActiveState as MapState;
                if (activeState != null)
                {
                    gameMenuId = activeState.GameMenuId;
                }
                else
                {
                    gameMenuId = null;
                }
                string str = gameMenuId;
                if (!string.IsNullOrEmpty(str))
                {
                    PlayerEncounter current = PlayerEncounter.Current;
                    if (current != null)
                    {
                        current.OnLoad();
                    }
                    Campaign.Current.GameMenuManager.SetNextMenu(str);
                }
                IPartyVisual visuals = PartyBase.MainParty.Visuals;
                if (visuals != null)
                {
                    visuals.SetMapIconAsDirty();
                }
                Campaign.Current.CampaignInformationManager.OnGameLoaded();
                foreach (Settlement all in Settlement.All)
                {
                    all.Party.Visuals.RefreshLevelMask(all.Party);
                }
                CampaignEventDispatcher.Instance.OnGameLoadFinished();
                if (activeState != null)
                {
                    activeState.OnLoadingFinished();
                }
            }
            else
            {
                MBDebug.Print("Switching to menu window...", 0, Debug.DebugColor.White, 17592186044416L);
                this.LaunchSandboxCharacterCreation();
            }
            base.IsLoaded = true;
        }
    }
}
