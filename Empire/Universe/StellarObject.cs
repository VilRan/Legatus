using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Empire.Universe
{
    public abstract class StellarObject
    {
        /// <summary>
        /// Other objects that orbit this object.
        /// </summary>
        private List<StellarObject> Satellites;

        public StellarObject()
        {
            Satellites = new List<StellarObject>();
        }
    }
}
