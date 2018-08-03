using System.Collections;
using UnityEngine;

namespace TPFramework
{
    public sealed class TPCoroutine : MonoBehaviour
    {
        private static TPCoroutine instance;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnScene()
        {
            if (instance == null)
            {
                instance = new GameObject("TPCoroutine").AddComponent<TPCoroutine>();
            }
        }

        public static void RunCoroutine(IEnumerator routine)
        {
            // TODO: Implementation
            instance.StartCoroutine(routine); // temporary use Unity's solution
        }
    }
}
