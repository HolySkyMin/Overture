using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Idol
{
    [Serializable]
    public enum IdolPersonality
    {
        Unknown, Bashful, Jolly, Naive, Quirky, Bold, Mild, Relaxed, Healthy, Focus, Sad, Ambitious, Mysterious, Frugal
    }

    [Serializable]
    public class IdolData
    {
        public const int IMAGE_COUNT = 42;

        public static readonly Dictionary<IdolPersonality, string> PersonaStringDic = new Dictionary<IdolPersonality, string>()
        {
            {IdolPersonality.Unknown, "???" },
            {IdolPersonality.Bashful, "수줍음" },
            {IdolPersonality.Jolly, "명랑함" },
            {IdolPersonality.Naive, "천진난만함" },
            {IdolPersonality.Quirky, "변덕스러움" },
            {IdolPersonality.Bold, "대담함" },
            {IdolPersonality.Mild, "의젓함" },
            {IdolPersonality.Relaxed, "무사태평함" },
            {IdolPersonality.Healthy, "기운참" },
            {IdolPersonality.Focus, "집중함" },
            {IdolPersonality.Sad, "우울함" },
            {IdolPersonality.Ambitious, "야망적임" },
            {IdolPersonality.Mysterious, "신비함" },
            {IdolPersonality.Frugal, "검소함" },
        };
        public static readonly int[] PersonaPercentageTable = { 0, 2, 5, 2, 5, 1, 2, 5, 1, 8, 2, 2, 1, 7 };
        public static readonly int[] CostThreshold = { 0, 8, 13, 17, 21, 24, 26, 28, 30, 31, 32, 33, 34, 35, 35, 36, 37, 38, 39, 40 };

        public string Name { get { return LastName + " " + FirstName; } }
        public int Cost
        {
            get
            {
                for (int i = 1; i < CostThreshold.Length; i++)
                    if (CostThreshold[i - 1] < Vocal + Dance + Visual + Variety && Vocal + Dance + Visual + Variety <= CostThreshold[i])
                        return i;
                return 0;
            }
        }

        public int Index;
        public string FirstName;
        public string LastName;
        public string ImageKey;
        public int Vocal;
        public int Dance;
        public int Visual;
        public int Variety;
        public int Potential;
        public int Honor;
        public int Fan;
        public IdolPersonality Personality;

        public bool IsWorkLessonPicked;

        public static IdolData[] Generate(int amount)
        {
            var namedata = CSVReader.Read("Data/Idol/IdolNameTable");

            var datalist = new List<IdolData>();
            for(int i = 0; i < amount; i++)
            {
                var data = new IdolData()
                {
                    FirstName = (string)namedata[UnityEngine.Random.Range(0, namedata.Count)]["first"],
                    LastName = (string)namedata[UnityEngine.Random.Range(0, namedata.Count)]["last"],
                    ImageKey = $"Images/Idol/chr{UnityEngine.Random.Range(0, IMAGE_COUNT)}",
                    Vocal = 1,
                    Dance = 1,
                    Visual = 1,
                    Variety = 1,
                    Honor = 0,
                    Fan = 0,
                };
                int rnd = UnityEngine.Random.Range(1, 36);
                int sum = 0;
                for (int j = 1; j < 12; j++)
                {
                    if (sum < rnd && rnd <= sum + PersonaPercentageTable[j])
                    {
                        data.Personality = (IdolPersonality)j;
                        break;
                    }
                    else
                        sum += PersonaPercentageTable[j];
                }
                for(int j = 4; j < 17; j++)
                {
                    bool distributed = false;
                    do
                    {
                        int dice = UnityEngine.Random.Range(0, 4);
                        if (dice == 0 && data.Vocal < 7)
                        {
                            data.Vocal++;
                            distributed = true;
                        }
                        else if (dice == 1 && data.Dance < 7)
                        {
                            data.Dance++;
                            distributed = true;
                        }
                        else if (dice == 2 && data.Visual < 7)
                        {
                            data.Visual++;
                            distributed = true;
                        }
                        else if (dice == 3 && data.Variety < 7)
                        {
                            data.Variety++;
                            distributed = true;
                        }
                    } while (!distributed);
                }
                datalist.Add(data);
            }
            return datalist.ToArray();
        }

        public bool CheckRestriction(int[] restriction)
        {
            bool chk = true;
            chk &= restriction[0] <= Vocal;
            chk &= restriction[1] <= Dance;
            chk &= restriction[2] <= Visual;
            chk &= restriction[3] <= Variety;
            chk &= restriction[5] <= Honor;
            chk &= restriction[6] <= Fan;
            return chk;
        }
    }
}