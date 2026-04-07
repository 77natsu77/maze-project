using MazeProject.Core;
using MazeProject.Interfaces;
using System;
using System.Collections.Generic;

namespace MazeProject.Algorithms;

public class RecursiveBacktracker : IMazeGenerator
{
    private readonly Random _rng;
    public string Seed { get; }

    /// <summary>
    /// Initializes the generator. If no seed is provided, a random one is generated.
    /// </summary>
    public RecursiveBacktracker(string? seed = null)
    {
        // If no seed is provided, use the current tick count to make it "random"
        Seed = seed ?? DateTime.Now.Ticks.ToString();
        
        // We use GetHashCode to turn any string into a unique integer for the Random object
        _rng = new Random(Seed.GetHashCode());
    }

    public void Generate(Maze maze)
    {
        var stack = new Stack<(int x, int y)>();
        var start = (x: 0, y: 0);

        maze.MarkVisited(start.x, start.y);
        stack.Push(start);

        while (stack.Count > 0)
        {
            var current = stack.Peek();
            var neighbors = GetUnvisitedNeighbors(maze, current.x, current.y);

            if (neighbors.Count > 0)
            {
                var next = neighbors[_rng.Next(neighbors.Count)];

                // Carve the path
                maze.RemoveWall(current.x, current.y, next.x, next.y, next.dir);

                // Move forward
                maze.MarkVisited(next.x, next.y);
                stack.Push((next.x, next.y));
            }
            else
            {
                // Dead end: Backtrack
                stack.Pop();
            }
        }
    }

    private List<(int x, int y, WallState dir)> GetUnvisitedNeighbors(Maze maze, int x, int y)
    {
        var list = new List<(int x, int y, WallState dir)>();

        if (maze.IsInBounds(x, y - 1) && !maze.IsVisited(x, y - 1)) 
            list.Add((x, y - 1, WallState.North));
        
        if (maze.IsInBounds(x, y + 1) && !maze.IsVisited(x, y + 1)) 
            list.Add((x, y + 1, WallState.South));
        
        if (maze.IsInBounds(x + 1, y) && !maze.IsVisited(x + 1, y)) 
            list.Add((x + 1, y, WallState.East));
        
        if (maze.IsInBounds(x - 1, y) && !maze.IsVisited(x - 1, y)) 
            list.Add((x - 1, y, WallState.West));

        return list;
    }
}