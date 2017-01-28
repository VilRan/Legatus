using Microsoft.Xna.Framework;
using Space.Battle;
using Space.Data;
using Space.Data.Saving;
using System;

namespace Space.ShipDesigner
{
    public class ShipDesignerMap
    {
        public enum Mode { Build, Test }

        public GameDataManager DataManager;
        public ModuleData[] Modules;
        public BattleShip Ship;
        public ShipSaveData TempSave;
        public ModuleData SelectedModule;
        public Mode ActiveMode;
        private int _ModuleRotation = 0;

        public float ModuleRotationRadians
        {
            get { return MathHelper.PiOver2 * _ModuleRotation; }
        }
        public int ModuleRotation
        {
            set { _ModuleRotation = value % 4; }
            get { return _ModuleRotation; }
        }

        public ShipDesignerMap(GameDataManager dataManager)
        {
            DataManager = dataManager;
            Modules = new ModuleData[dataManager.Modules.Count];
            dataManager.Modules.Values.CopyTo(Modules, 0);
            Ship = new BattleShip();
        }

        public void Update(GameTime gameTime)
        {
            Ship.Update(gameTime);
        }

        public void TestShip(object sender, EventArgs e)
        {
            if (ActiveMode == Mode.Build)
            {
                TempSave = new ShipSaveData(Ship);
                ActiveMode = Mode.Test;
            }
            else
            {
                Ship = new BattleShip(TempSave, DataManager);
                ActiveMode = Mode.Build;
            }
        }

        public void NewShip(object sender, EventArgs e)
        {
            Ship = new BattleShip();
        }

        public void SaveShip(object sender, EventArgs e)
        {
            ShipSaveData data = new ShipSaveData(Ship);
            data.Save("Ships", "test.ship");
        }

        public void LoadShip(object sender, EventArgs e)
        {
            ShipSaveData data = ShipSaveData.Load("Ships", "test.ship");
            Ship = new BattleShip(data, DataManager);
        }
    }
}
