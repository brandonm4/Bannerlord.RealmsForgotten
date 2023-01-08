using HarmonyLib;

using SandBox;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.Core;

using RealmsForgotten.Utility;

namespace RealmsForgotten.Patches.MainMapPatches
{
    internal class MainMapPatch : PatchBase<MainMapPatch>
    {
        public override bool Applied { get; protected set; }
        private static readonly MethodInfo TargetMethodInfo =
            typeof(MapScene).GetMethod(
                "Load",
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly
            );
        private static readonly MethodInfo PatchMethodInfo =
            typeof(MainMapPatch).GetMethod(
                nameof(Transpiler),
                BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly
            );

        public override bool IsApplicable(Game? game = null)
        {
            if (File.Exists(ModulePath + "MapFix.txt"))
            {
                string[] lines = File.ReadAllLines(Path.Combine(ModulePath, "ModuleData", "MapName.txt"));
                new_map_name = lines[0];
            }
            else
            {
               MessageHelper.LogDebugMessage("There does not exist a config for Aurelian's Map Fix at: " + ModulePath + "MapFix.txt" + "\nDefaulting to modded_main_map.");
            }

            return true;
        }
        public override bool IsEarlyPatch => true;

        public override void Apply(Game game)
        {
            if (Applied)
            {
                return;
            }

            SubModule.Harmony.Patch(
                TargetMethodInfo,
                transpiler: new HarmonyMethod(PatchMethodInfo) { priority = Priority.First, }
            );

            Applied = true;
        }

        public override void Reset() { }


        static string ModulePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase.Remove(0, 8)) + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar;
        static string new_map_name = "modded_main_map";
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            int truthOccurance = -1;
            bool truthFlag = false;
            foreach (CodeInstruction instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Ldstr && instruction.OperandIs("Main_map"))
                {
                    instruction.operand = new_map_name;
                }
                else if (instruction.opcode == OpCodes.Ldloca_S)
                {
                    truthOccurance++;
                    truthFlag = true;
                }
                else if (instruction.opcode == OpCodes.Stfld)
                {
                    truthFlag = false;
                }
                else if (instruction.opcode == OpCodes.Ldc_I4_0 && truthFlag && (truthOccurance == 1 || truthOccurance == 3))
                {
                    instruction.opcode = OpCodes.Ldc_I4_1;
                }
                yield return instruction;
            }
        }
    }
}
