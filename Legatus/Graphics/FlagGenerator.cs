using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Legatus.Graphics
{
    public class FlagGenerator
    {
        public FlagGenerator()
        {

        }

        public Texture2D GenerateRandom(GraphicsDevice graphicsDevice, int width, int height, Random rng)
        {
            int rn = rng.Next(600);

            if (rn < 100)
                return GenerateUnicolor(graphicsDevice, width, height, rng);
            else if (rn < 200)
                return GenerateVerticalBicolor(graphicsDevice, width, height, rng);
            else if (rn < 300)
                return GenerateVerticalTricolor(graphicsDevice, width, height, rng);
            else if (rn < 400)
                return GenerateHorizontalBicolor(graphicsDevice, width, height, rng);
            else if (rn < 500)
                return GenerateHorizontalTricolor(graphicsDevice, width, height, rng);
            else if (rn < 550)
                return GenerateCentralizedCross(graphicsDevice, width, height, rng);
            else
                return GenerateNordicCross(graphicsDevice, width, height, rng);

        }

        public Texture2D GenerateUnicolor(GraphicsDevice graphicsDevice, int width, int height, Random rng)
        {
            Texture2D flag = new Texture2D(graphicsDevice, width, height);
            Color[] data = new Color[flag.Width * flag.Height];
            Color primary = new Color(rng.Next(256), rng.Next(256), rng.Next(256));

            for (int y = 0; y < flag.Height; y++)
                for (int x = 0; x < flag.Width; x++)
                {
                    data[y * flag.Width + x] = primary;
                }

            flag.SetData<Color>(data);
            return flag;
        }

        public Texture2D GenerateVerticalBicolor(GraphicsDevice graphicsDevice, int width, int height, Random rng)
        {
            Texture2D flag = new Texture2D(graphicsDevice, width, height);
            Color[] data = new Color[flag.Width * flag.Height];
            Color primary = new Color(rng.Next(256), rng.Next(256), rng.Next(256));
            Color secondary = new Color(rng.Next(256), rng.Next(256), rng.Next(256));
            Vector3 difference = primary.ToVector3() - secondary.ToVector3();
            while (difference.Length() < 0.5f)
            {
                secondary = new Color(rng.Next(256), rng.Next(256), rng.Next(256));
                difference = primary.ToVector3() - secondary.ToVector3();
            }

            int secondaryLine = width / 2;

            for (int y = 0; y < flag.Height; y++)
                for (int x = 0; x < flag.Width; x++)
                {
                    if (x < secondaryLine)
                    {
                        data[y * flag.Width + x] = primary;
                    }
                    else
                    {
                        data[y * flag.Width + x] = secondary;
                    }
                }

            flag.SetData<Color>(data);
            return flag;
        }

        public Texture2D GenerateVerticalTricolor(GraphicsDevice graphicsDevice, int width, int height, Random rng)
        {
            Texture2D flag = new Texture2D(graphicsDevice, width, height);
            Color[] data = new Color[flag.Width * flag.Height];
            Color primary = new Color(rng.Next(256), rng.Next(256), rng.Next(256));
            Color secondary = new Color(rng.Next(256), rng.Next(256), rng.Next(256));
            Vector3 difference = primary.ToVector3() - secondary.ToVector3();
            while (difference.Length() < 0.5f)
            {
                secondary = new Color(rng.Next(256), rng.Next(256), rng.Next(256));
                difference = primary.ToVector3() - secondary.ToVector3();
            }
            Color tertiary = new Color(rng.Next(256), rng.Next(256), rng.Next(256));
            difference = secondary.ToVector3() - tertiary.ToVector3();
            while (difference.Length() < 0.5f)
            {
                tertiary = new Color(rng.Next(256), rng.Next(256), rng.Next(256));
                difference = secondary.ToVector3() - tertiary.ToVector3();
            }

            int secondaryLine = width / 3;
            int tertiaryLine = width * 2 / 3;

            for (int y = 0; y < flag.Height; y++)
                for (int x = 0; x < flag.Width; x++)
                {
                    if (x < secondaryLine)
                    {
                        data[y * flag.Width + x] = primary;
                    }
                    else if (x < tertiaryLine)
                    {
                        data[y * flag.Width + x] = secondary;
                    }
                    else
                    {
                        data[y * flag.Width + x] = tertiary;
                    }
                }

            flag.SetData<Color>(data);
            return flag;
        }

        public Texture2D GenerateHorizontalBicolor(GraphicsDevice graphicsDevice, int width, int height, Random rng)
        {
            Texture2D flag = new Texture2D(graphicsDevice, width, height);
            Color[] data = new Color[flag.Width * flag.Height];
            Color primary = new Color(rng.Next(256), rng.Next(256), rng.Next(256));
            Color secondary = new Color(rng.Next(256), rng.Next(256), rng.Next(256));
            Color tertiary = new Color(rng.Next(256), rng.Next(256), rng.Next(256));

            int secondaryLine = height / 2;

            for (int y = 0; y < flag.Height; y++)
                for (int x = 0; x < flag.Width; x++)
                {
                    if (y < secondaryLine)
                    {
                        data[y * flag.Width + x] = primary;
                    }
                    else
                    {
                        data[y * flag.Width + x] = tertiary;
                    }
                }

            flag.SetData<Color>(data);
            return flag;
        }

        public Texture2D GenerateHorizontalTricolor(GraphicsDevice graphicsDevice, int width, int height, Random rng)
        {
            Texture2D flag = new Texture2D(graphicsDevice, width, height);
            Color[] data = new Color[flag.Width * flag.Height];
            Color primary = new Color(rng.Next(256), rng.Next(256), rng.Next(256));
            Color secondary = new Color(rng.Next(256), rng.Next(256), rng.Next(256));
            Color tertiary = new Color(rng.Next(256), rng.Next(256), rng.Next(256));

            int secondaryLine = height / 3;
            int tertiaryLine = height * 2 / 3;

            for (int y = 0; y < flag.Height; y++)
                for (int x = 0; x < flag.Width; x++)
                {
                    if (y < secondaryLine)
                    {
                        data[y * flag.Width + x] = primary;
                    }
                    else if (y < tertiaryLine)
                    {
                        data[y * flag.Width + x] = secondary;
                    }
                    else
                    {
                        data[y * flag.Width + x] = tertiary;
                    }
                }

            flag.SetData<Color>(data);
            return flag;
        }

        public Texture2D GenerateCentralizedCross(GraphicsDevice graphicsDevice, int width, int height, Random rng)
        {
            Texture2D flag = new Texture2D(graphicsDevice, width, height);
            Color[] data = new Color[flag.Width * flag.Height];
            Color primary = new Color(rng.Next(256), rng.Next(256), rng.Next(256));
            Color secondary = new Color(rng.Next(256), rng.Next(256), rng.Next(256));

            int secondaryVerticalLine = width / 2 - height / 8;
            int tertiaryVerticalLine = width / 2 + height / 8;
            int secondaryHorizontalLine = height * 3 / 8;
            int tertiaryHorizontalLine = height * 5 / 8;

            for (int y = 0; y < flag.Height; y++)
                for (int x = 0; x < flag.Width; x++)
                {
                    if ((x >= secondaryVerticalLine && x < tertiaryVerticalLine) || (y >= secondaryHorizontalLine && y < tertiaryHorizontalLine))
                    {
                        data[y * flag.Width + x] = primary;
                    }
                    else
                    {
                        data[y * flag.Width + x] = secondary;
                    }
                }

            flag.SetData<Color>(data);
            return flag;
        }

        public Texture2D GenerateNordicCross(GraphicsDevice graphicsDevice, int width, int height, Random rng)
        {
            Texture2D flag = new Texture2D(graphicsDevice, width, height);
            Color[] data = new Color[flag.Width * flag.Height];
            Color primary = new Color(rng.Next(256), rng.Next(256), rng.Next(256));
            Color secondary = new Color(rng.Next(256), rng.Next(256), rng.Next(256));

            int secondaryVerticalLine = width / 3 - height / 8;
            int tertiaryVerticalLine = width / 3 + height / 8;
            int secondaryHorizontalLine = height * 3 / 8;
            int tertiaryHorizontalLine = height * 5 / 8;

            for (int y = 0; y < flag.Height; y++)
                for (int x = 0; x < flag.Width; x++)
                {
                    if ((x >= secondaryVerticalLine && x < tertiaryVerticalLine) || (y >= secondaryHorizontalLine && y < tertiaryHorizontalLine))
                    {
                        data[y * flag.Width + x] = primary;
                    }
                    else
                    {
                        data[y * flag.Width + x] = secondary;
                    }
                }

            flag.SetData<Color>(data);
            return flag;
        }

        public Texture2D GenerateNoisy(GraphicsDevice graphicsDevice, int width, int height, Random rng)
        {
            Texture2D flag = new Texture2D(graphicsDevice, width, height);
            Color[] data = new Color[flag.Width * flag.Height];

            for (int y = 0; y < flag.Height; y++)
                for (int x = 0; x < flag.Width; x++)
                {
                    data[x + y * flag.Width] = new Color(rng.Next(256), rng.Next(256), rng.Next(256));
                }

            flag.SetData<Color>(data);
            return flag;
        }
    }
}
