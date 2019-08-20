using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Song;

namespace Ingame
{
    public class CDSongVisualizeObj : MonoBehaviour
    {
        public Text NameText;
        public GameObject[] TypeImage;
        public Image RankDisplayFill;

        public void Set(SongData data)
        {
            NameText.text = data.Name;
            for(int i = 0; i < TypeImage.Length; i++)
            {
                if ((i + 1) == (int)data.Type)
                    TypeImage[i].SetActive(true);
                else
                    TypeImage[i].SetActive(false);
            }
            RankDisplayFill.fillAmount = (float)data.Rank / 5;
        }
    }
}