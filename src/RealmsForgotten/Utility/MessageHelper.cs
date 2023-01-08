using Bannerlord.ButterLib.Logger.Extensions;

using Microsoft.Extensions.Logging;

using System;

using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;


namespace RealmsForgotten.Utility
{
    public static class MessageHelper
    {        
        public static void ShowMessage(string msg, Color? color = null)
        {
            if (color == null)
            {
                color = Color.White;
            }

            InformationManager.DisplayMessage(new InformationMessage(msg, (Color)color));
        }

        public static void ShowNotification(TextObject message, int priority = 0, BasicCharacterObject charObj = null,
                                             string soundEventPath = "")
        {
            MBInformationManager.AddQuickInformation(message, 0, charObj, soundEventPath);
        }

        public static void LogDebugMessage(string msg)
        {
            if (Settings.Instance.DebugMode)
            {
                SubModule.Log.LogDebugAndDisplay(msg);
            }
            else
            {
                SubModule.Log.LogDebug(msg);
            }
        }
        public static void LogDebugMessage(string msg, Exception ex)
        {
            if (Settings.Instance.DebugMode)
            {
                SubModule.Log.LogDebugAndDisplay(ex, msg);
            }
            else
            {
                SubModule.Log.LogDebug(ex, msg);
            }
        }
    }
}
