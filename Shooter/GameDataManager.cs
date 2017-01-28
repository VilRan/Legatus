using Legatus.UI;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter
{
    public class GameDataManager
    {
        public UIDynamicSprite DefaultButtonFrame;
        public UIWindowDynamicSprite DefaultWindowFrame;
        public SpriteFont DefaultFont;
        public Texture2D PlayerTexture;
        public Texture2D ShotTexture;

        public GameDataManager(ShooterGame game)
        {
            //Test code: to be replaced.
            DefaultButtonFrame = new UIDynamicSprite(game.Content.Load<Texture2D>("Frame.png"), 8);
            DefaultWindowFrame = new UIWindowDynamicSprite(game.Content.Load<Texture2D>("Frame.png"), 8, 4);
            DefaultFont = game.Content.Load<SpriteFont>("DefaultFont12");
            PlayerTexture = game.Content.Load<Texture2D>("Player.png");
            ShotTexture = game.Content.Load<Texture2D>("Shot.png");
        }
    }
}
