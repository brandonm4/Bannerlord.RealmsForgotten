using HarmonyLib;

using RealmsForgotten.Managers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using TaleWorlds.Core;

namespace RealmsForgotten.Utility
{
    public static class Helper
    {
        internal static BodyProperties GenerateCultureBodyProperties(string culture)
        {
            BodyProperties bodyProperty;
            StaticBodyProperties staticBodyProperty;
            Tuple<string, string, string, string> item = CharacterCreationManager.Instance.fighterMin[culture];
            Tuple<string, string, string, string> tuple = CharacterCreationManager.Instance.fighterMax[culture];
            XmlDocument xmlDocument = new XmlDocument();
            XmlNode xmlNodes = xmlDocument.CreateNode(XmlNodeType.Element, "BodyProperty", null);
            xmlDocument.AppendChild(xmlNodes);
            XmlAttribute item4 = xmlDocument.CreateAttribute("key", null);
            item4.Value = item.Item4;
            xmlNodes.Attributes.Append(item4);
            BodyProperties.FromXmlNode(xmlNodes, out bodyProperty);
            ulong value = Traverse.Create(bodyProperty).Property<ulong>("KeyPart1", null).Value;
            ulong num = Traverse.Create(bodyProperty).Property<ulong>("KeyPart2", null).Value;
            ulong value1 = Traverse.Create(bodyProperty).Property<ulong>("KeyPart3", null).Value;
            ulong num1 = Traverse.Create(bodyProperty).Property<ulong>("KeyPart4", null).Value;
            ulong value2 = Traverse.Create(bodyProperty).Property<ulong>("KeyPart5", null).Value;
            ulong num2 = Traverse.Create(bodyProperty).Property<ulong>("KeyPart8", null).Value;
            value <<= 32;
            num <<= 32;
            value1 <<= 32;
            num1 <<= 32;
            value2 <<= 32;
            num2 <<= 32;
            item4.Value = tuple.Item4;
            StaticBodyProperties.FromXmlNode(xmlNodes, out staticBodyProperty);
            ulong value3 = Traverse.Create(staticBodyProperty).Property<ulong>("KeyPart1", null).Value;
            ulong num3 = Traverse.Create(staticBodyProperty).Property<ulong>("KeyPart2", null).Value;
            ulong value4 = Traverse.Create(staticBodyProperty).Property<ulong>("KeyPart3", null).Value;
            ulong num4 = Traverse.Create(staticBodyProperty).Property<ulong>("KeyPart4", null).Value;
            ulong value5 = Traverse.Create(staticBodyProperty).Property<ulong>("KeyPart5", null).Value;
            ulong num5 = Traverse.Create(staticBodyProperty).Property<ulong>("KeyPart8", null).Value;
            value3 <<= 32;
            num3 <<= 32;
            value4 <<= 32;
            num4 <<= 32;
            value5 <<= 32;
            num5 <<= 32;
            ulong num6 = (ulong)CharacterCreationManager.Instance.random.NextLong((long)value, (long)value3);
            ulong num7 = (ulong)CharacterCreationManager.Instance.random.NextLong((long)num, (long)num3);
            ulong num8 = (ulong)CharacterCreationManager.Instance.random.NextLong((long)value1, (long)value4);
            ulong num9 = (ulong)CharacterCreationManager.Instance.random.NextLong((long)num1, (long)num4);
            ulong num10 = (ulong)CharacterCreationManager.Instance.random.NextLong((long)value2, (long)value5);
            ulong num11 = (ulong)CharacterCreationManager.Instance.random.NextLong((long)num2, (long)num5);
            num6 <<= 32;
            num7 <<= 32;
            num8 <<= 32;
            num9 <<= 32;
            num10 <<= 32;
            CharacterCreationManager.Instance.random.NextLong(-9223372036854775808L, 9223372036854775807L);
            CharacterCreationManager.Instance.random.NextLong(-9223372036854775808L, 9223372036854775807L);
            num11 <<= 32;
            StaticBodyProperties staticBodyProperty1 = new StaticBodyProperties(value, num, value1, num1, value2, (ulong)0, (ulong)0, num2);
            float single = MBRandom.RandomFloatRanged(Convert.ToSingle(item.Item1), Convert.ToSingle(tuple.Item1));
            float single1 = MBRandom.RandomFloatRanged(Convert.ToSingle(item.Item2), Convert.ToSingle(tuple.Item2));
            float single2 = MBRandom.RandomFloatRanged(Convert.ToSingle(item.Item3), Convert.ToSingle(tuple.Item3));
            return new BodyProperties(new DynamicBodyProperties(single, single2, single1), staticBodyProperty1);
        }

        public static long NextLong(this Random random, long min, long max)
        {

            ulong uRange = (ulong)(max - min);
            ulong ulongRand;
            do
            {
                byte[] buf = new byte[8];
                random.NextBytes(buf);
                ulongRand = (ulong)BitConverter.ToInt64(buf, 0);
            } while (ulongRand > ulong.MaxValue - ((ulong.MaxValue % uRange) + 1) % uRange);

            return (long)(ulongRand % uRange) + min;
        }
    }
}
