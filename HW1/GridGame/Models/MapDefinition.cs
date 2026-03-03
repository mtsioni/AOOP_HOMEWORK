namespace GridGame.Models;

public class MapDefinition
{
    public string Name { get; }
    public int[] Cells { get; }
    public int Rows { get; }
    public int Columns { get; }

    public MapDefinition(string name, int[] cells, int rows, int columns)
    {
        Name = name;
        Cells = cells;
        Rows = rows;
        Columns = columns;
    }

    public override string ToString() => Name;
}