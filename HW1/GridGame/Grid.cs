namespace logiclayer;
public class Grid()
{
    public uint[] Grid {get; set;} // index - adress of a cell of a grid, values of cells: 0 - empty, 1 - wall, 2...n - players
    public uint Rows, Columns; // dimensions 1x1 - Rows = 0, Columns = 0
    public Grid() // default constructor, makes an empty 1x1 grid
    {
        Grid = new uint{0};
        Rows = 0;
        Columns = 0;    
    }
    public Grid(uint rows, uint columns) // constructor that makes an empty grid of specified dimensions
    {
        Grid = new uint[rows*columns];
        Rows = rows;
        Columns = columns;
        for (int i = 0; i < (rows+1)*(columns+1); i++)
        {
            Grid[i] = 0;
        }
    }
    public Grid(uint[] grid, uint rows, uint columns) // constructor that creates a grid of specified dimensions and contents
    {
        Grid = grid;
        Rows = rows;
        Columns = columns;
    }
    public uint GetCell(uint row, uint column)
    {
        return column + row*(Rows+1);
    }
    public void SetCell(uint row, uint column, uint value)
    {
        Grid[column+row*(Rows+1)] = value;
    }
    public uint[] GetGrid()
    {
        return Grid;
    }
    public void SetGrid(uint[] grid)
    {
        Grid = grid;
    }
}