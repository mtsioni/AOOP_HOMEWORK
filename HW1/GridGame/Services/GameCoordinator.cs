using GridGame.Models;
// namespace logiclayer;
namespace GridGame.Services;
public class GameCoordinator
{
    public Grid Grid {get; private set;} // Grid of the game
    public List<Player> Players {get; set;} // Players
    public int GameStatus {get; set;} // Shows the game status 0 - ongoing, 1 - draw, 2...n player won
    public int? LastMoveHolder {get; set;} // Index for the Players array, index reffers to a player who made the last move
    public int TotalTurns {get; set;} // Number of turns taken
    private int turn;
    public int Turn // Index for the Players array, index reffers to a player who's turn it is
    {
        get => turn%Players.Count; // never go out of bounds (say turn = 23 (24th) and there are 7 players (0 .. 6), 23 % 7 = 2)
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
        for (int i = 0; i < Players.Count; i++)
            FirstMovesLeft(i);
    }
    /*public void UpdateCanPlay(int playerIndex)
    {
        bool ans = false;
        if (TotalTurns+2 == Players[playerIndex].PlayerID) // player's first turn
        {
            for(int i = 0; i < (Grid.Rows+1)*(Grid.Columns+1); i++) // if there are any blank spots, player can take them, so can play
            {
                if (Grid.Cells[i] == 0)
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
                if (Grid.Cells[i] == Players[playerIndex].PlayerID)
                {
                    if (Grid.CheckAdjacentCells(0, i))
                    {
                        ans = true;
                        break;
                    }
                }
            }
        }
        Players[playerIndex].CanPlay = ans;
    }*/
    public void FirstMovesLeft(int playerIndex)
    {
        int ans = 0;
        for (int i = 0; i < Grid.Cells.Length; i++)
        {
            if (Grid.Cells[i] == 0)
                ans++;
        }
        Players[playerIndex].MovesLeft = ans;
    }
    public void UpdateMovesLeft(int playerIndex)
    {
        int ans = 0;
        bool spaceAvailable = false;
        bool firstMove = true;
        for (int i = 0; i < Grid.Cells.Length; i++)
        {
            if ((Grid.Cells[i] == Players[playerIndex].PlayerID) && firstMove)
            {
                firstMove = false;
            }
            if ((Grid.Cells[i] == 0) && !spaceAvailable)
            {
                spaceAvailable = true;
            }
            if (!firstMove && spaceAvailable)
                break;
        }
        if (spaceAvailable && firstMove)
        {
            FirstMovesLeft(playerIndex);
            return;
        }
        else if (spaceAvailable)
        {
            for (int i = 0; i < Grid.Cells.Length; i++)
            {
                if (Grid.Cells[i] == 0)
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
            Turn++;
            UpdateMovesLeft(Turn);
            if ((Players[Turn].MovesLeft > 0) || (Turn == LastMoveHolder))
                break;
        }
    }
    public void AdvanceGame()
    {
        if (GameStatus == 0)
        {
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
                int PlayersChoice = -1;
                if ((Players[Turn].SelectedRow != null) && (Players[Turn].SelectedColumn != null))
                    PlayersChoice = Grid.RowColumnToIndex(Players[Turn].SelectedRow ?? 0, Players[Turn].SelectedColumn ?? 0);
                if(PlayersChoice != -1)
                {
                    if (TotalTurns+2 == Players[Turn].PlayerID) // first move
                    {
                        if (Grid.Cells[PlayersChoice] == 0) // check if selected spot is empty
                        {
                            Grid.Cells[PlayersChoice] = Players[Turn].PlayerID; // set spot to be player's
                            LastMoveHolder = Turn; // adjust LastMoveHolder
                            TotalTurns++; // increase turn counter
                            PassOnTheTurn(); // pass on the turn
                        }
                    }
                    else // any other move
                    {
                        if ((Grid.Cells[PlayersChoice] == 0) && (Grid.CheckAdjacentCells(Players[Turn].PlayerID, PlayersChoice) == true)) // check if the spot is empty and adjacent to any of the player's cells
                        {
                            Grid.Cells[PlayersChoice] = Players[Turn].PlayerID; // set spot to be player's
                            LastMoveHolder = Turn; // adjust LastMoveHolder
                            TotalTurns++; // increase turn counter
                            PassOnTheTurn(); // pass on the turn
                        }
                    }
                }
            }
        }
    }
}