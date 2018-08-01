using UnityEngine;

namespace TPFramework
{
    public sealed class TPCoroutine : MonoBehaviour
    {
        public static TPCoroutine Instance { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnScene()
        {
            if (Instance == null)
            {
                Instance = new GameObject("TPCoroutine").AddComponent<TPCoroutine>();
            }
        }
    }
}
