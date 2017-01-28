using System.Globalization;
using System.Xml;

namespace Space.Data
{
    public class ProjectileData : GameData
    {
        public string ID { private set; get; }
        public float Damage { private set; get; }
        public float Mass { private set; get; }
        public float HP { private set; get; }
        
        public ProjectileData()
        {
        }
        
        public override void Initialize(XmlElement node, GameDataManager manager)
        {
            ID = node.GetAttribute("ID");
            Damage = float.Parse(node.GetAttribute("Damage"), CultureInfo.InvariantCulture);
            Mass = float.Parse(node.GetAttribute("Mass"), CultureInfo.InvariantCulture);
            HP = float.Parse(node.GetAttribute("HP"), CultureInfo.InvariantCulture);
        }
    }
}
