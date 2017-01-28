using Legatus.Input;
using Legatus.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space.Data;
using Microsoft.Xna.Framework.Input;
using Space.Battle;

namespace Space.ShipDesigner
{
    public class ShipDesignerMapView : IUIView
    {
        private BattleCamera Camera = new BattleCamera();
        private ShipDesignerMap Map;
        private SpriteData GridSprite;
        private Point GridPosition = Point.Zero;
        private bool _ShouldDrawGrid = true;

        private BattleShip Ship { get { return Map.Ship; } }
        private ModuleData SelectedModule { set { Map.SelectedModule = value; } get { return Map.SelectedModule; } }
        private int GridSize { get { return Map.DataManager.GridSize; } }
        public bool ShouldDrawGrid { get { return _ShouldDrawGrid && Map.ActiveMode == ShipDesignerMap.Mode.Build; } }

        public ShipDesignerMapView(ShipDesignerMap map)
        {
            Map = map;
            GridSprite = Map.DataManager.Sprites["PlacementGrid"];
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Rectangle bounds)
        {
            int minX = - Camera.X / GridSize - 1;
            int minY = - Camera.Y / GridSize - 1;
            int maxX = (bounds.Width - Camera.X) / GridSize;
            int maxY = (bounds.Height - Camera.Y ) / GridSize;
            
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);

            if (ShouldDrawGrid)
                for (int x = minX; x <= maxX; x++)
                    for (int y = minY; y <= maxY; y++)
                        spriteBatch.Draw(GridSprite.Texture, new Vector2(x , y) * GridSize + Camera.PositionVector, GridSprite.SourceRectangle, Color.White * 0.5f);
            
            foreach (BattleModule module in Ship.Modules)
            {
                Vector2 origin = module.Sprite.Origin;
                Vector2 position = new Vector2(module.Position.X, module.Position.Y) * GridSize + origin + Camera.PositionVector;
                //position = position.ToPoint().ToVector2();
                float rotation = module.RotationRadians + Ship.HeadingRadians;
                spriteBatch.Draw(module.Sprite.Texture, position, module.Sprite.SourceRectangle, Color.White, rotation, origin, 1f, SpriteEffects.None, 0f);
                foreach (BattleTurret turret in module.Turrets)
                {
                    spriteBatch.Draw(turret.Sprite.Texture, position, turret.Sprite.SourceRectangle, Color.White, rotation, origin, 1f, SpriteEffects.None, 0f);
                }
            }

            if (Map.ActiveMode == ShipDesignerMap.Mode.Build && SelectedModule != null)
            {
                Vector2 origin = SelectedModule.Sprite.Origin;
                Vector2 position = new Vector2(GridPosition.X, GridPosition.Y) * GridSize + origin + Camera.PositionVector;
                float rotation = SelectedModule.Rotatable ? Map.ModuleRotationRadians : 0;
                spriteBatch.Draw(SelectedModule.Sprite.Texture, position, SelectedModule.Sprite.SourceRectangle, Color.White, rotation, origin, 1f, SpriteEffects.None, 0f);
                foreach (TurretData turret in SelectedModule.Turrets)
                {
                    spriteBatch.Draw(turret.Sprite.Texture, position, turret.Sprite.SourceRectangle, Color.White, rotation, origin, 1f, SpriteEffects.None, 0f);
                }
            }

            spriteBatch.End();
        }

        public void OnKeyboardAction(KeyboardEventArgs keyboard)
        {
            switch (keyboard.Action)
            {
                case KeyboardAction.Held:
                    if (Map.ActiveMode == ShipDesignerMap.Mode.Test)
                    {
                        switch (keyboard.Key)
                        {
                            case Keys.Up:
                                Ship.Velocity += Vector2.Transform(new Vector2(0, -0.1f), Matrix.CreateRotationZ(Ship.HeadingRadians));
                                break;
                            case Keys.Left:
                                Ship.AngularMomentum -= 1000f;
                                Ship.TurnAssist = false;
                                break;
                            case Keys.Right:
                                Ship.AngularMomentum += 1000f;
                                Ship.TurnAssist = false;
                                break;
                        }
                    }
                    break;
                case KeyboardAction.Released:
                    switch (Map.ActiveMode)
                    {
                        case ShipDesignerMap.Mode.Build:
                            switch (keyboard.Key)
                            {
                                case Keys.R:
                                    Map.ModuleRotation++;
                                    break;
                                case Keys.G:
                                    _ShouldDrawGrid = !_ShouldDrawGrid;
                                    break;
                            }
                            break;
                        case ShipDesignerMap.Mode.Test:
                            switch (keyboard.Key)
                            {
                                case Keys.Left:
                                    Ship.TurnAssist = true;
                                    break;
                                case Keys.Right:
                                    Ship.TurnAssist = true;
                                    break;
                            }
                            break;
                    }

                    break;
            }
        }

        public void OnMouseAction(MouseActionEventArgs mouse)
        {
            switch (mouse.Action)
            {
                case MouseAction.Released:

                    if (Map.ActiveMode == ShipDesignerMap.Mode.Build)
                    {
                        switch (mouse.Button)
                        {
                            case MouseButton.Left:
                                Ship.AddModule(SelectedModule, GridPosition, Map.ModuleRotation);
                                break;
                            case MouseButton.Right:
                                Ship.RemoveModule(GridPosition);
                                break;
                            case MouseButton.Middle:
                                BattleModule module = Ship.GetModuleAt(GridPosition);
                                if (module != null)
                                    SelectedModule = module.Data;
                                Map.ModuleRotation++;
                                break;
                        }
                    }
                    break;
            }
        }

        public void OnMouseDrag(MouseDragEventArgs mouse)
        {
            
            switch (mouse.Button)
            {
                case MouseButton.Left:
                    if (Map.ActiveMode == ShipDesignerMap.Mode.Build)
                        Ship.AddModule(SelectedModule, GridPosition, Map.ModuleRotation);
                    break;
                case MouseButton.Right:
                    if (Map.ActiveMode == ShipDesignerMap.Mode.Build)
                        Ship.RemoveModule(GridPosition);
                    //Camera.MoveBy(mouse.DeltaToStart.ToVector2() / 16f);
                    break;
                case MouseButton.Middle:
                    //Camera.MoveBy(mouse.DeltaFromPrevious.ToVector2());
                    break;
            }
        }

        public void OnMouseOver(MouseOverEventArgs mouse)
        {
            GridPosition = GetGridPosition(mouse.Position);
        }

        public void OnMouseScroll(MouseScrollEventArgs mouse)
        {

        }

        private Point GetGridPosition(Point screenPosition)
        {
            return new Point((screenPosition.X - Camera.X) / GridSize, (screenPosition.Y - Camera.Y) / GridSize);
        }
    }
}
