using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Legatus;
using Legatus.Input;
using Linguistics;
using Legatus.MapGeneration;

namespace TestProject.Testing
{
    internal class AITest : Test, IInputReceiver
    {
        private class MapObject
        {
            public int X, Y;

            public Vector2 ScreenPosition { get { return new Vector2(X * 32, Y * 32 + 32); } }
            public Vector2 PositionVector { get { return new Vector2(X, Y); } }
            public Point Position { get { return new Point(X, Y); } }

            public MapObject(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        private class Tile : MapObject
        {
            public enum TileType { Land, Sea }

            public TileType Type;
            public City City;
            public List<Unit> Units;
            public Faction Owner;
            public Point UnitOffset;

            public Color Color
            {
                get
                {
                    switch (Type)
                    {
                        case TileType.Land:
                            return Color.ForestGreen;
                        case TileType.Sea:
                            return Color.Navy;
                        default:
                            return Color.White;
                    }
                }
            }

            public Tile(int x, int y, TileType type)
                : base (x, y)
            {
                Type = type;
                Units = new List<Unit>();
            }
        }

        private class OwnableMapObject : MapObject
        {
            public Faction Owner;

            public OwnableMapObject(int x, int y, Faction owner)
                : base(x, y)
            {
                Owner = owner;
            }
        }

        private class City : OwnableMapObject
        {
            public static Rectangle OutlineSource = new Rectangle(0, 0, 28, 28);
            public static Rectangle TextureSource = new Rectangle(0, 0, 24, 24);

            public string Name;

            public Rectangle OutlineDestination { get { return new Rectangle(X * 32 + 2, Y * 32 + 2 + 32, 28, 28); } }
            public Rectangle TextureDestination { get { return new Rectangle(X * 32 + 4, Y * 32 + 4 + 32, 24, 24); } }

            public City(int x, int y, Faction owner, Random rng)
                : base (x, y, owner)
            {
                Name = owner.Language.GenerateName(rng);
            }
        }

        private class Unit : OwnableMapObject
        {
            public static Rectangle OutlineSource = new Rectangle(0, 0, 8, 8);
            public static Rectangle TextureSource = new Rectangle(0, 0, 7, 7);

            public Rectangle OutlineDestination { get { return new Rectangle(X * 32 + 0, Y * 32 + 0 + 32, 8, 8); } }
            public Rectangle TextureDestination { get { return new Rectangle(X * 32 + 1, Y * 32 + 1 + 32, 6, 6); } }

            public Unit(int x, int y, Faction owner)
                : base(x, y, owner)
            {

            }
        }

        private class Faction
        {
            public Color Color;
            public Language Language;
            public string Name;
            public bool AIControlled;

            public Faction(Random rng, ProceduralLanguageFactory languageFactory)
            {
                Color = new Color(rng.Next(255), rng.Next(255), rng.Next(255));
                //Language = new ListedLanguage("Linguistics/RandomNames.txt");
                Language = languageFactory.Generate();
                Name = Language.GenerateName(rng);
                AIControlled = true;
            }
        }

        private ProceduralLanguageFactory LanguageFactory;
        private Tile[,] Tiles;
        private List<Faction> Factions;
        private List<City> Cities;
        private List<Unit> Units;
        private Faction ActiveFaction;
        private City CityInfo;
        private Random RNG;
        private Texture2D TileTexture;
        private SpriteFont Font;
        private InputReceiverHandler Input;
        private int Turn = 1;

        private Rectangle Bounds { get { return new Rectangle(0, 0, Width, Height); } }
        private int Width { get { return Tiles.GetLength(0); } }
        private int Height { get { return Tiles.GetLength(1); } }

        public AITest(LegatusGame game, int width, int height)
        {
            RNG = new Random();
            LanguageFactory = new ProceduralLanguageFactory(RNG);
            TileTexture = game.Content.Load<Texture2D>("Graphics/WhiteTile.png");
            Font = game.Content.Load<SpriteFont>("Graphics/DefaultFont12");
            Input = new InputReceiverHandler(this);
            Factions = new List<Faction>();
            Cities = new List<City>();
            Units = new List<Unit>();
            Generate(width, height);
        }
        public void Generate(int width, int height)
        {
            GenerateTerrain(width, height);
            GenerateFactions();
        }

        private void GenerateTerrain(int width, int height)
        {
            Tiles = new Tile[width, height];
            double[,] heightmap = new DiamondSquare(RNG, Width, Height, 1, 0.5, false, false).Heightmap;
            double[,] detailmap = new DiamondSquare(RNG, Width, Height, 1, 0.65, false, false).Heightmap;
            double heightmapWidth = heightmap.GetUpperBound(0);
            double heightmapHeight = heightmap.GetUpperBound(1);
            double widthRatio = heightmapWidth / (double)Width;
            double heightRatio = heightmapHeight / (double)Height;
            //int baseHeight = 0; //(int)(-planet.Radius / 10);
            int altitudeModifier = 10000; //(int)(planet.Radius / 2);
            int sealevel = 0;// RNG.Next(-10000, 10000);
            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                int x2 = (int)Math.Round((x) * widthRatio);
                int y2 = (int)Math.Round((y) * heightRatio);
                double altitude = (heightmap[x, y] + detailmap[x, y]) * altitudeModifier;
                if (altitude > sealevel)
                    Tiles[x, y] = new Tile(x, y, Tile.TileType.Land);
                else
                    Tiles[x, y] = new Tile(x, y, Tile.TileType.Sea);
            }
        }

        private void GenerateFactions()
        {
            ClearMap();

            for (int i = 0; i < RNG.Next(2, 9); i++)
            {
                Faction faction = new Faction(RNG, LanguageFactory);
                City city = null;
                int repeat = 10;
                do
                {
                    int x = RNG.Next(Width);
                    int y = RNG.Next(Height);
                    Tile tile = Tiles[x, y];
                    if (tile.Type == Tile.TileType.Land && tile.City == null && !Cities.Exists(c => Vector2.DistanceSquared(c.PositionVector, tile.PositionVector) <= 25f))
                    {
                        city = new City(x, y, faction, RNG);
                        tile.City = city;
                        ClaimTerritory(city.Position, 2.5f, faction);
                        Factions.Add(faction);
                        Cities.Add(city);
                        AddUnit(new Unit(x, y, faction), tile);
                        AddUnit(new Unit(x, y, faction), tile);
                        AddUnit(new Unit(x, y, faction), tile);
                        AddUnit(new Unit(x, y, faction), tile);
                    }

                    repeat--;
                }
                while (repeat > 0 && city == null);
            }
            if (Factions.Count > 0)
                ActiveFaction = Factions[0];
        }

        private void ClaimTerritory(Point position, float radius, Faction faction)
        {
            int minX = Math.Max(0, (int)Math.Round(position.X - radius));
            int minY = Math.Max(0, (int)Math.Round(position.Y - radius));
            int maxX = Math.Min(Width - 1, (int)Math.Round(position.X + radius));
            int maxY = Math.Min(Height - 1, (int)Math.Round(position.Y + radius));

            float rSquared = radius * radius;
            for (int x = minX; x <= maxX; x++)
            for (int y = minY; y <= maxY; y++)
            {
                int dx = position.X - x;
                int dy = position.Y - y;
                float dSquared = dx * dx + dy * dy;

                if (dSquared <= rSquared)
                {
                    Tile tile = Tiles[x, y];
                    if (tile.City == null || tile.City.Owner == faction)
                        tile.Owner = faction;
                }
            }
        }

        private void ClearMap()
        {
            foreach (Tile tile in Tiles)
            {
                tile.Owner = null;
                tile.City = null;
                tile.Units.Clear();
            }

            Cities.Clear();
            Units.Clear();
            Factions.Clear();
            ActiveFaction = null;
            CityInfo = null;
        }

        private void AddUnit(Unit unit, Tile tile)
        {
            Units.Add(unit);
            tile.Units.Add(unit);
        }

        private void MoveUnit(Unit unit, Tile tile)
        {
            Tiles[unit.X, unit.Y].Units.Remove(unit);
            tile.Units.Add(unit);
        }

        private void EndTurn()
        {
            if (ActiveFaction == null)
            {
                Turn++;
            }
            else if (Factions.IndexOf(ActiveFaction) == Factions.Count - 1)
            {
                ActiveFaction = Factions[0];
                Turn++;
            }
            else
            {
                ActiveFaction = Factions[Factions.IndexOf(ActiveFaction) + 1];
            }
        }

        public override void Update(GameTime gameTime)
        {
            Input.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (Tile tile in Tiles)
            {
                spriteBatch.Draw(TileTexture, tile.ScreenPosition, tile.Color);
                if (tile.Type == Tile.TileType.Land && tile.Owner != null)
                    spriteBatch.Draw(TileTexture, tile.ScreenPosition, tile.Owner.Color * 0.5f);
                tile.UnitOffset = Point.Zero;
            }
            foreach (City city in Cities)
            {
                spriteBatch.Draw(TileTexture, city.OutlineDestination, City.OutlineSource, Color.Black);
                spriteBatch.Draw(TileTexture, city.TextureDestination, City.TextureSource, city.Owner.Color);
            }
            foreach (Unit unit in Units)
            {
                Tile tile = Tiles[unit.X, unit.Y];
                Rectangle textureDestination = unit.OutlineDestination;
                textureDestination.Offset(tile.UnitOffset);
                spriteBatch.Draw(TileTexture, textureDestination, Unit.OutlineSource, Color.Black);
                textureDestination = unit.TextureDestination;
                textureDestination.Offset(tile.UnitOffset);
                spriteBatch.Draw(TileTexture, textureDestination, Unit.TextureSource, unit.Owner.Color);
                tile.UnitOffset += new Point(8, 0);
                if (tile.UnitOffset.X >= 32)
                    tile.UnitOffset = new Point(0, tile.UnitOffset.Y + 8);
            }

            Vector2 textPosition = Vector2.Zero;
            Vector2 textOffset = new Vector2(0, 16);
            if (ActiveFaction != null)
            {
                spriteBatch.DrawString(Font, "Turn: " + Turn + " - " + ActiveFaction.Name, textPosition, ActiveFaction.Color); textPosition += textOffset;
            }
            if (CityInfo != null)
            {
                spriteBatch.DrawString(Font, "City: " + CityInfo.Name, textPosition, CityInfo.Owner.Color); textPosition += textOffset;
            }

            spriteBatch.End();
        }

        public void OnKeyPressed(Keys pressedKey, Keys[] allHeldKeys)
        {
            switch (pressedKey)
            {
                case Keys.R:
                    Generate(Width, Height);
                    break;
                case Keys.F:
                    GenerateFactions();
                    break;
                case Keys.Enter:
                    EndTurn();
                    break;
            }
        }
        public void OnKeyRepeated(Keys repeatedKey, Keys[] allHeldKeys)
        {

        }
        public void OnKeyHeld(Keys heldKey, Keys[] allHeldKeys)
        {

        }
        public void OnKeyReleased(Keys releasedKey, Keys[] allHeldKeys)
        {

        }
        public void OnMouseOver(Point mousePosition, Keys[] allHeldKeys)
        {
            Point position = new Point(mousePosition.X / 32, mousePosition.Y / 32 - 1);
            if (Bounds.Contains(position))
            {
                CityInfo = Tiles[position.X, position.Y].City;
            }
        }
        public void OnMouseScroll(Point mousePosition, int delta, Keys[] allHeldKeys)
        {

        }
        public void OnMousePressed(MouseButton button, Point mousePosition, Keys[] allHeldKeys)
        {

        }
        public void OnMouseReleased(MouseButton button, Point mousePosition, Keys[] allHeldKeys)
        {

        }
        public void OnMouseDoubleClick(MouseButton button, Point mousePosition, Keys[] allHeldKeys)
        {

        }
        public void OnMouseDrag(MouseButton button, Point mousePosition, Point previousPosition, Point startPosition, Keys[] allHeldKeys)
        {

        }
    }
}
