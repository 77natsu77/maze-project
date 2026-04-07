using MazeProject.Core;
using MazeProject.Interfaces;
using System.Collections.Generic;

namespace MazeProject.Algorithms;

public class BreadthFirstSolver : IMazeSolver
{
    public string AlgorithmName {get;} = "BreadthFirstSolver";
    public int NodesExplored {get; private set;}
    public List<(int x, int y)> Solve(Maze maze, (int x, int y) start, (int x, int y) end)
    {
        NodesExplored = 0;


        // 1. The Queue: Cells we need to explore
        Queue<(int x, int y)> frontier = new Queue<(int x, int y)>();
        frontier.Enqueue(start);

        // 2. The 'Breadcrumbs': Dictionary to store (CurrentCell -> ParentCell)
        // This allows us to trace the path back from the end to the start.
        Dictionary<(int x, int y), (int x, int y)> parents = new Dictionary<(int x, int y), (int x, int y)>();
        parents[start] = start; // Start has no parent

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();
            NodesExplored++;

            // Check if we reached the exit
            if (current == end)
            {
                return ReconstructPath(parents, end);
            }

            // 3. TODO: Check all 4 neighbors. 
            // Define the directions we can move
        var directions = new (int dx, int dy, WallState wall)[]
        {
            (0, -1, WallState.North),
            (0, 1,  WallState.South),
            (1, 0,  WallState.East),
            (-1, 0, WallState.West)
        };

        foreach (var (dx, dy, wall) in directions)
        {
            int nx = current.x + dx;
            int ny = current.y + dy;

            if (maze.IsInBounds(nx, ny) && !maze.HasWall(current.x, current.y, wall))
            {
                if (!parents.ContainsKey((nx, ny)))
                {
                    parents[(nx, ny)] = current; // Save the "Breadcrumb"
                    frontier.Enqueue((nx, ny));
                }
            }
}
            // If there is NO wall between 'current' and 'neighbor', 
            // AND the neighbor hasn't been visited (isn't in the dictionary)...
            // Add it to the frontier and set its parent!
            
        }
        
        return new List<(int x, int y)>(); // No path found
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