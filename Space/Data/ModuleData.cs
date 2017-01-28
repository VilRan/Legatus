using Legatus.Mathematics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace Space.Data
{
    public class ModuleData : GameData
    {
        public const int East = 0;
        public const int North = 1;
        public const int West = 2;
        public const int South = 3;

        public ModuleComponentData[] Components { private set; get; }
        public HitElement[] HitRegions { private set; get; }
        public ResourceAmount[] ResourceCost { private set; get; }
        public SpriteData Sprite { private set; get; }
        public bool[] Attach { private set; get; }
        public string ID { private set; get; }
        public string Title { get { return _Title; } }
        public string Description { get { return _Description; } }
        public int Width { private set; get; }
        public int Height { private set; get; }
        public bool Rotatable { private set; get; }
        public float HP { private set; get; }
        public float Mass { private set; get; }
        //public float MassSquared { private set; get; }
        private string _Title;
        private string _Description;

        public IEnumerable<TurretData> Turrets { get { return Components.OfType<TurretData>(); } }
        public Point Size { get { return new Point(Width, Height); } }

        public ModuleData()
        {
        }

        public override void Initialize(XmlElement node, GameDataManager manager)
        {
            ID = node.GetAttribute("ID");
            _Title = node.GetAttribute("Title");
            _Description = node.GetAttribute("Desc");
            Sprite = manager.Sprites[node.GetAttribute("Sprite")];
            string[] size = node.GetAttribute("Size").Split(',');
            Width = int.Parse(size[0]);
            Height = int.Parse(size[1]);
            string attachString = node.GetAttribute("Attach");
            Attach = new bool[]{attachString.Contains("E"), attachString.Contains("N"), attachString.Contains("W"), attachString.Contains("S")};
            Rotatable = bool.Parse(node.GetAttribute("Rotate"));
            HP = float.Parse(node.GetAttribute("HP"), CultureInfo.InvariantCulture);
            Mass = float.Parse(node.GetAttribute("Mass"), CultureInfo.InvariantCulture);
            //MassSquared = Mass * Mass;

            List<ModuleComponentData> components = new List<ModuleComponentData>();
            List<HitElement> hitRegions = new List<HitElement>();
            List<ResourceAmount> resourceCost = new List<ResourceAmount>();
            foreach (XmlElement subnode in node.ChildNodes.OfType<XmlElement>())
            {
                switch (subnode.Name)
                {
                    case "ResourceProduction": components.Add(new ResourceProductionData(subnode, manager)); break;
                    case "ResourceStorage": components.Add(new ResourceStorageData(subnode, manager)); break;
                    case "Thruster": components.Add(new ThrusterData(subnode, manager)); break;
                    case "Turret": components.Add(new TurretData(subnode, manager)); break;
                    case "Shield": components.Add(new ShieldData(subnode, manager)); break;
                    case "ResourceCost": resourceCost.Add(new ResourceAmount(subnode, manager)); break;
                    case "HitPolygon": ; break;
                }
            }
            Components = components.ToArray();
            HitRegions = hitRegions.ToArray();
            ResourceCost = resourceCost.ToArray();
        }
    }
}
