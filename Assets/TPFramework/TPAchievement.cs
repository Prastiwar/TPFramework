/**
*   Authored by Tomasz Piowczyk
*   MIT LICENSE: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
#if HAS_TMPRO
using TMPro;
#endif

namespace TPFramework
{
    /* ---------------------------------------------------------------------- Achievement ---------------------------------------------------------------------- */

    [CreateAssetMenu(menuName = "TP/TPAchievement/Achievement", fileName = "Achievement")]
    public class TPAchievement : ScriptableObject
    {
        public string Title;
        [Multiline] public string Description;
        public bool IsCompleted;
        public Sprite Icon;
        public float Points;
        public float ReachPoints;
        [Space]
        public bool ShowNotifyOnComplete;
        public bool ShowNotifyOnProgress;
        public TPAchievementNotify TPNotify;

        public void AddPoints(float points)
        {
            if (IsCompleted)
                return;
            Points += points;

            if (Points >= ReachPoints)
            {
                Points = ReachPoints;
                Complete(ShowNotifyOnComplete);
            }
            else if (ShowNotifyOnProgress)
            {
                TPNotify.Show(this);
            }
        }

        public void Complete(bool showNotification = false)
        {
            IsCompleted = true;
            Points = ReachPoints;
            if (showNotification)
                TPNotify.Show(this);
        }
    }

    /* ---------------------------------------------------------------------- Notification ---------------------------------------------------------------------- */

    [Serializable]
    public class TPAchievementNotify : TPUILayout
    {
        public float NotifyLong;
        private Image iconImage;
#if HAS_TMPRO
        private TextMeshProUGUI pointsText;
        private TextMeshProUGUI reachPointsText;
        private TextMeshProUGUI titleText;
        private TextMeshProUGUI descriptionText;
#else
        private Text pointsText;
        private Text reachPointsText;
        private Text titleText;
        private Text descriptionText;
#endif

        protected override void OnInitialized()
        {
            iconImage = Images[0];
            pointsText = Texts[0];
            reachPointsText = Texts[1];
            titleText = Texts[2];
            descriptionText = Texts[3];
        }

        public void Show(TPAchievement achievement)
        {
            InitializeIfIsNot();
            iconImage.sprite = achievement.Icon;
            titleText.text = achievement.Title;
            descriptionText.text = achievement.Description;
            pointsText.text = achievement.Points.ToString();
            reachPointsText.text = achievement.ReachPoints.ToString();
            TPAchievementManager.ShowNotification(this);
        }
    }

    /* ---------------------------------------------------------------------- Manager ---------------------------------------------------------------------- */

    public static class TPAchievementManager
    {
        public static Action<GameObject, float, bool> OnNotifyActive = delegate { };

        private static bool isBusy;
        private static List<TPAchievement> achievements = new List<TPAchievement>(4);
        private static Dictionary<int, GameObject> sharedNotifications = new Dictionary<int, GameObject>(2);
        private static Queue<TPAchievementNotify> notificationQueue = new Queue<TPAchievementNotify>(4);

#if NET_2_0 || NET_2_0_SUBSET
        private static WaitForSeconds waitNotifyLong;
        private static WaitUntil waitUntilDeactive;
        private static float secondsToWait;
#endif

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void InitAchievement(TPAchievement achievement)
        {
            achievements.Add(achievement);
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void ShowNotification(TPAchievementNotify notification)
        {
            if (!notificationQueue.Contains(notification))
            {
                if (!isBusy)
                {
                    isBusy = true;
                    GameObject notifyObj = ShareNotificationObject(notification.TPLayout);
#if NET_2_0 || NET_2_0_SUBSET
                    TPCoroutine.Instance.StartCoroutine(IEShowNotification(notifyObj, notification));
#else
                    IEShowNotification(notifyObj, notification);
#endif
                }
                else
                {
                    notificationQueue.Enqueue(notification);
                }
            }
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static TPAchievement GetAchievement(string name)
        {
            int length = achievements.Count;
            for (int i = 0; i < length; i++)
            {
                if (name == achievements[i].name)
                    return achievements[i];
            }
            return null;
        }

#if NET_2_0 || NET_2_0_SUBSET    

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline    
        private static WaitUntil WaitUntilDeactive(GameObject go)
        {
            if (waitUntilDeactive == null)
                waitUntilDeactive = new WaitUntil(() => !go.activeSelf);
            return waitUntilDeactive;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        private static IEnumerator IEShowNotification(GameObject sharedObj, TPAchievementNotify notification)
        {
            OnNotifyActive(sharedObj, notification.NotifyLong, true);

            if (secondsToWait != notification.NotifyLong)
            {
                secondsToWait = notification.NotifyLong;
                waitNotifyLong = new WaitForSeconds(notification.NotifyLong);
            }

            yield return waitNotifyLong;

            OnNotifyActive(sharedObj, notification.NotifyLong, false);

            yield return waitNotifyLong;

            if (waitUntilDeactive == null)
                waitUntilDeactive = new WaitUntil(() => !sharedObj.activeSelf);

            yield return waitUntilDeactive;

            if (notificationQueue.Count > 0)
                ShowNotification(notificationQueue.Dequeue());
            else
                isBusy = false;
        }

#else
        
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        private static async void IEShowNotification(GameObject sharedObj, TPNotification notification)
        {
            OnNotifyActive(sharedObj, notification.NotifyLong, true);

            await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(notification.NotifyLong));
            sharedObj.SetActive(true);

            OnNotifyActive(sharedObj, notification.NotifyLong, false);

            await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(notification.NotifyLong));
            sharedObj.SetActive(false);

            if (notificationQueue.Count > 0)
                ShowNotification(notificationQueue.Dequeue());
            else
                isBusy = false;
        }

#endif

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        private static GameObject ShareNotificationObject(GameObject notificationObject)
        {
            int id = notificationObject.GetInstanceID();
            if (!sharedNotifications.ContainsKey(id))
            {
                notificationObject.SetActive(false);
                sharedNotifications[id] = UnityEngine.Object.Instantiate(notificationObject);
            }
            return sharedNotifications[id];
        }
    }
}
