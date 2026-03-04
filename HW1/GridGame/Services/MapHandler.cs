using System.Text.Json;
using System.IO;
using GridGame.Models;
namespace GridGame.Services;
public class Data
{
    public List<Map> Presets {get; set;}
    public List<Map> Saves {get; set;}
}
public class MapHandler
{
    public Data Maps {get; set;}
    public MapHandler(){
        Maps = new Data();
        string path = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Maps.json");
        string json = System.IO.File.ReadAllText(path);
        Maps = JsonSerializer.Deserialize<Data>(json);
    }
    public Map LoadMap(int index)
    {
        return Maps.Saves[index];
    }
    public void ResetMap(int index)
    {
        Maps.Saves[index] = Maps.Presets[index];
    }
}