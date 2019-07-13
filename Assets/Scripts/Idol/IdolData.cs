using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Idol
{
    [System.Serializable]
    public enum IdolPersonality
    {
        Unknown, Bashful, Jolly, Naive, Quirky, Bold, Mild, Relaxed
    }

    [System.Serializable]
    public class IdolData
    {
        public string Name { get { return LastName + " " + FirstName; } }

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
    }
}