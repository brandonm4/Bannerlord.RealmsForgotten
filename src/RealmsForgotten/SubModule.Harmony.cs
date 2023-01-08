using Bannerlord.ButterLib.Logger.Extensions;

using HarmonyLib;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Reflection;

using TaleWorlds.Core;
using TaleWorlds.Library;

using RealmsForgotten.Utility;

namespace RealmsForgotten;
public partial class SubModule
{

    public static readonly Harmony Harmony = new Harmony(HarmonyDomain);
    public static IDictionary<Type, IPatch> ActivePatches = new Dictionary<Type, IPatch>();

    protected void ApplyPatches(Game game, Type moduletype, bool debugmode = false, bool doEarlyPatch = false)
    {
        //ActivePatches.Clear();
        MessageHelper.LogDebugMessage("Patches Started...");
        foreach (var patch in GetPatches(moduletype))
        {
            try
            {
                if (doEarlyPatch == patch.IsEarlyPatch)
                    patch.Reset();
            }
            catch (Exception ex)
            {
                LogFactory.Get<SubModule>().LogError(ex, $"Error while resetting patch: {patch.GetType().Name}");
            }

            
            if (patch.IsApplicable(game) && doEarlyPatch == patch.IsEarlyPatch)
            {
                try
                {
                    patch.Apply(game);
                    MessageHelper.LogDebugMessage($"Patch {patch.GetType().Name} Processed.");
                }
                catch (Exception ex)
                {
                    LogFactory.Get<SubModule>().LogErrorAndDisplay(ex, $"Error while applying patch: {patch.GetType().Name}");
                    throw (ex);
                    //TXPHelper.ShowMessage($"Error Patching: {patch.GetType().Name}", Colors.Red);
                }
            }

            var patchApplied = patch.Applied;
            if (patchApplied)
            {
                ActivePatches[patch.GetType()] = patch;
            }
            MessageHelper.LogDebugMessage($"{(patchApplied ? "Applied" : "Skipped")} Patch: {patch.GetType().Name}");
            if (debugmode)
            {
                MessageHelper.LogDebugMessage($"{(patchApplied ? "Applied" : "Skipped")} Patch: {patch.GetType().Name}");
            }
            else
            {
                MessageHelper.LogDebugMessage($"{(patchApplied ? "Applied" : "Skipped")} Patch: {patch.GetType().Name}");
            }
        }
        MessageHelper.LogDebugMessage("Patches Finished...");
    }

    private LinkedList<IPatch> _patches;

    public LinkedList<IPatch> GetPatches(Type moduletype)
    {
        if (_patches != null)
        {
            return _patches;
        }

        var patchInterfaceType = typeof(IPatch);
        _patches = new LinkedList<IPatch>();

        foreach (var type in moduletype.Assembly.GetTypes())
        {
            if (type.IsInterface || type.IsAbstract)
            {
                continue;
            }

            if (!patchInterfaceType.IsAssignableFrom(type))
            {
                continue;
            }

            try
            {
                var patch = (IPatch)Activator.CreateInstance(type, true);
                //var patch = (IPatch) FormatterServices.GetUninitializedObject(type);
                _patches.AddLast(patch);
            }
            catch (TargetInvocationException tie)
            {
                //     Error(tie.InnerException, $"Failed to create instance of patch: {type.FullName}");
                //SubModule.Logger.LogMessage(LogLevel.Error, $"Failed to create instance of patch: {type.FullName}");
                //SubModule.Logger.LogMessage(LogLevel.Error, tie.ToStringFull());
            }
            catch (Exception ex)
            {
                // Error(ex, $"Failed to create instance of patch: {type.FullName}");
                //SubModule.Logger.LogMessage(LogLevel.Error, $"Failed to create instance of patch: {type.FullName}");
                //SubModule.Logger.LogMessage(LogLevel.Error, ex.ToStringFull());
            }
        }
        return _patches;
    }
}


/* Harmony Patch code courtesty of Bannerlord Community Patch*/
/* License for this code file */
/*MIT License

Copyright (c) 2020 Tyler Young

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
