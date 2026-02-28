namespace logiclayer;
public class Player
{
    public uint PlayerID {get; set;} //id of a player, same number as the value of a cell of a grid that belong to the player
    public string PlayerColor {get; set;} //player color, in hex?
    public bool CanPlay {get; set;} // true - had a move, false - out of moves
    public uint? SelectedRow {get; set;} // row of a cell player wants to fill
    public uint? SelectedColumn {get; set;} // column of a cell player wants to fill
    public uint? MovesLeft {get; set;}
    public Player(uint id, string color) // constructor
    {
        PlayerID = id;
        PlayerColor = color;
        CanPlay = true;
        SelectedRow = null;
        SelectedColumn = null;
        MovesLeft = null;
    }
}