namespace GridGame.Models;

public class MapDefinition
{
    public int[] Cells { get; }
    public int Rows { get; }
    public int Columns { get; }

    public MapDefinition(int[] cells, int rows, int columns)
    {
        Cells = cells;
        Rows = rows;
        Columns = columns;
    }

}