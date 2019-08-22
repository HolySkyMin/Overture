using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Song;

namespace Ingame
{
    public class SongListViewer : MonoBehaviour
    {
        public GameObject MasterPanel, MoreInfoPanel, CardHolder;
        public RectTransform CardParent;
        public SongCard InfoCard;
        public Scrollbar Bar;

        private List<GameObject> cards = new List<GameObject>();

        public void LoadCard(SongData data)
        {
            var holderObj = Instantiate(CardHolder);
            holderObj.GetComponent<RectTransform>().SetParent(CardParent);
            holderObj.transform.localScale = Vector3.one;
            holderObj.SetActive(true);

            var cardObj = Instantiate(Resources.Load<GameObject>("Prefabs/SongCard_SongList"));
            cardObj.GetComponent<SongCard>().SetSong(data);
            cardObj.GetComponent<RectTransform>().SetParent(holderObj.transform);
            cardObj.GetComponentInChildren<SongListCardClicker>().SetViewer(this);
            cardObj.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            cardObj.transform.localPosition = Vector3.zero;
            cardObj.SetActive(true);

            holderObj.SetActive(false);
            cards.Add(holderObj);
        }

        public void ShowList()
        {
            MasterPanel.SetActive(true);
            Bar.value = 0;
            for (int i = 0; i < cards.Count; i++)
                cards[i].SetActive(true);
        }

        public void CloseList()
        {
            MasterPanel.SetActive(false);
            for (int i = 0; i < cards.Count; i++)
                cards[i].SetActive(false);
        }

        public void ShowSpecificInfo(SongData data)
        {
            InfoCard.SetSong(data);
            MoreInfoPanel.SetActive(true);
        }
    }
}