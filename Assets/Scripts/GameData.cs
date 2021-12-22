using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class GameData
{
    public int Coins = 50;
    public int Level = 1;
    public int Exp;
    public bool MusicActive = true;
    public bool SoundActive = true;
    public bool PostProcActive = false;
    public string SavedMap;
    public string SavedCar;
    public List<string> AvailableMaps = new List<string> { "Earth" };
    public List<string> AvailableCars = new List<string> { "Poleaxe" };
    public Dictionary<string, int> DistancesCounter;
}
