using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using TaleWorlds.Core;
using TaleWorlds.LinQuick;
using TaleWorlds.ModuleManager;

namespace RealmsForgotten.Managers
{
    internal class CharacterCreationManager
    {
        public static CharacterCreationManager _instance = new();
        public static CharacterCreationManager Instance => _instance;

        internal Dictionary<string, Tuple<string, string, string, string>> villagerMin;
        internal Dictionary<string, Tuple<string, string, string, string>> villagerMax;
        internal Dictionary<string, Tuple<string, string, string, string>> fighterMin;
        internal Dictionary<string, Tuple<string, string, string, string>> fighterMax;
        internal readonly string[] cultures = { "battania", "aserai", "empire", "khuzait", "sturgia", "vlandia" };

        public CharacterCreationManager()
        {
            villagerMin = new Dictionary<string, Tuple<string, string, string, string>>();
            villagerMax = new Dictionary<string, Tuple<string, string, string, string>>();
            fighterMin = new Dictionary<string, Tuple<string, string, string, string>>();
            fighterMax = new Dictionary<string, Tuple<string, string, string, string>>();
        }

        public void OnInitializeGameStarter(Game game, IGameStarter starterObject)
        {
            Tuple<string, string, string, string> tuple;            
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(Path.Combine(new string[] { string.Concat(ModuleHelper.GetModuleFullPath("SandBoxCore"), "ModuleData/sandboxcore_bodyproperties.xml") }));
            villagerMax = new Dictionary<string, Tuple<string, string, string, string>>();
            villagerMin = new Dictionary<string, Tuple<string, string, string, string>>();
            fighterMax = new Dictionary<string, Tuple<string, string, string, string>>();
            fighterMin = new Dictionary<string, Tuple<string, string, string, string>>();
            foreach (object childNode in xmlDocument.ChildNodes)
            {
                XmlNode xmlNodes = (XmlNode)childNode;
                if (xmlNodes.Name == "BodyProperties")
                {
                    foreach (object obj in xmlNodes)
                    {
                        XmlNode xmlNodes1 = (XmlNode)obj;
                        if (xmlNodes1.Name == "BodyProperty")
                        {
                            XmlAttribute itemOf = xmlNodes1.Attributes["id"];
                            for (int i = 0; i < (int)cultures.Length; i++)
                            {
                                foreach (object childNode1 in xmlNodes1.ChildNodes)
                                {
                                    XmlElement xmlElement = (XmlElement)childNode1;
                                    if (itemOf.FirstChild.Value.Contains(cultures[i]))
                                    {
                                        if ((villagerMin.TryGetValue(cultures[i], out tuple) || !(xmlElement.Name == "BodyPropertiesMin") ? false : xmlNodes1.Attributes["id"].Value.Contains("villager")))
                                        {
                                            Dictionary<string, Tuple<string, string, string, string>> strs = villagerMin;
                                            Dictionary<string, Tuple<string, string, string, string>> strs1 = villagerMax;
                                            villagerMin.Add(cultures[i], new Tuple<string, string, string, string>(xmlElement.Attributes["age"].Value, xmlElement.Attributes["weight"].Value, xmlElement.Attributes["build"].Value, xmlElement.Attributes["key"].Value));
                                            continue;
                                        }
                                        else if ((villagerMax.TryGetValue(cultures[i], out tuple) || !(xmlElement.Name == "BodyPropertiesMax") ? false : xmlNodes1.Attributes["id"].Value.Contains("villager")))
                                        {
                                            villagerMax.Add(cultures[i], new Tuple<string, string, string, string>(xmlElement.Attributes["age"].Value, xmlElement.Attributes["weight"].Value, xmlElement.Attributes["build"].Value, xmlElement.Attributes["key"].Value));
                                            continue;
                                        }
                                        else if ((fighterMin.TryGetValue(cultures[i], out tuple) || !(xmlElement.Name == "BodyPropertiesMin") ? false : xmlNodes1.Attributes["id"].Value.Contains("fighter")))
                                        {
                                            fighterMin.Add(cultures[i], new Tuple<string, string, string, string>(xmlElement.Attributes["age"].Value, xmlElement.Attributes["weight"].Value, xmlElement.Attributes["build"].Value, xmlElement.Attributes["key"].Value));
                                            continue;
                                        }
                                        else if ((fighterMax.TryGetValue(cultures[i], out tuple) || !(xmlElement.Name == "BodyPropertiesMax") ? false : xmlNodes1.Attributes["id"].Value.Contains("fighter")))
                                        {
                                            fighterMax.Add(cultures[i], new Tuple<string, string, string, string>(xmlElement.Attributes["age"].Value, xmlElement.Attributes["weight"].Value, xmlElement.Attributes["build"].Value, xmlElement.Attributes["key"].Value));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
