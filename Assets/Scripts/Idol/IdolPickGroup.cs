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
        public int Count;
        public int[] IdolIndices;
        public Dictionary<IdolPersonality, int> PersonaDic;

        public override string ToString()
        {
            string res = "[ ";
            for (int i = 0; i < IdolIndices.Length; i++)
                if (IdolIndices[i] != -1)
                    res += IngameManager.Instance.Data.Idols[IdolIndices[i]].Name + ", ";
            res += "]";
            return res;
        }

        public IdolPickGroup(int capacity)
        {
            Capacity = capacity;
            Count = 0;
            IdolIndices = new int[capacity];
            for (int i = 0; i < capacity; i++)
                IdolIndices[i] = -1;
            PersonaDic = new Dictionary<IdolPersonality, int>();
        }

        public void SetIdol(int index, IdolData idol)
        {
            if (index < Capacity && index >= 0)
            {
                IdolIndices[index] = idol.Index;
                if (!PersonaDic.ContainsKey(idol.Personality))
                    PersonaDic.Add(idol.Personality, 0);
                PersonaDic[idol.Personality]++;
                Count++;
            }
        }

        public void RemoveIdol(int index)
        {
            if (index < Capacity && index >= 0)
            {
                var idol = IngameManager.Instance.Data.Idols[IdolIndices[index]];
                PersonaDic[idol.Personality]--;
                if (PersonaDic[idol.Personality] < 1)
                    PersonaDic.Remove(idol.Personality);
                IdolIndices[index] = -1;
                Count--;
            }
        }

        public (float, float[]) CalculateAppeal(SongData song)
        {
            float res = 0;
            var resIdol = new float[Capacity];

            for(int i = 0; i < Capacity; i++)
            {
                if (IdolIndices[i] != -1)
                {
                    var idol = IngameManager.Instance.Data.Idols[IdolIndices[i]];
                    resIdol[i] = song.CalculateAppeal(idol.Vocal, idol.Dance, idol.Visual);
                    res += resIdol[i];
                }
            }
            var finalRes = ApplyPersonality(res, resIdol);
            return (finalRes, resIdol);
        }

        public float CalculateAppeal(WorkData work)
        {
            return 0;
        }

        public float ApplyPersonality(float allAppeal, float[] idolAppeal)
        {
            return 0;
        }
    }
}