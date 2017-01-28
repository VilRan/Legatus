using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Legatus.Input;

namespace Legatus.UI
{
    public class UIButtonDynamic : UIButton
    {
        public UIDynamicSprite Sprite;
        public SpriteFont TextFont;
        public Color TextColor;
        public string Label;
        private Color CurrentColor;

        protected Rectangle TopLeft     { get { return new Rectangle(TopLeftX,      TopLeftY,       TopLeftWidth,       TopLeftHeight); } }
        protected Rectangle Top         { get { return new Rectangle(TopX,          TopY,           TopWidth,           TopHeight); } }
        protected Rectangle TopRight    { get { return new Rectangle(TopRightX,     TopRightY,      TopRightWidth,      TopRightHeight); } }
        protected Rectangle MiddleLeft  { get { return new Rectangle(MiddleLeftX,   MiddleLeftY,    MiddleLeftWidth,    MiddleLeftHeight); } }
        protected Rectangle MiddleRight { get { return new Rectangle(MiddleRightX,  MiddleRightY,   MiddleRightWidth,   MiddleRightHeight); } }
        protected Rectangle BottomLeft  { get { return new Rectangle(BottomLeftX,   BottomLeftY,    BottomLeftWidth,    BottomLeftHeight); } }
        protected Rectangle Bottom      { get { return new Rectangle(BottomX,       BottomY,        BottomWidth,        BottomHeight); } }
        protected Rectangle BottomRight { get { return new Rectangle(BottomRightX,  BottomRightY,   BottomRightWidth,   BottomRightHeight); } }

        private int TopLeftX { get { return X; } }
        private int TopX { get { return X + Sprite.TopLeft.Width; } }
        private int TopRightX { get { return X + Width - Sprite.TopRight.Width; } }
        private int MiddleLeftX { get { return X; } }
        private int MiddleRightX { get { return X + Width - Sprite.MiddleRight.Width; } }
        private int BottomLeftX { get { return X; } }
        private int BottomX { get { return X + Sprite.BottomLeft.Width; } }
        private int BottomRightX { get { return X + Width - Sprite.BottomRight.Width; } }

        private int TopLeftY { get { return Y; } }
        private int TopY { get { return Y; } }
        private int TopRightY { get { return Y; } }
        private int MiddleLeftY { get { return Y + Sprite.TopLeft.Height; } }
        private int MiddleRightY { get { return Y + Sprite.TopRight.Height; } }
        private int BottomLeftY { get { return Y + Height - Sprite.BottomLeft.Height; } }
        private int BottomY { get { return Y + Height - Sprite.Bottom.Height; } }
        private int BottomRightY { get { return Y + Height - Sprite.BottomRight.Height; } }

        private int TopLeftWidth { get { return Sprite.TopLeft.Width; } }
        private int TopWidth { get { return Width - TopLeftWidth - TopRightWidth; } }
        private int TopRightWidth { get { return Sprite.TopRight.Width; } }
        private int MiddleLeftWidth { get { return Sprite.MiddleLeft.Width; } }
        private int MiddleRightWidth { get { return Sprite.MiddleRight.Width; } }
        private int BottomLeftWidth { get { return Sprite.BottomLeft.Width; } }
        private int BottomWidth { get { return Width - BottomLeftWidth - BottomRightWidth; } }
        private int BottomRightWidth { get { return Sprite.BottomRight.Width; } }

        private int TopLeftHeight { get { return Sprite.TopLeft.Height; } }
        private int TopHeight { get { return Sprite.Top.Height; } }
        private int TopRightHeight { get { return Sprite.TopRight.Height; } }
        private int MiddleLeftHeight { get { return Height - TopLeftHeight - BottomLeftHeight; } }
        private int MiddleRightHeight { get { return Height - TopRightHeight - BottomRightHeight; } }
        private int BottomLeftHeight { get { return Sprite.BottomLeft.Height; } }
        private int BottomHeight { get { return Sprite.Bottom.Height; } }
        private int BottomRightHeight { get { return Sprite.BottomRight.Height; } }

        //private int GridSize { get { return Sprite.GridSize; } }
        private Color DarkerColor { get { return new Color(Color.R / 2, Color.G / 2, Color.B / 2); } }

        public UIButtonDynamic(int x, int y, int width, int height, UIDynamicSprite sprite, SpriteFont font, string label)
            : base(x, y, width, height)
        {
            Sprite = sprite;
            TextFont = font;
            TextColor = Color.Black;
            Label = label;
            CurrentColor = DarkerColor;
        }

        public UIButtonDynamic(int x, int y, int width, int height, UIButtonDelegate onClick, UIDynamicSprite sprite, SpriteFont font, string label)
            : base(x, y, width, height, onClick)
        {
            Sprite = sprite;
            TextFont = font;
            TextColor = Color.Black;
            Label = label;
            CurrentColor = DarkerColor;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            CurrentColor = DarkerColor;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite.Fill.Texture,           Bounds,         Sprite.Fill.Source,         CurrentColor);
            spriteBatch.Draw(Sprite.TopLeft.Texture,        TopLeft,        Sprite.TopLeft.Source,      CurrentColor);
            spriteBatch.Draw(Sprite.Top.Texture,            Top,            Sprite.Top.Source,          CurrentColor);
            spriteBatch.Draw(Sprite.TopRight.Texture,       TopRight,       Sprite.TopRight.Source,     CurrentColor);
            spriteBatch.Draw(Sprite.MiddleLeft.Texture,     MiddleLeft,     Sprite.MiddleLeft.Source,   CurrentColor);
            spriteBatch.Draw(Sprite.MiddleRight.Texture,    MiddleRight,    Sprite.MiddleRight.Source,  CurrentColor);
            spriteBatch.Draw(Sprite.BottomLeft.Texture,     BottomLeft,     Sprite.BottomLeft.Source,   CurrentColor);
            spriteBatch.Draw(Sprite.Bottom.Texture,         Bottom,         Sprite.Bottom.Source,       CurrentColor);
            spriteBatch.Draw(Sprite.BottomRight.Texture,    BottomRight,    Sprite.BottomRight.Source,  CurrentColor);
            spriteBatch.DrawString(TextFont, Label, new Vector2((int)(Center.X - TextFont.MeasureString(Label).X / 2), (int)(Center.Y - TextFont.MeasureString(Label).Y / 2)), TextColor);
        }

        public override UIElement OnMouseOver(MouseOverEventArgs mouse)
        {
            UIElement intercept = base.OnMouseOver(mouse);
            if (intercept == this)
            {
                CurrentColor = Color;
            }
            return intercept;
        }
    }
}
