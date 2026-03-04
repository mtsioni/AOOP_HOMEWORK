// namespace logiclayer;
namespace GridGame.Models;
public class Grid
{

    // had to change the name from Grid to Cells cause: " error CS0542: 'Grid': member names cannot be the same as their enclosing type "
    public int[] Cells {get; private set;} // index - adress of a cell of a grid, values of cells: 0 - empty, 1 - wall, 2...n - players
    public int Rows {get; private set;} // Rows, 0 is first
    public int Columns {get; private set;} // Columns, 0 is first
    public Grid() // default constructor, makes an empty 1x1 grid
    {
        Cells = new int[1];
        Rows = 0;
        Columns = 0;    
    }
    public Grid(int rows, int columns) // constructor that makes an empty grid of specified dimensions
    {
        Cells = new int[(rows+1)*(columns+1)];
        Rows = rows;
        Columns = columns;
        for (int i = 0; i < (rows+1)*(columns+1); i++)
        {
            Cells[i] = 0;
        }
    }
    public Grid(int[] cells, int rows, int columns) // constructor that creates a grid of specified dimensions and contents
    {
        Cells = cells;
        Rows = rows;
        Columns = columns;
    }
    public int RowColumnToIndex(int row, int column) // parses row and column to index
    {
        return column + row*(Columns+1);
    }
    public void SetGrid(int[] grid, int rows, int columns) // sets grid to given values and dimensions
    {
        Cells = grid;
        Rows = rows;
        Columns = columns;
    }
    public bool CheckAdjacentCells(int value, int? index) // returns true if any adjacent cell is of the desired value
    {
        bool ans = false; // return variable
        int targetIndex = index ?? -1; // target index, check around it
        int indexModifier; // adjusts index to look at adjacent cells
        int columnIndex = targetIndex%(Columns+1);
        int rowIndex = (targetIndex - columnIndex)/(Rows+1);
        int indexModified = -1;
        for (int i = 0; i < 9; i++)
        {
            if (targetIndex < 0) // check if index is null
                break;
            if((i == 4) || ((columnIndex == 0) && ((i%3) == 0)) || ((columnIndex == Columns) && ((i%3) == 2)) || ((rowIndex == 0) && (i < 3)) || ((rowIndex == Rows) && (i > 5)))
                continue; // skip checking out of the bounds of the edge cases and self
            if (i < 3) // searching above - substract number of Columns
            {
                indexModifier = (Columns+1)*-1;
            }
            else if(i < 6) // searching left and right, do nothing
            {
                indexModifier = 0; 
            }
            else // seatching below, add number of columns
            {
                indexModifier = Columns+1;
            }
            /*   0   1   2
            * 0 [0] [1] [2]
            * 1 [3] [T] [5]
            * 2 [6] [7] [8]
            */
            indexModified = targetIndex + indexModifier + (i%3 - 1);
            if ((indexModified < 0) || (indexModified > Cells.Length-1))
                continue;
            else if (Cells[indexModified] == value)
            {
                ans = true;
                break;
            }
        }
        return ans;
    }
    public bool CheckAdjacentCells(int value, int? row, int? column) // returns true if any adjacent cell is of the desired value
    {
        bool ans = false; // return variable
        int targetRow = row ?? -1, targetColumn = column ?? -1; // target row and column meaning around what cell we seatch
        int rowModifier, columnModifier; // these will be added to a target row and column to check contents of adjacent cells
        for(int i = 0; i < 9; i++)
        {
            if ((targetRow < 0) || (targetColumn < 0)) // if row or column provided is null stop search return false
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
            if (((targetColumn+columnModifier) < 0) || ((targetColumn+columnModifier) > Columns) || ((targetRow+rowModifier) < 0) || ((targetRow+rowModifier) > Rows))
                continue;
            else if (Cells[RowColumnToIndex(targetRow+rowModifier, targetColumn+columnModifier)] == value)
            {
                ans = true;
                break;
            }
        }
        return ans;
    }
}