using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace FootBallNet
{
    public class UIButton : Button
    {
        public Action ClickEvent;

        [SerializeField] protected TextMeshProUGUI Text;
        [SerializeField] protected Image ChangeableImage;

        protected override void Awake()
        {
            base.Awake();
            
            onClick.AddListener(() => ClickEvent?.Invoke());
        }

        public void SetText(string text)
        {
            if (Text is null)
                return;
            
            Text.text = text;
        }

        public void SetImage(Sprite sprite)
        {
            if (ChangeableImage is null)
                return;
            
            ChangeableImage.sprite = sprite;
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(UIButton), true)]
    public class UIButtonEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
#endif
}