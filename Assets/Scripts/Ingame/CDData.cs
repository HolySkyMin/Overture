using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Idol;

namespace Ingame
{
    [System.Serializable]
    public class CDData
    {
        public int CDCount;
        public bool IsProcessing;
        public int ProcessingTurnLeft;

        public List<int> Songs;
        public IdolPickGroup Idols;

        public CDData()
        {
            CDCount = 1;
            Songs = new List<int>();
            Idols = new IdolPickGroup(12);
        }
    }
}