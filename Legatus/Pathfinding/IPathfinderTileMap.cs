namespace Legatus.Pathfinding
{
    public interface IPathfinderTileMap
    {
        PathfinderTile[,] TileGrid { get; }
        int SizeX { get; }
        int SizeY { get; }
        bool WrapsX { get; }
        bool WrapsY { get; }
    }
}
