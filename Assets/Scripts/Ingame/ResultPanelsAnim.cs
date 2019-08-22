using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptedAnimation;

namespace Ingame
{
    public class ResultPanelsAnim : ScriptAnimation
    {
        public override IEnumerator Appear_C()
        {
            SetCurrentAsOrigin();

            TweenPosition(OriginPos + new Vector3(50, 0, 0), OriginPos, 0.3f);
            yield return TweenCanvasGroup_C(0, 1, 0.3f);
        }

        public override IEnumerator Disappear_C()
        {
            TweenPosition(OriginPos, OriginPos - new Vector3(50, 0, 0), 0.3f);
            yield return TweenCanvasGroup_C(1, 0, 0.3f);

            SetOriginAsCurrent();
        }
    }
}