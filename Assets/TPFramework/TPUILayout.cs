using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
#if HAS_TMPRO
using TMPro;
#endif

namespace TPFramework
{
    [Serializable]
    public class TPUILayout
    {
        public GameObject LayoutPrefab;
        public GameObject TPLayout { get; set; }

        protected Transform LayoutTransform { get; private set; }
        protected Image[] Images { get; private set; }
        protected Button[] Buttons { get; private set; }
#if HAS_TMPRO
        protected TextMeshProUGUI Texts { get; private set; }
#else
        protected Text[] Texts { get; private set; }
#endif
        protected bool IsInitialized { get { return TPLayout; } }

        protected void InitializeIfIsNot(Transform parent = null)
        {
            if (IsInitialized)
                return;

            if (!LayoutSpawn(parent))
            {
                TPLayout = UnityEngine.Object.Instantiate(LayoutPrefab, parent);
            }
            Initialize();
            OnInitialized();
        }

        /// <summary> Is called after Initialize </summary>
        protected virtual void OnInitialized() { }

        /// <summary> Returns if TPLayout is already spawned </summary>
        protected virtual bool LayoutSpawn(Transform parent = null) { return false; }

        private void Initialize()
        {
            LayoutTransform = TPLayout.transform.GetChild(0);
#if TPUISafeChecks
            SafeCheck(LayoutTransform);
#endif
            Images = Initialize(LayoutTransform.GetChild(0), Images);
            Buttons = Initialize(LayoutTransform.GetChild(1), Buttons);
            Texts = Initialize(LayoutTransform.GetChild(2), Texts);
        }

#if TPUISafeChecks
        private void SafeCheck(Transform transform)
        {
            if (transform.childCount < 3)
                throw new Exception("Invalid TPUILayout! LayoutTransform needs to have Child 0: Parent of Images, Child 1: Parent of Buttons, Child 2: Parent of Texts");
            else if (transform.GetChild(0).GetChilds().Any(x => x.GetComponent<Image>() == null))
                throw new Exception("Invalid TPUILayout! Child 0: Parent of Images must contain only Images as childs");
            else if (transform.GetChild(1).GetChilds().Any(x => x.GetComponent<Button>() == null))
                throw new Exception("Invalid TPUILayout! Child 1: Parent of Buttons must contain only Buttons as childs");
#if HAS_TMPRO
            else if(transform.GetChild(0).GetChildrens().Any(x => x.GetComponent<TextMeshProUGUI>() == null))
#else
            else if (transform.GetChild(2).GetChilds().Any(x => x.GetComponent<Text>() == null))
#endif
                throw new Exception("Invalid TPUILayout! Child 2: Parent of Texts must contain only Texts as childs");
        }
#endif

        private T[] Initialize<T>(Transform child, T[] array)
        {
            int length = child.childCount;
            array = new T[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = child.GetChild(i).GetComponent<T>();
            }
            return array;
        }
    }

    [Serializable]
    public class TPModalWindow { }
}
