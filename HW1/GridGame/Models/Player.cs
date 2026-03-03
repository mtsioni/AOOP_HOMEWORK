// namespace logiclayer;
namespace GridGame.Models;
public class Player
{
    public int PlayerID {get; set;} //id of a player, same number as the value of a cell of a grid that belong to the player
    public string PlayerColor {get; set;} //player color, in hex?
    public string Name {get; set;} // added the name property
    public bool CanPlay {get; set;} // true - had a move, false - out of moves
    public int? SelectedRow {get; set;} // row of a cell player wants to fill
    public int? SelectedColumn {get; set;} // column of a cell player wants to fill
    public int? MovesLeft {get; set;}
    public Player(int id, string color, string name) // constructor
    {
        PlayerID = id;
        PlayerColor = color;
        Name = name;
        CanPlay = true;
        SelectedRow = null;
        SelectedColumn = null;
        MovesLeft = null;
    }
}