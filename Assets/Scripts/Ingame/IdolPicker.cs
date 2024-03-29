﻿using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Idol;

namespace Ingame
{
    public class IdolPicker : MonoBehaviour
    {
        public static IdolPicker Instance { get; private set; }

        public bool IsWorkLessonPick { get; set; }
        public bool PickCompleted { get; set; }

        public GameObject MasterPickPanel, CardHolder;
        public ObjectWithText RejectObject;
        public RectTransform CardParent;
        public IdolPickVisualizer Visualizer;
        public Scrollbar Bar;
        public Animator Motion;

        [HideInInspector]
        public int MaxPick;
        [HideInInspector]
        public IdolPickGroup PickGroup;

        private bool[] isSlotFilled;
        private List<GameObject> cards = new List<GameObject>();

        private void Awake()
        {
            Instance = this;
            MasterPickPanel.SetActive(false);
        }

        public void LoadCard(IdolData data)
        {
            var holderObj = Instantiate(CardHolder);
            holderObj.GetComponent<RectTransform>().SetParent(CardParent);
            holderObj.transform.localScale = Vector3.one;
            holderObj.SetActive(true);

            var cardObj = Instantiate(Resources.Load<GameObject>("Prefabs/IdolCard_IdolPicker"));
            cardObj.GetComponent<IdolCard>().SetIdol(data);
            cardObj.GetComponent<RectTransform>().SetParent(holderObj.transform);
            cardObj.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            cardObj.transform.localPosition = Vector3.zero;
            cardObj.SetActive(true);

            holderObj.SetActive(false);
            cards.Add(holderObj);
        }

        public void RemoveCard(IdolData data)
        {
            for(int i = 0; i < cards.Count; i++)
            {
                if(cards[i].GetComponentInChildren<IdolCard>().LinkedIdol == data)
                {
                    Destroy(cards[i]);
                    cards.RemoveAt(i);
                    break;
                }
            }
        }

        private void Update()
        {
            Visualizer.Visualize(PickGroup);
        }

        public async Task<IdolPickGroup> Show(int count)
        {
            PickCompleted = false;
            PickGroup = new IdolPickGroup(count);
            MaxPick = count;

            isSlotFilled = new bool[count];
            for (int i = 0; i < MaxPick; i++)
                isSlotFilled[i] = false;

            // DO UI THING
            MasterPickPanel.SetActive(true);
            Bar.value = 0;
            for (int i = 0; i < cards.Count; i++)
            {
                cards[i].GetComponentInChildren<IdolPickerCardClicker>().Clean();
                cards[i].SetActive(true);
            }

            await new WaitUntil(() => PickCompleted);
            return PickGroup;
        }

        public async Task<IdolPickGroup> Show(IdolPickGroup existing, bool isWorkOrLesson = false)
        {
            IsWorkLessonPick = isWorkOrLesson;
            PickCompleted = false;
            PickGroup = existing;
            MaxPick = existing.Capacity;

            isSlotFilled = new bool[MaxPick];
            for (int i = 0; i < MaxPick; i++)
                if (existing.IdolIndices[i] != -1)
                    isSlotFilled[i] = true;

            MasterPickPanel.SetActive(true);
            for(int i = 0; i < cards.Count; i++)
            {
                var cardClicker = cards[i].GetComponentInChildren<IdolPickerCardClicker>();
                cardClicker.Clean();
                cards[i].SetActive(true);

                var linked = cards[i].GetComponentInChildren<IdolCard>().LinkedIdol;
                bool flag = false;
                for (int j = 0; j < existing.IdolIndices.Length; j++)
                {
                    if (linked.Index == existing.IdolIndices[j])
                    {
                        cardClicker.SetPicked(j);
                        flag = true;
                        break;
                    }
                }
                if(isWorkOrLesson && flag == false && linked.IsWorkLessonPicked)
                {
                    cardClicker.WorkLessonEffect.SetActive(true);
                    cardClicker.allowPick = false;
                }
            }
            Bar.value = 1;

            await new WaitUntil(() => PickCompleted);
            return PickGroup;
        }

        public (bool, int) TryPick(IdolData data)
        {
            for(int i = 0; i < MaxPick; i++)
            {
                if(!isSlotFilled[i])
                {
                    var res = PickGroup.SetIdol(i, data, IsWorkLessonPick, out string reason);
                    if(res == true)
                    {
                        isSlotFilled[i] = true;
                        return (true, i);
                    }
                    else
                    {
                        RejectObject.text.text = reason;
                        Motion.StopPlayback();
                        Motion.Play("PickerRejectNotiAnim", -1, 0);
                        return (false, -1);
                    }
                }
            }
            RejectObject.text.text = "슬롯이 다 찼습니다.";
            Motion.StopPlayback();
            Motion.Play("PickerRejectNotiAnim", -1, 0);
            return (false, -1);
        }

        public void Remove(int index)
        {
            PickGroup.RemoveIdol(index, IsWorkLessonPick);
            isSlotFilled[index] = false;
        }

        public void ConfirmPick()
        {
            PickCompleted = true;
            Motion.StopPlayback();
            RejectObject.SetActive(false);
            MasterPickPanel.SetActive(false);
        }
    }
}