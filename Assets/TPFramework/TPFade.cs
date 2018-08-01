///**
//*   Authored by Tomasz Piowczyk
//*   MIT LICENSE: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
//*   Repository: https://github.com/Prastiwar/TPFramework 
//*/
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;

//namespace TPFramework
//{
//    public class TPFade : MonoBehaviour, IPointerClickHandler
//    {
//        public enum FaderType
//        {
//            Alpha,
//            Progress
//        }

//        public FaderType FadeType;
//        public string FadeToScene;

//        public delegate void OnFade();
//        OnFade Fade;

//        TPFaderCreator creator;

//        void Awake()
//        {
//            creator = FindObjectOfType<TPFaderCreator>();
//            if (Fade == null)
//                Fade = () => creator.Fade(FadeToScene, FadeType);
//        }

//        public void SetOnFade(OnFade _onFade)
//        {
//            Fade = _onFade;
//        }

//        public void OnPointerClick(PointerEventData eventData)
//        {
//            Fade();
//        }
//    }

//    [RequireComponent(typeof(Image))]
//    [RequireComponent(typeof(Canvas))]
//    [RequireComponent(typeof(CanvasGroup))]
//    public class TPFaderCreator : MonoBehaviour
//    {
//        private static TPFaderCreator instance;
//        public static bool DebugMode;
//        [System.Serializable]
//        public struct TP_ProgressFade
//        {
//            public GameObject ProgressPrefab;
//            public Slider LoadingBar;
//            public Image LoadingImage;
//            public Text LoadingText;
//            public Text LoadingProgressText;
//            public string LoadingTextString;
//            public float ProgressFadeSpeed;
//            public bool MustKeyToStart;
//            public bool LoadingAnyKeyToStart;
//            public KeyCode LoadingKeyToStart;
//        }
//        [System.Serializable]
//        public struct TP_AlphaFade
//        {
//            public float FadeSpeed;
//            public Sprite FadeTexture;
//            public Color FadeColor;
//        }

//        public TP_ProgressFade ProgressFader;
//        public TP_AlphaFade AlphaFader;
//        public bool IsFading = false;
//        public List<GameObject> Faders;

//        public delegate void OnFading();
//        OnFading BeforeSceneIsLoaded;
//        OnFading OnFade;

//        [HideInInspector] [SerializeField] Image FadeImage;
//        CanvasGroup Alpha;
//        Canvas canvas;
//        string FadeScene;

//        GameObject layout = null;
//        Transform layTrans = null;
//        CanvasGroup ProgressAlpha = null;
//        Slider bar = null;
//        Text progress = null;
//        Text text = null;
//        Image image = null;

//        WaitForSeconds update = new WaitForSeconds(0.049f);
//        WaitForEndOfFrame waitForEnd = new WaitForEndOfFrame();

//        void Awake()
//        {
//            if (instance == null)
//            {
//                instance = this;
//            }
//            else
//            {
//                Destroy(this.gameObject);
//                return;
//            }
//            DontDestroyOnLoad(gameObject);
//            Refresh();
//        }

//        public void Refresh()
//        {
//            if (Alpha == null)
//                Alpha = GetComponent<CanvasGroup>();
//            if (FadeImage == null)
//                FadeImage = GetComponent<Image>();
//            if (canvas == null)
//                canvas = GetComponent<Canvas>();

//            FadeImage.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
//            FadeImage.sprite = AlphaFader.FadeTexture;
//            FadeImage.color = AlphaFader.FadeColor;
//            FadeImage.raycastTarget = false;

//            Alpha.interactable = false;
//            Alpha.blocksRaycasts = false;
//            Alpha.ignoreParentGroups = true;

//            canvas.enabled = false;
//            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
//            if (canvas.sortingOrder <= 1)
//                canvas.sortingOrder = 16;
//        }

//        public void Fade()
//        {
//            Fade("", TPFader.FaderType.Alpha);
//        }
//        public void Fade(string sceneName, TPFader.FaderType FadeType)
//        {
//            if (IsFading)
//                return;

//            IsFading = true;
//            FadeScene = sceneName;

//            switch (FadeType)
//            {
//                case TPFader.FaderType.Alpha:
//                    StartCoroutine(Fade(true, Alpha, AlphaFader.FadeSpeed));
//                    break;

//                case TPFader.FaderType.Progress:
//                    if (FadeScene == "")
//                    {
//                        Debug.Log("Progress loading is available to scene only!");
//                        break;
//                    }
//                    StartCoroutine(FadeProgress());
//                    break;

//                default:
//                    break;
//            }
//        }

//        IEnumerator Fade(bool IsAlpha, CanvasGroup _Alpha, float _FadeSpeed)
//        {
//            canvas.enabled = true;
//            if (OnFade != null)
//                OnFade();
//            _Alpha.alpha = 0;
//            while (_Alpha.alpha < 1)
//            {
//                yield return update;
//                _Alpha.alpha += _FadeSpeed / 100;
//            }
//            if (IsAlpha)
//                StartCoroutine(FadeOut(IsAlpha, _Alpha, _FadeSpeed));
//        }
//        IEnumerator FadeOut(bool IsAlpha, CanvasGroup _Alpha, float _FadeSpeed)
//        {
//            if (BeforeSceneIsLoaded != null)
//                BeforeSceneIsLoaded();
//            if (IsAlpha && FadeScene != "")
//                SceneManager.LoadScene(FadeScene);

//            yield return waitForEnd;

//            _Alpha.alpha = 1;
//            while (_Alpha.alpha > 0)
//            {
//                yield return update;
//                _Alpha.alpha -= _FadeSpeed / 100;
//                if (_Alpha.alpha < 0.1f)
//                    IsFading = false;
//            }
//            canvas.enabled = false;
//        }

//        void SpawnProgress(Transform layTrans)
//        {
//            if (ProgressFader.LoadingBar != null)
//                bar = Instantiate(ProgressFader.LoadingBar, layTrans);
//            if (ProgressFader.LoadingImage != null)
//                image = Instantiate(ProgressFader.LoadingImage, layTrans);
//            if (ProgressFader.LoadingProgressText != null)
//                progress = Instantiate(ProgressFader.LoadingProgressText, layTrans);
//            if (ProgressFader.LoadingText != null)
//                text = Instantiate(ProgressFader.LoadingText, layTrans);
//            if (!ProgressAlpha)
//                ProgressAlpha = layout.GetComponent<CanvasGroup>();
//        }

//        void SetProgress(AsyncOperation asyncLoad)
//        {
//            if (progress != null)
//                progress.text = (asyncLoad.progress * 100).ToString("0") + "%";
//            if (bar != null)
//                bar.value = asyncLoad.progress;
//            if (image != null)
//                image.fillAmount = asyncLoad.progress * 100;
//        }

//        void SetFullProgress()
//        {
//            if (progress != null)
//                progress.text = "100%";
//            if (bar != null)
//                bar.value = 1f;
//            if (image != null)
//                image.fillAmount = 100;
//            if (text != null)
//                text.text = ProgressFader.LoadingTextString;
//        }

//        IEnumerator FadeProgress()
//        {
//            if (OnFade != null)
//                OnFade();
//            ChangeCreator(true);

//            if (!layout)
//            {
//                layout = Instantiate(ProgressFader.ProgressPrefab, transform);
//                layTrans = layout.transform;
//                for (int i = 0; i < layTrans.childCount; i++)
//                {
//                    Destroy(layTrans.GetChild(i).gameObject);
//                }
//            }
//            else
//                layout.SetActive(true);

//            if (!bar || !image || !progress || !text || !ProgressAlpha)
//                SpawnProgress(layTrans);

//            StartCoroutine(Fade(false, ProgressAlpha, ProgressFader.ProgressFadeSpeed));
//            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(FadeScene);
//            asyncLoad.allowSceneActivation = false;

//            while (!asyncLoad.isDone)
//            {
//                SetProgress(asyncLoad);

//                if (asyncLoad.progress >= 0.9f)
//                {
//                    SetFullProgress();
//                    ReadKey(asyncLoad);
//                }
//                yield return null;
//            }

//            StartCoroutine(FadeOut(false, ProgressAlpha, ProgressFader.ProgressFadeSpeed));
//            yield return new WaitWhile(() => ProgressAlpha.alpha > 0);
//            layout.SetActive(false);
//            ChangeCreator(false);
//        }

//        void ChangeCreator(bool started)
//        {
//            canvas.enabled = started ? true : false;
//            FadeImage.enabled = started ? false : true;
//            Alpha.alpha = started ? 1 : 0;
//        }

//        void ReadKey(AsyncOperation asyncLoad)
//        {
//            if (ProgressFader.MustKeyToStart)
//            {
//                if (!ProgressFader.LoadingAnyKeyToStart)
//                {
//                    if (Input.GetKeyDown(ProgressFader.LoadingKeyToStart))
//                        asyncLoad.allowSceneActivation = true;
//                }
//                else
//                {
//                    if (Input.anyKeyDown)
//                        asyncLoad.allowSceneActivation = true;
//                }
//            }
//            else
//            {
//                asyncLoad.allowSceneActivation = true;
//            }
//        }

//        public void SetOnFaderStarted(OnFading _OnFade)
//        {
//            OnFade = _OnFade;
//        }

//        public void SetBeforeSceneLoaded(OnFading _BeforeSceneIsLoaded)
//        {
//            BeforeSceneIsLoaded = _BeforeSceneIsLoaded;
//        }
//    }

//}
