# 🧩 Maze Generator & Solver

> A procedural maze engine built in C# (.NET 8) that generates perfect mazes using **Recursive Backtracking** and solves them head-to-head with **Breadth-First Search** and **A\*** — outputting a side-by-side algorithm performance comparison in the terminal.

---

## 📐 Technical Depth

### 1. Bitmasking — The Core Data Structure (`Core/Maze.cs`)

Each cell in the maze is stored as a **single byte** using a `[Flags]` enum called `WallState`. Rather than maintaining four separate boolean fields per cell, every wall and the visited state are packed into individual bits:

```
Bit 0 (1)  → North wall
Bit 1 (2)  → East wall
Bit 2 (4)  → South wall
Bit 3 (8)  → West wall
Bit 4 (16) → Visited flag
```

This means checking, setting, or clearing a wall is a single **bitwise AND/OR/XOR** operation — O(1) and cache-friendly. When a wall is removed between two cells, the `RemoveWall` method automatically clears the **opposite** wall on the neighbouring cell (e.g., removing the `North` wall of cell A also removes the `South` wall of cell B), preventing data inconsistency.

```csharp
// Remove wall from current cell
_cells[x1, y1] &= ~direction;

// Remove opposite wall from neighbour
WallState opposite = GetOpposite(direction);
_cells[x2, y2] &= ~opposite;
```

### 2. Recursive Backtracking — Maze Generation (`Algorithms/RecursiveBacktracker.cs`)

The generator uses an **iterative depth-first search** with an explicit stack (avoiding real recursion stack overflow on large mazes). Starting at `(0,0)`, it:

1. Marks the current cell as visited.
2. Picks a random unvisited neighbour.
3. **Carves** a path (removes the shared wall) and pushes the neighbour onto the stack.
4. If no unvisited neighbours exist (dead end), it **backtracks** by popping the stack.

This always produces a **perfect maze** — exactly one unique path between any two cells, guaranteed. The generator accepts an optional string seed, hashed via `GetHashCode()` into the `Random` object, making mazes fully **reproducible**.

### 3. Breadth-First Search — Guaranteed Shortest Path (`Algorithms/BreadthFirstSolver.cs`)

BFS explores cells in expanding "rings" from the start using a **Queue**. Because every edge has equal weight (cost = 1 per step), BFS is guaranteed to find the **shortest path**. A `parents` dictionary acts as a breadcrumb trail:

```
parents[neighbour] = current
```

Once the end cell is reached, the path is reconstructed by following breadcrumbs backwards from end → start, then reversing the list.

**Time Complexity:** O(V + E), where V = cells and E = open passages.

### 4. A\* Search — Heuristic-Guided Pathfinding (`Algorithms/AStarSolver.cs`)

A\* uses a **PriorityQueue** ordered by `f = g + h`:

- **g** = exact cost from start to current cell (number of steps taken).
- **h** = **Manhattan Distance** heuristic: `|Δx| + |Δy|` — an admissible, never-overestimating estimate of remaining cost.

Because the maze is an unweighted grid, A\* typically explores **far fewer nodes** than BFS, reaching the goal much faster by prioritising cells that are geometrically closer to the exit. The `NodesExplored` counter on each solver makes this difference measurable and printable.

### 5. Interface-Driven Architecture

`IMazeGenerator` and `IMazeSolver` decouple the algorithm implementations from the program logic. New algorithms (Dijkstra, DFS solver, Wilson's algorithm, etc.) can be swapped in with **zero changes** to `Program.cs` — a textbook example of the **Open/Closed Principle**.

---

## 🚀 Installation & Usage

**Prerequisites:** [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

```bash
# Clone the repository
git clone https://github.com/YOUR_USERNAME/maze-project.git
cd maze-project

# Run the project
dotnet run
```

**Expected output:**

```
--- Procedural Maze Generator ---
+---+---+---+ ...
|   |       | ...
+   +---+   + ...
...

--- ALGORITHM COMPARISON ---
Algorithm            | Nodes Explored  | Path Length
-------------------------------------------------------
BFS (The Ripple)     | 312             | 57
A* (The Stream)      | 89              | 57
-------------------------------------------------------

[BFS solution path drawn]
[A* solution path drawn]
```

**Customising maze size** — edit the top of `Program.cs`:

```csharp
int width  = 20;  // ← change these
int height = 20;
```

**Reproducible mazes** — pass a seed string to the generator:

```csharp
IMazeGenerator generator = new RecursiveBacktracker("my-seed-123");
```

---

## 🎓 Learning Outcomes

Working through this project developed and solidified the following skills:

**Data Structures & Algorithms**
- Applied **bitmasking** with `[Flags]` enums as a compact, efficient cell representation — understanding bitwise AND (`&`), OR (`|`), and NOT (`~`) operators at a practical level.
- Implemented **BFS** from scratch using a Queue and a parent-tracking dictionary, understanding why it guarantees the shortest path on unweighted graphs.
- Implemented **A\*** with a `PriorityQueue<T, int>`, learning the distinction between `g` (actual cost) and `h` (heuristic), and what makes a heuristic *admissible*.
- Understood the **Recursive Backtracking / DFS** algorithm and why it produces perfect mazes with no loops and no isolated sections.

**C# & .NET**
- Used `[Flags]` enums, bitwise operators, and value tuples `(int x, int y)` idiomatically.
- Designed against **interfaces** (`IMazeGenerator`, `IMazeSolver`) to achieve polymorphism and extensibility.
- Used `StringBuilder` for efficient multi-line console rendering instead of repeated `Console.Write` calls.
- Worked with `PriorityQueue<TElement, TPriority>` introduced in .NET 6.

**Software Design**
- Applied the **Single Responsibility Principle**: `Maze` owns data, `RecursiveBacktracker` generates, `BreadthFirstSolver`/`AStarSolver` solve, `TextRenderer` displays.
- Understood **reproducibility** via seeding — important for testing and debugging procedural generation.
- Measured and compared algorithm performance empirically using `NodesExplored`, bridging theory and practice.

---

## 📁 Project Structure

```
maze-project/
├── Core/
│   └── Maze.cs                  # WallState bitmask enum + grid data structure
├── Algorithms/
│   ├── RecursiveBacktracker.cs  # Maze generator (DFS + backtracking)
│   ├── BreadthFirstSolver.cs    # BFS pathfinder (shortest path guaranteed)
│   └── AStarSolver.cs           # A* pathfinder (heuristic-guided)
├── Interfaces/
│   ├── IMazeGenerator.cs
│   └── IMazeSolver.cs
├── Display/
│   └── TextRenderer.cs          # ASCII console renderer with path overlay
├── Program.cs                   # Entry point & algorithm comparison output
└── MazeProject.csproj           # .NET 8 project file
```

---

## 🏷️ GitHub Topics

`csharp` · `dotnet` · `maze-generator` · `pathfinding` · `a-star` · `breadth-first-search` · `algorithms` · `data-structures` · `procedural-generation` · `computer-science`
