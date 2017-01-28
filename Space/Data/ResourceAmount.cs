using System.Xml;
using System.Globalization;

namespace Space.Data
{
    public struct ResourceAmount
    {
        public ResourceData Type;
        public float Amount;

        public ResourceAmount(ResourceData type, float amount)
        {
            Type = type;
            Amount = amount;
        }
        
        public ResourceAmount(XmlElement node, GameDataManager manager)
        {
            Type = manager.Resources[node.GetAttribute("ID")];
            Amount = float.Parse(node.GetAttribute("Amount"), CultureInfo.InvariantCulture);
        }
    }
}
