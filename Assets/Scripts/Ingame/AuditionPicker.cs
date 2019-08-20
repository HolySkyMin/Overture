using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Idol;

namespace Ingame
{
    public class AuditionPicker : MonoBehaviour
    {
        public static AuditionPicker Instance { get; private set; }

        public bool PickCompleted { get; set; }
        public bool ForcePickExists { get; set; }
        public int ForcePickCount { get; set; }

        public GameObject MasterPanel, CardHolder;
        public RectTransform CardParent;
        public Text LimitationText;
        public Scrollbar Bar;

        private List<IdolData> pickedIdols;
        private List<GameObject> cards;

        private void Awake()
        {
            Instance = this;
            cards = new List<GameObject>();
            MasterPanel.SetActive(false);
        }

        public async Task<List<IdolData>> Show(int amount, int forcePickAmount = 0)
        {
            pickedIdols = new List<IdolData>();
            while(cards.Count > 0)
            {
                Destroy(cards[0]);
                cards.RemoveAt(0);
            }

            MasterPanel.SetActive(true);
            if (forcePickAmount > 0)
            {
                ForcePickExists = true;
                ForcePickCount = forcePickAmount;
                LimitationText.text = $"반드시 {ForcePickCount}명 선택";
            }
            else
                LimitationText.text = "자율 선택";

            var newIdols = IdolData.Generate(amount);
            for(int i = 0; i < newIdols.Length; i++)
            {
                var holderObj = Instantiate(CardHolder);
                holderObj.transform.SetParent(CardParent);
                holderObj.SetActive(true);

                var cardObj = Instantiate(Resources.Load<GameObject>("Prefabs/IdolCard_Audition"));
                cardObj.GetComponent<IdolCard>().SetIdol(newIdols[i]);
                cardObj.transform.SetParent(holderObj.transform);
                cardObj.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                cardObj.transform.localPosition = Vector3.zero;
                cardObj.SetActive(true);

                cards.Add(holderObj);
            }
            Bar.value = 1;

            await new WaitUntil(() => PickCompleted);
            MasterPanel.SetActive(false);
            return pickedIdols;
        }

        public bool TryPick(IdolData data)
        {
            if (!ForcePickExists || (ForcePickExists && pickedIdols.Count < ForcePickCount))
            {
                pickedIdols.Add(data);
                return true;
            }
            else
                return false;
        }

        public void Remove(IdolData data)
        {
            pickedIdols.Remove(data);
        }

        public void TryCompletePick()
        {
            if (ForcePickExists)
            {
                if (pickedIdols.Count == ForcePickCount)
                    PickCompleted = true;
            }
            else
                PickCompleted = true;
        }
    }
}