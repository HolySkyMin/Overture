using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Idol;
using Song;

namespace Ingame
{
    [System.Serializable]
    public class IngameData
    {
        public Dictionary<int, IdolData> Idols;
        public Dictionary<int, SongData> Songs;
        public TheaterData Theater;

        public IngameData()
        {
            Theater = new TheaterData();
        }
    }
}