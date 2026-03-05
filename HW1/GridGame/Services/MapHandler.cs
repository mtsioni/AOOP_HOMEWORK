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
    public Data MapData {get; set;}
    public MapHandler()
    {
        MapData = new Data();
        string path = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Maps.json");
        string json = System.IO.File.ReadAllText(path);
        MapData = JsonSerializer.Deserialize<Data>(json);
    }
    public Map LoadMap(int index)
    {
        return MapData.Saves[index];
    }
    public void ResetMap(int index)
    {
        MapData.Saves[index] = MapData.Presets[index];
        Save();
    }
    public void Save()
    {
        string path = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Maps.json");
        string json = JsonSerializer.Serialize(MapData);
        System.IO.File.WriteAllText(path, json);
    }
}