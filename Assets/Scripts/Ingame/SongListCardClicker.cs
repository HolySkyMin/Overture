using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Song;

namespace Ingame
{
    [RequireComponent(typeof(SongCard))]
    public class SongListCardClicker : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private Vector3 originScale;
        private SongListViewer viewer;

        private void Awake()
        {
            originScale = gameObject.transform.localScale;
        }

        public void SetViewer(SongListViewer v)
        {
            viewer = v;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            viewer.ShowSpecificInfo(GetComponent<SongCard>().LinkedSong);
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