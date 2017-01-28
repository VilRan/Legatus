using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Legatus.Input;

namespace Legatus.UI
{
    public class UIViewport : UIActive, IDisposable
    {
        public IUIView View;
        public Color ClearColor = Color.Black;
        private GraphicsDevice GraphicsDevice;
        private RenderTarget2D RenderTarget;

        public UIViewport(int x, int y, int width, int height, IUIView view, LegatusGame game)
            : base (x, y, width, height)
        {
            View = view;
            GraphicsDevice = game.GraphicsDevice;
            RenderTarget = new RenderTarget2D(game.GraphicsDevice, width, height);
            game.Disposed += OnGameDisposed;
            TakesFocusOnMouseAction = true;
            TakesFocusOnMouseAction = true;
        }
        
        public override void Update(GameTime gameTime)
        {

        }

        public override void Predraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            GraphicsDevice.SetRenderTarget(RenderTarget);
            GraphicsDevice.Clear(ClearColor);
            View.Draw(gameTime, spriteBatch, Bounds);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(RenderTarget, Bounds, Color);
        }

        public override void OnKeyboardAction(KeyboardEventArgs keyboard)
        {
            View.OnKeyboardAction(keyboard);
            base.OnKeyboardAction(keyboard);
        }
        public override UIElement OnMouseAction(MouseActionEventArgs mouse)
        {
            UIElement intercept = base.OnMouseAction(mouse);

            if (intercept == this)
                View.OnMouseAction(new MouseActionEventArgs(mouse.Action, mouse.Button, mouse.Position - Position, mouse.HeldKeys));

            return intercept;
        }
        public override UIElement OnMouseDrag(MouseDragEventArgs mouse)
        {
            UIElement intercept = base.OnMouseDrag(mouse);

            if (intercept == this)
                View.OnMouseDrag(new MouseDragEventArgs(mouse.Button, mouse.CurrentPosition - Position, mouse.PreviousPosition - Position, mouse.StartPosition - Position, mouse.HeldKeys));

            return intercept;
        }
        public override UIElement OnMouseOver(MouseOverEventArgs mouse)
        {
            UIElement intercept = base.OnMouseOver(mouse);

            if (intercept == this)
                View.OnMouseOver(new MouseOverEventArgs(mouse.Position - Position, mouse.HeldKeys));

            return intercept;
        }
        public override UIElement OnMouseScroll(MouseScrollEventArgs mouse)
        {
            UIElement intercept = base.OnMouseScroll(mouse);

            if (intercept == this)
                View.OnMouseScroll(new MouseScrollEventArgs(mouse.Position - Position, mouse.Delta, mouse.HeldKeys));

            return intercept;
        }

        /// <summary>
        /// Note: the game will automatically dispose all viewports when it disposes itself.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (RenderTarget != null)
                {
                    RenderTarget.Dispose();
                    RenderTarget = null;
                }
            }
        }

        private void OnGameDisposed(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
