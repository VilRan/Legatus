using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Legatus.UI
{
    public class UIText : UIActive
    {
        /// <summary>
        /// SpriteFont used to draw the text.
        /// </summary>
        public SpriteFont Font;
        /// <summary>
        /// Maximum number of characters in the string.
        /// </summary>
        public int MaxLength = int.MaxValue;
        /// <summary>
        /// If true, all characters are drawn as '*'.
        /// </summary>
        public bool IsPassword = false;
        private string _String = "";

        public string String
        {
            get
            {
                return _String;
            }
            set
            {
                _String = value;
                if (_String.Length > MaxLength)
                    _String = _String.Remove(MaxLength);
            }
        }
        
        public UIText(int x, int y, int width, int height, SpriteFont font, Color color, string text = "")
            : base(x, y, width, height)
        {
            Font = font;
            _String = text;
            Color = color;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Font == null)
                return;

            Vector2 size = Font.MeasureString(String);
            if ( ! IsPassword && size.X < Width && size.Y < Height)
                spriteBatch.DrawString(Font, String, new Vector2(X, Y), Color);
            else
            {
                Vector2 position = new Vector2(X, Y);
                foreach (char c in String)
                {
                    if (position.Y > Y + Height - Font.LineSpacing)
                        break;

                    if (c == '\n')
                        position = new Vector2(X, position.Y + Font.LineSpacing);
                    else
                    {
                        string s = "" + (IsPassword ? '*' : c);
                        spriteBatch.DrawString(Font, s, position, Color);
                        position += new Vector2(Font.MeasureString(s).X, 0);

                        if (position.X > X + Width - Font.LineSpacing)
                            position = new Vector2(X, position.Y + Font.LineSpacing);
                    }
                }
            }
        }
    }
}
