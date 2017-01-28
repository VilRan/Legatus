using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Globalization;

namespace Space.Data
{
    public class ProjectileLaunchData
    {
        public ProjectileData Type { private set; get; }
        /// <summary>
        /// Initial speed of the projectile in tiles per second.
        /// </summary>
        public float Speed { private set; get; }
        /// <summary>
        /// In fractions of a full circle. Smaller numbers mean more precision.
        /// </summary>
        public float Precision { private set; get; }

        public ProjectileLaunchData(ProjectileData type, float speed, float precision)
        {
            Type = type;
            Speed = speed;
            Precision = precision;
        }

        public ProjectileLaunchData(XmlElement node, GameDataManager data)
        {
            ProjectileData projectile = data.Projectiles[node.GetAttribute("ID")];
            float speed = float.Parse(node.GetAttribute("Speed"), CultureInfo.InvariantCulture);
            float precision = float.Parse(node.GetAttribute("Precision"), CultureInfo.InvariantCulture);
        }
    }
}
