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
using UnityEngine.UI;
#if HAS_TMPRO
using TMPro;
#endif

namespace TPFramework
{
    [Serializable]
    public struct TPAchievementNotifyInfo
    {
        public string Title;
        [Multiline] public string Description;
        public bool IsCompleted;
        public float Points;
        public float ReachPoints;
    }

    /* ---------------------------------------------------------------------- Achievement ---------------------------------------------------------------------- */

    [CreateAssetMenu(menuName = "TP/TPAchievement/Achievement", fileName = "Achievement")]
    public class TPAchievement : ScriptableObject
    {
        public Action<TPAchievement> OnComplete = delegate { };
        public Sprite Icon;
        public TPAchievementNotifyInfo Info;
        [Space]
        public bool ShowNotifyOnComplete;
        public bool ShowNotifyOnProgress;
        public TPAchievementNotify TPNotify;

        public void AddPoints(float points)
        {
            if (Info.IsCompleted)
                return;
            Info.Points += points;

            if (Info.Points >= Info.ReachPoints)
            {
                Info.Points = Info.ReachPoints;
                Complete(ShowNotifyOnComplete);
            }
            else if (ShowNotifyOnProgress)
            {
                TPNotify.Show(this);
            }
        }

        public void Complete(bool showNotification = false)
        {
            Info.IsCompleted = true;
            Info.Points = Info.ReachPoints;
            if (showNotification)
                TPNotify.Show(this, true);
            OnComplete(this);
        }
    }

    /* ---------------------------------------------------------------------- Notification ---------------------------------------------------------------------- */

    [Serializable]
    public sealed class TPAchievementNotify : TPUILayout
    {
        public float AnimateSpeed;
        public AnimationCurve NotifyAnimatedWait;

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
        private Sprite achievementIcon;

        protected override void OnInitialized()
        {
            iconImage = Images[0];
            pointsText = Texts[0];
            reachPointsText = Texts[1];
            titleText = Texts[2];
            descriptionText = Texts[3];
        }

        public void Show(TPAchievement achievement, bool showDescription = false)
        {
            TPAchievementNotifyInfo fillInfo = achievement.Info;
            if (!showDescription)
                fillInfo.Description = string.Empty;
            achievementIcon = achievement.Icon;
            InitializeIfIsNot();
            TPAchievementManager.ShowNotification(this, fillInfo);
        }

        protected override bool LayoutSpawn(Transform parent = null)
        {
            TPLayout = TPAchievementManager.ShareLayout(LayoutPrefab, parent);
            return true;
        }

        public void FillNotify(TPAchievementNotifyInfo fillInfo)
        {
            iconImage.sprite = achievementIcon;
            titleText.text = fillInfo.Title;
            descriptionText.text = fillInfo.Description;
            pointsText.text = fillInfo.Points.ToString();
            reachPointsText.text = fillInfo.ReachPoints.ToString();
        }
    }

    /* ---------------------------------------------------------------------- Manager ---------------------------------------------------------------------- */

    public static class TPAchievementManager
    {
        public static Action<GameObject, float, bool> OnNotifyActive = delegate { };

        private static bool isBusy;
        private static SharedObjectCollection sharedLayouts = new SharedObjectCollection(2);
        private static Queue<KeyValuePair<TPAchievementNotify, TPAchievementNotifyInfo>> notificationQueue = new Queue<KeyValuePair<TPAchievementNotify, TPAchievementNotifyInfo>>(4);

#if NET_2_0 || NET_2_0_SUBSET
        private static WaitForSeconds waitNotifyLong;
        private static float secondsToWait;
        private static WaitForSeconds waitShow;
        private static float showSeconds;
#endif

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        internal static void ShowNotification(TPAchievementNotify notification, TPAchievementNotifyInfo notifyInfo)
        {
            if (!isBusy)
            {
                isBusy = true;
                notification.FillNotify(notifyInfo);
#if NET_2_0 || NET_2_0_SUBSET
                TPCoroutine.Instance.StartCoroutine(IEShowNotification(notification.TPLayout, notification));
#else
                IEShowNotification(notifyObj, notification);
#endif
            }
            else
            {
                notificationQueue.Enqueue(new KeyValuePair<TPAchievementNotify, TPAchievementNotifyInfo>(notification, notifyInfo));
            }
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static GameObject ShareLayout(GameObject layout, Transform parent = null)
        {
            return sharedLayouts.ShareObject(layout, parent);
        }

#if NET_2_0 || NET_2_0_SUBSET

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        private static IEnumerator IEShowNotification(GameObject sharedObj, TPAchievementNotify notification)
        {
            sharedObj.SetActive(true);
            float percentage = 0.0f;
            float evaluate = notification.NotifyAnimatedWait.Evaluate(percentage);
            while (evaluate < 1.0f)
            {
                OnNotifyActive(sharedObj, evaluate, evaluate <= 0.5f);
                percentage += Time.deltaTime * notification.AnimateSpeed;
                evaluate = notification.NotifyAnimatedWait.Evaluate(percentage);
                yield return null;
            }
            sharedObj.SetActive(false);

            isBusy = false;
            if (notificationQueue.Count > 0)
            {
                var pair = notificationQueue.Dequeue();
                ShowNotification(pair.Key, pair.Value);
            }
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

            isBusy = false;
            if (notificationQueue.Count > 0)
            {
                var pair = notificationQueue.Dequeue();
                ShowNotification(pair.Key, pair.Value);
            }
        }
#endif
    }
}
