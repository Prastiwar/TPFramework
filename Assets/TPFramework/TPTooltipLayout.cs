//using System;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Events;
//using UnityEngine.UI;

//namespace TPFramework
//{
//    [Serializable]
//    public struct TPTooltipLayout
//    {
//        [SerializeField] private List<TextMeshProUGUI> Texts;
//        [SerializeField] private List<Image> Images;
//        [SerializeField] private List<Button> Buttons;
//        public Transform PanelTransform { get; private set; }
        
//        public void Refresh()
//        {
//            if (PanelTransform == null)
//                PanelTransform = transform.GetChild(0);
//        }

//        public void SetText(string text, int index) { Set(index, text, Texts); }
//        public void SetText(string text, params int[] indexes) { Set(indexes, text, Texts); }
//        public void SetText(string text, TextMeshProUGUI Text) { Set(Text, text, Texts); }

//        public void SetImage(Sprite sprite, int index) { Set(index, sprite, Images); }
//        public void SetImage(Sprite sprite, params int[] indexes) { Set(indexes, sprite, Images); }
//        public void SetImage(Sprite sprite, Image image) { Set(image, sprite, Images); }

//        public void SetButtonClick(UnityAction action, int index) { Set(index, action, Buttons); }
//        public void SetButtonClick(UnityAction action, params int[] indexes) { Set(indexes, action, Buttons); }
//        public void SetButtonClick(UnityAction action, Button button) { Set(button, action, Buttons); }

//        // **** List's obj Getters ****//
//        public TextMeshProUGUI[] GetTexts(params int[] indexes)
//        {
//            int length = indexes.Length;
//            TextMeshProUGUI[] _texts = new TextMeshProUGUI[length];

//            for (int i = 0; i < length; i++)
//                _texts[i] = Texts[indexes[i]];

//            return _texts;
//        }

//        public Image[] GetImages(params int[] indexes)
//        {
//            int length = indexes.Length;
//            Image[] _images = new Image[length];

//            for (int i = 0; i < length; i++)
//                _images[i] = Images[indexes[i]];

//            return _images;
//        }

//        public Button[] GetButtons(params int[] indexes)
//        {
//            int length = indexes.Length;
//            Button[] _buttons = new Button[length];

//            for (int i = 0; i < length; i++)
//                _buttons[i] = Buttons[indexes[i]];

//            return _buttons;
//        }

//        public TextMeshProUGUI GetText(int index)
//        {
//            return Texts[index];
//        }

//        public Image GetImage(int index)
//        {
//            return Images[index];
//        }

//        public Button GetButton(int index)
//        {
//            return Buttons[index];
//        }

//        // **** List's obj Setters ****//
//        void Set<T, U>(int index, T obj, List<U> list)
//        {
//            Type Text = typeof(List<TextMeshProUGUI>);
//            Type Image = typeof(List<Image>);
//            Type Button = typeof(List<Button>);
//            Type List = list.GetType();

//            if (List == Text)
//            {
//                (list[index] as TextMeshProUGUI).text = obj as string;
//            }
//            else if (List == Image)
//            {
//                (list[index] as Image).sprite = obj as Sprite;
//            }
//            else if (List == Button)
//            {
//                (list[index] as Button).onClick.AddListener(obj as UnityAction);
//            }
//        }

//        void Set<T, D, U>(D type, T obj, List<U> list)
//        {
//            int length = Texts.Count;
//            for (int i = 0; i < length; i++)
//            {
//                if (list[i].Equals(type))
//                {
//                    Set(i, obj, list);
//                    break;
//                }
//            }
//        }
        
//        void Set<T, U>(int[] indexes, T obj, List<U> list)
//        {
//            int indexesLength = indexes.Length;
//            int length = Texts.Count;

//            for (int i = 0; i < length && i < indexesLength; i++)
//            {
//                Set(indexes[i], obj, list);
//            }
//        }
//    }
//}
