using System;
using UnityEngine;
using UnityEngine.UI;
#if HAS_TMPRO
using TMPro;
#endif

namespace TPFramework
{
    [System.Serializable]
    public class TPUILayout
    {
        [SerializeField] private GameObject layoutPrefab;

        public GameObject TPLayout { get; private set; }

        protected Image[] Images { get; private set; }
        protected Button[] Buttons { get; private set; }
#if HAS_TMPRO
        protected TextMeshProUGUI Texts { get; private set; }
#else
        protected Text[] Texts { get; private set; }
#endif
        protected bool IsInitialized { get { return TPLayout; } }

        protected void Initialize(Transform parent = null)
        {
            if (IsInitialized)
                return;
            Spawn(parent);
            OnInitialize();
        }

        protected virtual void OnInitialize() { }

        private void Spawn(Transform parent = null)
        {
            TPLayout = UnityEngine.Object.Instantiate(layoutPrefab, parent);
            Transform transform = layoutPrefab.transform;
#if TPUISafeChecks
            SafeCheck(transform);
#endif
            Images = Initialize(transform.GetChild(0), Images);
            Buttons = Initialize(transform.GetChild(1), Buttons);
            Texts = Initialize(transform.GetChild(2), Texts);
        }

#if TPUISafeChecks
        private void SafeCheck(Transform transform)
        {
            throw new NotImplementedException();
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

    [System.Serializable]
    public class TPModalWindow { }
}
