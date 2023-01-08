using HarmonyLib;

using RealmsForgotten.Patches.MainMapPatches;
using SandBox;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterCreation;
using TaleWorlds.Core;

using TaleWorlds.Localization;

using Utility;

namespace RealmsForgotten.Patches.CulturedStart
{
    [HarmonyPatch(typeof(CharacterCreationGenericStageVM), "RefreshSelectedOptions")]
    public class CSPatchCharacterCreationStageVM
    {
        private static CharacterCreationGenericStageVM _characterCreationGenericStageVM;

        public static void Postfix(CharacterCreationGenericStageVM __instance) => _characterCreationGenericStageVM = __instance;

        public static void OnNextStage() => _characterCreationGenericStageVM.OnNextStage();
    }
}

//internal class CharacacterCreationStageVMPatch : PatchBase<CharacacterCreationStageVMPatch>
//    {
//        public override bool Applied { get; protected set; }
//        private readonly MethodInfo TargetMethodInfo =
//            typeof(CharacterCreationGenericStageVM).GetMethod(
//                "RefreshSelectedOptions",
//                BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly
//            );
//        private readonly MethodInfo PatchMethodInfo =
//            typeof(MainMapPatch).GetMethod(
//                nameof(Transpiler),
//                BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly
//            );

//        public override bool IsApplicable(Game? game = null)
//        {           
//            return true;
//        }
//        public override bool IsEarlyPatch => true;

//        public override void Apply(Game game)
//        {
//            if (Applied)
//            {
//                return;
//            }

//            SubModule.Harmony.Patch(
//                TargetMethodInfo,
//                transpiler: new HarmonyMethod(PatchMethodInfo) { priority = Priority.First, }
//            );

//            Applied = true;
//        }

//        public override void Reset() { }

        
//    }
//}
