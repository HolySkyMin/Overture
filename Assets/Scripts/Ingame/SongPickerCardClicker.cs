using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Song;

namespace Ingame
{
    [RequireComponent(typeof(SongCard))]
    public class SongPickerCardClicker : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public GameObject PickedEffect;

        [HideInInspector]
        public int Index;

        private bool picked = false;
        private Vector3 originScale;

        private void Awake()
        {
            originScale = gameObject.transform.localScale;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (picked)
            {
                SongPicker.Instance.Remove();
                PickedEffect.SetActive(false);
                picked = false;
            }
            else
            {
                SongPicker.Instance.Pick(GetComponent<SongCard>().LinkedSong, Index);
                SetPicked();
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

        public void Clean()
        {
            picked = false;
            PickedEffect.SetActive(false);
        }

        public void SetPicked()
        {
            PickedEffect.SetActive(true);
            picked = true;
        }
    }
}