///**
//*   Authored by Tomasz Piowczyk
//*   MIT LICENSE: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
//*   Repository: https://github.com/Prastiwar/TPFramework 
//*/
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//namespace TPFramework
//{
//    public class TPAchievement : ScriptableObject
//    {
//        public string Title;
//        [Multiline]
//        public string Description;
//        public bool IsCompleted;
//        public Sprite Icon;
//        public float Points;
//        public float MaxPoints;
//        [Space]
//        public TPNotification Notification;
//        public float NotifyLong;

//        public class TPNotification : MonoBehaviour
//        {
//            public Image iconImage;
//            public TextMeshProUGUI pointsText;
//            public TextMeshProUGUI maxPointsText;
//            public TextMeshProUGUI titleText;
//            public TextMeshProUGUI descriptionText;
//            public TPAchievement OnAchievement { get; private set; }

//            public void SetNotification(TPAchievement achievement)
//            {
//                OnAchievement = achievement;
//                iconImage.sprite = achievement.Icon;
//                titleText.text = achievement.Title;
//                descriptionText.text = achievement.Description;
//                pointsText.text = achievement.Points.ToString();
//                maxPointsText.text = achievement.MaxPoints.ToString();
//            }
//        }

//        public class TPAchievementCreator : MonoBehaviour
//        {
//            public static bool DebugMode;

//            public List<TPAchievement> Achievements = new List<TPAchievement>();
//            Dictionary<string, GameObject> _NotifyGO = new Dictionary<string, GameObject>();
//            Queue<TPAchievement> AchievementQueue = new Queue<TPAchievement>();

//            WaitForSeconds Waiter;
//            WaitUntil Until;
//            float WaitSeconds;
//            [SerializeField] bool IsNotify;

//            public delegate void NotifyActivationEventHandler(GameObject notification, bool toActive);
//            public delegate void NotifySettingEventHandler(TPNotification notification, TPAchievement achievement);
//            NotifySettingEventHandler _onNotifySet;
//            NotifyActivationEventHandler _onNotifyActive;

//            NotifyActivationEventHandler OnNotifyActive {
//                get {
//                    if (_onNotifyActive == null)
//                        _onNotifyActive = SetActive;
//                    return _onNotifyActive;
//                }
//                set { _onNotifyActive = value; }
//            }
//            NotifySettingEventHandler OnNotifySet {
//                get {
//                    if (_onNotifySet == null)
//                        _onNotifySet = SetNotification;
//                    return _onNotifySet;
//                }

//                set { _onNotifySet = value; }
//            }


//            public TPAchievement GetAchievement(string name)
//            {
//                int length = Achievements.Count;
//                for (int i = 0; i < length; i++)
//                {
//                    if (name == Achievements[i].name)
//                        return Achievements[i];
//                }
//                return null;
//            }

//            GameObject GetNotificationObject(TPAchievement achievement)
//            {
//                int length = _NotifyGO.Count;
//                string _name = achievement.Notification.gameObject.name;

//                if (_NotifyGO.ContainsKey(_name))
//                {
//                    return _NotifyGO[_name];
//                }

//                return null;
//            }

//            void SetActive(GameObject notification, bool toActive)
//            {
//                notification.SetActive(toActive);
//            }

//            void ShowNotification()
//            {
//                IsNotify = true;
//                TPAchievement achievement = AchievementQueue.Dequeue();
//                GameObject GO = achievement.Notification.gameObject;

//                if (!_NotifyGO.ContainsKey(GO.name))
//                {
//                    string _GOName = GO.name;
//                    GO = Instantiate(GO);
//                    GO.SetActive(false);
//                    _NotifyGO.Add(_GOName, GO);
//                }
//                else
//                {
//                    GO = GetNotificationObject(achievement);
//                }
//                TPNotification notification = GO.GetComponent<TPNotification>();
//                OnNotifySet(notification, achievement);

//                StartCoroutine(IEShowNotification(GO, achievement));
//            }

//            void SetNotification(TPNotification notification, TPAchievement achievement)
//            {
//                notification.SetNotification(achievement);
//            }

//            IEnumerator IEShowNotification(GameObject go, TPAchievement achievement)
//            {
//                OnNotifyActive(go, true);

//                if (WaitSeconds != achievement.NotifyLong)
//                {
//                    WaitSeconds = achievement.NotifyLong;
//                    Waiter = new WaitForSeconds(achievement.NotifyLong);
//                }

//                yield return Waiter;

//                OnNotifyActive(go, false);

//                yield return Waiter;

//                if (Until == null)
//                {
//                    Until = new WaitUntil(() => !go.activeSelf);
//                }

//                yield return Until;

//                if (AchievementQueue.Count > 0)
//                {
//                    ShowNotification();
//                }
//                else
//                {
//                    IsNotify = false;
//                }
//            }

//            public void ShowNotification(TPAchievement achievement)
//            {
//                if (!AchievementQueue.Contains(achievement))
//                {
//                    AchievementQueue.Enqueue(achievement);
//                    if (!IsNotify)
//                        ShowNotification();
//                }
//            }

//            /// <param name="achievement"></param>
//            /// <param name="value">How many points add to achievement?</param>
//            /// <param name="showProgressNotify">Should show notification with progress?</param>
//            /// <param name="showNotifyAfterCompleted">Should show notification if achievement will'be completed after adding points?</param>
//            public void AddPointTo(TPAchievement achievement, float value, bool showProgressNotify, bool showNotifyAfterCompleted)
//            {
//                if (achievement.IsCompleted)
//                    return;

//                achievement.Points += value;
//                if (achievement.Points >= achievement.MaxPoints)
//                {
//                    CompleteAchievement(achievement, showNotifyAfterCompleted);
//                    return;
//                }

//                if (showProgressNotify)
//                    ShowNotification(achievement);
//            }

//            public void CompleteAchievement(TPAchievement achievement, bool showNotification)
//            {
//                achievement.Points = achievement.MaxPoints;
//                achievement.IsCompleted = true;
//                if (showNotification)
//                    ShowNotification(achievement);
//            }

//            public void SetOnNotifySet(NotifySettingEventHandler _NotifySettingEventHandler)
//            {
//                OnNotifySet = _NotifySettingEventHandler;
//            }

//            public void SetOnNotifyActive(NotifyActivationEventHandler _NotifyActivationEventHandler)
//            {
//                OnNotifyActive = _NotifyActivationEventHandler;
//            }

//#if UNITY_EDITOR
//            public void Refresh()
//            {
//                //Achievements = Utilities.TPFind.FindAssetsByType<TPAchievement>();
//            }
//#endif
//        }
//}
//}
