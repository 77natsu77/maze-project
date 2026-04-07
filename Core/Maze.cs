namespace MazeProject.Core;

[Flags]
public enum WallState : byte
{
    None = 0,
    North = 1, 
    East = 2,  
    South = 4, 
    West = 8,  
    Visited = 16, // New flag! 
    All = 15      // 1+2+4+8 (doesn't include Visited by default)
}

public class Maze
{
    private readonly WallState[,] _cells;
    public int Width { get; }
    public int Height { get; }

    public Maze(int width, int height)
    {
        Width = width;
        Height = height;
        _cells = new WallState[width, height];

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                _cells[x, y] = WallState.All;
    }

    public void MarkVisited(int x, int y)
    {
        _cells[x, y] |= WallState.Visited;
    }

    public bool IsVisited(int x, int y)
    {
        return (_cells[x, y] & WallState.Visited) != 0;
    }

    public bool HasWall(int x, int y, WallState wall) => (_cells[x, y] & wall) != 0;

    // This handles the "Neighbor Redundancy" automatically!
    public void RemoveWall(int x1, int y1, int x2, int y2, WallState direction)
    {
        // 1. Remove wall from current cell
        _cells[x1, y1] &= ~direction;

        // 2. Remove the opposite wall from the neighbor
        WallState opposite = GetOpposite(direction);
        _cells[x2, y2] &= ~opposite;
    }

    private WallState GetOpposite(WallState wall)
    {
        return wall switch
        {
            WallState.North => WallState.South,
            WallState.South => WallState.North,
            WallState.East => WallState.West,
            WallState.West => WallState.East,
            _ => WallState.None
        };
    }

    // Helper to ensure we don't go out of bounds
    public bool IsInBounds(int x, int y) => x >= 0 && x < Width && y >= 0 && y < Height;
}