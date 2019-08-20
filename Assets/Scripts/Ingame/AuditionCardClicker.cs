using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Idol;

namespace Ingame
{
    [RequireComponent(typeof(IdolCard))]
    public class AuditionCardClicker : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        public GameObject PickedEffect;

        private bool picked = false;
        private Vector3 originScale;

        private void Awake()
        {
            originScale = gameObject.transform.localScale;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(picked)
            {
                AuditionPicker.Instance.Remove(GetComponent<IdolCard>().LinkedIdol);
                PickedEffect.SetActive(false);
                picked = false;
            }
            else
            {
                var result = AuditionPicker.Instance.TryPick(GetComponent<IdolCard>().LinkedIdol);
                if(result == true)
                {
                    PickedEffect.SetActive(true);
                    picked = true;
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            gameObject.transform.localScale = originScale * 1.1f;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            gameObject.transform.localScale = originScale;
        }
    }
}