using Legatus;
using Legatus.UI;
using Microsoft.Xna.Framework;
using Space.Data;
using System;

namespace Space.ShipDesigner
{
    public class ShipDesignerGameState : GameState
    {
        public ShipDesignerMap Map;

        public ShipDesignerGameState(SpaceGame game)
            : base (game)
        {
            game.Resized += OnResized;

            Map = new ShipDesignerMap(game.Data);
            ShipDesignerMapView mapView = new ShipDesignerMapView(Map);
            UIViewport mapViewport = new UIViewport(96, 0, UI.Width - 96, UI.Height, mapView, game);
            mapViewport.IsAlwaysAtBottom = true;
            ShipDesignerModuleView moduleView = new ShipDesignerModuleView(Map);
            UIViewport moduleViewport = new UIViewport(0, 0, 96, UI.Height, moduleView, game);
            moduleViewport.IsAlwaysAtBottom = true;

            SpriteData testSprite = game.Data.Sprites["TestIcon"];
            UIButtonSimple testButton = new UIButtonSimple(
                mapViewport.Bounds.Right - testSprite.Width * 4, mapViewport.Bounds.Bottom - testSprite.Height, testSprite.Width, testSprite.Height,
                Map.TestShip, testSprite.Base);
            //testButton.Tooltip = "Test the ship";

            SpriteData newSprite = game.Data.Sprites["NewIcon"];
            UIButtonSimple newButton = new UIButtonSimple(
                mapViewport.Bounds.Right - newSprite.Width * 3, mapViewport.Bounds.Bottom - newSprite.Height, newSprite.Width, newSprite.Height, 
                Map.NewShip, newSprite.Base);
            //newButton.Tooltip = "Create a new ship";

            SpriteData saveSprite = game.Data.Sprites["SaveIcon"];
            UIButtonSimple saveButton = new UIButtonSimple(
                mapViewport.Bounds.Right - saveSprite.Width * 2, mapViewport.Bounds.Bottom - saveSprite.Height, saveSprite.Width, saveSprite.Height, 
                Map.SaveShip, saveSprite.Base);
            //saveButton.Tooltip = "Save the current ship";

            SpriteData loadSprite = game.Data.Sprites["LoadIcon"];
            UIButtonSimple loadButton = new UIButtonSimple(
                mapViewport.Bounds.Right - loadSprite.Width, mapViewport.Bounds.Bottom - loadSprite.Height, loadSprite.Width, loadSprite.Height, 
                Map.LoadShip, loadSprite.Base);
            //loadButton.Tooltip = "Load a ship";

            UI.Add(testButton, newButton, saveButton, loadButton, moduleViewport, mapViewport);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Map.Update(gameTime);
        }

        public void OnResized(object sender, ResizeEventArgs e)
        {
            Console.WriteLine(UI.Width + ", " + UI.Height);
            Console.WriteLine(e.Width + ", " + e.Height);
        }
    }
}
