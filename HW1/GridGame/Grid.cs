namespace logiclayer;
public class Grid
{
    public uint[] Grid {get; private set;} // index - adress of a cell of a grid, values of cells: 0 - empty, 1 - wall, 2...n - players
    public uint Rows {get; private set;} // Rows, 0 is first
    public uint Columns {get; private set;} // Columns, 0 is first
    public Grid() // default constructor, makes an empty 1x1 grid
    {
        Grid = new uint{0};
        Rows = 0;
        Columns = 0;    
    }
    public Grid(uint rows, uint columns) // constructor that makes an empty grid of specified dimensions
    {
        Grid = new uint[(rows+1)*(columns+1)];
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
    public uint RowColumnToIndex(uint row, uint column) // parses row and column to index
    {
        return column + row*(Rows+1);
    }
    public uint GetCell(uint index) // returns value of a cell at a given index
    {
        return Grid[index];
    }
    public uint GetCell(uint row, uint column) // returns value of a cell at a given row and column
    {
        return Grid[RowColumnToIndex(row, column)];
    }
    public void SetCell(uint value, uint index) //sets cell to a given value at a given index
    {
        Grid[index] = value;
    }
    public void SetCell(uint value, uint row, uint column) //sets cell to a given value at a given row and column
    {
        Grid[RowColumnToIndex(row, column)] = value;
    }
    public void SetGrid(uint[] grid, uint rows, uint columns) // sets grid to given values and dimensions
    {
        Grid = grid;
        Rows = rows;
        Columns = columns;
    }
}