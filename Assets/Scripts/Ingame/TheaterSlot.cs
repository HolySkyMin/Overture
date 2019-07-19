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
        public bool IdolAssigned { get { return Slot != null && Slot.Idols.Length > 0; } }

        public Text StatusText, SongNameText, MaxIdolText;

        [HideInInspector]
        public SongData Song;
        [HideInInspector]
        public IdolPickGroup Slot;

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
            Slot = new IdolPickGroup(Song.MaxIdol);
        }

        public void EraseSong()
        {
            Song = null;
            Slot = null;
        }

        public float CalculateAppeal()
        {
            return 0;
        }
    }
}