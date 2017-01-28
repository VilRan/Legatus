using Microsoft.Xna.Framework;
using Space.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Space.Battle
{
    public class BattleTurret
    {
        public readonly BattleModule Module;
        public TurretData Data { private set; get; }
        private float _Angle;

        public SpriteData Sprite { get { return Data.Sprite; } }
        public float AngleRadians
        {
            get { return MathHelper.PiOver2 * _Angle; }
        }
        public float Angle
        {
            set { _Angle = MathHelper.Clamp(value, Data.MinAngle, Data.MaxAngle); }
            get { return _Angle; }
        }

        public BattleTurret(BattleModule module, TurretData data)
        {
            Module = module;
            Data = data;
        }
    }
}
