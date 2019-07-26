using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ingame;

namespace Song
{
    [System.Serializable]
    public enum SongType
    {
        Undefined, Vocal, Performance, Appearance, Harmony
    }

    [System.Serializable]
    public class SongData
    {
        [System.NonSerialized]
        public static float[] RankMultiplyTable = { 0, 1, 1.2f, 1.5f, 2, 3 };
        [System.NonSerialized]
        public static Dictionary<SongType, (int, int, int)> TypeMultiplyTable = new Dictionary<SongType, (int, int, int)>()
        {
            { SongType.Undefined, (10, 10, 10) },
            { SongType.Vocal, (40, 10, 10) },
            { SongType.Performance, (10, 40, 10) },
            { SongType.Appearance, (10, 10, 40) },
            { SongType.Harmony, (20, 20, 20) }
        };

        public int Index;
        public string Name;
        public int Rank;
        public int Cost;
        public SongType Type;
        public string FlavorText;
        public int MaxIdol;
        public AbilityData[] Abilities;

        public static Dictionary<int, SongData> GetAll()
        {
            var datas = CSVReader.Read("Data/SongDataTable");

            var list = new Dictionary<int, SongData>();
            int cnt = 0;
            foreach(var data in datas)
            {
                var song = new SongData()
                {
                    Index = cnt,
                    Name = (string)data["name"],
                    Rank = (int)data["rank"],
                    Type = (SongType)data["type"],
                    MaxIdol = (int)data["maxIdol"],
                    FlavorText = ((string)data["description"]).Replace('@', '\n')
                };
                list.Add(cnt++, song);
            }
            return list;
        }

        public int CalculateAppeal(int vocal, int dance, int visual)
        {
            return Mathf.FloorToInt((vocal * TypeMultiplyTable[Type].Item1 + dance * TypeMultiplyTable[Type].Item2 + visual * TypeMultiplyTable[Type].Item3) * RankMultiplyTable[Rank]);
        }
    }
}