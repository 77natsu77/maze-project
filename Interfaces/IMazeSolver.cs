namespace MazeProject.Interfaces;
using MazeProject.Core;
public interface IMazeSolver
{
    // Returns a list of coordinates representing the path from start to end
    List<(int x, int y)> Solve(Maze maze, (int x, int y) start, (int x, int y) end);

    // Useful for NEA documentation: tracking how many nodes were visited
    int NodesExplored { get; }
    
    // The name of the algorithm (e.g., "A-Star", "BFS")
    string AlgorithmName { get; }
}