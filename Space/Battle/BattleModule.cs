using Legatus.Mathematics;
using Microsoft.Xna.Framework;
using Space.Data;
using Space.Data.Saving;
using System.Collections.Generic;

namespace Space.Battle
{
    public class BattleModule
    {
        public readonly BattleShip Ship;
        public List<BattleTurret> Turrets = new List<BattleTurret>();
        public ModuleData Data { private set; get; }
        public Point GridPosition { private set; get; }
        private int _Rotation;
        
        public SpriteData Sprite { get { return Data.Sprite; } }
        public Rectangle GridArea { get { return new Rectangle(GridPosition, GridSize); } }
        //public Vector2 Origin { get { return new Vector2(Data.Width / 2f, Data.Height / 2f); } }
        public Vector2 PositionRelative { get { return GridPosition.ToVector2(); } }
        public Vector2 Position
        {
            get
            {
                return Ship.CenterOfMass + Vector2.Transform(PositionRelative - Ship.CenterOfMassRelative, Ship.HeadingMatrix);
            }
        }
        public Point GridSize { get { return Data.Size; } }
        public int Rotation
        {
            set { if (Rotatable) _Rotation = MathExtra.Wrap(value, 4); else _Rotation = 0; }
            get { return _Rotation; }
        }
        public float RotationRadians { get { return MathHelper.PiOver2 * _Rotation; } }
        public bool Rotatable { get { return Data.Rotatable; } }

        public BattleModule(ModuleData data, BattleShip ship, Point gridPosition, int rotation)
        {
            Data = data;
            Ship = ship;
            GridPosition = gridPosition;
            Rotation = rotation;
            foreach (TurretData turret in Data.Turrets)
            {
                Turrets.Add(new BattleTurret(this, turret));
            }
        }

        public BattleModule(ModuleSaveData save, BattleShip ship, GameDataManager dataManager)
            : this(dataManager.Modules[save.DataID], ship, save.GridPosition, save.Rotation)
        {
        }
    }
}
