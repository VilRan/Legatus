using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;

namespace Space.Data
{
    public interface ITraitEffectData
    {

    }

    public class TraitData : GameData
    {
        public ITraitEffectData[] Effects { private set; get; }
        public string ID { private set; get; }
        public string Title { private set; get; }

        public override void Initialize(XmlElement node, GameDataManager manager)
        {
            ID = node.GetAttribute("ID");
            Title = node.GetAttribute("Title");
            
            List<ITraitEffectData> effects = new List<ITraitEffectData>();
            foreach (XmlElement subnode in node.ChildNodes.OfType<XmlElement>())
            {
                switch (subnode.Name)
                {
                    case "WorkModifier": effects.Add(new WorkModifier(subnode, manager)); break;
                    case "CrewAttackModifier": effects.Add(new CrewAttackModifier(subnode)); break;
                    case "CrewDefenseModifier": effects.Add(new CrewDefenseModifier(subnode)); break;
                    case "CrewWageModifier": effects.Add(new CrewWageModifier(subnode)); break;
                }
            }
            Effects = effects.ToArray();
        }
    }

    public struct CrewAttackModifier : ITraitEffectData
    {
        public float Value;

        public CrewAttackModifier(XmlElement node)
        {
            Value = float.Parse(node.GetAttribute("Multiplier"), CultureInfo.InvariantCulture);
        }
    }

    public struct CrewDefenseModifier : ITraitEffectData
    {
        public float Value;

        public CrewDefenseModifier(XmlElement node)
        {
            Value = float.Parse(node.GetAttribute("Multiplier"), CultureInfo.InvariantCulture);
        }
    }

    public struct CrewWageModifier : ITraitEffectData
    {
        public float Value;

        public CrewWageModifier(XmlElement node)
        {
            Value = float.Parse(node.GetAttribute("Multiplier"), CultureInfo.InvariantCulture);
        }
    }
}
