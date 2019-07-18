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
        public List<IdolData> Idols;
        public List<SongData> Songs;
    }
}