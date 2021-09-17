﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class GameData
{
    public int Coins;
    public int Level = 1;
    public int Exp;
    public bool MusicActive = true;
    public bool SoundActive = true;
    public bool PostProcActive = true;
    public Map Map;
}
