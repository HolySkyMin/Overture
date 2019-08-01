using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Idol;

namespace Ingame
{
    [RequireComponent(typeof(IdolCard))]
    public class IdolPickerCardClicker : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public bool allowPick;
        public GameObject PickedEffect, WorkLessonEffect;
        public Text PickOrder;

        private int pickNumber;
        private bool picked = false;
        private Vector3 originScale;

        private void Awake()
        {
            originScale = gameObject.transform.localScale;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(allowPick)
            {
                if (picked)
                {
                    IdolPicker.Instance.Remove(pickNumber);
                    PickedEffect.SetActive(false);
                    picked = false;
                }
                else
                {
                    var pickRes = IdolPicker.Instance.TryPick(GetComponent<IdolCard>().LinkedIdol);
                    if (pickRes.Item1 == true)
                        SetPicked(pickRes.Item2);
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(allowPick)
                gameObject.transform.localScale = originScale * 1.1f;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            gameObject.transform.localScale = originScale;
        }

        public void Clean()
        {
            picked = false;
            allowPick = true;
            PickedEffect.SetActive(false);
            WorkLessonEffect.SetActive(false);
        }

        public void SetPicked(int num)
        {
            PickedEffect.SetActive(true);
            pickNumber = num;
            PickOrder.text = (pickNumber + 1).ToString();
            picked = true;
        }
    }
}