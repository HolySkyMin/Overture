using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Song
{
    public class SongCard : MonoBehaviour
    {
        public Text NameText, FlavorText, MaxIdolText;
        public Text[] AbilityText;
        public GameObject[] RankColorStars;

        private SongData Song;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetData(SongData data)
        {
            Song = data;
            foreach (var obj in RankColorStars)
                obj.SetActive(false);

            NameText.text = Song.Name;
            FlavorText.text = Song.FlavorText;
            MaxIdolText.text = Song.MaxIdol.ToString();
            for (int i = 0; i < Song.Abilities.Length; i++)
                AbilityText[i].text = Song.Abilities[i].Description;
            for (int i = 0; i < Song.Rank; i++)
                RankColorStars[i].SetActive(true);
        }
    }
}