using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManageMent
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        IEnumerator FadeOutIn()
        {
            yield return FadeOut(2.0f);
            yield return FadeIn(2.0f);
        }

        public IEnumerator FadeOut(float time)
        {
            while (canvasGroup.alpha < 1.0f)
            {
                canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
        }

        public IEnumerator FadeIn(float time)
        {
            while (canvasGroup.alpha > 0.0f)
            {
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }
    }
}