namespace MazeProject.Display;
using MazeProject.Core;
using System.Text;

public class TextRenderer
{
    public void Draw(Maze maze, List<(int x, int y)>? path = null)
{
    // Convert path to a HashSet for lightning-fast lookups
    var pathSet = new HashSet<(int x, int y)>(path ?? new List<(int x, int y)>());

    StringBuilder output = new StringBuilder();

    for (int y = 0; y < maze.Height; y++)
    {
        // Pass A: The "Ceiling" (Same as before)
        for (int x = 0; x < maze.Width; x++)
        {
            output.Append("+");
            output.Append(maze.HasWall(x, y, WallState.North) ? "---" : "   ");
        }
        output.AppendLine("+");

        // Pass B: The "Room" and Path
        for (int x = 0; x < maze.Width; x++)
        {
            output.Append(maze.HasWall(x, y, WallState.West) ? "|" : " ");
            
            // ARCHITECT UPDATE: Check if this cell is on the path
            if (pathSet.Contains((x, y)))
            {
                output.Append(" * "); // Draw the breadcrumb
            }
            else
            {
                output.Append("   "); // Empty room
            }
        }
        output.AppendLine(maze.HasWall(maze.Width - 1, y, WallState.East) ? "|" : " ");
    }
    // THE "FLOOR" PASS
    for (int x = 0; x < maze.Width; x++)
    {
        output.Append("+");
        // We check the South wall of the very last row (Height - 1)
        output.Append(maze.HasWall(x, maze.Height - 1, WallState.South) ? "---" : "   ");
    }
    output.AppendLine("+");

    Console.WriteLine(output.ToString());
}
}