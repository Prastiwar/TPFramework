///**
//*   Authored by Tomasz Piowczyk
//*   MIT LICENSE: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
//*   Repository: https://github.com/Prastiwar/TPFramework 
//*/
//using System.Collections;
//using System.Collections.Generic;
//using System.Runtime.CompilerServices;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.EventSystems;
//using System;

//namespace TPFramework
//{
//    public enum TPTooltipType
//    {
//        DynamicEnter, // moves with cursor - show on pointer enter
//        DynamicClick, // moves with cursor - show on click
//        StaticEnter,  // doesn't move with cursor - show on pointer enter
//        StaticClick   // doesn't move with cursor - show on click
//    }

//    [UnityEditor.InitializeOnLoad]
//    internal class TPTooltipEditor
//    {

//    }

//    public class TPTooltip : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
//    {
//        public TPTooltipType TooltipType;
//        public bool IsObserving = true;

//        public void OnPointerClick(PointerEventData eventData)
//        {
//            if (!IsObserving)
//                return;

//            if (TooltipType.IsClickable())
//                TooltipManager.OnPointerEnter(eventData);
//        }

//        public void OnPointerEnter(PointerEventData eventData)
//        {
//            if (!IsObserving)
//                return;

//            if (!TooltipType.IsClickable())
//                TooltipManager.OnPointerEnter(eventData);
//        }

//        public void OnPointerExit(PointerEventData eventData)
//        {
//            if (!IsObserving)
//                return;

//            if (TooltipType != TPTooltipType.StaticClick)
//                TooltipManager.OnPointerExit(eventData);
//        }
//    }

   

//    public class TooltipManager : MonoBehaviour
//    {
//        public TPTooltipLayout TooltipLayout;
//        public Transform StaticTransform;
//        public Vector2 Offset;

//        private TPTooltip observer;
//        private PointerEventData _eventData;
//        private WaitWhile waitWhileOnPos;
//        private GameObject tooltipLayoutCanvas;
//        private Vector2 panelHalfVector;
//        private float panelHalfHeight;
//        private float panelHalfWidth;

//        public Action<TPTooltip> OnObserverEnter = delegate { };
//        public Action<TPTooltip> OnObserverExit = delegate { };
//        public Action<bool> ActiveTooltip;

//        void Awake()
//        {
//            Refresh();
//            if (waitWhileOnPos == null && _eventData != null)
//                waitWhileOnPos = new WaitWhile(() => panelHalfVector == _eventData.position);
//        }

//        public void Refresh()
//        {
//            if (TooltipLayout != null)
//            {
//                TooltipLayout.Refresh();
//                tooltipLayoutCanvas = TooltipLayout.gameObject;
//                Image PanelImage = TooltipLayout.PanelTransform.GetComponent<Image>();
//                Rect PanelRect = PanelImage.rectTransform.rect;
//                panelHalfWidth = PanelRect.width / 2;
//                panelHalfHeight = PanelRect.height / 2;
//            }
//        }

//        public void OnPointerEnter(PointerEventData eventData)
//        {
//            if (eventData == null)
//                return;

//            observer = eventData.pointerEnter.GetComponent<TPTooltip>();
//            _eventData = eventData;

//            if (observer == null)
//                return;

//            OnObserverEnter(observer);
//            SetActive(true);

//            if (observer.TooltipType.IsDynamic())
//                StartCoroutine(ToolTipPositioning());
//            else
//                TooltipLayout.PanelTransform.position = StaticTransform.position;
//        }

//        public void OnPointerExit(PointerEventData eventData)
//        {
//            OnObserverExit(observer);
//            observer = null;
//            _eventData = null;
//            SetActive(false);
//        }

//        public void SetActive(bool SetActive)
//        {
//            if (ActiveTooltip != null)
//                ActiveTooltip(SetActive);
//            else
//                tooltipLayoutCanvas.SetActive(SetActive);
//        }

//        private IEnumerator ToolTipPositioning()
//        {
//            while (_eventData != null)
//            {
//                panelHalfVector = _eventData.position + Offset;
//                panelHalfVector.Set(Mathf.Clamp(panelHalfVector.x, panelHalfWidth, Screen.width - panelHalfWidth),
//                                    Mathf.Clamp(panelHalfVector.y, panelHalfHeight, Screen.height - panelHalfHeight));

//                TooltipLayout.PanelTransform.position = panelHalfVector;
//                yield return waitWhileOnPos;
//            }
//        }
//    }
//}
