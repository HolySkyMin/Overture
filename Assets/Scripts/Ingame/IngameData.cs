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
        public WorkData[] Work;
        public IdolPickGroup[] LessonIdols;
        public CDData CDProcess;

        public string GroupName;
        public int Money;
        public int CurrentIdolIndex;
        public int CurrentSongIndex;
        public List<Dictionary<string, object>> SongDataPool;

        public IngameData()
        {
            Theater = new TheaterData();
            LessonIdols = new IdolPickGroup[4];
            CDProcess = new CDData();
            CurrentIdolIndex = 0;
            CurrentSongIndex = 0;
        }
    }
}