using JetBrains.Annotations;

using System.Linq;

namespace RealmsForgotten.Utility;

    [PublicAPI]
    internal static class HarmonyHelpers
    {
        public static bool AlreadyPatched(HarmonyLib.Patches patchInfo) =>
            patchInfo != null && patchInfo.Owners.Any();

        public static bool AlreadyPatchedByOthers(HarmonyLib.Patches patchInfo)
        {
            if (patchInfo == null)
            {
                return false;
            }

            if (patchInfo.Owners.Count > 1)
            {
                return true;
            }

            return patchInfo.Owners.Count == 1 && patchInfo.Owners[0] != nameof(Utility);
        }
    }

