using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Idol;

namespace Ingame
{
    public class IdolListViewer : MonoBehaviour
    {
        public GameObject MasterPanel, MoreInfoPanel, CardHolder;
        public RectTransform CardParent;
        public IdolCard InfoCard;
        public Scrollbar Bar;

        private List<GameObject> cards = new List<GameObject>();

        public void LoadCard(IdolData data)
        {
            var holderObj = Instantiate(CardHolder);
            holderObj.GetComponent<RectTransform>().SetParent(CardParent);
            holderObj.SetActive(true);

            var cardObj = Instantiate(Resources.Load<GameObject>("Prefabs/IdolCard_IdolList"));
            cardObj.GetComponent<IdolCard>().SetIdol(data);
            cardObj.GetComponent<RectTransform>().SetParent(holderObj.transform);
            cardObj.GetComponentInChildren<IdolListCardClicker>().SetViewer(this);
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

        public void ShowSpecificInfo(IdolData data)
        {
            InfoCard.SetIdol(data);
            MoreInfoPanel.SetActive(true);
        }
    }
}