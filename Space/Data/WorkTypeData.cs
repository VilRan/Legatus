using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Space.Data
{
    public class WorkTypeData : GameData
    {
        public string ID { private set; get; }
        public string Title { private set; get; }

        public WorkTypeData()
        {
        }

        public override void Initialize(XmlElement node, GameDataManager manager)
        {
            ID = node.GetAttribute("ID");
            Title = node.GetAttribute("Title");
        }

    }
}
