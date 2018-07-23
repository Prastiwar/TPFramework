/**
*   Authored by Tomasz Piowczyk
*   MIT LICENSE: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;

namespace TPFramework
{
    public enum TPTooltipType
    {
        DynamicEnter, // moves with cursor - show on pointer enter
        DynamicClick, // moves with cursor - show on click
        StaticEnter,  // doesn't move with cursor - show on pointer enter
        StaticClick   // doesn't move with cursor - show on click
    }


    // ---------------------------------------------------------------- Tooltip Component ---------------------------------------------------------------- //

    public class TPTooltip : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public TPTooltipType TooltipType;
        public bool IsObserving = true;
        public TPTooltipLayout TooltipLayout;

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!CanRaycast(eventData))
                return;

            if (TooltipType.IsClickable())
            {
                if (!TooltipLayout.IsInitialized)
                    TooltipLayout.Initialize();
                TooltipManager.OnPointerEnter(eventData);
            }
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!CanRaycast(eventData))
                return;

            if (!TooltipType.IsClickable())
            {
                if (!TooltipLayout.IsInitialized)
                    TooltipLayout.Initialize();
                TooltipManager.OnPointerEnter(eventData);
            }
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void OnPointerExit(PointerEventData eventData)
        {
            if (!CanRaycast(eventData))
                return;

            if (TooltipType != TPTooltipType.StaticClick)
                TooltipManager.OnPointerExit(eventData);
        }

        private bool CanRaycast(PointerEventData eventData)
        {
            return IsObserving && eventData != null;
        }
    }


    // ---------------------------------------------------------------- TooltipManager ---------------------------------------------------------------- //

    public static class TooltipManager
    {
        private static TPTooltip observer;
        private static PointerEventData _eventData;

        public static Action<TPTooltip> OnObserverEnter = delegate { observer.TooltipLayout.SetActive(true); };
        public static Action<TPTooltip> OnObserverExit = delegate { observer.TooltipLayout.SetActive(false); };

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void OnPointerEnter(PointerEventData eventData)
        {
            observer = eventData.pointerEnter.GetComponent<TPTooltip>();
            _eventData = eventData;
            
            OnObserverEnter(observer);

            if (observer.TooltipType.IsDynamic())
                observer.StartCoroutine(ToolTipPositioning());
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void OnPointerExit(PointerEventData eventData)
        {
            if (!observer)
                return;

            OnObserverExit(observer);
            observer = null;
            _eventData = null;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        private static IEnumerator ToolTipPositioning()
        {
            while (_eventData != null)
            {
                Vector2 panelHalfVector = _eventData.position + observer.TooltipLayout.DynamicOffset;
                panelHalfVector.Set(Mathf.Clamp(panelHalfVector.x, observer.TooltipLayout.panelHalfWidth, Screen.width - observer.TooltipLayout.panelHalfWidth),
                                    Mathf.Clamp(panelHalfVector.y, observer.TooltipLayout.panelHalfHeight, Screen.height - observer.TooltipLayout.panelHalfHeight));
                observer.TooltipLayout.SetPosition(panelHalfVector);
                yield return null;
            }
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static bool IsClickable(this TPTooltipType tooltipType)
        {
            return tooltipType == TPTooltipType.DynamicClick || tooltipType == TPTooltipType.StaticClick;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static bool IsDynamic(this TPTooltipType tooltipType)
        {
            return tooltipType == TPTooltipType.DynamicClick || tooltipType == TPTooltipType.DynamicEnter;
        }
    }


    // ---------------------------------------------------------------- UI Layout of Tooltip ---------------------------------------------------------------- //

    [Serializable]
    public struct TPTooltipLayout
    {
        private CanvasGroup panelCanvasGroup;
        private Transform panelTransform;
        private Image[] images;
        private Button[] buttons;
#if HAS_TMPRO
        private List<TextMeshProUGUI> Texts;
#else
        private Text[] texts;
#endif
        internal float panelHalfHeight;
        internal float panelHalfWidth;

        [SerializeField] private GameObject prefab;
        [SerializeField] private Transform staticPosition;
        public Vector2 DynamicOffset;
        public bool IsInitialized { get; set; }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void Initialize()
        {
#if TPTooltipSafeChecks
            SafeCheck();
#endif
            IsInitialized = true;
            prefab = UnityEngine.Object.Instantiate(prefab);
            panelTransform = prefab.transform.GetChild(0);

            panelCanvasGroup = prefab.transform.GetComponent<CanvasGroup>();
            panelCanvasGroup.alpha = 0;
            panelCanvasGroup.blocksRaycasts = false;

            images = panelTransform.GetChild(0).GetComponentsInChildren<Image>();
            buttons = panelTransform.GetChild(1).GetComponentsInChildren<Button>();
            texts = panelTransform.GetChild(2).GetComponentsInChildren<Text>();

            Image panelImage = panelTransform.GetComponent<Image>();
            Rect panelRect = panelImage.rectTransform.rect;
            panelHalfWidth = panelRect.width / 2;
            panelHalfHeight = panelRect.height / 2;
        }

#if TPTooltipSafeChecks
        private void SafeCheck()
        {
            if (prefab.transform.childCount < 1)
                throw new Exception("Invalid Tooltip Layout! Prefab needs to be canvas parent with panel as child");
            else if (!prefab.transform.GetComponent<CanvasGroup>())
                throw new Exception("Invalid Tooltip Layout! Prefab needs to have CanvasGroup component");
            else if (!prefab.transform.GetChild(0).GetComponent<Image>())
                throw new Exception("Invalid Tooltip Layout! PanelTransform(child of canvas) needs to have Image component");
            else if (prefab.transform.GetChild(0).childCount < 3)
                throw new Exception("Invalid Tooltip Layout! PanelTransform(child of canvas) needs to have at least 3 children (images parent, buttons parent, texts parent)");
        }
#endif

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void SetActive(bool active)
        {
            panelCanvasGroup.alpha = active ? 1 : 0;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void SetPosition(Vector2 position)
        {
            panelTransform.position = position;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void SetPositionToStatic()
        {
            panelTransform.position = staticPosition.position;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void SetText(string text, int index) { Set(index, text, texts); }
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void SetText(string text, params int[] indexes) { Set(indexes, text, texts); }
#if HAS_TMPRO
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void SetText(string text, TextMeshProUGUI Text) { Set(Text, text, Texts); }
#else
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void SetText(string text, Text Text) { Set(Text, text, texts); }
#endif

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void SetImage(Sprite sprite, int index) { Set(index, sprite, images); }
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void SetImage(Sprite sprite, params int[] indexes) { Set(indexes, sprite, images); }
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void SetImage(Sprite sprite, Image image) { Set(image, sprite, images); }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void SetButtonClick(UnityAction action, int index) { Set(index, action, buttons); }
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void SetButtonClick(UnityAction action, params int[] indexes) { Set(indexes, action, buttons); }
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void SetButtonClick(UnityAction action, Button button) { Set(button, action, buttons); }


#if HAS_TMPRO
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public TextMeshProUGUI[] GetTexts(params int[] indexes)
        {
            int length = indexes.Length;
            TextMeshProUGUI[] _texts = new TextMeshProUGUI[length];
            for (int i = 0; i < length; i++)
                _texts[i] = Texts[indexes[i]];
            return _texts;
        }
#else
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public Text[] GetTexts(params int[] indexes)
        {
            int length = indexes.Length;
            Text[] _texts = new Text[length];
            for (int i = 0; i < length; i++)
                _texts[i] = texts[indexes[i]];
            return _texts;
        }
#endif

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public Image[] GetImages(params int[] indexes)
        {
            int length = indexes.Length;
            Image[] _images = new Image[length];
            for (int i = 0; i < length; i++)
                _images[i] = images[indexes[i]];
            return _images;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public Button[] GetButtons(params int[] indexes)
        {
            int length = indexes.Length;
            Button[] _buttons = new Button[length];
            for (int i = 0; i < length; i++)
                _buttons[i] = buttons[indexes[i]];
            return _buttons;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
#if HAS_TMPRO
        public TextMeshProUGUI GetText(int index)
#else
        public Text GetText(int index)
#endif
        {
            return texts[index];
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public Image GetImage(int index)
        {
            return images[index];
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public Button GetButton(int index)
        {
            return buttons[index];
        }
        

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        private void Set<T, U>(int index, T obj, U[] collection)
        {
#if HAS_TMPRO
            if (collection is TextMeshProUGUI[])
            {
                (collection[index] as TextMeshProUGUI).text = obj as string;
            }
#else
            if (collection is Text[])
            {
                (collection[index] as Text).text = obj as string;
            }
#endif
            else if (collection is Image[])
            {
                (collection[index] as Image).sprite = obj as Sprite;
            }
            else if (collection is Button[])
            {
                (collection[index] as Button).onClick.AddListener(obj as UnityAction);
            }
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        private void Set<T, D, U>(D type, T obj, U[] collection)
        {
            int length = texts.Length;
            for (int i = 0; i < length; i++)
            {
                if (collection[i].Equals(type))
                {
                    Set(i, obj, collection);
                    break;
                }
            }
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        private void Set<T, U>(int[] indexes, T obj, U[] collection)
        {
            int indexesLength = indexes.Length;
            int length = texts.Length;
            for (int i = 0; i < length && i < indexesLength; i++)
            {
                Set(indexes[i], obj, collection);
            }
        }
    }
}
