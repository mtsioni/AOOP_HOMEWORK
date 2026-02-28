namespace logiclayer;
public static class GameCoordinator
{
    public Grid Grid {get; private set;} // Grid of the game
    public Player[] Players {get; set;} // Players
    public uint GameStatus {get; set;} // Shows the game status 0 - ongoing, 1 - draw, 2...n player won
    public uint? LastMoveHolder {get; set;} // Index for the Players array, index reffers to a player who made the last move
    public uint TotalTurns {get; set;} // Number of turns taken
    public uint Turn {get; set;} // Index for the Players array, index reffers to a player who's turn it is
    public GameCoordinator(Grid grid, Player[] players)
    {
        Grid = grid;
        Players = players;
        GameStatus = 0;
        LastMoveHolder = null;
        TotalTurns = 0;
        Turn = 0;
    }
    public bool PlayersChoiceIsAdjacent(uint playerIndex) // returns true if player's selected cell is adjacent to any of player's other cells(doesn't account cell being occupied, idk if it should be done here or somewhere else)
    {
        bool ans = false; // return variable
        int? targetRow = (int?)Players[playerIndex].SelectedRow, targetColumn = (int?)Players[playerIndex].SelectedColumn; // target row and column meaning where player wants to fill
        int rowModifier, columnModifier; // these will be added to a target row and column to check contents of adjacent cells
        for(int i = 0; i < 9; i++)
        {
            if ((targetRow == null) || (targetColumn == null)) // if player didn't select anything stop checking return false
                break;
            if (i == 4) // skip the target cell itself
                continue;
            if (i < 3) // i from 0 to 2, check cells above so row -1
            {
                rowModifier = -1;
                columnModifier = i-1;
            }
            else if (i < 6) // i 3 and 5, same row
            {
                rowModifier = 0;
                columnModifier = i-4;
            }
            else // i 6 to 8, row below so row +1
            {
                rowModifier = 1;
                columnModifier = i-7;
            }
            /*   0   1   2
            * 0 [0] [1] [2]
            * 1 [3] [T] [5]
            * 2 [6] [7] [8]
            */
            try // try to avoid index out of bounds
            {
                if (Grid.Grid[Grid.RowColumnToIndex((uint)(targetRow+rowModifier),(uint)(targetColumn+columnModifier))] == Players[playerIndex].PlayerID) // Check if cell adjacent to target cell belongs to a player
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