using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Space.Data
{
    public abstract class GameData
    {
        public abstract void Initialize(XmlElement node, GameDataManager manager);
    }
}
