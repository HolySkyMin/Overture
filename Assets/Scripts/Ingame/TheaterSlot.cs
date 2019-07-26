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
        public bool SongAssigned { get { return Data.SongIndex != -1; } }
        public bool IdolAssigned { get { return Data.Idols.Count > 0; } }

        public Text StatusText, SongNameText, MaxIdolText;
        public Button IdolSelectBtn;

        [HideInInspector]
        public TheaterSlotData Data;

        void Update()
        {
            if (SongAssigned)
            {
                if (IdolAssigned)
                    StatusText.text = "OK";
                else
                    StatusText.text = "RQ";
                IdolSelectBtn.interactable = true;
            }
            else
            {
                StatusText.text = "NA";
                IdolSelectBtn.interactable = false;
            }
        }

        public async void SetSong()
        {
            Data.SongIndex = await SongPicker.Instance.Show(Data.SongIndex);
            if (Data.SongIndex > -1)
            {
                Data.Idols = new IdolPickGroup(IngameManager.Instance.Data.Songs[Data.SongIndex].MaxIdol);
                SongNameText.text = IngameManager.Instance.Data.Songs[Data.SongIndex].Name;
                MaxIdolText.text = $"{Data.Idols.Count}/{Data.Idols.Capacity}";
            }
            else
                EraseSong();
        }

        public async void SetIdol()
        {
            Data.Idols = await IdolPicker.Instance.Show(Data.Idols);
            MaxIdolText.text = $"{Data.Idols.Count}/{Data.Idols.Capacity}";
        }

        public void EraseSong()
        {
            Data.SongIndex = -1;
            Data.Idols = new IdolPickGroup(0);
            SongNameText.text = "";
            MaxIdolText.text = "0/0";
        }

        public float CalculateAppeal()
        {
            var appeal = Data.Idols.CalculateAppeal(IngameManager.Instance.Data.Songs[Data.SongIndex]);
            // Process for appeal item 2
            return appeal.Item1;
        }
    }
}