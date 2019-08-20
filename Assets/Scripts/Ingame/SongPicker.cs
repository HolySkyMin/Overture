using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Song;

namespace Ingame
{
    public class SongPicker : MonoBehaviour
    {
        public static SongPicker Instance { get; private set; }

        public bool PickCompleted { get; set; }
        public bool IsMultiple { get; set; }

        public GameObject MasterPickPanel, CardHolder;
        public RectTransform CardParent;
        public Scrollbar Bar;

        [HideInInspector]
        public int PickedSongIndex;
        [HideInInspector]
        public List<int> PickedSongList;

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
            IsMultiple = false;

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
            Bar.value = 1;

            await new WaitUntil(() => PickCompleted);
            return PickedSongIndex;
        }

        public async Task<List<int>> Show(List<int> existingList)
        {
            PickCompleted = false;
            PickedSongList = existingList;
            IsMultiple = true;

            MasterPickPanel.SetActive(true);
            for (int i = 0; i < cards.Count; i++)
            {
                cards[i].GetComponentInChildren<SongPickerCardClicker>().Index = i;
                cards[i].GetComponentInChildren<SongPickerCardClicker>().Clean();
                cards[i].SetActive(true);

                var linked = cards[i].GetComponentInChildren<SongCard>().LinkedSong;
                if (PickedSongList.Contains(linked.Index))
                {
                    cards[i].GetComponentInChildren<SongPickerCardClicker>().SetPicked();
                    lastPickCardIndex = i;
                }
            }
            Bar.value = 1;

            await new WaitUntil(() => PickCompleted);
            return PickedSongList;
        }

        public bool TryPick(SongData data)
        {
            if(IsMultiple)
            {
                PickedSongList.Add(data.Index);
                return true;
            }
            else
            {
                if (PickedSongIndex == -1)
                {
                    PickedSongIndex = data.Index;
                    return true;
                }
                else
                    return false;
            }
        }

        public void Pick(SongData data, int cardIndex)
        {
            if(IsMultiple)
                PickedSongList.Add(data.Index);
            else
            {
                PickedSongIndex = data.Index;
                if (lastPickCardIndex > -1)
                    cards[lastPickCardIndex].GetComponentInChildren<SongPickerCardClicker>().Clean();
                lastPickCardIndex = cardIndex;
            }
        }

        public void Remove(SongData data)
        {
            if (IsMultiple)
                PickedSongList.Remove(data.Index);
            else
                PickedSongIndex = -1;
        }

        public void ConfirmPick()
        {
            PickCompleted = true;
            MasterPickPanel.SetActive(false);
        }
    }
}
