/**
*   Authored by Tomasz Piowczyk
*   MIT LICENSE: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework
*/

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using TPFramework.Core;
using TMPro;

namespace TPFramework.Unity
{
    /* ---------------------------------------------------------------------- Achievement ---------------------------------------------------------------------- */

    [CreateAssetMenu(menuName = "TP/TPAchievement/Achievement", fileName = "Achievement")]
    public class TPAchievement : ScriptableObject, ITPAchievement
    {
        public Sprite Icon;

        public TPAchievementData Data { get; private set; }
        public Action OnComplete { get; set; }

        public void AddPoints(float points)
        {
            throw new NotImplementedException();
        }

        public void Complete()
        {
            throw new NotImplementedException();
        }

        //public Action<TPAchievement> OnComplete = delegate { };
        //public TPAchievementNotifyInfo Info;
        //public bool ShowNotifyOnComplete;
        //public bool ShowNotifyOnProgress;
        //public TPAchievementNotify TPNotify;

        //public void AddPoints(float points)
        //{
        //    if (Info.IsCompleted)
        //        return;
        //    Info.Points += points;

        //    if (Info.Points >= Info.ReachPoints)
        //    {
        //        Info.Points = Info.ReachPoints;
        //        Complete(ShowNotifyOnComplete);
        //    }
        //    else if (ShowNotifyOnProgress)
        //    {
        //        TPNotify.Show(this);
        //    }
        //}

        //public void Complete(bool showNotification = false)
        //{
        //    Info.IsCompleted = true;
        //    Info.Points = Info.ReachPoints;
        //    if (showNotification)
        //        TPNotify.Show(this, true);
        //    OnComplete(this);
        //}
    }

    /* ---------------------------------------------------------------------- Notification ---------------------------------------------------------------------- */

    [Serializable]
    public sealed class TPAchievementNotify : TPUILayout
    {
        public TPAnimation NotifyAnim;

        private Image iconImage;
        private TextMeshProUGUI pointsText;
        private TextMeshProUGUI reachPointsText;
        private TextMeshProUGUI titleText;
        private TextMeshProUGUI descriptionText;
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
            TPAchievementData fillInfo = achievement.Data;
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

        public void FillNotify(TPAchievementData fillInfo)
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
        public static TPAnim.OnAnimActivationHandler OnNotifyActivation = delegate { };

        private static bool isBusy;
        private static SharedObjectCollection sharedLayouts = new SharedObjectCollection(2);
        private static Queue<KeyValuePair<TPAchievementNotify, TPAchievementData>> notificationQueue = new Queue<KeyValuePair<TPAchievementNotify, TPAchievementData>>(4);

#if NET_2_0 || NET_2_0_SUBSET
        private static WaitForSeconds waitNotifyLong;
        private static float secondsToWait;
        private static WaitForSeconds waitShow;
        private static float showSeconds;
#endif

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        internal static void ShowNotification(TPAchievementNotify notification, TPAchievementData notifyInfo)
        {
            if (!isBusy)
            {
                isBusy = true;
                notification.FillNotify(notifyInfo);
                TPAnim.Animate(notification.NotifyAnim, (time) => OnNotifyActivation(time, notification.LayoutTransform), () => notification.SetActive(true),
                    () => {
                        notification.SetActive(false);
                        isBusy = false;
                        if (notificationQueue.Count > 0)
                        {
                            var pair = notificationQueue.Dequeue();
                            ShowNotification(pair.Key, pair.Value);
                        }
                    });
            }
            else
            {
                notificationQueue.Enqueue(new KeyValuePair<TPAchievementNotify, TPAchievementData>(notification, notifyInfo));
            }
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static GameObject ShareLayout(GameObject layout, Transform parent = null)
        {
            return sharedLayouts.ShareObject(layout, parent);
        }
    }
}
