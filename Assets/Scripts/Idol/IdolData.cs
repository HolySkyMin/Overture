using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Idol
{
    [Serializable]
    public enum IdolPersonality
    {
        Unknown, Bashful, Jolly, Naive, Quirky, Bold, Mild, Relaxed
    }

    [Serializable]
    public class IdolData
    {
        public string Name { get { return LastName + " " + FirstName; } }

        public int Index;
        public string FirstName;
        public string LastName;
        public string ImageKey;
        public int Cost;
        public int Vocal;
        public int Dance;
        public int Visual;
        public int Variety;
        public int Potential;
        public int Honor;
        public int Fan;
        public IdolPersonality Personality;

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
                    // ImageKey
                    Cost = 1,
                    Vocal = 1,
                    Dance = 1,
                    Visual = 1,
                    Variety = 1,
                    Honor = 0,
                    Fan = 0,
                    Personality = (IdolPersonality)UnityEngine.Random.Range(1, 8)
                };
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
    }

    
}