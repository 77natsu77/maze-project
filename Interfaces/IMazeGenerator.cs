namespace MazeProject.Interfaces;
using MazeProject.Core;

public interface IMazeGenerator
{
    // The generator takes a blank maze and "carves" paths into it
    void Generate(Maze maze);
}