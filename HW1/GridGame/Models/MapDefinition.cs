namespace GridGame.Models;

public class Map
{
    public int[] Cells { get; set; }
    public int Rows { get; set; }
    public int Columns { get; set; }
    public string Name { get; set; }
    public Map()
    {
        Cells = new int[1];
        Rows = 0;
        Columns = 0;
        Name = "";
    }
    public Map(int rows, int columns)
    {
        Cells = new int[(rows+1)*(columns+1)];
        Rows = rows;
        Columns = columns;
        Name = "";
    }
    public Map(int[] cells, int rows, int columns)
    {
        Cells = cells;
        Rows = rows;
        Columns = columns;
        Name = "";
    }
    public Map(int[] cells, int rows, int columns, string name)
    {
        Cells = cells;
        Rows = rows;
        Columns = columns;
        Name = name;
    }
    public override string ToString()
    {
        return Name;
    }

}