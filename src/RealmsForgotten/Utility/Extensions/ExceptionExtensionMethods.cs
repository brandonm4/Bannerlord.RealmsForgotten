using System;
using System.Text;

namespace RealmsForgotten.Utility.Extensions;

public static class ExceptionExtensionMethods
{
    public static string ToStringFull(this Exception ex)
    {
        return GetString(ex);
    }

    private static string GetString(Exception ex)
    {
        StringBuilder sb = new StringBuilder();
        GetStringRecursive(ex, sb);
        sb.AppendLine();
        sb.AppendLine("Stack trace:");
        sb.AppendLine(ex.StackTrace);
        return sb.ToString();
    }

    private static void GetStringRecursive(Exception ex, StringBuilder sb)
    {
        sb.AppendLine($"{ex.GetType().Name}:");
        sb.AppendLine(ex.Message);
        if (ex.InnerException != null)
        {
            sb.AppendLine();
            GetStringRecursive(ex.InnerException, sb);
        }
    }
}