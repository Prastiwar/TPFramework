/**
*   Authored by Tomasz Piowczyk
*   MIT LICENSE: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TPFramework
{
    public interface ITPFade
    {
        void InitializeFade(TPFadeState state);
        void Fade(TPFadeInfo fadeInfo, TPFadeState state);
    }

    [Serializable]
    public struct TPFadeInfo
    {
        public string FadeToScene;
        public TPAnimation FadeAnim;
        public ITPFade ITPFade;
    }

    [Serializable]
    public struct TPFadeState
    {
        public float Time;
        public Image Image;
        public CanvasGroup CanvasGrouup;
    }


    [Serializable]
    public struct TPProgressFade : ITPFade
    {
        public GameObject ProgressPrefab;
        public Slider LoadingBar;
        public Image LoadingImage;
        public Text LoadingText;
        public Text LoadingProgressText;
        public string LoadingTextString;

        public float ProgressFadeSpeed;

        public bool MustKeyToStart;
        public bool LoadingAnyKeyToStart;
        public KeyCode LoadingKeyToStart;

        public void InitializeFade(TPFadeState state)
        {
        }

        public void Fade(TPFadeInfo fadeInfo, TPFadeState state)
        {
        }
    }

    [Serializable]
    public struct TPAlphaFade : ITPFade
    {
        public Sprite FadeTexture;
        public Color FadeColor;

        public void InitializeFade(TPFadeState state)
        {
            state.Image.sprite = FadeTexture;
            state.Image.color = FadeColor;
        }

        public void Fade(TPFadeInfo fadeInfo, TPFadeState state)
        {
            state.CanvasGrouup.alpha = TPAnim.NormalizedCurveTime(state.Time);

            if (state.Time >= 0.5f && !string.IsNullOrEmpty(fadeInfo.FadeToScene))
            {
                SceneManager.LoadScene(fadeInfo.FadeToScene);
            }
        }
    }

    public static class TPFade
    {
        private static TPFadeState fadeLayout;
        private static bool isFading;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnSceneLoad()
        {
            GameObject fader = new GameObject("TPFader");
            Canvas canvas = fader.AddComponent<Canvas>();

            fadeLayout.Image = fader.AddComponent<Image>();
            fadeLayout.Image.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
            fadeLayout.Image.raycastTarget = false;

            fadeLayout.CanvasGrouup = fader.AddComponent<CanvasGroup>();
            fadeLayout.CanvasGrouup.interactable = false;
            fadeLayout.CanvasGrouup.blocksRaycasts = false;
            fadeLayout.CanvasGrouup.ignoreParentGroups = true;
            fadeLayout.CanvasGrouup.alpha = 0;

            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            if (canvas.sortingOrder <= 1)
                canvas.sortingOrder = 16;
            UnityEngine.Object.DontDestroyOnLoad(fader);
        }

        public static void Fade(TPFadeInfo info)
        {
            TPCoroutine.RunCoroutine(IEFade(info, fadeLayout));
        }

        private static IEnumerator IEFade(TPFadeInfo fadeInfo, TPFadeState state)
        {
            isFading = true;
            float percentage = 0.0f;
            state.Time = fadeInfo.FadeAnim.Curve.Evaluate(percentage);
            while (state.Time < 1.0f)
            {
                fadeInfo.ITPFade.Fade(fadeInfo, state);

                percentage += Time.deltaTime * fadeInfo.FadeAnim.Speed;
                state.Time = fadeInfo.FadeAnim.Curve.Evaluate(percentage);
                yield return null;
            }
            isFading = false;
        }

        private static bool TryLoadScene(bool readAnyKey, AsyncOperation asyncLoad)
        {
            return TryLoadScene(readAnyKey, true, KeyCode.None, asyncLoad);
        }

        private static bool TryLoadScene(bool readAnyKey, KeyCode keyToRead, AsyncOperation asyncLoad)
        {
            return TryLoadScene(readAnyKey, false, keyToRead, asyncLoad);
        }

        private static bool TryLoadScene(bool readAnyKey)
        {
            return TryLoadScene(readAnyKey, true, KeyCode.None);
        }

        private static bool TryLoadScene(bool readAnyKey, KeyCode keyToRead)
        {
            return TryLoadScene(readAnyKey, false, keyToRead);
        }
        
        private static bool TryLoadScene(bool readKey, bool readAnyKey, KeyCode keyToRead)
        {
            return !readKey
                || readAnyKey && Input.anyKeyDown
                || !readAnyKey && Input.GetKeyDown(keyToRead);
        }

        private static bool TryLoadScene(bool readKey, bool readAnyKey, KeyCode keyToRead, AsyncOperation asyncLoad)
        {
            if (TryLoadScene(readKey, readAnyKey, keyToRead))
            {
                asyncLoad.allowSceneActivation = true;
                return true;
            }
            return false;
        }


        //public static void Fade() => Fade("", TPFade.FaderType.Alpha);
        //public static void Fade(string sceneName, TPFade.FaderType FadeType) =>
        //    case TPFade.FaderType.Alpha:  StartCoroutine(Fade(true, Alpha, AlphaFader.FadeSpeed));
        //    case TPFade.FaderType.Progress: StartCoroutine(FadeProgress());

        //void SpawnProgress(Transform layTrans) {
        //    if (ProgressFader.LoadingBar != null)
        //        bar = Instantiate(ProgressFader.LoadingBar, layTrans);
        //    if (ProgressFader.LoadingImage != null)
        //        image = Instantiate(ProgressFader.LoadingImage, layTrans);
        //    if (ProgressFader.LoadingProgressText != null)
        //        progress = Instantiate(ProgressFader.LoadingProgressText, layTrans);
        //    if (ProgressFader.LoadingText != null)
        //        text = Instantiate(ProgressFader.LoadingText, layTrans);
        //    if (!ProgressAlpha)
        //        ProgressAlpha = layout.GetComponent<CanvasGroup>();
        //}

        //void SetProgress(AsyncOperation asyncLoad) {
        //    if (progress != null)
        //        progress.text = (asyncLoad.progress * 100).ToString("0") + "%";
        //    if (bar != null)
        //        bar.value = asyncLoad.progress;
        //    if (image != null)
        //        image.fillAmount = asyncLoad.progress * 100;
        //}

        //void SetFullProgress() {
        //    if (progress != null)
        //        progress.text = "100%";
        //    if (bar != null)
        //        bar.value = 1f;
        //    if (image != null)
        //        image.fillAmount = 100;
        //    if (text != null)
        //        text.text = ProgressFader.LoadingTextString;
        //}

        //IEnumerator FadeProgress() {
        //    if (OnFade != null)
        //        OnFade();
        //    ChangeCreator(true);

        //    if (!layout)
        //    {
        //        layout = Instantiate(ProgressFader.ProgressPrefab, transform);
        //        layTrans = layout.transform;
        //        for (int i = 0; i < layTrans.childCount; i++)
        //        {
        //            Destroy(layTrans.GetChild(i).gameObject);
        //        }
        //    }
        //    else
        //        layout.SetActive(true);

        //    if (!bar || !image || !progress || !text || !ProgressAlpha)
        //        SpawnProgress(layTrans);

        //    StartCoroutine(Fade(false, ProgressAlpha, ProgressFader.ProgressFadeSpeed));
        //    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(FadeScene);
        //    asyncLoad.allowSceneActivation = false;

        //    while (!asyncLoad.isDone)
        //    {
        //        SetProgress(asyncLoad);

        //        if (asyncLoad.progress >= 0.9f)
        //        {
        //            SetFullProgress();
        //            ReadKey(asyncLoad);
        //        }
        //        yield return null;
        //    }

        //    StartCoroutine(FadeOut(false, ProgressAlpha, ProgressFader.ProgressFadeSpeed));
        //    yield return new WaitWhile(() => ProgressAlpha.alpha > 0);
        //    layout.SetActive(false);
        //    ChangeCreator(false);
        //}

        //void ChangeCreator(bool started) {
        //    canvas.enabled = started ? true : false;
        //    FadeImage.enabled = started ? false : true;
        //    Alpha.alpha = started ? 1 : 0;
        //}
    }

}
