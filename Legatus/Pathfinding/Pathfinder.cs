using System.Collections.Generic;

namespace Legatus.Pathfinding
{
    /// <summary>
    /// A generic pathfinder that supports arbitrary graphs.
    /// </summary>
    public static class Pathfinder
    {
        /// <summary>
        /// Used for checking if a node needs to be reset.
        /// </summary>
        private static ulong CurrentRun = 0;

        /// <summary>
        /// Calculates the lowest cost path with A* algorithm and returns the path. If no path is found, an empty list is returned.
        /// </summary>
        /// <param name="start">The node from which the path starts.</param>
        /// <param name="end">The destination of the path.</param>
        /// <param name="agent">The agent trying to find the path. Can be used to modify the cost of links.</param>
        /// <param name="maximumSearchDepth">How many loops the pathfinder will go through before terminating.</param>
        /// <returns></returns>
        public static IEnumerable<PathfinderNode> FindPath(PathfinderNode start, PathfinderNode end, int maximumSearchDepth = int.MaxValue, IPathfinderAgent agent = null)
        {
            if (start == end)
                return new List<PathfinderNode>();
            
            CurrentRun++;
            
            var open = new BinaryHeap<PathfinderNode>();
            start.LastVisit = CurrentRun;
            start.PathCost = 0;
            start.Previous = null;
            open.Add(start);
            
            for (int i = 0; open.Count > 0 && i < maximumSearchDepth; i++)
            {
                PathfinderNode active = open.Remove();
                active.Status = PathfinderNodeStatus.Closed;

                foreach (PathfinderLink link in active.GetNeighbors())
                {
                    PathfinderNode neighbor = link.Target;

                    if (neighbor == end)
                    {
                        var path = new LinkedList<PathfinderNode>();
                        end.Previous = active;
                        active = end;
                        while (active != null)
                        {
                            path.AddFirst(active);
                            active = active.Previous;
                        }
                        return path;
                    }

                    if (neighbor.LastVisit != CurrentRun) // Reset nodes that haven't been visited yet this run.
                    {
                        neighbor.Status = PathfinderNodeStatus.Unvisited;
                        neighbor.LastVisit = CurrentRun;
                    }

                    if ( ! neighbor.IsClosed)
                    {
                        double cost = active.PathCost + link.GetCost(agent);

                        if ( ! neighbor.IsOpen)
                        {
                            neighbor.Previous = active;
                            neighbor.PathCost = cost;
                            neighbor.Heuristic = neighbor.CalculateHeuristic(end);
                            neighbor.Status = PathfinderNodeStatus.Open;
                            open.Add(neighbor);
                        }
                        else if (cost < neighbor.PathCost)
                        {
                            neighbor.Previous = active;
                            neighbor.PathCost = cost;
                        }
                    }
                }
            }
            
            return new List<PathfinderNode>();
        }

        /// <summary>
        /// Uses Dijkstra's algorithm to find and return all nodes whose total cost is below the specified number.
        /// </summary>
        /// <param name="start">From where the pathfinding should start.</param>
        /// <param name="range">How many PathfinderLink.Cost-units away the tiles can be.</param>
        /// <returns></returns>
        public static List<PathfinderNode> FindNodesWithinRange(PathfinderNode start, int range, IPathfinderAgent agent = null)
        {
            CurrentRun++;

            var closed = new List<PathfinderNode>();
            var open = new BinaryHeap<PathfinderNode>();
            start.LastVisit = CurrentRun;
            start.PathCost = 0;
            open.Add(start);
            
            while (open.Count > 0)
            {
                PathfinderNode active = open.Remove();
                closed.Add(active);
                active.Status = PathfinderNodeStatus.Closed;

                foreach (PathfinderLink link in active.GetNeighbors())
                {
                    PathfinderNode neighbor = link.Target;
                    
                    if (neighbor.LastVisit != CurrentRun) // Reset nodes that haven't been visited yet this run.
                    {
                        neighbor.Status = PathfinderNodeStatus.Unvisited;
                        neighbor.LastVisit = CurrentRun;
                    }

                    if ( ! neighbor.IsClosed)
                    {
                        double cost = active.PathCost + link.GetCost(agent);

                        if (cost <= range)
                        {
                            if ( ! neighbor.IsOpen)
                            {
                                neighbor.PathCost = cost;
                                neighbor.Status = PathfinderNodeStatus.Open;
                                open.Add(neighbor);
                            }
                            else if (cost < neighbor.PathCost)
                            {
                                neighbor.PathCost = cost;
                            }
                        }
                    }
                }
            }
            
            return closed;
        }

        /// <summary>
        /// Returns all nodes visited while finding path to the end node. Can be used for debugging.
        /// </summary>
        /// <param name="start">The node from which the path starts.</param>
        /// <param name="end">The destination of the path.</param>
        /// <param name="agent">The agent trying to find the path. Can be used to modify the cost of links.</param>
        /// <param name="maximumSearchDepth">How many loops the pathfinder will go through before terminating.</param>
        /// <returns></returns>
        public static IEnumerable<PathfinderNode> FindPathDebug(PathfinderNode start, PathfinderNode end, int maximumSearchDepth = int.MaxValue, IPathfinderAgent agent = null)
        {
            if (start == end)
                return new List<PathfinderNode>();

            CurrentRun++;

            var closed = new List<PathfinderNode>();
            var open = new BinaryHeap<PathfinderNode>();
            start.LastVisit = CurrentRun;
            start.PathCost = 0;
            start.Previous = null;
            open.Add(start);

            for (int i = 0; open.Count > 0 && i < maximumSearchDepth; i++)
            {
                PathfinderNode active = open.Remove();
                active.Status = PathfinderNodeStatus.Closed;
                closed.Add(active);

                foreach (PathfinderLink link in active.GetNeighbors())
                {
                    PathfinderNode neighbor = link.Target;

                    if (neighbor == end)
                    {
                        closed.Add(end);
                        return closed;
                    }

                    if (neighbor.LastVisit != CurrentRun) // Reset nodes that haven't been visited yet this run.
                    {
                        neighbor.Status = PathfinderNodeStatus.Unvisited;
                        neighbor.LastVisit = CurrentRun;
                    }

                    if (!neighbor.IsClosed)
                    {
                        double cost = active.PathCost + link.GetCost(agent);

                        if (!neighbor.IsOpen)
                        {
                            neighbor.Previous = active;
                            neighbor.PathCost = cost;
                            neighbor.Heuristic = neighbor.CalculateHeuristic(end);
                            neighbor.Status = PathfinderNodeStatus.Open;
                            open.Add(neighbor);
                        }
                        else if (cost < neighbor.PathCost)
                        {
                            neighbor.Previous = active;
                            neighbor.PathCost = cost;
                        }
                    }
                }
            }

            return new List<PathfinderNode>();
        }

        /*
        /// <summary>
        /// The older algorithm using a closed list. Testing proved it about 1/3 slower.
        /// Calculates the lowest cost path with A* algorithm and returns the path. If no path is found, an empty list is returned.
        /// </summary>
        /// <param name="start">The node from which the path starts.</param>
        /// <param name="end">The destination of the path.</param>
        /// <param name="agent">The agent trying to find the path. Can be used to modify the cost of links.</param>
        /// <param name="maximumSearchDepth">How many loops the pathfinder will go through before terminating.</param>
        /// <returns></returns>
        public static List<PathfinderNode> FindPathOld(PathfinderNode start, PathfinderNode end, int maximumSearchDepth = int.MaxValue, IPathfinderAgent agent = null)
        {
            if (start == end)
                return new List<PathfinderNode>();

            List<PathfinderNode> closed = new List<PathfinderNode>(maximumSearchDepth);
            BinaryHeap<PathfinderNode> open = new BinaryHeap<PathfinderNode>();
            start.PathCost = 0;
            start.Previous = null;
            open.Add(start);

            PathfinderNode active, neighbor;
            for (int i = 0; open.Count > 0 && i < maximumSearchDepth; i++)
            {
                active = open.Remove();
                closed.Add(active);
                active.Status = PathfinderNodeStatus.Closed;

                foreach (PathfinderLink link in active.Neighbors)
                {
                    neighbor = link.Target;

                    if (neighbor == end)
                    {
                        ResetNodes(open.Union(closed));
                        //return closed; //For testing.

                        LinkedList<PathfinderNode> path = new LinkedList<PathfinderNode>();
                        end.Previous = active;
                        active = end;
                        while (true)
                        {
                            path.AddFirst(active);
                            if (active.Previous == null)
                                return path.ToList();
                            active = active.Previous;
                        }
                    }

                    if (!neighbor.IsClosed)
                    {
                        int cost = active.PathCost + link.GetCost(agent);

                        if (!neighbor.IsOpen)
                        {
                            neighbor.Previous = active;
                            neighbor.PathCost = cost;
                            neighbor.Heuristic = neighbor.CalculateHeuristic(end);
                            neighbor.Status = PathfinderNodeStatus.Open;
                            open.Add(neighbor);
                        }
                        else if (cost < neighbor.PathCost)
                        {
                            neighbor.Previous = active;
                            neighbor.PathCost = cost;
                        }
                    }
                }
            }

            ResetNodes(open.Union(closed));
            return new List<PathfinderNode>();
        }

        /// <summary>
        /// Must be called before returning for the pathfinding to work next time.
        /// </summary>
        /// <param name="nodes"></param>
        private static void ResetNodes(IEnumerable<PathfinderNode> nodes)
        {
            foreach (PathfinderNode node in nodes)
            {
                node.Status = PathfinderNodeStatus.Unvisited;
            }
        }
        */

    }
}
