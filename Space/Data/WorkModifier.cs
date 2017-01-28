using System.Xml;
using System.Globalization;

namespace Space.Data
{
    public struct WorkModifier : ITraitEffectData
    {
        public WorkTypeData Type;
        public float Value;

        public WorkModifier(WorkTypeData type, float value)
        {
            Type = type;
            Value = value;
        }

        public WorkModifier(XmlElement node, GameDataManager manager)
        {
            Type = manager.WorkTypes[node.GetAttribute("ID")];
            Value = float.Parse(node.GetAttribute("Multiplier"), CultureInfo.InvariantCulture);
        }
    }
}
