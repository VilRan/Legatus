using System.Xml;
using System.Globalization;

namespace Space.Data
{
    public struct WorkAmount
    {
        public WorkTypeData Type;
        public float Amount;

        public WorkAmount(WorkTypeData type, float amount)
        {
            Type = type;
            Amount = amount;
        }

        public WorkAmount(XmlElement node, GameDataManager manager)
        {
            Type = manager.WorkTypes[node.GetAttribute("ID")];
            Amount = float.Parse(node.GetAttribute("Amount"), CultureInfo.InvariantCulture);
        }
    }
}
