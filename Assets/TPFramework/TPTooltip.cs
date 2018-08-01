﻿/**
*   Authored by Tomasz Piowczyk
*   MIT LICENSE: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if HAS_TMPRO
using TMPro;
#endif

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
                TooltipLayout.Boot(TooltipType);
                TPTooltipManager.OnPointerClick(eventData);
            }
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!CanRaycast(eventData))
                return;

            if (!TooltipType.IsClickable())
            {
                TooltipLayout.Boot(TooltipType);
                TPTooltipManager.OnPointerEnter(eventData);
            }
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void OnPointerExit(PointerEventData eventData)
        {
            if (!CanRaycast(eventData))
                return;

            if (TooltipType != TPTooltipType.StaticClick)
                TPTooltipManager.OnPointerExit(eventData);
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        private bool CanRaycast(PointerEventData eventData)
        {
            return IsObserving && eventData != null;
        }
    }

    // ---------------------------------------------------------------- TooltipManager ---------------------------------------------------------------- //

    public static class TPTooltipManager
    {
        private static TPTooltip observer;
        private static PointerEventData _eventData;
        private static readonly Dictionary<int, GameObject> sharedLayouts = new Dictionary<int, GameObject>(2);

        public static Action<TPTooltip> OnObserverEnter = delegate { observer.TooltipLayout.SetActive(true); };
        public static Action<TPTooltip> OnObserverExit = delegate { observer.TooltipLayout.SetActive(false); };

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static GameObject ShareLayout(GameObject layout)
        {
            int id = layout.GetInstanceID();
            if (!sharedLayouts.ContainsKey(id))
                return sharedLayouts[id] = UnityEngine.Object.Instantiate(layout);
            return sharedLayouts[id];
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void OnPointerClick(PointerEventData eventData)
        {
            observer = eventData.pointerEnter.GetComponent<TPTooltip>();
            _eventData = eventData;

            if (!observer.TooltipLayout.IsActive())
            {
                OnObserverEnter(observer);
                if (observer.TooltipType.IsDynamic())
                    observer.StartCoroutine(ToolTipPositioning());
            }
            else
            {
                OnObserverExit(observer);
            }
        }

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
    public class TPTooltipLayout : TPUILayout
    {
        private CanvasGroup panelCanvasGroup;

        internal float panelHalfHeight;
        internal float panelHalfWidth;

        public Vector2 DynamicOffset;
        public bool UseSharedLayout;
        [SerializeField] private Transform staticPosition;

        protected override void OnInitialized()
        {
#if TPTooltipSafeChecks
            SafeCheck(TPLayout);
#endif
            panelCanvasGroup = TPLayout.GetComponent<CanvasGroup>();
            panelCanvasGroup.alpha = 0;
            panelCanvasGroup.blocksRaycasts = true;

            Rect panelRect = LayoutTransform.GetComponent<Image>().rectTransform.rect;
            panelHalfWidth = panelRect.width / 2;
            panelHalfHeight = panelRect.height / 2;
        }

        protected override bool LayoutSpawned()
        {
            if (UseSharedLayout)
            {
                TPLayout = TPTooltipManager.ShareLayout(LayoutPrefab);
                return true;
            }
            return false;
        }

        public void Boot(TPTooltipType type)
        {
            InitializeIfIsNot();

            if (!type.IsDynamic())
            {
                panelCanvasGroup.blocksRaycasts = true;
                SetPositionToStatic();
            }
            else
            {
                panelCanvasGroup.blocksRaycasts = false;
            }
        }

#if TPTooltipSafeChecks
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        private void SafeCheck(GameObject layout)
        {
            if (!layout.transform.GetComponent<CanvasGroup>())
                throw new Exception("Invalid Tooltip Layout! Prefab needs to have CanvasGroup component");
            else if (!LayoutTransform.GetComponent<Image>())
                throw new Exception("Invalid Tooltip Layout! LayoutTransform(child of canvas) needs to have Image component");
        }
#endif

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public bool IsActive()
        {
            return panelCanvasGroup.alpha == 1;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void SetActive(bool active)
        {
            panelCanvasGroup.alpha = active ? 1 : 0;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void SetPosition(Vector2 position)
        {
            LayoutTransform.position = position;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void SetPositionToStatic()
        {
            LayoutTransform.position = staticPosition.position;
        }
    }
}
