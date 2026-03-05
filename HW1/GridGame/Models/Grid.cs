// namespace logiclayer;
namespace GridGame.Models;
public class Grid : Map
{
    // had to change the name from Grid to Cells cause: " error CS0542: 'Grid': member names cannot be the same as their enclosing type "
    public Map Properties {get; private set;}
    public Grid() // default constructor, makes an empty 1x1 grid
    {
        Properties = new Map();  
    }
    public Grid(int rows, int columns) // constructor that makes an empty grid of specified dimensions
    {
        Properties = new Map(rows, columns); 
    }
    public Grid(int[] cells, int rows, int columns) // constructor that creates a grid of specified dimensions and contents
    {
        Properties = new Map(cells, rows, columns);
    }
    public Grid(Map map)
    {
        Properties = map;
    }
    public int RowColumnToIndex(int row, int column) // parses row and column to index
    {
        return column + row*(Properties.Columns+1);
    }
    public void SetGrid(int[] grid, int rows, int columns) // sets grid to given values and dimensions
    {
        Properties.Cells = grid;
        Properties.Rows = rows;
        Properties.Columns = columns;
    }
    public void SetGrid(Map map)
    {
        Properties = map;
    }
    public bool CheckAdjacentCells(int value, int? index) // returns true if any adjacent cell is of the desired value
    {
        bool ans = false; // return variable
        int targetIndex = index ?? -1; // target index, check around it
        int indexModifier; // adjusts index to look at adjacent cells
        int columnIndex = targetIndex%(Properties.Columns+1);
        int rowIndex = (targetIndex - columnIndex)/(Properties.Rows+1);
        int indexModified = -1;
        for (int i = 0; i < 9; i++)
        {
            if (targetIndex < 0) // check if index is null
                break;
            if((i == 4) || ((columnIndex == 0) && ((i%3) == 0)) || ((columnIndex == Properties.Columns) && ((i%3) == 2)) || ((rowIndex == 0) && (i < 3)) || ((rowIndex == Properties.Rows) && (i > 5)))
                continue; // skip checking out of the bounds of the edge cases and self
            if (i < 3) // searching above - substract number of Columns
            {
                indexModifier = (Properties.Columns+1)*-1;
            }
            else if(i < 6) // searching left and right, do nothing
            {
                indexModifier = 0; 
            }
            else // seatching below, add number of columns
            {
                indexModifier = Properties.Columns+1;
            }
            /*   0   1   2
            * 0 [0] [1] [2]
            * 1 [3] [T] [5]
            * 2 [6] [7] [8]
            */
            indexModified = targetIndex + indexModifier + (i%3 - 1);
            if ((indexModified < 0) || (indexModified > Properties.Cells.Length-1))
                continue;
            else if (Properties.Cells[indexModified] == value)
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
            if (((targetColumn+columnModifier) < 0) || ((targetColumn+columnModifier) > Properties.Columns) || ((targetRow+rowModifier) < 0) || ((targetRow+rowModifier) > Properties.Rows))
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