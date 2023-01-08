// Decompiled with JetBrains decompiler
// Type: RealmsForgotten.Patches
// Assembly: RealmsForgotten, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B26E324A-F09C-4542-BFFC-02D91686071A
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Mount & Blade II Bannerlord\Modules\RF_Core\bin\Win64_Shipping_Client\RealmsForgotten.dll

using HarmonyLib;

using RealmsForgotten.CampaignSystem;
using RealmsForgotten.Managers;
using RealmsForgotten.Utility;

using StoryMode.CharacterCreationContent;
using System;
using System.Collections.Generic;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;

namespace RealmsForgotten.Patch.CulturedStart
{
  public class RFPatches
  {
    private static readonly AccessTools.StructFieldRef<BodyProperties, StaticBodyProperties> StaticBodyProps = AccessTools.StructFieldRefAccess<BodyProperties, StaticBodyProperties>("_staticBodyProperties");

    [HarmonyPatch(typeof(SandboxCharacterCreationContent), "OnCultureSelected")]
    public class SandboxCharacterCreationContentRefreshPropsAndClothing
    {
      public static void Postfix()
      {
        CharacterObject playerCharacter = CharacterObject.PlayerCharacter;
        playerCharacter.UpdatePlayerCharacterBodyProperties(Helper.GenerateCultureBodyProperties(playerCharacter.Culture.StringId), playerCharacter.Race, playerCharacter.IsFemale);
      }
    }
        [HarmonyPatch]
        public class CSPatchCharacterCreationInitialized
        {
            private static IEnumerable<MethodBase> TargetMethods()
            {
                yield return AccessTools.Method(typeof(SandboxCharacterCreationContent), "OnInitialized");
                yield return AccessTools.Method(typeof(StoryModeCharacterCreationContent), "OnInitialized");
                yield return AccessTools.Method(typeof(RFCharacterCreationContent), "OnInitialized");
            }
        }

        [HarmonyPatch]
        public class CSPatchCharacterCreationFinalized
        {
            private static IEnumerable<MethodBase> TargetMethods()
            {
                yield return AccessTools.Method(typeof(SandboxCharacterCreationContent), "OnCharacterCreationFinalized");
                yield return AccessTools.Method(typeof(StoryModeCharacterCreationContent), "OnCharacterCreationFinalized");
                yield return AccessTools.Method(typeof(RFCharacterCreationContent), "OnCharacterCreationFinalized");
            }

           public static void Postfix() => CulturedStartAction.Apply(CulturedStartManager.Current.StoryOption, CulturedStartManager.Current.LocationOption);
        }
    }
}
