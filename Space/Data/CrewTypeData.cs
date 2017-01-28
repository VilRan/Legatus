using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;

namespace Space.Data
{
    public class CrewTypeData : GameData
    {
        public WorkAmount[] WorkOutputs { private set; get; }
        public string ID { private set; get; }
        public string Title { private set; get; }
        public float Attack { private set; get; }
        public float Defense { private set; get; }
        public float Wage { private set; get; }

        public override void Initialize(XmlElement node, GameDataManager manager)
        {
            ID = node.Attributes.GetNamedItem("ID").Value;
            Title = node.Attributes.GetNamedItem("Title").Value;
            Attack = float.Parse(node.GetAttribute("Attack"), CultureInfo.InvariantCulture);
            Defense = float.Parse(node.GetAttribute("Defense"), CultureInfo.InvariantCulture);
            Wage = float.Parse(node.GetAttribute("Wage"), CultureInfo.InvariantCulture);

            List<WorkAmount> workOutputs = new List<WorkAmount>();
            foreach (XmlElement subnode in node.ChildNodes.OfType<XmlElement>())
            {
                switch (subnode.Name)
                {
                    case "WorkOutput": workOutputs.Add(new WorkAmount(subnode, manager)); break;
                }
            }
            WorkOutputs = workOutputs.ToArray();
        }
    }
}
