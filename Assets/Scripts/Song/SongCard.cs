using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Song
{
    public class SongCard : MonoBehaviour
    {
        public Text NameText, FlavorText, MaxIdolText;
        public GameObject[] TypePanel;
        public Text[] AbilityText;
        public GameObject[] RankColorStars;

        [HideInInspector]
        public SongData LinkedSong;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetSong(SongData data)
        {
            LinkedSong = data;
            foreach (var obj in RankColorStars)
                obj.SetActive(false);

            NameText.text = LinkedSong.Name;
            FlavorText.text = LinkedSong.FlavorText;
            for (int i = 0; i < 5; i++)
            {
                if ((int)LinkedSong.Type == i)
                    TypePanel[i].SetActive(true);
                else
                    TypePanel[i].SetActive(false);
            }
            MaxIdolText.text = LinkedSong.MaxIdol.ToString();
            //for (int i = 0; i < LinkedSong.Abilities.Length; i++)
            //    AbilityText[i].text = LinkedSong.Abilities[i].Description;
            for (int i = 0; i < LinkedSong.Rank; i++)
                RankColorStars[i].SetActive(true);
        }
    }
}