using MazeProject.Core;
using MazeProject.Interfaces;
using System.Collections.Generic;

namespace MazeProject.Algorithms;

public class AStarSolver : IMazeSolver
{
    public int NodesExplored { get; private set; }
    public string AlgorithmName { get; } = "AStarSolver";
    public List<(int x, int y)> Solve(Maze maze, (int x, int y) start, (int x, int y) end)
    {
        NodesExplored = 0;
        
        // PriorityQueue stores the (x,y) coordinate and uses the 'f' score as priority
        var frontier = new PriorityQueue<(int x, int y), int>();
        frontier.Enqueue(start, 0);

        var parents = new Dictionary<(int x, int y), (int x, int y)>();
        var gScore = new Dictionary<(int x, int y), int>();
        
        parents[start] = start;
        gScore[start] = 0;

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();
            NodesExplored++;

            if (current == end) return ReconstructPath(parents, end);

            // ... Check neighbors ...
                var directions = new (int dx, int dy, WallState wall)[]
            {
                (0, -1, WallState.North),
                (0, 1,  WallState.South),
                (1, 0,  WallState.East),
                (-1, 0, WallState.West)
            };

             int tentativeGScore = gScore[current] + 1;
            foreach (var (dx, dy, wall) in directions)
            {
                // Inside the foreach loop...
                int nx = current.x + dx;
                int ny = current.y + dy;
                var neighbor = (x: nx, y: ny);

                if (maze.IsInBounds(nx, ny) && !maze.HasWall(current.x, current.y, wall))
                {
                    int tentativeG = gScore[current] + 1;

                    // If we haven't seen this neighbor OR we found a shorter way to get there
                    if (!gScore.ContainsKey(neighbor) || tentativeG < gScore[neighbor])
                    {
                        // 1. Update the 'Hard Facts'
                        gScore[neighbor] = tentativeG;
                        
                        // 2. Calculate the 'Total Estimate' (f = g + h)
                        int h = GetManhattanDistance(neighbor, end);
                        int f = tentativeG + h;

                        // 3. Update parent and queue
                        parents[neighbor] = current;
                        frontier.Enqueue(neighbor, f);
                    }
                }
                                
            }
        }
        return new List<(int x, int y)>();
    }

    private int GetManhattanDistance((int x, int y) a, (int x, int y) b)
    {
        return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
    }

     private List<(int x, int y)> ReconstructPath(Dictionary<(int x, int y), (int x, int y)> parents, (int x, int y) end)
{
        var path = new List<(int x, int y)>();
        var current = end;

        // Follow the breadcrumbs back to the start
        while (parents.ContainsKey(current))
        {
            path.Add(current);
            
            // If we reached the start (which is its own parent), stop!
            if (parents[current] == current) break;
            
            current = parents[current];
        }

        path.Reverse(); // Since we went end -> start, we flip it to get start -> end
        return path;
}
}