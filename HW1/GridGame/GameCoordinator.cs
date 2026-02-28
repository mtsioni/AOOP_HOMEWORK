namespace logiclayer;
public static class GameCoordinator
{
    public Grid Grid {get; private set;} // Grid of the game
    public List<Player> Players {get; set;} // Players
    public uint GameStatus {get; set;} // Shows the game status 0 - ongoing, 1 - draw, 2...n player won
    public uint? LastMoveHolder {get; set;} // Index for the Players array, index reffers to a player who made the last move
    public uint TotalTurns {get; set;} // Number of turns taken
    private uint turn;
    public uint Turn // Index for the Players array, index reffers to a player who's turn it is
    {
        get => turn%Player.Count; // never go out of bounds (say turn = 23 (24th) and there are 7 players (0 .. 6), 23 % 7 = 2)
        set => turn = value; // set as normal
    }
    public GameCoordinator(Grid grid, List<Player> players)
    {
        Grid = grid;
        Players = players;
        GameStatus = 0;
        LastMoveHolder = null;
        TotalTurns = 0;
        Turn = 0;
    }
    /*public void UpdateCanPlay(uint playerIndex)
    {
        bool ans = false;
        if (TotalTurns+2 == Players[playerIndex].PlayerID) // player's first turn
        {
            for(int i = 0; i < (Grid.Rows+1)*(Grid.Columns+1); i++) // if there are any blank spots, player can take them, so can play
            {
                if (Grid.Grid[i] == 0)
                {
                    ans = true;
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < (Grid.Rows+1)*(Grid.Columns+1); i++) // if any player's cells have blank adjacent spaces, player can take them, so can play
            {
                if (Grid.Grid[i] == Players[playerIndex].PlayerID)
                {
                    if (Grid.CheckAdjacentCells(0, (uint)i))
                    {
                        ans = true;
                        break;
                    }
                }
            }
        }
        Players[playerIndex].CanPlay = ans;
    }*/
    public void UpdateMovesLeft(uint playerIndex)
    {
        uint ans = 0;
        if (TotalTurns+2 == Players[playerIndex].PlayerID)
        {
            for (int i = 0; i < (Grid.Rows+1)*(Grid.Columns+1); i++)
            {
                if (Grid.Grid[i] == 0)
                    ans++;
            }
        }
        else
        {
            for (int i = 0; i < (Grid.Rows+1)*(Grid.Columns+1); i++)
            {
                if (Grid.Grid[i] == 0)
                {
                    if(Grid.CheckAdjacentCells(Players[playerIndex].PlayerID, i) == true)
                        ans++;
                }
            }
        }
        Players[playerIndex].MovesLeft = ans;
    }
    public void PassOnTheTurn()
    {
        while (true)
        {
            if (Players[(Turn + 1) % Players.Count].CanPlay)
            {
                Turn++;
                break;
            }
            else
            {
                Turn++;
            }
        }
    }
    public void AdvanceGame(object sender, RoutedEventArgs args)
    {
        if (GameStatus == 0)
        {
            UpdateMovesLeft(Turn); // updates how many moves player has left
            Players[Turn].CanPlay = (Players[Turn].MovesLeft > 0) ? true : false; // see if they can move
            if (Turn == LastMoveHolder) // if turn returned to the last move holder, check if they won or drawed
            {
                if (Players[Turn].CanPlay == true)
                    GameStatus = Players[Turn].PlayerID; // win
                else
                    GameStatus = 1; // draw
            }
            if (Players[Turn].CanPlay) // if player can play check if their selection is valid
            {
                if ((Players[Turn].SelectedRow == null) || (Players[Turn].SelectedColumn == null)) // checks if cell is selected
                {
                    Console.Write("cell not selected"); // placeholder
                }
                else if (TotalTurns+2 == Players[Turn].PlayerID) // first move
                {
                    if (Grid.Grid[Grid.RowColumnToIndex(Players[Turn].SelectedRow, Players[Turn].SelectedColumn)] == 0) // check if selected spot is empty
                    {
                        Grid.Grid[Grid.RowColumnToIndex(Players[Turn].SelectedRow, Players[Turn].SelectedColumn)] = Players[Turn].PlayerID; // set spot to be player's
                        LastMoveHolder = Turn; // adjust LastMoveHolder
                        TotalTurns++; // increase turn counter
                        PassOnTheTurn(); // pass on the turn
                    }
                }
                else // any other move
                {
                    if ((Grid.Grid[Grid.RowColumnToIndex(Players[Turn].SelectedRow, Players[Turn].SelectedColumn)] == 0) && (Grid.CheckAdjacentCells(Players[Turn].PlayerID, Players[Turn].SelectedRow, Players[Turn].SelectedColumn) == true)) // check if the spot is empty and adjacent to any of the player's cells
                    {
                        Grid.Grid[Grid.RowColumnToIndex(Players[Turn].SelectedRow, Players[Turn].SelectedColumn)] = Players[Turn].PlayerID; // set spot to be player's
                        LastMoveHolder = Turn; // adjust LastMoveHolder
                        TotalTurns++; // increase turn counter
                        PassOnTheTurn(); // pass on the turn
                    }
                }
            }
        }
    }
}