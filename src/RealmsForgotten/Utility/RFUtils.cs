using HarmonyLib.BUTR.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaleWorlds.Library;

using TaleWorlds.MountAndBlade;

namespace RealmsForgotten.Utility
{
    internal class RFUtils
    {
        public RFUtils()
        {
        }

        public static void removeInitialStateOption(string name)
        {
            try
            {                
                var t = Traverse2.Create(Module.CurrentModule).Field("_initialStateOptions");

                var initialStateOptions = t.GetValue<List<InitialStateOption>>();
                foreach (InitialStateOption initialStateOption in initialStateOptions)
                {
                    if (initialStateOption.Id.Contains(name))
                    {
                        initialStateOptions.Remove(initialStateOption);
                    }
                }
                t.SetValue(initialStateOptions);
            }
            catch (Exception exception)
            {
                InformationManager.DisplayMessage(new InformationMessage(exception.Message));
            }
        }
    }
}
