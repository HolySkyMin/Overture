using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Song;

namespace Ingame
{
    public class SongPicker : MonoBehaviour
    {
        public static SongPicker Instance { get; private set; }

        public bool PickCompleted { get; set; }

        public GameObject MasterPickPanel, CardHolder;
        public RectTransform CardParent;

        [HideInInspector]
        public int PickedSongIndex;

        private int lastPickCardIndex;
        private List<GameObject> cards = new List<GameObject>();

        private void Awake()
        {
            Instance = this;
            MasterPickPanel.SetActive(false);
            PickedSongIndex = -1;
            lastPickCardIndex = -1;
        }

        public void LoadCard(SongData data)
        {
            var holderObj = Instantiate(CardHolder);
            holderObj.GetComponent<RectTransform>().SetParent(CardParent);
            holderObj.SetActive(true);

            var cardObj = Instantiate(Resources.Load<GameObject>("Prefabs/SongCard_SongPicker"));
            cardObj.GetComponent<SongCard>().SetSong(data);
            cardObj.GetComponent<RectTransform>().SetParent(holderObj.transform);
            cardObj.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            cardObj.transform.localPosition = Vector3.zero;
            cardObj.SetActive(true);

            holderObj.SetActive(false);
            cards.Add(holderObj);
        }

        public async Task<int> Show(int existingIndex)
        {
            PickCompleted = false;
            PickedSongIndex = existingIndex;
            lastPickCardIndex = -1;

            MasterPickPanel.SetActive(true);
            for (int i = 0; i < cards.Count; i++)
            {
                cards[i].GetComponentInChildren<SongPickerCardClicker>().Index = i;
                cards[i].GetComponentInChildren<SongPickerCardClicker>().Clean();
                cards[i].SetActive(true);

                var linked = cards[i].GetComponentInChildren<SongCard>().LinkedSong;
                if (linked.Index == existingIndex)
                {
                    cards[i].GetComponentInChildren<SongPickerCardClicker>().SetPicked();
                    lastPickCardIndex = i;
                }
            }

            await new WaitUntil(() => PickCompleted);
            return PickedSongIndex;
        }

        public bool TryPick(SongData data)
        {
            if(PickedSongIndex == -1)
            {
                PickedSongIndex = data.Index;
                return true;
            }
            else
                return false;
        }

        public void Pick(SongData data, int cardIndex)
        {
            PickedSongIndex = data.Index;
            if(lastPickCardIndex > -1)
                cards[lastPickCardIndex].GetComponentInChildren<SongPickerCardClicker>().Clean();
            lastPickCardIndex = cardIndex;
        }

        public void Remove()
        {
            PickedSongIndex = -1;
        }

        public void ConfirmPick()
        {
            PickCompleted = true;
            MasterPickPanel.SetActive(false);
        }
    }
}
