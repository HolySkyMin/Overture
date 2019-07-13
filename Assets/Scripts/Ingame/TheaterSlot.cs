using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Idol;
using Song;

namespace Ingame
{
    public class TheaterSlot : MonoBehaviour
    {
        public bool SongAssigned { get { return Song != null; } }
        public bool IdolAssigned { get { return Idols.Count > 0; } }

        public Text StatusText, SongNameText, MaxIdolText;

        [HideInInspector]
        public SongData Song;
        [HideInInspector]
        public List<IdolData> Idols;

        private void Awake()
        {
            Idols = new List<IdolData>();
        }

        void Start()
        {

        }

        void Update()
        {
            if (SongAssigned)
            {
                if (IdolAssigned)
                    StatusText.text = "OK";
                else
                    StatusText.text = "RQ";
            }
            else
                StatusText.text = "NA";
        }

        public void SetSong(SongData data)
        {
            Song = data;
            Idols.Clear();
        }

        public float CalculateAppeal()
        {
            return 0;
        }
    }
}