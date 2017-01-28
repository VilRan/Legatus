using System.Globalization;
using System.Xml;

namespace Space.Data
{
    public class WeaponData : GameData
    {
        public ProjectileLaunchData[] Projectiles { private set; get; }
        public ResourceAmount[] ReloadCost { private set; get; }
        public string ID { private set; get; }
        public float Cooldown { private set; get; }
        public float ReloadTime { private set; get; }

        public WeaponData()
        {
        }

        public override void Initialize(XmlElement node, GameDataManager manager)
        {
            ID = node.GetAttribute("ID");
            Cooldown = float.Parse(node.GetAttribute("Cooldown"), CultureInfo.InvariantCulture);

            XmlNodeList projectileNodes = node.SelectNodes("Projectile");
            Projectiles = new ProjectileLaunchData[projectileNodes.Count];
            for (int i = 0; i < projectileNodes.Count; i++)
                Projectiles[i] = new ProjectileLaunchData((XmlElement)projectileNodes[i], manager);
            
            XmlElement reload = (XmlElement)node.SelectSingleNode("Reload");
            ReloadTime = float.Parse(reload.GetAttribute("Time"), CultureInfo.InvariantCulture);
            XmlNodeList resourceUsageNodes = reload.SelectNodes("ResourceUsage");
            ReloadCost = new ResourceAmount[resourceUsageNodes.Count];
            for (int i = 0; i < resourceUsageNodes.Count; i++)
                ReloadCost[i] = new ResourceAmount((XmlElement)resourceUsageNodes[i], manager);
        }
    }
}
