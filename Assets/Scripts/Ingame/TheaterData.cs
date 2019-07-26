using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Idol;
using Song;

namespace Ingame
{
    [Serializable]
    public class TheaterData
    {
        public TheaterUnitData[] Units;

        public TheaterData()
        {
            Units = new TheaterUnitData[5];
            for (int i = 0; i < Units.Length; i++)
                Units[i] = new TheaterUnitData();
            Units[0].Unlocked = true;
            Units[3].Unlocked = true;
        }
    }

    [Serializable]
    public class TheaterUnitData
    {
        public bool Unlocked;
        public TheaterSlotData[] Slots;

        public TheaterUnitData()
        {
            Slots = new TheaterSlotData[4];
            for (int i = 0; i < Slots.Length; i++)
                Slots[i] = new TheaterSlotData();
        }
    }

    [Serializable]
    public class TheaterSlotData
    {
        public int SongIndex;
        public IdolPickGroup Idols;

        public TheaterSlotData()
        {
            SongIndex = -1;
        }
    }
}