using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Legatus.Input;

namespace Legatus.UI
{
    public class GameUI : IUIBase
    {
        /// <summary>
        /// A list of all direct child elements.
        /// </summary>
        private List<UIElement> Children;
        /// <summary>
        /// The element that will receive keyboard input.
        /// </summary>
        private UIElement KeyboardFocus;
        /// <summary>
        /// The currently displayed tooltip.
        /// </summary>
        private UITooltip Tooltip;
        private UIElement TooltipFocus;
        private LegatusGame Game;
        private double TooltipTimer;

        /// <summary>
        /// Always zero. Needed because the elements calculate their position based on their parent's position.
        /// </summary>
        public int X { get { return 0; } }
        /// <summary>
        /// Always zero. Needed because the elements calculate their position based on their parent's position.
        /// </summary>
        public int Y { get { return 0; } }
        /// <summary>
        /// Gets the width of the game's window.
        /// </summary>
        public int Width { get { return Game.Width; } }
        /// <summary>
        /// Gets the height of the game's window.
        /// </summary>
        public int Height { get { return Game.Height; } }
        /// <summary>
        /// Default offset for tooltips.
        /// </summary>
        private Point TooltipOffset { get { return new Point(16, 0); } }

        public GameUI(LegatusGame game, InputEventHandler input)
        {
            Game = game;
            Children = new List<UIElement>();
            input.KeyboardAction += OnKeyboardAction;
            input.MouseOver += OnMouseOver;
            input.MouseAction += OnMouseAction;
            input.MouseDrag += OnMouseDrag;
            input.MouseScroll += OnMouseScroll;
            game.Window.TextInput += OnTextInput;
        }

        public void Update(GameTime gameTime)
        {
            foreach (UIElement element in Children)
            {
                element.Update(gameTime);
            }
            Tooltip = null;
        }

        public void PostUpdate(GameTime gameTime)
        {
            Children.RemoveAll(e => e.Removing);

            if (TooltipFocus != null)
            {
                TooltipTimer -= gameTime.ElapsedGameTime.TotalSeconds;

                if (TooltipTimer < 0)
                    TooltipTimer = 0;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (UIElement element in Children)
            {
                element.Predraw(gameTime, spriteBatch);
            }

            Game.GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone);
            for (int i = Children.Count - 1; i >= 0; i--)
            {
                Children[i].Draw(gameTime, spriteBatch);
            }
            if (Tooltip != null)
                Tooltip.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }

        public void Add(UIElement element)
        {
            element.Parent = this;

            if (element.IsAlwaysAtBottom)
                Children.Add(element);
            else
                InsertToTop(element);

            if (element.CanTakeFocus)
                KeyboardFocus = element;
        }

        public void Add(params UIElement[] elements)
        {
            foreach (UIElement element in elements)
                Add(element);
        }

        public void Remove(UIElement element)
        {
            Children.Remove(element);
        }

        private void InsertToTop(UIElement element)
        {
            int i = 0;
            if ( ! element.IsAlwaysAtTop)
                foreach (UIElement e in Children)
                    if (e.IsAlwaysAtTop) i++; else break;

            Children.Insert(i, element);
        }

        private void OnKeyboardAction(object sender, KeyboardEventArgs keyboard)
        {
            if (KeyboardFocus != null)
                KeyboardFocus.OnKeyboardAction(keyboard);
        }

        private void OnTextInput(object sender, TextInputEventArgs e)
        {
            if (KeyboardFocus != null)
                KeyboardFocus.OnTextInput(e);
        }

        private void OnMouseOver(object sender, MouseOverEventArgs mouse)
        {
            bool nullifyTooltipFocus = true;
            UIElement interceptor = null;
            foreach (UIElement child in Children)
            {
                if ( ( interceptor = child.OnMouseOver(mouse) ) != null)
                {
                    if (interceptor.TakesFocusOnMouseOver)
                        KeyboardFocus = interceptor;

                    if (interceptor.Tooltip != null)
                    {
                        nullifyTooltipFocus = false;

                        if (interceptor == TooltipFocus)
                        {
                            if (TooltipTimer == 0)
                            {
                                Tooltip = interceptor.Tooltip;
                                Tooltip.MoveTo(mouse.Position + TooltipOffset);
                            }
                        }
                        else
                        {
                            TooltipFocus = interceptor;
                            TooltipTimer = interceptor.TooltipDelay;
                        }
                    }

                    break;
                }
            }

            if (nullifyTooltipFocus)
                TooltipFocus = null;
        }

        private void OnMouseAction(object sender, MouseActionEventArgs mouse)
        {
            UIElement interceptor = null;
            foreach (UIElement child in Children)
            {
                if ( ( interceptor = child.OnMouseAction(mouse) ) != null)
                {
                    if (mouse.Button == MouseButton.Left && mouse.Action == MouseAction.Pressed)
                    {
                        if ( ! child.IsAlwaysAtBottom)
                        {
                            Children.Remove(child);
                            InsertToTop(child);
                        }

                        if (interceptor.TakesFocusOnMouseAction)
                            KeyboardFocus = interceptor;
                    }

                    break;
                }
            }
        }

        private void OnMouseDrag(object sender, MouseDragEventArgs mouse)
        {
            foreach (UIElement child in Children)
            {
                if (child.OnMouseDrag(mouse) != null)
                {
                    break;
                }
            }
        }

        private void OnMouseScroll(object sender, MouseScrollEventArgs mouse)
        {
            foreach (UIElement child in Children)
            {
                if (child.OnMouseScroll(mouse) != null)
                {
                    break;
                }
            }
        }
    }
}
