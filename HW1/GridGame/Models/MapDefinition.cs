namespace GridGame.Models;

public class Map
{
    public int[] Cells { get; }
    public int Rows { get; }
    public int Columns { get; }

    public Map(int[] cells, int rows, int columns)
    {
        Cells = cells;
        Rows = rows;
        Columns = columns;
    }

    public string Name { get; set; } = "Map Preset 1";
    public override string ToString()
    {
        return Name;
    }

}