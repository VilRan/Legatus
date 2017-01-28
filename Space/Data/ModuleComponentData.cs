using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace Space.Data
{
    public abstract class ModuleComponentData
    {

    }

    public class ResourceProductionData : ModuleComponentData
    {
        public ResourceAmount[] Outputs { private set; get; }
        public ResourceAmount[] Inputs { private set; get; }

        public ResourceProductionData(XmlElement node, GameDataManager manager)
        {
            List<ResourceAmount> outputs = new List<ResourceAmount>();
            List<ResourceAmount> inputs = new List<ResourceAmount>();
            foreach (XmlElement subnode in node.ChildNodes.OfType<XmlElement>())
            {
                switch (subnode.Name)
                {
                    case "Output": outputs.Add(new ResourceAmount(subnode, manager)); break;
                    case "Input": inputs.Add(new ResourceAmount(subnode, manager)); break;
                }
            }
            Outputs = outputs.ToArray();
            Inputs = inputs.ToArray();
        }
    }

    public class ResourceStorageData : ModuleComponentData
    {
        public float Maximum { private set; get; }
        public bool Exclusive { private set; get; }
        private Dictionary<ResourceData, bool> Allowed = new Dictionary<ResourceData, bool>();

        public ResourceStorageData(XmlElement node, GameDataManager manager)
        {
            Maximum = float.Parse(node.GetAttribute("Amount"), CultureInfo.InvariantCulture);
            Exclusive = bool.Parse(node.GetAttribute("Exclusive"));

            foreach (XmlElement subnode in node.ChildNodes.OfType<XmlElement>())
            {
                ResourceData type = manager.Resources[subnode.GetAttribute("ID")];
                switch (subnode.Name)
                {
                    case "Allow": Allowed.Add(type, true); break;
                    case "Forbid": Allowed.Add(type, false); break;
                }
            }
        }

        public bool Allows(ResourceData type)
        {
            bool allowed = false;
            if (Allowed.TryGetValue(type, out allowed))
                return allowed;

            if (Exclusive)
                return false;
            return true;
        }
    }

    public class ShieldData : ModuleComponentData
    {
        public ResourceAmount[] ResourceUsage { private set; get; }
        public float Radius { private set; get; }
        public float HP { private set; get; }
        public float Regeneration { private set; get; }

        public ShieldData(XmlElement node, GameDataManager manager)
        {
            Radius = float.Parse(node.GetAttribute("Radius"), CultureInfo.InvariantCulture);
            HP = float.Parse(node.GetAttribute("HP"), CultureInfo.InvariantCulture);
            Regeneration = float.Parse(node.GetAttribute("Regeneration"), CultureInfo.InvariantCulture);

            List<ResourceAmount> resourceUsage = new List<ResourceAmount>();
            foreach (XmlElement subnode in node.ChildNodes.OfType<XmlElement>())
            {
                resourceUsage.Add(new ResourceAmount(subnode, manager));
            }
            ResourceUsage = resourceUsage.ToArray();
        }
    }

    public class ThrusterData : ModuleComponentData
    {
        public ResourceAmount[] ResourceUsage { private set; get; }
        public float Thrust { private set; get; }
        public float Direction { private set; get; }

        public ThrusterData(XmlElement node, GameDataManager manager)
        {
            Thrust = float.Parse(node.GetAttribute("Thrust"), CultureInfo.InvariantCulture);
            Direction = float.Parse(node.GetAttribute("Direction"), CultureInfo.InvariantCulture);

            List<ResourceAmount> resourceUsage = new List<ResourceAmount>();
            foreach (XmlElement subnode in node.ChildNodes.OfType<XmlElement>())
            {
                ResourceData type = manager.Resources[subnode.GetAttribute("ID")];
                float amount = float.Parse(subnode.GetAttribute("Amount"), CultureInfo.InvariantCulture);
                resourceUsage.Add(new ResourceAmount(type, amount));
            }
            ResourceUsage = resourceUsage.ToArray();
        }
    }

    public class TurretData : ModuleComponentData
    {
        public WeaponData Weapon { private set; get; }
        public SpriteData Sprite { private set; get; }
        public Vector2 Position { private set; get; }
        public float MinAngle { private set; get; }
        public float MaxAngle { private set; get; }
        public float TurnSpeed { private set; get; }

        public TurretData(XmlElement node, GameDataManager manager)
        {
            Weapon = manager.Weapons[node.GetAttribute("Weapon")];
            Sprite = manager.Sprites[node.GetAttribute("Sprite")];
            string[] position = node.GetAttribute("Position").Split(',');
            Position = new Vector2(float.Parse(position[0], CultureInfo.InvariantCulture), float.Parse(position[1], CultureInfo.InvariantCulture));
            MinAngle = float.Parse(node.GetAttribute("MinAngle"), CultureInfo.InvariantCulture);
            MaxAngle = float.Parse(node.GetAttribute("MaxAngle"), CultureInfo.InvariantCulture);
            TurnSpeed = float.Parse(node.GetAttribute("TurnSpeed"), CultureInfo.InvariantCulture);
        }
    }
}
