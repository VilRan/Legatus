using Microsoft.Xna.Framework;
using Space.Battle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Space.Data.Saving
{
    public class ModuleSaveData
    {
        public string DataID;
        public Point GridPosition;
        public int Rotation;

        public ModuleSaveData()
        {

        }

        public ModuleSaveData(BattleModule module)
        {
            DataID = module.Data.ID;
            GridPosition = module.GridPosition;
            Rotation = module.Rotation;
        }
    }
}
