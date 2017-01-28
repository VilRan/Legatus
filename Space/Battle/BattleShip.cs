using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Space.Data;
using Space.Data.Saving;
using Legatus.Mathematics;

namespace Space.Battle
{
    public class BattleShip
    {
        private List<BattleModule> ModuleList = new List<BattleModule>();
        public Vector2 Position;
        public Vector2 Velocity;
        public Vector2 CenterOfMassRelative { private set; get; }
        public float Mass { private set; get; }
        public float AngularMass { private set; get; }
        public float AngularMomentum { set; get; }
        public float Heading { set; get; }
        public bool TurnAssist { set; get; }

        public Matrix HeadingMatrix { get { return Matrix.CreateRotationZ(HeadingRadians); } }
        public Vector2 CenterOfMass { get { return Position + CenterOfMassRelative; } }
        public float HeadingRadians { get { return Heading * MathHelper.TwoPi; } }
        public float AngularVelocity { get { return AngularMass == 0 ? 0 : AngularMomentum / AngularMass; } }

        public IEnumerable<BattleModule> Modules { get { return ModuleList; } }

        public BattleShip()
        {

        }

        public BattleShip(ShipSaveData save, GameDataManager dataManager)
        {
            List<BattleModule> modules = new List<BattleModule>();
            foreach (ModuleSaveData module in save.Modules)
            {
                modules.Add(new BattleModule(module, this, dataManager));
            }
            AddModules(modules);
            Position = save.Position;
            Velocity = save.Velocity;
        }

        public void Update(GameTime gameTime)
        {
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Heading += AngularVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (TurnAssist)
                AngularMomentum = MathExtra.Reduce(AngularMomentum, 1000f);
        }

        public void AddModule(ModuleData moduleType, Point position, int rotation)
        {
            if (moduleType == null)
                return;
            if ( ! AreaIsFree(new Rectangle(position, moduleType.Size)))
                return;
            
            BattleModule module = new BattleModule(moduleType, this, position, rotation);
            ModuleList.Add(module);
            RecalculateMass();
        }

        public void AddModules(IEnumerable<BattleModule> modules)
        {
            ModuleList.AddRange(modules);
            RecalculateMass();
        }

        public void RemoveModule(Point position)
        {
            BattleModule module = GetModuleAt(position);
            ModuleList.Remove(module);
            RecalculateMass();
        }

        public bool AreaIsFree(Rectangle area)
        {
            return AreaIsFree(area.X, area.Y, area.Right, area.Bottom);
        }

        public bool AreaIsFree(int minX, int minY, int maxX, int maxY)
        {
            for (int x = minX; x < maxX; x++)
                for (int y = minY; y < maxY; y++)
                    if ( ModuleList.Exists( m => m.GridArea.Contains( new Point(x, y) ) ) )
                        return false;

            return true;
        }
        
        public BattleModule GetModuleAt(Point position)
        {
            return ModuleList.Find( m => m.GridArea.Contains(position) );
        }
        
        private void RecalculateMass()
        {
            CenterOfMassRelative = Vector2.Zero;
            Mass = 0f;
            foreach(BattleModule module in ModuleList)
            {
                CenterOfMassRelative += module.Data.Mass * module.PositionRelative;
                Mass += module.Data.Mass;
            }
            CenterOfMassRelative /= Mass;

            AngularMass = 0f;
            foreach(BattleModule module in ModuleList)
            {
                AngularMass += module.Data.Mass * Vector2.DistanceSquared(CenterOfMassRelative, module.PositionRelative);
            }

        }
    }
}
