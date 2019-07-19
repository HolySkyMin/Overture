using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Song;
using Ingame;

namespace Idol
{
    [System.Serializable]
    public class IdolPickGroup
    {
        public int Capacity;
        public IdolData[] Idols;
        public Dictionary<IdolPersonality, int> PersonaDic;

        public override string ToString()
        {
            string res = "[ ";
            for (int i = 0; i < Idols.Length; i++)
                if (Idols[i] != null)
                    res += Idols[i].Name + ", ";
            res += "]";
            return res;
        }

        public IdolPickGroup(int capacity)
        {
            Capacity = capacity;
            Idols = new IdolData[capacity];
            PersonaDic = new Dictionary<IdolPersonality, int>();
        }

        public void SetIdol(int index, IdolData idol)
        {
            if (index < Capacity && index >= 0)
            {
                Idols[index] = idol;
                if (!PersonaDic.ContainsKey(idol.Personality))
                    PersonaDic.Add(idol.Personality, 0);
                PersonaDic[idol.Personality]++;
            }
        }

        public void RemoveIdol(int index)
        {
            if (index < Capacity && index >= 0)
            {
                PersonaDic[Idols[index].Personality]--;
                if (PersonaDic[Idols[index].Personality] < 1)
                    PersonaDic.Remove(Idols[index].Personality);
                Idols[index] = null;
            }
        }

        public float CaculateAppeal(SongData song)
        {
            return 0;
        }

        public float CalculateAppeal(WorkData work)
        {
            return 0;
        }

        public float ApplyPersonality(float allAppeal, float[] idolAppeal)
        {

        }
    }
}