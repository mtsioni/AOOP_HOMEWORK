namespace logiclayer;
public class Grid
{

    // had to change the name from Grid to Cells cause: " error CS0542: 'Grid': member names cannot be the same as their enclosing type "
    public uint[] Cells {get; private set;} // index - adress of a cell of a grid, values of cells: 0 - empty, 1 - wall, 2...n - players
    public uint Rows {get; private set;} // Rows, 0 is first
    public uint Columns {get; private set;} // Columns, 0 is first
    public Grid() // default constructor, makes an empty 1x1 grid
    {
        Cells = new uint[1];
        Rows = 0;
        Columns = 0;    
    }
    public Grid(uint rows, uint columns) // constructor that makes an empty grid of specified dimensions
    {
        Cells = new uint[(rows+1)*(columns+1)];
        Rows = rows;
        Columns = columns;
        for (int i = 0; i < (rows+1)*(columns+1); i++)
        {
            Cells[i] = 0;
        }
    }
    public Grid(uint[] grid, uint rows, uint columns) // constructor that creates a grid of specified dimensions and contents
    {
        Cells = grid;
        Rows = rows;
        Columns = columns;
    }
    public uint RowColumnToIndex(uint row, uint column) // parses row and column to index
    {
        return column + row*(Columns+1);
    }
    public uint GetCell(uint index) // returns value of a cell at a given index
    {
        return Cells[index];
    }
    public uint GetCell(uint row, uint column) // returns value of a cell at a given row and column
    {
        return Cells[RowColumnToIndex(row, column)];
    }
    public void SetCell(uint value, uint index) //sets cell to a given value at a given index
    {
        Cells[index] = value;
    }
    public void SetCell(uint value, uint row, uint column) //sets cell to a given value at a given row and column
    {
        Cells[RowColumnToIndex(row, column)] = value;
    }
    public void SetGrid(uint[] grid, uint rows, uint columns) // sets grid to given values and dimensions
    {
        Cells = grid;
        Rows = rows;
        Columns = columns;
    }
    public bool CheckAdjacentCells(uint value, uint? index) // returns true if any adjacent cell is of the desired value
    {
        bool ans = false; // return variable
        int? targetIndex = (int?)index; // target index, check around it
        int indexModifier; // adjusts index to look at adjacent cells
        for (int i = 0; i < 9; i++)
        {
            if (targetIndex == null) // check if index is null
                break;
            if (i == 4) // skip checking index itself
                continue;
            if (i < 3) // searching above - substract number of Columns
            {
                // indexModifier = (Columns+1)*-1;
            }
            else if(i < 6) // searching left and right, do nothing
            {
                indexModifier = 0; 
            }
            else // seatching below, add number of columns
            {
                // indexModifier = Columns+1;
            }
            /*   0   1   2
            * 0 [0] [1] [2]
            * 1 [3] [T] [5]
            * 2 [6] [7] [8]
            */
            try // try - to avoid index out of bounds exception
            {
                // if (Cells[targetIndex + indexModifier + (i-1)] == value) // Check if cell adjacent to target holds the desired value
                // {
                //     ans = true; //set ans to true and stop checking
                //     break;
                // }
                // else continue;
            }
            catch (Exception e) // skip whatever is wrong
            {
                continue;
            }
        }
        return ans;
    }
    public bool CheckAdjacentCells(uint value, uint? row, uint? column) // returns true if any adjacent cell is of the desired value
    {
        bool ans = false; // return variable
        int? targetRow = (int?)row, targetColumn = (int?)column; // target row and column meaning around what cell we seatch
        int rowModifier, columnModifier; // these will be added to a target row and column to check contents of adjacent cells
        for(int i = 0; i < 9; i++)
        {
            if ((targetRow == null) || (targetColumn == null)) // if row or column provided is null stop search return false
                break;
            if (i == 4) // skip the target cell itself
                continue;
            if (i < 3) // i from 0 to 2, check the cells above so rowMod -1
            {
                rowModifier = -1;
                columnModifier = i-1;
            }
            else if (i < 6) // i 3 and 5, check cells on the same row, so rowMod = 0
            {
                rowModifier = 0;
                columnModifier = i-4;
            }
            else // i 6 to 8, check the row below, so rowMod +1
            {
                rowModifier = 1;
                columnModifier = i-7;
            }
            /*   0   1   2
            * 0 [0] [1] [2]
            * 1 [3] [T] [5]
            * 2 [6] [7] [8]
            */
            try // try - to avoid index out of bounds exception
            {
                if (Cells[RowColumnToIndex((uint)(targetRow+rowModifier),(uint)(targetColumn+columnModifier))] == value) // Check if cell adjacent to target holds the desired value
                {
                    ans = true; //set ans to true and stop checking
                    break;
                }
                else continue;
            }
            catch (Exception e) // skip whatever is wrong
            {
                continue;
            }
        }
        return ans;
    }
}