namespace Legatus.Pathfinding
{
    public abstract class PathfinderLink
    {
        /// <summary>
        /// The node at the other end of the link.
        /// </summary>
        public readonly PathfinderNode Target;

        public PathfinderLink(PathfinderNode target)
        {
            Target = target;
        }

        /// <summary>
        /// Gets the link's cost for the specified agent.
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        public abstract double GetCost(IPathfinderAgent agent);
    }

    public class SimpleLink : PathfinderLink
    {
        /// <summary>
        /// Cost of traveling from the origin to the target.
        /// </summary>
        public double Cost;

        public SimpleLink(PathfinderNode target, double cost)
            : base(target)
        {
            Cost = cost;
        }

        public override double GetCost(IPathfinderAgent agent)
        {
            return Cost;
        }
    }
}
