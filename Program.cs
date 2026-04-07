using MazeProject.Core;
using MazeProject.Algorithms;
using MazeProject.Interfaces;
using MazeProject.Display;

// Setting up the dimensions
int width = 20;
int height = 20;

// Instantiate the components
Maze myMaze = new Maze(width, height);
IMazeGenerator generator = new RecursiveBacktracker();
TextRenderer renderer = new TextRenderer();

// Execute
Console.Clear();
Console.WriteLine("--- Procedural Maze Generator ---");
generator.Generate(myMaze);
renderer.Draw(myMaze);

// ... (After generation)
// ... after maze generation is done ...

var bfs = new BreadthFirstSolver();
var astar = new AStarSolver();
var start = (0, 0);
var end = (width - 1, height - 1);

// Run BFS
var pathBfs = bfs.Solve(myMaze, start, end);
int nodesBfs = bfs.NodesExplored;

// Run A*
var pathAStar = astar.Solve(myMaze, start, end);
int nodesAStar = astar.NodesExplored;

// Output Table
Console.WriteLine("\n--- ALGORITHM COMPARISON ---");
Console.WriteLine($"{"Algorithm",-20} | {"Nodes Explored",-15} | {"Path Length",-12}");
Console.WriteLine(new string('-', 55));
Console.WriteLine($"{"BFS (The Ripple)",-20} | {nodesBfs,-15} | {pathBfs.Count,-12}");
Console.WriteLine($"{"A* (The Stream)",-20} | {nodesAStar,-15} | {pathAStar.Count,-12}");
Console.WriteLine("-------------------------------------------------------\n");

renderer.Draw(myMaze, pathBfs);
renderer.Draw(myMaze, pathAStar);