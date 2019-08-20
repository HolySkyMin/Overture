using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Idol;

namespace Ingame
{
    [RequireComponent(typeof(IdolCard))]
    public class IdolListCardClicker : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private Vector3 originScale;
        private IdolListViewer viewer;

        private void Awake()
        {
            originScale = gameObject.transform.localScale;
        }

        public void SetViewer(IdolListViewer v)
        {
            viewer = v;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            viewer.ShowSpecificInfo(GetComponent<IdolCard>().LinkedIdol);
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