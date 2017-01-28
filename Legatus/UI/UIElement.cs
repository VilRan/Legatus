using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Legatus.Input;
using Legatus.Mathematics;
using System;

namespace Legatus.UI
{
    public abstract class UIElement : IUIBase
    {
        /// <summary>
        /// The element that is one level above in the hierarchy. Can be another element or the GameUI.
        /// </summary>
        public IUIBase Parent;
        /// <summary>
        /// Displayed on mouse over after a small delay.
        /// </summary>
        public UITooltip Tooltip;
        /// <summary>
        /// Color used when drawing the element.
        /// </summary>
        public Color Color = Color.White;
        /// <summary>
        /// Delay, in seconds, until the tooltip for the element is shown.
        /// </summary>
        public double TooltipDelay = 0.500;
        /// <summary>
        /// Desired width of the element, either in pixels or fraction of parent's width.
        /// </summary>
        public float DesiredWidth;
        /// <summary>
        /// Desired height of the element, either in pixels or fraction of parent's width.
        /// </summary>
        public float DesiredHeight;
        /// <summary>
        /// Determines whether the element's Width is in pixels or percentage.
        /// </summary>
        public DimensionType HeightType = DimensionType.Pixels;
        /// <summary>
        /// Determines whether the element's Height is in pixels or percentage.
        /// </summary>
        public DimensionType WidthType = DimensionType.Pixels;
        /// <summary>
        /// Offset from the X-coordinate of the parent. In pixels or percentage of parent's width.
        /// </summary>
        public float MarginLeft;
        /// <summary>
        /// Offset from the Y-coordinate of the parent. In pixels or percentage of parent's height.
        /// </summary>
        public float MarginTop;
        public float MarginRight;
        public float MarginBottom;
        public DimensionType MarginLeftType = DimensionType.Pixels;
        public DimensionType MarginTopType = DimensionType.Pixels;
        public DimensionType MarginRightType = DimensionType.Pixels;
        public DimensionType MarginBottomType = DimensionType.Pixels;
        public Alignment HorizontalAlign = Alignment.LeftOrTop;
        public Alignment VerticalAlign = Alignment.LeftOrTop;
        /// <summary>
        /// If true, the element will be removed in the next Update in GameUI.
        /// </summary>
        public bool Removing { private set; get; }
        /// <summary>
        /// If true, the element will not be brought to the top when clicked.
        /// </summary>
        public bool IsAlwaysAtBottom = false;
        /// <summary>
        /// If true, elements that are clicked won't be brought above this element.
        /// </summary>
        public bool IsAlwaysAtTop = false;
        /// <summary>
        /// If false, the element won't automatically take keyboard focus on mouse over.
        /// </summary>
        public bool TakesFocusOnMouseOver = false;
        /// <summary>
        /// If false, the element won't automatically take keyboard focus on mouse action.
        /// </summary>
        public bool TakesFocusOnMouseAction = false;


        public Point Position { get { return new Point(X, Y); } }
        public Vector2 PositionVector { get { return new Vector2(X, Y); } }
        public Point Center
        {
            get { return new Point(X + Width / 2, Y + Height / 2); }
            set { MoveTo(value - new Point(Width / 2, Height / 2)); }
        }
        public Rectangle Bounds
        {
            get { return new Rectangle(X, Y, Width, Height); }
            set { ResizeTo(value); }
        }
        /// <summary>
        /// The screen X-coordinate of the upper-left corner of this element.
        /// </summary>
        public int X
        {
            get
            {
                int x = 0;
                switch (MarginLeftType)
                {
                    case DimensionType.Percentage:
                        if (Parent != null)
                            x = Parent.X + (int)(Parent.Width * MarginLeft);
                        break;
                    default:
                        if (Parent != null)
                            x = Parent.X;
                        x += (int)MarginLeft;
                        break;
                }
                switch (HorizontalAlign)
                {
                    case Alignment.Center:
                        x -= Width / 2;
                        break;
                    case Alignment.RightOrBottom:
                        x -= Width;
                        break;
                }
                return x;
            }
        }
        /// <summary>
        /// The screen Y-coordinate of the upper-left corner of this element.
        /// </summary>
        public int Y
        {
            get
            {
                int y = 0;
                switch (MarginTopType)
                {
                    case DimensionType.Percentage:
                        if (Parent != null)
                            y = Parent.Y + (int)(Parent.Height * MarginTop);
                        break;
                    default:
                        if (Parent != null)
                            y = Parent.Y;
                        y += (int)MarginTop;
                        break;
                }
                switch (VerticalAlign)
                {
                    case Alignment.Center:
                        y -= Height / 2;
                        break;
                    case Alignment.RightOrBottom:
                        y -= Height;
                        break;
                }
                return y;
            }
        }
        public int Width
        {
            get
            {
                int width = 0;
                switch (WidthType)
                {
                    case DimensionType.Percentage:
                        width = (int)(Parent.Width * DesiredWidth);
                        break;
                    default:
                        width = (int)DesiredWidth;
                        break;
                }
                return width;
            }
        }
        public int Height
        {
            get
            {
                int height = 0;
                switch (HeightType)
                {
                    case DimensionType.Percentage:
                        height = (int)(Parent.Height * DesiredHeight);
                        break;
                    default:
                        height = (int)DesiredHeight;
                        break;
                }
                return height;
            }
        }
        /// <summary>
        /// Set all margins (left, right, top, bottom) to the given value.
        /// </summary>
        public int Margin { set { MarginLeft = MarginRight = MarginTop = MarginBottom = value; } }
        /// <summary>
        /// True if the element can take keyboard focus by mouse action or mouseover.
        /// </summary>
        public bool CanTakeFocus { get { return TakesFocusOnMouseAction || TakesFocusOnMouseOver; } }


        public UIElement(Rectangle bounds)
            : this(bounds.X, bounds.Y, bounds.Width, bounds.Height)
        {

        }

        public UIElement(int x, int y, int width, int height)
        {
            MarginLeft = x;
            MarginTop = y;
            DesiredWidth = width;
            DesiredHeight = height;
        }


        public void Remove()
        {
            Removing = true;
        }
        
        public void MoveTo(int x, int y)
        {
            MarginLeft = x;
            MarginTop = y;
        }
        public void MoveTo(Point newPosition)
        {
            MarginLeft = newPosition.X;
            MarginTop = newPosition.Y;
        }
        public void MoveBy(int x, int y)
        {
            MarginLeft += x;
            MarginTop += y;
        }
        public void MoveBy(Point delta)
        {
            MarginLeft += delta.X;
            MarginTop += delta.Y;
        }
        
        public void ResizeTo(Rectangle bounds)
        {
            ResizeTo(bounds.X, bounds.Y, bounds.Width, bounds.Height);
        }
        public void ResizeTo(int x, int y, int width, int height)
        {
            MoveTo(x, y);
            ResizeBy(new Point(width - Width, height - Height));
        }
        public void ResizeBy(int x, int y)
        {
            ResizeBy(new Point(x, y));
        }
        public void ResizeBy(Point delta)
        {
            DesiredWidth += delta.X;
            DesiredHeight += delta.Y;

            OnResize(delta);
        }
        public virtual void OnResize(Point delta) { }

        public virtual void Update(GameTime gameTime) { }
        public virtual void Predraw(GameTime gameTime, SpriteBatch spriteBatch) { }
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        public virtual void OnKeyboardAction(KeyboardEventArgs keyboard) { }
        public virtual void OnTextInput(TextInputEventArgs input) { }
        public abstract UIElement OnMouseOver(MouseOverEventArgs mouse);
        public abstract UIElement OnMouseAction(MouseActionEventArgs mouse);
        public abstract UIElement OnMouseDrag(MouseDragEventArgs mouse);
        public abstract UIElement OnMouseScroll(MouseScrollEventArgs mouse);
        
        public enum DimensionType : byte
        {
            Pixels,
            Percentage,
            Automatic
        }

        public enum Alignment : byte
        {
            LeftOrTop,
            Center,
            RightOrBottom
        }
    }
}
