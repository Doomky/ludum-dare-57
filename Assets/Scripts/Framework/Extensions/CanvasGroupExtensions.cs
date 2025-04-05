using Framework.DataStructures;
using System.Collections;
using UnityEngine;

namespace Framework.Extensions
{
    public static class CanvasGroupExtensions
    {
        public static IEnumerator WaitForFadeIn(this CanvasGroup canvasGroup, FloatCerper floatCerper)
        {
            floatCerper.Set(0, 1);

            while (floatCerper.IsInProgess())
            {
                canvasGroup.alpha = floatCerper.Get();
                yield return new WaitForEndOfFrame();
            }
        }

        public static IEnumerator WaitForFadeOut(this CanvasGroup canvasGroup, FloatCerper floatCerper)
        {
            floatCerper.Set(1, 0);

            while (floatCerper.IsInProgess())
            {
                canvasGroup.alpha = floatCerper.Get();
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
