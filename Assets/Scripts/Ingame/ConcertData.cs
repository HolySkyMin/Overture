using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
    [System.Serializable]
    public class ConcertData
    {
        public bool IsProcessing;
        public int ProcessingTurnLeft;

        public string Scale;
        public List<string> StageNames;
        public List<int> SeatCounts;
        public List<float> SoldoutCoeffs;

        public ConcertData()
        {
            StageNames = new List<string>();
            SeatCounts = new List<int>();
            SoldoutCoeffs = new List<float>();
        }
    }
}