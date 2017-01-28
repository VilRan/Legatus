using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Legatus.Input;

namespace Legatus.UI
{
    public class UIWindowDynamic : UIWindow
    {
        private enum Edge { None, /*Body,*/ TopLeft, Top, TopRight, Left, Right, BottomLeft, Bottom, BottomRight }

        public UIWindowDynamicSprite Sprite;
        public Color EdgeColor;
        public bool ResizeableByUser;
        private Edge EdgeCaught;
        
        protected Rectangle FrameDimensions { get { return new Rectangle(FrameX,        FrameY,         FrameWidth,         FrameHeight); } }
        protected Rectangle TopLeft         { get { return new Rectangle(TopLeftX,      TopLeftY,       TopLeftWidth,       TopLeftHeight); } }
        protected Rectangle Top             { get { return new Rectangle(TopX,          TopY,           TopWidth,           TopHeight); } }
        protected Rectangle TopRight        { get { return new Rectangle(TopRightX,     TopRightY,      TopRightWidth,      TopRightHeight); } }
        protected Rectangle MiddleLeft      { get { return new Rectangle(MiddleLeftX,   MiddleLeftY,    MiddleLeftWidth,    MiddleLeftHeight); } }
        protected Rectangle MiddleRight     { get { return new Rectangle(MiddleRightX,  MiddleRightY,   MiddleRightWidth,   MiddleRightHeight); } }
        protected Rectangle BottomLeft      { get { return new Rectangle(BottomLeftX,   BottomLeftY,    BottomLeftWidth,    BottomLeftHeight); } }
        protected Rectangle Bottom          { get { return new Rectangle(BottomX,       BottomY,        BottomWidth,        BottomHeight); } }
        protected Rectangle BottomRight     { get { return new Rectangle(BottomRightX,  BottomRightY,   BottomRightWidth,   BottomRightHeight); } }

        private int FrameX { get { return X - EdgeMargin; } }
        private int FrameY { get { return Y - EdgeMargin; } }
        private int FrameWidth { get { return Width + EdgeMargin * 2; } }
        private int FrameHeight { get { return Height + EdgeMargin * 2; } }
        private int EdgeMargin { get { return Sprite.EdgeMargin; } }
        
        private int TopLeftX { get { return FrameX; } }
        private int TopX { get { return FrameX + Sprite.TopLeft.Width; } }
        private int TopRightX { get { return FrameX + FrameWidth - Sprite.TopRight.Width; } }
        private int MiddleLeftX { get { return FrameX; } }
        private int MiddleRightX { get { return FrameX + Width - Sprite.MiddleRight.Width; } }
        private int BottomLeftX { get { return FrameX; } }
        private int BottomX { get { return FrameX + Sprite.BottomLeft.Width; } }
        private int BottomRightX { get { return FrameX + Width - Sprite.BottomRight.Width; } }

        private int TopLeftY { get { return FrameY; } }
        private int TopY { get { return FrameY; } }
        private int TopRightY { get { return FrameY; } }
        private int MiddleLeftY { get { return FrameY + Sprite.TopLeft.Height; } }
        private int MiddleRightY { get { return FrameY + Sprite.TopRight.Height; } }
        private int BottomLeftY { get { return FrameY + Height - Sprite.BottomLeft.Height; } }
        private int BottomY { get { return FrameY + Height - Sprite.Bottom.Height; } }
        private int BottomRightY { get { return FrameY + Height - Sprite.BottomRight.Height; } }

        private int TopLeftWidth { get { return Sprite.TopLeft.Width; } }
        private int TopWidth { get { return FrameWidth - TopLeftWidth - TopRightWidth; } }
        private int TopRightWidth { get { return Sprite.TopRight.Width; } }
        private int MiddleLeftWidth { get { return Sprite.MiddleLeft.Width; } }
        private int MiddleRightWidth { get { return Sprite.MiddleRight.Width; } }
        private int BottomLeftWidth { get { return Sprite.BottomLeft.Width; } }
        private int BottomWidth { get { return FrameWidth - BottomLeftWidth - BottomRightWidth; } }
        private int BottomRightWidth { get { return Sprite.BottomRight.Width; } }

        private int TopLeftHeight { get { return Sprite.TopLeft.Height; } }
        private int TopHeight { get { return Sprite.Top.Height; } }
        private int TopRightHeight { get { return Sprite.TopRight.Height; } }
        private int MiddleLeftHeight { get { return FrameHeight - TopLeftHeight - BottomLeftHeight; } }
        private int MiddleRightHeight { get { return FrameHeight - TopRightHeight - BottomRightHeight; } }
        private int BottomLeftHeight { get { return Sprite.BottomLeft.Height; } }
        private int BottomHeight { get { return Sprite.Bottom.Height; } }
        private int BottomRightHeight { get { return Sprite.BottomRight.Height; } }

        public UIWindowDynamic(int x, int y, int width, int height, UIWindowDynamicSprite sprite)
            : base(x, y, width, height)
        {
            Sprite = sprite;
            EdgeColor = Color;
            ResizeableByUser = true;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Sprite != null)
            {
                spriteBatch.Draw(Sprite.Fill.Texture,           Bounds,         Sprite.Fill.Source,         Color);
                spriteBatch.Draw(Sprite.TopLeft.Texture,        TopLeft,        Sprite.TopLeft.Source,      EdgeColor);
                spriteBatch.Draw(Sprite.Top.Texture,            Top,            Sprite.Top.Source,          EdgeColor);
                spriteBatch.Draw(Sprite.TopRight.Texture,       TopRight,       Sprite.TopRight.Source,     EdgeColor);
                spriteBatch.Draw(Sprite.MiddleLeft.Texture,     MiddleLeft,     Sprite.MiddleLeft.Source,   EdgeColor);
                spriteBatch.Draw(Sprite.MiddleRight.Texture,    MiddleRight,    Sprite.MiddleRight.Source,  EdgeColor);
                spriteBatch.Draw(Sprite.BottomLeft.Texture,     BottomLeft,     Sprite.BottomLeft.Source,   EdgeColor);
                spriteBatch.Draw(Sprite.Bottom.Texture,         Bottom,         Sprite.Bottom.Source,       EdgeColor);
                spriteBatch.Draw(Sprite.BottomRight.Texture,    BottomRight,    Sprite.BottomRight.Source,  EdgeColor);
            }
            
            base.Draw(gameTime, spriteBatch);
        }

        public override UIElement OnMouseOver(MouseOverEventArgs args)
        {
            UIElement intercept = base.OnMouseOver(args);

            if (intercept == null && FrameDimensions.Contains(args.Position))
                intercept = this;

            return intercept;
        }

        public override UIElement OnMouseAction(MouseActionEventArgs mouse)
        {
            UIElement intercept = base.OnMouseAction(mouse);

            if (intercept == null && FrameDimensions.Contains(mouse.Position))
            {
                intercept = this;

                if (ResizeableByUser && mouse.Button == MouseButton.Left && mouse.Action == MouseAction.Pressed)
                {
                    if (TopLeft.Contains(mouse.Position))
                        EdgeCaught = Edge.TopLeft;
                    else if (Top.Contains(mouse.Position))
                        EdgeCaught = Edge.Top;
                    else if (TopRight.Contains(mouse.Position))
                        EdgeCaught = Edge.TopRight;
                    else if (MiddleLeft.Contains(mouse.Position))
                        EdgeCaught = Edge.Left;
                    else if (MiddleRight.Contains(mouse.Position))
                        EdgeCaught = Edge.Right;
                    else if (BottomLeft.Contains(mouse.Position))
                        EdgeCaught = Edge.BottomLeft;
                    else if (Bottom.Contains(mouse.Position))
                        EdgeCaught = Edge.Bottom;
                    else if (BottomRight.Contains(mouse.Position))
                        EdgeCaught = Edge.BottomRight;
                }
            }

            if (mouse.Button == MouseButton.Left && mouse.Action == MouseAction.Released)
            {
                EdgeCaught = Edge.None;
            }

            return intercept;
        }
        
        public override UIElement OnMouseDrag(MouseDragEventArgs mouse)
        {
            if (mouse.Button == MouseButton.Left)
            {

                switch (EdgeCaught)
                {
                    /*case Edge.Body:
                        {
                            MoveBy(mousePosition - previousPosition);
                            break;
                        }*/
                    case Edge.TopLeft:
                        {
                            int bottomY = Y + Height;
                            int newY = Math.Min(mouse.CurrentPosition.Y, bottomY - Sprite.MinimumHeight);
                            int newHeight = bottomY - newY;
                            int rightX = X + Width;
                            int newX = Math.Min(mouse.CurrentPosition.X, rightX - Sprite.MinimumWidth);
                            int newWidth = rightX - newX;
                            ResizeTo(newX, newY, newWidth, newHeight);
                            break;
                        }
                    case Edge.Top:
                        {
                            int bottomY = Y + Height;
                            int newY = Math.Min(mouse.CurrentPosition.Y, bottomY - Sprite.MinimumHeight);
                            int newHeight = bottomY - newY;
                            ResizeTo(X, newY, Width, newHeight);
                            break;
                        }
                    case Edge.TopRight:
                        {
                            int bottomX = Y + Height;
                            int newY = Math.Min(mouse.CurrentPosition.Y, bottomX - Sprite.MinimumHeight);
                            int newHeight = bottomX - newY;
                            int newWidth = Math.Max(mouse.CurrentPosition.X - X, Sprite.MinimumWidth);
                            ResizeTo(X, newY, newWidth, newHeight);
                            break;
                        }
                    case Edge.Left:
                        {
                            int rightX = X + Width;
                            int newX = Math.Min(mouse.CurrentPosition.X, rightX - Sprite.MinimumWidth);
                            int newWidth = rightX - newX;
                            ResizeTo(newX, Y, newWidth, Height);
                            break;
                        }
                    case Edge.Right:
                        {
                            int newWidth = Math.Max(mouse.CurrentPosition.X - X, Sprite.MinimumWidth);
                            ResizeTo(X, Y, newWidth, Height);
                            break;
                        }
                    case Edge.BottomLeft:
                        {
                            int rightX = X + Width;
                            int newX = Math.Min(mouse.CurrentPosition.X, rightX - Sprite.MinimumWidth);
                            int newWidth = rightX - newX;
                            int newHeight = Math.Max(mouse.CurrentPosition.Y - Y, Sprite.MinimumHeight);
                            ResizeTo(newX, Y, newWidth, newHeight);
                            break;
                        }
                    case Edge.Bottom:
                        {
                            int newHeight = Math.Max(mouse.CurrentPosition.Y - Y, Sprite.MinimumHeight);
                            ResizeTo(X, Y, Width, newHeight);
                            break;
                        }
                    case Edge.BottomRight:
                        {
                            int newWidth = Math.Max(mouse.CurrentPosition.X - X, Sprite.MinimumWidth);
                            int newHeight = Math.Max(mouse.CurrentPosition.Y - Y, Sprite.MinimumHeight);
                            ResizeTo(X, Y, newWidth, newHeight);
                            break;
                        }
                }

                if (EdgeCaught != Edge.None)
                    return this;
            }

            return base.OnMouseDrag(mouse);
        }
    }
}
