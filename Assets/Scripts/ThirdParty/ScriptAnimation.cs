using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

#pragma warning disable 1998, 4014
namespace ScriptedAnimation
{
    [AddComponentMenu("")]
    public class ScriptAnimation : MonoBehaviour
    {
        [HideInInspector]
        public Vector3 OriginPos, OriginScale, OriginRotation;
        [HideInInspector]
        public Color OriginColor;
        [HideInInspector]
        public float OriginAlpha;

        public async virtual Task Appear()
        {
            Debug.Log("이 메시지를 숨기려면 해당 클래스에 Appear() 함수를 override로 구현하고, 그 함수에서 base.Appear() 를 지우세요.");
        }

        public async virtual Task Disappear()
        {
            Debug.Log("이 메시지를 숨기려면 해당 클래스에 Disappear() 함수를 override로 구현하고, 그 함수에서 base.Disappear() 를 지우세요.");
        }

        protected async Task TweenPosition(Vector3 initial, Vector3 final, float duration)
        {
            GetComponent<RectTransform>().position = initial;
            GetComponent<RectTransform>().DOMove(final, duration);
            await new WaitForSeconds(duration);
        }

        protected async Task TweenScale(Vector3 initial, Vector3 final, float duration)
        {
            GetComponent<RectTransform>().localScale = initial;
            GetComponent<RectTransform>().DOScale(final, duration);
            await new WaitForSeconds(duration);
        }

        protected async Task TweenRotation(Vector3 initial, Vector3 final, float duration)
        {
            GetComponent<RectTransform>().localEulerAngles = initial;
            GetComponent<RectTransform>().DOLocalRotate(final, duration);
            await new WaitForSeconds(duration);
        }

        protected async Task TweenColor(Color initial, Color final, float duration)
        {
            GetComponent<Graphic>().color = initial;
            GetComponent<Graphic>().DOColor(final, duration);
            await new WaitForSeconds(duration);
        }

        protected async Task TweenCanvasGroup(float initial, float final, float duration)
        {
            GetComponent<CanvasGroup>().alpha = initial;
            GetComponent<CanvasGroup>().DOFade(final, duration);
            await new WaitForSeconds(duration);
        }

        protected void SetCurrentAsOrigin()
        {
            OriginPos = GetComponent<RectTransform>().position;
            OriginScale = GetComponent<RectTransform>().localScale;
            OriginRotation = GetComponent<RectTransform>().localEulerAngles;
            OriginColor = GetComponent<Graphic>().color;
            if (GetComponent<CanvasGroup>() != null)
                OriginAlpha = GetComponent<CanvasGroup>().alpha;
        }

        protected void SetOriginAsCurrent()
        {
            GetComponent<RectTransform>().position = OriginPos;
            GetComponent<RectTransform>().localScale = OriginScale;
            GetComponent<RectTransform>().localEulerAngles = OriginRotation;
            GetComponent<Graphic>().color = OriginColor;
            if (GetComponent<CanvasGroup>() != null)
                GetComponent<CanvasGroup>().alpha = OriginAlpha;
        }
    }
}