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

using TaleWorlds.Core;

using TaleWorlds.Localization;

using RealmsForgotten.Utility;

namespace RealmsForgotten.Patch.CulturedStart
{
    internal class CharacterCreationOptionPatch : PatchBase<CharacterCreationOptionPatch>
    {
        public override bool Applied { get; protected set; }
        private readonly MethodInfo TargetMethodInfo =
            typeof(MapScene).GetMethod(
                "Load",
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly
            );
        private readonly MethodInfo PatchMethodInfo =
            typeof(MainMapPatch).GetMethod(
                nameof(Transpiler),
                BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly
            );

        public override bool IsApplicable(Game? game = null)
        {           
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

        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            List<CodeInstruction> codesAtIndex = new List<CodeInstruction>();
            List<CodeInstruction> codesToInsert = new List<CodeInstruction>();
            for (int i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Stloc_0 && codes[i - 1].opcode != OpCodes.Ldsfld)
                {
                    codes[i - 2].opcode = OpCodes.Nop;
                    codes[i - 1].opcode = OpCodes.Nop;
                    codesAtIndex.Add(codes[i]);
                }
            }
            codesToInsert.Add(new CodeInstruction(OpCodes.Ldarg_1));
            codesToInsert.Add(new CodeInstruction(OpCodes.Ldarg, 5));
            codesToInsert.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(CharacterCreationOptionPatch), "RemoveZeroPointText", new Type[] { typeof(string), typeof(List<SkillObject>), typeof(int) })));
            foreach (CodeInstruction codeAtIndex in codesAtIndex)
            {
                codes.InsertRange(codes.IndexOf(codeAtIndex), codesToInsert);
            }
            return codes;
        }

        // Remove redundant text if the attribute points to add is 0.
        private static TextObject RemoveZeroPointText(string text, List<SkillObject> skills, int attributeLevelToAdd)
        {
            if (attributeLevelToAdd == 0)
            {
                if (skills.Count == 3)
                {
                    return new TextObject("{=CulturedStart49}{EXP_VALUE} Skill {?IS_PLURAL_SKILL}Levels{?}Level{\\?} and {FOCUS_VALUE} Focus {?IS_PLURAL_FOCUS}Points{?}Point{\\?} to {SKILL_ONE}, {SKILL_TWO} and {SKILL_THREE}{NEWLINE}{TRAIT_DESC}{RENOWN_DESC}{GOLD_DESC}");
                }
                else if (skills.Count == 2)
                {
                    return new TextObject("{=CulturedStart50}{EXP_VALUE} Skill {?IS_PLURAL_SKILL}Levels{?}Level{\\?} and {FOCUS_VALUE} Focus {?IS_PLURAL_FOCUS}Points{?}Point{\\?} to {SKILL_ONE} and {SKILL_TWO}{NEWLINE}{TRAIT_DESC}{RENOWN_DESC}{GOLD_DESC}");
                }
                else if (skills.Count == 1)
                {
                    return new TextObject("{=CulturedStart51}{EXP_VALUE} Skill {?IS_PLURAL_SKILL}Levels{?}Level{\\?} and {FOCUS_VALUE} Focus {?IS_PLURAL_FOCUS}Points{?}Point{\\?} to {SKILL_ONE}{NEWLINE}{TRAIT_DESC}{RENOWN_DESC}{GOLD_DESC}");
                }
                else
                {
                    return new TextObject(null);
                }
            }
            else
            {
                return new TextObject(text);
            }
        }
    }
}
